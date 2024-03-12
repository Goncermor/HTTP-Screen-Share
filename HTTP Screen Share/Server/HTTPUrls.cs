

using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;

namespace HTTP_Screen_Share.Server
{
    public class HTTPUrls
    {

        [HTTPPath("/")]
        public static async Task Index(HttpListenerContext ctx)
        {
            HttpListenerRequest Req = ctx.Request;
            HttpListenerResponse Res = ctx.Response;

            byte[] data = Encoding.UTF8.GetBytes("Index");

            Res.ContentType = "text/html";
            Res.ContentLength64 = data.LongLength;
            await Res.OutputStream.WriteAsync(data, 0, data.Length);
        }


        [HTTPPath("/img")]
        public static async Task Img(HttpListenerContext ctx)
        {
            HttpListenerRequest Req = ctx.Request;
            HttpListenerResponse Res = ctx.Response;

            int screenWidth = (int)SystemParameters.FullPrimaryScreenWidth;
            int screenHeight = (int)SystemParameters.FullPrimaryScreenHeight;
            using (Bitmap captureBitmap = new Bitmap(screenWidth, screenHeight))
            {
                using (Graphics g = Graphics.FromImage(captureBitmap))
                {
                    g.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(screenWidth, screenHeight));
                }

                // Convert the bitmap to a byte array (adjust format as needed)
                byte[] imageData;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    captureBitmap.Save(memoryStream, ImageFormat.Jpeg);
                    imageData = memoryStream.ToArray();
                   
                }

               

            }

            ctx.Response.ContentType = "image/jpeg";
            ctx.Response.ContentLength64 = imageData.LongLength;
            await ctx.Response.OutputStream.WriteAsync(imageData, 0, imageData.Length);

        }

    }
}
