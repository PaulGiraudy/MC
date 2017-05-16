using System;
using System.Collections.Generic;
using MinerControl.PriceEntries;
using MinerControl.Utility;

namespace MinerControl.Services
{
    public class LtcRabbitService : ServiceBase<LtcRabbitPriceEntry>
    {

        public LtcRabbitService()
        {
            ServiceName = "LTCRabbit";
            DonationAccount = "MinerControl";
            DonationWorker = "1";
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
            WebUtil1.DownloadJson("https://www.ltcrabbit.com/index.php?page=api&action=public", ProcessPrices);
        }

        public override void CheckData()
        {
            WebUtil1.DownloadJson(
                string.Format(
                    "https://www.ltcrabbit.com/index.php?page=api&action=getappdata&appname=MinerControl&appversion=1&api_key={0}",
                    _apikey), ProcessBalances);
        }

        private void ProcessPrices(object jsonData)
        {
            Dictionary<string, object> data = jsonData as Dictionary<string, object>;
            Dictionary<string, object> profitability = data["profitability"] as Dictionary<string, object>;
            Dictionary<string, object> current = profitability["current"] as Dictionary<string, object>;

            lock (MiningEngine)
            {
                foreach (string key in current.Keys)
                {
                    object rawitem = current[key];
                    Dictionary<string, object> item = rawitem as Dictionary<string, object>;
                    string algo = key.ToLower();

                    LtcRabbitPriceEntry entry = GetEntry(algo);
                    if (entry == null) continue;

                    entry.Price = item["btc_mh"].ExtractDecimal()*1000;
                }

                MiningEngine.PricesUpdated = true;
                MiningEngine.HasPrices = true;

                LastUpdated = DateTime.Now;

                UpdateHistory();
            }
        }

        private void ProcessBalances(object jsonData)
        {
            Dictionary<string, object> data = jsonData as Dictionary<string, object>;
            Dictionary<string, object> getappdata = data["getappdata"] as Dictionary<string, object>;
            Dictionary<string, object> ltc_exchange_rates = getappdata["ltc_exchange_rates"] as Dictionary<string, object>;
            Dictionary<string, object> btc_exchange_rates = getappdata["btc_exchange_rates"] as Dictionary<string, object>;
            Dictionary<string, object> user = getappdata["user"] as Dictionary<string, object>;
            ServiceBalance = user["balance_btc"].ExtractDecimal();

            LtcRabbitPriceEntry entry = GetEntry("x11");
            if (entry != null)
            {
                decimal hashrate = user["hashrate_x11"].ExtractDecimal();
                entry.AcceptSpeed = hashrate/1000;
            }

            entry = GetEntry("scrypt");
            if (entry != null)
            {
                decimal hashrate = user["hashrate_scrypt"].ExtractDecimal();
                entry.AcceptSpeed = hashrate/1000;
            }
        }
    }
}