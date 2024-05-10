using AppTimeControl.AppDataClasses;
using System.Windows;

namespace AppTimeControl
{
    /// <summary>
    /// Interaction logic for SecuritySettings.xaml
    /// </summary>
    public partial class SecuritySettings : Window
    {
        internal SecurityData SecurityData;

        public SecuritySettings(ref SecurityData securityData)
        {
            InitializeComponent();
            SecurityData = securityData;
            CreatingNewListenerCB.IsChecked = SecurityData.CreatingNewListener;
            RemovingListenerCB.IsChecked = SecurityData.RemovingListener;
            EditingAppNameOfListenerCB.IsChecked = SecurityData.ChangingAppNameOfListener;
            EditingProcessNameOfListenerCB.IsChecked = SecurityData.ChangingProcessNameOfListener;
            EditingTimeLimitOfListenerCB.IsChecked = SecurityData.ChangingTimLimitOfListener;
            ClosingWindowCB.IsChecked = SecurityData.ClosingWindow;
            ShowingWindowCB.IsChecked = SecurityData.ShowingWindow;
            EditingPauseStateOfListenerCB.IsChecked = SecurityData.ChangingPauseStateOfListener;
            ResetingTimerOfListenerCB.IsChecked = SecurityData.ResetingTimerOfListener;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NewPasswordTB.Password))
            {
                SecurityData.SecretPassword = NewPasswordTB.Password;
            }
            SecurityData.CreatingNewListener = (bool)CreatingNewListenerCB.IsChecked;
            SecurityData.RemovingListener = (bool)RemovingListenerCB.IsChecked;
            SecurityData.ChangingAppNameOfListener = (bool)EditingAppNameOfListenerCB.IsChecked;
            SecurityData.ChangingProcessNameOfListener = (bool)EditingProcessNameOfListenerCB.IsChecked;
            SecurityData.ChangingTimLimitOfListener = (bool)EditingTimeLimitOfListenerCB.IsChecked;
            SecurityData.ClosingWindow = (bool)ClosingWindowCB.IsChecked;
            SecurityData.ShowingWindow = (bool)ShowingWindowCB.IsChecked;
            SecurityData.ChangingPauseStateOfListener = (bool)EditingPauseStateOfListenerCB.IsChecked;
            SecurityData.ResetingTimerOfListener = (bool)ResetingTimerOfListenerCB.IsChecked;
            this.Close();
        }
    }
}
