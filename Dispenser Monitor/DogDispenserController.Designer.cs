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
            this.toolStripLedBulbInlet = new DispenserController.ToolStripLedBulb();
            this.toolStripStatusLabelGap = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelOutlet = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripLedBulbOutlet = new DispenserController.ToolStripLedBulb();
            this.toolStripStatusLabelDateTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerWhole = new System.Windows.Forms.SplitContainer();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.panel = new System.Windows.Forms.Panel();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.labelCurrentLevel = new System.Windows.Forms.Label();
            this.labelCurrentLevelLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
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
            this.panel1.SuspendLayout();
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
            this.zedGraphDispenserLevel.Size = new System.Drawing.Size(508, 402);
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
            this.logMessageView.Size = new System.Drawing.Size(443, 436);
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
            this.statusStrip.Location = new System.Drawing.Point(0, 449);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(969, 27);
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
            this.toolStripStatusLabelInlet.Size = new System.Drawing.Size(46, 22);
            this.toolStripStatusLabelInlet.Text = "     Inlet";
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
            this.toolStripStatusLabelOutlet.Size = new System.Drawing.Size(40, 22);
            this.toolStripStatusLabelOutlet.Text = "Outlet";
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
            this.toolStripStatusLabelDateTime.Size = new System.Drawing.Size(716, 22);
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
            this.splitContainerWhole.Panel2.Controls.Add(this.panel1);
            this.splitContainerWhole.Size = new System.Drawing.Size(957, 438);
            this.splitContainerWhole.SplitterDistance = 508;
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
            this.splitContainerLeft.Size = new System.Drawing.Size(508, 438);
            this.splitContainerLeft.SplitterDistance = 402;
            this.splitContainerLeft.TabIndex = 0;
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.Transparent;
            this.panel.Controls.Add(this.buttonSettings);
            this.panel.Controls.Add(this.labelCurrentLevel);
            this.panel.Controls.Add(this.labelCurrentLevelLabel);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(508, 32);
            this.panel.TabIndex = 0;
            // 
            // buttonSettings
            // 
            this.buttonSettings.Location = new System.Drawing.Point(422, 4);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSettings.TabIndex = 28;
            this.buttonSettings.Text = "Settings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // labelCurrentLevel
            // 
            this.labelCurrentLevel.AutoSize = true;
            this.labelCurrentLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentLevel.Location = new System.Drawing.Point(78, 8);
            this.labelCurrentLevel.Name = "labelCurrentLevel";
            this.labelCurrentLevel.Size = new System.Drawing.Size(32, 16);
            this.labelCurrentLevel.TabIndex = 26;
            this.labelCurrentLevel.Text = "???";
            // 
            // labelCurrentLevelLabel
            // 
            this.labelCurrentLevelLabel.AutoSize = true;
            this.labelCurrentLevelLabel.Location = new System.Drawing.Point(6, 9);
            this.labelCurrentLevelLabel.Name = "labelCurrentLevelLabel";
            this.labelCurrentLevelLabel.Size = new System.Drawing.Size(73, 13);
            this.labelCurrentLevelLabel.TabIndex = 24;
            this.labelCurrentLevelLabel.Text = "Current Level:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.logMessageView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(445, 438);
            this.panel1.TabIndex = 0;
            // 
            // DogDispenserController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DispenserController.Properties.Resources.Background;
            this.ClientSize = new System.Drawing.Size(969, 476);
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
            this.panel1.ResumeLayout(false);
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
        private ToolStripLedBulb toolStripLedBulbInlet;
        private ToolStripLedBulb toolStripLedBulbOutlet;
        private System.Windows.Forms.Label labelCurrentLevel;
        private System.Windows.Forms.Label labelCurrentLevelLabel;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonSettings;
    }
}

