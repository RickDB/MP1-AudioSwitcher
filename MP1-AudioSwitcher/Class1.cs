using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;

using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;

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

      // Button Handler
      GUIWindowManager.OnNewAction += new OnActionHandler(OnNewAction);
      Settings.LoadSettings();

      return true;
    }

    public void OnNewAction(MediaPortal.GUI.Library.Action action)
    {
      // Remote Key to open Menu
      if ((action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_REMOTE_RED_BUTTON && Settings.RemoteKeyDialogContextMenu == 1) ||
        (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_REMOTE_GREEN_BUTTON && Settings.RemoteKeyDialogContextMenu == 2) ||
         (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_REMOTE_BLUE_BUTTON && Settings.RemoteKeyDialogContextMenu == 3) ||
          (action.wID == MediaPortal.GUI.Library.Action.ActionType.ACTION_REMOTE_YELLOW_BUTTON &&Settings.RemoteKeyDialogContextMenu == 4))
      {
        DialogContextMenu();
      }
    }

    private IEnumerable<CoreAudioDevice> GetPlaybackDevices()
    {
      IEnumerable<CoreAudioDevice> devices = null;

      try
      {
        Log.Debug("Audio Switcher - getting all playback devices");
        devices = _ac.GetDevices(DeviceType.Playback, DeviceState.Active);
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
    }
  }
}
