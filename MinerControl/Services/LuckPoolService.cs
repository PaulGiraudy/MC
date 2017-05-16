using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class LuckPoolService : ServiceBase<LuckPoolPriceEntry>
    {
        public LuckPoolService()
        {
            ServiceName = "LuckPool";
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
            WTMUpdate();
        }
        public override void CheckData()
        {
            WebUtil2.DownloadJson("http://luckpool.org/api/stats", ProcessStat);

            foreach (LuckPoolPriceEntry entry in PriceEntries)
            {
                if (!string.IsNullOrWhiteSpace(entry.Wallet))
                    WebUtil2.DownloadJson("http://luckpool.org/api/worker_stats?" + entry.Wallet, ProcessData);

            }
        }

        private void ProcessStat(object RawData)
        {
            if (RawData != null)
            {

            }
        }
        private void ProcessData(object RawData)
        {
            if (RawData != null)
            {
                string speed = null;
               
                JObject data = (JObject)RawData;

                JToken address = data["miner"];
                speed = data["totalHash"].ToString();
                var balance = data["balance"];
                JToken workers = data["workers"];

                foreach (LuckPoolPriceEntry entry in PriceEntries)
                {                   
                    if (entry.Wallet.ToLower().Contains(address.ToString().ToLower()) )
                    {
                        entry.Balance = (decimal) balance;
                        entry.BalanceBTC = entry.Balance * entry.ExRate;
                        entry.AcceptSpeed = speed.ExtractDecimal()/1000000000000000;

                        if (workers != null)
                        {
                            foreach (JProperty item in workers.Children())
                            {
                                string hashrate = item.Value["hashrate"].ToString();
                                if (item.Name.ToString().ToLower() == entry.Wallet.ToLower() + "." + _worker.ToLower() && !string.IsNullOrWhiteSpace(hashrate))
                                {  
                                    entry.AcSpWrk = hashrate.ExtractDecimal() / 1000000000000000;

                                    AverageSpeed(entry);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
