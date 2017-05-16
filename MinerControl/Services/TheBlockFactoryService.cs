using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class TheBlockFactoryService : ServiceBase<TheBlockFactoryPriceEntry>
    {
        public TheBlockFactoryService()
        {
            ServiceName = "TheBlockFactory";
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
            string urd =  "https://" + "TAG" + ".theblocksfactory.com/api.php?api_key=" + "APIKEY";
            APIDataUpdate(urd);
        }     
    }
}