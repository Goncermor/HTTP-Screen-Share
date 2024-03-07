using System.Net;
using System.Reflection;

namespace HTTP_Screen_Share.Server
{
    public class HTTPPath : Attribute
    {
        public string Path { get; set; }
        public HTTPPath(string path)
        {
            Path = path;
        }
    }
}
