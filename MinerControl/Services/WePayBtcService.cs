using MinerControl.PriceEntries;
using MinerControl.Utility;
using System;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class WePayBtcService : ServiceBase<WePayBtcPriceEntry>
    {
        // http://wepaybtc.com/payouts.json

        public WePayBtcService()
        {
            ServiceName = "WePayBTC";
            DonationAccount = "1PMj3nrVq5CH4TXdJSnHHLPdvcXinjG72y";
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
            WebUtil1.DownloadJson("http://wepaybtc.com/payouts.json", ProcessPrices);
        }

        public override void CheckData()
        {
        }
        private void ProcessPrices(object jsonData)
        {
            Dictionary<string, object> data = jsonData as Dictionary<string, object>;

            lock (MiningEngine)
            {
                foreach (string key in data.Keys)
                {
                    object rawitem = data[key];
                    Dictionary<string, object> item = rawitem as Dictionary<string, object>;
                    string algo = key.ToLower();

                    WePayBtcPriceEntry entry = GetEntry(algo);
                    if (entry == null) continue;

                    entry.Price = data[key].ExtractDecimal()*1000;
                    //AveragePrice(entry);
                }

                MiningEngine.PricesUpdated = true;
                MiningEngine.HasPrices = true;

                LastUpdated = DateTime.Now;

                UpdateHistory();
          }
        }
    }
}