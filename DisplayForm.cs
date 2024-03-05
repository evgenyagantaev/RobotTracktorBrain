using System;
using System.Drawing;
using System.Windows.Forms;

namespace RobotTracktorBrain
{
    public class DisplayForm : Form
    {
        private Bitmap displayedBitmap;
        private Button stopSaveButton;

        public DisplayForm()
        {
            this.ClientSize = new Size(136, 136); // Match the size of the original form
            this.Text = "Display Form";

            InitializeControls();
        }

        private void InitializeControls()
        {
            // Create and setup the Stop/Save button
            stopSaveButton = new Button
            {
                Text = "Stop/Save",
                Location = new Point(10, this.ClientSize.Height - 40), // Position the button at the bottom
                Size = new Size(100, 30)
            };
            this.Controls.Add(stopSaveButton);

            // Register the click event handler
            stopSaveButton.Click += StopSaveButton_Click;
        }

        private void StopSaveButton_Click(object sender, EventArgs e)
        {
            // Implement the stop/save logic here
            Console.WriteLine("stop/save button");
            BrainSerializer.SerializeBrainMap($"E:/workspace/robot_tracktor/brain_repo");
            Application.Exit();
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
