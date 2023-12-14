namespace OMSITexManSettings
{
    partial class MainForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.settingsPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.secondsLabel = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.rulesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.texturesPage = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.addToRulesButton = new System.Windows.Forms.Button();
            this.selectedItemsLabel = new System.Windows.Forms.Label();
            this.openPreviewButton = new System.Windows.Forms.Button();
            this.memUsageLabel = new System.Windows.Forms.Label();
            this.textureListView = new System.Windows.Forms.ListView();
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.resolutionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.memoryColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.memUsageBar = new System.Windows.Forms.ProgressBar();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.helpButton = new System.Windows.Forms.Button();
            this.alwaysOnTopCB = new System.Windows.Forms.CheckBox();
            this.updateCheckBox = new System.Windows.Forms.CheckBox();
            this.updateBGWorker = new System.ComponentModel.BackgroundWorker();
            this.tabControl.SuspendLayout();
            this.settingsPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.texturesPage.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.settingsPage);
            this.tabControl.Controls.Add(this.texturesPage);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(760, 537);
            this.tabControl.TabIndex = 0;
            // 
            // settingsPage
            // 
            this.settingsPage.Controls.Add(this.groupBox2);
            this.settingsPage.Controls.Add(this.button4);
            this.settingsPage.Controls.Add(this.groupBox1);
            this.settingsPage.Controls.Add(this.rulesListView);
            this.settingsPage.Location = new System.Drawing.Point(4, 22);
            this.settingsPage.Name = "settingsPage";
            this.settingsPage.Padding = new System.Windows.Forms.Padding(3);
            this.settingsPage.Size = new System.Drawing.Size(752, 511);
            this.settingsPage.TabIndex = 0;
            this.settingsPage.Text = "Settings";
            this.settingsPage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.secondsLabel);
            this.groupBox2.Controls.Add(this.numericUpDown1);
            this.groupBox2.Location = new System.Drawing.Point(405, 457);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(341, 48);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Game Info Updates";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(181, 13);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(92, 30);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Disable Game\r\nInfo Updates";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Interval:";
            // 
            // secondsLabel
            // 
            this.secondsLabel.AutoSize = true;
            this.secondsLabel.Location = new System.Drawing.Point(128, 22);
            this.secondsLabel.Name = "secondsLabel";
            this.secondsLabel.Size = new System.Drawing.Size(47, 13);
            this.secondsLabel.TabIndex = 1;
            this.secondsLabel.Text = "seconds";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(57, 19);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(336, 457);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(65, 48);
            this.button4.TabIndex = 5;
            this.button4.Text = "Create New Rule";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(6, 457);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 48);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected Item Actions";
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(218, 19);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Move Down";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(6, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Remove";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(112, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Move Up";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // rulesListView
            // 
            this.rulesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.rulesListView.HideSelection = false;
            this.rulesListView.Location = new System.Drawing.Point(6, 6);
            this.rulesListView.Name = "rulesListView";
            this.rulesListView.Size = new System.Drawing.Size(740, 445);
            this.rulesListView.TabIndex = 2;
            this.rulesListView.UseCompatibleStateImageBehavior = false;
            this.rulesListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Index";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "E";
            this.columnHeader2.Width = 20;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name match";
            this.columnHeader3.Width = 400;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Check";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Action";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Value";
            this.columnHeader6.Width = 50;
            // 
            // texturesPage
            // 
            this.texturesPage.Controls.Add(this.groupBox3);
            this.texturesPage.Controls.Add(this.memUsageLabel);
            this.texturesPage.Controls.Add(this.textureListView);
            this.texturesPage.Controls.Add(this.memUsageBar);
            this.texturesPage.Location = new System.Drawing.Point(4, 22);
            this.texturesPage.Name = "texturesPage";
            this.texturesPage.Padding = new System.Windows.Forms.Padding(3);
            this.texturesPage.Size = new System.Drawing.Size(752, 511);
            this.texturesPage.TabIndex = 1;
            this.texturesPage.Text = "Game Textures";
            this.texturesPage.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.addToRulesButton);
            this.groupBox3.Controls.Add(this.selectedItemsLabel);
            this.groupBox3.Controls.Add(this.openPreviewButton);
            this.groupBox3.Location = new System.Drawing.Point(6, 457);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(740, 48);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Selected Item Actions";
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(218, 19);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 23);
            this.button5.TabIndex = 7;
            this.button5.Text = "Open Location";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // addToRulesButton
            // 
            this.addToRulesButton.Enabled = false;
            this.addToRulesButton.Location = new System.Drawing.Point(6, 19);
            this.addToRulesButton.Name = "addToRulesButton";
            this.addToRulesButton.Size = new System.Drawing.Size(100, 23);
            this.addToRulesButton.TabIndex = 2;
            this.addToRulesButton.Text = "Add to Rules";
            this.addToRulesButton.UseVisualStyleBackColor = true;
            // 
            // selectedItemsLabel
            // 
            this.selectedItemsLabel.AutoSize = true;
            this.selectedItemsLabel.Location = new System.Drawing.Point(324, 24);
            this.selectedItemsLabel.Name = "selectedItemsLabel";
            this.selectedItemsLabel.Size = new System.Drawing.Size(399, 13);
            this.selectedItemsLabel.TabIndex = 6;
            this.selectedItemsLabel.Text = "Total memory of 0000 selected items: 0000 MB (100.00% of loaded texture memory)";
            this.selectedItemsLabel.Visible = false;
            // 
            // openPreviewButton
            // 
            this.openPreviewButton.Enabled = false;
            this.openPreviewButton.Location = new System.Drawing.Point(112, 19);
            this.openPreviewButton.Name = "openPreviewButton";
            this.openPreviewButton.Size = new System.Drawing.Size(100, 23);
            this.openPreviewButton.TabIndex = 3;
            this.openPreviewButton.Text = "Open Preview";
            this.openPreviewButton.UseVisualStyleBackColor = true;
            // 
            // memUsageLabel
            // 
            this.memUsageLabel.AutoSize = true;
            this.memUsageLabel.Location = new System.Drawing.Point(562, 5);
            this.memUsageLabel.Name = "memUsageLabel";
            this.memUsageLabel.Size = new System.Drawing.Size(171, 26);
            this.memUsageLabel.TabIndex = 4;
            this.memUsageLabel.Text = "Loaded texture memory: 0000 MB\r\nTexture count: 0000 (0000 loaded)";
            // 
            // textureListView
            // 
            this.textureListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.resolutionColumnHeader,
            this.memoryColumnHeader});
            this.textureListView.HideSelection = false;
            this.textureListView.Location = new System.Drawing.Point(6, 35);
            this.textureListView.Name = "textureListView";
            this.textureListView.Size = new System.Drawing.Size(740, 416);
            this.textureListView.TabIndex = 1;
            this.textureListView.UseCompatibleStateImageBehavior = false;
            this.textureListView.View = System.Windows.Forms.View.Details;
            this.textureListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.textureListView_ColumnClick);
            this.textureListView.SelectedIndexChanged += new System.EventHandler(this.textureListView_SelectedIndexChanged);
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Text = "Name";
            this.nameColumnHeader.Width = 520;
            // 
            // resolutionColumnHeader
            // 
            this.resolutionColumnHeader.Text = "Resolution";
            this.resolutionColumnHeader.Width = 100;
            // 
            // memoryColumnHeader
            // 
            this.memoryColumnHeader.Text = "Memory";
            this.memoryColumnHeader.Width = 100;
            // 
            // memUsageBar
            // 
            this.memUsageBar.Location = new System.Drawing.Point(6, 6);
            this.memUsageBar.Maximum = 2048;
            this.memUsageBar.Name = "memUsageBar";
            this.memUsageBar.Size = new System.Drawing.Size(550, 23);
            this.memUsageBar.TabIndex = 0;
            // 
            // updateTimer
            // 
            this.updateTimer.Interval = 200;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // helpButton
            // 
            this.helpButton.Location = new System.Drawing.Point(718, 5);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(50, 23);
            this.helpButton.TabIndex = 7;
            this.helpButton.Text = "Help";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // alwaysOnTopCB
            // 
            this.alwaysOnTopCB.AutoSize = true;
            this.alwaysOnTopCB.Location = new System.Drawing.Point(616, 9);
            this.alwaysOnTopCB.Name = "alwaysOnTopCB";
            this.alwaysOnTopCB.Size = new System.Drawing.Size(96, 17);
            this.alwaysOnTopCB.TabIndex = 10;
            this.alwaysOnTopCB.Text = "Always on Top";
            this.alwaysOnTopCB.UseVisualStyleBackColor = true;
            this.alwaysOnTopCB.CheckedChanged += new System.EventHandler(this.alwaysOnTopCB_CheckedChanged);
            // 
            // updateCheckBox
            // 
            this.updateCheckBox.AutoSize = true;
            this.updateCheckBox.Location = new System.Drawing.Point(162, 9);
            this.updateCheckBox.Name = "updateCheckBox";
            this.updateCheckBox.Size = new System.Drawing.Size(97, 17);
            this.updateCheckBox.TabIndex = 11;
            this.updateCheckBox.Text = "Pause updates";
            this.updateCheckBox.UseVisualStyleBackColor = true;
            this.updateCheckBox.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.updateCheckBox);
            this.Controls.Add(this.alwaysOnTopCB);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "OMSITextureManager Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl.ResumeLayout(false);
            this.settingsPage.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.texturesPage.ResumeLayout(false);
            this.texturesPage.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage settingsPage;
        private System.Windows.Forms.TabPage texturesPage;
        private System.Windows.Forms.ListView textureListView;
        private System.Windows.Forms.ProgressBar memUsageBar;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader resolutionColumnHeader;
        private System.Windows.Forms.ColumnHeader memoryColumnHeader;
        private System.Windows.Forms.Button addToRulesButton;
        private System.Windows.Forms.Label memUsageLabel;
        private System.Windows.Forms.Button openPreviewButton;
        private System.Windows.Forms.Label selectedItemsLabel;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.ListView rulesListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label secondsLabel;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.CheckBox alwaysOnTopCB;
        private System.Windows.Forms.CheckBox updateCheckBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button5;
        private System.ComponentModel.BackgroundWorker updateBGWorker;
    }
}

