using System.Windows;

namespace AppTimeControl.MessageBoxPressets
{
    public static class MessBox
    {
        public static void ShowInfo<T>(T msg)
        {
            MessageBox.Show(msg.ToString(), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowWarning<T>(T msg)
        {
            MessageBox.Show(msg.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void ShowError<T>(T msg)
        {
            MessageBox.Show(msg.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        internal static bool AskPassword()
        {
            AuthenticationWindow auth = new AuthenticationWindow();
            auth.ShowDialog();
            return auth.IsConfirmed;
        }

    }
}
