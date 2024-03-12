
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using Size = System.Drawing.Size;


namespace HTTP_Screen_Share
{
    internal class ScreenCapture
    {
        private static Size Screen = new System.Drawing.Size()
        {
            Height = (int)SystemParameters.FullPrimaryScreenHeight,
            Width = (int)SystemParameters.FullPrimaryScreenWidth
        };
        private static EncoderParameter[] EncoderCfg = new EncoderParameter[] {
            new EncoderParameter(Encoder.Quality, (long)75),
        };
        private static EncoderParameters EncoderParams = new EncoderParameters();
        public static byte[] GetImage()
        {
            using (Bitmap captureBitmap = new Bitmap(Screen.Width, Screen.Height))
            {
                using (Graphics g = Graphics.FromImage(captureBitmap))
                {
                    g.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(Screen.Width, Screen.Height));
                }
                byte[] imageData;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    captureBitmap.Save(memoryStream, ImageFormat.Jpeg, EncoderParams);
                    imageData = memoryStream.ToArray();
                }
                return imageData;
            }

        }

    }
}
