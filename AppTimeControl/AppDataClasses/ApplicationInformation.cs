using System;

namespace AppTimeControl.AppDataClasses
{
    public sealed class ApplicationInformation
    {
        public TimeSpan TimeLimit;
        public TimeSpan TimeDone;
        public TimeSpan TimeToNotifyBeforeClosing;
        public string ProccessName;
        public bool NotifyAboutClosing;

        public ApplicationInformation(string proccessName, TimeSpan limit, TimeSpan timeToNotifyBeforeClosing, bool notifyAboutClosing)
        {
            ProccessName = proccessName;
            TimeLimit = limit;
            TimeDone = TimeSpan.Zero;
            TimeToNotifyBeforeClosing = timeToNotifyBeforeClosing;
            NotifyAboutClosing = notifyAboutClosing;
        }
    }
}
