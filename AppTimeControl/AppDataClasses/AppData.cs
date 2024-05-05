using System;
using System.Collections.Generic;

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
    }
}
