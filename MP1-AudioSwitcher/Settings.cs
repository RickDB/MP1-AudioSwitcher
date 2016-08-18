using System;
using MediaPortal.Profile;
using System.Globalization;

namespace MP1_AudioSwitcher
{
  public class Settings
  {
    #region Config variables

    // Remote settings
    public static int RemoteKeyDialogContextMenu;

    // Default playback device
    public static string DefaultPlaybackDevice;

    // Bitstream options
    public static bool LAVbitstreamAlwaysShowToggleInContextMenu;
    public static bool LAVbitstreamPerDevice;
    public static string LAVbitstreamPropertyList;

    // Audio delay
    public static bool LAVaudioDelayControlsInContextMenu;
    public static bool LAVaudioDelayEnabled;
    public static string LAVaudioDelay;

    #endregion

    public static void LoadSettings()
    {
      using (
        MediaPortal.Profile.Settings reader =
          new MediaPortal.Profile.Settings(
            MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config, "MediaPortal.xml")))
      {

        // Load previously incorrectly set value if it exists and clear afterwards
        int RemoteKeyDialogContextMenuOld = reader.GetValueAsInt("VideoCleaner", "remoteKeyDialogContextMenu", -1);

        if (RemoteKeyDialogContextMenuOld != -1)
        {
          RemoteKeyDialogContextMenu = RemoteKeyDialogContextMenuOld;
          reader.RemoveEntry("VideoCleaner", "remoteKeyDialogContextMenu");
        }
        else
        {
          RemoteKeyDialogContextMenu = reader.GetValueAsInt("AudioSwitcher", "remoteKeyDialogContextMenu", 0);
        }

        RemoteKeyDialogContextMenu = reader.GetValueAsInt("AudioSwitcher", "remoteKeyDialogContextMenu", 0);

        DefaultPlaybackDevice = reader.GetValueAsString("AudioSwitcher", "defaultPlaybackDevice", "");

        LAVbitstreamAlwaysShowToggleInContextMenu = reader.GetValueAsBool("AudioSwitcher", "LAVbitstreamAlwaysShowToggleInContextMenu", false);
        LAVbitstreamPerDevice = reader.GetValueAsBool("AudioSwitcher", "LAVbitstreamPerDevice", false);
        LAVbitstreamPropertyList = reader.GetValueAsString("AudioSwitcher", "LAVbitstreamPropertyList", "");

        LAVaudioDelayControlsInContextMenu = reader.GetValueAsBool("AudioSwitcher", "LAVaudioDelayControlsInContextMenu", false);
        LAVaudioDelayEnabled = reader.GetValueAsBool("AudioSwitcher", "LAVaudioDelayEnabled", false);
        LAVaudioDelay = reader.GetValueAsString("AudioSwitcher", "LAVaudioDelay", "0");
      }
    }

    public static void SaveSettings()
    {

      using (
        MediaPortal.Profile.Settings reader =
          new MediaPortal.Profile.Settings(
            MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config, "MediaPortal.xml")))
      {
        reader.SetValue("AudioSwitcher", "remoteKeyDialogContextMenu", RemoteKeyDialogContextMenu);
        reader.SetValue("AudioSwitcher", "defaultPlaybackDevice", DefaultPlaybackDevice);

        reader.SetValueAsBool("AudioSwitcher", "LAVbitstreamAlwaysShowToggleInContextMenu", LAVbitstreamAlwaysShowToggleInContextMenu);
        reader.SetValueAsBool("AudioSwitcher", "LAVbitstreamPerDevice", LAVbitstreamPerDevice);
        reader.SetValue("AudioSwitcher", "LAVbitstreamPropertyList", LAVbitstreamPropertyList);

        reader.SetValueAsBool("AudioSwitcher", "LAVaudioDelayControlsInContextMenu", LAVaudioDelayControlsInContextMenu);
        reader.SetValueAsBool("AudioSwitcher", "LAVaudioDelayEnabled", LAVaudioDelayEnabled);
        reader.SetValue("AudioSwitcher", "LAVaudioDelay", LAVaudioDelay);
      }
    }

    public static void LoadSpecificSetting(string setting, String value)
    {
      using (
        MediaPortal.Profile.Settings reader =
          new MediaPortal.Profile.Settings(
            MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config, "MediaPortal.xml")))
      {
        value = reader.GetValueAsString("AudioSwitcher", setting, "");
        reader.SetValue("AudioSwitcher", setting, value);
      }
    }

    public static void SaveSpecificSetting(string setting, String value)
    {
      using (
        MediaPortal.Profile.Settings reader =
          new MediaPortal.Profile.Settings(
            MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config, "MediaPortal.xml")))
      {
        reader.SetValue("AudioSwitcher", setting, value);
      }
    }
  }
}
