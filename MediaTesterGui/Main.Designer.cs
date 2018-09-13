namespace MediaTesterGui
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
			this.TestOptionsGgroupBox.SuspendLayout();
			this.StatusStrip.SuspendLayout();
			this.StatisticsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// TargetLabel
			// 
			this.TargetLabel.AutoSize = true;
			this.TargetLabel.Location = new System.Drawing.Point(6, 28);
			this.TargetLabel.Name = "TargetLabel";
			this.TargetLabel.Size = new System.Drawing.Size(50, 17);
			this.TargetLabel.TabIndex = 0;
			this.TargetLabel.Text = "Target";
			this.TargetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// TargetTextBox
			// 
			this.TargetTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TargetTextBox.Location = new System.Drawing.Point(62, 27);
			this.TargetTextBox.Name = "TargetTextBox";
			this.TargetTextBox.Size = new System.Drawing.Size(231, 22);
			this.TargetTextBox.TabIndex = 1;
			this.TargetTextBox.TextChanged += new System.EventHandler(this.TargetTextBox_TextChanged);
			// 
			// ChooseTargetButton
			// 
			this.ChooseTargetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ChooseTargetButton.Location = new System.Drawing.Point(299, 26);
			this.ChooseTargetButton.Name = "ChooseTargetButton";
			this.ChooseTargetButton.Size = new System.Drawing.Size(28, 24);
			this.ChooseTargetButton.TabIndex = 2;
			this.ChooseTargetButton.Text = "...";
			this.ChooseTargetButton.UseVisualStyleBackColor = true;
			this.ChooseTargetButton.Click += new System.EventHandler(this.ChooseTargetButton_Click);
			// 
			// StopProcessingOnFailureCheckBox
			// 
			this.StopProcessingOnFailureCheckBox.AutoSize = true;
			this.StopProcessingOnFailureCheckBox.Location = new System.Drawing.Point(9, 127);
			this.StopProcessingOnFailureCheckBox.Name = "StopProcessingOnFailureCheckBox";
			this.StopProcessingOnFailureCheckBox.Size = new System.Drawing.Size(222, 21);
			this.StopProcessingOnFailureCheckBox.TabIndex = 3;
			this.StopProcessingOnFailureCheckBox.Text = "Stop processing on first failure";
			this.toolTip1.SetToolTip(this.StopProcessingOnFailureCheckBox, "Stop the test process after a single failure. (recommended)");
			this.StopProcessingOnFailureCheckBox.UseVisualStyleBackColor = true;
			// 
			// TotalBytesToTestLabel
			// 
			this.TotalBytesToTestLabel.AutoSize = true;
			this.TotalBytesToTestLabel.Location = new System.Drawing.Point(6, 100);
			this.TotalBytesToTestLabel.Name = "TotalBytesToTestLabel";
			this.TotalBytesToTestLabel.Size = new System.Drawing.Size(132, 17);
			this.TotalBytesToTestLabel.TabIndex = 5;
			this.TotalBytesToTestLabel.Text = "Total Bytes To Test";
			this.TotalBytesToTestLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// MaxBytesToTestComboBox
			// 
			this.MaxBytesToTestComboBox.FormattingEnabled = true;
			this.MaxBytesToTestComboBox.Items.AddRange(new object[] {
            "All Available Bytes"});
			this.MaxBytesToTestComboBox.Location = new System.Drawing.Point(146, 97);
			this.MaxBytesToTestComboBox.Name = "MaxBytesToTestComboBox";
			this.MaxBytesToTestComboBox.Size = new System.Drawing.Size(155, 24);
			this.MaxBytesToTestComboBox.TabIndex = 6;
			// 
			// QuickTestAfterEachFileCheckBox
			// 
			this.QuickTestAfterEachFileCheckBox.AutoSize = true;
			this.QuickTestAfterEachFileCheckBox.Location = new System.Drawing.Point(9, 155);
			this.QuickTestAfterEachFileCheckBox.Name = "QuickTestAfterEachFileCheckBox";
			this.QuickTestAfterEachFileCheckBox.Size = new System.Drawing.Size(307, 21);
			this.QuickTestAfterEachFileCheckBox.TabIndex = 7;
			this.QuickTestAfterEachFileCheckBox.Text = "Quick test after writing each GiB of test data";
			this.toolTip1.SetToolTip(this.QuickTestAfterEachFileCheckBox, "Performs brief spot checks as data is written. Enabling this will detect common f" +
        "ailures much faster. (recommended)");
			this.QuickTestAfterEachFileCheckBox.UseVisualStyleBackColor = true;
			// 
			// QuickFirstFailingByteMethodCheckBox
			// 
			this.QuickFirstFailingByteMethodCheckBox.AutoSize = true;
			this.QuickFirstFailingByteMethodCheckBox.Location = new System.Drawing.Point(9, 183);
			this.QuickFirstFailingByteMethodCheckBox.Name = "QuickFirstFailingByteMethodCheckBox";
			this.QuickFirstFailingByteMethodCheckBox.Size = new System.Drawing.Size(192, 21);
			this.QuickFirstFailingByteMethodCheckBox.TabIndex = 8;
			this.QuickFirstFailingByteMethodCheckBox.Text = "Quick find first failing byte";
			this.toolTip1.SetToolTip(this.QuickFirstFailingByteMethodCheckBox, "If a failure is encountered, skip testing all written files from the beginning. S" +
        "earch for the last successful byte in reverse instead. (recommended)");
			this.QuickFirstFailingByteMethodCheckBox.UseVisualStyleBackColor = true;
			// 
			// WriteAndVerifyButton
			// 
			this.WriteAndVerifyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.WriteAndVerifyButton.Location = new System.Drawing.Point(351, 247);
			this.WriteAndVerifyButton.Name = "WriteAndVerifyButton";
			this.WriteAndVerifyButton.Size = new System.Drawing.Size(129, 34);
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
			this.VerifyOnlyButton.Location = new System.Drawing.Point(486, 247);
			this.VerifyOnlyButton.Name = "VerifyOnlyButton";
			this.VerifyOnlyButton.Size = new System.Drawing.Size(129, 34);
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
			this.TestOptionsGgroupBox.Location = new System.Drawing.Point(12, 29);
			this.TestOptionsGgroupBox.Name = "TestOptionsGgroupBox";
			this.TestOptionsGgroupBox.Size = new System.Drawing.Size(333, 212);
			this.TestOptionsGgroupBox.TabIndex = 11;
			this.TestOptionsGgroupBox.TabStop = false;
			this.TestOptionsGgroupBox.Text = "Test Options";
			// 
			// TargetAvailableBytesLabel
			// 
			this.TargetAvailableBytesLabel.AutoSize = true;
			this.TargetAvailableBytesLabel.Location = new System.Drawing.Point(143, 77);
			this.TargetAvailableBytesLabel.Name = "TargetAvailableBytesLabel";
			this.TargetAvailableBytesLabel.Size = new System.Drawing.Size(23, 17);
			this.TargetAvailableBytesLabel.TabIndex = 12;
			this.TargetAvailableBytesLabel.Text = "---";
			// 
			// TargetTotalBytesLabel
			// 
			this.TargetTotalBytesLabel.AutoSize = true;
			this.TargetTotalBytesLabel.Location = new System.Drawing.Point(143, 56);
			this.TargetTotalBytesLabel.Name = "TargetTotalBytesLabel";
			this.TargetTotalBytesLabel.Size = new System.Drawing.Size(23, 17);
			this.TargetTotalBytesLabel.TabIndex = 11;
			this.TargetTotalBytesLabel.Text = "---";
			// 
			// TargetAvailableBytesLabelLabel
			// 
			this.TargetAvailableBytesLabelLabel.AutoSize = true;
			this.TargetAvailableBytesLabelLabel.Location = new System.Drawing.Point(6, 77);
			this.TargetAvailableBytesLabelLabel.Name = "TargetAvailableBytesLabelLabel";
			this.TargetAvailableBytesLabelLabel.Size = new System.Drawing.Size(109, 17);
			this.TargetAvailableBytesLabelLabel.TabIndex = 10;
			this.TargetAvailableBytesLabelLabel.Text = "Available Space";
			// 
			// TargetTotalBytesLabelLabel
			// 
			this.TargetTotalBytesLabelLabel.AutoSize = true;
			this.TargetTotalBytesLabelLabel.Location = new System.Drawing.Point(6, 54);
			this.TargetTotalBytesLabelLabel.Name = "TargetTotalBytesLabelLabel";
			this.TargetTotalBytesLabelLabel.Size = new System.Drawing.Size(71, 17);
			this.TargetTotalBytesLabelLabel.TabIndex = 9;
			this.TargetTotalBytesLabelLabel.Text = "Total Size";
			// 
			// ActivityLogTextBox
			// 
			this.ActivityLogTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ActivityLogTextBox.Location = new System.Drawing.Point(12, 287);
			this.ActivityLogTextBox.Multiline = true;
			this.ActivityLogTextBox.Name = "ActivityLogTextBox";
			this.ActivityLogTextBox.ReadOnly = true;
			this.ActivityLogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.ActivityLogTextBox.Size = new System.Drawing.Size(603, 124);
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
			this.StatusStrip.Location = new System.Drawing.Point(0, 424);
			this.StatusStrip.Name = "StatusStrip";
			this.StatusStrip.Size = new System.Drawing.Size(627, 25);
			this.StatusStrip.TabIndex = 13;
			// 
			// ProgressBar
			// 
			this.ProgressBar.Maximum = 1000;
			this.ProgressBar.Name = "ProgressBar";
			this.ProgressBar.Size = new System.Drawing.Size(200, 19);
			// 
			// ReadBytesPerSecondStatusLabel
			// 
			this.ReadBytesPerSecondStatusLabel.Name = "ReadBytesPerSecondStatusLabel";
			this.ReadBytesPerSecondStatusLabel.Size = new System.Drawing.Size(18, 20);
			this.ReadBytesPerSecondStatusLabel.Text = "...";
			// 
			// WriteBytesPerSecondStatusLabel
			// 
			this.WriteBytesPerSecondStatusLabel.Name = "WriteBytesPerSecondStatusLabel";
			this.WriteBytesPerSecondStatusLabel.Size = new System.Drawing.Size(18, 20);
			this.WriteBytesPerSecondStatusLabel.Text = "...";
			// 
			// AboutLinkLabel
			// 
			this.AboutLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.AboutLinkLabel.AutoSize = true;
			this.AboutLinkLabel.Location = new System.Drawing.Point(558, 9);
			this.AboutLinkLabel.Name = "AboutLinkLabel";
			this.AboutLinkLabel.Size = new System.Drawing.Size(57, 17);
			this.AboutLinkLabel.TabIndex = 15;
			this.AboutLinkLabel.TabStop = true;
			this.AboutLinkLabel.Text = "About...";
			this.AboutLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// SaveOptionsButton
			// 
			this.SaveOptionsButton.Location = new System.Drawing.Point(12, 248);
			this.SaveOptionsButton.Name = "SaveOptionsButton";
			this.SaveOptionsButton.Size = new System.Drawing.Size(129, 33);
			this.SaveOptionsButton.TabIndex = 16;
			this.SaveOptionsButton.Text = "Save Options";
			this.SaveOptionsButton.UseVisualStyleBackColor = true;
			this.SaveOptionsButton.Click += new System.EventHandler(this.SaveOptions_Click);
			// 
			// DefaultOptionsButton
			// 
			this.DefaultOptionsButton.Location = new System.Drawing.Point(147, 248);
			this.DefaultOptionsButton.Name = "DefaultOptionsButton";
			this.DefaultOptionsButton.Size = new System.Drawing.Size(129, 33);
			this.DefaultOptionsButton.TabIndex = 17;
			this.DefaultOptionsButton.Text = "Default Options";
			this.DefaultOptionsButton.UseVisualStyleBackColor = true;
			this.DefaultOptionsButton.Click += new System.EventHandler(this.DefaultOptionsButton_Click);
			// 
			// AbortButton
			// 
			this.AbortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.AbortButton.Location = new System.Drawing.Point(351, 247);
			this.AbortButton.Name = "AbortButton";
			this.AbortButton.Size = new System.Drawing.Size(264, 34);
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
			this.StatisticsGroupBox.Location = new System.Drawing.Point(351, 29);
			this.StatisticsGroupBox.Name = "StatisticsGroupBox";
			this.StatisticsGroupBox.Size = new System.Drawing.Size(264, 212);
			this.StatisticsGroupBox.TabIndex = 19;
			this.StatisticsGroupBox.TabStop = false;
			this.StatisticsGroupBox.Text = "Statistics";
			// 
			// TotalTimeRemainingLabel
			// 
			this.TotalTimeRemainingLabel.AutoSize = true;
			this.TotalTimeRemainingLabel.Location = new System.Drawing.Point(79, 171);
			this.TotalTimeRemainingLabel.Name = "TotalTimeRemainingLabel";
			this.TotalTimeRemainingLabel.Size = new System.Drawing.Size(23, 17);
			this.TotalTimeRemainingLabel.TabIndex = 13;
			this.TotalTimeRemainingLabel.Text = "---";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(7, 171);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(75, 17);
			this.label9.TabIndex = 12;
			this.label9.Text = "Remaining";
			// 
			// ElapsedTimeLabel
			// 
			this.ElapsedTimeLabel.AutoSize = true;
			this.ElapsedTimeLabel.Location = new System.Drawing.Point(79, 147);
			this.ElapsedTimeLabel.Name = "ElapsedTimeLabel";
			this.ElapsedTimeLabel.Size = new System.Drawing.Size(23, 17);
			this.ElapsedTimeLabel.TabIndex = 11;
			this.ElapsedTimeLabel.Text = "---";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(7, 147);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(59, 17);
			this.label7.TabIndex = 10;
			this.label7.Text = "Elapsed";
			// 
			// FailedBytesLabel
			// 
			this.FailedBytesLabel.AutoSize = true;
			this.FailedBytesLabel.Location = new System.Drawing.Point(79, 123);
			this.FailedBytesLabel.Name = "FailedBytesLabel";
			this.FailedBytesLabel.Size = new System.Drawing.Size(23, 17);
			this.FailedBytesLabel.TabIndex = 9;
			this.FailedBytesLabel.Text = "---";
			// 
			// VerifiedBytesLabel
			// 
			this.VerifiedBytesLabel.AutoSize = true;
			this.VerifiedBytesLabel.Location = new System.Drawing.Point(79, 99);
			this.VerifiedBytesLabel.Name = "VerifiedBytesLabel";
			this.VerifiedBytesLabel.Size = new System.Drawing.Size(23, 17);
			this.VerifiedBytesLabel.TabIndex = 8;
			this.VerifiedBytesLabel.Text = "---";
			// 
			// WrittenBytesLabel
			// 
			this.WrittenBytesLabel.AutoSize = true;
			this.WrittenBytesLabel.Location = new System.Drawing.Point(79, 51);
			this.WrittenBytesLabel.Name = "WrittenBytesLabel";
			this.WrittenBytesLabel.Size = new System.Drawing.Size(23, 17);
			this.WrittenBytesLabel.TabIndex = 7;
			this.WrittenBytesLabel.Text = "---";
			// 
			// ReadSpeedLabel
			// 
			this.ReadSpeedLabel.AutoSize = true;
			this.ReadSpeedLabel.Location = new System.Drawing.Point(79, 75);
			this.ReadSpeedLabel.Name = "ReadSpeedLabel";
			this.ReadSpeedLabel.Size = new System.Drawing.Size(23, 17);
			this.ReadSpeedLabel.TabIndex = 6;
			this.ReadSpeedLabel.Text = "---";
			// 
			// WriteSpeedLabel
			// 
			this.WriteSpeedLabel.AutoSize = true;
			this.WriteSpeedLabel.Location = new System.Drawing.Point(79, 27);
			this.WriteSpeedLabel.Name = "WriteSpeedLabel";
			this.WriteSpeedLabel.Size = new System.Drawing.Size(23, 17);
			this.WriteSpeedLabel.TabIndex = 5;
			this.WriteSpeedLabel.Text = "---";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(7, 123);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(46, 17);
			this.label5.TabIndex = 4;
			this.label5.Text = "Failed";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(7, 99);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 17);
			this.label4.TabIndex = 3;
			this.label4.Text = "Verified";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(7, 52);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 17);
			this.label3.TabIndex = 2;
			this.label3.Text = "Written";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 76);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 17);
			this.label2.TabIndex = 1;
			this.label2.Text = "Read Avg";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Write Avg";
			// 
			// Main
			// 
			this.AcceptButton = this.WriteAndVerifyButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(627, 449);
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
			this.MinimumSize = new System.Drawing.Size(645, 405);
			this.Name = "Main";
			this.Text = "Media Tester";
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
	}
}

