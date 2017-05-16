using System;
using System.Collections.Generic;
using MinerControl.PriceEntries;

namespace MinerControl.Services
{
    public class ManualService : ServiceBase<ManualPriceEntry>
    {
        public ManualService()
        {
            ServiceName = "Manual";
        }

        public ManualService(string name)
        {
            ServiceName = name;
        }

        public override void Initialize(IDictionary<string, object> data)
        {
            ExtractCommon(data);

            decimal price = 0, fee = 0;
            if (data.ContainsKey("price"))
                price = data["price"].ExtractDecimal();
            if (data.ContainsKey("fee"))
                fee = data["fee"].ExtractDecimal();

            object[] items = data["algos"] as object[];
            foreach (object rawitem in items)
            {
                Dictionary<string, object> item = rawitem as Dictionary<string, object>;

                if (item.ContainsKey("price") && item["price"].ExtractDecimal()!=0)
                {
                    fee = item["fee"].ExtractDecimal();
                }

                    ManualPriceEntry entry = CreateEntry(item);
                entry.Price = price;
                entry.FeePercent = fee/100;

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
            lock (MiningEngine)
            {
                LastUpdated = DateTime.Now;

                UpdateHistory();
            }
        }

        public override void CheckData()
        {
        }
    }
}