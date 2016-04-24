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
        RemoteKeyDialogContextMenu = reader.GetValueAsInt("VideoCleaner", "remoteKeyDialogContextMenu", 0);
      }
    }

    public static void SaveSettings()
    {

      using (
        MediaPortal.Profile.Settings reader =
          new MediaPortal.Profile.Settings(
            MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config, "MediaPortal.xml")))
      {
        reader.SetValue("VideoCleaner", "remoteKeyDialogContextMenu", RemoteKeyDialogContextMenu);
      }
    }

    public static void LoadSpecificSetting(string setting, String value)
    {
      using (
        MediaPortal.Profile.Settings reader =
          new MediaPortal.Profile.Settings(
            MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config, "MediaPortal.xml")))
      {
        value = reader.GetValueAsString("VideoCleaner", setting, "");
        reader.SetValue("VideoCleaner", setting, value);
      }
    }

    public static void SaveSpecificSetting(string setting, String value)
    {
      using (
        MediaPortal.Profile.Settings reader =
          new MediaPortal.Profile.Settings(
            MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config, "MediaPortal.xml")))
      {
        reader.SetValue("VideoCleaner", setting, value);
      }
    }
  }
}
