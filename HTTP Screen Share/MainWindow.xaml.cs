using HTTP_Screen_Share.Server;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static HTTP_Screen_Share.WIN32.DWM;

namespace HTTP_Screen_Share
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
            DWM_WINDOW_CORNER_PREFERENCE Pref = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUNDSMALL;
            WIN32.DWM.DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref Pref, sizeof(uint));
        }

        Server.HTTPServer Srv = new Server.HTTPServer("127.0.0.1",8180, 12);

        private void ServerStartButton_Click(object sender, RoutedEventArgs e)
        {
            if (Srv.IsListening)
            {
                _ = Srv.Stop();
                this.ServerStartButton.Content = "Start Server";

            } else
            {
                Srv.Start();
                this.ServerStartButton.Content = "Stop Server";
            }
        }
    }
}