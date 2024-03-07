using System.Net;
using System.Text;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System;

namespace HTTP_Screen_Share.Server
{
    internal class HTTPServer
    {
        #region Internal Objects

        private HttpListener Listener = new HttpListener();
        private CancellationTokenSource _cancellationSource;
        private SemaphoreSlim _semaphore;
        private int _workers;
        private List<Task> _workerList;
        private Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>> _routes = new Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>>();

        #endregion


        #region Public Objects
        public string URL => Listener.Prefixes.First();
        public bool IsListening => Listener.IsListening;

        #endregion

        public HTTPServer(string ip, int port, int workers)
        {
            Listener.Prefixes.Add($"http://{ip}:{port}/");
            _workers = workers;
            _cancellationSource = new CancellationTokenSource();
            _semaphore = new SemaphoreSlim(workers);
            _workerList = new List<Task>(workers);

            foreach (MethodInfo Method in typeof(HTTPUrls).GetMethods())
            {
                HTTPPath Attr = Method.GetCustomAttribute<HTTPPath>()!;
                if (Attr != null)
                {
                    Func<HttpListenerRequest, HttpListenerResponse, Task> handler = async (req, res) =>
                    {
                        await (Task)Method.Invoke(this, new object[] { req, res })!;
                    };
                    _routes.Add(Attr.Path, handler);

                }

            }

        }

        #region Functions

        public void Start()
        {
            Listener.Start();
            for (int x = 0; x < _workers; x++)
                _workerList.Add(Listen(_cancellationSource.Token));
        }

        public async Task Stop()
        {
            _cancellationSource.Cancel();
            Listener.Stop();
            await Task.WhenAll(_workerList.Where(task => !task.IsCompleted).ToArray());
            _workerList.Clear();
            _cancellationSource = new CancellationTokenSource();
        }

        #endregion

        #region Workers
        private async Task Listen(CancellationToken cancellationToken)
        {
            while (true)
            {
                try
                {
                    await Task.WhenAny(_semaphore.WaitAsync(), Task.Run(() => _cancellationSource.Token.WaitHandle.WaitOne()));
                    if (_cancellationSource.IsCancellationRequested) return;
                    HttpListenerContext context = await Listener.GetContextAsync();
                    await Handle(context);
                }
                catch (Exception ex)
                {
                    if (!_cancellationSource.IsCancellationRequested)
                    {
                        Console.WriteLine($"Error in worker task: {ex.Message}");
                        // Log the error or handle it appropriately
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }

        private async Task Handle(HttpListenerContext ctx)
        {
            HttpListenerRequest Req = ctx.Request;
            HttpListenerResponse Res = ctx.Response;
            Res.ContentEncoding = Encoding.UTF8;
            Func<HttpListenerRequest, HttpListenerResponse, Task>? RoutedFunction;
            if (_routes.TryGetValue(Req.Url!.AbsolutePath.ToString(), out RoutedFunction))
            {
                await RoutedFunction.Invoke(Req,Res);
            } else
            {
                // Show 404 Error
                byte[] data = Encoding.UTF8.GetBytes("404 Error");
                Res.ContentType = "text/html";
                Res.ContentLength64 = data.LongLength;
                await Res.OutputStream.WriteAsync(data, 0, data.Length);
            }

  
            Res.Close();
        }

        #endregion

    }
}
