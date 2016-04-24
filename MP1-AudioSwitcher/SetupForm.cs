using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MP1_AudioSwitcher
{
  public partial class SetupForm : Form
  {
    public SetupForm()
    {
      InitializeComponent();
      Settings.LoadSettings();
      cbRemoteKey.SelectedIndex = Settings.RemoteKeyDialogContextMenu;
    }

    private void btnSaveSettings_Click(object sender, EventArgs e)
    {
      Settings.RemoteKeyDialogContextMenu = cbRemoteKey.SelectedIndex;
      Settings.SaveSettings();
      Close();
    }
  }
}
