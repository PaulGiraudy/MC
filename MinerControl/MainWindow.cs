using MinerControl.History;
using MinerControl.PriceEntries;
using MinerControl.Services;
using MinerControl.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;



namespace MinerControl
{
    public partial class MainWindow : Form
    {
        public string CFile;
        public DateTime _headerclick = DateTime.Today;
        private DateTime _appStartTime = DateTime.Now;
        private readonly SlidingBuffer<string> _consoleBuffer = new SlidingBuffer<string>(200);
        private MiningEngine _engine = new MiningEngine();
        private readonly SlidingBuffer<string> _remoteBuffer = new SlidingBuffer<string>(200);
        private TotalHistoryForm _totalHistoryForm;
        public TimeSpan MT;

        public MainWindow()
        {
            _engine.WriteConsoleAction = WriteConsole;
            _engine.WriteRemoteAction = WriteRemote;

            InitializeComponent();
        }

        private bool IsMinimizedToTray
        {
            get { return _engine.TrayMode > 0 && WindowState == FormWindowState.Minimized; }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (!_engine.LoadConfig(CFile))
                Close();
            if (!string.IsNullOrWhiteSpace(_engine.CurrencyCode))
                _engine.LoadExchangeRates();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            // speeds up data grid view performance.
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dgPrices,
                new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dgServices,
                new object[] { true });

            dgServices.AutoGenerateColumns = false;
            dgServices.DataSource = new SortableBindingList<IService>(_engine.Services);

            dgPrices.AutoGenerateColumns = false;
            dgPrices.DataSource = new SortableBindingList<PriceEntryBase>(_engine.PriceEntries);

            HideColumns();
            

            if (!_engine.DoDonationMinging)
            {
                textDonationStart.Enabled = false;
                textDonationEnd.Enabled = false;
            }

            lblCurrencySymbol.Text = string.Empty; // Avoid flashing template value when starting

            if (!_engine.RemoteReceive)
                tabPage.TabPages.Remove(tabRemote);

            UpdateButtons();
            //RunCycle();
            _engine.CheckFees();
            _engine.CheckPrices();
            UpdateGrid(true);


            if (Program.MinimizeOnStart)
                MinimizeWindow();

            tmrPriceCheck.Enabled = true;
            tmrIdleCheck.Enabled = true;
            if (!string.IsNullOrWhiteSpace(_engine.CurrencyCode))
                tmrExchangeUpdate.Enabled = true;
            if (Program.HasAutoStart)
            {
                _engine.MiningMode = MiningModeEnum.Automatic;
                UpdateButtons();
                RunBestAlgo();
            }

            if (_engine.ShowHistory)
            {
                InitHistoryChart();
            }
            else
            {
                tabHistory.Controls.Clear();
                tabPage.TabPages.Remove(tabHistory);
            }
        }


       
        private void InitHistoryChart()
        {
            HistoryChart preChart = tabHistory.Controls["historyChart"] as HistoryChart;
            if (preChart != null)
            {
                HistoryChart historyChart = new HistoryChart
                {
                    Dock = DockStyle.Fill,
                    History = _engine.PriceHistories
                };

                historyChart.FlipLegend();
                historyChart.UpdateChart(_engine.StatWindow, 3);
                historyChart.Chart.DoubleClick += ChartOnDoubleClick;
                tabHistory.Controls.Remove(preChart);
                tabHistory.Controls.Add(historyChart);
            }
        }

        private void ChartOnDoubleClick(object sender, EventArgs eventArgs)
        {
            if (_totalHistoryForm != null)
            {
                _totalHistoryForm.Focus();
            }
            else
            {
                _totalHistoryForm = new TotalHistoryForm(_engine.PriceHistories);
                _totalHistoryForm.FormClosing += _totalHistoryForm_FormClosing;
                _totalHistoryForm.Show();
            }
        }

        void _totalHistoryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _totalHistoryForm = null;
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _engine.RequestStop();
            _engine.Cleanup();
        }

        private void RunCycle()
        {
            _engine.CheckPrices();
            _engine.CheckData();
            _engine.UpdateBalance();
            _engine.MinPrice();
        }

        private void RunBestAlgo()
        {
            if (!_engine.HasPrices || (Program.HasAutoStart && (DateTime.Now - _appStartTime).TotalSeconds < 3)) return;

            int? oldCurrent = _engine.CurrentRunning;
            int? oldNext = _engine.NextRun;
            _engine.RunBestAlgo(IsMinimizedToTray);
            if (_engine.CurrentRunning != oldCurrent || _engine.NextRun != oldNext)
                UpdateGrid();
        }

        private void UpdateButtons()
        {
            btnStart.Enabled = _engine.MiningMode == MiningModeEnum.Stopped;
            btnStop.Enabled = _engine.MiningMode != MiningModeEnum.Stopped;
            dgPrices.Columns[dgPrices.Columns.Count - 2].Visible = _engine.MiningMode != MiningModeEnum.Stopped;
            // Status column
            dgPrices.Columns[dgPrices.Columns.Count - 1].Visible = _engine.MiningMode == MiningModeEnum.Stopped;
            // Action column
        }

        public void UpdateGrid(bool forceReorder = false)
        {
            lock (_engine)
            {
                // mode 2 == sort always, mode 1 == sort when running, mode 0 == sort never
                if (_engine.GridSortMode == 2 ||
                    (_engine.GridSortMode == 1 && (forceReorder || _engine.MiningMode == MiningModeEnum.Automatic)))
                {
                    if (_headerclick.AddSeconds(30) < DateTime.Now)
                    {
                        string column = _engine.MineByAverage ? "NetAverage" : "NetEarn";
                        dgPrices.Sort(dgPrices.Columns[column], ListSortDirection.Descending);
                    }
                }
                Coloring();
            }
        }

        private void linkDonate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://blockchain.info/address/1NoCAhu4dYxi162srrKzi5qZiQrERuu7A4");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://blockchain.info/address/1F9fsG5xLLn6uc7cdbgV1LTFWjWQSqXohX");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://blockchain.info/address/1PMj3nrVq5CH4TXdJSnHHLPdvcXinjG72y");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://blockchain.info/address/1C5X7k7gt9KteVaWrfp1nN8MguivTK2S1X");
        }
        private void UpdateTimes()
        {
            textRunningTotal.Text = _engine.TotalTime.FormatTime();
            textTimeCurrent.Text = _engine.MiningTime.FormatTime();
            textTimeSwitch.Text = _engine.NextRunTime.FormatTime();
            textTimeRestart.Text = _engine.RestartTime.FormatTime();
            textDonationStart.Text = _engine.TimeUntilDonation.FormatTime();
            textDonationEnd.Text = _engine.TimeDuringDonation.FormatTime();
            textCurrencyExchange.Text = _engine.Exchange.ToString("N2");
            lblCurrencySymbol.Text = _engine.CurrencySymbol;
            label5.Text = _engine.CurrencySymbol;
            this.ServiceCurrency.HeaderText = "Balance " + _engine.CurrencySymbol;
            this.NetEarnCUR.HeaderText = "Net_" + _engine.CurrencySymbol;
            if (_engine.Services != null)
            {
                decimal balance = _engine.Services.Select(o => o.Currency).Sum();
                textCurrencyBalance.Text = balance.ToString("N4");
            }
        }

        private string ActiveTime(PriceEntryBase priceEntry)
        {
            TimeSpan time = priceEntry.TimeMining;
            if (_engine.CurrentRunning == priceEntry.Id && _engine.StartMining.HasValue)
                time += (DateTime.Now - _engine.StartMining.Value);
            return time.FormatTime();
        }

        private void WriteConsole(string text)
        {

            Invoke(new MethodInvoker(
                delegate
                {
                    _consoleBuffer.Add(text);
                    textConsole.Lines = _consoleBuffer.ToArray();
                    textConsole.Focus();
                    textConsole.SelectionStart = textConsole.Text.Length;
                    textConsole.SelectionLength = 0;
                    textConsole.ScrollToCaret();
                    textConsole.Refresh();
                }));
        }

        private void WriteRemote(IPAddress source, string text)
        {
            if (!_engine.RemoteReceive)
            {
                Invoke(new MethodInvoker(
                    delegate
                    {
                        _remoteBuffer.Add(string.Format("[{0}] {1}", source, text));

                        textRemote.Lines = _remoteBuffer.ToArray();
                        textRemote.Focus();
                        textRemote.SelectionStart = textRemote.Text.Length;
                        textRemote.SelectionLength = 0;
                        textRemote.ScrollToCaret();
                        textRemote.Refresh();
                    }
                    ));
            }
        }

        #region Show/hide window

        private void MinimizeWindow()
        {
            if (_engine.TrayMode == 0)
            {
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                HideWindow();
            }
        }

        private void HideWindow()
        {
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(500);
            Hide();

            _engine.HideMinerWindow();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Show();
            WindowState = FormWindowState.Normal;

            _engine.MinimizeMinerWindow();
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (_engine.TrayMode > 0 && WindowState == FormWindowState.Minimized)
            {
                HideWindow();
            }
        }

        #endregion

        #region Buttons



        public void btnStart_Click(object sender, EventArgs e)
        {
            if (_engine.MiningMode != MiningModeEnum.Stopped) return;
            _engine.MiningMode = MiningModeEnum.Automatic;

            _engine.miningTime = MT;

            UpdateButtons();
            RunBestAlgo();
            UpdateGrid();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_engine.MiningMode == MiningModeEnum.Stopped) return;
            _engine.MiningMode = MiningModeEnum.Stopped;

            MT = _engine.miningTime;

            UpdateButtons();
            _engine.RequestStop();
            UpdateGrid();
        }

        private void btnReloadConfig_Click(object sender, EventArgs e)
        {
            Refresh(CFile);
        }

        private void Refresh(string CFile)
        {
            MiningModeEnum originalMode = _engine.MiningMode;
            string service = "Manual";
            string algo = string.Empty;
            int id = 0;
            MT = _engine.miningTime;
            if (_engine.CurrentPriceEntry != null)
            {
                service = _engine.CurrentPriceEntry.ServiceEntry.ServiceName;
                algo = _engine.CurrentPriceEntry.AlgoName;
                id = _engine.CurrentPriceEntry.Id;
            }

            _engine.RequestStop();
            _engine.Cleanup();

            _engine = new MiningEngine
            {
                WriteConsoleAction = WriteConsole,
                WriteRemoteAction = WriteRemote
            };
            _appStartTime = DateTime.Now;

            if (!_engine.LoadConfig(CFile))
                MessageBox.Show("Something went wrong with reloading your configuration file. Check for errors.",
                    "Error loading conf", MessageBoxButtons.OK, MessageBoxIcon.Error);

            dgServices.DataSource = new SortableBindingList<IService>(_engine.Services);
            dgPrices.DataSource = new SortableBindingList<PriceEntryBase>(_engine.PriceEntries);

            _engine.MiningMode = originalMode;

            _engine.LoadExchangeRates();

            UpdateButtons();
            //RunCycle();
            _engine.CheckFees();
            _engine.CheckPrices();
            UpdateGrid();

            _engine._autoMiningTime = MT;

            InitHistoryChart();

            if (originalMode == MiningModeEnum.Manual)
            {
                //_engine.RequestStart(service, algo, IsMinimizedToTray);
                _engine.RequestStart(id, IsMinimizedToTray);
            }
        }

        private void dgPrices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_engine.MiningMode != MiningModeEnum.Stopped) return;

            DataGridView senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                IList<PriceEntryBase> data = senderGrid.DataSource as IList<PriceEntryBase>;
                PriceEntryBase entry = data[e.RowIndex];

                _engine.MiningMode = MiningModeEnum.Manual;
                UpdateButtons();
                _engine.RequestStart(entry.Id, IsMinimizedToTray);
                UpdateGrid();
            }
        }

        private void dgPrices_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Coloring();
            _headerclick = DateTime.Now;
        }

        private void CheckBoxUpdate()
        {
            int _stActive = 1;
            if (dgPrices.Columns.Contains("Active")) _stActive = dgPrices.Columns.IndexOf(dgPrices.Columns["Active"]);

            foreach (DataGridViewRow row in dgPrices.Rows)
            {
                IList<PriceEntryBase> data = dgPrices.DataSource as IList<PriceEntryBase>;
                PriceEntryBase entry = data[row.Index];

                if (entry.Enabled == true)
                {
                    row.Cells[_stActive].Value = "true";
                }
                else
                {
                    row.Cells[_stActive].Value = "false";
                    row.DefaultCellStyle.ForeColor = Color.DarkGray;
                }
            }
        }
        private void dgPrices_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView senderGrid = (DataGridView)sender;

            if (e.RowIndex >= 0)
            {
                IList<PriceEntryBase> data = senderGrid.DataSource as IList<PriceEntryBase>;
                PriceEntryBase entry = data[e.RowIndex];

                _engine.SwitchBanStatus(entry.ServicePrint);
                UpdateGrid();
            }
        }

        private void dgPrices_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
            {
                IList<PriceEntryBase> data = senderGrid.DataSource as IList<PriceEntryBase>;
                PriceEntryBase entry = data[e.RowIndex];

                switch ((bool)dgPrices.CurrentCell.FormattedValue)
                {
                    case true:
                        dgPrices.CurrentCell.Value = "false";
                        entry.Enabled = false;
                        // entry.Banned = true;
                        dgPrices.CurrentRow.DefaultCellStyle.ForeColor = Color.DarkGray;
                        break;
                    case false:
                        dgPrices.CurrentCell.Value = "true";
                        entry.Enabled = true;
                        // entry.Banned = false;
                        dgPrices.CurrentRow.DefaultCellStyle.ForeColor = Color.Black;
                        break;
                }
                UpdateGrid();
            }

        }

        #endregion

        #region Timer events

        private void tmrPriceCheck_Tick(object sender, EventArgs e)
        {
            RunCycle();
            UpdateGrid();
        }

        private void tmrIdleCheck_Tick(object sender, EventArgs e)
        {
            IdleCheck_Tick();
        }

        private void IdleCheck_Tick()
        {
            if (checkBox1.Checked == true)
            {
                uint MSIdle = MinerControl.Idle.GetIdleTime();

                if (MSIdle < (MiningEngine._MinIdleSeconds * 1000))
                {
                    btnStop.PerformClick();
                }
                else
                {
                    if (MSIdle > (MiningEngine._MinIdleSeconds * 1000))
                    {
                        btnStart.PerformClick();
                    }

                }
            }
        }

        private void tmrExchangeUpdate_Tick(object sender, EventArgs e)
        {
            _engine.LoadExchangeRates();
        }

        private void tmrTimeUpdate_Tick(object sender, EventArgs e)
        {
            if (_engine.ExitTime.HasValue && DateTime.Now >= _engine.ExitTime) Application.Exit();

            UpdateTimes();

            if (_engine.PricesUpdated)
            {
                UpdateGrid();

                if (_engine.ShowHistory)
                {
                    HistoryChart historyChart = tabHistory.Controls["historyChart"] as HistoryChart;
                    historyChart?.UpdateChart(_engine.StatWindow, 3);
                    _totalHistoryForm?.UpdateChart();
                }

                _engine.PricesUpdated = false;
            }

            MiningModeEnum[] autoModes = { MiningModeEnum.Automatic, MiningModeEnum.Donation };
            if (!autoModes.Contains(_engine.MiningMode)) return;

            RunBestAlgo();
        }

        private void tmrPingNiceHash_Tick(object sender, EventArgs e)
        {
            IService service = _engine.Services.FirstOrDefault(s => s.ServiceName == "NiceHash");
            NiceHashService niceHash = service as NiceHashService;
            niceHash?.CheckPingTimes();
        }

        #endregion


        private void button1_Click(object sender, EventArgs e)
        {
            //button1.Click += new EventHandler(button1_Click);
            try
            {
                openFileDialog1.Title = "Choose config file";
                openFileDialog1.Filter = "Text files|*.conf";
                openFileDialog1.FileName = null;
                if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
                CFile = openFileDialog1.FileName.ToString();
            }

            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
            }

            if (CFile != null)
            {
                this.label6.Text = System.IO.Path.GetFileName(CFile);
                Refresh(CFile);
            }
        }

        private void Coloring()
        {
            CheckBoxUpdate();


            int _indCoin = 4; int _indDyn = 5; int _indHash = 6; int _indAcSpWrk = 7; int _indTopAvgSp = 8; int _indStatus = 24;
            int _indService = 2; // int _indAlgo = 3;
            //int _indMU = 9; int _indPlFee = 10; int _indPrice = 11; int _indEarn = 12;
            //int _indFees = 13; int _indPower = 14; int _indNet = 15; int _indAverage = 16;
            //int _indEarnCur = 17; int _indBalanceBTC = 18; int _indBalanceCoin = 19; int _indPendingCoin = 20;
            /*int _indAccept = 21; int _indReject = 22; int _indRunning = 23; int _indAction = 23;*/

            if (dgPrices.Columns.Contains("Status"))
                _indStatus = dgPrices.Columns.IndexOf(dgPrices.Columns["Status"]);
            if (dgPrices.Columns.Contains("Coin"))
                _indCoin = dgPrices.Columns.IndexOf(dgPrices.Columns["Coin"]);
            if (dgPrices.Columns.Contains("PriceDynamics"))
                _indDyn = dgPrices.Columns.IndexOf(dgPrices.Columns["PriceDynamics"]);
            if (dgPrices.Columns.Contains("Hashrate"))
                _indHash = dgPrices.Columns.IndexOf(dgPrices.Columns["Hashrate"]);
            if (dgPrices.Columns.Contains("AcSpWrk"))
                _indAcSpWrk = dgPrices.Columns.IndexOf(dgPrices.Columns["AcSpWrk"]);
            if (dgPrices.Columns.Contains("TopAvgSp"))
                _indTopAvgSp = dgPrices.Columns.IndexOf(dgPrices.Columns["TopAvgSp"]);

            if (MiningEngine._coloring)
            {

                string SN = string.Empty;
                foreach (DataGridViewRow row in dgPrices.Rows)
                {
                    row.Cells[_indAcSpWrk].Style.BackColor = Color.White; row.Cells[_indTopAvgSp].Style.BackColor = Color.White;

                    decimal arg1 = 0; decimal arg2 = 0; decimal arg3 = 0; decimal diff1 = 0; decimal diff2 = 0;

                    if (!string.IsNullOrEmpty(row.Cells[_indHash].Value.ToString())) arg1 = decimal.Parse(row.Cells[_indHash].Value.ToString());
                    if (!string.IsNullOrEmpty(row.Cells[_indAcSpWrk].Value.ToString())) arg2 = decimal.Parse(row.Cells[_indAcSpWrk].Value.ToString());
                    if (!string.IsNullOrEmpty(row.Cells[_indTopAvgSp].Value.ToString())) arg3 = decimal.Parse(row.Cells[_indTopAvgSp].Value.ToString());

                    if (arg1 != 0) diff1 = (arg2 - arg1) / arg1 * 100;
                    if (arg1 != 0) diff2 = (arg3 - arg1) / arg1 * 100;


                    if (diff1 <= -60 && arg2.ExtractDecimal() > 0) row.Cells[_indAcSpWrk].Style.BackColor = Color.HotPink;

                    if (diff1 >= -60 && diff1 <= -30 && arg2.ExtractDecimal() > 0) row.Cells[_indAcSpWrk].Style.BackColor = Color.Pink;

                    if (diff1 >= -30 && diff1 <= -5 && arg2.ExtractDecimal() > 0) row.Cells[_indAcSpWrk].Style.BackColor = Color.LightPink;

                    if (diff1 >= -5 && diff1 <= 5 && arg2.ExtractDecimal() > 0) row.Cells[_indAcSpWrk].Style.BackColor = Color.SpringGreen;

                    if (diff1 >= 5 && diff1 <= 30 && arg2.ExtractDecimal() > 0) row.Cells[_indAcSpWrk].Style.BackColor = Color.GreenYellow;

                    if (diff1 >= 30 && diff1 <= 60 && arg2.ExtractDecimal() > 0) row.Cells[_indAcSpWrk].Style.BackColor = Color.Yellow;

                    if (diff1 >= 60 && arg2.ExtractDecimal() > 0) row.Cells[_indAcSpWrk].Style.BackColor = Color.Gold;

                    if (diff2 <= -60 && arg3.ExtractDecimal() > 0) row.Cells[_indTopAvgSp].Style.BackColor = Color.HotPink;

                    if (diff2 >= -60 && diff2 <= -30 && arg3.ExtractDecimal() > 0) row.Cells[_indTopAvgSp].Style.BackColor = Color.Pink;

                    if (diff2 >= -30 && diff2 <= -5 && arg3.ExtractDecimal() > 0) row.Cells[_indTopAvgSp].Style.BackColor = Color.LightPink;

                    if (diff2 >= -5 && diff2 <= 5 && arg3.ExtractDecimal() > 0) row.Cells[_indTopAvgSp].Style.BackColor = Color.SpringGreen;

                    if (diff2 >= 5 && diff2 <= 30 && arg3.ExtractDecimal() > 0) row.Cells[_indTopAvgSp].Style.BackColor = Color.GreenYellow;

                    if (diff2 >= 30 && diff2 <= 60 && arg3.ExtractDecimal() > 0) row.Cells[_indTopAvgSp].Style.BackColor = Color.Yellow;

                    if (diff2 >= 60 && arg3.ExtractDecimal() > 0) row.Cells[_indTopAvgSp].Style.BackColor = Color.Gold;


                    if (row.Cells[_indStatus].FormattedValue.ToString() == "Running")
                    {
                        dgPrices.ClearSelection();
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                        row.Selected = true;
                        SN = row.Cells[_indService].Value.ToString();
                    }

                    if (row.Cells[_indStatus].FormattedValue.ToString() == "Lagging")
                    {
                        row.Cells[_indCoin].Style.BackColor = Color.LightBlue;
                    }

                    if ((row.Cells[_indStatus].FormattedValue).ToString() == "Pumping" || (row.Cells[_indStatus].FormattedValue).ToString() == "PumpReduction")
                    {
                        row.Cells[_indDyn].Style.BackColor = Color.LightGreen;
                    }
                    if ((row.Cells[_indStatus].FormattedValue).ToString() == "Dumping")
                    {
                        row.Cells[_indDyn].Style.BackColor = Color.LightPink;
                    }
                }
                foreach (DataGridViewRow row in dgServices.Rows)
                {
                    if (row.Cells[0].Value.ToString() == SN)
                    {
                        dgServices.ClearSelection();
                        //row.DefaultCellStyle.BackColor = Color.LightGray;
                        row.Selected = true;
                    }
                }
            }
        }

        private void HideColumns()
        {

            if (MiningEngine._hidecolumngr1)
            {
                if (dgPrices.Columns.Contains("Price")) dgPrices.Columns[dgPrices.Columns.IndexOf(dgPrices.Columns["Price"])].Visible = false;
                if (dgPrices.Columns.Contains("PowerCost")) dgPrices.Columns[dgPrices.Columns.IndexOf(dgPrices.Columns["PowerCost"])].Visible = false;
                if (dgPrices.Columns.Contains("Fees")) dgPrices.Columns[dgPrices.Columns.IndexOf(dgPrices.Columns["Fees"])].Visible = false;
            }

            if (MiningEngine._hidecolumngr2)
            {
                if (dgPrices.Columns.Contains("RejectSpeed")) dgPrices.Columns[dgPrices.Columns.IndexOf(dgPrices.Columns["RejectSpeed"])].Visible = false;
                if (dgPrices.Columns.Contains("TopAvgSp")) dgPrices.Columns[dgPrices.Columns.IndexOf(dgPrices.Columns["TopAvgSp"])].Visible = false;
                if (dgPrices.Columns.Contains("AcSpWrk")) dgPrices.Columns[dgPrices.Columns.IndexOf(dgPrices.Columns["AcSpWrk"])].Visible = false;
                if (dgPrices.Columns.Contains("MU")) dgPrices.Columns[dgPrices.Columns.IndexOf(dgPrices.Columns["MU"])].Visible = false;
                if (dgPrices.Columns.Contains("PoolFee")) dgPrices.Columns[dgPrices.Columns.IndexOf(dgPrices.Columns["PoolFee"])].Visible = false;
            }

            if (MiningEngine._hidecolumngr1)
                this.ClientSize = new System.Drawing.Size(1520, 616);

            if (MiningEngine._hidecolumngr2)
                this.ClientSize = new System.Drawing.Size(1350, 616);

            if (MiningEngine._hidecolumngr1 && MiningEngine._hidecolumngr2)
                this.ClientSize = new System.Drawing.Size(1170, 616);
            /*this.ClientSize = new System.Drawing.Size(1250, 616)*/
            ;

            if (!MiningEngine._hidecolumngr1 && !MiningEngine._hidecolumngr2)
                this.ClientSize = new System.Drawing.Size(1684, 616);
        }
    }
}


        