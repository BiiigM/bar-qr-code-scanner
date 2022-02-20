using System;
using System.Drawing;
using System.Windows.Forms;

namespace QRcodeScanner
{
    public partial class Form1 : Form
    {
        public Image Image;
        
        private Point _startPnt;
        private Rectangle _selectRc = new Rectangle();
        
        public static Image Snip() {
            var rc = Screen.PrimaryScreen.Bounds;
            using (Bitmap bmp = new Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb)) {
                using (Graphics gr = Graphics.FromImage(bmp))
                    gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                using (var snipper = new Form1(bmp)) {
                    if (snipper.ShowDialog() == DialogResult.OK) {
                        return snipper.Image;
                    }
                }
                return null;
            }
        }

        public Form1(Image screenShot)
        {
            InitializeComponent();
            BackgroundImage = screenShot;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            DoubleBuffered = true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if(e.Button != MouseButtons.Left) return;
            _startPnt = e.Location;
            _selectRc = new Rectangle(e.Location, new Size(0, 0));
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if(e.Button != MouseButtons.Left) return;
            int x1 = Math.Min(e.X, _startPnt.X);
            int y1 = Math.Min(e.Y, _startPnt.Y);
            int x2 = Math.Max(e.X, _startPnt.X);
            int y2 = Math.Max(e.Y, _startPnt.Y);
            _selectRc = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if(_selectRc.Width <= 0 || _selectRc.Height <= 0) return;
            Image = new Bitmap(_selectRc.Width, _selectRc.Height);
            using (Graphics gr = Graphics.FromImage(Image))
            {
                gr.DrawImage(this.BackgroundImage, new Rectangle(0, 0, Image.Width, Image.Height), _selectRc, GraphicsUnit.Pixel);
            }
            DialogResult = DialogResult.OK;
        }
        protected override void OnPaint(PaintEventArgs e) {
            // Draw the current selection
            using (Brush br = new SolidBrush(Color.FromArgb(120, Color.White))) {
                int x1 = _selectRc.X; int x2 = _selectRc.X + _selectRc.Width;
                int y1 = _selectRc.Y; int y2 = _selectRc.Y + _selectRc.Height;
                e.Graphics.FillRectangle(br, new Rectangle(0, 0, x1, this.Height));
                e.Graphics.FillRectangle(br, new Rectangle(x2, 0, this.Width - x2, this.Height));
                e.Graphics.FillRectangle(br, new Rectangle(x1, 0, x2 - x1, y1));
                e.Graphics.FillRectangle(br, new Rectangle(x1, y2, x2 - x1, this.Height - y2));
            }
            using (Pen pen = new Pen(Color.Red, 3)) {
                e.Graphics.DrawRectangle(pen, _selectRc);
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
        }
    }
}