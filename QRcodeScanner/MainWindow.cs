using System;
using System.Drawing;
using System.Windows.Forms;
using ZXing;

namespace QRcodeScanner
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
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
                }
            }
            Show();
        }
    }
}