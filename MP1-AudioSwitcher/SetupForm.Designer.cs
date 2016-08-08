namespace MP1_AudioSwitcher
{
  partial class SetupForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupForm));
      this.lblRemoteKey = new System.Windows.Forms.Label();
      this.cbRemoteKey = new System.Windows.Forms.ComboBox();
      this.btnSaveSettings = new System.Windows.Forms.Button();
      this.cbAvailableAudioDevices = new System.Windows.Forms.ComboBox();
      this.chkEnableLAVbitstreamPerDevice = new System.Windows.Forms.CheckBox();
      this.lblBitstreamingHint = new System.Windows.Forms.Label();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageGeneric = new System.Windows.Forms.TabPage();
      this.btnClearDevice = new System.Windows.Forms.Button();
      this.btbRefreshDefaultPlaybackDevices = new System.Windows.Forms.Button();
      this.cbStartupPlaybackDevices = new System.Windows.Forms.ComboBox();
      this.lblDefaultPlaybackDevice = new System.Windows.Forms.Label();
      this.tabPageBitStream = new System.Windows.Forms.TabPage();
      this.btnRefreshDevices = new System.Windows.Forms.Button();
      this.btnRemoveDevice = new System.Windows.Forms.Button();
      this.cblSupportedBitstreamOptions = new System.Windows.Forms.CheckedListBox();
      this.lblSupportedBitstreamOptions = new System.Windows.Forms.Label();
      this.lvBitstreamDevices = new System.Windows.Forms.ListView();
      this.chDeviceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.chBitstream = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.cbhBitstreamOptions = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.btnAddDevice = new System.Windows.Forms.Button();
      this.cbBitstreamDeviceEnabled = new System.Windows.Forms.CheckBox();
      this.lblDevice = new System.Windows.Forms.Label();
      this.btnCheckAllBitstreamOptions = new System.Windows.Forms.Button();
      this.btnCheckNoBitstreamOptions = new System.Windows.Forms.Button();
      this.chkAlwaysShowLavBitstreamToggle = new System.Windows.Forms.CheckBox();
      this.tabControl.SuspendLayout();
      this.tabPageGeneric.SuspendLayout();
      this.tabPageBitStream.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblRemoteKey
      // 
      this.lblRemoteKey.AutoSize = true;
      this.lblRemoteKey.Location = new System.Drawing.Point(6, 25);
      this.lblRemoteKey.Name = "lblRemoteKey";
      this.lblRemoteKey.Size = new System.Drawing.Size(187, 13);
      this.lblRemoteKey.TabIndex = 0;
      this.lblRemoteKey.Text = "Remote key to bring up context menu:";
      // 
      // cbRemoteKey
      // 
      this.cbRemoteKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbRemoteKey.FormattingEnabled = true;
      this.cbRemoteKey.Items.AddRange(new object[] {
            "None",
            "Red",
            "Green",
            "Blue",
            "Yellow",
            "DVD menu",
            "Sub page down",
            "Sub page Up"});
      this.cbRemoteKey.Location = new System.Drawing.Point(196, 22);
      this.cbRemoteKey.Name = "cbRemoteKey";
      this.cbRemoteKey.Size = new System.Drawing.Size(149, 21);
      this.cbRemoteKey.TabIndex = 1;
      // 
      // btnSaveSettings
      // 
      this.btnSaveSettings.Location = new System.Drawing.Point(258, 449);
      this.btnSaveSettings.Name = "btnSaveSettings";
      this.btnSaveSettings.Size = new System.Drawing.Size(314, 32);
      this.btnSaveSettings.TabIndex = 2;
      this.btnSaveSettings.Text = "Save and close";
      this.btnSaveSettings.UseVisualStyleBackColor = true;
      this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
      // 
      // cbAvailableAudioDevices
      // 
      this.cbAvailableAudioDevices.Location = new System.Drawing.Point(8, 173);
      this.cbAvailableAudioDevices.Name = "cbAvailableAudioDevices";
      this.cbAvailableAudioDevices.Size = new System.Drawing.Size(270, 21);
      this.cbAvailableAudioDevices.TabIndex = 0;
      // 
      // chkEnableLAVbitstreamPerDevice
      // 
      this.chkEnableLAVbitstreamPerDevice.AutoSize = true;
      this.chkEnableLAVbitstreamPerDevice.Location = new System.Drawing.Point(9, 107);
      this.chkEnableLAVbitstreamPerDevice.Name = "chkEnableLAVbitstreamPerDevice";
      this.chkEnableLAVbitstreamPerDevice.Size = new System.Drawing.Size(228, 17);
      this.chkEnableLAVbitstreamPerDevice.TabIndex = 1;
      this.chkEnableLAVbitstreamPerDevice.Text = "Enable LAV bitstreaming setting per device";
      this.chkEnableLAVbitstreamPerDevice.UseVisualStyleBackColor = true;
      // 
      // lblBitstreamingHint
      // 
      this.lblBitstreamingHint.AutoSize = true;
      this.lblBitstreamingHint.Location = new System.Drawing.Point(6, 13);
      this.lblBitstreamingHint.Name = "lblBitstreamingHint";
      this.lblBitstreamingHint.Size = new System.Drawing.Size(683, 78);
      this.lblBitstreamingHint.TabIndex = 2;
      this.lblBitstreamingHint.Text = resources.GetString("lblBitstreamingHint.Text");
      // 
      // tabControl
      // 
      this.tabControl.Controls.Add(this.tabPageGeneric);
      this.tabControl.Controls.Add(this.tabPageBitStream);
      this.tabControl.Location = new System.Drawing.Point(0, 3);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(838, 440);
      this.tabControl.TabIndex = 4;
      // 
      // tabPageGeneric
      // 
      this.tabPageGeneric.BackColor = System.Drawing.SystemColors.Control;
      this.tabPageGeneric.Controls.Add(this.btnClearDevice);
      this.tabPageGeneric.Controls.Add(this.btbRefreshDefaultPlaybackDevices);
      this.tabPageGeneric.Controls.Add(this.cbStartupPlaybackDevices);
      this.tabPageGeneric.Controls.Add(this.lblDefaultPlaybackDevice);
      this.tabPageGeneric.Controls.Add(this.cbRemoteKey);
      this.tabPageGeneric.Controls.Add(this.lblRemoteKey);
      this.tabPageGeneric.Location = new System.Drawing.Point(4, 22);
      this.tabPageGeneric.Name = "tabPageGeneric";
      this.tabPageGeneric.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageGeneric.Size = new System.Drawing.Size(817, 414);
      this.tabPageGeneric.TabIndex = 0;
      this.tabPageGeneric.Text = "Generic";
      // 
      // btnClearDevice
      // 
      this.btnClearDevice.Location = new System.Drawing.Point(196, 88);
      this.btnClearDevice.Name = "btnClearDevice";
      this.btnClearDevice.Size = new System.Drawing.Size(127, 23);
      this.btnClearDevice.TabIndex = 14;
      this.btnClearDevice.Text = "Clear startup device";
      this.btnClearDevice.UseVisualStyleBackColor = true;
      this.btnClearDevice.Click += new System.EventHandler(this.btnClearDevice_Click);
      // 
      // btbRefreshDefaultPlaybackDevices
      // 
      this.btbRefreshDefaultPlaybackDevices.Location = new System.Drawing.Point(421, 88);
      this.btbRefreshDefaultPlaybackDevices.Name = "btbRefreshDefaultPlaybackDevices";
      this.btbRefreshDefaultPlaybackDevices.Size = new System.Drawing.Size(104, 23);
      this.btbRefreshDefaultPlaybackDevices.TabIndex = 13;
      this.btbRefreshDefaultPlaybackDevices.Text = "Refresh devices";
      this.btbRefreshDefaultPlaybackDevices.UseVisualStyleBackColor = true;
      this.btbRefreshDefaultPlaybackDevices.Click += new System.EventHandler(this.btbRefreshDefaultPlaybackDevices_Click);
      // 
      // cbStartupPlaybackDevices
      // 
      this.cbStartupPlaybackDevices.Location = new System.Drawing.Point(196, 61);
      this.cbStartupPlaybackDevices.Name = "cbStartupPlaybackDevices";
      this.cbStartupPlaybackDevices.Size = new System.Drawing.Size(329, 21);
      this.cbStartupPlaybackDevices.TabIndex = 12;
      // 
      // lblDefaultPlaybackDevice
      // 
      this.lblDefaultPlaybackDevice.AutoSize = true;
      this.lblDefaultPlaybackDevice.Location = new System.Drawing.Point(9, 64);
      this.lblDefaultPlaybackDevice.Name = "lblDefaultPlaybackDevice";
      this.lblDefaultPlaybackDevice.Size = new System.Drawing.Size(178, 13);
      this.lblDefaultPlaybackDevice.TabIndex = 2;
      this.lblDefaultPlaybackDevice.Text = "Default playback device on startup: ";
      // 
      // tabPageBitStream
      // 
      this.tabPageBitStream.BackColor = System.Drawing.SystemColors.Control;
      this.tabPageBitStream.Controls.Add(this.chkAlwaysShowLavBitstreamToggle);
      this.tabPageBitStream.Controls.Add(this.btnCheckNoBitstreamOptions);
      this.tabPageBitStream.Controls.Add(this.btnCheckAllBitstreamOptions);
      this.tabPageBitStream.Controls.Add(this.btnRefreshDevices);
      this.tabPageBitStream.Controls.Add(this.btnRemoveDevice);
      this.tabPageBitStream.Controls.Add(this.cblSupportedBitstreamOptions);
      this.tabPageBitStream.Controls.Add(this.lblSupportedBitstreamOptions);
      this.tabPageBitStream.Controls.Add(this.lvBitstreamDevices);
      this.tabPageBitStream.Controls.Add(this.btnAddDevice);
      this.tabPageBitStream.Controls.Add(this.cbBitstreamDeviceEnabled);
      this.tabPageBitStream.Controls.Add(this.lblDevice);
      this.tabPageBitStream.Controls.Add(this.lblBitstreamingHint);
      this.tabPageBitStream.Controls.Add(this.chkEnableLAVbitstreamPerDevice);
      this.tabPageBitStream.Controls.Add(this.cbAvailableAudioDevices);
      this.tabPageBitStream.Location = new System.Drawing.Point(4, 22);
      this.tabPageBitStream.Name = "tabPageBitStream";
      this.tabPageBitStream.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageBitStream.Size = new System.Drawing.Size(830, 414);
      this.tabPageBitStream.TabIndex = 1;
      this.tabPageBitStream.Text = "Bitstream";
      // 
      // btnRefreshDevices
      // 
      this.btnRefreshDevices.Location = new System.Drawing.Point(166, 200);
      this.btnRefreshDevices.Name = "btnRefreshDevices";
      this.btnRefreshDevices.Size = new System.Drawing.Size(106, 23);
      this.btnRefreshDevices.TabIndex = 11;
      this.btnRefreshDevices.Text = "Refresh devices";
      this.btnRefreshDevices.UseVisualStyleBackColor = true;
      this.btnRefreshDevices.Click += new System.EventHandler(this.btnRefreshDevices_Click);
      // 
      // btnRemoveDevice
      // 
      this.btnRemoveDevice.Location = new System.Drawing.Point(715, 376);
      this.btnRemoveDevice.Name = "btnRemoveDevice";
      this.btnRemoveDevice.Size = new System.Drawing.Size(112, 23);
      this.btnRemoveDevice.TabIndex = 10;
      this.btnRemoveDevice.Text = "Remove device";
      this.btnRemoveDevice.UseVisualStyleBackColor = true;
      this.btnRemoveDevice.Click += new System.EventHandler(this.btnRemoveDevice_Click);
      // 
      // cblSupportedBitstreamOptions
      // 
      this.cblSupportedBitstreamOptions.FormattingEnabled = true;
      this.cblSupportedBitstreamOptions.Items.AddRange(new object[] {
            "AC3",
            "DTS",
            "DTS HD",
            "EAC3",
            "TRUE HD"});
      this.cblSupportedBitstreamOptions.Location = new System.Drawing.Point(166, 233);
      this.cblSupportedBitstreamOptions.Name = "cblSupportedBitstreamOptions";
      this.cblSupportedBitstreamOptions.Size = new System.Drawing.Size(109, 109);
      this.cblSupportedBitstreamOptions.TabIndex = 9;
      this.cblSupportedBitstreamOptions.Visible = false;
      // 
      // lblSupportedBitstreamOptions
      // 
      this.lblSupportedBitstreamOptions.AutoSize = true;
      this.lblSupportedBitstreamOptions.Location = new System.Drawing.Point(6, 233);
      this.lblSupportedBitstreamOptions.Name = "lblSupportedBitstreamOptions";
      this.lblSupportedBitstreamOptions.Size = new System.Drawing.Size(141, 13);
      this.lblSupportedBitstreamOptions.TabIndex = 8;
      this.lblSupportedBitstreamOptions.Text = "Supported bitstream options:";
      this.lblSupportedBitstreamOptions.Visible = false;
      // 
      // lvBitstreamDevices
      // 
      this.lvBitstreamDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDeviceName,
            this.chBitstream,
            this.cbhBitstreamOptions});
      this.lvBitstreamDevices.Location = new System.Drawing.Point(294, 109);
      this.lvBitstreamDevices.Name = "lvBitstreamDevices";
      this.lvBitstreamDevices.Size = new System.Drawing.Size(533, 261);
      this.lvBitstreamDevices.TabIndex = 6;
      this.lvBitstreamDevices.UseCompatibleStateImageBehavior = false;
      this.lvBitstreamDevices.View = System.Windows.Forms.View.Details;
      // 
      // chDeviceName
      // 
      this.chDeviceName.Text = "Device name";
      this.chDeviceName.Width = 277;
      // 
      // chBitstream
      // 
      this.chBitstream.Text = "Bitstream";
      this.chBitstream.Width = 55;
      // 
      // cbhBitstreamOptions
      // 
      this.cbhBitstreamOptions.Text = "Bitstream options";
      this.cbhBitstreamOptions.Width = 194;
      // 
      // btnAddDevice
      // 
      this.btnAddDevice.Location = new System.Drawing.Point(11, 385);
      this.btnAddDevice.Name = "btnAddDevice";
      this.btnAddDevice.Size = new System.Drawing.Size(262, 23);
      this.btnAddDevice.TabIndex = 5;
      this.btnAddDevice.Text = "Add device";
      this.btnAddDevice.UseVisualStyleBackColor = true;
      this.btnAddDevice.Click += new System.EventHandler(this.btnAddDevice_Click);
      // 
      // cbBitstreamDeviceEnabled
      // 
      this.cbBitstreamDeviceEnabled.AutoSize = true;
      this.cbBitstreamDeviceEnabled.Location = new System.Drawing.Point(9, 200);
      this.cbBitstreamDeviceEnabled.Name = "cbBitstreamDeviceEnabled";
      this.cbBitstreamDeviceEnabled.Size = new System.Drawing.Size(69, 17);
      this.cbBitstreamDeviceEnabled.TabIndex = 4;
      this.cbBitstreamDeviceEnabled.Text = "Bitstream";
      this.cbBitstreamDeviceEnabled.UseVisualStyleBackColor = true;
      this.cbBitstreamDeviceEnabled.CheckedChanged += new System.EventHandler(this.cbBitstreamDeviceEnabled_CheckedChanged);
      // 
      // lblDevice
      // 
      this.lblDevice.AutoSize = true;
      this.lblDevice.Location = new System.Drawing.Point(8, 153);
      this.lblDevice.Name = "lblDevice";
      this.lblDevice.Size = new System.Drawing.Size(69, 13);
      this.lblDevice.TabIndex = 3;
      this.lblDevice.Text = "Audio device";
      // 
      // btnCheckAllBitstreamOptions
      // 
      this.btnCheckAllBitstreamOptions.Location = new System.Drawing.Point(166, 347);
      this.btnCheckAllBitstreamOptions.Name = "btnCheckAllBitstreamOptions";
      this.btnCheckAllBitstreamOptions.Size = new System.Drawing.Size(55, 23);
      this.btnCheckAllBitstreamOptions.TabIndex = 12;
      this.btnCheckAllBitstreamOptions.Text = "All";
      this.btnCheckAllBitstreamOptions.UseVisualStyleBackColor = true;
      this.btnCheckAllBitstreamOptions.Visible = false;
      this.btnCheckAllBitstreamOptions.Click += new System.EventHandler(this.btnCheckAllBitstreamOptions_Click);
      // 
      // btnCheckNoBitstreamOptions
      // 
      this.btnCheckNoBitstreamOptions.Location = new System.Drawing.Point(220, 347);
      this.btnCheckNoBitstreamOptions.Name = "btnCheckNoBitstreamOptions";
      this.btnCheckNoBitstreamOptions.Size = new System.Drawing.Size(55, 23);
      this.btnCheckNoBitstreamOptions.TabIndex = 13;
      this.btnCheckNoBitstreamOptions.Text = "None";
      this.btnCheckNoBitstreamOptions.UseVisualStyleBackColor = true;
      this.btnCheckNoBitstreamOptions.Visible = false;
      this.btnCheckNoBitstreamOptions.Click += new System.EventHandler(this.btnCheckNoBitstreamOptions_Click);
      // 
      // chkAlwaysShowLavBitstreamToggle
      // 
      this.chkAlwaysShowLavBitstreamToggle.AutoSize = true;
      this.chkAlwaysShowLavBitstreamToggle.Location = new System.Drawing.Point(8, 130);
      this.chkAlwaysShowLavBitstreamToggle.Name = "chkAlwaysShowLavBitstreamToggle";
      this.chkAlwaysShowLavBitstreamToggle.Size = new System.Drawing.Size(265, 17);
      this.chkAlwaysShowLavBitstreamToggle.TabIndex = 14;
      this.chkAlwaysShowLavBitstreamToggle.Text = "Always show LAV bitstream toggle in context menu";
      this.chkAlwaysShowLavBitstreamToggle.UseVisualStyleBackColor = true;
      // 
      // SetupForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(839, 493);
      this.Controls.Add(this.tabControl);
      this.Controls.Add(this.btnSaveSettings);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Name = "SetupForm";
      this.Text = "AudioSwitcher - Setup";
      this.tabControl.ResumeLayout(false);
      this.tabPageGeneric.ResumeLayout(false);
      this.tabPageGeneric.PerformLayout();
      this.tabPageBitStream.ResumeLayout(false);
      this.tabPageBitStream.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblRemoteKey;
    private System.Windows.Forms.ComboBox cbRemoteKey;
    private System.Windows.Forms.Button btnSaveSettings;
    private System.Windows.Forms.ComboBox cbAvailableAudioDevices;
    private System.Windows.Forms.CheckBox chkEnableLAVbitstreamPerDevice;
    private System.Windows.Forms.Label lblBitstreamingHint;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabPageGeneric;
    private System.Windows.Forms.TabPage tabPageBitStream;
    private System.Windows.Forms.Button btnAddDevice;
    private System.Windows.Forms.CheckBox cbBitstreamDeviceEnabled;
    private System.Windows.Forms.Label lblDevice;
    private System.Windows.Forms.ListView lvBitstreamDevices;
    private System.Windows.Forms.Label lblSupportedBitstreamOptions;
    private System.Windows.Forms.CheckedListBox cblSupportedBitstreamOptions;
    private System.Windows.Forms.ColumnHeader chDeviceName;
    private System.Windows.Forms.ColumnHeader chBitstream;
    private System.Windows.Forms.ColumnHeader cbhBitstreamOptions;
    private System.Windows.Forms.Button btnRemoveDevice;
    private System.Windows.Forms.Button btnRefreshDevices;
    private System.Windows.Forms.Button btbRefreshDefaultPlaybackDevices;
    private System.Windows.Forms.ComboBox cbStartupPlaybackDevices;
    private System.Windows.Forms.Label lblDefaultPlaybackDevice;
    private System.Windows.Forms.Button btnClearDevice;
    private System.Windows.Forms.Button btnCheckAllBitstreamOptions;
    private System.Windows.Forms.Button btnCheckNoBitstreamOptions;
    private System.Windows.Forms.CheckBox chkAlwaysShowLavBitstreamToggle;
  }
}