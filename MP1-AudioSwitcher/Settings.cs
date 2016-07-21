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
