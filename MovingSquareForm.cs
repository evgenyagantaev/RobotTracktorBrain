﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace RobotTracktorBrain
{
    public class MovingSquareForm : Form
    {
        private DisplayForm displayForm;

        private const int MovingSquareSize = 4;
        private const int StaticSquareSize = 20;
        private Point movingSquarePosition;
        private Point staticSquarePosition;
        private readonly Timer timer;
        private readonly Random random = new Random();
        private Neuron[,,] brainMap; // Assuming this is how you define your brain map

        const int CLIENT_SIZE = 136;

        public MovingSquareForm(Neuron[,,] brainMap)
        {
            // Set the form's border style to None to remove the border and title bar
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Initialize and show the display form
            displayForm = new DisplayForm();
            displayForm.Show();

            this.brainMap = brainMap; // Initialize the brain map

            this.ClientSize = new Size(CLIENT_SIZE, CLIENT_SIZE);
            this.BackColor = Color.Black;
            //this.Text = "Moving Square";

            InitializePositions();

            timer = new Timer();
            timer.Interval = 1;
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

            // Optionally, directly project the frame after each paint
            ProjectFrameToBrain();
            var inputLayerBitmap = CreateBitmapFromBrainMapLayerZero(brainMap, CLIENT_SIZE, CLIENT_SIZE);
            DisplayCapturedBitmap(inputLayerBitmap);
        }

        Bitmap frameBitmap = new Bitmap(CLIENT_SIZE, CLIENT_SIZE);
        private void ProjectFrameToBrain()
        {
            //Bitmap frameBitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            this.DrawToBitmap(frameBitmap, new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height));
            

            FrameProjector.ProjectFrameToNeurons(frameBitmap, brainMap);
            //DisplayCapturedBitmap(frameBitmap);

            //frameBitmap.Dispose(); // Dispose of the bitmap to free resources
        }

        public Bitmap CreateBitmapFromBrainMapLayerZero(Neuron[,,] brainMap, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Assuming potential is a byte from 0 to 255
                    byte potential = brainMap[x, y, 0].potential;
                    Color color = Color.FromArgb(potential, potential, potential); // Grayscale
                    bitmap.SetPixel(x, y, color);
                }
            }
            return bitmap;
        }

        private void DisplayCapturedBitmap(Bitmap capturedBitmap)
        {
            if (capturedBitmap != null && !displayForm.IsDisposed)
            {
                displayForm.SetBitmap(capturedBitmap);
            }
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
