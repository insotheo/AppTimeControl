using System;

namespace AppTimeControl.AppDataClasses
{
    public sealed class ApplicationInformation
    {
        public TimeSpan TimeLimit;
        public TimeSpan TimeDone;
        public TimeSpan WorkedInTotal;
        public string ProccessName;
        public string AppName;

        public ApplicationInformation(string proccessName, string appName, TimeSpan limit)
        {
            ProccessName = proccessName;
            TimeLimit = limit;
            AppName = appName;
            TimeDone = TimeSpan.Zero;
            WorkedInTotal = TimeSpan.Zero;
        }
    }
}
