using System.Windows;
using System.IO;

namespace AppTimeControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            if(!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "app_settings.settings")) ||
                !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "app_data.json")))
            {
                WelcomeWindow welcomeWindow = new WelcomeWindow();
                welcomeWindow.Show();
                this.Close();
            }
            InitializeComponent();
        }
    }
}
