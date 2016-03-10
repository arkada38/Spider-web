using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Spider_web.Utils
{
    internal static class Settings
    {
        private static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        private static readonly StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;

        internal static int Score
        {
            get { return (int) GetSetting("Score"); }
            set
            {
                if ((int) GetSetting("Score") == value) return;
                SetSetting("Score", value);
            }
        }

        internal static void InitSettings()
        {
            if (!LocalSettings.Values.ContainsKey("Score"))
                SetSetting("Score", 0);
            var c = GetPassedLvls();
            Debug.WriteLine(c.ToString());
        }

        #region Methods
        internal static async Task<List<int>> GetPassedLvls()
        {
            try
            {
                var targetFile = await LocalFolder.GetFileAsync("PassedLvls.txt");
                var passedLvlsString = await FileIO.ReadTextAsync(targetFile);

                var res = new List<int>();
                var strings = passedLvlsString.Split(' ');

                for (var i = 0; i < strings.Length - 1; i++)
                    res.Add(int.Parse(strings[i]));

                return res;
            }
            catch (FileNotFoundException)
            {
                SetPassedLvls(new List<int> { -1 });
                return new List<int> { -1 };
            }
        }

        internal static async void SetPassedLvls(List<int> lvls)
        {
            var contents = string.Empty;

            if (lvls.Any())
            {
                var s = lvls.Distinct().ToList();
                s.Sort();
                contents = s.Aggregate(contents, (current, lvl) => current + $"{lvl} ");
            }
            
            var sampleFile = await LocalFolder.CreateFileAsync("PassedLvls.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, contents);
        }

        internal static void SetSetting(string key, object value)
        {
            LocalSettings.Values[key] = value;
        }

        internal static object GetSetting(string key)
        {
            if (LocalSettings.Values.ContainsKey(key))
                return LocalSettings.Values[key];
            throw new ArgumentNullException();
        }
        #endregion
    }
}