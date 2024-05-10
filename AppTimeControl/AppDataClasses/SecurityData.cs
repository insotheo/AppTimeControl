namespace AppTimeControl.AppDataClasses
{
    public sealed class SecurityData
    {
        public string SecretPassword;
        //Will we ask password in case...
        public bool CreatingNewListener;
        public bool RemovingListener;
        public bool ChangingAppNameOfListener;
        public bool ChangingProcessNameOfListener;
        public bool ChangingTimLimitOfListener;
        public bool ClosingWindow;
        public bool ShowingWindow;
        public bool ChangingPauseStateOfListener;
        public bool ResetingTimerOfListener;

        public SecurityData(string secretPassword, bool creatingNewListener, bool removingListener, bool changingAppNameOfListener, bool changingProcessNameOfListener, bool changingTimLimitOfListener, bool closingWindow, bool showingWindow, bool changingPauseStateOfListener, bool resetingTimerOfListener)
        {
            SecretPassword = secretPassword;
            CreatingNewListener = creatingNewListener;
            RemovingListener = removingListener;
            ChangingAppNameOfListener = changingAppNameOfListener;
            ChangingProcessNameOfListener = changingProcessNameOfListener;
            ChangingTimLimitOfListener = changingTimLimitOfListener;
            ClosingWindow = closingWindow;
            ShowingWindow = showingWindow;
            ChangingPauseStateOfListener = changingPauseStateOfListener;
            ResetingTimerOfListener = resetingTimerOfListener;
        }
    }
}
