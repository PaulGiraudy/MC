using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class DualMiningService : ServiceBase<DualMiningPriceEntry>
    {
        public DualMiningService()
        {
            ServiceName = "DualMining";
        }
        public override void Initialize(IDictionary<string, object> data)
        {
            ExtractCommon(data);

           
            object[] items = data["algos"] as object[];
            foreach (object rawitem in items)
            {
                Dictionary<string, object> item = rawitem as Dictionary<string, object>;
                DualMiningPriceEntry entry = CreateEntry(item);

                if (item.ContainsKey("cname_first"))
                    entry.CoinNameFirst = item["cname_first"].ToString();
                if (item.ContainsKey("cname_second"))
                    entry.CoinNameSecond = item["cname_second"].ToString();
                if (item.ContainsKey("tag_first"))
                    entry.TagFirst = item["tag_first"].ToString();
                if (item.ContainsKey("tag_second"))
                    entry.TagSecond = item["tag_second"].ToString();
                if (item.ContainsKey("hashrate_first"))
                    entry.HashrateFirst = item["hashrate_first"].ExtractDecimal();
                if (item.ContainsKey("hashrate_second"))
                    entry.HashrateSecond = item["hashrate_second"].ExtractDecimal();


                if (entry.AlgoName.ToLower() == "dual")
                {
                    entry.CoinName = entry.TagFirst.ToUpper() + "/" + entry.TagSecond.ToUpper();
                    entry.AlgoName = entry.AlgoName + "_" + entry.CoinName;
                    entry.Name = entry.Name + "_" + entry.CoinName;
                    entry.ApiKey = null;
                    entry.UserId = null;                    
                }

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
            PriceUpdate();
        }

        public override void CheckData()
        {
        }

       
        public void PriceUpdate()
        {
            if (MiningEngine.WTMArray[0, 0] != null)
            {
                for (int i = 0; i < MiningEngine.WTMArray.Length / 6; i++)
                {
                    float price = 0; float er = 0; float price24 = 0; float er24 = 0;

                    price = MiningEngine.WTMArray[i, 1];
                    price24 = MiningEngine.WTMArray[i, 2];
                    er = MiningEngine.WTMArray[i, 3];
                    er24 = MiningEngine.WTMArray[i, 4];
                    bool _lagstatus = MiningEngine.WTMArray[i, 5];

                    var cname = MiningEngine.WTMArray[i, 0];

                    lock (MiningEngine)
                    {

                        foreach (DualMiningPriceEntry entry in PriceEntries)
                        {
                            if (entry.AlgoName.StartsWith("dual") == false && entry.CoinName.ToLower() == cname.ToLower())
                            {
                                entry.Price = price.ExtractDecimal() * 1000000000;
                                entry.ExRate = er.ExtractDecimal();
                            }

                            if (entry.AlgoName.StartsWith("dual") && entry.CoinNameFirst.ToLower() == cname.ToLower())
                                entry.PriceFirst = price.ExtractDecimal() * 1000000000;
 
                            if (entry.AlgoName.StartsWith("dual") && entry.CoinNameSecond.ToLower() == cname.ToLower())
                                entry.PriceSecond = price.ExtractDecimal() * 1000000000;

                            entry.Lagging = _lagstatus;
                        }

                        foreach (DualMiningPriceEntry entry in PriceEntries)
                        {
                            if (entry.AlgoName.StartsWith("dual"))
                            {
                                entry.Hashrate = entry.HashrateFirst + entry.HashrateSecond;
                                entry.Price = (entry.PriceFirst * entry.HashrateFirst + entry.PriceSecond * entry.HashrateSecond) / entry.Hashrate;
                                if (entry.Lagging == true) 
                                    entry.Price = entry.Price / 10;
                            }
                        }                     

                        MiningEngine.PricesUpdated = true;
                        MiningEngine.HasPrices = true;

                        LastUpdated = DateTime.Now;

                        UpdateHistory();

                    }
                }
            }
        }
    }    
}



