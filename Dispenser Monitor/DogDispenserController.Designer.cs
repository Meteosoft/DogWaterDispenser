namespace DispenserController
{
    partial class DogDispenserController
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DogDispenserController));
            this.zedGraphDispenserLevel = new ZedGraph.ZedGraphControl();
            this.logMessageView = new Meteosoft.MessageLogger.LogMessageView();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelInlet = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripLedBulbInlet = new Meteosoft.ControllerClient.ToolStripLedBulb();
            this.toolStripStatusLabelGap = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelOutlet = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripLedBulbOutlet = new Meteosoft.ControllerClient.ToolStripLedBulb();
            this.toolStripStatusLabelDateTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerWhole = new System.Windows.Forms.SplitContainer();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.panel = new System.Windows.Forms.Panel();
            this.checkSendRawValues = new System.Windows.Forms.CheckBox();
            this.checkAllowFlushing = new System.Windows.Forms.CheckBox();
            this.labelSec2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericMaxSensorValue = new System.Windows.Forms.NumericUpDown();
            this.labelMaxSensorValue = new System.Windows.Forms.Label();
            this.numericMinSensorValue = new System.Windows.Forms.NumericUpDown();
            this.labelMinSensorValue = new System.Windows.Forms.Label();
            this.buttonSyncSettings = new System.Windows.Forms.Button();
            this.labelSec = new System.Windows.Forms.Label();
            this.labelReadInterval = new System.Windows.Forms.Label();
            this.numericReadInterval = new System.Windows.Forms.NumericUpDown();
            this.textEthernetIP = new System.Windows.Forms.TextBox();
            this.labelIP = new System.Windows.Forms.Label();
            this.buttonSetTime = new System.Windows.Forms.Button();
            this.groupBoxWaterLevelMarks = new System.Windows.Forms.GroupBox();
            this.numericHighLevelMark = new System.Windows.Forms.NumericUpDown();
            this.labelHighLevelMark = new System.Windows.Forms.Label();
            this.numericLowLevelMark = new System.Windows.Forms.NumericUpDown();
            this.labelLowLevelMark = new System.Windows.Forms.Label();
            this.buttonFlush = new System.Windows.Forms.Button();
            this.labelCurrentLevel = new System.Windows.Forms.Label();
            this.labelFlushMinute = new System.Windows.Forms.Label();
            this.labelCurrentLevelLabel = new System.Windows.Forms.Label();
            this.comboComPorts = new System.Windows.Forms.ComboBox();
            this.checkFastReadings = new System.Windows.Forms.CheckBox();
            this.checkSendReadings = new System.Windows.Forms.CheckBox();
            this.numericFlushMinute = new System.Windows.Forms.NumericUpDown();
            this.checkOutlet = new System.Windows.Forms.CheckBox();
            this.labelComPorts = new System.Windows.Forms.Label();
            this.checkInlet = new System.Windows.Forms.CheckBox();
            this.labelFlushHour = new System.Windows.Forms.Label();
            this.numericFlushHour = new System.Windows.Forms.NumericUpDown();
            this.numericFlushPeriod = new System.Windows.Forms.NumericUpDown();
            this.labelFlushPeriod = new System.Windows.Forms.Label();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWhole)).BeginInit();
            this.splitContainerWhole.Panel1.SuspendLayout();
            this.splitContainerWhole.Panel2.SuspendLayout();
            this.splitContainerWhole.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).BeginInit();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            this.panel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericMaxSensorValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericMinSensorValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericReadInterval)).BeginInit();
            this.groupBoxWaterLevelMarks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericHighLevelMark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLowLevelMark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFlushMinute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFlushHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFlushPeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // zedGraphDispenserLevel
            // 
            this.zedGraphDispenserLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphDispenserLevel.Location = new System.Drawing.Point(0, 0);
            this.zedGraphDispenserLevel.Name = "zedGraphDispenserLevel";
            this.zedGraphDispenserLevel.ScrollGrace = 0D;
            this.zedGraphDispenserLevel.ScrollMaxX = 0D;
            this.zedGraphDispenserLevel.ScrollMaxY = 0D;
            this.zedGraphDispenserLevel.ScrollMaxY2 = 0D;
            this.zedGraphDispenserLevel.ScrollMinX = 0D;
            this.zedGraphDispenserLevel.ScrollMinY = 0D;
            this.zedGraphDispenserLevel.ScrollMinY2 = 0D;
            this.zedGraphDispenserLevel.Size = new System.Drawing.Size(398, 223);
            this.zedGraphDispenserLevel.TabIndex = 0;
            // 
            // logMessageView
            // 
            this.logMessageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logMessageView.FileNameAppendage = null;
            this.logMessageView.IncludeDateTime = true;
            this.logMessageView.Location = new System.Drawing.Point(0, 0);
            this.logMessageView.MaxLogLines = 2000;
            this.logMessageView.Name = "logMessageView";
            this.logMessageView.Size = new System.Drawing.Size(387, 384);
            this.logMessageView.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelStatus,
            this.toolStripStatusLabelInlet,
            this.toolStripLedBulbInlet,
            this.toolStripStatusLabelGap,
            this.toolStripStatusLabelOutlet,
            this.toolStripLedBulbOutlet,
            this.toolStripStatusLabelDateTime});
            this.statusStrip.Location = new System.Drawing.Point(0, 395);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(801, 27);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "StatusBar";
            // 
            // toolStripStatusLabelStatus
            // 
            this.toolStripStatusLabelStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabelStatus.Name = "toolStripStatusLabelStatus";
            this.toolStripStatusLabelStatus.Size = new System.Drawing.Size(96, 22);
            this.toolStripStatusLabelStatus.Text = "Solenoid Status:";
            // 
            // toolStripStatusLabelInlet
            // 
            this.toolStripStatusLabelInlet.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.toolStripStatusLabelInlet.Name = "toolStripStatusLabelInlet";
            this.toolStripStatusLabelInlet.Size = new System.Drawing.Size(81, 22);
            this.toolStripStatusLabelInlet.Text = "Inlet Solenoid";
            // 
            // toolStripLedBulbInlet
            // 
            this.toolStripLedBulbInlet.Name = "toolStripLedBulbInlet";
            this.toolStripLedBulbInlet.Size = new System.Drawing.Size(20, 25);
            this.toolStripLedBulbInlet.Text = "Inlet";
            // 
            // toolStripStatusLabelGap
            // 
            this.toolStripStatusLabelGap.Name = "toolStripStatusLabelGap";
            this.toolStripStatusLabelGap.Size = new System.Drawing.Size(16, 22);
            this.toolStripStatusLabelGap.Text = "   ";
            // 
            // toolStripStatusLabelOutlet
            // 
            this.toolStripStatusLabelOutlet.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.toolStripStatusLabelOutlet.Name = "toolStripStatusLabelOutlet";
            this.toolStripStatusLabelOutlet.Size = new System.Drawing.Size(90, 22);
            this.toolStripStatusLabelOutlet.Text = "Outlet Solenoid";
            // 
            // toolStripLedBulbOutlet
            // 
            this.toolStripLedBulbOutlet.Name = "toolStripLedBulbOutlet";
            this.toolStripLedBulbOutlet.Size = new System.Drawing.Size(20, 25);
            this.toolStripLedBulbOutlet.Text = "Outlet";
            // 
            // toolStripStatusLabelDateTime
            // 
            this.toolStripStatusLabelDateTime.Name = "toolStripStatusLabelDateTime";
            this.toolStripStatusLabelDateTime.RightToLeftAutoMirrorImage = true;
            this.toolStripStatusLabelDateTime.Size = new System.Drawing.Size(463, 22);
            this.toolStripStatusLabelDateTime.Spring = true;
            this.toolStripStatusLabelDateTime.Text = "DateTime";
            this.toolStripStatusLabelDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // splitContainerWhole
            // 
            this.splitContainerWhole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerWhole.BackColor = System.Drawing.Color.Transparent;
            this.splitContainerWhole.Location = new System.Drawing.Point(8, 8);
            this.splitContainerWhole.Name = "splitContainerWhole";
            // 
            // splitContainerWhole.Panel1
            // 
            this.splitContainerWhole.Panel1.Controls.Add(this.splitContainerLeft);
            // 
            // splitContainerWhole.Panel2
            // 
            this.splitContainerWhole.Panel2.Controls.Add(this.logMessageView);
            this.splitContainerWhole.Size = new System.Drawing.Size(789, 384);
            this.splitContainerWhole.SplitterDistance = 398;
            this.splitContainerWhole.TabIndex = 3;
            // 
            // splitContainerLeft
            // 
            this.splitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLeft.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLeft.Name = "splitContainerLeft";
            this.splitContainerLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.zedGraphDispenserLevel);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.panel);
            this.splitContainerLeft.Size = new System.Drawing.Size(398, 384);
            this.splitContainerLeft.SplitterDistance = 223;
            this.splitContainerLeft.TabIndex = 0;
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.Transparent;
            this.panel.Controls.Add(this.checkSendRawValues);
            this.panel.Controls.Add(this.checkAllowFlushing);
            this.panel.Controls.Add(this.labelSec2);
            this.panel.Controls.Add(this.groupBox1);
            this.panel.Controls.Add(this.buttonSyncSettings);
            this.panel.Controls.Add(this.labelSec);
            this.panel.Controls.Add(this.labelReadInterval);
            this.panel.Controls.Add(this.numericReadInterval);
            this.panel.Controls.Add(this.textEthernetIP);
            this.panel.Controls.Add(this.labelIP);
            this.panel.Controls.Add(this.buttonSetTime);
            this.panel.Controls.Add(this.groupBoxWaterLevelMarks);
            this.panel.Controls.Add(this.buttonFlush);
            this.panel.Controls.Add(this.labelCurrentLevel);
            this.panel.Controls.Add(this.labelFlushMinute);
            this.panel.Controls.Add(this.labelCurrentLevelLabel);
            this.panel.Controls.Add(this.comboComPorts);
            this.panel.Controls.Add(this.checkFastReadings);
            this.panel.Controls.Add(this.checkSendReadings);
            this.panel.Controls.Add(this.numericFlushMinute);
            this.panel.Controls.Add(this.checkOutlet);
            this.panel.Controls.Add(this.labelComPorts);
            this.panel.Controls.Add(this.checkInlet);
            this.panel.Controls.Add(this.labelFlushHour);
            this.panel.Controls.Add(this.numericFlushHour);
            this.panel.Controls.Add(this.numericFlushPeriod);
            this.panel.Controls.Add(this.labelFlushPeriod);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(398, 157);
            this.panel.TabIndex = 0;
            // 
            // checkSendRawValues
            // 
            this.checkSendRawValues.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkSendRawValues.AutoSize = true;
            this.checkSendRawValues.Location = new System.Drawing.Point(120, 21);
            this.checkSendRawValues.Name = "checkSendRawValues";
            this.checkSendRawValues.Size = new System.Drawing.Size(83, 17);
            this.checkSendRawValues.TabIndex = 26;
            this.checkSendRawValues.Text = "Raw Values";
            this.checkSendRawValues.UseVisualStyleBackColor = true;
            this.checkSendRawValues.CheckedChanged += new System.EventHandler(this.checkSendRawValues_CheckedChanged);
            // 
            // checkAllowFlushing
            // 
            this.checkAllowFlushing.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkAllowFlushing.AutoSize = true;
            this.checkAllowFlushing.Checked = true;
            this.checkAllowFlushing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAllowFlushing.Location = new System.Drawing.Point(120, 4);
            this.checkAllowFlushing.Name = "checkAllowFlushing";
            this.checkAllowFlushing.Size = new System.Drawing.Size(90, 17);
            this.checkAllowFlushing.TabIndex = 25;
            this.checkAllowFlushing.Text = "AllowFlushing";
            this.checkAllowFlushing.UseVisualStyleBackColor = true;
            this.checkAllowFlushing.CheckedChanged += new System.EventHandler(this.checkAllowFlushing_CheckedChanged);
            // 
            // labelSec2
            // 
            this.labelSec2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelSec2.AutoSize = true;
            this.labelSec2.Location = new System.Drawing.Point(259, 22);
            this.labelSec2.Name = "labelSec2";
            this.labelSec2.Size = new System.Drawing.Size(31, 13);
            this.labelSec2.TabIndex = 8;
            this.labelSec2.Text = "Secs";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.Controls.Add(this.numericMaxSensorValue);
            this.groupBox1.Controls.Add(this.labelMaxSensorValue);
            this.groupBox1.Controls.Add(this.numericMinSensorValue);
            this.groupBox1.Controls.Add(this.labelMinSensorValue);
            this.groupBox1.Location = new System.Drawing.Point(111, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(95, 53);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sensor Values";
            // 
            // numericMaxSensorValue
            // 
            this.numericMaxSensorValue.Location = new System.Drawing.Point(49, 27);
            this.numericMaxSensorValue.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericMaxSensorValue.Name = "numericMaxSensorValue";
            this.numericMaxSensorValue.Size = new System.Drawing.Size(40, 20);
            this.numericMaxSensorValue.TabIndex = 3;
            this.numericMaxSensorValue.Value = new decimal(new int[] {
            530,
            0,
            0,
            0});
            this.numericMaxSensorValue.ValueChanged += new System.EventHandler(this.numericMaxSensorValue_ValueChanged);
            // 
            // labelMaxSensorValue
            // 
            this.labelMaxSensorValue.AutoSize = true;
            this.labelMaxSensorValue.Location = new System.Drawing.Point(46, 13);
            this.labelMaxSensorValue.Name = "labelMaxSensorValue";
            this.labelMaxSensorValue.Size = new System.Drawing.Size(30, 13);
            this.labelMaxSensorValue.TabIndex = 1;
            this.labelMaxSensorValue.Text = "Max:";
            // 
            // numericMinSensorValue
            // 
            this.numericMinSensorValue.Location = new System.Drawing.Point(6, 27);
            this.numericMinSensorValue.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericMinSensorValue.Name = "numericMinSensorValue";
            this.numericMinSensorValue.Size = new System.Drawing.Size(40, 20);
            this.numericMinSensorValue.TabIndex = 2;
            this.numericMinSensorValue.Value = new decimal(new int[] {
            390,
            0,
            0,
            0});
            this.numericMinSensorValue.ValueChanged += new System.EventHandler(this.numericMinSensorValue_ValueChanged);
            // 
            // labelMinSensorValue
            // 
            this.labelMinSensorValue.AutoSize = true;
            this.labelMinSensorValue.Location = new System.Drawing.Point(3, 13);
            this.labelMinSensorValue.Name = "labelMinSensorValue";
            this.labelMinSensorValue.Size = new System.Drawing.Size(27, 13);
            this.labelMinSensorValue.TabIndex = 0;
            this.labelMinSensorValue.Text = "Min:";
            // 
            // buttonSyncSettings
            // 
            this.buttonSyncSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonSyncSettings.Location = new System.Drawing.Point(299, 84);
            this.buttonSyncSettings.Name = "buttonSyncSettings";
            this.buttonSyncSettings.Size = new System.Drawing.Size(93, 23);
            this.buttonSyncSettings.TabIndex = 19;
            this.buttonSyncSettings.Text = "Sync Settings";
            this.buttonSyncSettings.UseVisualStyleBackColor = true;
            this.buttonSyncSettings.Click += new System.EventHandler(this.buttonSyncSettings_Click);
            // 
            // labelSec
            // 
            this.labelSec.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelSec.AutoSize = true;
            this.labelSec.Location = new System.Drawing.Point(259, 133);
            this.labelSec.Name = "labelSec";
            this.labelSec.Size = new System.Drawing.Size(31, 13);
            this.labelSec.TabIndex = 21;
            this.labelSec.Text = "Secs";
            // 
            // labelReadInterval
            // 
            this.labelReadInterval.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelReadInterval.AutoSize = true;
            this.labelReadInterval.Location = new System.Drawing.Point(214, 115);
            this.labelReadInterval.Name = "labelReadInterval";
            this.labelReadInterval.Size = new System.Drawing.Size(74, 13);
            this.labelReadInterval.TabIndex = 13;
            this.labelReadInterval.Text = "Read Interval:";
            // 
            // numericReadInterval
            // 
            this.numericReadInterval.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericReadInterval.Location = new System.Drawing.Point(217, 129);
            this.numericReadInterval.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numericReadInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericReadInterval.Name = "numericReadInterval";
            this.numericReadInterval.Size = new System.Drawing.Size(40, 20);
            this.numericReadInterval.TabIndex = 14;
            this.numericReadInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericReadInterval.ValueChanged += new System.EventHandler(this.numericReadInterval_ValueChanged);
            // 
            // textEthernetIP
            // 
            this.textEthernetIP.Location = new System.Drawing.Point(299, 57);
            this.textEthernetIP.Name = "textEthernetIP";
            this.textEthernetIP.Size = new System.Drawing.Size(93, 20);
            this.textEthernetIP.TabIndex = 18;
            this.textEthernetIP.Text = "10.1.1.200";
            this.textEthernetIP.TextChanged += new System.EventHandler(this.textEthernetIP_TextChanged);
            // 
            // labelIP
            // 
            this.labelIP.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(296, 43);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(63, 13);
            this.labelIP.TabIndex = 17;
            this.labelIP.Text = "Ethernet IP:";
            // 
            // buttonSetTime
            // 
            this.buttonSetTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonSetTime.Location = new System.Drawing.Point(299, 107);
            this.buttonSetTime.Name = "buttonSetTime";
            this.buttonSetTime.Size = new System.Drawing.Size(93, 23);
            this.buttonSetTime.TabIndex = 20;
            this.buttonSetTime.Text = "Set Date/Time";
            this.buttonSetTime.UseVisualStyleBackColor = true;
            this.buttonSetTime.Click += new System.EventHandler(this.buttonSetTime_Click);
            // 
            // groupBoxWaterLevelMarks
            // 
            this.groupBoxWaterLevelMarks.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBoxWaterLevelMarks.Controls.Add(this.numericHighLevelMark);
            this.groupBoxWaterLevelMarks.Controls.Add(this.labelHighLevelMark);
            this.groupBoxWaterLevelMarks.Controls.Add(this.numericLowLevelMark);
            this.groupBoxWaterLevelMarks.Controls.Add(this.labelLowLevelMark);
            this.groupBoxWaterLevelMarks.Location = new System.Drawing.Point(2, 77);
            this.groupBoxWaterLevelMarks.Name = "groupBoxWaterLevelMarks";
            this.groupBoxWaterLevelMarks.Size = new System.Drawing.Size(95, 53);
            this.groupBoxWaterLevelMarks.TabIndex = 4;
            this.groupBoxWaterLevelMarks.TabStop = false;
            this.groupBoxWaterLevelMarks.Text = "Water Levels";
            // 
            // numericHighLevelMark
            // 
            this.numericHighLevelMark.Location = new System.Drawing.Point(49, 27);
            this.numericHighLevelMark.Name = "numericHighLevelMark";
            this.numericHighLevelMark.Size = new System.Drawing.Size(40, 20);
            this.numericHighLevelMark.TabIndex = 3;
            this.numericHighLevelMark.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numericHighLevelMark.ValueChanged += new System.EventHandler(this.numericHighLevelMark_ValueChanged);
            // 
            // labelHighLevelMark
            // 
            this.labelHighLevelMark.AutoSize = true;
            this.labelHighLevelMark.Location = new System.Drawing.Point(46, 13);
            this.labelHighLevelMark.Name = "labelHighLevelMark";
            this.labelHighLevelMark.Size = new System.Drawing.Size(32, 13);
            this.labelHighLevelMark.TabIndex = 1;
            this.labelHighLevelMark.Text = "High:";
            // 
            // numericLowLevelMark
            // 
            this.numericLowLevelMark.Location = new System.Drawing.Point(6, 27);
            this.numericLowLevelMark.Name = "numericLowLevelMark";
            this.numericLowLevelMark.Size = new System.Drawing.Size(40, 20);
            this.numericLowLevelMark.TabIndex = 2;
            this.numericLowLevelMark.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericLowLevelMark.ValueChanged += new System.EventHandler(this.numericLowLevelMark_ValueChanged);
            // 
            // labelLowLevelMark
            // 
            this.labelLowLevelMark.AutoSize = true;
            this.labelLowLevelMark.Location = new System.Drawing.Point(3, 13);
            this.labelLowLevelMark.Name = "labelLowLevelMark";
            this.labelLowLevelMark.Size = new System.Drawing.Size(30, 13);
            this.labelLowLevelMark.TabIndex = 0;
            this.labelLowLevelMark.Text = "Low:";
            // 
            // buttonFlush
            // 
            this.buttonFlush.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonFlush.Location = new System.Drawing.Point(299, 131);
            this.buttonFlush.Name = "buttonFlush";
            this.buttonFlush.Size = new System.Drawing.Size(93, 23);
            this.buttonFlush.TabIndex = 22;
            this.buttonFlush.Text = "Manual Flush";
            this.buttonFlush.UseVisualStyleBackColor = true;
            this.buttonFlush.Click += new System.EventHandler(this.buttonFlush_Click);
            // 
            // labelCurrentLevel
            // 
            this.labelCurrentLevel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelCurrentLevel.AutoSize = true;
            this.labelCurrentLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentLevel.Location = new System.Drawing.Point(70, 135);
            this.labelCurrentLevel.Name = "labelCurrentLevel";
            this.labelCurrentLevel.Size = new System.Drawing.Size(32, 16);
            this.labelCurrentLevel.TabIndex = 24;
            this.labelCurrentLevel.Text = "???";
            // 
            // labelFlushMinute
            // 
            this.labelFlushMinute.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelFlushMinute.AutoSize = true;
            this.labelFlushMinute.Location = new System.Drawing.Point(214, 78);
            this.labelFlushMinute.Name = "labelFlushMinute";
            this.labelFlushMinute.Size = new System.Drawing.Size(70, 13);
            this.labelFlushMinute.TabIndex = 11;
            this.labelFlushMinute.Text = "Flush Minute:";
            // 
            // labelCurrentLevelLabel
            // 
            this.labelCurrentLevelLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelCurrentLevelLabel.AutoSize = true;
            this.labelCurrentLevelLabel.Location = new System.Drawing.Point(-2, 136);
            this.labelCurrentLevelLabel.Name = "labelCurrentLevelLabel";
            this.labelCurrentLevelLabel.Size = new System.Drawing.Size(73, 13);
            this.labelCurrentLevelLabel.TabIndex = 23;
            this.labelCurrentLevelLabel.Text = "Current Level:";
            // 
            // comboComPorts
            // 
            this.comboComPorts.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboComPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboComPorts.FormattingEnabled = true;
            this.comboComPorts.Location = new System.Drawing.Point(299, 19);
            this.comboComPorts.Name = "comboComPorts";
            this.comboComPorts.Size = new System.Drawing.Size(93, 21);
            this.comboComPorts.TabIndex = 16;
            this.comboComPorts.SelectedIndexChanged += new System.EventHandler(this.comboComPorts_SelectedIndexChanged);
            // 
            // checkFastReadings
            // 
            this.checkFastReadings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkFastReadings.AutoSize = true;
            this.checkFastReadings.Location = new System.Drawing.Point(20, 58);
            this.checkFastReadings.Name = "checkFastReadings";
            this.checkFastReadings.Size = new System.Drawing.Size(123, 17);
            this.checkFastReadings.TabIndex = 3;
            this.checkFastReadings.Text = "Fast Level Readings";
            this.checkFastReadings.UseVisualStyleBackColor = true;
            this.checkFastReadings.CheckedChanged += new System.EventHandler(this.checkFastReadings_CheckedChanged);
            // 
            // checkSendReadings
            // 
            this.checkSendReadings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkSendReadings.AutoSize = true;
            this.checkSendReadings.Checked = true;
            this.checkSendReadings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSendReadings.Location = new System.Drawing.Point(10, 40);
            this.checkSendReadings.Name = "checkSendReadings";
            this.checkSendReadings.Size = new System.Drawing.Size(128, 17);
            this.checkSendReadings.TabIndex = 2;
            this.checkSendReadings.Text = "Send Level Readings";
            this.checkSendReadings.UseVisualStyleBackColor = true;
            this.checkSendReadings.CheckedChanged += new System.EventHandler(this.checkSendReadings_CheckedChanged);
            // 
            // numericFlushMinute
            // 
            this.numericFlushMinute.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericFlushMinute.Location = new System.Drawing.Point(217, 92);
            this.numericFlushMinute.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericFlushMinute.Name = "numericFlushMinute";
            this.numericFlushMinute.Size = new System.Drawing.Size(40, 20);
            this.numericFlushMinute.TabIndex = 12;
            this.numericFlushMinute.ValueChanged += new System.EventHandler(this.numericFlushMinute_ValueChanged);
            // 
            // checkOutlet
            // 
            this.checkOutlet.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkOutlet.AutoSize = true;
            this.checkOutlet.Location = new System.Drawing.Point(10, 21);
            this.checkOutlet.Name = "checkOutlet";
            this.checkOutlet.Size = new System.Drawing.Size(98, 17);
            this.checkOutlet.TabIndex = 1;
            this.checkOutlet.Text = "Outlet Solenoid";
            this.checkOutlet.UseVisualStyleBackColor = true;
            this.checkOutlet.CheckedChanged += new System.EventHandler(this.checkOutlet_CheckedChanged);
            // 
            // labelComPorts
            // 
            this.labelComPorts.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelComPorts.AutoSize = true;
            this.labelComPorts.Location = new System.Drawing.Point(296, 5);
            this.labelComPorts.Name = "labelComPorts";
            this.labelComPorts.Size = new System.Drawing.Size(61, 13);
            this.labelComPorts.TabIndex = 15;
            this.labelComPorts.Text = "COM Ports:";
            // 
            // checkInlet
            // 
            this.checkInlet.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkInlet.AutoSize = true;
            this.checkInlet.Location = new System.Drawing.Point(10, 4);
            this.checkInlet.Name = "checkInlet";
            this.checkInlet.Size = new System.Drawing.Size(90, 17);
            this.checkInlet.TabIndex = 0;
            this.checkInlet.Text = "Inlet Solenoid";
            this.checkInlet.UseVisualStyleBackColor = true;
            this.checkInlet.CheckedChanged += new System.EventHandler(this.checkInlet_CheckedChanged);
            // 
            // labelFlushHour
            // 
            this.labelFlushHour.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelFlushHour.AutoSize = true;
            this.labelFlushHour.Location = new System.Drawing.Point(214, 41);
            this.labelFlushHour.Name = "labelFlushHour";
            this.labelFlushHour.Size = new System.Drawing.Size(61, 13);
            this.labelFlushHour.TabIndex = 9;
            this.labelFlushHour.Text = "Flush Hour:";
            // 
            // numericFlushHour
            // 
            this.numericFlushHour.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericFlushHour.Location = new System.Drawing.Point(217, 55);
            this.numericFlushHour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numericFlushHour.Name = "numericFlushHour";
            this.numericFlushHour.Size = new System.Drawing.Size(40, 20);
            this.numericFlushHour.TabIndex = 10;
            this.numericFlushHour.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericFlushHour.ValueChanged += new System.EventHandler(this.numericFlushHour_ValueChanged);
            // 
            // numericFlushPeriod
            // 
            this.numericFlushPeriod.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericFlushPeriod.Location = new System.Drawing.Point(217, 19);
            this.numericFlushPeriod.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericFlushPeriod.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericFlushPeriod.Name = "numericFlushPeriod";
            this.numericFlushPeriod.Size = new System.Drawing.Size(40, 20);
            this.numericFlushPeriod.TabIndex = 7;
            this.numericFlushPeriod.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericFlushPeriod.ValueChanged += new System.EventHandler(this.numericFlushPeriod_ValueChanged);
            // 
            // labelFlushPeriod
            // 
            this.labelFlushPeriod.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelFlushPeriod.AutoSize = true;
            this.labelFlushPeriod.Location = new System.Drawing.Point(214, 5);
            this.labelFlushPeriod.Name = "labelFlushPeriod";
            this.labelFlushPeriod.Size = new System.Drawing.Size(68, 13);
            this.labelFlushPeriod.TabIndex = 6;
            this.labelFlushPeriod.Text = "Flush Period:";
            // 
            // DogDispenserController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DispenserController.Properties.Resources.Background;
            this.ClientSize = new System.Drawing.Size(801, 422);
            this.Controls.Add(this.splitContainerWhole);
            this.Controls.Add(this.statusStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(652, 358);
            this.Name = "DogDispenserController";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dog Water Dispenser Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DogDispenserController_FormClosing);
            this.Load += new System.EventHandler(this.DogDispenserController_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainerWhole.Panel1.ResumeLayout(false);
            this.splitContainerWhole.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWhole)).EndInit();
            this.splitContainerWhole.ResumeLayout(false);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).EndInit();
            this.splitContainerLeft.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericMaxSensorValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericMinSensorValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericReadInterval)).EndInit();
            this.groupBoxWaterLevelMarks.ResumeLayout(false);
            this.groupBoxWaterLevelMarks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericHighLevelMark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLowLevelMark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFlushMinute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFlushHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFlushPeriod)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphDispenserLevel;
        private Meteosoft.MessageLogger.LogMessageView logMessageView;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelGap;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelInlet;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelOutlet;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelDateTime;
        private System.Windows.Forms.SplitContainer splitContainerWhole;
        private System.Windows.Forms.SplitContainer splitContainerLeft;
        private Meteosoft.ControllerClient.ToolStripLedBulb toolStripLedBulbInlet;
        private Meteosoft.ControllerClient.ToolStripLedBulb toolStripLedBulbOutlet;
        private System.Windows.Forms.Label labelFlushPeriod;
        private System.Windows.Forms.NumericUpDown numericFlushPeriod;
        private System.Windows.Forms.Button buttonFlush;
        private System.Windows.Forms.CheckBox checkOutlet;
        private System.Windows.Forms.CheckBox checkInlet;
        private System.Windows.Forms.Label labelCurrentLevel;
        private System.Windows.Forms.Label labelCurrentLevelLabel;
        private System.Windows.Forms.CheckBox checkFastReadings;
        private System.Windows.Forms.CheckBox checkSendReadings;
        private System.Windows.Forms.Label labelComPorts;
        private System.Windows.Forms.ComboBox comboComPorts;
        private System.Windows.Forms.Button buttonSetTime;
        private System.Windows.Forms.Label labelFlushMinute;
        private System.Windows.Forms.NumericUpDown numericFlushMinute;
        private System.Windows.Forms.Label labelFlushHour;
        private System.Windows.Forms.NumericUpDown numericFlushHour;
        private System.Windows.Forms.Label labelLowLevelMark;
        private System.Windows.Forms.NumericUpDown numericLowLevelMark;
        private System.Windows.Forms.GroupBox groupBoxWaterLevelMarks;
        private System.Windows.Forms.NumericUpDown numericHighLevelMark;
        private System.Windows.Forms.Label labelHighLevelMark;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.TextBox textEthernetIP;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Label labelReadInterval;
        private System.Windows.Forms.NumericUpDown numericReadInterval;
        private System.Windows.Forms.Label labelSec;
        private System.Windows.Forms.Button buttonSyncSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericMaxSensorValue;
        private System.Windows.Forms.Label labelMaxSensorValue;
        private System.Windows.Forms.NumericUpDown numericMinSensorValue;
        private System.Windows.Forms.Label labelMinSensorValue;
        private System.Windows.Forms.Label labelSec2;
        private System.Windows.Forms.CheckBox checkAllowFlushing;
        private System.Windows.Forms.CheckBox checkSendRawValues;
    }
}

