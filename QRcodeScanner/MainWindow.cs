using System;
using System.Drawing;
using System.Windows.Forms;
using ZXing;

namespace QRcodeScanner
{
    public partial class MainWindow : Form
    {
        private NotifyIcon _notifyIcon = new NotifyIcon();
        
        public MainWindow()
        {
            InitializeComponent();
            _notifyIcon.Icon = SystemIcons.Application;
            _notifyIcon.BalloonTipTitle = "Bar/QR-Code Scanner";
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            Hide();
            Image test = SelectArea.GetCode();
            if (test != null)
            {
                Bitmap bitmap = new Bitmap(test);
                BarcodeReader reader = new BarcodeReader {AutoRotate = true, Options = {TryHarder = true}};
                Result results = reader.Decode(bitmap);
                if (results != null)
                {
                    string decoded = results.ToString().Trim();
                    Clipboard.SetText(decoded);
                    SendNotification("Content: " + decoded + "\nSaved to your clipboard");
                }
                else
                {
                    SendNotification("Could not read Bar/QR-Code");
                }
            }
            Show();
        }

        private void SendNotification(string tipText, int timeout = 5000)
        {
            _notifyIcon.BalloonTipText = tipText;
            _notifyIcon.Visible = true;
            _notifyIcon.ShowBalloonTip(timeout);
        }
    }
}