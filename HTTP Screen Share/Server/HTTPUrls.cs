

using System.Net;
using System.Text;

namespace HTTP_Screen_Share.Server
{
    public class HTTPUrls
    {

        [HTTPPath("/")]
        public static async Task Index(HttpListenerRequest Req, HttpListenerResponse Res, HttpListenerContext ctx)
        {
            byte[] data = Encoding.UTF8.GetBytes("Index");
            
            Res.ContentType = "text/html";
            Res.ContentLength64 = data.LongLength;
            await Res.OutputStream.WriteAsync(data, 0, data.Length);
        }

        [HTTPPath("/sla")]
        public static async Task sla(HttpListenerRequest Req, HttpListenerResponse Res, HttpListenerContext ctx)
        {
            byte[] data = Encoding.UTF8.GetBytes("Sla");
            Res.ContentType = "text/html";
            Res.ContentLength64 = data.LongLength;
            await Res.OutputStream.WriteAsync(data, 0, data.Length);
        }

        [HTTPPath("/123")]
        public static async Task s123(HttpListenerRequest Req, HttpListenerResponse Res, HttpListenerContext ctx)
        {
            byte[] data = Encoding.UTF8.GetBytes("23456789");
            Res.ContentType = "text/html";
            Res.ContentLength64 = data.LongLength;

            await Res.OutputStream.WriteAsync(data, 0, data.Length);
        }

        [HTTPPath("/gcm")]
        public static async Task Gcm(HttpListenerRequest Req, HttpListenerResponse Res, HttpListenerContext ctx)
        {
            byte[] data = Encoding.UTF8.GetBytes("Goncermor");
            Res.ContentType = "text/html";
            Res.ContentLength64 = data.LongLength;

            await Res.OutputStream.WriteAsync(data, 0, data.Length);
        }

    }

}
