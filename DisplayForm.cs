using System.Drawing;
using System.Windows.Forms;

namespace RobotTracktorBrain
{
    public class DisplayForm : Form
    {
        private Bitmap displayedBitmap;

        public DisplayForm()
        {
            this.ClientSize = new Size(136, 136); // Match the size of the original form
            this.Text = "Display Form";
        }

        public void SetBitmap(Bitmap bitmap)
        {
            // Dispose of the previous bitmap
            if (displayedBitmap != null) displayedBitmap.Dispose();

            displayedBitmap = (Bitmap)bitmap.Clone();

            this.Invalidate(); // Cause the form to redraw
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (displayedBitmap != null)
            {
                e.Graphics.DrawImage(displayedBitmap, 0, 0);
            }
        }
    }
}
