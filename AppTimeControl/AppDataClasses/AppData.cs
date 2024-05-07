using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace AppTimeControl.AppDataClasses
{
    public sealed class AppData
    {
        public List<ApplicationInformation> Apps;
        public DateTime LastTimeOpened;

        public AppData()
        {
            Apps = new List<ApplicationInformation>();
            LastTimeOpened = DateTime.Now;
        }

        public static void SaveToFile(ref AppData appData)
        {
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "app_data.json"), JsonConvert.SerializeObject(appData, Formatting.Indented));
        }

    }
}
