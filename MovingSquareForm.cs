using System;
using System.Drawing;
using System.Windows.Forms;

namespace RobotTracktorBrain
{
    public class MovingSquareForm : Form
    {
        private const int SquareSize = 4;
        private Point squarePosition;
        private readonly Timer timer;
        private readonly Random random = new Random();

        public MovingSquareForm()
        {
            this.ClientSize = new Size(400, 400);
            this.BackColor = Color.Black;
            this.Text = "Moving Square";

            // Initialize square position
            squarePosition = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2);

            // Setup timer
            timer = new Timer();
            timer.Interval = 1000; // Move every second
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRectangle(Brushes.White, squarePosition.X, squarePosition.Y, SquareSize, SquareSize);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Move the square in a random direction by 2 pixels
            switch (random.Next(4)) // 0: Up, 1: Down, 2: Left, 3: Right
            {
                case 0: squarePosition.Y = Math.Max(0, squarePosition.Y - 2); break;
                case 1: squarePosition.Y = Math.Min(this.ClientSize.Height - SquareSize, squarePosition.Y + 2); break;
                case 2: squarePosition.X = Math.Max(0, squarePosition.X - 2); break;
                case 3: squarePosition.X = Math.Min(this.ClientSize.Width - SquareSize, squarePosition.X + 2); break;
            }

            this.Invalidate(); // Causes the form to be redrawn
        }
    }
}
