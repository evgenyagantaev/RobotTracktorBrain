using System;
using System.Drawing;
using System.Windows.Forms;

namespace RobotTracktorBrain
{
    public class MovingSquareForm : Form
    {
        private const int MovingSquareSize = 4;
        private const int StaticSquareSize = 40;
        private Point movingSquarePosition;
        private Point staticSquarePosition;
        private readonly Timer timer;
        private readonly Random random = new Random();

        public MovingSquareForm()
        {
            this.ClientSize = new Size(400, 400);
            this.BackColor = Color.Black;
            this.Text = "Moving Square";

            InitializePositions();

            timer = new Timer();
            //timer.Interval = 1000; // Move every second
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void InitializePositions()
        {
            movingSquarePosition = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2);
            InitializeStaticSquarePosition();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRectangle(Brushes.White, movingSquarePosition.X, movingSquarePosition.Y, MovingSquareSize, MovingSquareSize);
            e.Graphics.FillRectangle(Brushes.White, staticSquarePosition.X, staticSquarePosition.Y, StaticSquareSize, StaticSquareSize);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (IsCollision())
            {
                InitializePositions(); // Reset positions if collided
            }
            else
            {
                MoveMovingSquareTowardsStaticSquare();
            }

            this.Invalidate(); // Redraw form
        }

        private void MoveMovingSquareTowardsStaticSquare()
        {
            // Determine the direction to move towards the static square
            int dx = staticSquarePosition.X + StaticSquareSize / 2 - movingSquarePosition.X;
            int dy = staticSquarePosition.Y + StaticSquareSize / 2 - movingSquarePosition.Y;

            if (Math.Abs(dx) > Math.Abs(dy))
            {
                // Move horizontally
                movingSquarePosition.X += Math.Sign(dx) * 2;
            }
            else if (dy != 0)
            {
                // Move vertically
                movingSquarePosition.Y += Math.Sign(dy) * 2;
            }

            // Keep the moving square within the bounds of the window
            movingSquarePosition.X = Math.Max(0, Math.Min(this.ClientSize.Width - MovingSquareSize, movingSquarePosition.X));
            movingSquarePosition.Y = Math.Max(0, Math.Min(this.ClientSize.Height - MovingSquareSize, movingSquarePosition.Y));
        }

        private bool IsCollision()
        {
            Rectangle movingSquareRect = new Rectangle(movingSquarePosition.X, movingSquarePosition.Y, MovingSquareSize, MovingSquareSize);
            Rectangle staticSquareRect = new Rectangle(staticSquarePosition.X, staticSquarePosition.Y, StaticSquareSize, StaticSquareSize);

            return movingSquareRect.IntersectsWith(staticSquareRect);
        }

        private void InitializeStaticSquarePosition()
        {
            switch (random.Next(4)) // 0: Top, 1: Bottom, 2: Left, 3: Right
            {
                case 0: staticSquarePosition = new Point(random.Next(0, this.ClientSize.Width - StaticSquareSize), 0); break;
                case 1: staticSquarePosition = new Point(random.Next(0, this.ClientSize.Width - StaticSquareSize), this.ClientSize.Height - StaticSquareSize); break;
                case 2: staticSquarePosition = new Point(0, random.Next(0, this.ClientSize.Height - StaticSquareSize)); break;
                case 3: staticSquarePosition = new Point(this.ClientSize.Width - StaticSquareSize, random.Next(0, this.ClientSize.Height - StaticSquareSize)); break;
            }
        }
    }
}
