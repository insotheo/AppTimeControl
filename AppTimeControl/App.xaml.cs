using System.Globalization;
using System.Threading;
using System.Windows;

namespace AppTimeControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App() : base()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

    }
}
