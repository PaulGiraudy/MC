namespace MinerControl
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            MinerControl.History.HistoryChart historyChart;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.tmrPriceCheck = new System.Windows.Forms.Timer(this.components);
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.btnReloadConfig = new System.Windows.Forms.Button();
            this.linkDonate = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label13 = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.gbTimes = new System.Windows.Forms.GroupBox();
            this.textRunningTotal = new System.Windows.Forms.TextBox();
            this.textTimeRestart = new System.Windows.Forms.TextBox();
            this.textTimeSwitch = new System.Windows.Forms.TextBox();
            this.textTimeCurrent = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblTimeCurrent = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tmrTimeUpdate = new System.Windows.Forms.Timer(this.components);
            this.dgPrices = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ServicePrint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Algo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Coin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceDynamics = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hashrate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AcSpWrk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TopAvgSp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PoolFee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Earn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fees = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PowerCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetEarn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetAverage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetEarnCUR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BalanceBTC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pending = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AcceptSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RejectSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeMining = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusPrint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnNhStart = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgServices = new System.Windows.Forms.DataGridView();
            this.ServiceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceLastUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServicePending = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceCurrency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceRunning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbDonation = new System.Windows.Forms.GroupBox();
            this.textDonationStart = new System.Windows.Forms.TextBox();
            this.textDonationEnd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gbCurrency = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblCurrencySymbol = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textCurrencyBalance = new System.Windows.Forms.TextBox();
            this.textCurrencyExchange = new System.Windows.Forms.TextBox();
            this.tmrExchangeUpdate = new System.Windows.Forms.Timer(this.components);
            this.tabPage = new System.Windows.Forms.TabControl();
            this.tabPrices = new System.Windows.Forms.TabPage();
            this.tabConsole = new System.Windows.Forms.TabPage();
            this.textConsole = new System.Windows.Forms.TextBox();
            this.tabRemote = new System.Windows.Forms.TabPage();
            this.textRemote = new System.Windows.Forms.TextBox();
            this.tabHistory = new System.Windows.Forms.TabPage();
            this.cmsDataGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmrPingNiceHash = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tmrIdleCheck = new System.Windows.Forms.Timer(this.components);
            historyChart = new MinerControl.History.HistoryChart();
            this.gbActions.SuspendLayout();
            this.gbTimes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPrices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgServices)).BeginInit();
            this.gbDonation.SuspendLayout();
            this.gbCurrency.SuspendLayout();
            this.tabPage.SuspendLayout();
            this.tabPrices.SuspendLayout();
            this.tabConsole.SuspendLayout();
            this.tabRemote.SuspendLayout();
            this.tabHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // historyChart
            // 
            historyChart.Dock = System.Windows.Forms.DockStyle.Fill;
            historyChart.FocussedSeries = new System.Windows.Forms.DataVisualization.Charting.Series[] {
        null,
        null};
            historyChart.Location = new System.Drawing.Point(3, 3);
            historyChart.Name = "historyChart";
            historyChart.Size = new System.Drawing.Size(1558, 389);
            historyChart.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(6, 17);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(50, 20);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Auto";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(6, 43);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(50, 20);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // tmrPriceCheck
            // 
            this.tmrPriceCheck.Interval = 60000;
            this.tmrPriceCheck.Tick += new System.EventHandler(this.tmrPriceCheck_Tick);
            // 
            // gbActions
            // 
            this.gbActions.Controls.Add(this.btnReloadConfig);
            this.gbActions.Controls.Add(this.btnStop);
            this.gbActions.Controls.Add(this.btnStart);
            this.gbActions.Location = new System.Drawing.Point(16, 31);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(62, 96);
            this.gbActions.TabIndex = 6;
            this.gbActions.TabStop = false;
            this.gbActions.Text = "Actions";
            // 
            // btnReloadConfig
            // 
            this.btnReloadConfig.Location = new System.Drawing.Point(6, 69);
            this.btnReloadConfig.Name = "btnReloadConfig";
            this.btnReloadConfig.Size = new System.Drawing.Size(50, 20);
            this.btnReloadConfig.TabIndex = 10;
            this.btnReloadConfig.Text = "Reload";
            this.btnReloadConfig.UseVisualStyleBackColor = true;
            this.btnReloadConfig.Click += new System.EventHandler(this.btnReloadConfig_Click);
            // 
            // linkDonate
            // 
            this.linkDonate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkDonate.AutoSize = true;
            this.linkDonate.LinkArea = new System.Windows.Forms.LinkArea(38, 34);
            this.linkDonate.Location = new System.Drawing.Point(274, 579);
            this.linkDonate.Name = "linkDonate";
            this.linkDonate.Size = new System.Drawing.Size(406, 17);
            this.linkDonate.TabIndex = 9;
            this.linkDonate.TabStop = true;
            this.linkDonate.Text = "Giraudy (Developer of current Mod) to 1NoCAhu4dYxi162srrKzi5qZiQrERuu7A4";
            this.linkDonate.UseCompatibleTextRendering = true;
            this.linkDonate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDonate_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(39, 34);
            this.linkLabel1.Location = new System.Drawing.Point(697, 579);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(440, 17);
            this.linkLabel1.TabIndex = 20;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "KBomba (Developer of previous Mods) to 1F9fsG5xLLn6uc7cdbgV1LTFWjWQSqXohX";
            this.linkLabel1.UseCompatibleTextRendering = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.LinkArea = new System.Windows.Forms.LinkArea(48, 34);
            this.linkLabel2.Location = new System.Drawing.Point(687, 600);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(450, 17);
            this.linkLabel2.TabIndex = 21;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "StuffOfInterest (Creator of the Application) to 1PMj3nrVq5CH4TXdJSnHHLPdvcXinjG72" +
    "y";
            this.linkLabel2.UseCompatibleTextRendering = true;
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(132, 13);
            this.label13.TabIndex = 10;
            this.label13.Text = "Miner Control v1.7.0 (Mod)";
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "Miner Control is still running\r\nDouble-click to restore";
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Miner Control";
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // gbTimes
            // 
            this.gbTimes.Controls.Add(this.textRunningTotal);
            this.gbTimes.Controls.Add(this.textTimeRestart);
            this.gbTimes.Controls.Add(this.textTimeSwitch);
            this.gbTimes.Controls.Add(this.textTimeCurrent);
            this.gbTimes.Controls.Add(this.label20);
            this.gbTimes.Controls.Add(this.label19);
            this.gbTimes.Controls.Add(this.lblTimeCurrent);
            this.gbTimes.Controls.Add(this.label18);
            this.gbTimes.Location = new System.Drawing.Point(84, 31);
            this.gbTimes.Name = "gbTimes";
            this.gbTimes.Size = new System.Drawing.Size(163, 96);
            this.gbTimes.TabIndex = 11;
            this.gbTimes.TabStop = false;
            this.gbTimes.Text = "Times";
            // 
            // textRunningTotal
            // 
            this.textRunningTotal.Location = new System.Drawing.Point(9, 32);
            this.textRunningTotal.Name = "textRunningTotal";
            this.textRunningTotal.ReadOnly = true;
            this.textRunningTotal.Size = new System.Drawing.Size(63, 20);
            this.textRunningTotal.TabIndex = 1;
            this.textRunningTotal.TabStop = false;
            this.textRunningTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textTimeRestart
            // 
            this.textTimeRestart.Location = new System.Drawing.Point(87, 70);
            this.textTimeRestart.Name = "textTimeRestart";
            this.textTimeRestart.ReadOnly = true;
            this.textTimeRestart.Size = new System.Drawing.Size(63, 20);
            this.textTimeRestart.TabIndex = 1;
            this.textTimeRestart.TabStop = false;
            this.textTimeRestart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textTimeSwitch
            // 
            this.textTimeSwitch.Location = new System.Drawing.Point(9, 70);
            this.textTimeSwitch.Name = "textTimeSwitch";
            this.textTimeSwitch.ReadOnly = true;
            this.textTimeSwitch.Size = new System.Drawing.Size(63, 20);
            this.textTimeSwitch.TabIndex = 1;
            this.textTimeSwitch.TabStop = false;
            this.textTimeSwitch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textTimeCurrent
            // 
            this.textTimeCurrent.Location = new System.Drawing.Point(87, 32);
            this.textTimeCurrent.Name = "textTimeCurrent";
            this.textTimeCurrent.ReadOnly = true;
            this.textTimeCurrent.Size = new System.Drawing.Size(63, 20);
            this.textTimeCurrent.TabIndex = 1;
            this.textTimeCurrent.TabStop = false;
            this.textTimeCurrent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(84, 54);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(41, 13);
            this.label20.TabIndex = 0;
            this.label20.Text = "Restart";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 54);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(39, 13);
            this.label19.TabIndex = 0;
            this.label19.Text = "Switch";
            // 
            // lblTimeCurrent
            // 
            this.lblTimeCurrent.AutoSize = true;
            this.lblTimeCurrent.Location = new System.Drawing.Point(84, 16);
            this.lblTimeCurrent.Name = "lblTimeCurrent";
            this.lblTimeCurrent.Size = new System.Drawing.Size(41, 13);
            this.lblTimeCurrent.TabIndex = 0;
            this.lblTimeCurrent.Text = "Current";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 16);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(47, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Running";
            // 
            // tmrTimeUpdate
            // 
            this.tmrTimeUpdate.Enabled = true;
            this.tmrTimeUpdate.Interval = 1000;
            this.tmrTimeUpdate.Tick += new System.EventHandler(this.tmrTimeUpdate_Tick);
            // 
            // dgPrices
            // 
            this.dgPrices.AllowUserToAddRows = false;
            this.dgPrices.AllowUserToDeleteRows = false;
            this.dgPrices.AllowUserToResizeColumns = false;
            this.dgPrices.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPrices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgPrices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPrices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Active,
            this.ServicePrint,
            this.Algo,
            this.Coin,
            this.PriceDynamics,
            this.Hashrate,
            this.AcSpWrk,
            this.TopAvgSp,
            this.MU,
            this.PoolFee,
            this.Price,
            this.Earn,
            this.Fees,
            this.PowerCost,
            this.NetEarn,
            this.NetAverage,
            this.NetEarnCUR,
            this.BalanceBTC,
            this.Balance,
            this.Pending,
            this.AcceptSpeed,
            this.RejectSpeed,
            this.TimeMining,
            this.StatusPrint,
            this.btnNhStart});
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgPrices.DefaultCellStyle = dataGridViewCellStyle19;
            this.dgPrices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgPrices.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgPrices.Location = new System.Drawing.Point(3, 3);
            this.dgPrices.Name = "dgPrices";
            this.dgPrices.ReadOnly = true;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPrices.RowHeadersDefaultCellStyle = dataGridViewCellStyle20;
            this.dgPrices.RowHeadersVisible = false;
            this.dgPrices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgPrices.ShowCellErrors = false;
            this.dgPrices.ShowEditingIcon = false;
            this.dgPrices.ShowRowErrors = false;
            this.dgPrices.Size = new System.Drawing.Size(1558, 389);
            this.dgPrices.TabIndex = 12;
            this.dgPrices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgPrices_CellContentClick);
            this.dgPrices.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgPrices_CellMouseClick);
            this.dgPrices.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgPrices_CellMouseDoubleClick);
            this.dgPrices.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgPrices_ColumnHeaderMouseClick);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // Active
            // 
            this.Active.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Active.HeaderText = "Act";
            this.Active.MinimumWidth = 25;
            this.Active.Name = "Active";
            this.Active.ReadOnly = true;
            this.Active.ToolTipText = "Enabled/disabled";
            this.Active.TrueValue = "True";
            this.Active.Width = 25;
            // 
            // ServicePrint
            // 
            this.ServicePrint.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ServicePrint.DataPropertyName = "ServicePrint";
            this.ServicePrint.HeaderText = "Service";
            this.ServicePrint.MinimumWidth = 90;
            this.ServicePrint.Name = "ServicePrint";
            this.ServicePrint.ReadOnly = true;
            this.ServicePrint.ToolTipText = "Pool Name";
            // 
            // Algo
            // 
            this.Algo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Algo.DataPropertyName = "Name";
            this.Algo.HeaderText = "Algo";
            this.Algo.MinimumWidth = 100;
            this.Algo.Name = "Algo";
            this.Algo.ReadOnly = true;
            this.Algo.ToolTipText = "Algorithm";
            // 
            // Coin
            // 
            this.Coin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Coin.DataPropertyName = "CoinName";
            this.Coin.HeaderText = "Coin";
            this.Coin.MinimumWidth = 100;
            this.Coin.Name = "Coin";
            this.Coin.ReadOnly = true;
            this.Coin.ToolTipText = "Coin Name";
            // 
            // PriceDynamics
            // 
            this.PriceDynamics.DataPropertyName = "PriceDynamics";
            this.PriceDynamics.HeaderText = "Dyn(Pr/ExR)";
            this.PriceDynamics.Name = "PriceDynamics";
            this.PriceDynamics.ReadOnly = true;
            this.PriceDynamics.ToolTipText = "Price daily dynamics (Price/Exchange Rate)";
            this.PriceDynamics.Width = 80;
            // 
            // Hashrate
            // 
            this.Hashrate.DataPropertyName = "HashratePrint";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Hashrate.DefaultCellStyle = dataGridViewCellStyle2;
            this.Hashrate.HeaderText = "Hashrate";
            this.Hashrate.MinimumWidth = 90;
            this.Hashrate.Name = "Hashrate";
            this.Hashrate.ReadOnly = true;
            this.Hashrate.ToolTipText = "User declared hashrate kH/s";
            this.Hashrate.Width = 90;
            // 
            // AcSpWrk
            // 
            this.AcSpWrk.DataPropertyName = "AcSpWrkPrint";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.NullValue = null;
            this.AcSpWrk.DefaultCellStyle = dataGridViewCellStyle3;
            this.AcSpWrk.HeaderText = "AcSpWrk";
            this.AcSpWrk.MinimumWidth = 90;
            this.AcSpWrk.Name = "AcSpWrk";
            this.AcSpWrk.ReadOnly = true;
            this.AcSpWrk.ToolTipText = "Worker speed kH/s";
            this.AcSpWrk.Width = 90;
            // 
            // TopAvgSp
            // 
            this.TopAvgSp.DataPropertyName = "TopAvgSpPrint";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.NullValue = null;
            this.TopAvgSp.DefaultCellStyle = dataGridViewCellStyle4;
            this.TopAvgSp.HeaderText = "TopAvgSp";
            this.TopAvgSp.MinimumWidth = 90;
            this.TopAvgSp.Name = "TopAvgSp";
            this.TopAvgSp.ReadOnly = true;
            this.TopAvgSp.ToolTipText = "Top worker speed kH/s";
            this.TopAvgSp.Width = 90;
            // 
            // MU
            // 
            this.MU.DataPropertyName = "MU";
            this.MU.HeaderText = "MU";
            this.MU.MinimumWidth = 40;
            this.MU.Name = "MU";
            this.MU.ReadOnly = true;
            this.MU.ToolTipText = "Measuring unit";
            this.MU.Width = 40;
            // 
            // PoolFee
            // 
            this.PoolFee.DataPropertyName = "PoolFeePrint";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.NullValue = null;
            this.PoolFee.DefaultCellStyle = dataGridViewCellStyle5;
            this.PoolFee.HeaderText = "PlFee";
            this.PoolFee.MinimumWidth = 40;
            this.PoolFee.Name = "PoolFee";
            this.PoolFee.ReadOnly = true;
            this.PoolFee.ToolTipText = "Pool fee %";
            this.PoolFee.Width = 40;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N6";
            dataGridViewCellStyle6.NullValue = null;
            this.Price.DefaultCellStyle = dataGridViewCellStyle6;
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            this.Price.ToolTipText = "Calculated profit BTC for 1GH per day";
            this.Price.Width = 60;
            // 
            // Earn
            // 
            this.Earn.DataPropertyName = "Earn";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N6";
            dataGridViewCellStyle7.NullValue = null;
            this.Earn.DefaultCellStyle = dataGridViewCellStyle7;
            this.Earn.HeaderText = "Earn";
            this.Earn.MinimumWidth = 50;
            this.Earn.Name = "Earn";
            this.Earn.ReadOnly = true;
            this.Earn.ToolTipText = "Possible earn for user\'s hashrate BTC per day";
            this.Earn.Width = 60;
            // 
            // Fees
            // 
            this.Fees.DataPropertyName = "Fees";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N6";
            this.Fees.DefaultCellStyle = dataGridViewCellStyle8;
            this.Fees.HeaderText = "Fees";
            this.Fees.MinimumWidth = 50;
            this.Fees.Name = "Fees";
            this.Fees.ReadOnly = true;
            this.Fees.ToolTipText = "Fees BTC per day";
            this.Fees.Width = 60;
            // 
            // PowerCost
            // 
            this.PowerCost.DataPropertyName = "PowerCost";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N6";
            dataGridViewCellStyle9.NullValue = null;
            this.PowerCost.DefaultCellStyle = dataGridViewCellStyle9;
            this.PowerCost.HeaderText = "Power";
            this.PowerCost.Name = "PowerCost";
            this.PowerCost.ReadOnly = true;
            this.PowerCost.ToolTipText = "Power costs BTC per day";
            this.PowerCost.Width = 60;
            // 
            // NetEarn
            // 
            this.NetEarn.DataPropertyName = "NetEarn";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N6";
            dataGridViewCellStyle10.NullValue = null;
            this.NetEarn.DefaultCellStyle = dataGridViewCellStyle10;
            this.NetEarn.HeaderText = "Net";
            this.NetEarn.MinimumWidth = 50;
            this.NetEarn.Name = "NetEarn";
            this.NetEarn.ReadOnly = true;
            this.NetEarn.ToolTipText = "Net earn BTC per day";
            this.NetEarn.Width = 60;
            // 
            // NetAverage
            // 
            this.NetAverage.DataPropertyName = "NetAverage";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N6";
            this.NetAverage.DefaultCellStyle = dataGridViewCellStyle11;
            this.NetAverage.HeaderText = "Average";
            this.NetAverage.MinimumWidth = 50;
            this.NetAverage.Name = "NetAverage";
            this.NetAverage.ReadOnly = true;
            this.NetAverage.ToolTipText = "Average earn BTC per day";
            this.NetAverage.Width = 60;
            // 
            // NetEarnCUR
            // 
            this.NetEarnCUR.DataPropertyName = "NetEarnCURPrint";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.NullValue = null;
            this.NetEarnCUR.DefaultCellStyle = dataGridViewCellStyle12;
            this.NetEarnCUR.HeaderText = "NetCur";
            this.NetEarnCUR.MinimumWidth = 55;
            this.NetEarnCUR.Name = "NetEarnCUR";
            this.NetEarnCUR.ReadOnly = true;
            this.NetEarnCUR.ToolTipText = "Net earn in selected currency per day";
            this.NetEarnCUR.Width = 55;
            // 
            // BalanceBTC
            // 
            this.BalanceBTC.DataPropertyName = "BalanceBTCPrint";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.NullValue = null;
            this.BalanceBTC.DefaultCellStyle = dataGridViewCellStyle13;
            this.BalanceBTC.HeaderText = "BalanceBTC";
            this.BalanceBTC.MinimumWidth = 70;
            this.BalanceBTC.Name = "BalanceBTC";
            this.BalanceBTC.ReadOnly = true;
            this.BalanceBTC.ToolTipText = "Balance in BTC";
            this.BalanceBTC.Width = 70;
            // 
            // Balance
            // 
            this.Balance.DataPropertyName = "BalancePrint";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.NullValue = null;
            this.Balance.DefaultCellStyle = dataGridViewCellStyle14;
            this.Balance.HeaderText = "Balance";
            this.Balance.MinimumWidth = 50;
            this.Balance.Name = "Balance";
            this.Balance.ReadOnly = true;
            this.Balance.ToolTipText = "Balance in mined coins";
            this.Balance.Width = 50;
            // 
            // Pending
            // 
            this.Pending.DataPropertyName = "PendingPrint";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.NullValue = null;
            this.Pending.DefaultCellStyle = dataGridViewCellStyle15;
            this.Pending.HeaderText = "Pending";
            this.Pending.MinimumWidth = 50;
            this.Pending.Name = "Pending";
            this.Pending.ReadOnly = true;
            this.Pending.ToolTipText = "Pending balance in coins";
            this.Pending.Width = 50;
            // 
            // AcceptSpeed
            // 
            this.AcceptSpeed.DataPropertyName = "AcceptSpeedPrint";
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.NullValue = null;
            this.AcceptSpeed.DefaultCellStyle = dataGridViewCellStyle16;
            this.AcceptSpeed.HeaderText = "Accept";
            this.AcceptSpeed.MinimumWidth = 90;
            this.AcceptSpeed.Name = "AcceptSpeed";
            this.AcceptSpeed.ReadOnly = true;
            this.AcceptSpeed.ToolTipText = "Total accepted speed";
            this.AcceptSpeed.Width = 90;
            // 
            // RejectSpeed
            // 
            this.RejectSpeed.DataPropertyName = "RejectSpeedPrint";
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle17.NullValue = null;
            this.RejectSpeed.DefaultCellStyle = dataGridViewCellStyle17;
            this.RejectSpeed.HeaderText = "Reject";
            this.RejectSpeed.MinimumWidth = 90;
            this.RejectSpeed.Name = "RejectSpeed";
            this.RejectSpeed.ReadOnly = true;
            this.RejectSpeed.ToolTipText = "Rejected speed";
            this.RejectSpeed.Width = 90;
            // 
            // TimeMining
            // 
            this.TimeMining.DataPropertyName = "TimeMiningPrint";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.TimeMining.DefaultCellStyle = dataGridViewCellStyle18;
            this.TimeMining.HeaderText = "Running";
            this.TimeMining.MinimumWidth = 50;
            this.TimeMining.Name = "TimeMining";
            this.TimeMining.ReadOnly = true;
            this.TimeMining.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TimeMining.ToolTipText = "Running time";
            this.TimeMining.Width = 70;
            // 
            // StatusPrint
            // 
            this.StatusPrint.DataPropertyName = "StatusPrint";
            this.StatusPrint.HeaderText = "Status";
            this.StatusPrint.MinimumWidth = 50;
            this.StatusPrint.Name = "StatusPrint";
            this.StatusPrint.ReadOnly = true;
            this.StatusPrint.ToolTipText = "Status";
            this.StatusPrint.Width = 55;
            // 
            // btnNhStart
            // 
            this.btnNhStart.HeaderText = "Action";
            this.btnNhStart.MinimumWidth = 55;
            this.btnNhStart.Name = "btnNhStart";
            this.btnNhStart.ReadOnly = true;
            this.btnNhStart.Text = "Start";
            this.btnNhStart.ToolTipText = "Action";
            this.btnNhStart.UseColumnTextForButtonValue = true;
            this.btnNhStart.Width = 55;
            // 
            // dgServices
            // 
            this.dgServices.AllowUserToAddRows = false;
            this.dgServices.AllowUserToDeleteRows = false;
            this.dgServices.AllowUserToResizeColumns = false;
            this.dgServices.AllowUserToResizeRows = false;
            this.dgServices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgServices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle21;
            this.dgServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgServices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ServiceName,
            this.ServiceLastUpdated,
            this.ServicePending,
            this.ServiceBalance,
            this.ServiceCurrency,
            this.ServiceRunning});
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle27.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgServices.DefaultCellStyle = dataGridViewCellStyle27;
            this.dgServices.Location = new System.Drawing.Point(458, 9);
            this.dgServices.Name = "dgServices";
            this.dgServices.ReadOnly = true;
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle28.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle28.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle28.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle28.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle28.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgServices.RowHeadersDefaultCellStyle = dataGridViewCellStyle28;
            this.dgServices.RowHeadersVisible = false;
            this.dgServices.ShowCellErrors = false;
            this.dgServices.ShowEditingIcon = false;
            this.dgServices.ShowRowErrors = false;
            this.dgServices.Size = new System.Drawing.Size(1119, 140);
            this.dgServices.TabIndex = 13;
            // 
            // ServiceName
            // 
            this.ServiceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ServiceName.DataPropertyName = "ServicePrint";
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ServiceName.DefaultCellStyle = dataGridViewCellStyle22;
            this.ServiceName.HeaderText = "Service";
            this.ServiceName.Name = "ServiceName";
            this.ServiceName.ReadOnly = true;
            this.ServiceName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ServiceName.ToolTipText = "Pool Name";
            // 
            // ServiceLastUpdated
            // 
            this.ServiceLastUpdated.DataPropertyName = "LastUpdatedPrint";
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ServiceLastUpdated.DefaultCellStyle = dataGridViewCellStyle23;
            this.ServiceLastUpdated.HeaderText = "Updated";
            this.ServiceLastUpdated.Name = "ServiceLastUpdated";
            this.ServiceLastUpdated.ReadOnly = true;
            this.ServiceLastUpdated.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ServicePending
            // 
            this.ServicePending.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ServicePending.DataPropertyName = "ServicePendingPrint";
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ServicePending.DefaultCellStyle = dataGridViewCellStyle24;
            this.ServicePending.HeaderText = "Pending BTC";
            this.ServicePending.Name = "ServicePending";
            this.ServicePending.ReadOnly = true;
            this.ServicePending.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ServicePending.ToolTipText = "Pool\'s pending balance in BTC";
            // 
            // ServiceBalance
            // 
            this.ServiceBalance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ServiceBalance.DataPropertyName = "ServiceBalancePrint";
            this.ServiceBalance.HeaderText = "Balance BTC";
            this.ServiceBalance.Name = "ServiceBalance";
            this.ServiceBalance.ReadOnly = true;
            this.ServiceBalance.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ServiceBalance.ToolTipText = "Pool\'s earned balance in BTC";
            // 
            // ServiceCurrency
            // 
            this.ServiceCurrency.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ServiceCurrency.DataPropertyName = "CurrencyPrint";
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ServiceCurrency.DefaultCellStyle = dataGridViewCellStyle25;
            this.ServiceCurrency.HeaderText = "Balance ";
            this.ServiceCurrency.Name = "ServiceCurrency";
            this.ServiceCurrency.ReadOnly = true;
            this.ServiceCurrency.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ServiceCurrency.ToolTipText = "Pool\'s earned balance in selected currency";
            // 
            // ServiceRunning
            // 
            this.ServiceRunning.DataPropertyName = "TimeMiningPrint";
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ServiceRunning.DefaultCellStyle = dataGridViewCellStyle26;
            this.ServiceRunning.HeaderText = "Running";
            this.ServiceRunning.Name = "ServiceRunning";
            this.ServiceRunning.ReadOnly = true;
            this.ServiceRunning.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ServiceRunning.ToolTipText = "Running time";
            this.ServiceRunning.Width = 65;
            // 
            // gbDonation
            // 
            this.gbDonation.Controls.Add(this.textDonationStart);
            this.gbDonation.Controls.Add(this.textDonationEnd);
            this.gbDonation.Controls.Add(this.label1);
            this.gbDonation.Controls.Add(this.label2);
            this.gbDonation.Location = new System.Drawing.Point(254, 31);
            this.gbDonation.Name = "gbDonation";
            this.gbDonation.Size = new System.Drawing.Size(76, 96);
            this.gbDonation.TabIndex = 14;
            this.gbDonation.TabStop = false;
            this.gbDonation.Text = "Donation";
            // 
            // textDonationStart
            // 
            this.textDonationStart.Location = new System.Drawing.Point(6, 32);
            this.textDonationStart.Name = "textDonationStart";
            this.textDonationStart.ReadOnly = true;
            this.textDonationStart.Size = new System.Drawing.Size(63, 20);
            this.textDonationStart.TabIndex = 1;
            this.textDonationStart.TabStop = false;
            this.textDonationStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textDonationEnd
            // 
            this.textDonationEnd.Location = new System.Drawing.Point(6, 70);
            this.textDonationEnd.Name = "textDonationEnd";
            this.textDonationEnd.ReadOnly = true;
            this.textDonationEnd.Size = new System.Drawing.Size(63, 20);
            this.textDonationEnd.TabIndex = 1;
            this.textDonationEnd.TabStop = false;
            this.textDonationEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Time Until";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Mining Time";
            // 
            // gbCurrency
            // 
            this.gbCurrency.Controls.Add(this.label5);
            this.gbCurrency.Controls.Add(this.label4);
            this.gbCurrency.Controls.Add(this.lblCurrencySymbol);
            this.gbCurrency.Controls.Add(this.label3);
            this.gbCurrency.Controls.Add(this.textCurrencyBalance);
            this.gbCurrency.Controls.Add(this.textCurrencyExchange);
            this.gbCurrency.Location = new System.Drawing.Point(336, 31);
            this.gbCurrency.Name = "gbCurrency";
            this.gbCurrency.Size = new System.Drawing.Size(107, 96);
            this.gbCurrency.TabIndex = 15;
            this.gbCurrency.TabStop = false;
            this.gbCurrency.Text = "Currency";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(73, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "USD";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Balance";
            // 
            // lblCurrencySymbol
            // 
            this.lblCurrencySymbol.AutoSize = true;
            this.lblCurrencySymbol.Location = new System.Drawing.Point(73, 74);
            this.lblCurrencySymbol.Name = "lblCurrencySymbol";
            this.lblCurrencySymbol.Size = new System.Drawing.Size(30, 13);
            this.lblCurrencySymbol.TabIndex = 0;
            this.lblCurrencySymbol.Text = "USD";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Exchange";
            // 
            // textCurrencyBalance
            // 
            this.textCurrencyBalance.Location = new System.Drawing.Point(9, 70);
            this.textCurrencyBalance.Name = "textCurrencyBalance";
            this.textCurrencyBalance.ReadOnly = true;
            this.textCurrencyBalance.Size = new System.Drawing.Size(63, 20);
            this.textCurrencyBalance.TabIndex = 1;
            this.textCurrencyBalance.TabStop = false;
            this.textCurrencyBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textCurrencyExchange
            // 
            this.textCurrencyExchange.Location = new System.Drawing.Point(9, 33);
            this.textCurrencyExchange.Name = "textCurrencyExchange";
            this.textCurrencyExchange.ReadOnly = true;
            this.textCurrencyExchange.Size = new System.Drawing.Size(63, 20);
            this.textCurrencyExchange.TabIndex = 1;
            this.textCurrencyExchange.TabStop = false;
            this.textCurrencyExchange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tmrExchangeUpdate
            // 
            this.tmrExchangeUpdate.Interval = 1800000;
            this.tmrExchangeUpdate.Tick += new System.EventHandler(this.tmrExchangeUpdate_Tick);
            // 
            // tabPage
            // 
            this.tabPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPage.Controls.Add(this.tabPrices);
            this.tabPage.Controls.Add(this.tabConsole);
            this.tabPage.Controls.Add(this.tabRemote);
            this.tabPage.Controls.Add(this.tabHistory);
            this.tabPage.Location = new System.Drawing.Point(12, 155);
            this.tabPage.Name = "tabPage";
            this.tabPage.SelectedIndex = 0;
            this.tabPage.Size = new System.Drawing.Size(1572, 421);
            this.tabPage.TabIndex = 16;
            // 
            // tabPrices
            // 
            this.tabPrices.Controls.Add(this.dgPrices);
            this.tabPrices.Location = new System.Drawing.Point(4, 22);
            this.tabPrices.Name = "tabPrices";
            this.tabPrices.Padding = new System.Windows.Forms.Padding(3);
            this.tabPrices.Size = new System.Drawing.Size(1564, 395);
            this.tabPrices.TabIndex = 0;
            this.tabPrices.Text = "Prices";
            this.tabPrices.UseVisualStyleBackColor = true;
            // 
            // tabConsole
            // 
            this.tabConsole.Controls.Add(this.textConsole);
            this.tabConsole.Location = new System.Drawing.Point(4, 22);
            this.tabConsole.Name = "tabConsole";
            this.tabConsole.Padding = new System.Windows.Forms.Padding(3);
            this.tabConsole.Size = new System.Drawing.Size(1564, 395);
            this.tabConsole.TabIndex = 1;
            this.tabConsole.Text = "Console";
            this.tabConsole.UseVisualStyleBackColor = true;
            // 
            // textConsole
            // 
            this.textConsole.BackColor = System.Drawing.Color.Black;
            this.textConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textConsole.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textConsole.ForeColor = System.Drawing.Color.White;
            this.textConsole.Location = new System.Drawing.Point(3, 3);
            this.textConsole.Multiline = true;
            this.textConsole.Name = "textConsole";
            this.textConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textConsole.Size = new System.Drawing.Size(1558, 389);
            this.textConsole.TabIndex = 0;
            // 
            // tabRemote
            // 
            this.tabRemote.Controls.Add(this.textRemote);
            this.tabRemote.Location = new System.Drawing.Point(4, 22);
            this.tabRemote.Name = "tabRemote";
            this.tabRemote.Padding = new System.Windows.Forms.Padding(3);
            this.tabRemote.Size = new System.Drawing.Size(1564, 395);
            this.tabRemote.TabIndex = 2;
            this.tabRemote.Text = "Remote";
            this.tabRemote.UseVisualStyleBackColor = true;
            // 
            // textRemote
            // 
            this.textRemote.BackColor = System.Drawing.Color.Black;
            this.textRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textRemote.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textRemote.ForeColor = System.Drawing.Color.White;
            this.textRemote.Location = new System.Drawing.Point(3, 3);
            this.textRemote.Multiline = true;
            this.textRemote.Name = "textRemote";
            this.textRemote.ReadOnly = true;
            this.textRemote.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textRemote.Size = new System.Drawing.Size(1558, 389);
            this.textRemote.TabIndex = 1;
            // 
            // tabHistory
            // 
            this.tabHistory.Controls.Add(historyChart);
            this.tabHistory.Location = new System.Drawing.Point(4, 22);
            this.tabHistory.Name = "tabHistory";
            this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabHistory.Size = new System.Drawing.Size(1564, 395);
            this.tabHistory.TabIndex = 3;
            this.tabHistory.Text = "History";
            this.tabHistory.UseVisualStyleBackColor = true;
            // 
            // cmsDataGrid
            // 
            this.cmsDataGrid.Name = "cmsDataGrid";
            this.cmsDataGrid.Size = new System.Drawing.Size(61, 4);
            // 
            // tmrPingNiceHash
            // 
            this.tmrPingNiceHash.Interval = 21600000;
            this.tmrPingNiceHash.Tick += new System.EventHandler(this.tmrPingNiceHash_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label6
            // 
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label6.Location = new System.Drawing.Point(200, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(169, 25);
            this.label6.TabIndex = 18;
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(375, 133);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 25);
            this.button1.TabIndex = 19;
            this.button1.Text = "Config file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // linkLabel3
            // 
            this.linkLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.linkLabel3.LinkColor = System.Drawing.Color.Black;
            this.linkLabel3.Location = new System.Drawing.Point(19, 579);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(219, 13);
            this.linkLabel3.TabIndex = 22;
            this.linkLabel3.Text = "If you like this, please donate some mBTC to ";
            // 
            // linkLabel4
            // 
            this.linkLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.LinkArea = new System.Windows.Forms.LinkArea(42, 34);
            this.linkLabel4.Location = new System.Drawing.Point(240, 600);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(441, 17);
            this.linkLabel4.TabIndex = 23;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "Fuzzbawls (Developer of previous Mods) to 1C5X7k7gt9KteVaWrfp1nN8MguivTK2S1X";
            this.linkLabel4.UseCompatibleTextRendering = true;
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(22, 134);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 17);
            this.checkBox1.TabIndex = 24;
            this.checkBox1.Text = "IdleMining";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // tmrIdleCheck
            // 
            this.tmrIdleCheck.Interval = 1000;
            this.tmrIdleCheck.Tick += new System.EventHandler(this.tmrIdleCheck_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1596, 616);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.linkLabel4);
            this.Controls.Add(this.linkLabel3);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tabPage);
            this.Controls.Add(this.gbCurrency);
            this.Controls.Add(this.gbDonation);
            this.Controls.Add(this.dgServices);
            this.Controls.Add(this.gbTimes);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.linkDonate);
            this.Controls.Add(this.gbActions);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Miner Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.gbActions.ResumeLayout(false);
            this.gbTimes.ResumeLayout(false);
            this.gbTimes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPrices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgServices)).EndInit();
            this.gbDonation.ResumeLayout(false);
            this.gbDonation.PerformLayout();
            this.gbCurrency.ResumeLayout(false);
            this.gbCurrency.PerformLayout();
            this.tabPage.ResumeLayout(false);
            this.tabPrices.ResumeLayout(false);
            this.tabConsole.ResumeLayout(false);
            this.tabConsole.PerformLayout();
            this.tabRemote.ResumeLayout(false);
            this.tabRemote.PerformLayout();
            this.tabHistory.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Timer tmrPriceCheck;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.LinkLabel linkDonate;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.GroupBox gbTimes;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Timer tmrTimeUpdate;
        private System.Windows.Forms.TextBox textRunningTotal;
        private System.Windows.Forms.TextBox textTimeCurrent;
        private System.Windows.Forms.Label lblTimeCurrent;
        private System.Windows.Forms.TextBox textTimeRestart;
        private System.Windows.Forms.TextBox textTimeSwitch;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.DataGridView dgPrices;
        private System.Windows.Forms.DataGridView dgServices;
        private System.Windows.Forms.GroupBox gbDonation;
        private System.Windows.Forms.TextBox textDonationStart;
        private System.Windows.Forms.TextBox textDonationEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbCurrency;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textCurrencyExchange;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textCurrencyBalance;
        private System.Windows.Forms.Timer tmrExchangeUpdate;
        private System.Windows.Forms.Label lblCurrencySymbol;
        private System.Windows.Forms.TabControl tabPage;
        private System.Windows.Forms.TabPage tabPrices;
        private System.Windows.Forms.TabPage tabConsole;
        public System.Windows.Forms.TextBox textConsole;
        private System.Windows.Forms.TabPage tabRemote;
        private System.Windows.Forms.TextBox textRemote;
        private System.Windows.Forms.Button btnReloadConfig;
        private System.Windows.Forms.TabPage tabHistory;
        private System.Windows.Forms.ContextMenuStrip cmsDataGrid;
        private System.Windows.Forms.Timer tmrPingNiceHash;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Timer tmrIdleCheck;
        public System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceLastUpdated;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServicePending;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceCurrency;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceRunning;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Active;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServicePrint;
        private System.Windows.Forms.DataGridViewTextBoxColumn Algo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Coin;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceDynamics;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hashrate;
        private System.Windows.Forms.DataGridViewTextBoxColumn AcSpWrk;
        private System.Windows.Forms.DataGridViewTextBoxColumn TopAvgSp;
        private System.Windows.Forms.DataGridViewTextBoxColumn MU;
        private System.Windows.Forms.DataGridViewTextBoxColumn PoolFee;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Earn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fees;
        private System.Windows.Forms.DataGridViewTextBoxColumn PowerCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn NetEarn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NetAverage;
        private System.Windows.Forms.DataGridViewTextBoxColumn NetEarnCUR;
        private System.Windows.Forms.DataGridViewTextBoxColumn BalanceBTC;
        private System.Windows.Forms.DataGridViewTextBoxColumn Balance;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pending;
        private System.Windows.Forms.DataGridViewTextBoxColumn AcceptSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn RejectSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeMining;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusPrint;
        private System.Windows.Forms.DataGridViewButtonColumn btnNhStart;
        private System.Windows.Forms.Label label5;
    }
}

