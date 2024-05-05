using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AppTimeControl
{
    internal static class Notificator
    {
        public static void SendNotification<T>(T message)
        {
            if (message == null)
            {
                return;
            }
            try
            {
                new ToastContentBuilder().AddText(message.ToString()).Show();
            }
            catch (PlatformNotSupportedException)
            {
                NotifyIcon notifyIcon = new NotifyIcon();
                notifyIcon.Icon = SystemIcons.Information;
                notifyIcon.Visible = true;
                notifyIcon.Text = message.ToString();
                notifyIcon.BalloonTipTitle = "AppTimeControl";
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon.ShowBalloonTip(3000);
            }
        }
    }
}
