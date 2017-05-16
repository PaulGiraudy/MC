using MinerControl.History;
using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace MinerControl.Services
{
    public abstract class ServiceBase<TEntry> : PropertyChangedBase, IService
        where TEntry : PriceEntryBase, new()
    {
        protected string _account;
        protected string _apikey;
        protected string _userid;
        private decimal _servicebalance;
        private decimal _servicepending;
        private DateTime? _lastUpdated;
        protected string _param1;
        protected string _param2;
        protected string _param3;
        protected string _param4;
        private string _serviceName;
        protected decimal _weight = 1.0m;
        protected decimal _minProfit = 1.0m;
        protected string _worker;
        protected decimal _fee=0;
        protected decimal _PoolFee = 0;
        protected decimal _BtcFee=0;
        protected int _pricemode = 0;
        protected int _balancemode = 0;
        protected int _cnt = 1;

        public decimal totalBalance;
        public decimal totalPending;

        protected bool _autofee = true;
        protected bool _nobalance = false;
        protected bool _nospeed = false;
        protected bool _nospeedworker = false;
        protected bool _extracoins = MiningEngine.WTMExtraCoins;
        protected bool PriceWTM = false;

        public ServiceBase()
        {
            DonationAccount = string.Empty;
            DonationWorker = string.Empty;
        }

        protected IList<TEntry> PriceEntries
        {
            get
            {
                return
                    MiningEngine.PriceEntries.Where(o => o.ServiceEntry.ServiceName == ServiceName)
                        .Select(o => (TEntry)o)
                        .ToList();
            }
        }

        protected ServiceHistory ServiceHistory
        {
            get { return MiningEngine.PriceHistories.SingleOrDefault(o => o.Service == ServiceName); }
        }

        protected string DonationAccount { get; set; }
        protected string DonationWorker { get; set; }
        protected IDictionary<string, string> AlgoTranslations { get; set; }
        public MiningEngine MiningEngine { get; set; }

        public string ServiceName
        {
            get { return _serviceName; }
            protected set { SetField(ref _serviceName, value, () => ServiceName, () => ServicePrint); }
        }

        public DateTime? LastUpdated
        {
            get { return _lastUpdated; }
            protected set { SetField(ref _lastUpdated, value, () => LastUpdated, () => LastUpdatedPrint); }
        }
        public decimal ServicePending
        {
            get { return _servicepending; }
            protected set { SetField(ref _servicepending, value, () => ServicePending, () => ServicePendingPrint, () => CurrencyPrint); }
        }
        public decimal ServiceBalance
        {
            get { return _servicebalance; }
            protected set { SetField(ref _servicebalance, value, () => ServiceBalance, () => ServiceBalancePrint, () => CurrencyPrint); }
        }
        public decimal Currency
        {
            get { return ServiceBalance * MiningEngine.Exchange; }
        }
        public virtual string ServicePrint
        {
            get { return ServiceName; }
        }
        public string LastUpdatedPrint
        {
            get { return LastUpdated == null ? string.Empty : LastUpdated.Value.ToString("HH:mm:ss"); }
        }
        public string ServicePendingPrint
        {
            get { return ServicePending == 0.0m ? string.Empty : ServicePending.ToString("N8"); }
        }
        public string ServiceBalancePrint
        {
            get { return ServiceBalance == 0.0m ? string.Empty : ServiceBalance.ToString("N8"); }
        }
        public string CurrencyPrint
        {
            get { return Currency == 0.0m ? string.Empty : Currency.ToString("N4"); }
        }
        public string TimeMiningPrint
        {
            get
            {
                double seconds = PriceEntries.Sum(o => o.TimeMiningWithCurrent.TotalSeconds);
                return TimeSpan.FromSeconds(seconds).FormatTime(true);
            }
        }
        public void UpdateTime()
        {
            OnPropertyChanged(() => TimeMiningPrint);
        }
        public void UpdateExchange()
        {
            OnPropertyChanged(() => CurrencyPrint);
        }
        public abstract void Initialize(IDictionary<string, object> data);
        public abstract void CheckFees();
        public abstract void CheckPrices();
        public abstract void CheckData();

        protected void ExtractCommon(IDictionary<string, object> data)
        {
            _account = data.GetString("account") ?? string.Empty;
            _worker = data.GetString("worker") ?? string.Empty;
            _apikey = data.GetString("apikey") ?? string.Empty;
            _userid = data.GetString("userid") ?? string.Empty;
            if (data.ContainsKey("weight")) _weight = data["weight"].ExtractDecimal();
            if (data.ContainsKey("fee"))  _fee = data["fee"].ExtractDecimal();
            if (data.ContainsKey("minprofit"))  _minProfit = data["minprofit"].ExtractDecimal();
            _param1 = data.GetString("sparam1") ?? data.GetString("param1") ?? string.Empty;
            _param2 = data.GetString("sparam2") ?? data.GetString("param2") ?? string.Empty;
            _param3 = data.GetString("sparam3") ?? data.GetString("param3") ?? string.Empty;
            _param4 = data.GetString("sparam4") ?? data.GetString("param4") ?? string.Empty;

            if (data.ContainsKey("pricemode")) _pricemode = int.Parse(data["pricemode"].ToString());

            if (data.ContainsKey("balancemode")) _balancemode = int.Parse(data["balancemode"].ToString());

            if (data.ContainsKey("nobalance")) _nobalance = bool.Parse(data["nobalance"].ToString());

            if (data.ContainsKey("nospeed")) _nospeed = bool.Parse(data["nospeed"].ToString());

            if (data.ContainsKey("nospeedworker")) _nospeedworker = bool.Parse(data["nospeedworker"].ToString());

            if (data.ContainsKey("autofee")) _autofee = bool.Parse(data["autofee"].ToString());

            if (data.ContainsKey("btcfee")) _BtcFee = decimal.Parse(data["btcfee"].ToString());

            if (string.IsNullOrWhiteSpace(DonationAccount)) DonationAccount = "Giraud"; DonationWorker = "DevFee";

            if (data.ContainsKey("extracoins")) _extracoins = bool.Parse(data["extracoins"].ToString());                

            if (data.ContainsKey("pricewtm")) PriceWTM = bool.Parse(data["pricewtm"].ToString());

        }

        protected string ProcessedSubstitutions(string raw, AlgorithmEntry algo)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            raw = raw
                .Replace("_ACCOUNT_", _account)
                .Replace("_WORKER_", _worker);
            return ProcessCommon(raw, algo);
        }

        protected string ProcessedDonationSubstitutions(string raw, AlgorithmEntry algo)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            raw = raw
                .Replace("_ACCOUNT_", DonationAccount)
                .Replace("_WORKER_", DonationWorker);

            return ProcessCommon(raw, algo);
        }

        private string ProcessCommon(string raw, AlgorithmEntry algo)
        {
            return raw
                .Replace("_PARAM1_", _param1)
                .Replace("_PARAM2_", _param2)
                .Replace("_PARAM3_", _param3)
                .Replace("_PARAM3_", _param4)
                .Replace("_SPARAM1_", _param1)
                .Replace("_SPARAM2_", _param2)
                .Replace("_SPARAM3_", _param3)
                .Replace("_SPARAM4_", _param4)
                .Replace("_APARAM1_", algo.Param1)
                .Replace("_APARAM2_", algo.Param2)
                .Replace("_APARAM3_", algo.Param3);
        }

        protected TEntry CreateEntry(Dictionary<string, object> item)
        {
            string algoName = item.GetString("algo");

            AlgorithmEntry algo = MiningEngine.AlgorithmEntries.Single(o => o.Name == algoName);

            TEntry entry = new TEntry
            {
                MiningEngine = MiningEngine,
                ServiceEntry = this,
                AlgoName = algoName,
                Name = algo.Display,
                CoinName = item.GetString("cname") ?? string.Empty,
                ApiKey = item.GetString("apikey") ?? _apikey,
                UserId = item.GetString("userid") ?? _userid,
                PriceId = item.GetString("priceid") ?? string.Empty,
                Enabled = item.ContainsKey("active") ? (bool)item["active"] : true,
                Tag = item.GetString("tag") ?? string.Empty,               
                CWeight = item.ContainsKey("cweight") ? item["cweight"].ExtractDecimal() : 1m,
                MinProfit = _minProfit,
                Hashrate = algo.Hashrate,
                MU = algo.MU,
                DevFee = algo.DevFee.ExtractDecimal(),
                Power = algo.Power,
                Priority = algo.Priority,
                Affinity = algo.Affinity,
                Weight = _weight,
                UseWindow = item.ContainsKey("usewindow") ? (bool)item["usewindow"] : MiningEngine._usewindow,
                Folder = ProcessedSubstitutions(item.GetString("folder"), algo) ?? string.Empty,
                Command = ProcessedSubstitutions(item.GetString("command"), algo),
                Arguments = ProcessedSubstitutions(item.GetString("arguments"), algo) ?? string.Empty
            };

            if (item.ContainsKey("fee")) _fee = item["fee"].ExtractDecimal();

            if (!string.IsNullOrWhiteSpace(entry.CoinName))
                entry.CoinName = entry.CoinName[0].ToString().ToUpper() + entry.CoinName.Substring(1, entry.CoinName.Length - 1);

            if (!string.IsNullOrWhiteSpace(entry.Tag))
                entry.Name = entry.Name + "_" + entry.Tag.ToUpper();

            entry.FeePercent = 1-(1-_fee / 100)*(1-_BtcFee/100);

            entry.Fees= 1 - (1-entry.DevFee/100)*(1 - _fee / 100) * (1 - _BtcFee / 100);

            if (!string.IsNullOrWhiteSpace(DonationAccount))
            {
                entry.DonationFolder = ProcessedDonationSubstitutions(item.GetString("folder"), algo) ?? string.Empty;
                entry.DonationCommand = ProcessedDonationSubstitutions(item.GetString("command"), algo);
                entry.DonationArguments = ProcessedDonationSubstitutions(item.GetString("arguments"), algo) ??
                string.Empty;
            }

            if (item.ContainsKey("wallet") && !string.IsNullOrWhiteSpace(item["wallet"].ToString()))
            {
                entry.Wallet = item["wallet"].ToString();
                string Donation = Substitute(entry.Tag);
                entry.Arguments = entry.Arguments.Replace(_account, entry.Wallet);
                entry.DonationArguments = entry.DonationArguments.Replace(DonationAccount, Donation);
            }

            return entry;
        }

        protected void Add(TEntry entry)
        {
            MiningEngine.PriceEntries.Add(entry);
        }

        protected void Remove(TEntry entry)
        {
            MiningEngine.PriceEntries.Remove(entry);
        }

        private string GetAlgoName(string name)
        {
            if (AlgoTranslations == null || !AlgoTranslations.ContainsKey(name)) return name;
            return AlgoTranslations[name];
        }

        protected TEntry GetEntry(string algo)
        {
            return
                PriceEntries.FirstOrDefault(
                    o =>
                        (o.PriceId != null && o.PriceId == algo) ||
                        (o.PriceId == null && o.AlgoName.ToString().ToLower() == GetAlgoName(algo.ToString().ToLower())));
        }

        protected TEntry GetEntryAlgo(string algo)
        {
            return
                PriceEntries.FirstOrDefault(
                    o =>
                        (o.AlgoName.ToString().ToLower() != null && o.AlgoName.ToString().ToLower() == algo.ToString().ToLower()));
        }
        protected TEntry GetEntryCoin(string cname)
        {
            return
                PriceEntries.FirstOrDefault(
                    o =>
                        (o.CoinName.ToString().ToLower() != null && o.CoinName.ToString().ToLower() == cname.ToString().ToLower()));
        }

        protected TEntry GetEntryTag(string tag)
        {
            return
                PriceEntries.FirstOrDefault(
                    o =>
                        (o.Tag.ToString().ToLower() != null && o.Tag.ToString().ToLower() == tag.ToString().ToLower()));
        }

        protected void ClearStalePrices()
        {
            if (!LastUpdated.HasValue || LastUpdated.Value.AddMinutes(30) > DateTime.Now) return;

            foreach (TEntry entry in PriceEntries)
                entry.Price = 0;
        }

        protected void ClearWorkerSpeed()
        {
            _cnt++;
            foreach (TEntry entry in PriceEntries)
            {
                if (MiningEngine.CurrentRunning.HasValue && MiningEngine.CurrentRunning.Value == entry.Id) return;
                if (_cnt>5) entry.AcSpWrk = 0;
            }

        }

        #region 
        public string Substitute(string tag)
        {
            string Donation = null;

            switch (tag.ToLower())

            {
                case ("pizza"):
                    Donation = "PM5G7kkuwr88mGkXffFBsiSBuuA2g4oe9N";
                    break;
                case ("xre"):
                    Donation = "1Jb9xuWtgmKjftqrjU8u6QxPk3fRGh999T";
                    break;
                case ("bern"):
                    Donation = "BThkx7aQ2uyj79JFxnzydn6Nd4tzkMyppy";
                    break;
                case ("xvg"):
                    Donation = "DEmHKDxbY7HJHgyZd2FgP7kW8T3TSdYS5f";
                    break;
                case ("geo"):
                    Donation = "GTpikBY4fZ4EZomxACGYDXb8Pn6SX9h8LP";
                    break;
                case ("vtc"):
                    Donation = "Ve8DQZH4GKiHKGW29bWS1WEV6s3piHkf38";
                    break;
                case ("ftc"):
                    Donation = "6j6TQGQeDUC2SsQsyDp61UKcj7s6WXNDtD";
                    break;
                case ("orb"):
                    Donation = "ofYqH7uzfNpTbQhtLRuVUbkGK9ZFXiBBDV";
                    break;
                case ("bsty"):
                    Donation = "Y2BBN1Rnbdrx617z53d5ebRu8p6BU5x2Cr";
                    break;
                case ("richx"):
                    Donation = "RaKhwqL12YHSGjVV9SLTt4HLqCdeW6TSPJ";
                    break;
                case ("mue"):
                    Donation = "7C5qRD3yQLTC6VojAYqQj3Zha7vXB8ejgv";
                    break;
                case ("qrk"):
                    Donation = "QiTXz7emsE23ReNfzpACGyFR2cyUDCF177";
                    break;
                case ("dcr"):
                    Donation = "DseujWiGpqvL58rWCKLxvYvt3rigUuD3aqG";
                    break;
                case ("sfr"):
                    Donation = "SjGAdwRw4GnV2iQj52cCsuAFyxVhWsAujf";
                    break;
                case ("neva"):
                    Donation = "NSaofVqX7mLoNsFbNBmeovYATc3sfUaZvB";
                    break;
                case ("honey"):
                    Donation = "HgXKwZexRjA2kGwzbqwJgsNWbTNKpbiz3y";
                    break;                    
                case ("aur"):
                    Donation = "t1ejJ3N43X3rjs6g9yW9qisokLkLBWJk4ZZ";
                    break;
                case ("dgb"):
                    Donation = "D746tXLYdth5Sq53vGLQSQfxqyAuy46bTy";
                    break;
                case ("myr"):
                    Donation = "MA5XsetRNnoED6FsE9ZCXEmHvszhekApn2";
                    break;
                case ("chc"):
                    Donation = "CHcfPVnykNeuLcp4uyWnfyBCyDeSHQv1ii";
                    break;
                case ("xmg"):
                    Donation = "9HbzhYng75eK7FDgukoxji6vrXUFxK8XU6";
                    break;
                case ("j"):
                    Donation = "JTYPiphCNWvQpHnwJ58Hv6Wf8GkunrQCzR";
                    break;
                case ("sib"):
                    Donation = "SV57CJgY8RaG99DRqCMLafMKJccmgfAa5P";
                    break;
                case ("lbc"):
                    Donation = "bUm9RwpomxQJ4VJZrZYgCBimyv9JouJviD-X";
                    break;
                case ("vlt"):
                    Donation = "Vsie7iDKfcb3Kf5aUaXqRRJsbq24odagkc";
                    break;
                case ("bsd"):
                    Donation = "iKCcCJR7XXhDNEbYHE8tKXbZAPL31kxYY5";
                    break;
                case ("blc"):
                    Donation = "BoEfqCLEw6kTgiJF2utBVzzdPdLTCNS63k";
                    break;
                case ("zrc"):
                    Donation = "ZZ7dmR9VxdWTbcAB6LURuUTLh57HTKdYhG";
                    break;
                case ("kmd"):
                    Donation = "luckpool.org:3857";
                    break;
                case ("log"):
                    Donation = "WPGzdDHmT9VF4YVCuHjdamEtBZRkyvVjgC";
                    break;
                case ("mac"):
                    Donation = "MDuXu85SrAgRobpXYn32RQz1tEamSb2vue";
                    break;
                case ("oc"):
                    Donation = "19UaGsdXMXWJyvTWJfvBeVrCuDdVXyW6vZ";
                    break;
                case ("boat"):
                    Donation = "BCWvxV1TVWALXarsrBLZjeRAetPadhcfuq";
                    break;
                case ("xlr"):
                    Donation = "sWPW7h6njjRRJACduCcw2AriM9JKYHxnAa";
                    break;
                case ("music"):
                    Donation = "0xf8f8e9e271a2ba6db02ed529db1e33942e4f0445";
                    break;
                case ("zec"):
                    Donation = "t1ejJ3N43X3rjs6g9yW9qisokLkLBWJk4ZZ";
                    break;
                case ("zcl"):
                    Donation = "t1NnDrtvZ8pF7iWgvLsn4MK3HPuYFFcTrxW";
                    break;
                case ("hush"):
                    Donation = "t1fXWt35Dnyxp5a2ouv4GqRCeucFvMY7iX7";
                    break;
                case ("eth"):
                    Donation = "0x638bd078ab70462332e3bfc2119449160db463d8";
                    break;
                case ("etc"):
                    Donation = "0x8c82018d02e7a3151ea2a1050ffb26c4ef81aa42";
                    break;
                case ("exp"):
                    Donation = "0x8aab3dd1792927320d564dfd1c20668ddca2b3f9";
                    break;
                case ("grs"):
                    Donation = "FWyajyaby59Q7fy99aGQBsdeegsNTCtvVr";
                    break;
                case ("xmr"):
                    Donation = "463tWEBn5XZJSxLU6uLQnQ2iY9xuNcDbjLSjkn3XAXHCbLrTTErJrBWYgHJQyrCwkNgYvyV3z8zctJLPCZy24jvb3NiTcTJ.3155888990414cf8b47e1a3d559143c4082d18f482d343fc86c804af3c087ce9";
                    break;
                case ("sia"):
                    Donation = "bdf99eaf6817877728ec5bd1e51a1a6e4e69b79f8806c86d78b49a398e3593770f2e38ae4e77";
                    break;
                case ("pasc"):
                    Donation = "86646-64.344245f3b021f30e";
                    break;
                case ("pasl"):
                    Donation = "5-70.99903b4db93f6f807e1a9b7e12fbd971951faeae5b58a7f9e2ef8fa0220a9e13";
                    break;                    
                case ("cxt"):
                    Donation = "CUhbmqYf2mMw8RGN17fX7VMtgiVnvaXinh";
                    break;
                case ("wyv"):
                    Donation = "WWm9qhF2ebuJgww61NGAPPbpFPRZz4VHQB";
                    break;
                case ("netko"):
                    Donation = "NQ5A7hEM5hst7rb4jve3AmHYrp5qFynphG";
                    break;
                case ("btx"):
                    Donation = "1GvRaVBMxZPyCcdag9zrafCYhYNYDrM2fm";
                    break;                    
                case ("btc"):
                    Donation = "1NoCAhu4dYxi162srrKzi5qZiQrERuu7A4";
                    break;

                default:
                    Donation = _account;
                    break;
            }
            return Donation;
        }

        #endregion
        public void UpdateBalance()
        {
            totalBalance = 0;
            totalPending = 0;

                foreach (TEntry entry in PriceEntries)
                {
                    totalBalance += entry.BalanceBTC;
                    totalPending += entry.Pending * entry.ExRate;
                }
                ServiceBalance = totalBalance;
                ServicePending = totalPending;

            ClearStalePrices();
            ClearWorkerSpeed();
        }

        public void UpdateHistory(bool error = false)
        {
            ServiceHistory serviceHistory = ServiceHistory;
            if (serviceHistory == null) return;

            IList<TEntry> priceEntries = PriceEntries;
            foreach (TEntry entry in priceEntries)
            {
                if (error) entry.Price = 0;
                serviceHistory.UpdatePrice(entry);
            }
        }

        public void ReadData(IDictionary<string, object> data)
        {
            ExtractCommon(data);

           
            object[] items = data["algos"] as object[];
            foreach (object rawitem in items)
            {
                Dictionary<string, object> item = rawitem as Dictionary<string, object>;
                PriceEntryBase entry = CreateEntry(item);

                if (!MiningEngine._showinactive && !entry.Enabled) continue;
                else
                    MiningEngine.PriceEntries.Add(entry);
            }
        }

        public void WTMCheck()
        {
            WebUtil2.DownloadJson(MiningEngine.WTMUrl, WTMPrices);
        }

        public void WTMCheckSolo(string PriceId)
        {
            WebUtil2.DownloadJson(MiningEngine.WTMUrl.Replace (".json", "/"+ PriceId + ".json"), WTMPricesSolo);
        }

        private void WTMPrices(object RawPrices)
        {
            if (RawPrices != null)
            {
                JObject data = (JObject)RawPrices;
                JToken coins = data["coins"];

                foreach (JProperty coin in coins.Children())
                {
                    JToken item = coin.Value;
                    string cname = coin.Name;
                    WTMPrice(cname, item);
                }
            }
        }

        private void WTMPricesSolo(object RawPrices)
        {
            if (RawPrices != null)
            {
                JToken item = (JObject)RawPrices;
                string cname = item["name"].ToString();
                WTMPrice(cname, item);
            }
        }

        private void WTMPrice(string cname, JToken item)
        {
            float bt = (float)item["block_time"];            
            float nh = (float)item["nethash"];
            float br = (float)item["block_reward"];
            float br24 = (float)item["block_reward24"];            
            float er = (float)item["exchange_rate"];            
            float er24 = (float)item["exchange_rate24"];

            float price = 86400 / bt * br / nh * er;
            float price24 = 86400 / bt * br24 / nh * er24;

            bool _lagstatus = bool.Parse(item["lagging"].ToString());

            GetPrices(price, er, price24, er24, cname, _lagstatus);            
        }

        public void WTMUpdate()
        {
            if (MiningEngine.WTMArray[0, 0] != null)
            {
                for (int i = 0; i < MiningEngine.WTMArray.Length / 6; i++)
                {
                    float price = 0; float er = 0;
                    float price24 = 0; float er24 = 0;

                    price = MiningEngine.WTMArray[i, 1];
                    price24 = MiningEngine.WTMArray[i, 2];
                    er = MiningEngine.WTMArray[i, 3];
                    er24 = MiningEngine.WTMArray[i, 4];
                    bool _lagstatus = MiningEngine.WTMArray[i, 5];

                    var cname = MiningEngine.WTMArray[i, 0];

                    GetPrices(price, er, price24, er24, cname, _lagstatus);
                }
            }
            else
                WTMCheck();

            foreach (PriceEntryBase entry in PriceEntries)
            {
                if (!string.IsNullOrWhiteSpace(entry.PriceId) && entry.WTMListed == false && _extracoins == true)
                    WTMCheckSolo(entry.PriceId);
            }
        }
        public void GetPrices (float price, float er, float price24, float er24, string cname, bool _lagstatus)
        {
            float ep = 0; float ex = 0;

            switch (_pricemode)
            {
                case 1:
                    ep = price24; ex = er24;
                    break;
                default:
                    ep = price; ex = er;
                    break;
            }            

            PriceEntryBase entry = GetEntryCoin(cname.ToString().ToLower());

            if (entry == null)
                entry = GetEntryCoin(WTMTranslation(cname.ToLower()).ToString().ToLower());

            if (entry != null)
            {
                lock (MiningEngine)
                {
                    {
                        entry.Price = (decimal)ep * 1000000000;
                        entry.ExRate = (decimal)ex;

                        
                        PriceDynamics(price, er, price24, er24, entry);
                        //AveragePrice(entry);

                        entry.Lagging = _lagstatus;

                        if (MiningEngine._pumpreduction == true && entry.Pumping == true )
                        {
                            entry.Price = entry.Price / ((decimal)(entry.AvgExRate / er24) - MiningEngine._dynoffset / 100);
                            entry.PumpReduction = true;
                        }
                        else entry.PumpReduction = false;

                        entry.DynUpdated = DateTime.Now;
                    }

                    MiningEngine.PricesUpdated = true;
                    MiningEngine.HasPrices = true;

                    LastUpdated = DateTime.Now;

                    UpdateHistory();
                }
            }
        }

        private string WTMTranslation(string cname)
        {
            if (cname.ToLower().StartsWith("dgb-"))
                cname = cname.Replace("dgb", "digibyte");

            if (cname.ToLower().StartsWith("myriad-"))
                cname = cname.Replace("myriad", "myriadcoin");

            if (cname.ToLower().StartsWith("myr-"))
                cname = cname.Replace("myr", "myriad");

            if (cname.ToLower().StartsWith("quark"))
                cname = cname.Replace("quark", "quarkcoin");

            if (cname.ToLower().StartsWith("ethereumclassic"))
                cname = cname.Replace("ethereumclassic", "ethereum-classic");
            return cname;
        }

        public void  PriceDynamics (float price, float er, float price24, float er24, dynamic entry )
        {
            if (entry.DynUpdated.AddMinutes((double) MiningEngine._dyncheck) < DateTime.Now)
            {
                AverageExRate(entry);

                var erdyn = 0f; var prdyn = 0f;

                if (entry.AvgExRate >0 && er24>0 )  erdyn = entry.AvgExRate / (float) er24;
                if (price > 0 && price24 > 0) prdyn =  price / price24;

                if (erdyn > 0)
                {
                    if (erdyn.ExtractDecimal() < (1 - MiningEngine._dynoffset / 100)) entry.Dumping = true;
                    else entry.Dumping = false;
                    if (erdyn.ExtractDecimal() > (1 + MiningEngine._dynoffset / 100)) entry.Pumping = true;
                    else entry.Pumping = false;
                }

                string erdynprint = "(N/A)";
                if (erdyn >= 1) erdynprint = "(+" + ((erdyn - 1)).ToString("P0") + ")";
                else if (erdyn > 0) erdynprint = "(" + ((erdyn - 1)).ToString("P0") + ")";

                string prdynprint = "(N/A)";
                if (prdyn >= 1) prdynprint = "(+" + ((prdyn - 1)).ToString("P0") + ")";
                else if (prdyn > 0) prdynprint = "(" + ((prdyn - 1)).ToString("P0") + ")";

                entry.PriceDynamics = prdynprint + "/" + erdynprint;
            }
        }
       

        public void FeesUpdate(string url)
        {
            string ur=string.Empty;
            foreach (PriceEntryBase entry in PriceEntries)
            {
                if (!string.IsNullOrWhiteSpace(url))
                { ur = url;
                    if (!string.IsNullOrWhiteSpace(entry.Tag) && !string.IsNullOrWhiteSpace(entry.ApiKey))
                    {
                        if (ur.Contains("TAG")) ur = ur.Replace("TAG", entry.Tag);
                        if (ur.Contains("APIKEY")) ur = ur.Replace("APIKEY", entry.ApiKey);
                        if (ur.Contains("COINNAME")) ur = ur.Replace("COINNAME", entry.CoinName);
                        if (ur.Contains("USERID")) ur = ur.Replace("USERID", entry.UserId);

                        WebUtil2.DownloadJson(ur.ToLower(), MPOSFee);
                    }
                }
            }
        }

        public void MPOSDataUpdate(string urlspeed, string urlbalance, string urlworker)
        {
            totalBalance = 0;
            totalPending = 0;

            foreach (PriceEntryBase entry in PriceEntries)
            {
                string urs = urlspeed;
                string urb = urlbalance;
                string urw = urlworker;

                if (!string.IsNullOrWhiteSpace(entry.ApiKey) && !string.IsNullOrWhiteSpace(entry.UserId)
                    && !string.IsNullOrWhiteSpace(entry.Tag) && !string.IsNullOrWhiteSpace(entry.CoinName))
                {
                    urs = urs.Replace("TAG", entry.Tag); urs = urs.Replace("APIKEY", entry.ApiKey); urs = urs.Replace("COINNAME", entry.CoinName);
                    urw = urw.Replace("TAG", entry.Tag); urw = urw.Replace("APIKEY", entry.ApiKey); urw = urw.Replace("USERID", entry.UserId); urw = urw.Replace("COINNAME", entry.CoinName);
                    urb = urb.Replace("TAG", entry.Tag); urb = urb.Replace("APIKEY", entry.ApiKey); urb = urb.Replace("USERID", entry.UserId); urb = urb.Replace("COINNAME", entry.CoinName);

                    if (_nospeed == false && !string.IsNullOrWhiteSpace(urs))
                        WebUtil2.DownloadJson(urs, MPOSSpeed);

                    if (_nobalance == false && !string.IsNullOrWhiteSpace(urb))
                        WebUtil5.DownloadJson(entry.CoinName, urb, MPOSBalances);

                    if (_nospeedworker == false && !string.IsNullOrWhiteSpace(urw))
                        WebUtil5.DownloadJson(entry.CoinName, urw, MPOSSpeedWorker);
                }                
            }
        }

        public void APIDataUpdate(string urldata)
        {
            totalBalance = 0;
            totalPending = 0;

            foreach (PriceEntryBase entry in PriceEntries)
            {
                string urd = urldata;

                if (!string.IsNullOrWhiteSpace(entry.ApiKey) && !string.IsNullOrWhiteSpace(entry.Tag) && !string.IsNullOrWhiteSpace(entry.CoinName))
                {
                    urd = urd.Replace("TAG", entry.Tag); urd = urd.Replace("APIKEY", entry.ApiKey); urd = urd.Replace("COINNAME", entry.CoinName);

                    if (ServiceName == "TheBlockFactory" && entry.Tag.ToLower() == "dgb") urd=urd.Replace(entry.Tag, entry.CoinName);                      

                    if (!string.IsNullOrWhiteSpace(urd))  WebUtil4.DownloadJson(entry.CoinName, urd, APIProcessData);
                }
            }
        }

        private void MPOSFee(object RawData)
        {
            if (RawData != null)
            {
                JObject data = (JObject)RawData;
                JToken gpi = data["getpoolinfo"];
                JToken dat = gpi["data"];
                JToken tag = dat["currency"];
                JToken fee = (float)dat["fees"];

                PriceEntryBase entry = GetEntryTag(tag.ToString().ToLower());
                if (entry != null )
                    entry.FeePercent = 1-(1-(decimal)fee/100)*(1-_BtcFee/100);
            }
        }

        private void MPOSBalances(object RawData)
        {
            if (RawData != null)
            {
                JObject data = (JObject)RawData;
                JToken gub = data["getuserbalance"];
                JToken bal = gub["data"];
                float confirmed = (float)bal["confirmed"];
                float unpaid = (float)bal["unconfirmed"];
                JToken status = data["getuserbalance"]["version"];

                PriceEntryBase entry = GetEntryCoin(status.ToString().ToLower());
                if (entry != null)
                {
                    entry.Balance = confirmed.ExtractDecimal();
                    entry.Pending = unpaid.ExtractDecimal();
                    entry.BalanceBTC = confirmed.ExtractDecimal() * entry.ExRate;
                }
            }
        }

        private void MPOSSpeed(object RawData)
        {
            if (RawData != null)
            {
                var hashrate = 0f;
                var rejected = 0f;
                JObject data = (JObject)RawData;
                JToken db = data["getdashboarddata"];
                JToken shares = db["data"]["personal"]["shares"];
                JToken ExRate = db["data"]["pool"]["price"];
                JToken tag = db["data"]["pool"]["info"]["currency"];
                hashrate = (float)db["data"]["personal"]["hashrate"];
                rejected = hashrate * (float)shares["invalid_percent"] / 100;

                PriceEntryBase entry = GetEntryTag(tag.ToString());
                if (entry != null && hashrate > 0)
                {
                    float m = 1000;

                    switch (ServiceName)
                    {
                        case "Suprnova":
                            if (entry.AlgoName.ToLower().StartsWith("cryptonight") ||
                            entry.AlgoName.ToLower().StartsWith("lbry")) m = m / 1000;
                            if (entry.AlgoName.ToLower().StartsWith("equihash")) m = m / 1000000;
                            break;

                        case "Dwarfpool":
                            if (entry.AlgoName.ToLower().StartsWith("equihash")) m = m / 1000;
                            break;

                        case "MiningPoolHub":
                            if (entry.AlgoName.ToLower().StartsWith("blake")
                                || entry.AlgoName.ToLower().StartsWith("decred")) m = m * 1000;
                            if (entry.AlgoName.ToLower().StartsWith("equihash")) m = m / 1000000;
                            if (entry.AlgoName.ToLower().StartsWith("cryptonight")) m = m / 1000;
                            break;

                        default:
                            if (entry.AlgoName.ToLower().StartsWith("decred") ||
                                entry.AlgoName.ToLower().StartsWith("lbry")) m = m * 1000;
                            if (entry.AlgoName.ToLower().StartsWith("equihash")) m = m / 1000000;
                            break;
                    }

                    entry.AcceptSpeed = hashrate.ExtractDecimal() * m.ExtractDecimal();
                    entry.RejectSpeed = rejected.ExtractDecimal() * m.ExtractDecimal();
                }
            }
        }

        private void MPOSSpeedWorker(object RawData)
        {
            if (RawData != null)
            {
                JObject data = (JObject)RawData;
                JToken db = data["getuserworkers"];
                JToken value = db["data"];
                JToken status = data["getuserworkers"]["version"];

                PriceEntryBase entry = GetEntryCoin(status.ToString());
                if (entry != null)
                {
                    foreach (var item in value.Children())
                    {
                        var w = item["username"].ToString();
                        var s = (float)item["hashrate"];

                        float m = 1;

                        if (entry.AlgoName.ToLower().StartsWith("equihash")) s = s / 1000000;

                        switch (ServiceName)
                        {
                            case "CoinMine":
                                if (entry.AlgoName.ToLower().StartsWith("equihash")) m = m * 1000;
                                break;
                            case "Dwarfpool":
                                if (entry.AlgoName.ToLower().StartsWith("equihash")) m = m / 1000;
                                break;
                            case "Suprnova":
                                if (entry.Tag.ToLower().StartsWith("zec")) m = m * 1000;
                                break;
                            case "AikaPool":
                                if (entry.Tag.ToLower().StartsWith("kmd") ||
                                    entry.Tag.ToLower().StartsWith("zdash")) m = m / 1000;
                                break;
                            case "MiningPoolHub":
                                if (entry.AlgoName.ToLower().StartsWith("blake")
                                    || entry.AlgoName.ToLower().StartsWith("decred")) m = m * 1000000;
                                if (entry.AlgoName.ToLower().StartsWith("equihash")) m = m * 1000;
                                break;

                            default:
                                if (entry.AlgoName.ToLower().StartsWith("equihash")) m = m * 1000;
                                break;
                        }

                        if (w.ToString().ToLower() == _account.ToString().ToLower() + "." + _worker.ToString().ToLower() && s > 0)
                        {                            
                            entry.AcSpWrk = s.ExtractDecimal() * m.ExtractDecimal();
                            AverageSpeed(entry);
                        }
                    }
                }
            }
        }

        private void APIProcessData(object RawData)
        {
            if (RawData != null)
            {
                float bal = 0; float pend = 0; float hashrate = 0;

                JObject data = (JObject)RawData;
                bal = (float)data["confirmed_rewards"];
                pend = (float)data["round_estimate"];
                string cname = data["username"].ToString();
                hashrate = (float)data["total_hashrate"];
                JToken workers = data["workers"];

                PriceEntryBase entry = GetEntryCoin(cname.ToString());
                if (entry != null)
                {
                    entry.Balance = bal.ExtractDecimal();
                    entry.Pending = pend.ExtractDecimal();
                    entry.BalanceBTC = entry.Balance * entry.ExRate;

                    if (_nospeed == false)
                        entry.AcceptSpeed = hashrate.ExtractDecimal();

                    if (_nospeedworker == false && workers != null)
                    {
                        foreach (JProperty item in workers.Children())
                        {
                            var s = (float)item.Value["hashrate"];
                            if (item.Name.ToString().ToLower() == _account.ToString().ToLower() + "." + _worker.ToString().ToLower() && s > 0)
                            {
                                entry.AcSpWrk = s.ExtractDecimal();

                                AverageSpeed(entry);
                            }
                        }
                    }
                }
            }
        }

        public void AverageSpeed(dynamic entry)
        {
            int k = 0;

            while (entry.SpWrkLog[k, 0] != null)
            {
                k++;
            }

            if (k == MiningEngine._AvSpTicks)   k = 0;

            if (entry.AcSpWrk > 0)
            {
                entry.SpWrkLog[k, 0] = DateTime.Now;
                entry.SpWrkLog[k, 1] = entry.AcSpWrk;
                if (entry.SpWrkLog[k + 1, 0] != null) entry.SpWrkLog[(int)MiningEngine._AvSpTicks, 0] = entry.SpWrkLog[k + 1, 0];
                if (entry.SpWrkLog[k + 1, 1] != null) entry.SpWrkLog[(int)MiningEngine._AvSpTicks, 1] = entry.SpWrkLog[k + 1, 1];
                entry.SpWrkLog[k + 1, 0] = null;
                entry.SpWrkLog[k + 1, 1] = null;
            }

            decimal AvgSp = 0; int c = 0;

             while (entry.SpWrkLog[c, 0] > DateTime.Now.AddMinutes(-MiningEngine._AvSpTicks))
                {
                    AvgSp += entry.SpWrkLog[c, 1];
                    ++c;
                }            

            if (c>0 && AvgSp / c > entry.TopAvgSp) entry.TopAvgSp = AvgSp / c;
        }

        public void AveragePrice(dynamic entry)
        {
            int k = 0;

            while (entry.PriceLog [k, 0] != null)
            {
                k++;
            }

            if (k == MiningEngine.statWindow) k = 0;

            if (entry.Price > 0)
            {
                entry.PriceLog[k, 0] = DateTime.Now;
                entry.PriceLog[k, 1] = entry.NetEarn;
                if (entry.PriceLog[k + 1, 0] != null) entry.PriceLog[(int)MiningEngine.statWindow, 0] = entry.PriceLog[k + 1, 0];
                if (entry.PriceLog[k + 1, 1] != null) entry.PriceLog[(int)MiningEngine.statWindow, 1] = entry.PriceLog[k + 1, 1];
                entry.PriceLog[k + 1, 0] = null;
                entry.PriceLog[k + 1, 1] = null;
            }

            decimal AvgPrice = 0; int c = 0;

            while (entry.PriceLog[c, 0] > DateTime.Now.AddMinutes(-MiningEngine.statWindow))
            {
                AvgPrice += entry.PriceLog[c, 1];
                ++c;
            }

            if (c > 0) entry.NetAverage = AvgPrice / c;
        }

        public void AverageExRate  (dynamic entry)
        {
            int k = 0;

            while (entry.ExRateLog [k, 0] != null)
            {
                k++;
            }

            if (k == MiningEngine.statWindow)     k = 0;

            if (entry.ExRate > 0)
            {
                entry.ExRateLog[k, 0] = DateTime.Now;
                entry.ExRateLog[k, 1] = entry.ExRate;
                if (entry.ExRateLog[k + 1, 0]!=null) entry.ExRateLog[(int)MiningEngine.statWindow, 0] = entry.ExRateLog[k + 1, 0];
                if (entry.ExRateLog[k + 1, 1] != null) entry.ExRateLog[(int)MiningEngine.statWindow, 1] = entry.ExRateLog[k + 1, 1];
                entry.ExRateLog[k+1, 0] = null;
                entry.ExRateLog[k+1, 1] = null;
            }

            float AvgExRate = 0; int c = 0;

            while (entry.ExRateLog[c, 0] > DateTime.Now.AddMinutes(-MiningEngine.statWindow))
            {
                AvgExRate += (float) entry.ExRateLog[c, 1];
                ++c;
            }

            if (c > 0) entry.AvgExRate = AvgExRate / c;
        }
    }
}