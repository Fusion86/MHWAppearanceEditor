using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;

namespace MHWAppearanceEditor
{
    static class Utility
    {
        public const string MONSTER_HUNTER_WORLD_APPID = "582010";

        public static string GetSteamRoot()
        {
            string reg = Environment.Is64BitOperatingSystem ? @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam" : @"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam";
            object val = Registry.GetValue(reg, "InstallPath", null);

            if (val != null)
                return (string)val;
            else
                return null;
        }

        public static string GetMhwSaveDir()
        {
            string steamRoot = GetSteamRoot();

            if (steamRoot == null)
                return null;

            string userDataRoot = Path.Combine(steamRoot, "userdata");

            // Each steam account has its own userdata folder, and there could be multiple steam accounts on this computer
            foreach (var userDataPath in Directory.GetDirectories(userDataRoot))
            {
                if (Directory.GetDirectories(userDataPath).FirstOrDefault(x => x.Contains(MONSTER_HUNTER_WORLD_APPID)) != null)
                    return Path.Combine(userDataPath, MONSTER_HUNTER_WORLD_APPID, "remote");
            }

            return null;
        }
    }
}
