using System;
using System.Drawing;
using System.Windows.Forms;

namespace QRcodeScanner
{
    public partial class SelectArea : Form
    {
        private Image Image;
        
        private Point _startPnt;
        private Rectangle _selectRc;
        
        public static Image GetCode() {
            var rc = Screen.PrimaryScreen.Bounds;
            using (Bitmap bmp = new Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb)) {
                using (Graphics gr = Graphics.FromImage(bmp))
                    gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                using (var selectedArea = new SelectArea(bmp)) {
                    if (selectedArea.ShowDialog() == DialogResult.OK) {
                        return selectedArea.Image;
                    }
                }
                return null;
            }
        }

        private SelectArea(Image screenShot)
        {
            InitializeComponent();
            //Default settings for the form
            BackgroundImage = screenShot;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;
            Cursor = Cursors.Cross;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            //Saves the start point
            if(e.Button != MouseButtons.Left) return;
            _startPnt = e.Location;
            _selectRc = new Rectangle(e.Location, new Size(0, 0));
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if(e.Button != MouseButtons.Left) return;
            //Gets all four corners of the rectangle
            int x1 = Math.Min(e.X, _startPnt.X);
            int y1 = Math.Min(e.Y, _startPnt.Y);
            int x2 = Math.Max(e.X, _startPnt.X);
            int y2 = Math.Max(e.Y, _startPnt.Y);
            _selectRc = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            //Draw the new image
            if(_selectRc.Width <= 0 || _selectRc.Height <= 0) return;
            Image = new Bitmap(_selectRc.Width, _selectRc.Height);
            using (Graphics gr = Graphics.FromImage(Image))
            {
                gr.DrawImage(BackgroundImage, new Rectangle(0, 0, Image.Width, Image.Height), _selectRc, GraphicsUnit.Pixel);
            }
            DialogResult = DialogResult.OK;
        }
        
        protected override void OnPaint(PaintEventArgs e) {
            //Draw the current selection
            using (Brush br = new SolidBrush(Color.FromArgb(175, Color.Black))) {
                int x1 = _selectRc.X; int x2 = _selectRc.X + _selectRc.Width;
                int y1 = _selectRc.Y; int y2 = _selectRc.Y + _selectRc.Height;
                //Fill the area around the selected area
                e.Graphics.FillRectangle(br, new Rectangle(0, 0, x1, this.Height));
                e.Graphics.FillRectangle(br, new Rectangle(x2, 0, this.Width - x2, this.Height));
                e.Graphics.FillRectangle(br, new Rectangle(x1, 0, x2 - x1, y1));
                e.Graphics.FillRectangle(br, new Rectangle(x1, y2, x2 - x1, this.Height - y2));
            }
            using (Pen pen = new Pen(Color.Purple, 2)) {
                e.Graphics.DrawRectangle(pen, _selectRc);
            }
        }
        
        //Cancel the selection when the window is out of focus
        protected override void OnDeactivate(EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}