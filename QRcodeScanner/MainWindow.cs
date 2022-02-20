using System;
using System.Drawing;
using System.Windows.Forms;

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
                Clipboard.SetImage(test);
            }
            Show();
        }
    }
}