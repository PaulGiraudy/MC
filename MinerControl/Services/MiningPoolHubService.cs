using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class MiningPoolHubService : ServiceBase<MiningPoolHubPriceEntry>
    {
        public MiningPoolHubService()
        {
            ServiceName = "MiningPoolHub";
        }
        public override void Initialize(IDictionary<string, object> data)
        {
            ReadData(data);
        }

        public override void CheckFees()
        {
            if (_autofee == true)
               FeesUpdate("http://" + "COINNAME" + ".miningpoolhub.com/index.php?page=api&action=getpoolinfo&api_key=" + "APIKEY" + "&id=" + "USERID");
        }

        public override void CheckPrices()
        {
            if (PriceWTM == true) WTMUpdate();
            else
            {
                WebUtil2.DownloadJson("https://miningpoolhub.com/index.php?page=api&action=getminingandprofitsstatistics", ProcessPrices);
            }
        }
        public override void CheckData()
        {

            string urs = string.Empty; string urb = string.Empty; string urw = "https://" + "COINNAME" + ".miningpoolhub.com/index.php?page=api&action=getuserworkers&api_key=" + "APIKEY" + "&id=" + "USERID";

            if (_balancemode == 0)
            {
                urs = "https://" + "COINNAME" + ".miningpoolhub.com/index.php?page=api&action=getdashboarddata&api_key=" + "APIKEY";
                urb = "https://" + "COINNAME" + ".miningpoolhub.com/index.php?page=api&action=getuserbalance&api_key=" + "APIKEY" + "&id=" + "USERID";

                MPOSDataUpdate(urs, urb, urw);
            }
            else
            {
                MPOSDataUpdate(urs, urb, urw);

                foreach (MiningPoolHubPriceEntry entry in PriceEntries)
                {
                    if (_nobalance == false)
                        WebUtil2.DownloadJson("http://" + entry.CoinName + ".miningpoolhub.com/index.php?page=api&action=getdashboarddata&api_key=" + _apikey + "&id=" + _userid, ProcessBalances);
                }
            }
        }

        private void ProcessPrices(object RawPrices)
        {
            if (RawPrices != null)
            {
                JObject data = (JObject)RawPrices;
                JToken ret = data["return"];

                lock (MiningEngine)
                {
                    foreach (JToken item in ret.Children())
                    {
                        string cname = item["coin_name"].ToString();
                        string algo = item["algo"].ToString();
                        string host = item["host"].ToString();
                        float profit = (float)item["profit"];
                        float exrate = (float)item["highest_buy_price"];

                        MiningPoolHubPriceEntry entry = GetEntryCoin(cname);
                        if (entry == null) continue;

                        entry.Price = profit.ExtractDecimal();
                        entry.ExRate = exrate.ExtractDecimal();

                       // AveragePrice(entry);

                        MiningEngine.PricesUpdated = true;
                        MiningEngine.HasPrices = true;

                        LastUpdated = DateTime.Now;

                        UpdateHistory();
                    }
                }
            }
        }

        private void ProcessBalances(object RawBalances)
        {
            if (RawBalances != null)
            {
                float bal = 0; float pend = 0;
                float hashrate = 0; float rejected = 0;

                JObject rdat = (JObject)RawBalances;
                JToken db = rdat["getdashboarddata"];
                JToken value = db["data"];

                string cur = db["data"]["pool"]["info"]["currency"].ToString();
                string name = db["data"]["pool"]["info"]["name"].ToString();
                string coinname = name.Remove(name.IndexOf("(") - 1);
                coinname = coinname.Replace(" ", string.Empty);
                JToken balance = value["balance"];
                JToken balanceFAE = value["balance_for_auto_exchange"];
                JToken balanceOE = value["balance_on_exchange"];

                switch (_balancemode)
                {
                    case (0):
                        bal = (float)balance["confirmed"];
                        pend = (float)balance["unconfirmed"];
                        break;
                    case (1):
                        bal = (float)balanceFAE["confirmed"];
                        pend = (float)balanceFAE["unconfirmed"];
                        break;
                    case (2):
                        bal = (float)balanceOE["confirmed"];
                        pend = (float)balanceOE["unconfirmed"];
                        break;
                    default:
                        bal = (float)balanceFAE["confirmed"];
                        pend = (float)balanceFAE["unconfirmed"];
                        break;
                }

                JToken personal = value["personal"];
                JToken shares = personal["shares"];

                hashrate = (float)personal["hashrate"];
                rejected = hashrate * (float)shares["invalid_percent"] / 100;


                MiningPoolHubPriceEntry entry = GetEntryCoin(coinname);
                if (entry != null)
                {
                    entry.Balance = bal.ExtractDecimal();
                    entry.Pending = pend.ExtractDecimal();
                    entry.BalanceBTC = entry.Balance * entry.ExRate;

                    if (_nospeed == false)
                        ProcessSpeed(entry, hashrate, rejected);
                }
            }
        }

        private void ProcessSpeed(dynamic entry, float hashrate, float rejected)
        {

            if (entry.AlgoName.ToLower().StartsWith("blake") || entry.AlgoName.ToLower().StartsWith("decred"))
            {
                entry.AcceptSpeed = hashrate.ExtractDecimal() * 1000000;
                entry.RejectSpeed = rejected.ExtractDecimal() * 1000000;
            }
            else if (entry.AlgoName.ToLower().StartsWith("equihash"))
            {
                entry.AcceptSpeed = hashrate.ExtractDecimal() / 1000;
                entry.RejectSpeed = rejected.ExtractDecimal() / 1000;
            }
            else if (entry.AlgoName.ToLower().StartsWith("cryptonight"))
            {
                entry.AcceptSpeed = hashrate.ExtractDecimal();
                entry.RejectSpeed = rejected.ExtractDecimal();
            }
            else
            {
                entry.AcceptSpeed = hashrate.ExtractDecimal() * 1000;
                entry.RejectSpeed = rejected.ExtractDecimal() * 1000;
            }

        }
    }
}