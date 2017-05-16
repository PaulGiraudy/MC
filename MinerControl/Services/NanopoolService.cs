using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class NanopoolService : ServiceBase<NanopoolPriceEntry>
    {
        public NanopoolService()
        {
            ServiceName = "Nanopool";
        }

        public override void Initialize(IDictionary<string, object> data)
        {
            ReadData(data);
        }


        public override void CheckFees()
        {
        }
        public override void CheckPrices()
        {          
            if (PriceWTM == true) WTMUpdate();
            else
            {
                foreach (NanopoolPriceEntry entry in PriceEntries)
                {
                    if (!string.IsNullOrWhiteSpace(entry.Tag) && !string.IsNullOrWhiteSpace(entry.CoinName))
                        WebUtil4.DownloadJson(entry.CoinName, "https://api.nanopool.org/v1/" + entry.Tag + "/approximated_earnings/1", ProcessPrices);
                }
            }
        }

        public override void CheckData()
        {
            foreach (NanopoolPriceEntry entry in PriceEntries)
            {
                if (!string.IsNullOrWhiteSpace(entry.Wallet) && !string.IsNullOrWhiteSpace(entry.Tag)  && !string.IsNullOrWhiteSpace(entry.CoinName))
                {
                    if (entry.Tag.ToLower() == "pasc")
                        entry.Wallet = entry.Wallet.Remove("-64");
                    WebUtil4.DownloadJson(entry.CoinName, "https://api.nanopool.org/v1/" + entry.Tag + "/prices", ProcessExRate);
                    WebUtil4.DownloadJson(entry.CoinName, "https://api.nanopool.org/v1/" + entry.Tag + "/user/" + entry.Wallet, Process);
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

                JToken value = data["data"];
                JToken status = data["status"];
                JToken min = value["minute"];
                float bitcoins = (float)min["bitcoins"];

                lock (MiningEngine)
                {
                    NanopoolPriceEntry entry = GetEntryCoin(status.ToString());
                    if (entry != null)
                    {
                        entry.Price = bitcoins.ExtractDecimal() * 60 * 24 * 1000;

                        if (entry.Tag.ToLower() == "zec" || entry.Tag.ToLower() == "xmr")
                            entry.Price *= 1000000;

                       // AveragePrice(entry);
                    }
                    MiningEngine.PricesUpdated = true;
                    MiningEngine.HasPrices = true;

                    LastUpdated = DateTime.Now;

                    UpdateHistory();
                }
            }
        }

        private void ProcessExRate(object RawPrices)
        {
            if (RawPrices != null)
            {
                JObject data = (JObject)RawPrices;

                JToken value = data["data"];
                JToken status = data["status"];
                float prbtc = (float)value["price_btc"];

                NanopoolPriceEntry entry = GetEntryCoin(status.ToString());
                if (entry != null)
                    entry.ExRate = prbtc.ExtractDecimal();
            }
        }
        private void ProcessBalances(object RawBalances)
        {
            var bal = 0f;
            var pend = 0f;

            if (RawBalances != null)
            {
                JObject data = (JObject)RawBalances;

                JToken value = data["data"];
                JToken status = data["status"];
                JToken balance = value["balance"];
                JToken pending = value["unconfirmed_balance"];
                
                NanopoolPriceEntry entry = GetEntryCoin(status.ToString());
                if (entry != null)
                {
                    bal = (float)balance;
                    pend = (float)pending;
                    entry.Balance = bal.ExtractDecimal();
                    entry.Pending = pend.ExtractDecimal();
                    entry.BalanceBTC = entry.Balance * entry.ExRate;
                    //totalPending += entry.Pending * entry.ExRate;
                    //totalBalance += entry.BalanceBTC;
                }
                //ServiceBalance = totalBalance;
                //ServicePending = totalPending;
            }
        }
        private void ProcessSpeed(object RawSpeed)
        {
            var hashrate = 0f;

            if (RawSpeed != null)
            {
                JObject data = (JObject)RawSpeed;
                JToken value = data["data"];
                JToken status = data["status"];
                hashrate = (float)value["hashrate"];

                NanopoolPriceEntry entry = GetEntryCoin(status.ToString());
                if (entry != null)
                {
                    double m = 1;
                    if (entry.Tag == "zec" || entry.Tag == "xmr") m = 0.001;  
                    if (entry.Tag == "sia")          m = 1000;                    

                    if (value["workers"].HasValues == true)
                    {
                        JToken workers = value["workers"];

                        foreach (var item in workers.Children())
                        {

                            var w = item["id"].ToString();
                            var s = (float)item["hashrate"];
                            
                            entry.AcceptSpeed = hashrate.ExtractDecimal()*m.ExtractDecimal();

                            if (w.ToString().ToLower() == _worker.ToString().ToLower() && s>0)
                            {
                                entry.AcSpWrk = s.ExtractDecimal()*m.ExtractDecimal();

                                AverageSpeed(entry);
                            }
                        }
                    }
                }
            }
        }
    }
}
