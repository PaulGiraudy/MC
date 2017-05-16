using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class DwarfpoolService : ServiceBase<DwarfpoolPriceEntry>
    {
        public DwarfpoolService()
        {
            ServiceName = "Dwarfpool";    }

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
            string _yourMail = "mail@example.com";

            if (_param2 != null)
                _yourMail = _param2;

            foreach (DwarfpoolPriceEntry entry in PriceEntries)
            {
                if (!string.IsNullOrWhiteSpace(entry.Wallet) && !string.IsNullOrWhiteSpace(entry.Tag))
                {
                    WebUtil2.DownloadJson("http://dwarfpool.com/" + entry.Tag + "/api?wallet=" + entry.Wallet + "&email=" + _yourMail, ProcessData);
                }
            }
        }
       
        private void ProcessData(object RawData)
        {
            if (RawData != null)
            {
                float speed = 0;

                JObject data = (JObject)RawData;

                if (data["error"].ToString().ToLower() != "true")
                {

                    string wallet = data["wallet"].ToString();
                    speed = (float)data["total_hashrate"];
                    JToken workers = data["workers"];
                    wallet = wallet.Replace("0x", "");

                    foreach (DwarfpoolPriceEntry entry in PriceEntries)
                    {
                        if (entry.Wallet.ToLower().Contains(wallet.ToString().ToLower()))
                        {
                            entry.AcceptSpeed = speed.ExtractDecimal();

                            if (workers != null)
                            {
                                foreach (JProperty item in workers.Children())
                                {
                                    var AcSpWrk = (float)item.Value["hashrate"];
                                    if (item.Name.ToString().ToLower() == _worker.ToLower() && AcSpWrk >0)
                                    {
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
}
