using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using MediaPortal.GUI.Library;
using Action = System.Action;

namespace MP1_AudioSwitcher
{
  public partial class SetupForm : Form
  {
    public SetupForm()
    {
      InitializeComponent();
      Settings.LoadSettings();
      LoadSettings();
    }

    public void FillLists()
    {
      LoadKnownDevices();
    }

    public void LoadKnownDevices()
    {
      try
      {
        cbAvailableAudioDevices.Items.Clear();
        cbAvailableAudioDevices.AutoCompleteCustomSource.Clear();
        cbStartupPlaybackDevices.Items.Clear();
        cbStartupPlaybackDevices.AutoCompleteCustomSource.Clear();


        CoreAudioController _ac = new CoreAudioController();
        IEnumerable<CoreAudioDevice> _knownDevices = _ac.GetDevices(DeviceType.Playback, DeviceState.Active);

        foreach (var device in _knownDevices)
        {
          cbAvailableAudioDevices.Items.Add(device.FullName);
          cbAvailableAudioDevices.AutoCompleteCustomSource.Add(device.FullName);
          cbStartupPlaybackDevices.Items.Add(device.FullName);
          cbStartupPlaybackDevices.AutoCompleteCustomSource.Add(device.FullName);

        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error occured during GetPlaybackDevices()");
        MessageBox.Show(ex.Message);
      }
    }

    private void btnSaveSettings_Click(object sender, EventArgs e)
    {
      SaveSettings();
    }

    private void LoadSettings()
    {
      cbRemoteKey.SelectedIndex = Settings.RemoteKeyDialogContextMenu;
      cbStartupPlaybackDevices.Text = Settings.DefaultPlaybackDevice;
      chkAlwaysShowLavBitstreamToggle.Checked = Settings.LAVbitstreamAlwaysShowToggleInContextMenu;

      chkEnableLavAudioDelayControl.Checked = Settings.LAVaudioDelayControlsInContextMenu;
      chkEnableLAVbitstreamPerDevice.Checked = Settings.LAVbitstreamPerDevice;

      LoadKnownDevices();

      try
      {
        if (!string.IsNullOrEmpty(Settings.LAVbitstreamPropertyList))
        {
          if (Settings.LAVbitstreamPropertyList.Contains("|"))
          {
            var splitDevices = Settings.LAVbitstreamPropertyList.Split('|');
            foreach (var device in splitDevices)
            {
              var splitDevice = device.Split('^');
              string deviceName = splitDevice[0];
              string bitStreamEnabled = splitDevice[1];
              string bitStreamOptions = splitDevice[2];

              string[] subItems =
              {
                bitStreamEnabled,
                bitStreamOptions
              };

              lvBitstreamDevices.Items.Add(deviceName).SubItems.AddRange(subItems);
            }
          }
          else
          {
            var splitDevice = Settings.LAVbitstreamPropertyList.Split('^');

            string deviceName = splitDevice[0];
            string bitStreamEnabled = splitDevice[1];
            string bitStreamOptions = splitDevice[2];

            string[] subItems =
            {
            bitStreamEnabled,
            bitStreamOptions
          };

            lvBitstreamDevices.Items.Add(deviceName).SubItems.AddRange(subItems);
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error occured during LoadSettings()");
        MessageBox.Show(ex.Message);
      }
    }

    private void SaveSettings()
    {
      try
      {
        Settings.RemoteKeyDialogContextMenu = cbRemoteKey.SelectedIndex;
        Settings.DefaultPlaybackDevice = cbStartupPlaybackDevices.Text;
        Settings.LAVaudioDelayControlsInContextMenu = chkEnableLavAudioDelayControl.Checked;
        Settings.LAVbitstreamAlwaysShowToggleInContextMenu = chkAlwaysShowLavBitstreamToggle.Checked;
        Settings.LAVbitstreamPerDevice = chkEnableLAVbitstreamPerDevice.Checked;

        string formatedBitStreamPropertiesList = "";
        if (lvBitstreamDevices.Items.Count > 0)
        {
          foreach (ListViewItem item in lvBitstreamDevices.Items)
          {
            if (string.IsNullOrEmpty(formatedBitStreamPropertiesList))
            {
              formatedBitStreamPropertiesList = string.Format("{0}^{1}^{2}", item.Text, item.SubItems[1].Text,
                item.SubItems[2].Text);
            }
            else
            {
              formatedBitStreamPropertiesList = string.Format("{0}|{1}^{2}^{3}", formatedBitStreamPropertiesList,
                item.Text, item.SubItems[1].Text, item.SubItems[2].Text);
            }
          }
        }

        Settings.LAVbitstreamPropertyList = formatedBitStreamPropertiesList;
        Settings.SaveSettings();
        Close();
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error occured during SaveSettings()");
        MessageBox.Show(ex.Message);
      }
    }

    private void cbBitstreamDeviceEnabled_CheckedChanged(object sender, EventArgs e)
    {
      if (cbBitstreamDeviceEnabled.Checked)
      {
        lblSupportedBitstreamOptions.Visible = true;
        cblSupportedBitstreamOptions.Visible = true;
        btnCheckAllBitstreamOptions.Visible = true;
        btnCheckNoBitstreamOptions.Visible = true;
      }
      else
      {
        lblSupportedBitstreamOptions.Visible = false;
        cblSupportedBitstreamOptions.Visible = false;
        btnCheckAllBitstreamOptions.Visible = false;
        btnCheckNoBitstreamOptions.Visible = false;
      }
    }

    private void btnAddDevice_Click(object sender, EventArgs e)
    {
      var deviceName = cbAvailableAudioDevices.Text.Trim();

      if (!string.IsNullOrEmpty(deviceName))
      {
        string checkBitstreamOptions = "";
        foreach (var item in cblSupportedBitstreamOptions.CheckedItems)
        {
          if (string.IsNullOrEmpty(checkBitstreamOptions))
          {
            checkBitstreamOptions = item.ToString();
          }
          else
          {
            checkBitstreamOptions += "," + item;
          }
        }

        string[] subItems =
        {
          cbBitstreamDeviceEnabled.Checked.ToString(),
          checkBitstreamOptions
        };

        var isDuplicate = false;
        foreach (ListViewItem item in lvBitstreamDevices.Items)
        {
          if (item.Text.ToLower() == deviceName.ToLower())
          {
            isDuplicate = true;
          }
        }

        if (!isDuplicate)
        {
          lvBitstreamDevices.Items.Add(deviceName).SubItems.AddRange(subItems);

          cbAvailableAudioDevices.Text = "";
          cbBitstreamDeviceEnabled.Checked = false;

          foreach (int i in cblSupportedBitstreamOptions.CheckedIndices)
          {
            cblSupportedBitstreamOptions.SetItemCheckState(i, CheckState.Unchecked);
          }
        }
        else
        {
          MessageBox.Show(string.Format("Device [ {0} ] already exists in list.", deviceName));
        }
      }
    }

    private void btnRemoveDevice_Click(object sender, EventArgs e)
    {
      try
      {
        foreach (ListViewItem eachItem in lvBitstreamDevices.SelectedItems)
        {
          lvBitstreamDevices.Items.Remove(eachItem);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error occured while removing device: " + ex.Message);
      }
    }

    private enum MoveDirection
    {
      Up = -1,
      Down = 1
    };

    private void MoveListViewItems(ListView sender, MoveDirection direction)
    {
      var valid = sender.SelectedItems.Count > 0 &&
                  ((direction == MoveDirection.Down &&
                    (sender.SelectedItems[sender.SelectedItems.Count - 1].Index < sender.Items.Count - 1))
                   || (direction == MoveDirection.Up && (sender.SelectedItems[0].Index > 0)));

      if (valid)
      {
        var start = true;
        var first_idx = 0;
        var items = new List<ListViewItem>();

        // ambil data
        foreach (ListViewItem i in sender.SelectedItems)
        {
          if (start)
          {
            first_idx = i.Index;
            start = false;
          }
          items.Add(i);
        }

        sender.BeginUpdate();

        // hapus
        foreach (ListViewItem i in sender.SelectedItems) i.Remove();

        // insert
        if (direction == MoveDirection.Up)
        {
          var insert_to = first_idx - 1;
          foreach (var i in items)
          {
            sender.Items.Insert(insert_to, i);
            insert_to++;
          }
        }
        else
        {
          var insert_to = first_idx + 1;
          foreach (var i in items)
          {
            sender.Items.Insert(insert_to, i);
            insert_to++;
          }
        }
        sender.EndUpdate();
      }
    }

    private void btnRefreshDevices_Click(object sender, EventArgs e)
    {
      LoadKnownDevices();
    }

    private void btbRefreshDefaultPlaybackDevices_Click(object sender, EventArgs e)
    {
      LoadKnownDevices();
    }

    private void btnClearDevice_Click(object sender, EventArgs e)
    {
      cbStartupPlaybackDevices.Text = "";
    }

    private void btnCheckAllBitstreamOptions_Click(object sender, EventArgs e)
    {
      for (int i = 0; i < cblSupportedBitstreamOptions.Items.Count; i++)
      {
        cblSupportedBitstreamOptions.SetItemChecked(i, true);
      }
    }

    private void btnCheckNoBitstreamOptions_Click(object sender, EventArgs e)
    {
      for (int i = 0; i < cblSupportedBitstreamOptions.Items.Count; i++)
      {
        cblSupportedBitstreamOptions.SetItemChecked(i, false);
      }
    }

    private void UnFocusComboBox(ComboBox cb)
    {
      BeginInvoke(new Action(() => { cb.Select(0, 0); }));
    }

    private void cbStartupPlaybackDevices_DropDownClosed(object sender, EventArgs e)
    {
      UnFocusComboBox(cbStartupPlaybackDevices);
    }

    private void cbStartupPlaybackDevices_SelectedIndexChanged(object sender, EventArgs e)
    {
      UnFocusComboBox(cbStartupPlaybackDevices);
    }

    private void cbAvailableAudioDevices_SelectedIndexChanged(object sender, EventArgs e)
    {
      UnFocusComboBox(cbAvailableAudioDevices);
    }
    private void cbAvailableAudioDevices_DropDownClosed(object sender, EventArgs e)
    {
      UnFocusComboBox(cbAvailableAudioDevices);
    }

    private void btnDeviceUp_Click(object sender, EventArgs e)
    {
      MoveListViewItems(lvBitstreamDevices, MoveDirection.Up);
    }

    private void btnDeviceDown_Click(object sender, EventArgs e)
    {
      MoveListViewItems(lvBitstreamDevices, MoveDirection.Down);
    }
  }
}
