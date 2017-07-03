namespace DispenserController
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.checkSendRawValues = new System.Windows.Forms.CheckBox();
            this.buttonSyncSettings = new System.Windows.Forms.Button();
            this.labelReadInterval = new System.Windows.Forms.Label();
            this.numericReadInterval = new System.Windows.Forms.NumericUpDown();
            this.textLocalIP = new System.Windows.Forms.TextBox();
            this.labelLocalIP = new System.Windows.Forms.Label();
            this.buttonSetTime = new System.Windows.Forms.Button();
            this.buttonFlush = new System.Windows.Forms.Button();
            this.labelFlushMinute = new System.Windows.Forms.Label();
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
            this.labelSec2 = new System.Windows.Forms.Label();
            this.labelSec = new System.Windows.Forms.Label();
            this.grouperFlushSettings = new CodeVendor.Controls.Grouper();
            this.grouperSolenoidSettings = new CodeVendor.Controls.Grouper();
            this.grouperReadings = new CodeVendor.Controls.Grouper();
            this.grouperCommsSettings = new CodeVendor.Controls.Grouper();
            this.grouperArduinoTCPServer = new CodeVendor.Controls.Grouper();
            this.buttonSetArduinoIP = new System.Windows.Forms.Button();
            this.labelArduinoGateway = new System.Windows.Forms.Label();
            this.textArduinoGateway = new System.Windows.Forms.TextBox();
            this.labelArduinoDNS = new System.Windows.Forms.Label();
            this.textArduinoDNS = new System.Windows.Forms.TextBox();
            this.labelArduinoIP = new System.Windows.Forms.Label();
            this.textArduinoIP = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonResetNow = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericReadInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFlushMinute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFlushHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFlushPeriod)).BeginInit();
            this.grouperFlushSettings.SuspendLayout();
            this.grouperSolenoidSettings.SuspendLayout();
            this.grouperReadings.SuspendLayout();
            this.grouperCommsSettings.SuspendLayout();
            this.grouperArduinoTCPServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkSendRawValues
            // 
            this.checkSendRawValues.AutoSize = true;
            this.checkSendRawValues.Location = new System.Drawing.Point(10, 65);
            this.checkSendRawValues.Name = "checkSendRawValues";
            this.checkSendRawValues.Size = new System.Drawing.Size(89, 17);
            this.checkSendRawValues.TabIndex = 2;
            this.checkSendRawValues.Text = "Raw Values?";
            this.checkSendRawValues.UseVisualStyleBackColor = true;
            this.checkSendRawValues.CheckedChanged += new System.EventHandler(this.checkSendRawValues_CheckedChanged);
            // 
            // buttonSyncSettings
            // 
            this.buttonSyncSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonSyncSettings.Location = new System.Drawing.Point(274, 197);
            this.buttonSyncSettings.Name = "buttonSyncSettings";
            this.buttonSyncSettings.Size = new System.Drawing.Size(93, 23);
            this.buttonSyncSettings.TabIndex = 6;
            this.buttonSyncSettings.Text = "Sync Settings";
            this.buttonSyncSettings.UseVisualStyleBackColor = true;
            this.buttonSyncSettings.Click += new System.EventHandler(this.buttonSyncSettings_Click);
            // 
            // labelReadInterval
            // 
            this.labelReadInterval.AutoSize = true;
            this.labelReadInterval.Location = new System.Drawing.Point(7, 86);
            this.labelReadInterval.Name = "labelReadInterval";
            this.labelReadInterval.Size = new System.Drawing.Size(74, 13);
            this.labelReadInterval.TabIndex = 3;
            this.labelReadInterval.Text = "Read Interval:";
            // 
            // numericReadInterval
            // 
            this.numericReadInterval.Location = new System.Drawing.Point(10, 100);
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
            this.numericReadInterval.TabIndex = 4;
            this.numericReadInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericReadInterval.ValueChanged += new System.EventHandler(this.numericReadInterval_ValueChanged);
            // 
            // textLocalIP
            // 
            this.textLocalIP.Location = new System.Drawing.Point(12, 78);
            this.textLocalIP.Name = "textLocalIP";
            this.textLocalIP.Size = new System.Drawing.Size(116, 20);
            this.textLocalIP.TabIndex = 3;
            this.textLocalIP.Text = "10.1.1.200";
            this.textLocalIP.Leave += new System.EventHandler(this.textLocalIP_Leave);
            // 
            // labelLocalIP
            // 
            this.labelLocalIP.AutoSize = true;
            this.labelLocalIP.Location = new System.Drawing.Point(9, 64);
            this.labelLocalIP.Name = "labelLocalIP";
            this.labelLocalIP.Size = new System.Drawing.Size(49, 13);
            this.labelLocalIP.TabIndex = 2;
            this.labelLocalIP.Text = "Local IP:";
            // 
            // buttonSetTime
            // 
            this.buttonSetTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonSetTime.Location = new System.Drawing.Point(170, 197);
            this.buttonSetTime.Name = "buttonSetTime";
            this.buttonSetTime.Size = new System.Drawing.Size(93, 23);
            this.buttonSetTime.TabIndex = 5;
            this.buttonSetTime.Text = "Set Date/Time";
            this.buttonSetTime.UseVisualStyleBackColor = true;
            this.buttonSetTime.Click += new System.EventHandler(this.buttonSetTime_Click);
            // 
            // buttonFlush
            // 
            this.buttonFlush.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonFlush.Location = new System.Drawing.Point(378, 197);
            this.buttonFlush.Name = "buttonFlush";
            this.buttonFlush.Size = new System.Drawing.Size(93, 23);
            this.buttonFlush.TabIndex = 7;
            this.buttonFlush.Text = "Manual Flush";
            this.buttonFlush.UseVisualStyleBackColor = true;
            this.buttonFlush.Click += new System.EventHandler(this.buttonFlush_Click);
            // 
            // labelFlushMinute
            // 
            this.labelFlushMinute.AutoSize = true;
            this.labelFlushMinute.Location = new System.Drawing.Point(71, 26);
            this.labelFlushMinute.Name = "labelFlushMinute";
            this.labelFlushMinute.Size = new System.Drawing.Size(70, 13);
            this.labelFlushMinute.TabIndex = 2;
            this.labelFlushMinute.Text = "Flush Minute:";
            // 
            // comboComPorts
            // 
            this.comboComPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboComPorts.FormattingEnabled = true;
            this.comboComPorts.Location = new System.Drawing.Point(12, 42);
            this.comboComPorts.Name = "comboComPorts";
            this.comboComPorts.Size = new System.Drawing.Size(116, 21);
            this.comboComPorts.TabIndex = 1;
            this.comboComPorts.SelectedIndexChanged += new System.EventHandler(this.comboComPorts_SelectedIndexChanged);
            // 
            // checkFastReadings
            // 
            this.checkFastReadings.AutoSize = true;
            this.checkFastReadings.Location = new System.Drawing.Point(20, 45);
            this.checkFastReadings.Name = "checkFastReadings";
            this.checkFastReadings.Size = new System.Drawing.Size(129, 17);
            this.checkFastReadings.TabIndex = 1;
            this.checkFastReadings.Text = "Fast Level Readings?";
            this.checkFastReadings.UseVisualStyleBackColor = true;
            this.checkFastReadings.CheckedChanged += new System.EventHandler(this.checkFastReadings_CheckedChanged);
            // 
            // checkSendReadings
            // 
            this.checkSendReadings.AutoSize = true;
            this.checkSendReadings.Checked = true;
            this.checkSendReadings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSendReadings.Location = new System.Drawing.Point(10, 27);
            this.checkSendReadings.Name = "checkSendReadings";
            this.checkSendReadings.Size = new System.Drawing.Size(134, 17);
            this.checkSendReadings.TabIndex = 0;
            this.checkSendReadings.Text = "Send Level Readings?";
            this.checkSendReadings.UseVisualStyleBackColor = true;
            this.checkSendReadings.CheckedChanged += new System.EventHandler(this.checkSendReadings_CheckedChanged);
            // 
            // numericFlushMinute
            // 
            this.numericFlushMinute.Location = new System.Drawing.Point(74, 40);
            this.numericFlushMinute.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericFlushMinute.Name = "numericFlushMinute";
            this.numericFlushMinute.Size = new System.Drawing.Size(40, 20);
            this.numericFlushMinute.TabIndex = 3;
            this.numericFlushMinute.ValueChanged += new System.EventHandler(this.numericFlushMinute_ValueChanged);
            // 
            // checkOutlet
            // 
            this.checkOutlet.AutoSize = true;
            this.checkOutlet.Location = new System.Drawing.Point(10, 43);
            this.checkOutlet.Name = "checkOutlet";
            this.checkOutlet.Size = new System.Drawing.Size(121, 17);
            this.checkOutlet.TabIndex = 1;
            this.checkOutlet.Text = "Outlet Solenoid On?";
            this.checkOutlet.UseVisualStyleBackColor = true;
            this.checkOutlet.CheckedChanged += new System.EventHandler(this.checkOutlet_CheckedChanged);
            // 
            // labelComPorts
            // 
            this.labelComPorts.AutoSize = true;
            this.labelComPorts.Location = new System.Drawing.Point(9, 29);
            this.labelComPorts.Name = "labelComPorts";
            this.labelComPorts.Size = new System.Drawing.Size(61, 13);
            this.labelComPorts.TabIndex = 0;
            this.labelComPorts.Text = "COM Ports:";
            // 
            // checkInlet
            // 
            this.checkInlet.AutoSize = true;
            this.checkInlet.Location = new System.Drawing.Point(10, 25);
            this.checkInlet.Name = "checkInlet";
            this.checkInlet.Size = new System.Drawing.Size(113, 17);
            this.checkInlet.TabIndex = 0;
            this.checkInlet.Text = "Inlet Solenoid On?";
            this.checkInlet.UseVisualStyleBackColor = true;
            this.checkInlet.CheckedChanged += new System.EventHandler(this.checkInlet_CheckedChanged);
            // 
            // labelFlushHour
            // 
            this.labelFlushHour.AutoSize = true;
            this.labelFlushHour.Location = new System.Drawing.Point(9, 26);
            this.labelFlushHour.Name = "labelFlushHour";
            this.labelFlushHour.Size = new System.Drawing.Size(61, 13);
            this.labelFlushHour.TabIndex = 0;
            this.labelFlushHour.Text = "Flush Hour:";
            // 
            // numericFlushHour
            // 
            this.numericFlushHour.Location = new System.Drawing.Point(12, 40);
            this.numericFlushHour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numericFlushHour.Name = "numericFlushHour";
            this.numericFlushHour.Size = new System.Drawing.Size(40, 20);
            this.numericFlushHour.TabIndex = 1;
            this.numericFlushHour.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericFlushHour.ValueChanged += new System.EventHandler(this.numericFlushHour_ValueChanged);
            // 
            // numericFlushPeriod
            // 
            this.numericFlushPeriod.Location = new System.Drawing.Point(12, 77);
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
            this.numericFlushPeriod.TabIndex = 5;
            this.numericFlushPeriod.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericFlushPeriod.ValueChanged += new System.EventHandler(this.numericFlushPeriod_ValueChanged);
            // 
            // labelFlushPeriod
            // 
            this.labelFlushPeriod.AutoSize = true;
            this.labelFlushPeriod.Location = new System.Drawing.Point(9, 63);
            this.labelFlushPeriod.Name = "labelFlushPeriod";
            this.labelFlushPeriod.Size = new System.Drawing.Size(68, 13);
            this.labelFlushPeriod.TabIndex = 4;
            this.labelFlushPeriod.Text = "Flush Period:";
            // 
            // labelSec2
            // 
            this.labelSec2.AutoSize = true;
            this.labelSec2.Location = new System.Drawing.Point(50, 80);
            this.labelSec2.Name = "labelSec2";
            this.labelSec2.Size = new System.Drawing.Size(31, 13);
            this.labelSec2.TabIndex = 6;
            this.labelSec2.Text = "Secs";
            // 
            // labelSec
            // 
            this.labelSec.AutoSize = true;
            this.labelSec.Location = new System.Drawing.Point(50, 104);
            this.labelSec.Name = "labelSec";
            this.labelSec.Size = new System.Drawing.Size(31, 13);
            this.labelSec.TabIndex = 5;
            this.labelSec.Text = "Secs";
            // 
            // grouperFlushSettings
            // 
            this.grouperFlushSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grouperFlushSettings.BackgroundColor = System.Drawing.Color.Transparent;
            this.grouperFlushSettings.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grouperFlushSettings.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None;
            this.grouperFlushSettings.BorderColor = System.Drawing.Color.Black;
            this.grouperFlushSettings.BorderThickness = 1F;
            this.grouperFlushSettings.Controls.Add(this.labelFlushPeriod);
            this.grouperFlushSettings.Controls.Add(this.numericFlushPeriod);
            this.grouperFlushSettings.Controls.Add(this.labelFlushHour);
            this.grouperFlushSettings.Controls.Add(this.numericFlushHour);
            this.grouperFlushSettings.Controls.Add(this.labelFlushMinute);
            this.grouperFlushSettings.Controls.Add(this.numericFlushMinute);
            this.grouperFlushSettings.Controls.Add(this.labelSec2);
            this.grouperFlushSettings.CustomGroupBoxColor = System.Drawing.Color.White;
            this.grouperFlushSettings.ForeColor = System.Drawing.Color.Black;
            this.grouperFlushSettings.GroupImage = null;
            this.grouperFlushSettings.GroupTitle = "Flush Settings";
            this.grouperFlushSettings.Location = new System.Drawing.Point(4, 144);
            this.grouperFlushSettings.Name = "grouperFlushSettings";
            this.grouperFlushSettings.Padding = new System.Windows.Forms.Padding(20);
            this.grouperFlushSettings.PaintGroupBox = true;
            this.grouperFlushSettings.RoundCorners = 10;
            this.grouperFlushSettings.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouperFlushSettings.ShadowControl = false;
            this.grouperFlushSettings.ShadowThickness = 3;
            this.grouperFlushSettings.Size = new System.Drawing.Size(157, 108);
            this.grouperFlushSettings.TabIndex = 3;
            // 
            // grouperSolenoidSettings
            // 
            this.grouperSolenoidSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grouperSolenoidSettings.BackgroundColor = System.Drawing.Color.Transparent;
            this.grouperSolenoidSettings.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grouperSolenoidSettings.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None;
            this.grouperSolenoidSettings.BorderColor = System.Drawing.Color.Black;
            this.grouperSolenoidSettings.BorderThickness = 1F;
            this.grouperSolenoidSettings.Controls.Add(this.checkInlet);
            this.grouperSolenoidSettings.Controls.Add(this.checkOutlet);
            this.grouperSolenoidSettings.CustomGroupBoxColor = System.Drawing.Color.White;
            this.grouperSolenoidSettings.ForeColor = System.Drawing.Color.Black;
            this.grouperSolenoidSettings.GroupImage = null;
            this.grouperSolenoidSettings.GroupTitle = "Solenoid Settings";
            this.grouperSolenoidSettings.Location = new System.Drawing.Point(172, 125);
            this.grouperSolenoidSettings.Name = "grouperSolenoidSettings";
            this.grouperSolenoidSettings.Padding = new System.Windows.Forms.Padding(20);
            this.grouperSolenoidSettings.PaintGroupBox = true;
            this.grouperSolenoidSettings.RoundCorners = 10;
            this.grouperSolenoidSettings.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouperSolenoidSettings.ShadowControl = false;
            this.grouperSolenoidSettings.ShadowThickness = 3;
            this.grouperSolenoidSettings.Size = new System.Drawing.Size(140, 65);
            this.grouperSolenoidSettings.TabIndex = 4;
            // 
            // grouperReadings
            // 
            this.grouperReadings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grouperReadings.BackgroundColor = System.Drawing.Color.Transparent;
            this.grouperReadings.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grouperReadings.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None;
            this.grouperReadings.BorderColor = System.Drawing.Color.Black;
            this.grouperReadings.BorderThickness = 1F;
            this.grouperReadings.Controls.Add(this.checkSendReadings);
            this.grouperReadings.Controls.Add(this.labelSec);
            this.grouperReadings.Controls.Add(this.checkFastReadings);
            this.grouperReadings.Controls.Add(this.numericReadInterval);
            this.grouperReadings.Controls.Add(this.checkSendRawValues);
            this.grouperReadings.Controls.Add(this.labelReadInterval);
            this.grouperReadings.CustomGroupBoxColor = System.Drawing.Color.White;
            this.grouperReadings.ForeColor = System.Drawing.Color.Black;
            this.grouperReadings.GroupImage = null;
            this.grouperReadings.GroupTitle = "Reading Data";
            this.grouperReadings.Location = new System.Drawing.Point(4, 3);
            this.grouperReadings.Name = "grouperReadings";
            this.grouperReadings.Padding = new System.Windows.Forms.Padding(20);
            this.grouperReadings.PaintGroupBox = true;
            this.grouperReadings.RoundCorners = 10;
            this.grouperReadings.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouperReadings.ShadowControl = false;
            this.grouperReadings.ShadowThickness = 3;
            this.grouperReadings.Size = new System.Drawing.Size(157, 135);
            this.grouperReadings.TabIndex = 0;
            // 
            // grouperCommsSettings
            // 
            this.grouperCommsSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grouperCommsSettings.BackgroundColor = System.Drawing.Color.Transparent;
            this.grouperCommsSettings.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grouperCommsSettings.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None;
            this.grouperCommsSettings.BorderColor = System.Drawing.Color.Black;
            this.grouperCommsSettings.BorderThickness = 1F;
            this.grouperCommsSettings.Controls.Add(this.labelComPorts);
            this.grouperCommsSettings.Controls.Add(this.comboComPorts);
            this.grouperCommsSettings.Controls.Add(this.labelLocalIP);
            this.grouperCommsSettings.Controls.Add(this.textLocalIP);
            this.grouperCommsSettings.CustomGroupBoxColor = System.Drawing.Color.White;
            this.grouperCommsSettings.ForeColor = System.Drawing.Color.Black;
            this.grouperCommsSettings.GroupImage = null;
            this.grouperCommsSettings.GroupTitle = "Comms Settings";
            this.grouperCommsSettings.Location = new System.Drawing.Point(172, 3);
            this.grouperCommsSettings.Name = "grouperCommsSettings";
            this.grouperCommsSettings.Padding = new System.Windows.Forms.Padding(20);
            this.grouperCommsSettings.PaintGroupBox = true;
            this.grouperCommsSettings.RoundCorners = 10;
            this.grouperCommsSettings.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouperCommsSettings.ShadowControl = false;
            this.grouperCommsSettings.ShadowThickness = 3;
            this.grouperCommsSettings.Size = new System.Drawing.Size(140, 116);
            this.grouperCommsSettings.TabIndex = 1;
            // 
            // grouperArduinoTCPServer
            // 
            this.grouperArduinoTCPServer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grouperArduinoTCPServer.BackgroundColor = System.Drawing.Color.Transparent;
            this.grouperArduinoTCPServer.BackgroundGradientColor = System.Drawing.Color.Transparent;
            this.grouperArduinoTCPServer.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None;
            this.grouperArduinoTCPServer.BorderColor = System.Drawing.Color.Black;
            this.grouperArduinoTCPServer.BorderThickness = 1F;
            this.grouperArduinoTCPServer.Controls.Add(this.buttonSetArduinoIP);
            this.grouperArduinoTCPServer.Controls.Add(this.labelArduinoGateway);
            this.grouperArduinoTCPServer.Controls.Add(this.textArduinoGateway);
            this.grouperArduinoTCPServer.Controls.Add(this.labelArduinoDNS);
            this.grouperArduinoTCPServer.Controls.Add(this.textArduinoDNS);
            this.grouperArduinoTCPServer.Controls.Add(this.labelArduinoIP);
            this.grouperArduinoTCPServer.Controls.Add(this.textArduinoIP);
            this.grouperArduinoTCPServer.CustomGroupBoxColor = System.Drawing.Color.White;
            this.grouperArduinoTCPServer.ForeColor = System.Drawing.Color.Black;
            this.grouperArduinoTCPServer.GroupImage = null;
            this.grouperArduinoTCPServer.GroupTitle = "Arduino TCP Server";
            this.grouperArduinoTCPServer.Location = new System.Drawing.Point(323, 3);
            this.grouperArduinoTCPServer.Name = "grouperArduinoTCPServer";
            this.grouperArduinoTCPServer.Padding = new System.Windows.Forms.Padding(20);
            this.grouperArduinoTCPServer.PaintGroupBox = true;
            this.grouperArduinoTCPServer.RoundCorners = 10;
            this.grouperArduinoTCPServer.ShadowColor = System.Drawing.Color.DarkGray;
            this.grouperArduinoTCPServer.ShadowControl = false;
            this.grouperArduinoTCPServer.ShadowThickness = 3;
            this.grouperArduinoTCPServer.Size = new System.Drawing.Size(157, 164);
            this.grouperArduinoTCPServer.TabIndex = 2;
            // 
            // buttonSetArduinoIP
            // 
            this.buttonSetArduinoIP.Location = new System.Drawing.Point(52, 136);
            this.buttonSetArduinoIP.Name = "buttonSetArduinoIP";
            this.buttonSetArduinoIP.Size = new System.Drawing.Size(47, 23);
            this.buttonSetArduinoIP.TabIndex = 53;
            this.buttonSetArduinoIP.Text = "Set";
            this.buttonSetArduinoIP.UseVisualStyleBackColor = true;
            this.buttonSetArduinoIP.Click += new System.EventHandler(this.buttonSetArduinoIP_Click);
            // 
            // labelArduinoGateway
            // 
            this.labelArduinoGateway.AutoSize = true;
            this.labelArduinoGateway.Location = new System.Drawing.Point(10, 98);
            this.labelArduinoGateway.Name = "labelArduinoGateway";
            this.labelArduinoGateway.Size = new System.Drawing.Size(91, 13);
            this.labelArduinoGateway.TabIndex = 4;
            this.labelArduinoGateway.Text = "Arduino Gateway:";
            // 
            // textArduinoGateway
            // 
            this.textArduinoGateway.Location = new System.Drawing.Point(13, 112);
            this.textArduinoGateway.Name = "textArduinoGateway";
            this.textArduinoGateway.Size = new System.Drawing.Size(130, 20);
            this.textArduinoGateway.TabIndex = 5;
            this.textArduinoGateway.Text = "10.1.1.1";
            // 
            // labelArduinoDNS
            // 
            this.labelArduinoDNS.AutoSize = true;
            this.labelArduinoDNS.Location = new System.Drawing.Point(10, 61);
            this.labelArduinoDNS.Name = "labelArduinoDNS";
            this.labelArduinoDNS.Size = new System.Drawing.Size(72, 13);
            this.labelArduinoDNS.TabIndex = 2;
            this.labelArduinoDNS.Text = "Arduino DNS:";
            // 
            // textArduinoDNS
            // 
            this.textArduinoDNS.Location = new System.Drawing.Point(13, 75);
            this.textArduinoDNS.Name = "textArduinoDNS";
            this.textArduinoDNS.Size = new System.Drawing.Size(130, 20);
            this.textArduinoDNS.TabIndex = 3;
            this.textArduinoDNS.Text = "10.1.1.1";
            // 
            // labelArduinoIP
            // 
            this.labelArduinoIP.AutoSize = true;
            this.labelArduinoIP.Location = new System.Drawing.Point(10, 27);
            this.labelArduinoIP.Name = "labelArduinoIP";
            this.labelArduinoIP.Size = new System.Drawing.Size(59, 13);
            this.labelArduinoIP.TabIndex = 0;
            this.labelArduinoIP.Text = "Arduino IP:";
            // 
            // textArduinoIP
            // 
            this.textArduinoIP.Location = new System.Drawing.Point(13, 41);
            this.textArduinoIP.Name = "textArduinoIP";
            this.textArduinoIP.Size = new System.Drawing.Size(130, 20);
            this.textArduinoIP.TabIndex = 1;
            this.textArduinoIP.Text = "10.1.1.200";
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(410, 229);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(61, 23);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // buttonResetNow
            // 
            this.buttonResetNow.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonResetNow.Location = new System.Drawing.Point(170, 224);
            this.buttonResetNow.Name = "buttonResetNow";
            this.buttonResetNow.Size = new System.Drawing.Size(93, 23);
            this.buttonResetNow.TabIndex = 9;
            this.buttonResetNow.Text = "Reset Arduino";
            this.buttonResetNow.UseVisualStyleBackColor = true;
            this.buttonResetNow.Click += new System.EventHandler(this.buttonResetNow_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(491, 259);
            this.Controls.Add(this.buttonResetNow);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.grouperArduinoTCPServer);
            this.Controls.Add(this.grouperCommsSettings);
            this.Controls.Add(this.grouperReadings);
            this.Controls.Add(this.grouperSolenoidSettings);
            this.Controls.Add(this.grouperFlushSettings);
            this.Controls.Add(this.buttonSyncSettings);
            this.Controls.Add(this.buttonSetTime);
            this.Controls.Add(this.buttonFlush);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dispenser Properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Properties_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericReadInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFlushMinute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFlushHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFlushPeriod)).EndInit();
            this.grouperFlushSettings.ResumeLayout(false);
            this.grouperFlushSettings.PerformLayout();
            this.grouperSolenoidSettings.ResumeLayout(false);
            this.grouperSolenoidSettings.PerformLayout();
            this.grouperReadings.ResumeLayout(false);
            this.grouperReadings.PerformLayout();
            this.grouperCommsSettings.ResumeLayout(false);
            this.grouperCommsSettings.PerformLayout();
            this.grouperArduinoTCPServer.ResumeLayout(false);
            this.grouperArduinoTCPServer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonSyncSettings;
        private System.Windows.Forms.Label labelReadInterval;
        public System.Windows.Forms.Label labelLocalIP;
        private System.Windows.Forms.Button buttonSetTime;
        private System.Windows.Forms.Button buttonFlush;
        private System.Windows.Forms.Label labelFlushMinute;
        public System.Windows.Forms.Label labelComPorts;
        private System.Windows.Forms.Label labelFlushHour;
        private System.Windows.Forms.Label labelFlushPeriod;
        private System.Windows.Forms.Label labelSec2;
        private System.Windows.Forms.Label labelSec;
        private CodeVendor.Controls.Grouper grouperFlushSettings;
        private CodeVendor.Controls.Grouper grouperSolenoidSettings;
        private CodeVendor.Controls.Grouper grouperReadings;
        private CodeVendor.Controls.Grouper grouperCommsSettings;
        private CodeVendor.Controls.Grouper grouperArduinoTCPServer;
        private System.Windows.Forms.Label labelArduinoGateway;
        private System.Windows.Forms.Label labelArduinoDNS;
        private System.Windows.Forms.Label labelArduinoIP;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSetArduinoIP;
        public System.Windows.Forms.CheckBox checkSendRawValues;
        public System.Windows.Forms.NumericUpDown numericReadInterval;
        public System.Windows.Forms.TextBox textLocalIP;
        public System.Windows.Forms.ComboBox comboComPorts;
        public System.Windows.Forms.CheckBox checkFastReadings;
        public System.Windows.Forms.CheckBox checkSendReadings;
        public System.Windows.Forms.NumericUpDown numericFlushMinute;
        public System.Windows.Forms.CheckBox checkOutlet;
        public System.Windows.Forms.CheckBox checkInlet;
        public System.Windows.Forms.NumericUpDown numericFlushHour;
        public System.Windows.Forms.NumericUpDown numericFlushPeriod;
        public System.Windows.Forms.TextBox textArduinoGateway;
        public System.Windows.Forms.TextBox textArduinoDNS;
        public System.Windows.Forms.TextBox textArduinoIP;
        private System.Windows.Forms.Button buttonResetNow;
    }
}