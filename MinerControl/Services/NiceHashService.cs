using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace MinerControl.Services
{
    public class NiceHashService : ServiceBase<NiceHashPriceEntry>
    {
        private readonly IDictionary<string, int> _algoTranslation = new Dictionary<string, int>
        {
            {"scrypt", 0},
            {"sha256", 1},
            {"scryptn", 2},
            {"x11", 3},
            {"x13", 4},
            {"keccak", 5},
            {"x15", 6},
            {"nist5", 7},
            {"neoscrypt", 8},
            {"lyra2", 9},
            {"whirlpoolx", 10},
            {"qubit", 11},
            {"quark", 12},
            {"axiom", 13},
            {"lyra2v2", 14},
            {"scryptjanenf16", 15},
            {"blake256r8", 16},
            {"blake256r14", 17},
            {"blake256r8vnl", 18},
            {"hodl", 19},
            {"daggerhashimoto", 20},
            {"decred", 21},
            {"cryptonight", 22},
            {"lbry", 23},
            {"equihash", 24},
            {"equihashNH", 24},
            {"pascal", 25},
            {"x11gost", 26}
        };

        private Dictionary<string, double> _pingTimes;
        public bool DetectLocation;
        private decimal AcSpWrk;

        public NiceHashService()
        {
            ServiceName = "NiceHash";
            DonationAccount = "1PMj3nrVq5CH4TXdJSnHHLPdvcXinjG72y";
            DonationWorker = "1";
        }
        public override void Initialize(IDictionary<string, object> data)
        {
            ExtractCommon(data);

            if ((data.ContainsKey("detectlocation") && (bool)data["detectlocation"]))
            {
                DetectLocation = true;
                InitPingTimes(); // Quick sync test
                CheckPingTimes(); // Thorough async test
            }
            else
            {
                DetectLocation = false;
            }
                       
            object[] items = data["algos"] as object[];
            foreach (object rawitem in items)
            {
                Dictionary<string, object> item = rawitem as Dictionary<string, object>;
                NiceHashPriceEntry entry = CreateEntry(item);
                if (string.IsNullOrWhiteSpace(entry.PriceId))
                    entry.PriceId = GetAgorithmId(entry.AlgoName).ToString();

                if (!MiningEngine._showinactive && !entry.Enabled) continue;
                else
                    MiningEngine.PriceEntries.Add(entry);
            }
        }

        public override void CheckFees()
        {
        }
        public override void CheckPrices()
        {
            WebUtil2.DownloadJson("https://www.nicehash.com/api?method=stats.global.current", ProcessCurrentPrices);
            WebUtil2.DownloadJson("https://www.nicehash.com/api?method=stats.global.24h", ProcessDailyPrices);
        }

        public override void CheckData()
        {
            if (_nobalance == false)
                WebUtil2.DownloadJson("https://www.nicehash.com/api?method=stats.provider&addr=" + _account, ProcessBalances);
        }

        private void ProcessCurrentPrices(object RawPrices)
        {
            int mode = 0;
            ProcessPrices(RawPrices, mode);
        }

        private void ProcessDailyPrices(object RawPrices)
        {
            int mode = 1;
            ProcessPrices(RawPrices, mode);
        }
        private void ProcessPrices(object RawPrices, int mode)
        {
            if (RawPrices != null)
            {
                JObject data = (JObject)RawPrices;
                JToken result = data["result"];
                JToken stats = result["stats"];

                lock (MiningEngine)
                {
                    foreach (JToken stat in stats.Children())
                    {
                        JToken item = stat;
                        string algo = item["algo"].ToString();
                        NiceHashPriceEntry entry = GetEntry(algo);
                        if (entry == null) continue;
                        float  m = 1; float price24 = 0;

                        switch (entry.Name.ToLower())
                        {
                            case "sha256":
                                m = 0.001f; // SHA256 listed in TH/s
                                break;
                            case "pascal":
                                m = 0.001f; // Pascal listed in GH/s
                                break;
                            case "equihash":
                                m = 1 * 1000; // ZEC listed in H/s
                                break;
                            case "cryptonight":
                                m = 1 * 1000; // CN listed in H/s
                                break;
                            case "blake256r8vnl":
                                m = 0.001f; // Blake listed in MH/s
                                break;
                            case "blake256r8":
                                m = 0.001f; // Blake listed in MH/s
                                break;
                            case "lbry":
                                m = 0.001f; // LBRY listed in MH/s
                                break;
                            case "decred":
                                m = 0.001f; // Decred listed in MH/s
                                break;
                            default:
                                m = 1; // All others in kH/s
                                break;
                        }

                        if (mode == 0)
                        {
                            entry.Price = (m.ExtractDecimal() * item["price"].ExtractDecimal());
                            //AveragePrice(entry);
                        }
                            if (mode == 1)
                        {
                            price24 = (float)(m.ExtractDecimal() * item["price"].ExtractDecimal());                           
                            PriceDynamics((float)entry.Price, 0, price24, 0, entry);
                        }
                    }

                    MiningEngine.PricesUpdated = true;
                    MiningEngine.HasPrices = true;

                    LastUpdated = DateTime.Now;

                    UpdateHistory();
                }
            }
        }

        private void ProcessBalances(object RawBalance)
        {
            //decimal _totalBalance = 0m;

            if (RawBalance != null)
            {
                JObject data = (JObject)RawBalance;
                JToken result = data["result"];
                JToken stats = result["stats"];
                foreach (JToken item in stats.Children())
                {
                    //_totalBalance += item["balance"].ExtractDecimal();
                    string algo = item["algo"].ToString();

                    NiceHashPriceEntry entry = GetEntry(algo);
                    if (entry == null) continue;

                    entry.BalanceBTC = item["balance"].ExtractDecimal();

                    switch (entry.AlgoName)
                    {
                        case "sha256":
                            entry.AcceptSpeed = item["accepted_speed"].ExtractDecimal();
                            entry.RejectSpeed = item["rejected_speed"].ExtractDecimal();
                            break;
                        default:
                            entry.AcceptSpeed = item["accepted_speed"].ExtractDecimal() * 1000000;
                            entry.RejectSpeed = item["rejected_speed"].ExtractDecimal() * 1000000;
                            break;
                    }

                    if (_nospeed == false)
                        WebUtil2.DownloadJson("https://www.nicehash.com/api?method=stats.provider.workers&addr=" + _account + "&algo=" + entry.PriceId, ProcessSpeed);
                }

                //ServiceBalance = _totalBalance;
            }
        }

        public void ProcessSpeed(object RawSpeed)
        {
            if (RawSpeed != null)
            {

                JObject data = (JObject)RawSpeed;

                JToken result = data["result"];
                JToken workers = result["workers"];
                string algonum = result["algo"].ToString();

                NiceHashPriceEntry entry = GetEntry(algonum);
                if (entry != null)
                {
                    entry.AcSpWrk = 0;
                    entry.AcceptSpeed = 0;
                    string algo = entry.Name.ToString();

                    foreach (var item in workers.Children())
                    {
                        AcSpWrk = 0;
                        var w = item[0];
                        var n = item[1];
                        var s = n["a"];

                        switch (algo.ToLower())
                        {
                            case "equihash":
                                AcSpWrk = s.ExtractDecimal() / 1000;
                                break;
                            case "cryptonight":
                                AcSpWrk = s.ExtractDecimal();
                                break;
                            case "blake256r8vnl":
                                AcSpWrk = s.ExtractDecimal() * 1000000;
                                break;
                            case "blake256r8":
                                AcSpWrk = s.ExtractDecimal() * 1000000;
                                break;
                            case "decred":
                                AcSpWrk = s.ExtractDecimal() * 1000;
                                break;
                            case "pascal":
                                AcSpWrk = s.ExtractDecimal() * 1000;
                                break;
                            case "lbry":
                                AcSpWrk = s.ExtractDecimal() * 1000000;
                                break;

                            default:
                                AcSpWrk = s.ExtractDecimal() * 1000;
                                break;
                        }

                        if (w.ToString().ToLower() == _worker.ToString().ToLower() && AcSpWrk>0)
                        {                            
                            entry.AcSpWrk = AcSpWrk.ExtractDecimal();
                            AverageSpeed(entry);
                        }
                        entry.AcceptSpeed += AcSpWrk;
                    }
                }
            }
        }

        private int GetAgorithmId(string algorithmName)
        {
            return _algoTranslation[algorithmName];
        }

        private void InitPingTimes()
        {
            const int tries = 3;

            _pingTimes = new Dictionary<string, double>(4)
                {
                    {".eu.nicehash.com", 0},
                    {".usa.nicehash.com", 0},
                    {".hk.nicehash.com", 0},
                    {".jp.nicehash.com", 0}
                };

            Dictionary<string, double> clone = new Dictionary<string, double>(4);

            foreach (string url in _pingTimes.Keys)
            {
                Ping pinger = new Ping();
                double roundTripTime = 0;

                for (int i = 0; i < tries; i++)
                {
                    try
                    {
                        PingReply reply = pinger.Send("speedtest" + url, 500);
                        roundTripTime = CheckRoundTripTime(reply, url, roundTripTime);
                    }
                    catch
                    {
                        roundTripTime += 1000;
                    }
                }

                clone.Add(url, roundTripTime / tries);
            }

            _pingTimes = clone;
        }

        public async void CheckPingTimes()
        {
            const int tries = 50;
            Dictionary<string, double> clone = new Dictionary<string, double>(4);
            foreach (string url in _pingTimes.Keys)
            {
                Ping pinger = new Ping();
                double roundTripTime = 0;

                for (int i = 0; i < tries; i++)
                {
                    try
                    {
                        PingReply reply = await pinger.SendPingAsync("speedtest" + url, 1000);
                        roundTripTime = CheckRoundTripTime(reply, url, roundTripTime);
                    }
                    catch
                    {
                        roundTripTime += 1000;
                    }
                }

                clone.Add(url, roundTripTime / tries);
            }

            _pingTimes = clone;
        }

        private static double CheckRoundTripTime(PingReply reply, string url, double roundTripTime)
        {
            if (reply != null && reply.Status == IPStatus.Success)
            {
                switch (url)
                {
                    case ".hk.nicehash.com":
                        roundTripTime += 150 + reply.RoundtripTime;
                        break;
                    case ".jp.nicehash.com":
                        roundTripTime += 100 + reply.RoundtripTime;
                        break;
                    default:
                        roundTripTime += reply.RoundtripTime;
                        break;
                }
            }
            else
            {
                roundTripTime += 1000;
            }

            return roundTripTime;
        }

        public string GetBestLocation(string algorithmName)
        {
            string substituteAlgo;
            switch (algorithmName)
            {
                case "lyra2":
                    substituteAlgo = "lyra2re";
                    break;
                case "lyra2v2":
                    substituteAlgo = "lyra2rev2";
                    break;
                case "scryptn":
                    substituteAlgo = "scryptnf";
                    break;
                default:
                    substituteAlgo = algorithmName;
                    break;
            }

            int port = 3333 + _algoTranslation[algorithmName]; // 3333 + priceid = port
                                                               // return substituteAlgo + _pingTimes.OrderBy(ping => ping.Value).First().Key + ":" + port;
            return _pingTimes.OrderBy(ping => ping.Value).First().Key;
        }
    }
}
