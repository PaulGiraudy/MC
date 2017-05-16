using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class CoinMineService : ServiceBase<CoinMinePriceEntry>
    {
        public CoinMineService()
        {
            ServiceName = "CoinMine";
        }
        public override void Initialize(IDictionary<string, object> data)
        {
            ReadData(data);
        }

        public override void CheckFees()
        {
            if (_autofee == true)
                FeesUpdate("https://www2.coinmine.pl/" + "TAG" + "/index.php?page=api&action=getpoolinfo&api_key=" + "APIKEY");
        }
        public override void CheckPrices()
        {
            WTMUpdate();
        }
        public override void CheckData()
        {
            string urs = "https://www2.coinmine.pl/" + "TAG" + "/index.php?page=api&action=getdashboarddata&api_key=" + "APIKEY";
            string urb = "https://www2.coinmine.pl/" + "TAG" + "/index.php?page=api&action=getuserbalance&api_key=" + "APIKEY" + "&id=" + "USERID";
            string urw = "https://www2.coinmine.pl/" + "TAG" + "/index.php?page=api&action=getuserworkers&api_key=" + "APIKEY" + "&id=" + "USERID";

            MPOSDataUpdate(urs, urb, urw);
        }
    }
}



