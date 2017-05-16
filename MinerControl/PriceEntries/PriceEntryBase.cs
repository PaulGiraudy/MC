using System;
using System.Drawing;
using MinerControl.Services;
using MinerControl.Utility;

namespace MinerControl.PriceEntries
{
    public abstract class PriceEntryBase : PropertyChangedBase
    {
        private decimal _balance;
        private decimal _pending;
        private string _dynamic;
        private decimal _balanceBTC;
        private decimal _balanceCUR;
        private bool _banned;
        private bool _belowMinPrice;
        private bool _outlier;
        private bool _lagging;
        private bool _pumping;
        private bool _dumping;
        private bool _pumpcorrection;
        private bool _enabled;
        private DateTime _deadTime;
        private decimal _fees;
        private decimal _price;
        private decimal _AcSpWrk;
        private decimal _AvgSp;
        private decimal _TopAvgSp;
        private decimal _acceptSpeed;
        private decimal _rejectSpeed;
        private TimeSpan _timeMining;
        private decimal _weight;
        private Color? _color;

        public PriceEntryBase()
        {
            DeadTime = DateTime.MinValue;
        }

        public MiningEngine MiningEngine { get; set; }
        public int Id { get; set; }
        public IService ServiceEntry { get; set; }
        public string PriceId { get; set; }
        public string AlgoName { get; set; }
        public string Name { get; set; }
        public bool UseWindow { get; set; }
        public decimal MinProfit { get; set; }
        public decimal FeePercent { get; set; }
        public decimal Hashrate { get; set; }

        public dynamic[,] SpWrkLog = new dynamic [(int)MiningEngine._AvSpTicks+1, 2];
        public dynamic[,] PriceLog = new dynamic[(int) MiningEngine.statWindow+1, 2];
        public dynamic[,] ExRateLog = new dynamic[(int) MiningEngine.statWindow+1, 2];
        public float AvgExRate { get; set; }
        public decimal Power { get; set; }
        public string Priority { get; set; }
        public int Affinity { get; set; }
        public string Folder { get; set; }
        public string Command { get; set; }
        public string Arguments { get; set; }
        public string ApiKey { get; set; }
        public string Wallet { get; set; }

        public bool WTMListed = false;
        public string UserId { get; set; }
        public string CoinName { get ; set; }     
        public string Tag { get; set; }       
        public decimal ExRate { get; set; }
        public decimal CWeight { get; set; }
        public DateTime DynUpdated { get; set; }
        public string MU { get; set; }
        public decimal DevFee { get; set; }
        public string DonationFolder { get; set; }
        public string DonationCommand { get; set; }
        public string DonationArguments { get; set; }

        public decimal NetAverage { get; set; }

        public decimal Price
        {
            get { return _price; }
            set { SetField(ref _price, value, () => Price, () => Earn, () => Fees, () => NetEarn); }
        }

        public string PriceDynamics
        {
            get { return _dynamic; }
            set { SetField(ref _dynamic, value, () => PriceDynamics); }
        }
        public decimal Earn
        {
            get { return Price/1000*Hashrate/1000; }
        }

        public decimal PowerCost
        {
            get { return Power/1000*24*MiningEngine.PowerCost/MiningEngine.Exchange; }
        }

        public decimal Fees
        {
            get
            { if (DevFee != 0) return Earn * (FeePercent * DevFee / 100);
                else return Earn * (FeePercent);
            }
            set { SetField(ref _fees, value, () => Fees, () => NetEarn); }
        }

        public decimal Weight
        {
            get { return _weight; }
            set { SetField(ref _weight, value, () => Weight, () => NetEarn); }
        }

        public decimal NetEarn
        {
            get { return ((Earn - Fees) *Weight*CWeight) - PowerCost; }
        }

        public Color? Color
        {
            get
            {
                return _color ?? (_color = (ServiceEntry.ServiceName + AlgoName).GetColorRepresentation());
            }
        }

        public decimal Balance
        {
            get { return _balance; }
            set { SetField(ref _balance, value, () => Balance, () => BalancePrint); }
        }

        public decimal Pending
        {
            get { return _pending; }
            set { SetField(ref _pending, value, () => Pending, () => PendingPrint); }
        }

        public decimal BalanceBTC
        {
            get { return _balanceBTC; }
            set { SetField(ref _balanceBTC, value, () => BalanceBTC, () => BalanceBTCPrint); }
        }

        public decimal NetEarnCUR
        {
            get { return NetEarn * MiningEngine.Exchange; }
        }

        public decimal AcSpWrk
        {
            get { return _AcSpWrk; }
            set { SetField(ref _AcSpWrk, value, () => AcSpWrk, () => AcSpWrkPrint); }
        }

        public decimal AvgSp
        {
            get { return _AvgSp; }
            set { SetField(ref _AvgSp, value, () => AvgSp, () => AvgSpPrint); }
        }
        public decimal TopAvgSp
        {
            get { return _TopAvgSp; }
            set { SetField(ref _TopAvgSp, value, () => TopAvgSp, () => TopAvgSpPrint); }
        }
        public decimal AcceptSpeed
        {
            get { return _acceptSpeed; }
            set { SetField(ref _acceptSpeed, value, () => AcceptSpeed, () => AcceptSpeedPrint); }
        }

        public decimal RejectSpeed
        {
            get { return _rejectSpeed; }
            set { SetField(ref _rejectSpeed, value, () => RejectSpeed, () => RejectSpeedPrint); }
        }

        public TimeSpan TimeMining
        {
            get { return _timeMining; }
            set { SetField(ref _timeMining, value, () => TimeMining, () => TimeMiningPrint); }
        }

        public bool Banned
        {
            get { return _banned; }
            set { SetField(ref _banned, value, () => Banned, () => StatusPrint); }
        }

        public bool BelowMinPrice
        {
            get { return _belowMinPrice; }
            set { SetField(ref _belowMinPrice, value, () => BelowMinPrice, () => StatusPrint); }
        }

        public bool Outlier
        {
            get { return _outlier; }
            set { SetField(ref _outlier, value, () => Outlier, () => StatusPrint); }
        }

        public bool Lagging
        {
            get { return _lagging; }
            set { SetField(ref _lagging, value, () => Lagging, () => StatusPrint); }
        }

        public bool Pumping
        {
            get { return _pumping; }
            set { SetField(ref _pumping, value, () => Pumping, () => StatusPrint); }
        }

        public bool Dumping
        {
            get { return _dumping; }
            set { SetField(ref _dumping, value, () => Dumping, () => StatusPrint); }
        }

        public bool PumpReduction
        {
            get { return _pumpcorrection; }
            set { SetField(ref _pumpcorrection, value, () => PumpReduction, () => StatusPrint); }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { SetField(ref _enabled, value, () => Enabled, () => StatusPrint); }
        }
        public DateTime DeadTime
        {
            get { return _deadTime; }
            set { SetField(ref _deadTime, value, () => DeadTime, () => StatusPrint); }
        }

        public bool IsDead
        {
            get { return (DeadTime + MiningEngine.DeadTime) > DateTime.Now; }
        }

        public TimeSpan TimeMiningWithCurrent
        {
            get
            {
                return MiningEngine.CurrentRunning.HasValue && MiningEngine.CurrentRunning.Value == Id &&
                       MiningEngine.StartMining.HasValue
                    ? (TimeMining + (DateTime.Now - MiningEngine.StartMining.Value))
                    : TimeMining;
            }
        }

        public string ServicePrint
        {
            get { return ServiceEntry.ServiceName; }
        }

        public string HashratePrint
        {
            get { return Hashrate == 0.0m ? string.Empty : Hashrate.ToString("N2"); }
        }

        public string PoolFeePrint
        {
            get { return FeePercent == 0.0m ? "0.0%" : FeePercent.ToString("P1"); }
        }
        public string BalancePrint
        {
            get { return Balance == 0.0m ? string.Empty : Balance.ToString("N4"); }
        }

        public string PendingPrint
        {
            get { return Pending == 0.0m ? string.Empty : Pending.ToString("N4"); }
        }

        public string BalanceBTCPrint
        {
            get { return BalanceBTC == 0.0m ? string.Empty : BalanceBTC.ToString("N8"); }
        }

        public string NetEarnCURPrint
        {
            get { return NetEarnCUR == 0.0m ? string.Empty : NetEarnCUR.ToString("N3"); }
        }

        public string AcceptSpeedPrint
        {
            get { return AcceptSpeed == 0.0m ? string.Empty : AcceptSpeed.ToString("N2"); }
        }

        public string AcSpWrkPrint
        {
            get { return AcSpWrk == 0.0m ? string.Empty : AcSpWrk.ToString("N2"); }
        }

        public string AvgSpPrint
        {
            get { return AvgSp == 0.0m ? string.Empty : AvgSp.ToString("N2"); }
        }

        public string TopAvgSpPrint
        {
            get { return TopAvgSp == 0.0m ? string.Empty : TopAvgSp.ToString("N2"); }
        }
        public string RejectSpeedPrint
        {
            get { return RejectSpeed == 0.0m ? string.Empty : RejectSpeed.ToString("N2"); }
        }

        public string TimeMiningPrint
        {
            get { return TimeMiningWithCurrent.FormatTime(true); }
        }

        public string StatusPrint
        {
            get
            {
                if (MiningEngine.CurrentRunning.HasValue && MiningEngine.CurrentRunning.Value == Id)
                    return "Running";
                if (IsDead)
                    return "Dead";
                if (Banned)
                    return "Banned";
                if (BelowMinPrice)
                    return "Too low";
                if (Outlier)
                    return "Outlier";
                if (Lagging)
                    return "Lagging";
                if (!Enabled)
                    return "Disabled";
                if (Pumping && !PumpReduction)
                    return "Pumping";
                if (Pumping && PumpReduction)
                    return "PumpReduction";
                if (Dumping)
                    return "Dumping";
                if (MiningEngine.NextRun.HasValue && MiningEngine.NextRun.Value == Id)
                    return "Pending";
                return string.Empty;
            }
        }

        public void UpdateStatus()
        {
            OnPropertyChanged(() => StatusPrint);
            OnPropertyChanged(() => TimeMiningPrint);
            ServiceEntry.UpdateTime();
        }

        public void UpdateExchange()
        {
            OnPropertyChanged(() => PowerCost);
            OnPropertyChanged(() => NetEarn);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PriceEntryBase) obj);
        }

        protected bool Equals(PriceEntryBase other)
        {
            return Id == other.Id && Equals(ServiceEntry, other.ServiceEntry) && string.Equals(PriceId, other.PriceId) &&
            string.Equals(AlgoName, other.AlgoName) && string.Equals(Name, other.Name) && string.Equals(CoinName, other.CoinName) &&
            string.Equals(Tag, other.Tag) && UseWindow.Equals(other.UseWindow) && MinProfit == other.MinProfit && Hashrate == other.Hashrate &&
            Power == other.Power && string.Equals(Priority, other.Priority) && Affinity == other.Affinity &&
            string.Equals(Folder, other.Folder) && string.Equals(Command, other.Command) &&
            string.Equals(Arguments, other.Arguments) && string.Equals(DonationFolder, other.DonationFolder) &&
            string.Equals(DonationCommand, other.DonationCommand) &&
            string.Equals(DonationArguments, other.DonationArguments);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id;
                hashCode = (hashCode * 397) ^ (ServiceEntry != null ? ServiceEntry.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PriceId != null ? PriceId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AlgoName != null ? AlgoName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ UseWindow.GetHashCode();
                hashCode = (hashCode * 397) ^ MinProfit.GetHashCode();
                hashCode = (hashCode * 397) ^ Hashrate.GetHashCode();
                hashCode = (hashCode * 397) ^ Power.GetHashCode();
                hashCode = (hashCode * 397) ^ (Priority != null ? Priority.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Affinity;
                hashCode = (hashCode * 397) ^ (Folder != null ? Folder.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Command != null ? Command.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Arguments != null ? Arguments.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DonationFolder != null ? DonationFolder.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DonationCommand != null ? DonationCommand.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DonationArguments != null ? DonationArguments.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}