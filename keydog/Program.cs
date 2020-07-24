using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Configuration;

namespace keydog
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static Dictionary<string, string> ReadAllSettings()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count > 0)
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        if (!dic.ContainsKey(key))
                            dic.Add(key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return dic;
        }

        public static bool isSetting(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[key] != null;
        }

        public static string ReadSetting(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[key] != null ? appSettings[key] : string.Empty;
        }

        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
