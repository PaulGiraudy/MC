using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class YAAMPCloneService : ServiceBase<YAAMPClonePriceEntry>
    {

        private string _url;

        public YAAMPCloneService(string pool)
        {
            ServiceName = pool;
            DonationAccount = "1PMj3nrVq5CH4TXdJSnHHLPdvcXinjG72y";
        }
        public override void Initialize(IDictionary<string, object> data)
        {
            if (data.ContainsKey("url")) _url = data.GetString("url");            
            else
                _url = "http://" + ServiceName;

            ReadData(data);
        }

        public override void CheckFees()
        {
        }
        public override void CheckPrices()
        {
            WebUtil2.DownloadJson(_url + "/api/status", ProcessPrices);
        }
        public override void CheckData()
        { 
        WebUtil2.DownloadJson(_url + "/api/walletEx?address=" + _account, Process);
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
                        float price = 0;
                        JToken item = key.Value;
                        string algo = item["name"].ToString();

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

                        YAAMPClonePriceEntry entry = GetEntryAlgo(algo);
                        if (entry == null) continue;

                        entry.Price = price.ExtractDecimal() * 1000;

                        if (entry.AlgoName.ToLower().StartsWith("decred") || entry.AlgoName.ToLower().StartsWith("blake"))
                            entry.Price = entry.Price.ExtractDecimal() / 1000;
                        if (entry.AlgoName.ToLower().StartsWith("equihash"))
                                    entry.Price = price.ExtractDecimal() * 1000000;
                        if (entry.AlgoName.ToLower().StartsWith("sha256"))
                            entry.Price = price.ExtractDecimal() / 1000;

                        var feePercent = (float)item["fees"];

                        entry.FeePercent = 1-(1-feePercent.ExtractDecimal()/100) *(1- _BtcFee/100);

                        entry.AcceptSpeed = 0; entry.RejectSpeed = 0; entry.AcSpWrk = 0;

                        //AveragePrice(entry);

                        PriceDynamics((float)item["estimate_current"], 0, (float)item["estimate_last24h"], 0, entry);

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
            if (RawBalance != null)
            {
                var bal = 0f;
                var pend = 0f;

                JObject data = (JObject)RawBalance;
                ServiceBalance = 0;
                ServicePending = 0;

                switch (_balancemode)
                {
                    case 1:
                        bal = (float)data["unpaid"];
                        break;
                    case 2:
                        bal = (float)data["unsold"];
                        break;
                    case 3:
                        bal = (float)data["paid"];
                        break;
                    case 4:
                        bal = (float)data["total"];
                        break;
                    default:
                        bal = (float)data["balance"];
                        pend = (float)data["unsold"];
                        break;
                }

                ServiceBalance = bal.ExtractDecimal();
                ServicePending = pend.ExtractDecimal();
            }
        }

        private void ProcessSpeed(object RawSpeed)
        {
            if (RawSpeed != null)
            {

                JObject data = (JObject)RawSpeed;
                JToken workers = data["miners"];

                if (workers != null)
                {
                    //var acs = 0f; var rej = 0f;

                    foreach (var item in workers.Children())
                    {
                        var AcSpWrk = 0f; var s = 0f; var r = 0f;
                        var w = item["password"].ToString();
                        string algo = item["algo"].ToString();
                        s = (float)item["accepted"]/1000;
                        r = (float)item["rejected"]/1000;
                        string wrk = _param2.Trim(new char[] { '-', ' ', 'p' });                        

                        YAAMPClonePriceEntry entry = GetEntryAlgo(algo);
                        if (entry == null) continue;

                        if (entry.AlgoName.ToLower().StartsWith("equihash"))
                        {
                            s = s / 1000; r = r / 1000;
                        }

                        AcSpWrk = s;

                        if (w.ToString().ToLower() == wrk.ToString().ToLower() && _nospeedworker == false && s>0)
                        {  
                            entry.AcSpWrk = s.ExtractDecimal();

                            AverageSpeed(entry);
                        }

                        if (algo == entry.AlgoName &&_nospeed == false)
                        {
                            entry.AcceptSpeed += s.ExtractDecimal();
                            entry.RejectSpeed += r.ExtractDecimal();
                        }

                        //entry.AcceptSpeed = acs.ExtractDecimal() / 1;
                        //entry.RejectSpeed = rej.ExtractDecimal() / 1;
                    }
                }
            }
        }
    }
}
    
