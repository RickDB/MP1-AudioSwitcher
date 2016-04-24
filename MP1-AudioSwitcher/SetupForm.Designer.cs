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
      this.SuspendLayout();
      // 
      // lblRemoteKey
      // 
      this.lblRemoteKey.AutoSize = true;
      this.lblRemoteKey.Location = new System.Drawing.Point(12, 34);
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
      this.cbRemoteKey.Location = new System.Drawing.Point(202, 31);
      this.cbRemoteKey.Name = "cbRemoteKey";
      this.cbRemoteKey.Size = new System.Drawing.Size(118, 21);
      this.cbRemoteKey.TabIndex = 1;
      // 
      // btnSaveSettings
      // 
      this.btnSaveSettings.Location = new System.Drawing.Point(202, 85);
      this.btnSaveSettings.Name = "btnSaveSettings";
      this.btnSaveSettings.Size = new System.Drawing.Size(118, 32);
      this.btnSaveSettings.TabIndex = 2;
      this.btnSaveSettings.Text = "Save and close";
      this.btnSaveSettings.UseVisualStyleBackColor = true;
      this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
      // 
      // SetupForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(332, 129);
      this.Controls.Add(this.btnSaveSettings);
      this.Controls.Add(this.cbRemoteKey);
      this.Controls.Add(this.lblRemoteKey);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "SetupForm";
      this.Text = "AudioSwitcher - Setup";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblRemoteKey;
    private System.Windows.Forms.ComboBox cbRemoteKey;
    private System.Windows.Forms.Button btnSaveSettings;
  }
}