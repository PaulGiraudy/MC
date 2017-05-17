﻿using MinerControl.History;
using MinerControl.PriceEntries;
using MinerControl.Services;
using MinerControl.Utility;
using MinerControl.Utility.Multicast;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MinerControl
{
    public class MiningEngine
    {
        private const int RemotePortNumber = 12814;

        private Process _process;
        private readonly IList<AlgorithmEntry> _algorithmEntries = new List<AlgorithmEntry>();
        private readonly IList<PriceEntryBase> _priceEntries = new List<PriceEntryBase>();
        private readonly IList<IService> _services = new List<IService>();
        private readonly IList<ServiceHistory> _priceHistories = new List<ServiceHistory>();

        private decimal _powerCost;
        private decimal _exchange;
        private string _currencyCode;
        private string _currencySymbol;


        private bool _showHistory;
        private bool _logactivity;
        private bool _mineByAverage;

        private readonly DateTime _engineCreation;
        private DateTime? _startMining;

        private TimeSpan _minTime;
        private TimeSpan _maxTime;
        private TimeSpan _switchTime;
        private TimeSpan _delay;
        private TimeSpan _autoExitTime;
        private TimeSpan _statWindow;
        public static double statWindow=30;
        public static int _AvSpTicks=10;


        private decimal _iqrMultiplier;
        private double _outlierPercentage;
        private bool _ignoreOutliers;

        private DateTime? _stoppedMining;
        private TimeSpan _deadtime;

        private PriceEntryBase _currentRunning;

        private int? _nextRun; // Next algo to run
        private DateTime? _nextRunFromTime; // When the next run algo became best

        private volatile bool _hasPrices;
        private volatile bool _pricesUpdated;

        private double _dynamicSwitchOffset;
        private double _dynamicSwitchPivot;
        private double _dynamicSwitchPower;
        private TimeSpan _dynamicSwitchTime;
        private bool _dynamicSwitching;

        private decimal _profitBestOverRunning;
        private decimal _minProfit;
        private decimal? _minPrice;
        private string MinerFolderPath;
        public string WTMUrl = "http://www.whattomine.com/coins.json";

        public static bool WTMExtraCoins = true;
        public bool _bufferarray = true;

        public dynamic[,] WTMArray = new dynamic[1, 6];
        private double WTMSyncPeriod =3;
        public static int _MinIdleSeconds = 30;
        public bool _pumpreduction = false;
        public bool _poolEnabled = true;
        public decimal _dynoffset = 10;
        public decimal _dyncheck = 1;
        public bool _showinactive = true;
        public static bool _coloring = true;
        public bool _usewindow = false;
        public static bool _hidecolumngr1 = false;
        public static bool _hidecolumngr2 = false;

        private DateTime WTMTime;

        public MiningEngine()
        {
            _engineCreation = DateTime.Now;
            MinerKillMode = 1;
            GridSortMode = 1;
            MiningMode = MiningModeEnum.Stopped;
        }

        public MiningModeEnum MiningMode { get; set; }

        public int MinerKillMode { get; private set; }
        public int GridSortMode { get; private set; }
        public int TrayMode { get; private set; } // How to handle minimizing ot the tray

        public int? CurrentRunning
        {
            get { return _currentRunning == null ? (int?)null : _currentRunning.Id; }
        }

        public PriceEntryBase CurrentPriceEntry
        {
            get { return _currentRunning; }
        }

        public int? NextRun
        {
            get { return _nextRun; }
        }

        public DateTime? StartMining
        {
            get { return _startMining; }
        }

        public decimal PowerCost
        {
            get { return _powerCost; }
        }

        public decimal Exchange
        {
            get { return _exchange; }
        }

        public string CurrencyCode
        {
            get { return _currencyCode; }
        }

        public string CurrencySymbol
        {
            get { return _currencySymbol; }
        }

        public bool MineByAverage
        {
            get { return _mineByAverage; }
        }

        public TimeSpan StatWindow
        {
            get { return _statWindow; }
        }

        public TimeSpan DeadTime
        {
            get { return _deadtime; }
        }

        public TimeSpan? RestartTime
        {
            get
            {
                return _startMining.HasValue && _maxTime > TimeSpan.Zero
                    ? _maxTime - (DateTime.Now - _startMining.Value)
                    : (TimeSpan?)null;
            }
        }

        public TimeSpan? MiningTime
        {
            get
            {
                return _startMining.HasValue
                    ? DateTime.Now - _startMining.Value
                    : (TimeSpan?)null;
            }
        }

        // How long until next run starts
        public TimeSpan? NextRunTime
        {
            get
            {
                if (_nextRun == null || _nextRunFromTime == null || _startMining == null)
                    return null;

                _dynamicSwitchTime =
                    TimeSpan.FromSeconds((_switchTime.TotalSeconds /
                                          Math.Pow((double)_profitBestOverRunning, _dynamicSwitchPower) +
                                          _dynamicSwitchOffset));

                TimeSpan? timeToSwitch = _dynamicSwitching
                    ? _dynamicSwitchTime - (DateTime.Now - _nextRunFromTime)
                    : _switchTime - (DateTime.Now - _nextRunFromTime);
                TimeSpan? timeToMin = _minTime - (DateTime.Now - _startMining);

                return timeToMin > timeToSwitch ? timeToMin : timeToSwitch;
            }
        }

        public DateTime? ExitTime
        {
            get
            {
                if (_autoExitTime < new TimeSpan(0, 1, 0)) return null;
                return _engineCreation + _autoExitTime;
            }
        }

        public IList<IService> Services
        {
            get { return _services; }
        }

        public IList<PriceEntryBase> PriceEntries
        {
            get { return _priceEntries; }
        }

        public IList<AlgorithmEntry> AlgorithmEntries
        {
            get { return _algorithmEntries; }
        }

        public IList<ServiceHistory> PriceHistories
        {
            get { return _priceHistories; }
        }

        public TimeSpan TotalTime
        {
            get
            {
                double totalTime = PriceEntries.Sum(o => o.TimeMining.TotalMilliseconds);
                if (_startMining.HasValue)
                    totalTime += (DateTime.Now - _startMining.Value).TotalMilliseconds;
                return TimeSpan.FromMilliseconds(totalTime);
            }
        }

        // Signals for UI updates

        public bool PricesUpdated
        {
            get { return _pricesUpdated; }
            set { _pricesUpdated = value; }
        }

        public bool HasPrices
        {
            get { return _hasPrices; }
            set { _hasPrices = value; }
        }

        public bool ShowHistory
        {
            get { return _showHistory; }
            set { _showHistory = value; }
        }

        #region Donation mining settings

        public TimeSpan _autoMiningTime = TimeSpan.Zero;
        private MiningModeEnum _donationMiningMode = MiningModeEnum.Stopped;
        private TimeSpan _donationMiningTime = TimeSpan.Zero;
        private TimeSpan _donationfrequency = TimeSpan.FromMinutes(60);
        private double _donationpercentage = 0.005;

        public TimeSpan miningTime = TimeSpan.Zero;

        public bool DoDonationMinging
        {
            get { return _donationpercentage > 0 && _donationfrequency > TimeSpan.Zero; }
        }

        private TimeSpan MiningBeforeDonation
        {
            get
            {
                if (!DoDonationMinging) return TimeSpan.Zero;
                return TimeSpan.FromMinutes(_donationfrequency.TotalMinutes * (1 - _donationpercentage));
            }
        }

        private TimeSpan MiningDuringDonation
        {
            get
            {
                if (!DoDonationMinging) return TimeSpan.Zero;
                return _donationfrequency - MiningBeforeDonation + _delay;
            }
        }

        public TimeSpan TimeUntilDonation
        {
            get
            {
                if (!DoDonationMinging) return TimeSpan.Zero;
                 miningTime = _autoMiningTime;
                if (MiningMode == MiningModeEnum.Automatic && _startMining.HasValue)
                    miningTime += (DateTime.Now - _startMining.Value);
                TimeSpan value = MiningBeforeDonation - miningTime;
                return value < TimeSpan.Zero ? TimeSpan.Zero : value;
            }
        }

        public TimeSpan TimeDuringDonation
        {
            get
            {
                if (!DoDonationMinging) return TimeSpan.Zero;
                 miningTime = _donationMiningTime;
                if (MiningMode == MiningModeEnum.Donation && _startMining.HasValue)
                    miningTime += (DateTime.Now - _startMining.Value);
                TimeSpan value = MiningDuringDonation - miningTime;
                return value < TimeSpan.Zero ? TimeSpan.Zero : value;
            }
        }

        #endregion

        #region Remote console

        private readonly IPEndPoint _endPoint = new IPEndPoint(new IPAddress(new byte[] { 239, 14, 10, 30 }),
            RemotePortNumber);

        private bool _remoteReceive;

        private MulticastReceiver _remoteReceiver;
        private bool _remoteSend;
        private MulticastSender _remoteSender;

        public bool RemoteReceive
        {
            get { return _remoteReceive; }
        }

        #endregion

        public void Cleanup()
        {
                        
            WriteConsoleAction = null;
            WriteRemoteAction = null;          

            if (_currentRunning != null && _currentRunning.UseWindow == false)
                StopMiner();

            if (_process != null)
                _process.Dispose();

            if (_remoteSender != null)
                _remoteSender.Dispose();

            if (_remoteReceiver != null)
                _remoteReceiver.Dispose();
        }

        public bool LoadConfig(string CFile)
        {
            string configFile = GetConfigPath();

            if (CFile != null)
            {
                configFile = CFile;
            }
            

            if (!File.Exists(configFile))
            {
                MessageBox.Show(string.Format("Config file, '{0}', not found.", configFile),
                    "Miner Control: Config file missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            Dictionary<string, object> data;

            try
            {
                string pageString = File.ReadAllText(configFile);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                data = serializer.DeserializeObject(pageString) as Dictionary<string, object>;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(string.Format("Error loading config file: '{0}'.", ex.Message),
                    "Miner Control: Config file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                LoadConfigGeneral(data["general"] as Dictionary<string, object>);
                LoadConfigAlgorithms(data["algorithms"] as object[]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error processing general configuration: '{0}'.", ex.Message),
                    "Miner Control: Config file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                LoadService(new NiceHashService(), data, "Nicehash");
                LoadService(new NanopoolService(), data, "Nanopool");
                LoadService(new MiningPoolHubService(), data, "MiningPoolHub");
                LoadService(new YIIMPService(), data, "YIIMP");
                LoadService(new CoinMinersService(), data, "CoinMiners");
                LoadService(new SuprnovaService(), data, "Suprnova");
                LoadService(new CoinMineService(), data, "CoinMine");
                LoadService(new AikaPoolService(), data, "AikaPool");
                LoadService(new MiningfieldService(), data, "Miningfield");
                LoadService(new EthermineService(), data, "Ethermine");
                LoadService(new LuckPoolService(), data, "LuckPool");
                LoadService(new DwarfpoolService(), data, "Dwarfpool");
                LoadService(new TheBlockFactoryService(), data, "TheBlockFactory");
                LoadService(new GiveMeCoinsService(), data, "GiveMeCoins");
                LoadService(new CoinotronService(), data, "Coinotron");
                LoadService(new DualMiningService(), data, "DualMining");
                LoadService(new LtcRabbitService(), data, "LTCRabbit");
                LoadService(new WePayBtcService(), data, "WepayBTC");

                foreach (string service in data.Keys)
                {
                    switch (service.ToLower())
                    {
                        case "general": break;
                        case "algorithms": break;
                        case "nicehash": break;                      
                        case "nanopool": break;
                        case "miningpoolhub": break;
                        case "yiimp": break;
                        case "coinminers": break;
                        case "ethermine": break;
                        case "luckpool": break;
                        case "dwarfpool": break;
                        case "theblockfactory": break;
                        case "givemecoins": break;
                        case "coinmine": break;
                        case "suprnova": break;
                        case "aikapool": break;
                        case "miningfield": break;     
                        case "coinotron": break;
                        case "dualmining": break;
                        case "ltcrabbit": break;
                        case "wepaybtc": break;

                        default:
                            Dictionary<string, object> serviceData = data[service] as Dictionary<string, object>;
                            if (service.StartsWith("manual", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (serviceData != null && serviceData.ContainsKey("name"))
                                    LoadService(new ManualService(serviceData.GetString("name")), serviceData);
                                else
                                    LoadService(new ManualService(service), serviceData);
                            }
                            else if (service.StartsWith("WTM-", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (serviceData != null && serviceData.ContainsKey("name"))
                                    LoadService(new WhattomineService(serviceData.GetString("name")), serviceData);

                                else
                                    LoadService(new WhattomineService(service), serviceData);
                            }
                            else if (service.StartsWith("MPOS-", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (serviceData != null && serviceData.ContainsKey("name"))
                                    LoadService(new MPOSCloneService(serviceData.GetString("name")), serviceData);

                                else
                                    LoadService(new MPOSCloneService(service), serviceData);
                            }
                            else
                            {
                                //All others are treated as YaampClones
                                LoadService(new YAAMPCloneService(service), serviceData);
                            }
                            break;
                    }
                }

                // Set Id for each entry
                for (int x = 0; x < _priceEntries.Count; x++)
                    _priceEntries[x].Id = x + 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error processing service configuration: '{0}'.", ex.Message),
                    "Miner Control: Configuration file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (_remoteSend)
            {
                _remoteSender = new MulticastSender(_endPoint, 1);
                _remoteSender.Start();
            }

            if (_remoteReceive)
            {
                _remoteReceiver = new MulticastReceiver(_endPoint);
                _remoteReceiver.DataReceived += ProcessRemoteData;
                _remoteReceiver.Start();
            }

            return true;
        }

        private string GetConfigPath()
        {

            string config = "MinerControl";

            Process[] processList = Process.GetProcessesByName(config);
            if (processList.Length > 1) config += processList.Length;
            config += ".conf";

            return File.Exists(config) ? config : "MinerControl.conf";
        }

        private void LoadService(IService service, IDictionary<string, object> data, string name)
        {

            if (!data.ContainsKey(name)) return;

            Dictionary<string, object> serviceData = data[name] as Dictionary<string, object>;
            LoadService(service, serviceData);
        }

        private void LoadService(IService service, Dictionary<string, object> serviceData)
        {
            if (serviceData.ContainsKey("poolenabled"))
                _poolEnabled = bool.Parse(serviceData["poolenabled"].ToString());
            else _poolEnabled = true;
            if (_poolEnabled)
            {
                service.MiningEngine = this;
                _services.Add(service);
                service.Initialize(serviceData);

                _priceHistories.Add(new ServiceHistory(service.ServiceName, _statWindow, _outlierPercentage, _iqrMultiplier));
            }
        }

        private void LoadConfigGeneral(IDictionary<string, object> data)
        {
            _powerCost = data["power"].ExtractDecimal();
            _exchange = data["exchange"].ExtractDecimal();
            if (data.ContainsKey("currencycode"))
                _currencyCode = data["currencycode"].ToString().ToUpper();
            _minTime = TimeSpan.FromMinutes((double)data["mintime"].ExtractDecimal());
            _maxTime = TimeSpan.FromMinutes((double)data["maxtime"].ExtractDecimal());
            _switchTime = TimeSpan.FromMinutes((double)data["switchtime"].ExtractDecimal());
            _deadtime = TimeSpan.FromMinutes((double)data["deadtime"].ExtractDecimal());

            double delay = data.ContainsKey("delay") ? (double)data["delay"].ExtractDecimal() : 0;
            _delay = TimeSpan.FromSeconds(delay);

            double autoExitTime = data.ContainsKey("exittime") ? (double)data["exittime"].ExtractDecimal() : 0;
            _autoExitTime = TimeSpan.FromMinutes(autoExitTime);           
            _ignoreOutliers = (data.ContainsKey("ignoreoutliers") && bool.Parse(data["ignoreoutliers"].ToString()));
             statWindow = data.ContainsKey("statwindow") ? (double)data["statwindow"].ExtractDecimal() : 60;
            _statWindow = TimeSpan.FromMinutes(statWindow);
            _iqrMultiplier = data.ContainsKey("iqrmultiplier")
                ? data["iqrmultiplier"].ExtractDecimal()
                : 2.2M;
            _outlierPercentage = data.ContainsKey("outlierpercentage")
                ? (double)data["outlierpercentage"].ExtractDecimal()
                : 0.99;

            _showHistory = !data.ContainsKey("showhistory") || bool.Parse(data["showhistory"].ToString());

            if (data.ContainsKey("minerfolderpath"))
                MinerFolderPath = (data["minerfolderpath"].ToString());

            if (data.ContainsKey("logerrors"))
                ErrorLogger.LogExceptions = bool.Parse(data["logerrors"].ToString());
            if (data.ContainsKey("logactivity"))
                _logactivity = bool.Parse(data["logactivity"].ToString());
            if (data.ContainsKey("minebyaverage"))
                _mineByAverage = bool.Parse(data["minebyaverage"].ToString());
            if (data.ContainsKey("minerkillmode"))
                MinerKillMode = int.Parse(data["minerkillmode"].ToString());
            if (data.ContainsKey("gridsortmode"))
                GridSortMode = int.Parse(data["gridsortmode"].ToString());
            if (data.ContainsKey("traymode"))
                TrayMode = int.Parse(data["traymode"].ToString());
            if (Program.MinimizeToTray && TrayMode == 0)
                TrayMode = 2;
            if (data.ContainsKey("donationpercentage"))
                _donationpercentage = (double)(data["donationpercentage"].ExtractDecimal()) / 100;
                //_donationpercentage = ((double)(data["donationpercentage"].ExtractDecimal()) + _donationpercentage) / 100;
            if (data.ContainsKey("donationfrequency"))
                _donationfrequency = TimeSpan.FromMinutes((double)data["donationfrequency"].ExtractDecimal());
           // if (_donationfrequency > TimeSpan.FromMinutes(60))
           //     _donationfrequency = TimeSpan.FromMinutes(60);
            if (data.ContainsKey("remotesend"))
                _remoteSend = bool.Parse(data["remotesend"].ToString());
            if (data.ContainsKey("remotereceive"))
                _remoteReceive = bool.Parse(data["remotereceive"].ToString());

            if (data.ContainsKey("dynamicswitching"))
                _dynamicSwitching = bool.Parse(data["dynamicswitching"].ToString());
            _dynamicSwitchPower = data.ContainsKey("dynamicswitchpower")
                ? double.Parse(data["dynamicswitchpower"].ToString())
                : 2;
            _dynamicSwitchPivot = data.ContainsKey("dynamicswitchpivot")
                ? double.Parse(data["dynamicswitchpivot"].ToString())
                : 1.05;
            _dynamicSwitchOffset = data.ContainsKey("dynamicswitchoffset")
                ? double.Parse(data["dynamicswitchoffset"].ToString())
                : Math.Pow(_dynamicSwitchPivot, _dynamicSwitchPower) != 0
                    ? _switchTime.TotalMinutes -
                      (_switchTime.TotalMinutes * Math.Pow(1 / _dynamicSwitchPivot, _dynamicSwitchPower))
                    : 0.5;

            _minProfit = data.ContainsKey("minprofit")
                ? data["minprofit"].ExtractDecimal()
                : 1M;

            string minPrice = data.ContainsKey("minprice") ? data["minprice"].ToString() : "0";
            if (minPrice.EndsWith("BTC"))
            {
                string trimmed = minPrice.Remove(minPrice.Length - 3);
                _minPrice = trimmed.ExtractDecimal();
            }
            else
            {
                _minPrice = minPrice.ExtractDecimal() / _exchange;
            }

            if (data.ContainsKey("usewindow")) _usewindow = bool.Parse(data["usewindow"].ToString());

            if (data.ContainsKey("wtmextracoins")) WTMExtraCoins = bool.Parse(data["wtmextracoins"].ToString());

            if (data.ContainsKey("wtmbuffer")) _bufferarray = bool.Parse(data["wtmbuffer"].ToString());

            if (data.ContainsKey("wtmsyncperiod")) WTMSyncPeriod = (double) data["wtmsyncperiod"].ExtractDecimal();

            if (data.ContainsKey("wtmurl")) WTMUrl = data["wtmurl"].ToString();

            if (data.ContainsKey("avgspeedticks")) _AvSpTicks = int.Parse(data["avgspeedticks"].ToString());
            //if (_AvSpTicks > 60) _AvSpTicks = 60;

            if (data.ContainsKey("minidleseconds")) _MinIdleSeconds = (int)data["minidleseconds"];
            if (data.ContainsKey("dynoffset")) _dynoffset = data["dynoffset"].ExtractDecimal();
            if (data.ContainsKey("dyncheck")) _dyncheck= data["dyncheck"].ExtractDecimal();
            if (data.ContainsKey("showinactive")) _showinactive= bool.Parse(data["showinactive"].ToString());
            if (data.ContainsKey("hidecolumngr1")) _hidecolumngr1 = bool.Parse(data["hidecolumngr1"].ToString());
            if (data.ContainsKey("hidecolumngr2")) _hidecolumngr2 = bool.Parse(data["hidecolumngr2"].ToString());
            if (data.ContainsKey("coloring")) _coloring = bool.Parse(data["coloring"].ToString());
            if (data.ContainsKey("pumpreduction")) _pumpreduction = bool.Parse(data["pumpreduction"].ToString());

            if (_bufferarray == true)
            {
                if (WTMArray == null || WTMArray.Length == 0 || WTMTime.AddMinutes(WTMSyncPeriod - 0.2) < DateTime.Now)
                    WTMGetPrices(WebUtil3.DownloadJson(WTMUrl));
            }

        }

        private void LoadConfigAlgorithms(object[] data)
        {
            if (MinerFolderPath =="MinerControl") 
                MinerFolderPath = Environment.CurrentDirectory + "\\" ;


            foreach (object rawitem in data)
            {
                Dictionary<string, object> item = rawitem as Dictionary<string, object>;
                AlgorithmEntry entry = new AlgorithmEntry
                {
                    Name = item["name"] as string,
                    Display =
                        item.ContainsKey("display")
                            ? item["display"] as string
                            : GetAlgoDisplayName(item["name"] as string),
                    Hashrate = item["hashrate"].ExtractDecimal(),
                    Power = item["power"].ExtractDecimal(),
                    Priority = item.GetString("priority") ?? string.Empty,
                    MU = item.GetString("mu") ?? string.Empty,                    
                    DevFee = item.ContainsKey("devfee") ? item["devfee"].ExtractDecimal() : 0m,
                    Affinity = item.GetInt("affinity") ?? 0,
                    Param1 = MinerFolderPath + item.GetString("aparam1") ?? string.Empty,
                    Param2 = item.GetString("aparam2") ?? string.Empty,
                    Param3 = item.GetString("aparam3") ?? string.Empty
                };

                _algorithmEntries.Add(entry);
            }
        }

        public void StopMiner()
        {
            if (!_process.IsRunning())
            {
                _process = null;
                return;
            }

            LogActivity(_donationMiningMode == MiningModeEnum.Donation ? "DonationStop" : "Stop");
            WriteConsole(string.Format("Stopping {0} {1}", _currentRunning.ServicePrint, _currentRunning.AlgoName), true);
            RecordMiningTime();
            if (MinerKillMode == 0)
                ProcessUtil.KillProcess(_process);
            else
                ProcessUtil.KillProcessAndChildren(_process.Id);

            _process = null;
            _donationMiningMode = MiningModeEnum.Stopped;

            if (_currentRunning != null)
            {
                PriceEntryBase entry = PriceEntries.Single(o => o.Id == _currentRunning.Id);
                entry.UpdateStatus();
            }

            if (_stoppedMining == null) _stoppedMining = DateTime.Now;
            _currentRunning = null;
        }

        private void RecordMiningTime()
        {
            if (_currentRunning == null || !_startMining.HasValue) return;

            if (_donationMiningMode == MiningModeEnum.Automatic) _autoMiningTime += (DateTime.Now - _startMining.Value);
            if (_donationMiningMode == MiningModeEnum.Donation)
                _donationMiningTime += (DateTime.Now - _startMining.Value);

            _currentRunning.TimeMining += (DateTime.Now - _startMining.Value);
            _currentRunning.UpdateStatus();
            _startMining = null;
        }

        private void StartMiner(PriceEntryBase entry, bool isMinimizedToTray = false)
        {
            _nextRun = null;
            _nextRunFromTime = null;
            _currentRunning = entry;
            _startMining = DateTime.Now;
            _stoppedMining = null;

            _process = new Process();
            if (_donationMiningMode == MiningModeEnum.Donation)
            {
                if (!string.IsNullOrWhiteSpace(entry.DonationFolder))
                    _process.StartInfo.WorkingDirectory = entry.DonationFolder;
                _process.StartInfo.FileName = string.IsNullOrWhiteSpace(entry.DonationFolder)
                    ? entry.DonationCommand
                    : string.Format(@"{0}\{1}", entry.DonationFolder, entry.DonationCommand);
                
                if (entry.Arguments.Contains ("nicehash"))
                _process.StartInfo.Arguments = entry.DonationArguments.Replace("usa", DetectNiceHashLocation(entry));
                else
                    _process.StartInfo.Arguments = entry.DonationArguments;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(entry.Folder))
                    _process.StartInfo.WorkingDirectory = entry.Folder;
                _process.StartInfo.FileName = string.IsNullOrWhiteSpace(entry.Folder)
                    ? entry.Command
                    : string.Format(@"{0}\{1}", entry.Folder, entry.Command);
                if (entry.Arguments.Contains("nicehash"))
                    _process.StartInfo.Arguments = entry.Arguments.Replace("usa", DetectNiceHashLocation(entry));
                else
                    _process.StartInfo.Arguments = entry.Arguments;
            }

            WriteConsole(
                string.Format("Starting {0} {1} with {2} {3}", _currentRunning.ServicePrint, _currentRunning.Name,
                    _process.StartInfo.FileName, _process.StartInfo.Arguments), true);

            if (!string.IsNullOrWhiteSpace(_process.StartInfo.WorkingDirectory) &&
                !Directory.Exists(_process.StartInfo.WorkingDirectory))
            {
                entry.DeadTime = DateTime.Now;
                string message = string.Format("Path '{0}' does not exist.", _process.StartInfo.WorkingDirectory);
                _process = null;
                WriteConsole(message, true);
                throw new ArgumentException(message);
            }
            if (!string.IsNullOrWhiteSpace(_process.StartInfo.FileName) && !File.Exists(_process.StartInfo.FileName))
            {
                entry.DeadTime = DateTime.Now;
                string message = string.Format("File '{0}' does not exist.", _process.StartInfo.FileName);
                _process = null;
                WriteConsole(message, true);
                throw new ArgumentException(message);
            }

            if (entry.UseWindow)
            {
                _process.StartInfo.WindowStyle = (isMinimizedToTray && TrayMode == 2)
                    ? ProcessWindowStyle.Hidden
                    : ProcessWindowStyle.Minimized;
                _process.Start();

                Thread.Sleep(100);
                try
                {
                    ProcessUtil.SetWindowTitle(_process, string.Format("{0} {1} Miner", entry.ServicePrint, entry.Name));
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                }

                if (isMinimizedToTray && TrayMode == 1)
                    HideMinerWindow();
            }
            else
            {
                _process.StartInfo.RedirectStandardOutput = true;
                _process.StartInfo.RedirectStandardError = true;
                _process.EnableRaisingEvents = true;
                _process.StartInfo.CreateNoWindow = true;
                _process.StartInfo.UseShellExecute = false;

                _process.ErrorDataReceived += ProcessConsoleOutput;
                _process.OutputDataReceived += ProcessConsoleOutput;

                _process.Start();

                _process.BeginOutputReadLine();
                _process.BeginErrorReadLine();
            }

            ProcessPriorityClass processPriority;
            if (entry.Priority != "Normal" && entry.Priority != string.Empty
                && Enum.TryParse(entry.Priority, out processPriority))
            {
                // Defaults to Normal, other possible values are Idle, BelowNormal, 
                // AboveNormal, High & RealTime. ccminer <3 RealTime
                // Note 1: Realtime by minercontrol is only possible when given administrator privileges to minercontrol
                // Note 2: --cpu-priority by ccminer overrides minercontrols priority
                // Note 3: When giving administrator privileges to minercontrol and setting the priority by minercontrol to 
                // something DIFFERENT than what's used by --cpu-priority by ccminer, then your whole system locks up 
                _process.PriorityClass = processPriority;
            }

            if (entry.Affinity > 0)
            {
                // Just like with start /affinity, you can use 1 for first core, 2 for second core, 4 for third core, etc
                _process.ProcessorAffinity = (IntPtr)entry.Affinity;
            }

            _startMining = DateTime.Now;
            _donationMiningMode = MiningMode;

            entry.UpdateStatus();

            LogActivity(_donationMiningMode == MiningModeEnum.Donation ? "DonationStart" : "Start");
        }

        private string DetectNiceHashLocation(PriceEntryBase entry)
        {
            if (entry.ServiceEntry.ServiceName != "NiceHash" && entry.ServiceEntry.ServiceName != "WestHash") return string.Empty;
            NiceHashService niceHash = entry.ServiceEntry as NiceHashService;
            if (niceHash == null || niceHash.DetectLocation == false) return string.Empty;

           // return "-o stratum+tcp://" + niceHash.GetBestLocation(entry.AlgoName) + " ";

            string path = niceHash.GetBestLocation(entry.AlgoName).Remove(".nicehash.com");
            path = path.Substring(1);
            return path;
    
        }

        private void ClearDeadTimes()
        {
            foreach (PriceEntryBase entry in _priceEntries)
                entry.DeadTime = DateTime.MinValue;
        }

        public void RequestStop()
        {
            _nextRun = null;
            _nextRunFromTime = null;

            StopMiner();
            ClearDeadTimes();
        }

        public void RequestStart(int id, bool isMinimizedToTray = false)
        {
            PriceEntryBase entry = _priceEntries.Single(o => o.Id == id);
            StartMiner(entry, isMinimizedToTray);
        }

        public void RequestStart(string service, string algo, bool isMinimizedToTray)
        {
            PriceEntryBase entry = null;
            foreach (PriceEntryBase priceEntry in _priceEntries)
            {
                if (priceEntry.AlgoName == algo)
                {
                    entry = priceEntry;
                    if (priceEntry.ServiceEntry.ServiceName == service)
                    {
                        break;
                    }
                }
            }

            if (entry == null)
            {
                RunBestAlgo(isMinimizedToTray);
            }
            else
            {
                StartMiner(entry, isMinimizedToTray);
            }
        }

        public void CheckFees()
        {
            foreach (IService service in _services)
            {
                service.CheckFees();  
            }
        }

        public void CheckPrices()
        {
            WTMLoad();
            foreach (IService service in _services)
            {
                service.CheckPrices();
            }
        }

        public void CheckData()
        {
            foreach (IService service in _services)
            {
                service.CheckData();
            }
        }
        public void UpdateBalance()
        {
            foreach (IService service in _services)
            {
                service.UpdateBalance();
            }
        }

        public void RunBestAlgo(bool isMinimizedToTray)
        {
            try
            {
                // Check for dead process
                if (!_process.IsRunning() && _currentRunning != null)
                {
                    lock (this)
                    {
                        _currentRunning.DeadTime = DateTime.Now;
                        LogActivity(_donationMiningMode == MiningModeEnum.Donation ? "DonationDead" : "Dead");
                        WriteConsole(string.Format("Dead {0} {1}", _currentRunning.ServicePrint, _currentRunning.Name),
                            true);
                        RecordMiningTime();
                    }
                }

                // Clear information if process not running
                if (!_process.IsRunning())
                {
                    _currentRunning = null;
                    _startMining = null;
                    _nextRun = null;
                    _nextRunFromTime = null;
                }

                // Donation mining
                if (DoDonationMinging)
                {
                    if (_donationMiningMode == MiningModeEnum.Automatic && TimeUntilDonation == TimeSpan.Zero)
                    {
                        StopMiner();
                        _donationMiningMode = MiningModeEnum.Donation;
                        MiningMode = _donationMiningMode;
                        _autoMiningTime = TimeSpan.Zero;
                    }
                    else if (_donationMiningMode == MiningModeEnum.Donation && TimeDuringDonation == TimeSpan.Zero)
                    {
                        StopMiner();
                        _donationMiningMode = MiningModeEnum.Automatic;
                        MiningMode = _donationMiningMode;
                        _donationMiningTime = TimeSpan.Zero;
                    }
                }

                // Restart miner if max time reached
                if (RestartTime.HasValue && RestartTime.Value <= TimeSpan.Zero)
                    StopMiner();

                
                // Find the best, live entry
                PriceEntryBase best =
                    _priceEntries
                        .Where(o => !IsBadEntry(o))
                        .Where(o =>
                                !string.IsNullOrWhiteSpace(_donationMiningMode == MiningModeEnum.Donation
                                    ? o.DonationCommand
                                    : o.Command))
                                    .OrderByDescending(o => _mineByAverage ? o.NetAverage : o.NetEarn)
                        .FirstOrDefault();

                // If none is found, because they're all banned, dead, below minprice
                // All should quit
                if (best == null && _currentRunning != null)
                {
                    StopMiner();
                    return;
                }

                // If the current pool is banned, it should directly start the best one
                if (_currentRunning != null && _currentRunning.Banned
                    && best.Id != _currentRunning.Id)
                {
                    StopMiner();
                    StartMiner(best, isMinimizedToTray);
                    return;
                }

                decimal highestMinProfit = 1M;

                // Handle minimum time for better algorithm before switching
                if (_switchTime > TimeSpan.Zero && _currentRunning != null)
                {
                    if (!_nextRun.HasValue && _currentRunning.Id != best.Id)
                    {
                        _nextRun = best.Id;
                        _nextRunFromTime = DateTime.Now;
                    }
                    else if (_nextRun.HasValue && _currentRunning.Id == best.Id)
                    {
                        _nextRun = null;
                        _nextRunFromTime = null;
                    }

                    _profitBestOverRunning = _mineByAverage ? best.NetAverage / _currentRunning.NetAverage : best.NetEarn / _currentRunning.NetEarn;
                    highestMinProfit = best.ServiceEntry.ServiceName != _currentRunning.ServiceEntry.ServiceName
                        ? Math.Max(best.MinProfit, _minProfit)
                        : _minProfit;


                    if (NextRunTime.HasValue && NextRunTime > TimeSpan.Zero)
                        best = _priceEntries.First(o => o.Id == _currentRunning.Id);
                }

                // Update undead entries
                IEnumerable<PriceEntryBase> entries =
                    PriceEntries.Where(o => !o.IsDead && o.DeadTime != DateTime.MinValue);
                foreach (PriceEntryBase entry in entries)
                    entry.DeadTime = DateTime.MinValue;


                if (_currentRunning != null
                    // Guarantees a minimum profit before switching
                    && (_profitBestOverRunning < highestMinProfit
                    // Keeps outliers pending/ignores them if requested and not mining by average
                    || (!_mineByAverage && _ignoreOutliers && best.Outlier)
                    // Just update time if we are already running the right entry
                    || _currentRunning.Id == best.Id
                    // Honor minimum time to run in auto mode
                    || (MiningTime.HasValue && MiningTime.Value < _minTime)))
                {
                    _currentRunning.UpdateStatus();
                    return;
                }

                StopMiner();
                if (_stoppedMining == null || _stoppedMining + _delay <= DateTime.Now) StartMiner(best, isMinimizedToTray);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
            }
        }

        public void MinPrice()
        {
            foreach (PriceEntryBase entry in _priceEntries)
            {
                if (entry.Price == 0 || entry.NetEarn < _minPrice) entry.BelowMinPrice = true;
                else    entry.BelowMinPrice = false;
            }
        }

        public bool IsBadEntry(PriceEntryBase priceEntry)
        {
            return (priceEntry.BelowMinPrice || priceEntry.Banned || priceEntry.IsDead || priceEntry.Lagging || !priceEntry.Enabled);
        }

        public void SwitchBanStatus(string pool)
        {
            foreach (PriceEntryBase priceEntry in _priceEntries)
            {
                if (priceEntry.ServicePrint == pool)
                {
                    priceEntry.Banned = !priceEntry.Banned;
                    priceEntry.Enabled = !priceEntry.Banned;
                }
            }                
        }

        public void HideMinerWindow()
        {
            if (_currentRunning == null || !_currentRunning.UseWindow) return;

            ProcessUtil.HideWindow(_process);
        }

        public void MinimizeMinerWindow()
        {
            if (_currentRunning == null || !_currentRunning.UseWindow) return;

            ProcessUtil.MinimizeWindow(_process);
        }

        public void LoadExchangeRates()
        {
            WebUtil1.DownloadJson("http://blockchain.info/ticker", ProcessExchangeRates);
        }

        private void ProcessExchangeRates(object jsonData)
        {
            Dictionary<string, object> data = jsonData as Dictionary<string, object>;
            if (!data.ContainsKey(_currencyCode)) return;
            Dictionary<string, object> item = data[_currencyCode] as Dictionary<string, object>;
            decimal exchange = item["last"].ExtractDecimal();

            string symbol = item["symbol"].ToString();
           //MainWindow.ServiceCurrency.HeaderText = "Balance " + symbol;

           lock (this)
            {
                if (exchange > 0 && exchange != _exchange)
                {
                    _exchange = exchange;
                    if (PriceEntries != null)
                        foreach (PriceEntryBase entry in PriceEntries)
                            entry.UpdateExchange();
                    if (Services != null)
                        foreach (IService service in Services)
                            service.UpdateExchange();
                }

                if (!string.IsNullOrWhiteSpace(symbol)) _currencySymbol = symbol;
            }
        }

        private void LogActivity(string action)
        {
            if (!_logactivity) return;

            //string[] items =
            //{
            //    DateTime.Now.ToString("s"),
            //    action,
            //    MiningMode.ToString(),
            //    _currentRunning != null ? _currentRunning.ServicePrint : string.Empty,
            //    _currentRunning != null ? _currentRunning.AlgoName : string.Empty,
            //    _currentRunning != null ? _currentRunning.Price.ToString("F6") : string.Empty,
            //    _currentRunning != null ? _currentRunning.Earn.ToString("F6") : string.Empty,
            //    _currentRunning != null ? _currentRunning.Fees.ToString("F6") : string.Empty,
            //    _currentRunning != null ? _currentRunning.Power.ToString("F6") : string.Empty,
            //    _currentRunning != null ? _currentRunning.NetEarn.ToString("F6") : string.Empty,
            //    _exchange.ToString("F2"),
            //    _currentRunning != null ? _currentRunning.ServiceEntry.ServiceBalance.ToString("F8") : string.Empty
            //};

            string[] items =
{
              //"time: " +
             DateTime.Now.ToString("s"),
              //"action: "+
             " "+ action,
              //"mode: "+
             " "+ MiningMode.ToString(),
              //"service: "+
              _currentRunning != null ? " "+ _currentRunning.ServicePrint : string.Empty,
              //"algo: "+
              _currentRunning != null ? " "+_currentRunning.AlgoName : string.Empty,
              _currentRunning != null ? " "+ _currentRunning.CoinName : string.Empty,
              _currentRunning != null ? " "+ _currentRunning.Tag : string.Empty,
              _currentRunning != null ? " hashrt:"+ _currentRunning.Hashrate.ToString("F2") : string.Empty,
              _currentRunning != null ? " tpavgsp:"+ _currentRunning.TopAvgSp.ToString("F2") : string.Empty,
              _currentRunning != null ? " plfee:"+ _currentRunning.PoolFeePrint : string.Empty,
              _currentRunning != null ? " price:"+ _currentRunning.Price.ToString("F6") : string.Empty,
              _currentRunning != null ? " earn:"+ _currentRunning.Earn.ToString("F6") : string.Empty,
              _currentRunning != null ? " fees:"+ _currentRunning.Fees.ToString("F6") : string.Empty,
              _currentRunning != null ? " power:"+ _currentRunning.Power.ToString("F4") : string.Empty,
              _currentRunning != null ? " netearn:"+ _currentRunning.NetEarn.ToString("F6") : string.Empty,
              _currentRunning != null ? " avgearn:"+ _currentRunning.NetAverage.ToString("F6") : string.Empty,
              //"exchange: "+  _exchange.ToString("F2"),
              _currentRunning != null ? " balBTC:"+ _currentRunning.BalanceBTC.ToString("F6") : string.Empty,
              _currentRunning != null ? " servbal:"+_currentRunning.ServiceEntry.ServiceBalance.ToString("F6") : string.Empty,
              _currentRunning != null ? " runtime:"+_currentRunning.ServiceEntry.TimeMiningPrint: string.Empty
            };

            string line = string.Join(",", items);
            const string logfile = "activity.log";

            bool exists = File.Exists(logfile);
            using (StreamWriter w = exists ? File.AppendText(logfile) : File.CreateText(logfile))
            {
                //if (!exists)
                //    w.WriteLine("time, action, mode, service, algo, hashrate, topavgsp, poolfee, price, earn, fees, power, netearn, avgearn, exchange, balanceBTC, servicebalance");
                w.WriteLine(line);
            }
        }

        public void WTMLoad()
        {
            if (_bufferarray == true)
            {
                if (WTMArray == null || WTMArray.Length == 0 || WTMTime.AddMinutes(WTMSyncPeriod - 0.2) < DateTime.Now)
                    WebUtil2.DownloadJson(WTMUrl, WTMGetPrices);
            }
        }        

        public void WTMGetPrices(object RawPrices)
        {
            if (RawPrices != null)
            {
                JObject data = (JObject)RawPrices;
                JToken coins = data["coins"];
                var cnt = coins.Count(); var i = 0;
                dynamic[,] WTMData = new dynamic[cnt, 6];

                foreach (JProperty coin in coins.Children())
                {
                    JToken item = coin.Value;
                    string cname = coin.Name.ToString().ToLower();
                    float price = 0; float price24 = 0;
                    float bt = 0; float br = 0; float br24 = 0; float nh = 0; float er = 0; float er24 = 0;

                    bt = (float)item["block_time"];
                    nh = (float)item["nethash"];
                    br = (float)item["block_reward"];
                    er = (float)item["exchange_rate"];
                    br24 = (float)item["block_reward24"];
                    er24 = (float)item["exchange_rate24"];
                    bool _lagstatus = bool.Parse(item["lagging"].ToString());
                    price24 = 86400 / bt * br24 / nh * er24;
                    price = 86400 / bt * br / nh * er;

                    WTMData[i, 0] = cname;
                    WTMData[i, 1] = price;
                    WTMData[i, 2] = price24;
                    WTMData[i, 3] = er;
                    WTMData[i, 4] = er24;
                    WTMData[i, 5] = _lagstatus;
                    i = i + 1;
                }
                WTMTime = DateTime.Now;
                WTMArray = WTMData;
            }
        }

      


        #region Console interaction

        public Action<string> WriteConsoleAction { get; set; }
        public Action<IPAddress, string> WriteRemoteAction { get; set; }

        private void WriteConsole(string text, bool prefixTime = false)
        {
            if (WriteConsoleAction == null) return;

            if (prefixTime)
                text = string.Format("[{0:HH:mm:ss}] {1}", DateTime.Now, text);

            WriteConsoleAction(text);

            if (_remoteSender == null) return;
            text = string.Format("{0} {1}", "CON", text);
            _remoteSender.Send(text);
        }

        private void ProcessConsoleOutput(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;
            WriteConsole(e.Data);
        }

        private void ProcessRemoteData(object sender, MulticastDataReceivedEventArgs e)
        {
            if (WriteRemoteAction == null) return;

            try
            {
                string data = e.StringData;
                string command = data.Substring(0, 4);
                string body = data.Substring(4);

                switch (command)
                {
                    case "CON ":
                        WriteRemoteAction(e.RemoteEndPoint.Address, body);
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
            }
        }

        #endregion

        #region Helpers

        private readonly IDictionary<string, string> _algoNames = new Dictionary<string, string>
        {
            {"x11", "X11"},
            {"x13", "X13"},
            {"x14", "X14"},
            {"x15", "X15"},
            {"scrypt", "Scrypt"},
            {"scryptn", "Scrypt-N"},
            {"sha256", "SHA256"},
            {"nist5", "Nist5"},
            {"keccak", "Keccak"},
            {"quark", "Quark"},
            {"neoscrypt", "NeoScrypt"}
        };

        private string GetAlgoDisplayName(string rawname)
        {
            return _algoNames.ContainsKey(rawname) ? _algoNames[rawname] : rawname;
        }

        #endregion
    }
}