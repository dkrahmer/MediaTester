namespace KrahmerSoft.MediaTester
{
	partial class Main
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
			this.TargetLabel = new System.Windows.Forms.Label();
			this.TargetTextBox = new System.Windows.Forms.TextBox();
			this.ChooseTargetButton = new System.Windows.Forms.Button();
			this.StopProcessingOnFailureCheckBox = new System.Windows.Forms.CheckBox();
			this.TotalBytesToTestLabel = new System.Windows.Forms.Label();
			this.MaxBytesToTestComboBox = new System.Windows.Forms.ComboBox();
			this.QuickTestAfterEachFileCheckBox = new System.Windows.Forms.CheckBox();
			this.QuickFirstFailingByteMethodCheckBox = new System.Windows.Forms.CheckBox();
			this.WriteAndVerifyButton = new System.Windows.Forms.Button();
			this.VerifyOnlyButton = new System.Windows.Forms.Button();
			this.TestOptionsGgroupBox = new System.Windows.Forms.GroupBox();
			this.SaveTestResultsFileToMediaCheckBox = new System.Windows.Forms.CheckBox();
			this.RemoveTempDataFilesUponCompletionCheckBox = new System.Windows.Forms.CheckBox();
			this.TargetAvailableBytesLabel = new System.Windows.Forms.Label();
			this.TargetTotalBytesLabel = new System.Windows.Forms.Label();
			this.TargetAvailableBytesLabelLabel = new System.Windows.Forms.Label();
			this.TargetTotalBytesLabelLabel = new System.Windows.Forms.Label();
			this.ActivityLogTextBox = new System.Windows.Forms.TextBox();
			this.StatusStrip = new System.Windows.Forms.StatusStrip();
			this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.ReadBytesPerSecondStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.WriteBytesPerSecondStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.AboutLinkLabel = new System.Windows.Forms.LinkLabel();
			this.SaveOptionsButton = new System.Windows.Forms.Button();
			this.DefaultOptionsButton = new System.Windows.Forms.Button();
			this.AbortButton = new System.Windows.Forms.Button();
			this.StatisticsGroupBox = new System.Windows.Forms.GroupBox();
			this.TotalTimeRemainingLabel = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.ElapsedTimeLabel = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.FailedBytesLabel = new System.Windows.Forms.Label();
			this.VerifiedBytesLabel = new System.Windows.Forms.Label();
			this.WrittenBytesLabel = new System.Windows.Forms.Label();
			this.ReadSpeedLabel = new System.Windows.Forms.Label();
			this.WriteSpeedLabel = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.RemoveTempDataFilesButton = new System.Windows.Forms.Button();
			this.TestOptionsGgroupBox.SuspendLayout();
			this.StatusStrip.SuspendLayout();
			this.StatisticsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// TargetLabel
			// 
			this.TargetLabel.AutoSize = true;
			this.TargetLabel.Location = new System.Drawing.Point(4, 23);
			this.TargetLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.TargetLabel.Name = "TargetLabel";
			this.TargetLabel.Size = new System.Drawing.Size(38, 13);
			this.TargetLabel.TabIndex = 0;
			this.TargetLabel.Text = "Target";
			this.TargetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// TargetTextBox
			// 
			this.TargetTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TargetTextBox.Location = new System.Drawing.Point(46, 22);
			this.TargetTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.TargetTextBox.Name = "TargetTextBox";
			this.TargetTextBox.Size = new System.Drawing.Size(167, 20);
			this.TargetTextBox.TabIndex = 1;
			this.TargetTextBox.TextChanged += new System.EventHandler(this.TargetTextBox_TextChanged);
			// 
			// ChooseTargetButton
			// 
			this.ChooseTargetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ChooseTargetButton.Location = new System.Drawing.Point(217, 21);
			this.ChooseTargetButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.ChooseTargetButton.Name = "ChooseTargetButton";
			this.ChooseTargetButton.Size = new System.Drawing.Size(21, 20);
			this.ChooseTargetButton.TabIndex = 2;
			this.ChooseTargetButton.Text = "...";
			this.ChooseTargetButton.UseVisualStyleBackColor = true;
			this.ChooseTargetButton.Click += new System.EventHandler(this.ChooseTargetButton_Click);
			// 
			// StopProcessingOnFailureCheckBox
			// 
			this.StopProcessingOnFailureCheckBox.AutoSize = true;
			this.StopProcessingOnFailureCheckBox.Location = new System.Drawing.Point(7, 97);
			this.StopProcessingOnFailureCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.StopProcessingOnFailureCheckBox.Name = "StopProcessingOnFailureCheckBox";
			this.StopProcessingOnFailureCheckBox.Size = new System.Drawing.Size(167, 17);
			this.StopProcessingOnFailureCheckBox.TabIndex = 3;
			this.StopProcessingOnFailureCheckBox.Text = "Stop processing on first failure";
			this.toolTip1.SetToolTip(this.StopProcessingOnFailureCheckBox, "Stop the test process after a single failure. (recommended)");
			this.StopProcessingOnFailureCheckBox.UseVisualStyleBackColor = true;
			// 
			// TotalBytesToTestLabel
			// 
			this.TotalBytesToTestLabel.AutoSize = true;
			this.TotalBytesToTestLabel.Location = new System.Drawing.Point(4, 76);
			this.TotalBytesToTestLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.TotalBytesToTestLabel.Name = "TotalBytesToTestLabel";
			this.TotalBytesToTestLabel.Size = new System.Drawing.Size(100, 13);
			this.TotalBytesToTestLabel.TabIndex = 5;
			this.TotalBytesToTestLabel.Text = "Total Bytes To Test";
			this.TotalBytesToTestLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// MaxBytesToTestComboBox
			// 
			this.MaxBytesToTestComboBox.FormattingEnabled = true;
			this.MaxBytesToTestComboBox.Items.AddRange(new object[] {
            "All Available Bytes"});
			this.MaxBytesToTestComboBox.Location = new System.Drawing.Point(110, 74);
			this.MaxBytesToTestComboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.MaxBytesToTestComboBox.Name = "MaxBytesToTestComboBox";
			this.MaxBytesToTestComboBox.Size = new System.Drawing.Size(117, 21);
			this.MaxBytesToTestComboBox.TabIndex = 6;
			// 
			// QuickTestAfterEachFileCheckBox
			// 
			this.QuickTestAfterEachFileCheckBox.AutoSize = true;
			this.QuickTestAfterEachFileCheckBox.Location = new System.Drawing.Point(7, 116);
			this.QuickTestAfterEachFileCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.QuickTestAfterEachFileCheckBox.Name = "QuickTestAfterEachFileCheckBox";
			this.QuickTestAfterEachFileCheckBox.Size = new System.Drawing.Size(234, 17);
			this.QuickTestAfterEachFileCheckBox.TabIndex = 7;
			this.QuickTestAfterEachFileCheckBox.Text = "Quick test after writing each GiB of test data";
			this.toolTip1.SetToolTip(this.QuickTestAfterEachFileCheckBox, "Performs brief spot checks as data is written. Enabling this will detect common f" +
        "ailures much faster. (recommended)");
			this.QuickTestAfterEachFileCheckBox.UseVisualStyleBackColor = true;
			// 
			// QuickFirstFailingByteMethodCheckBox
			// 
			this.QuickFirstFailingByteMethodCheckBox.AutoSize = true;
			this.QuickFirstFailingByteMethodCheckBox.Location = new System.Drawing.Point(7, 136);
			this.QuickFirstFailingByteMethodCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.QuickFirstFailingByteMethodCheckBox.Name = "QuickFirstFailingByteMethodCheckBox";
			this.QuickFirstFailingByteMethodCheckBox.Size = new System.Drawing.Size(146, 17);
			this.QuickFirstFailingByteMethodCheckBox.TabIndex = 8;
			this.QuickFirstFailingByteMethodCheckBox.Text = "Quick find first failing byte";
			this.toolTip1.SetToolTip(this.QuickFirstFailingByteMethodCheckBox, "If a failure is encountered, skip testing all written files from the beginning. S" +
        "earch for the last successful byte in reverse instead. (recommended)");
			this.QuickFirstFailingByteMethodCheckBox.UseVisualStyleBackColor = true;
			// 
			// WriteAndVerifyButton
			// 
			this.WriteAndVerifyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.WriteAndVerifyButton.Location = new System.Drawing.Point(257, 222);
			this.WriteAndVerifyButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.WriteAndVerifyButton.Name = "WriteAndVerifyButton";
			this.WriteAndVerifyButton.Size = new System.Drawing.Size(97, 28);
			this.WriteAndVerifyButton.TabIndex = 9;
			this.WriteAndVerifyButton.Text = "Write and Verify";
			this.toolTip1.SetToolTip(this.WriteAndVerifyButton, "Write test data to media then read back to verify data was stored correctly. Only" +
        " available space will be used. Non-destructive.");
			this.WriteAndVerifyButton.UseVisualStyleBackColor = true;
			this.WriteAndVerifyButton.Click += new System.EventHandler(this.WriteAndVerifyButton_Click);
			// 
			// VerifyOnlyButton
			// 
			this.VerifyOnlyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.VerifyOnlyButton.Location = new System.Drawing.Point(358, 222);
			this.VerifyOnlyButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.VerifyOnlyButton.Name = "VerifyOnlyButton";
			this.VerifyOnlyButton.Size = new System.Drawing.Size(97, 28);
			this.VerifyOnlyButton.TabIndex = 10;
			this.VerifyOnlyButton.Text = "Verify Only";
			this.toolTip1.SetToolTip(this.VerifyOnlyButton, "Verify previous written test data. Will not write anything to media.");
			this.VerifyOnlyButton.UseVisualStyleBackColor = true;
			this.VerifyOnlyButton.Click += new System.EventHandler(this.VerifyOnlyButton_Click);
			// 
			// TestOptionsGgroupBox
			// 
			this.TestOptionsGgroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TestOptionsGgroupBox.Controls.Add(this.SaveTestResultsFileToMediaCheckBox);
			this.TestOptionsGgroupBox.Controls.Add(this.RemoveTempDataFilesUponCompletionCheckBox);
			this.TestOptionsGgroupBox.Controls.Add(this.TargetAvailableBytesLabel);
			this.TestOptionsGgroupBox.Controls.Add(this.TargetTotalBytesLabel);
			this.TestOptionsGgroupBox.Controls.Add(this.TargetAvailableBytesLabelLabel);
			this.TestOptionsGgroupBox.Controls.Add(this.TargetTotalBytesLabelLabel);
			this.TestOptionsGgroupBox.Controls.Add(this.TotalBytesToTestLabel);
			this.TestOptionsGgroupBox.Controls.Add(this.StopProcessingOnFailureCheckBox);
			this.TestOptionsGgroupBox.Controls.Add(this.MaxBytesToTestComboBox);
			this.TestOptionsGgroupBox.Controls.Add(this.QuickFirstFailingByteMethodCheckBox);
			this.TestOptionsGgroupBox.Controls.Add(this.TargetTextBox);
			this.TestOptionsGgroupBox.Controls.Add(this.ChooseTargetButton);
			this.TestOptionsGgroupBox.Controls.Add(this.TargetLabel);
			this.TestOptionsGgroupBox.Controls.Add(this.QuickTestAfterEachFileCheckBox);
			this.TestOptionsGgroupBox.Location = new System.Drawing.Point(9, 24);
			this.TestOptionsGgroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.TestOptionsGgroupBox.Name = "TestOptionsGgroupBox";
			this.TestOptionsGgroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.TestOptionsGgroupBox.Size = new System.Drawing.Size(243, 193);
			this.TestOptionsGgroupBox.TabIndex = 11;
			this.TestOptionsGgroupBox.TabStop = false;
			this.TestOptionsGgroupBox.Text = "Test Options";
			// 
			// SaveTestResultsFileToMediaCheckBox
			// 
			this.SaveTestResultsFileToMediaCheckBox.AutoSize = true;
			this.SaveTestResultsFileToMediaCheckBox.Location = new System.Drawing.Point(7, 173);
			this.SaveTestResultsFileToMediaCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.SaveTestResultsFileToMediaCheckBox.Name = "SaveTestResultsFileToMediaCheckBox";
			this.SaveTestResultsFileToMediaCheckBox.Size = new System.Drawing.Size(184, 17);
			this.SaveTestResultsFileToMediaCheckBox.TabIndex = 14;
			this.SaveTestResultsFileToMediaCheckBox.Text = "Save test results file to media root";
			this.toolTip1.SetToolTip(this.SaveTestResultsFileToMediaCheckBox, "Save test results file to media with full test log output. MediaTesterResults_YYY" +
        "Y-MM-DD_hh-mm-ss.txt\nTemp test data files will be removed, if necessary, to crea" +
        "te space for the test results file.");
			this.SaveTestResultsFileToMediaCheckBox.UseVisualStyleBackColor = true;
			// 
			// RemoveTempDataFilesUponCompletionCheckBox
			// 
			this.RemoveTempDataFilesUponCompletionCheckBox.AutoSize = true;
			this.RemoveTempDataFilesUponCompletionCheckBox.Location = new System.Drawing.Point(7, 154);
			this.RemoveTempDataFilesUponCompletionCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.RemoveTempDataFilesUponCompletionCheckBox.Name = "RemoveTempDataFilesUponCompletionCheckBox";
			this.RemoveTempDataFilesUponCompletionCheckBox.Size = new System.Drawing.Size(218, 17);
			this.RemoveTempDataFilesUponCompletionCheckBox.TabIndex = 13;
			this.RemoveTempDataFilesUponCompletionCheckBox.Text = "Remove temp data files upon completion";
			this.toolTip1.SetToolTip(this.RemoveTempDataFilesUponCompletionCheckBox, "Remove temporary data files after verification completion (pass or fail).");
			this.RemoveTempDataFilesUponCompletionCheckBox.UseVisualStyleBackColor = true;
			// 
			// TargetAvailableBytesLabel
			// 
			this.TargetAvailableBytesLabel.AutoSize = true;
			this.TargetAvailableBytesLabel.Location = new System.Drawing.Point(107, 59);
			this.TargetAvailableBytesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.TargetAvailableBytesLabel.Name = "TargetAvailableBytesLabel";
			this.TargetAvailableBytesLabel.Size = new System.Drawing.Size(16, 13);
			this.TargetAvailableBytesLabel.TabIndex = 12;
			this.TargetAvailableBytesLabel.Text = "---";
			// 
			// TargetTotalBytesLabel
			// 
			this.TargetTotalBytesLabel.AutoSize = true;
			this.TargetTotalBytesLabel.Location = new System.Drawing.Point(107, 44);
			this.TargetTotalBytesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.TargetTotalBytesLabel.Name = "TargetTotalBytesLabel";
			this.TargetTotalBytesLabel.Size = new System.Drawing.Size(16, 13);
			this.TargetTotalBytesLabel.TabIndex = 11;
			this.TargetTotalBytesLabel.Text = "---";
			// 
			// TargetAvailableBytesLabelLabel
			// 
			this.TargetAvailableBytesLabelLabel.AutoSize = true;
			this.TargetAvailableBytesLabelLabel.Location = new System.Drawing.Point(4, 59);
			this.TargetAvailableBytesLabelLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.TargetAvailableBytesLabelLabel.Name = "TargetAvailableBytesLabelLabel";
			this.TargetAvailableBytesLabelLabel.Size = new System.Drawing.Size(84, 13);
			this.TargetAvailableBytesLabelLabel.TabIndex = 10;
			this.TargetAvailableBytesLabelLabel.Text = "Available Space";
			// 
			// TargetTotalBytesLabelLabel
			// 
			this.TargetTotalBytesLabelLabel.AutoSize = true;
			this.TargetTotalBytesLabelLabel.Location = new System.Drawing.Point(4, 42);
			this.TargetTotalBytesLabelLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.TargetTotalBytesLabelLabel.Name = "TargetTotalBytesLabelLabel";
			this.TargetTotalBytesLabelLabel.Size = new System.Drawing.Size(54, 13);
			this.TargetTotalBytesLabelLabel.TabIndex = 9;
			this.TargetTotalBytesLabelLabel.Text = "Total Size";
			// 
			// ActivityLogTextBox
			// 
			this.ActivityLogTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ActivityLogTextBox.Location = new System.Drawing.Point(9, 254);
			this.ActivityLogTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.ActivityLogTextBox.Multiline = true;
			this.ActivityLogTextBox.Name = "ActivityLogTextBox";
			this.ActivityLogTextBox.ReadOnly = true;
			this.ActivityLogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.ActivityLogTextBox.Size = new System.Drawing.Size(447, 112);
			this.ActivityLogTextBox.TabIndex = 12;
			this.ActivityLogTextBox.WordWrap = false;
			// 
			// StatusStrip
			// 
			this.StatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgressBar,
            this.ReadBytesPerSecondStatusLabel,
            this.WriteBytesPerSecondStatusLabel});
			this.StatusStrip.Location = new System.Drawing.Point(0, 374);
			this.StatusStrip.Name = "StatusStrip";
			this.StatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
			this.StatusStrip.Size = new System.Drawing.Size(465, 22);
			this.StatusStrip.TabIndex = 13;
			// 
			// ProgressBar
			// 
			this.ProgressBar.Maximum = 1000;
			this.ProgressBar.Name = "ProgressBar";
			this.ProgressBar.Size = new System.Drawing.Size(150, 16);
			// 
			// ReadBytesPerSecondStatusLabel
			// 
			this.ReadBytesPerSecondStatusLabel.Name = "ReadBytesPerSecondStatusLabel";
			this.ReadBytesPerSecondStatusLabel.Size = new System.Drawing.Size(16, 17);
			this.ReadBytesPerSecondStatusLabel.Text = "...";
			// 
			// WriteBytesPerSecondStatusLabel
			// 
			this.WriteBytesPerSecondStatusLabel.Name = "WriteBytesPerSecondStatusLabel";
			this.WriteBytesPerSecondStatusLabel.Size = new System.Drawing.Size(16, 17);
			this.WriteBytesPerSecondStatusLabel.Text = "...";
			// 
			// AboutLinkLabel
			// 
			this.AboutLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.AboutLinkLabel.AutoSize = true;
			this.AboutLinkLabel.Location = new System.Drawing.Point(412, 7);
			this.AboutLinkLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.AboutLinkLabel.Name = "AboutLinkLabel";
			this.AboutLinkLabel.Size = new System.Drawing.Size(44, 13);
			this.AboutLinkLabel.TabIndex = 15;
			this.AboutLinkLabel.TabStop = true;
			this.AboutLinkLabel.Text = "About...";
			this.AboutLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.AboutLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AboutLinkLabel_LinkClicked);
			// 
			// SaveOptionsButton
			// 
			this.SaveOptionsButton.Location = new System.Drawing.Point(9, 222);
			this.SaveOptionsButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.SaveOptionsButton.Name = "SaveOptionsButton";
			this.SaveOptionsButton.Size = new System.Drawing.Size(97, 28);
			this.SaveOptionsButton.TabIndex = 16;
			this.SaveOptionsButton.Text = "Save Options";
			this.SaveOptionsButton.UseVisualStyleBackColor = true;
			this.SaveOptionsButton.Click += new System.EventHandler(this.SaveOptions_Click);
			// 
			// DefaultOptionsButton
			// 
			this.DefaultOptionsButton.Location = new System.Drawing.Point(110, 222);
			this.DefaultOptionsButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.DefaultOptionsButton.Name = "DefaultOptionsButton";
			this.DefaultOptionsButton.Size = new System.Drawing.Size(97, 28);
			this.DefaultOptionsButton.TabIndex = 17;
			this.DefaultOptionsButton.Text = "Default Options";
			this.DefaultOptionsButton.UseVisualStyleBackColor = true;
			this.DefaultOptionsButton.Click += new System.EventHandler(this.DefaultOptionsButton_Click);
			// 
			// AbortButton
			// 
			this.AbortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.AbortButton.Location = new System.Drawing.Point(257, 222);
			this.AbortButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.AbortButton.Name = "AbortButton";
			this.AbortButton.Size = new System.Drawing.Size(198, 28);
			this.AbortButton.TabIndex = 18;
			this.AbortButton.Text = "Abort Operation";
			this.AbortButton.UseVisualStyleBackColor = true;
			this.AbortButton.Visible = false;
			this.AbortButton.Click += new System.EventHandler(this.AbortButton_Click);
			// 
			// StatisticsGroupBox
			// 
			this.StatisticsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.StatisticsGroupBox.Controls.Add(this.TotalTimeRemainingLabel);
			this.StatisticsGroupBox.Controls.Add(this.label9);
			this.StatisticsGroupBox.Controls.Add(this.ElapsedTimeLabel);
			this.StatisticsGroupBox.Controls.Add(this.label7);
			this.StatisticsGroupBox.Controls.Add(this.FailedBytesLabel);
			this.StatisticsGroupBox.Controls.Add(this.VerifiedBytesLabel);
			this.StatisticsGroupBox.Controls.Add(this.WrittenBytesLabel);
			this.StatisticsGroupBox.Controls.Add(this.ReadSpeedLabel);
			this.StatisticsGroupBox.Controls.Add(this.WriteSpeedLabel);
			this.StatisticsGroupBox.Controls.Add(this.label5);
			this.StatisticsGroupBox.Controls.Add(this.label4);
			this.StatisticsGroupBox.Controls.Add(this.label3);
			this.StatisticsGroupBox.Controls.Add(this.label2);
			this.StatisticsGroupBox.Controls.Add(this.label1);
			this.StatisticsGroupBox.Location = new System.Drawing.Point(257, 24);
			this.StatisticsGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.StatisticsGroupBox.Name = "StatisticsGroupBox";
			this.StatisticsGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.StatisticsGroupBox.Size = new System.Drawing.Size(198, 160);
			this.StatisticsGroupBox.TabIndex = 19;
			this.StatisticsGroupBox.TabStop = false;
			this.StatisticsGroupBox.Text = "Statistics";
			// 
			// TotalTimeRemainingLabel
			// 
			this.TotalTimeRemainingLabel.AutoSize = true;
			this.TotalTimeRemainingLabel.Location = new System.Drawing.Point(59, 139);
			this.TotalTimeRemainingLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.TotalTimeRemainingLabel.Name = "TotalTimeRemainingLabel";
			this.TotalTimeRemainingLabel.Size = new System.Drawing.Size(16, 13);
			this.TotalTimeRemainingLabel.TabIndex = 13;
			this.TotalTimeRemainingLabel.Text = "---";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(5, 139);
			this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(57, 13);
			this.label9.TabIndex = 12;
			this.label9.Text = "Remaining";
			// 
			// ElapsedTimeLabel
			// 
			this.ElapsedTimeLabel.AutoSize = true;
			this.ElapsedTimeLabel.Location = new System.Drawing.Point(59, 119);
			this.ElapsedTimeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.ElapsedTimeLabel.Name = "ElapsedTimeLabel";
			this.ElapsedTimeLabel.Size = new System.Drawing.Size(16, 13);
			this.ElapsedTimeLabel.TabIndex = 11;
			this.ElapsedTimeLabel.Text = "---";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(5, 119);
			this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(45, 13);
			this.label7.TabIndex = 10;
			this.label7.Text = "Elapsed";
			// 
			// FailedBytesLabel
			// 
			this.FailedBytesLabel.AutoSize = true;
			this.FailedBytesLabel.Location = new System.Drawing.Point(59, 100);
			this.FailedBytesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.FailedBytesLabel.Name = "FailedBytesLabel";
			this.FailedBytesLabel.Size = new System.Drawing.Size(16, 13);
			this.FailedBytesLabel.TabIndex = 9;
			this.FailedBytesLabel.Text = "---";
			// 
			// VerifiedBytesLabel
			// 
			this.VerifiedBytesLabel.AutoSize = true;
			this.VerifiedBytesLabel.Location = new System.Drawing.Point(59, 80);
			this.VerifiedBytesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VerifiedBytesLabel.Name = "VerifiedBytesLabel";
			this.VerifiedBytesLabel.Size = new System.Drawing.Size(16, 13);
			this.VerifiedBytesLabel.TabIndex = 8;
			this.VerifiedBytesLabel.Text = "---";
			// 
			// WrittenBytesLabel
			// 
			this.WrittenBytesLabel.AutoSize = true;
			this.WrittenBytesLabel.Location = new System.Drawing.Point(59, 41);
			this.WrittenBytesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.WrittenBytesLabel.Name = "WrittenBytesLabel";
			this.WrittenBytesLabel.Size = new System.Drawing.Size(16, 13);
			this.WrittenBytesLabel.TabIndex = 7;
			this.WrittenBytesLabel.Text = "---";
			// 
			// ReadSpeedLabel
			// 
			this.ReadSpeedLabel.AutoSize = true;
			this.ReadSpeedLabel.Location = new System.Drawing.Point(59, 61);
			this.ReadSpeedLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.ReadSpeedLabel.Name = "ReadSpeedLabel";
			this.ReadSpeedLabel.Size = new System.Drawing.Size(16, 13);
			this.ReadSpeedLabel.TabIndex = 6;
			this.ReadSpeedLabel.Text = "---";
			// 
			// WriteSpeedLabel
			// 
			this.WriteSpeedLabel.AutoSize = true;
			this.WriteSpeedLabel.Location = new System.Drawing.Point(59, 22);
			this.WriteSpeedLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.WriteSpeedLabel.Name = "WriteSpeedLabel";
			this.WriteSpeedLabel.Size = new System.Drawing.Size(16, 13);
			this.WriteSpeedLabel.TabIndex = 5;
			this.WriteSpeedLabel.Text = "---";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(5, 100);
			this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(35, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Failed";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(5, 80);
			this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(42, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Verified";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(5, 42);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(41, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Written";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(5, 62);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Read Avg";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(5, 22);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Write Avg";
			// 
			// RemoveTempDataFilesButton
			// 
			this.RemoveTempDataFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.RemoveTempDataFilesButton.Location = new System.Drawing.Point(257, 188);
			this.RemoveTempDataFilesButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.RemoveTempDataFilesButton.Name = "RemoveTempDataFilesButton";
			this.RemoveTempDataFilesButton.Size = new System.Drawing.Size(198, 28);
			this.RemoveTempDataFilesButton.TabIndex = 20;
			this.RemoveTempDataFilesButton.Text = "Remove temp data files";
			this.RemoveTempDataFilesButton.UseVisualStyleBackColor = true;
			this.RemoveTempDataFilesButton.Click += new System.EventHandler(this.RemoveTempDataFilesButton_Click);
			// 
			// Main
			// 
			this.AcceptButton = this.WriteAndVerifyButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(465, 396);
			this.Controls.Add(this.RemoveTempDataFilesButton);
			this.Controls.Add(this.StatisticsGroupBox);
			this.Controls.Add(this.DefaultOptionsButton);
			this.Controls.Add(this.SaveOptionsButton);
			this.Controls.Add(this.AboutLinkLabel);
			this.Controls.Add(this.StatusStrip);
			this.Controls.Add(this.ActivityLogTextBox);
			this.Controls.Add(this.TestOptionsGgroupBox);
			this.Controls.Add(this.VerifyOnlyButton);
			this.Controls.Add(this.WriteAndVerifyButton);
			this.Controls.Add(this.AbortButton);
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.MinimumSize = new System.Drawing.Size(481, 371);
			this.Name = "Main";
			this.Text = "MediaTester";
			this.Load += new System.EventHandler(this.Main_Load);
			this.TestOptionsGgroupBox.ResumeLayout(false);
			this.TestOptionsGgroupBox.PerformLayout();
			this.StatusStrip.ResumeLayout(false);
			this.StatusStrip.PerformLayout();
			this.StatisticsGroupBox.ResumeLayout(false);
			this.StatisticsGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label TargetLabel;
		private System.Windows.Forms.TextBox TargetTextBox;
		private System.Windows.Forms.Button ChooseTargetButton;
		private System.Windows.Forms.CheckBox StopProcessingOnFailureCheckBox;
		private System.Windows.Forms.Label TotalBytesToTestLabel;
		private System.Windows.Forms.ComboBox MaxBytesToTestComboBox;
		private System.Windows.Forms.CheckBox QuickTestAfterEachFileCheckBox;
		private System.Windows.Forms.CheckBox QuickFirstFailingByteMethodCheckBox;
		private System.Windows.Forms.Button WriteAndVerifyButton;
		private System.Windows.Forms.Button VerifyOnlyButton;
		private System.Windows.Forms.GroupBox TestOptionsGgroupBox;
		private System.Windows.Forms.TextBox ActivityLogTextBox;
		private System.Windows.Forms.StatusStrip StatusStrip;
		private System.Windows.Forms.LinkLabel AboutLinkLabel;
		private System.Windows.Forms.Label TargetTotalBytesLabelLabel;
		private System.Windows.Forms.Label TargetAvailableBytesLabelLabel;
		private System.Windows.Forms.Label TargetAvailableBytesLabel;
		private System.Windows.Forms.Label TargetTotalBytesLabel;
		private System.Windows.Forms.Button SaveOptionsButton;
		private System.Windows.Forms.Button DefaultOptionsButton;
		private System.Windows.Forms.Button AbortButton;
		private System.Windows.Forms.ToolStripProgressBar ProgressBar;
		private System.Windows.Forms.ToolStripStatusLabel ReadBytesPerSecondStatusLabel;
		private System.Windows.Forms.ToolStripStatusLabel WriteBytesPerSecondStatusLabel;
		private System.Windows.Forms.GroupBox StatisticsGroupBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label FailedBytesLabel;
		private System.Windows.Forms.Label VerifiedBytesLabel;
		private System.Windows.Forms.Label WrittenBytesLabel;
		private System.Windows.Forms.Label ReadSpeedLabel;
		private System.Windows.Forms.Label WriteSpeedLabel;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label TotalTimeRemainingLabel;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label ElapsedTimeLabel;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox RemoveTempDataFilesUponCompletionCheckBox;
		private System.Windows.Forms.Button RemoveTempDataFilesButton;
		private System.Windows.Forms.CheckBox SaveTestResultsFileToMediaCheckBox;
	}
}

