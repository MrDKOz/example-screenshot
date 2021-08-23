using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace example_screenshots.Models
{
    /// <summary>
    /// Contains all of the operations needed to take a screenshot of a window.
    /// </summary>
    public class WindowModel
    {
        public IntPtr Handle { get; set; }

        /// <summary>
        /// A created WindowModel must have a handle.
        /// </summary>
        /// <param name="windowHandle"></param>
        public WindowModel(IntPtr windowHandle)
        {
            Handle = windowHandle;
        }

        /// <summary>
        /// Fetch the dimensions of the window.
        /// </summary>
        /// <returns>A populated Dimensions model.</returns>
        public DimensionModel Dimensions()
        {
            _ = User32.GetWindowRect(new HandleRef(this, Handle), out User32.RECT rect);

            return new DimensionModel
            {
                Height = rect.Bottom - rect.Top,
                Width = rect.Right - rect.Left
            };
        }

        /// <summary>
        /// Fetch the coordinates of the window.
        /// </summary>
        /// <returns>A populated Coordinates model.</returns>
        public CoordinateModel Coordinates()
        {
            _ = User32.GetWindowRect(new HandleRef(this, Handle), out User32.RECT rect);

            return new CoordinateModel
            {
                Top = rect.Top,
                Bottom = rect.Bottom,
                Left = rect.Left,
                Right = rect.Right
            };
        }

        /// <summary>
        /// Take a screenshot of the window.
        /// </summary>
        /// <returns>A screenshot in the format of a Bitmap (.BMP)</returns>
        public Bitmap Screenshot()
        {
            var dimensions = Dimensions();
            var coordinates = Coordinates();

            var bitmap = new Bitmap(dimensions.Width, dimensions.Height, PixelFormat.Format24bppRgb);
            var graphics = Graphics.FromImage(bitmap);

            graphics.CopyFromScreen(
                coordinates.Left,
                coordinates.Top,
                0,
                0,
                new Size(
                    dimensions.Width,
                    dimensions.Height),
                CopyPixelOperation.SourceCopy);

            return bitmap;
        }

        /// <summary>
        /// Here so we can call the Windows API function for "GetWindowRect"
        /// </summary>
        private class User32
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }
        }
    }
}
