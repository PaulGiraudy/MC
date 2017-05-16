using MinerControl.PriceEntries;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class WhattomineService : ServiceBase<WhattominePriceEntry>
    {
       
        private const string Url = "http://www.whattomine.com/coins.json";
        private decimal _minVolume;

        public WhattomineService(string pool)
        {
            ServiceName = pool;
        }

        public override void Initialize(IDictionary<string, object> data)
        {
            ExtractCommon(data);

            DonationAccount = _account;
            DonationWorker = _worker;

       
            if (data.ContainsKey("minvolume"))
                _minVolume = data["minvolume"].ExtractDecimal();
            
            object[] items = data["algos"] as object[];

            foreach (object rawitem in items)
            {
                Dictionary<string, object> item = rawitem as Dictionary<string, object>;
                WhattominePriceEntry entry = CreateEntry(item);

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
            WTMUpdate();
        }

        public override void CheckData()
        {
        }   

    }
}