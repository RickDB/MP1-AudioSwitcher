using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;

using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using Microsoft.Win32;

namespace MP1_AudioSwitcher
{
  [PluginIcons("MP1_AudioSwitcher.Resources.Enabled.png", "MP1_AudioSwitcher.Resources.Disabled.png")]

  public class Class1 : GUIWindow, ISetupForm
  {

    private CoreAudioController _ac;
    private static bool _bitstreamEnabled;

    // With GetID it will be an window-plugin / otherwise a process-plugin
    // Enter the id number here again
    public override int GetID
    {
      get { return 16200; }
      set { }
    }

    public string Author()
    {
      return "Rick164";
    }

    public void ShowPlugin()
    {
      Form settings = new SetupForm();
      settings.Show();
    }

    // Indicates whether plugin can be enabled/disabled
    public bool CanEnable()
    {
      return true;
    }

    // Indicates if plugin is enabled by default;
    public bool DefaultEnabled()
    {
      return true;
    }

    // Returns the description of the plugin is shown in the plugin menu
    public string PluginName()
    {
      return "AudioSwitcher";
    }

    public string Description()
    {
      return "Easily switch audio devices via context menu using a mapped remote key.";
    }

    public bool GetHome(out string strButtonText, out string strButtonImage,
      out string strButtonImageFocus, out string strPictureImage)
    {
      strButtonText = string.Empty;
      strButtonImage = string.Empty;
      strButtonImageFocus = string.Empty;
      strPictureImage = string.Empty;
      return false;
    }

    // Get Windows-ID
    public int GetWindowId()
    {
      // WindowID of windowplugin belonging to this setup
      // enter your own unique code
      return 16200;
    }

    // indicates if a plugin has it's own setup screen
    public bool HasSetup()
    {
      return true;
    }

    public override bool Init()
    {
      try
      {
        _ac = new CoreAudioController();
      }
      catch (Exception ex)
      {
        Log.Error("Error occured during Init()");
        Log.Error(ex.Message);
      }

      // Button Handler
      GUIWindowManager.OnNewAction += new OnActionHandler(OnNewAction);
      Settings.LoadSettings();

      IEnumerable<CoreAudioDevice> devices = GetPlaybackDevices();

      if (!string.IsNullOrEmpty(Settings.DefaultPlaybackDevice))
      {
        Log.Debug("Setting default playback device on startup: " + Settings.DefaultPlaybackDevice);
        if (devices != null)
        {
          foreach (var device in devices)
          {
            if (device.FullName == Settings.DefaultPlaybackDevice && !device.IsDefaultDevice)
            {
              SetPlaybackDevice(device);
            }
            else if (device.IsDefaultDevice && Settings.LAVbitstreamPerDevice)
            {
              SetLavBitstreamSettings(device.FullName);
            }
          }
        }
        else
        {
         Log.Error("No playback devices found!");
        }
      }
      else if (Settings.LAVbitstreamPerDevice)
      {
        // Get current device
        string currentDeviceName = "";
        if (devices != null)
        {
          foreach (var device in devices)
          {
            if (device.IsDefaultDevice)
            {
              currentDeviceName = device.FullName;
            }
          }

          if (!string.IsNullOrEmpty(currentDeviceName))
          {
            SetLavBitstreamSettings(currentDeviceName);
          }
        }
        else
        {
         Log.Error("No playback devices found!");
        }
      }

      return true;
    }

    public void SetLavBitstreamSettings(string currentDeviceName)
    {
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

              if (deviceName == currentDeviceName)
              {
                ToggleLAVBitstreaming(bool.Parse(bitStreamEnabled), bitStreamOptions);
              }
            }
          }
          else
          {
            var splitDevice = Settings.LAVbitstreamPropertyList.Split('^');

            string deviceName = splitDevice[0];
            string bitStreamEnabled = splitDevice[1];
            string bitStreamOptions = splitDevice[2];

            if (deviceName == currentDeviceName)
            {
              ToggleLAVBitstreaming(bool.Parse(bitStreamEnabled), bitStreamOptions);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Error occured during SetLavBitstreamSettings()");
        Log.Error(ex.Message);
      }

    }

    public void OnNewAction(MediaPortal.GUI.Library.Action action)
    {
      // Remote Key to open Menu
      if ((action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_REMOTE_RED_BUTTON && Settings.RemoteKeyDialogContextMenu == 1) ||
         (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_REMOTE_GREEN_BUTTON && Settings.RemoteKeyDialogContextMenu == 2) ||
         (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_REMOTE_BLUE_BUTTON && Settings.RemoteKeyDialogContextMenu == 3) ||
         (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_REMOTE_YELLOW_BUTTON && Settings.RemoteKeyDialogContextMenu == 4) ||
         (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_DVD_MENU && Settings.RemoteKeyDialogContextMenu == 5) ||
         (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_REMOTE_SUBPAGE_DOWN && Settings.RemoteKeyDialogContextMenu == 6) ||
         (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_REMOTE_SUBPAGE_UP && Settings.RemoteKeyDialogContextMenu == 7))
      {
        DialogContextMenu();
      }
    }

    private IEnumerable<CoreAudioDevice> GetPlaybackDevices()
    {
      IEnumerable<CoreAudioDevice> devices = null;
      bool reInitCoreAudioController = false;

      try
      {
        Log.Debug("Audio Switcher - getting all playback devices");

        devices = _ac.GetDevices(DeviceType.Playback, DeviceState.Active);

        // Check for UNKNOWN devices and re-init CoreAudioControler if needed (AudioSwitcher API bug)
        foreach (var device in devices.Where(device => device.FullName.ToLower().Contains("unknown")))
        {
          Log.Debug("Found unknown device during GetPlaybackDevices(), gonna re-init CoreAudioControler and update list");
          Log.Debug("Device ID: " + device.Id);
          reInitCoreAudioController = true;
        }

        if (reInitCoreAudioController)
        {
          _ac = null;
          devices = null;
          _ac = new CoreAudioController();
          devices = _ac.GetDevices(DeviceType.Playback, DeviceState.Active);
        }

      }
      catch (Exception ex)
      {
        Log.Error("Error occured during GetPlaybackDevices()");
        Log.Error(ex.Message);
      }

      return devices;
    }

    private void SetPlaybackDevice(CoreAudioDevice device)
    {
      try
      {
        Log.Debug("Audio Switcher - setting default playback device to: " + device.FullName);
        _ac.SetDefaultDevice(device);
        if (Settings.LAVbitstreamPerDevice)
        {
          SetLavBitstreamSettings(device.FullName);
        }
      }
      catch (Exception ex)
      {
        Log.Error("Error occured during SetPlaybackDevice()");
        Log.Error(ex.Message);
      }
    }

    private void DialogContextMenu()
    {
      // Showing context menu
      var dlg = (GUIDialogMenu) GUIWindowManager.GetWindow((int) Window.WINDOW_DIALOG_MENU);
      dlg.Reset();
      dlg.SetHeading("Audio switcher");

      dlg.Add(new GUIListItem("Change playback device"));
      if (Settings.LAVbitstreamPerDevice || Settings.LAVbitstreamAlwaysShowToggleInContextMenu)
      {
        dlg.Add(_bitstreamEnabled
          ? new GUIListItem("Disable LAV bitstreaming")
          : new GUIListItem("Enable LAV bitstreaming"));
      }


      dlg.SelectedLabel = 0;
      dlg.DoModal(GUIWindowManager.ActiveWindow);


      if (dlg.SelectedLabelText == "Change playback device")
      {
        IEnumerable<CoreAudioDevice> devices = GetPlaybackDevices();

        var dlgSetPlaybackDevice = (GUIDialogMenu) GUIWindowManager.GetWindow((int) Window.WINDOW_DIALOG_MENU);
        dlgSetPlaybackDevice.Reset();
        dlgSetPlaybackDevice.SetHeading("Select playback device");
        dlgSetPlaybackDevice.SelectedLabel = 0;

        int i = 0;
        if (devices != null)
        {
          foreach (var device in devices)
          {
              dlgSetPlaybackDevice.Add(device.FullName);
              if (device.IsDefaultDevice)
              {
                dlgSetPlaybackDevice.SelectedLabel = i;
              }
              i++;
          }
        }
        else
        {
          dlgSetPlaybackDevice.Add("No playback devices found!");
        }

        dlgSetPlaybackDevice.DoModal(GUIWindowManager.ActiveWindow);

        if (dlgSetPlaybackDevice.SelectedLabel >= 0)
        {
          string selectedDevice = dlgSetPlaybackDevice.SelectedLabelText;
          if (selectedDevice != "No playback devices found!" && !string.IsNullOrEmpty(selectedDevice))
          {
            var device = devices.ElementAt(dlgSetPlaybackDevice.SelectedLabel);
            SetPlaybackDevice(device);
          }
        }
      }
      if (dlg.SelectedLabelText == "Enable LAV bitstreaming")
      {
        ToggleLAVBitstreaming(true, "", true);
      }

      if (dlg.SelectedLabelText == "Disable LAV bitstreaming")
      {
        ToggleLAVBitstreaming(false,"", true);
      }
    }

    public static void ToggleLAVBitstreaming(bool enable, string bitstreamOptions, bool forced = false)
    {
      try
      {
        var myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\LAV\\Audio", true);
        _bitstreamEnabled = enable;

        if (myKey != null)
        {
          if (enable)
          {
            if (forced)
            {
              Log.Debug("Enabling LAV bitstreaming (forced)");
              myKey.SetValue("Bitstreaming_ac3", "1", RegistryValueKind.DWord);
              myKey.SetValue("Bitstreaming_dts", "1", RegistryValueKind.DWord);
              myKey.SetValue("Bitstreaming_dtshd", "1", RegistryValueKind.DWord);
              myKey.SetValue("Bitstreaming_eac3", "1", RegistryValueKind.DWord);
              myKey.SetValue("Bitstreaming_truehd", "1", RegistryValueKind.DWord);
            }
            else
            {
              List<string> bitstreamOptionsList = new List<string>();
              if (bitstreamOptions.Contains(","))
              {
                bitstreamOptionsList = bitstreamOptions.Split(',').ToList();
              }
              else
              {
                bitstreamOptionsList.Add(bitstreamOptions);
              }

              Log.Debug("Enabling LAV bitstreaming");
              foreach (var bitstreamCodec in bitstreamOptionsList)
              {
                switch (bitstreamCodec)
                {
                  case "AC3":
                    myKey.SetValue("Bitstreaming_ac3", "1", RegistryValueKind.DWord);
                    break;
                  case "DTS":
                    myKey.SetValue("Bitstreaming_dts", "1", RegistryValueKind.DWord);
                    break;
                  case "DTS HD":
                    myKey.SetValue("Bitstreaming_dtshd", "1", RegistryValueKind.DWord);
                    break;
                  case "EAC3":
                    myKey.SetValue("Bitstreaming_eac3", "1", RegistryValueKind.DWord);
                    break;
                  case "TRUE HD":
                    myKey.SetValue("Bitstreaming_truehd", "1", RegistryValueKind.DWord);
                    break;
                }
              }
            }
          }
          else
          {
            Log.Debug(forced ? "Disabling LAV bitstreaming (forced)" : "Disabling LAV bitstreaming");

            myKey.SetValue("Bitstreaming_ac3", "0", RegistryValueKind.DWord);
            myKey.SetValue("Bitstreaming_dts", "0", RegistryValueKind.DWord);
            myKey.SetValue("Bitstreaming_dtshd", "0", RegistryValueKind.DWord);
            myKey.SetValue("Bitstreaming_eac3", "0", RegistryValueKind.DWord);
            myKey.SetValue("Bitstreaming_truehd", "0", RegistryValueKind.DWord);
          }
        }
        else
        {
          Log.Debug("LAV main registry key not found");
        }

        myKey.Close();
      }
      catch (Exception ex)
      {
        Log.Error("Error occured during ToggleLAVBitstreaming()");
        Log.Error(ex.Message);
      }
    }
  }
}
