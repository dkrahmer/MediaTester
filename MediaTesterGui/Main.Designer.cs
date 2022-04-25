namespace KrahmerSoft.MediaTesterGui
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
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
            this.AboutButton = new System.Windows.Forms.Button();
            this.SaveOptionsButton = new System.Windows.Forms.Button();
            this.DefaultOptionsButton = new System.Windows.Forms.Button();
            this.AbortButton = new System.Windows.Forms.Button();
            this.StatisticsGroupBox = new System.Windows.Forms.GroupBox();
            this.TotalTimeRemainingLabel = new System.Windows.Forms.Label();
            this.RemainingLabel = new System.Windows.Forms.Label();
            this.ElapsedTimeLabel = new System.Windows.Forms.Label();
            this.ElapsedLabel = new System.Windows.Forms.Label();
            this.FailedBytesLabel = new System.Windows.Forms.Label();
            this.VerifiedBytesLabel = new System.Windows.Forms.Label();
            this.WrittenBytesLabel = new System.Windows.Forms.Label();
            this.ReadSpeedLabel = new System.Windows.Forms.Label();
            this.WriteSpeedLabel = new System.Windows.Forms.Label();
            this.FailedLabel = new System.Windows.Forms.Label();
            this.VerifiedLabel = new System.Windows.Forms.Label();
            this.WrittenLabel = new System.Windows.Forms.Label();
            this.ReadAverageLabel = new System.Windows.Forms.Label();
            this.WriteAverageLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.LanguageComboBox = new System.Windows.Forms.ComboBox();
            this.RemoveTempDataFilesButton = new System.Windows.Forms.Button();
            this.TestOptionsGgroupBox.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.StatisticsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // TargetLabel
            // 
            resources.ApplyResources(this.TargetLabel, "TargetLabel");
            this.TargetLabel.Name = "TargetLabel";
            // 
            // TargetTextBox
            // 
            resources.ApplyResources(this.TargetTextBox, "TargetTextBox");
            this.TargetTextBox.Name = "TargetTextBox";
            this.TargetTextBox.TextChanged += new System.EventHandler(this.TargetTextBox_TextChanged);
            // 
            // ChooseTargetButton
            // 
            resources.ApplyResources(this.ChooseTargetButton, "ChooseTargetButton");
            this.ChooseTargetButton.Name = "ChooseTargetButton";
            this.ChooseTargetButton.UseVisualStyleBackColor = true;
            this.ChooseTargetButton.Click += new System.EventHandler(this.ChooseTargetButton_Click);
            // 
            // StopProcessingOnFailureCheckBox
            // 
            resources.ApplyResources(this.StopProcessingOnFailureCheckBox, "StopProcessingOnFailureCheckBox");
            this.StopProcessingOnFailureCheckBox.Name = "StopProcessingOnFailureCheckBox";
            this.toolTip1.SetToolTip(this.StopProcessingOnFailureCheckBox, resources.GetString("StopProcessingOnFailureCheckBox.ToolTip"));
            this.StopProcessingOnFailureCheckBox.UseVisualStyleBackColor = true;
            // 
            // TotalBytesToTestLabel
            // 
            resources.ApplyResources(this.TotalBytesToTestLabel, "TotalBytesToTestLabel");
            this.TotalBytesToTestLabel.Name = "TotalBytesToTestLabel";
            // 
            // MaxBytesToTestComboBox
            // 
            this.MaxBytesToTestComboBox.FormattingEnabled = true;
            this.MaxBytesToTestComboBox.Items.AddRange(new object[] {
            resources.GetString("MaxBytesToTestComboBox.Items")});
            resources.ApplyResources(this.MaxBytesToTestComboBox, "MaxBytesToTestComboBox");
            this.MaxBytesToTestComboBox.Name = "MaxBytesToTestComboBox";
            // 
            // QuickTestAfterEachFileCheckBox
            // 
            resources.ApplyResources(this.QuickTestAfterEachFileCheckBox, "QuickTestAfterEachFileCheckBox");
            this.QuickTestAfterEachFileCheckBox.Name = "QuickTestAfterEachFileCheckBox";
            this.toolTip1.SetToolTip(this.QuickTestAfterEachFileCheckBox, resources.GetString("QuickTestAfterEachFileCheckBox.ToolTip"));
            this.QuickTestAfterEachFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // QuickFirstFailingByteMethodCheckBox
            // 
            resources.ApplyResources(this.QuickFirstFailingByteMethodCheckBox, "QuickFirstFailingByteMethodCheckBox");
            this.QuickFirstFailingByteMethodCheckBox.Name = "QuickFirstFailingByteMethodCheckBox";
            this.toolTip1.SetToolTip(this.QuickFirstFailingByteMethodCheckBox, resources.GetString("QuickFirstFailingByteMethodCheckBox.ToolTip"));
            this.QuickFirstFailingByteMethodCheckBox.UseVisualStyleBackColor = true;
            // 
            // WriteAndVerifyButton
            // 
            resources.ApplyResources(this.WriteAndVerifyButton, "WriteAndVerifyButton");
            this.WriteAndVerifyButton.Name = "WriteAndVerifyButton";
            this.toolTip1.SetToolTip(this.WriteAndVerifyButton, resources.GetString("WriteAndVerifyButton.ToolTip"));
            this.WriteAndVerifyButton.UseVisualStyleBackColor = true;
            this.WriteAndVerifyButton.Click += new System.EventHandler(this.WriteAndVerifyButton_Click);
            // 
            // VerifyOnlyButton
            // 
            resources.ApplyResources(this.VerifyOnlyButton, "VerifyOnlyButton");
            this.VerifyOnlyButton.Name = "VerifyOnlyButton";
            this.toolTip1.SetToolTip(this.VerifyOnlyButton, resources.GetString("VerifyOnlyButton.ToolTip"));
            this.VerifyOnlyButton.UseVisualStyleBackColor = true;
            this.VerifyOnlyButton.Click += new System.EventHandler(this.VerifyOnlyButton_Click);
            // 
            // TestOptionsGgroupBox
            // 
            resources.ApplyResources(this.TestOptionsGgroupBox, "TestOptionsGgroupBox");
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
            this.TestOptionsGgroupBox.Name = "TestOptionsGgroupBox";
            this.TestOptionsGgroupBox.TabStop = false;
            // 
            // SaveTestResultsFileToMediaCheckBox
            // 
            resources.ApplyResources(this.SaveTestResultsFileToMediaCheckBox, "SaveTestResultsFileToMediaCheckBox");
            this.SaveTestResultsFileToMediaCheckBox.Name = "SaveTestResultsFileToMediaCheckBox";
            this.toolTip1.SetToolTip(this.SaveTestResultsFileToMediaCheckBox, resources.GetString("SaveTestResultsFileToMediaCheckBox.ToolTip"));
            this.SaveTestResultsFileToMediaCheckBox.UseVisualStyleBackColor = true;
            // 
            // RemoveTempDataFilesUponCompletionCheckBox
            // 
            resources.ApplyResources(this.RemoveTempDataFilesUponCompletionCheckBox, "RemoveTempDataFilesUponCompletionCheckBox");
            this.RemoveTempDataFilesUponCompletionCheckBox.Name = "RemoveTempDataFilesUponCompletionCheckBox";
            this.toolTip1.SetToolTip(this.RemoveTempDataFilesUponCompletionCheckBox, resources.GetString("RemoveTempDataFilesUponCompletionCheckBox.ToolTip"));
            this.RemoveTempDataFilesUponCompletionCheckBox.UseVisualStyleBackColor = true;
            // 
            // TargetAvailableBytesLabel
            // 
            resources.ApplyResources(this.TargetAvailableBytesLabel, "TargetAvailableBytesLabel");
            this.TargetAvailableBytesLabel.Name = "TargetAvailableBytesLabel";
            // 
            // TargetTotalBytesLabel
            // 
            resources.ApplyResources(this.TargetTotalBytesLabel, "TargetTotalBytesLabel");
            this.TargetTotalBytesLabel.Name = "TargetTotalBytesLabel";
            // 
            // TargetAvailableBytesLabelLabel
            // 
            resources.ApplyResources(this.TargetAvailableBytesLabelLabel, "TargetAvailableBytesLabelLabel");
            this.TargetAvailableBytesLabelLabel.Name = "TargetAvailableBytesLabelLabel";
            // 
            // TargetTotalBytesLabelLabel
            // 
            resources.ApplyResources(this.TargetTotalBytesLabelLabel, "TargetTotalBytesLabelLabel");
            this.TargetTotalBytesLabelLabel.Name = "TargetTotalBytesLabelLabel";
            // 
            // ActivityLogTextBox
            // 
            resources.ApplyResources(this.ActivityLogTextBox, "ActivityLogTextBox");
            this.ActivityLogTextBox.Name = "ActivityLogTextBox";
            this.ActivityLogTextBox.ReadOnly = true;
            this.ActivityLogTextBox.TabStop = false;
            // 
            // StatusStrip
            // 
            this.StatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgressBar,
            this.ReadBytesPerSecondStatusLabel,
            this.WriteBytesPerSecondStatusLabel});
            resources.ApplyResources(this.StatusStrip, "StatusStrip");
            this.StatusStrip.Name = "StatusStrip";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Maximum = 1000;
            this.ProgressBar.Name = "ProgressBar";
            resources.ApplyResources(this.ProgressBar, "ProgressBar");
            // 
            // ReadBytesPerSecondStatusLabel
            // 
            this.ReadBytesPerSecondStatusLabel.Name = "ReadBytesPerSecondStatusLabel";
            resources.ApplyResources(this.ReadBytesPerSecondStatusLabel, "ReadBytesPerSecondStatusLabel");
            // 
            // WriteBytesPerSecondStatusLabel
            // 
            this.WriteBytesPerSecondStatusLabel.Name = "WriteBytesPerSecondStatusLabel";
            resources.ApplyResources(this.WriteBytesPerSecondStatusLabel, "WriteBytesPerSecondStatusLabel");
            // 
            // AboutButton
            // 
            resources.ApplyResources(this.AboutButton, "AboutButton");
            this.AboutButton.Name = "AboutButton";
            this.toolTip1.SetToolTip(this.AboutButton, resources.GetString("AboutButton.ToolTip"));
            this.AboutButton.UseVisualStyleBackColor = true;
            this.AboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // SaveOptionsButton
            // 
            resources.ApplyResources(this.SaveOptionsButton, "SaveOptionsButton");
            this.SaveOptionsButton.Name = "SaveOptionsButton";
            this.toolTip1.SetToolTip(this.SaveOptionsButton, resources.GetString("SaveOptionsButton.ToolTip"));
            this.SaveOptionsButton.UseVisualStyleBackColor = true;
            this.SaveOptionsButton.Click += new System.EventHandler(this.SaveOptions_Click);
            // 
            // DefaultOptionsButton
            // 
            resources.ApplyResources(this.DefaultOptionsButton, "DefaultOptionsButton");
            this.DefaultOptionsButton.Name = "DefaultOptionsButton";
            this.toolTip1.SetToolTip(this.DefaultOptionsButton, resources.GetString("DefaultOptionsButton.ToolTip"));
            this.DefaultOptionsButton.UseVisualStyleBackColor = true;
            this.DefaultOptionsButton.Click += new System.EventHandler(this.DefaultOptionsButton_Click);
            // 
            // AbortButton
            // 
            resources.ApplyResources(this.AbortButton, "AbortButton");
            this.AbortButton.Name = "AbortButton";
            this.toolTip1.SetToolTip(this.AbortButton, resources.GetString("AbortButton.ToolTip"));
            this.AbortButton.UseVisualStyleBackColor = true;
            this.AbortButton.Click += new System.EventHandler(this.AbortButton_Click);
            // 
            // StatisticsGroupBox
            // 
            resources.ApplyResources(this.StatisticsGroupBox, "StatisticsGroupBox");
            this.StatisticsGroupBox.Controls.Add(this.TotalTimeRemainingLabel);
            this.StatisticsGroupBox.Controls.Add(this.RemainingLabel);
            this.StatisticsGroupBox.Controls.Add(this.ElapsedTimeLabel);
            this.StatisticsGroupBox.Controls.Add(this.ElapsedLabel);
            this.StatisticsGroupBox.Controls.Add(this.FailedBytesLabel);
            this.StatisticsGroupBox.Controls.Add(this.VerifiedBytesLabel);
            this.StatisticsGroupBox.Controls.Add(this.WrittenBytesLabel);
            this.StatisticsGroupBox.Controls.Add(this.ReadSpeedLabel);
            this.StatisticsGroupBox.Controls.Add(this.WriteSpeedLabel);
            this.StatisticsGroupBox.Controls.Add(this.FailedLabel);
            this.StatisticsGroupBox.Controls.Add(this.VerifiedLabel);
            this.StatisticsGroupBox.Controls.Add(this.WrittenLabel);
            this.StatisticsGroupBox.Controls.Add(this.ReadAverageLabel);
            this.StatisticsGroupBox.Controls.Add(this.WriteAverageLabel);
            this.StatisticsGroupBox.Name = "StatisticsGroupBox";
            this.StatisticsGroupBox.TabStop = false;
            // 
            // TotalTimeRemainingLabel
            // 
            resources.ApplyResources(this.TotalTimeRemainingLabel, "TotalTimeRemainingLabel");
            this.TotalTimeRemainingLabel.Name = "TotalTimeRemainingLabel";
            // 
            // RemainingLabel
            // 
            resources.ApplyResources(this.RemainingLabel, "RemainingLabel");
            this.RemainingLabel.Name = "RemainingLabel";
            // 
            // ElapsedTimeLabel
            // 
            resources.ApplyResources(this.ElapsedTimeLabel, "ElapsedTimeLabel");
            this.ElapsedTimeLabel.Name = "ElapsedTimeLabel";
            // 
            // ElapsedLabel
            // 
            resources.ApplyResources(this.ElapsedLabel, "ElapsedLabel");
            this.ElapsedLabel.Name = "ElapsedLabel";
            // 
            // FailedBytesLabel
            // 
            resources.ApplyResources(this.FailedBytesLabel, "FailedBytesLabel");
            this.FailedBytesLabel.Name = "FailedBytesLabel";
            // 
            // VerifiedBytesLabel
            // 
            resources.ApplyResources(this.VerifiedBytesLabel, "VerifiedBytesLabel");
            this.VerifiedBytesLabel.Name = "VerifiedBytesLabel";
            // 
            // WrittenBytesLabel
            // 
            resources.ApplyResources(this.WrittenBytesLabel, "WrittenBytesLabel");
            this.WrittenBytesLabel.Name = "WrittenBytesLabel";
            // 
            // ReadSpeedLabel
            // 
            resources.ApplyResources(this.ReadSpeedLabel, "ReadSpeedLabel");
            this.ReadSpeedLabel.Name = "ReadSpeedLabel";
            // 
            // WriteSpeedLabel
            // 
            resources.ApplyResources(this.WriteSpeedLabel, "WriteSpeedLabel");
            this.WriteSpeedLabel.Name = "WriteSpeedLabel";
            // 
            // FailedLabel
            // 
            resources.ApplyResources(this.FailedLabel, "FailedLabel");
            this.FailedLabel.Name = "FailedLabel";
            // 
            // VerifiedLabel
            // 
            resources.ApplyResources(this.VerifiedLabel, "VerifiedLabel");
            this.VerifiedLabel.Name = "VerifiedLabel";
            // 
            // WrittenLabel
            // 
            resources.ApplyResources(this.WrittenLabel, "WrittenLabel");
            this.WrittenLabel.Name = "WrittenLabel";
            // 
            // ReadAverageLabel
            // 
            resources.ApplyResources(this.ReadAverageLabel, "ReadAverageLabel");
            this.ReadAverageLabel.Name = "ReadAverageLabel";
            // 
            // WriteAverageLabel
            // 
            resources.ApplyResources(this.WriteAverageLabel, "WriteAverageLabel");
            this.WriteAverageLabel.Name = "WriteAverageLabel";
            // 
            // LanguageComboBox
            // 
            resources.ApplyResources(this.LanguageComboBox, "LanguageComboBox");
            this.LanguageComboBox.DisplayMember = "Description";
            this.LanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguageComboBox.DropDownWidth = 100;
            this.LanguageComboBox.FormattingEnabled = true;
            this.LanguageComboBox.Name = "LanguageComboBox";
            this.LanguageComboBox.TabStop = false;
            this.toolTip1.SetToolTip(this.LanguageComboBox, resources.GetString("LanguageComboBox.ToolTip"));
            this.LanguageComboBox.ValueMember = "Code";
            this.LanguageComboBox.DropDown += new System.EventHandler(this.LanguageComboBox_DropDown);
            this.LanguageComboBox.DropDownClosed += new System.EventHandler(this.LanguageComboBox_DropDownClosed);
            this.LanguageComboBox.SelectedValueChanged += new System.EventHandler(this.LanguageComboBox_SelectedValueChanged);
            // 
            // RemoveTempDataFilesButton
            // 
            resources.ApplyResources(this.RemoveTempDataFilesButton, "RemoveTempDataFilesButton");
            this.RemoveTempDataFilesButton.Name = "RemoveTempDataFilesButton";
            this.toolTip1.SetToolTip(this.RemoveTempDataFilesButton, resources.GetString("RemoveTempDataFilesButton.ToolTip"));
            this.RemoveTempDataFilesButton.UseVisualStyleBackColor = true;
            this.RemoveTempDataFilesButton.Click += new System.EventHandler(this.RemoveTempDataFilesButton_Click);
            // 
            // Main
            // 
            this.AcceptButton = this.WriteAndVerifyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LanguageComboBox);
            this.Controls.Add(this.RemoveTempDataFilesButton);
            this.Controls.Add(this.StatisticsGroupBox);
            this.Controls.Add(this.DefaultOptionsButton);
            this.Controls.Add(this.SaveOptionsButton);
            this.Controls.Add(this.AboutButton);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.ActivityLogTextBox);
            this.Controls.Add(this.TestOptionsGgroupBox);
            this.Controls.Add(this.VerifyOnlyButton);
            this.Controls.Add(this.WriteAndVerifyButton);
            this.Controls.Add(this.AbortButton);
            this.Name = "Main";
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
		private System.Windows.Forms.Button AboutButton;
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
		private System.Windows.Forms.Label VerifiedLabel;
		private System.Windows.Forms.Label WrittenLabel;
		private System.Windows.Forms.Label ReadAverageLabel;
		private System.Windows.Forms.Label WriteAverageLabel;
		private System.Windows.Forms.Label FailedBytesLabel;
		private System.Windows.Forms.Label VerifiedBytesLabel;
		private System.Windows.Forms.Label WrittenBytesLabel;
		private System.Windows.Forms.Label ReadSpeedLabel;
		private System.Windows.Forms.Label WriteSpeedLabel;
		private System.Windows.Forms.Label FailedLabel;
		private System.Windows.Forms.Label TotalTimeRemainingLabel;
		private System.Windows.Forms.Label RemainingLabel;
		private System.Windows.Forms.Label ElapsedTimeLabel;
		private System.Windows.Forms.Label ElapsedLabel;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox RemoveTempDataFilesUponCompletionCheckBox;
		private System.Windows.Forms.Button RemoveTempDataFilesButton;
		private System.Windows.Forms.CheckBox SaveTestResultsFileToMediaCheckBox;
		private System.Windows.Forms.ComboBox LanguageComboBox;
	}
}

