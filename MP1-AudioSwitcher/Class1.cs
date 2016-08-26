using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MediaPortal.Player;
using Microsoft.Win32;
using Action = MediaPortal.GUI.Library.Action;

namespace MP1_AudioSwitcher
{
  [PluginIcons("MP1_AudioSwitcher.Resources.Enabled.png", "MP1_AudioSwitcher.Resources.Disabled.png")]
  public class Class1 : GUIWindow, ISetupForm
  {
    private CoreAudioController _ac;


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

      // Load settings
      Settings.LoadSettings();

      // Button Handler
      GUIWindowManager.OnNewAction += OnNewAction;
      Settings.LoadSettings();


      if (!string.IsNullOrEmpty(Settings.DefaultPlaybackDevice))
      {
        var devices = GetPlaybackDevices();

        Log.Debug("Setting default playback device on startup: " + Settings.DefaultPlaybackDevice);

        // Find default audio device match to setting
        CoreAudioDevice playbackDeviceLookup = devices.FirstOrDefault(d =>
                        d != null &&
                        d.FullName.Equals(Settings.DefaultPlaybackDevice, StringComparison.CurrentCultureIgnoreCase));

        if (playbackDeviceLookup != null)
        {
          SetPlaybackDevice(playbackDeviceLookup);

          if (Settings.LAVbitstreamPerDevice)
          {
            SetLavBitstreamSettings(playbackDeviceLookup.FullName);
          }
        }
        else
        {
          Log.Error("Default playback devices not found!");
        }
      }
      else if (Settings.LAVbitstreamPerDevice)
      {
        var devices = GetPlaybackDevices();

        // Find default audio device
        CoreAudioDevice defaultPlaybackDevice = devices.FirstOrDefault(d =>
                d != null &&
                d.IsDefaultDevice);

        if (defaultPlaybackDevice != null)
        {
          SetLavBitstreamSettings(defaultPlaybackDevice.FullName);
        }
        else
        {
          Log.Error("No playback devices found!");
        }
      }

      return true;
    }

    public void OnNewAction(Action action)
    {
      // Remote Key to open Menu
      if ((action.wID == Action.ActionType.ACTION_REMOTE_RED_BUTTON && Settings.RemoteKeyDialogContextMenu == 1) ||
          (action.wID == Action.ActionType.ACTION_REMOTE_GREEN_BUTTON && Settings.RemoteKeyDialogContextMenu == 2) ||
          (action.wID == Action.ActionType.ACTION_REMOTE_BLUE_BUTTON && Settings.RemoteKeyDialogContextMenu == 3) ||
          (action.wID == Action.ActionType.ACTION_REMOTE_YELLOW_BUTTON && Settings.RemoteKeyDialogContextMenu == 4) ||
          (action.wID == Action.ActionType.ACTION_DVD_MENU && Settings.RemoteKeyDialogContextMenu == 5) ||
          (action.wID == Action.ActionType.ACTION_REMOTE_SUBPAGE_DOWN && Settings.RemoteKeyDialogContextMenu == 6) ||
          (action.wID == Action.ActionType.ACTION_REMOTE_SUBPAGE_UP && Settings.RemoteKeyDialogContextMenu == 7))
      {
        DialogContextMenu();
      }
    }

    private IEnumerable<CoreAudioDevice> GetPlaybackDevices()
    {
      IEnumerable<CoreAudioDevice> devices = null;
      var reInitCoreAudioController = false;

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

    public void ReadAllLavDelaySettings()
    {
      try
      {
        Settings.LAVaudioDelayEnabled = Convert.ToBoolean(Convert.ToInt16(IsLavAudioDelayEnabled()));
        Settings.LAVaudioDelay = GetLavAudioDelay().ToString();
      }
      catch (Exception ex)
      {
        Log.Error("Error occured during ReadAllLavDelaySettings()");
        Log.Error(ex.Message);
      }
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
      try
      {
        // Showing context menu
        var dlg = (GUIDialogMenu) GUIWindowManager.GetWindow((int) Window.WINDOW_DIALOG_MENU);
        dlg.Reset();
        dlg.SetHeading("Audio switcher");

        dlg.Add(new GUIListItem("Change playback device"));
        if (Settings.LAVbitstreamPerDevice || Settings.LAVbitstreamAlwaysShowToggleInContextMenu)
        {
          bool bitstreamingIsEnabled = Convert.ToBoolean(Convert.ToInt16(IsLavBitstreamingEnabled()));

          dlg.Add(bitstreamingIsEnabled
            ? new GUIListItem("Disable LAV bitstreaming")
            : new GUIListItem("Enable LAV bitstreaming"));
        }

        if (Settings.LAVaudioDelayControlsInContextMenu)
        {
          ReadAllLavDelaySettings();
          if (!Settings.LAVaudioDelayEnabled)
          {
            dlg.Add(new GUIListItem("Enable LAV audio delay"));
          }
          else
          {
            dlg.Add(new GUIListItem(string.Format("Change LAV audio delay ( {0}ms )", Settings.LAVaudioDelay)));
            dlg.Add(new GUIListItem("Disable LAV audio delay"));
          }
        }

        dlg.SelectedLabel = 0;
        dlg.DoModal(GUIWindowManager.ActiveWindow);


        if (dlg.SelectedLabelText == "Change playback device")
        {
          var devices = GetPlaybackDevices();

          var dlgSetPlaybackDevice = (GUIDialogMenu) GUIWindowManager.GetWindow((int) Window.WINDOW_DIALOG_MENU);
          dlgSetPlaybackDevice.Reset();
          dlgSetPlaybackDevice.SetHeading("Select playback device");
          dlgSetPlaybackDevice.SelectedLabel = 0;

          var i = 0;
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
            var selectedDevice = dlgSetPlaybackDevice.SelectedLabelText;
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
          ToggleLAVBitstreaming(false, "", true);
        }

        if (dlg.SelectedLabelText.StartsWith("Disable LAV audio delay"))
        {
          SetLavAudioDelay(false, "");
          RestartVideoPlayback(false, "");
        }

        if (dlg.SelectedLabelText == "Enable LAV audio delay" ||
            dlg.SelectedLabelText.StartsWith("Change LAV audio delay"))
        {
          SelectLavAudioDelay();
        }

        // Save settings
        Settings.SaveSettings();
      }
      catch (Exception ex)
      {
        Log.Error("Error occured during DialogContextMenu()");
        Log.Error(ex.Message);
      }
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
              var deviceName = splitDevice[0];
              var bitStreamEnabled = splitDevice[1];
              var bitStreamOptions = splitDevice[2];

              if (deviceName == currentDeviceName)
              {
                ToggleLAVBitstreaming(bool.Parse(bitStreamEnabled), bitStreamOptions);
              }
            }
          }
          else
          {
            var splitDevice = Settings.LAVbitstreamPropertyList.Split('^');

            var deviceName = splitDevice[0];
            var bitStreamEnabled = splitDevice[1];
            var bitStreamOptions = splitDevice[2];

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

    public static int IsLavBitstreamingEnabled()
    {
      var bitstreamEnabled = 0;
      try
      {
        var myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\LAV\\Audio", true);
        if (myKey != null)
        {
          List<int> bitreamOptions = new List<int>();
          bitreamOptions.Add(int.Parse(myKey.GetValue("Bitstreaming_ac3", RegistryValueKind.DWord).ToString()));
          bitreamOptions.Add(int.Parse(myKey.GetValue("Bitstreaming_dts", RegistryValueKind.DWord).ToString()));
          bitreamOptions.Add(int.Parse(myKey.GetValue("Bitstreaming_dtshd", RegistryValueKind.DWord).ToString()));
          bitreamOptions.Add(int.Parse(myKey.GetValue("Bitstreaming_eac3", RegistryValueKind.DWord).ToString()));
          bitreamOptions.Add(int.Parse(myKey.GetValue("Bitstreaming_truehd", RegistryValueKind.DWord).ToString()));

          var bistreamEnable = bitreamOptions.Where(b => b == 1);
          if (bistreamEnable.Any())
          {
            bitstreamEnabled = 1;
          }
        }
        else
        {
          Log.Debug("LAV main registry key not found");
        }

        myKey?.Close();
      }
      catch (Exception ex)
      {
        Log.Error("Error occured during IsLavBitstreamingEnabled()");
        Log.Error(ex.Message);
      }

      return bitstreamEnabled;
    }

    public static void ToggleLAVBitstreaming(bool enable, string bitstreamOptions, bool forced = false)
    {
      try
      {
        var myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\LAV\\Audio", true);

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
              var bitstreamOptionsList = new List<string>();
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

        myKey?.Close();
      }
      catch (Exception ex)
      {
        Log.Error("Error occured during ToggleLAVBitstreaming()");
        Log.Error(ex.Message);
      }
    }

    public void SelectLavAudioDelay()
    {
      try
      {
        var keyBoard =
          (VirtualKeyboard) GUIWindowManager.GetWindow((int) Window.WINDOW_VIRTUAL_KEYBOARD);

        if (keyBoard != null)
        {
          keyBoard.Reset();
          keyBoard.SetLabelAsInitialText(false);
          if (Settings.LAVaudioDelay != "0")
          {
            keyBoard.Text = Settings.LAVaudioDelay.Trim();
          }

          keyBoard.DoModal(GUIWindowManager.ActiveWindow);
          if (keyBoard.IsConfirmed)
          {
            if (!string.IsNullOrEmpty(keyBoard.Text))
            {
              int delay;
              var isValidInt = int.TryParse(keyBoard.Text, out delay);
              if (isValidInt)
              {
                SetLavAudioDelay(true, delay.ToString());
                RestartVideoPlayback(true, delay.ToString());
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Error occured during SelectLAVAudioDelay()");
        Log.Error(ex.Message);
      }
    }

    public static int IsLavAudioDelayEnabled()
    {
      var audioDelayEnabled = 0;
      try
      {
        var myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\LAV\\Audio", true);
        if (myKey != null)
        {
          audioDelayEnabled = int.Parse(myKey.GetValue("AudioDelayEnabled", RegistryValueKind.DWord).ToString());
        }
        else
        {
          Log.Debug("LAV main registry key not found");
        }

        myKey?.Close();
      }
      catch (Exception ex)
      {
        Log.Error("Error occured during GetLavAudioDelay()");
        Log.Error(ex.Message);
      }

      return audioDelayEnabled;
    }

    public static int GetLavAudioDelay()
    {
      var audioDelay = 0;
      try
      {
        var myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\LAV\\Audio", true);
        if (myKey != null)
        {
          var delay = myKey.GetValue("AudioDelay", RegistryValueKind.DWord).ToString();
          if (!string.IsNullOrEmpty(delay))
          {
            audioDelay = int.Parse(delay);
          }
        }
        else
        {
          Log.Debug("LAV main registry key not found");
        }

        myKey?.Close();
      }
      catch (Exception ex)
      {
        Log.Error("Error occured during GetLavAudioDelay()");
        Log.Error(ex.Message);
      }

      return audioDelay;
    }

    public void SetLavAudioDelay(bool enable, string delay)
    {
      try
      {
        var myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\LAV\\Audio", true);

        if (myKey != null)
        {
          if (enable)
          {
            Log.Debug("Enabling LAV audio delay with delay of {0}ms", delay);
            myKey.SetValue("AudioDelayEnabled", "1", RegistryValueKind.DWord);
            myKey.SetValue("AudioDelay", delay, RegistryValueKind.DWord);
          }
          else
          {
            myKey.SetValue("AudioDelayEnabled", "0", RegistryValueKind.DWord);
          }
        }
        else
        {
          Log.Debug("LAV main registry key not found");
        }

        myKey?.Close();
      }
      catch (Exception ex)
      {
        Log.Error("Error occured during SetLAVAudioDelay()");
        Log.Error(ex.Message);
      }
    }

    public void RestartVideoPlayback(bool enableDelay, string delay)
    {
      if (g_Player.Playing)
      {
        var currentPlayingFile = g_Player.currentFilePlaying;
        var resumePosition = Convert.ToInt32(g_Player.CurrentPosition);

        var dlgYesNo = (GUIDialogYesNo) GUIWindowManager.GetWindow((int) Window.WINDOW_DIALOG_YES_NO);
        if (null != dlgYesNo)
        {
          dlgYesNo.SetHeading(enableDelay ? "Restart playback with new delay?" : "Restart playback with delay disabled?");
          dlgYesNo.SetLine(1, g_Player.currentFileName);
          dlgYesNo.SetLine(2, "Will resume at: " +TimeSpan.FromSeconds(resumePosition).ToString(@"hh\:mm\:ss"));

          if (enableDelay)
          {
            dlgYesNo.SetLine(3, "New delay: (ms): " + delay);
          }

          dlgYesNo.SetDefaultToYes(true);

          dlgYesNo.DoModal(GUIWindowManager.ActiveWindow);

          if (dlgYesNo.IsConfirmed)
          {
            PlayVideo(currentPlayingFile, resumePosition, "", "", true);
          }
        }
      }
    }

    public void PlayVideo(string curFileName, int timeMovieStopped, string audioLanguage, string subLanguage,
      bool stopPlayback)
    {
      try
      {
        GUIGraphicsContext.IsFullScreenVideo = true;
        GUIWindowManager.ActivateWindow((int) Window.WINDOW_FULLSCREEN_VIDEO);

        g_Player.Play(curFileName, g_Player.MediaType.Video);
        if (g_Player.Playing)
        {
          if (timeMovieStopped > 0)
            g_Player.SeekAbsolute(timeMovieStopped);
        }
      }
      catch (Exception ex)
      {
        Log.Error("Error occured during PlayVideo()");
        Log.Error(ex.Message);
      }
    }
  }
}