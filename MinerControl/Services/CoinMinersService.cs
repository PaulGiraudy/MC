using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MinerControl.Services
{
    public class CoinMinersService : ServiceBase<CoinMinersPriceEntry>
    {
        public CoinMinersService()
        {
            ServiceName = "CoinMiners";
        }
        public override void Initialize(IDictionary<string, object> data)
        {
            ReadData(data);
            foreach (CoinMinersPriceEntry entry in PriceEntries)
            {
                entry.CoinName = "NO SUCH COIN";
                entry.Banned = true;
            }
        }     


        public override void CheckFees()
        {
        }
        public override void CheckPrices()
        {            
            if (PriceWTM == true) WTMUpdate();
            else
            {
                WebUtil2.DownloadJson("http://pool.coin-miners.info/api/currencies", ProcessCurrencies);
                WebUtil2.DownloadJson("http://pool.coin-miners.info/api/status", ProcessPrices);
            }
        }

        public override void CheckData()
        {
            foreach (CoinMinersPriceEntry entry in PriceEntries)
            {
                if (!string.IsNullOrWhiteSpace(entry.Wallet))
                {                     
                    WebUtil2.DownloadJson("http://pool.coin-miners.info/api/walletEx?address=" + entry.Wallet, Process);
                }
            }
        }

        private void Process(object RawData)
        {
            if (_nobalance == false)
                ProcessBalances(RawData);

            if (_nospeed == false)
                ProcessSpeed(RawData);
        }

        private void ProcessPrices(object RawPrices)
        {
            if (RawPrices != null)
            {
                JObject data = (JObject)RawPrices;

                lock (MiningEngine)
                {
                    foreach (JProperty key in data.Children())
                    {
                        JToken item = key.Value;
                        string algo = item["name"].ToString();
                        float price = 0;

                        switch (_pricemode)
                        {
                            case 1:
                                price = (float)item["estimate_last24h"];
                                break;
                            case 2:
                                price = (float)item["actual_last24h"];
                                break;
                            default:
                                price = (float)item["estimate_current"];
                                break;
                        }
                        
                        foreach (CoinMinersPriceEntry entry in PriceEntries)
                        {
                            if (entry.AlgoName.ToLower() == algo.ToString().ToLower())
                            {
                                entry.Price = price.ExtractDecimal() * 1000;

                                if (entry.AlgoName.ToLower().StartsWith("blake") || entry.AlgoName.ToLower().StartsWith("decred"))
                                    entry.Price = price.ExtractDecimal();
                                if (entry.AlgoName.ToLower().StartsWith("equihash"))
                                    entry.Price = price.ExtractDecimal() * 1000000;
                                if (entry.AlgoName.ToLower().StartsWith("sha256"))
                                    entry.Price = price.ExtractDecimal() / 1000;

                                var feePercent = (float)item["fees"];
                                
                                entry.FeePercent = 1 - (1 - feePercent.ExtractDecimal() / 100) ;

                                //AveragePrice(entry);

                                PriceDynamics((float)item["estimate_current"], 0, (float)item["estimate_last24h"], 0, entry);
                            }
                        }
                    }

                    MiningEngine.PricesUpdated = true;
                    MiningEngine.HasPrices = true;

                    LastUpdated = DateTime.Now;

                    UpdateHistory();
                }
            }
        }

        private void ProcessCurrencies(object RawData)
        {
            if (RawData != null)
            {
                JObject data = (JObject)RawData;

                foreach (JProperty key in data.Children())
                {
                    JToken item = key.Value;

                    string algo = item["algo"].ToString();
                    string cname = item["name"].ToString();

                    decimal hashrate = item["hashrate"].ExtractDecimal();
                    int digitCount = 0;
                    if (hashrate>0)  digitCount = (int)Math.Log10((double)hashrate) + 1;
                    

                    CoinMinersPriceEntry entry = GetEntryTag(key.Name.ToString());
                    if (entry == null) continue;

                    entry.CoinName = cname.ToString();
                    entry.Banned = false;
                    entry.CoinName = entry.CoinName[0].ToString().ToUpper() + entry.CoinName.Substring(1, entry.CoinName.Length - 1);
                    string mod = (digitCount + hashrate.ToString().Substring(0,1));                    
                    entry.CWeight = 1 + mod.ExtractDecimal() / 1000000;
                }
            }
        }
        private void ProcessBalances(object RawData)
        {
            var bal = 0f;
            var pend = 0f;

            if (RawData != null)

            {
                JObject data = (JObject)RawData;
                var tag = data["currency"].ToString();

               if (!string.IsNullOrEmpty(data["unsold"].ToString())) pend = (float)data["unsold"];

                CoinMinersPriceEntry entry = GetEntryTag(tag);
                if (entry != null)
                {
                    switch (_pricemode)
                    {
                        case 1:
                            bal = (float)data["unpaid"];
                            break;
                        case 2:
                            bal = (float)data["paid"];
                            break;
                        case 3:
                            bal = (float)data["total"];
                            break;
                        default:
                            bal = (float)data["balance"];
                            break;
                    }
                    entry.Balance = bal.ExtractDecimal();
                    entry.Pending = pend.ExtractDecimal();
                }
            }
        }


        private void ProcessSpeed(object RawData)
        {
            if (RawData != null)
            {
                JObject data = (JObject)RawData;
                JToken workers = data["miners"];

                if (workers != null)
                {
                    var tag = data["currency"].ToString();

                    CoinMinersPriceEntry entry = GetEntryTag(tag);
                    if (entry != null)

                    {
                         var acs = 0f; var rej = 0f;

                        foreach (var item in workers.Children())
                        {
                            var w = item["password"].ToString();
                            string algo = item["algo"].ToString();
                            var s = (float)item["accepted"]/1000;
                            var r = (float)item["rejected"]/1000;
                            string wrk = _param2.Trim(new char[] { '-', ' ', 'p' });

                            if (entry.AlgoName.ToLower().StartsWith("equihash"))
                            {
                                s = s / 1000; r = r / 1000;
                            }

                            if (w.ToString().ToLower() == wrk.ToString().ToLower() && s>0)
                            {   
                                entry.AcSpWrk = s.ExtractDecimal();

                                AverageSpeed(entry);
                            }

                            if (algo == entry.AlgoName)
                            {
                                acs += s;
                                rej += r;
                            }
                        }
                        
                        entry.AcceptSpeed = acs.ExtractDecimal() / 1;
                        entry.RejectSpeed = rej.ExtractDecimal() / 1;

                    }
                }
            }
        }
    }
}



