using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class EthermineService : ServiceBase<EtherminePriceEntry>
    {
        public EthermineService()
        {
            ServiceName = "Ethermine";
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
            foreach (EtherminePriceEntry entry in PriceEntries)
            {
               if (!string.IsNullOrWhiteSpace(entry.Wallet))
                {
                    if (entry.CoinName.ToLower() == "ethereum")
                        WebUtil2.DownloadJson("https://ethermine.org/api/miner_new/" + entry.Wallet, ProcessData);
                    if (entry.CoinName.ToLower() == "ethereumclassic")
                        WebUtil2.DownloadJson("https://etc.ethermine.org/api/miner_new/" + entry.Wallet, ProcessData);
                    if (entry.CoinName.ToLower() == "zcash")
                        WebUtil2.DownloadJson("http://zcash.flypool.org/api/miner_new/" + entry.Wallet, ProcessData);
                }
            }
        }
      
        private void ProcessData(object RawData)
        {
            if (RawData != null)
            {
                string speed = null;
               
                JObject data = (JObject)RawData;

                JToken address = data["address"];
                speed = data["reportedHashRate"].ToString();
                var unpaid = data["unpaid"];
                JToken workers = data["workers"];

                foreach (EtherminePriceEntry entry in PriceEntries)
                {                   
                    if (entry.Wallet.ToLower().Contains(address.ToString().ToLower()) )
                    {
                        entry.Balance = (decimal)unpaid;
                        if (entry.CoinName.ToLower() == "zcash") entry.Balance /= 100000000;
                        if (entry.CoinName.ToLower() == "ethereum") entry.Balance /= 1000000000000000000;
                        if (entry.CoinName.ToLower() == "ethereumclassic") entry.Balance /= 1000000000000000000;

                        entry.BalanceBTC = entry.Balance * entry.ExRate;
                        //totalBalance += entry.BalanceBTC;
                        
                        if (speed.EndsWith("H/s"))
                            entry.AcceptSpeed = speed.Replace("H/s","").ExtractDecimal()/1000;
                        if (speed.EndsWith("MH/s"))
                            entry.AcceptSpeed = speed.Replace("MH/s", "").ExtractDecimal()*1000;
                        if (speed.EndsWith("kH/s"))
                            entry.AcceptSpeed = speed.Replace("kH/s", "").ExtractDecimal();

                        if (workers != null)
                        {
                            foreach (JProperty item in workers.Children())
                            {
                                var AcSpWrk = 0m;
                                string hashrate = item.Value["hashrate"].ToString();
                                if (item.Name.ToString().ToLower() == _worker.ToLower() && !string.IsNullOrWhiteSpace(hashrate))
                                {                                   
                                    if (hashrate.EndsWith("H/s"))
                                       AcSpWrk = hashrate.Replace("H/s", "").ExtractDecimal() / 1000;
                                    if (hashrate.EndsWith("MH/s"))
                                        AcSpWrk = hashrate.Replace("MH/s", "").ExtractDecimal() * 1000;
                                    if (hashrate.EndsWith("kH/s"))
                                        AcSpWrk = hashrate.Replace("kH/s", "").ExtractDecimal();

                                    if (AcSpWrk.ExtractDecimal() > entry.AcceptSpeed)  AcSpWrk = 0;

                                    entry.AcSpWrk = AcSpWrk.ExtractDecimal();

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
