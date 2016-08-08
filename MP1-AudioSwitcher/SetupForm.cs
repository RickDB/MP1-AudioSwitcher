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
        cbStartupPlaybackDevices.Items.Clear();

        CoreAudioController _ac = new CoreAudioController();
        IEnumerable<CoreAudioDevice> _knownDevices = _ac.GetDevices(DeviceType.Playback, DeviceState.Active);

        foreach (var device in _knownDevices)
        {
          cbAvailableAudioDevices.Items.Add(device.FullName);
          cbStartupPlaybackDevices.Items.Add(device.FullName);
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

      lvBitstreamDevices.Items.Add(cbAvailableAudioDevices.Text).SubItems.AddRange(subItems);

      cbAvailableAudioDevices.Text = "";
      cbBitstreamDeviceEnabled.Checked = false;

      foreach (int i in cblSupportedBitstreamOptions.CheckedIndices)
      {
        cblSupportedBitstreamOptions.SetItemCheckState(i, CheckState.Unchecked);
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
  }
}
