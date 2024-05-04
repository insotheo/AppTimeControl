using System;
using System.Collections.Generic;

namespace AppTimeControl.AppDataClasses
{
    public sealed class AppData
    {
        public List<ApplicationInformation> Apps;
        public TimeSpan WorkedToday;
        public DateTime LastTimeOpened;

        public AppData()
        {
            Apps = new List<ApplicationInformation>();
            WorkedToday = TimeSpan.Zero;
            LastTimeOpened = DateTime.Now;
        }
    }
}
