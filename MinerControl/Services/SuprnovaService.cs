using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class SuprnovaService : ServiceBase<SuprnovaPriceEntry>
    {
        public SuprnovaService()
        {
            ServiceName = "Suprnova";
        }
        public override void Initialize(IDictionary<string, object> data)
        {
            ReadData(data);
        }

        public override void CheckFees()
        {
            if (_autofee == true)
                FeesUpdate("https://" + "TAG" + ".suprnova.cc/index.php?page=api&action=getpoolinfo&api_key=" + "APIKEY");
        }

        public override void CheckPrices()
        {
            WTMUpdate();
        }

        public override void CheckData()
        {
            string urs = "https://" + "TAG" + ".suprnova.cc/index.php?page=api&action=getdashboarddata&api_key=" + "APIKEY";
            string urb = "https://" + "TAG" + ".suprnova.cc/index.php?page=api&action=getuserbalance&api_key=" + "APIKEY" + "&id=" + "USERID";
            string urw = "https://" + "TAG" + ".suprnova.cc/index.php?page=api&action=getuserworkers&api_key=" + "APIKEY" + "&id=" + "USERID";

            MPOSDataUpdate(urs, urb, urw);
        }
    }
}



