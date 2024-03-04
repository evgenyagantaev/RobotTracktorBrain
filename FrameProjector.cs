using System;
using System.Drawing; // For Bitmap and Color

namespace RobotTracktorBrain
{
    public class FrameProjector
    {
        public static void ProjectFrameToNeurons(Bitmap frame, Neuron[,,] brainMap)
        {
            var rect = new Rectangle(0, 0, frame.Width, frame.Height);
            var bitmapData = frame.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, frame.PixelFormat);

            int bytesPerPixel = Image.GetPixelFormatSize(frame.PixelFormat) / 8;
            int byteCount = bitmapData.Stride * frame.Height;
            byte[] pixels = new byte[byteCount];

            IntPtr ptrFirstPixel = bitmapData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);

            int heightInPixels = bitmapData.Height;
            int widthInBytes = bitmapData.Width * bytesPerPixel;

            for (int y = 0; y < heightInPixels; y++)
            {
                int currentLine = y * bitmapData.Stride;
                for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                {
                    // Assuming grayscale, so taking one color component is enough
                    byte colorValue = pixels[currentLine + x + 1];

                    // Convert colorValue to potential and update neuron
                    // Note: Adjust the mapping logic based on your actual requirements
                    byte potentialValue = colorValue; // Simplification for example purposes
                    brainMap[x / bytesPerPixel, y, 0].potential = potentialValue;
                }
            }

            frame.UnlockBits(bitmapData);
        }
    }
}
