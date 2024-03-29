﻿using System.Diagnostics;
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
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
            DWM_WINDOW_CORNER_PREFERENCE Pref = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUNDSMALL;
            WIN32.DWM.DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref Pref, sizeof(uint));
        }


        private Dictionary<Size, string> Resolutions = new Dictionary<Size, string>() {
            {new Size(3840,2160),"4K"},
            {new Size(2560,1440),"2K/QHD"},
            {new Size(1920,1080),"FHD"},
            {new Size(1600,900),"HD+"},
            {new Size(1366,768),"WXGA HD"},
            {new Size(1280,720),"HD"},
            {new Size(1024,768),"XGA"},
            {new Size(800,600),"SVGA"},
            {new Size(640,480),"VGA"},
        };

        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
            double Width = SystemParameters.PrimaryScreenWidth;
            foreach (KeyValuePair<Size, string> Resolution in Resolutions)
            {
                if (Resolution.Key.Width <= Width) break;
                else Resolutions.Remove(Resolution.Key);
            }
            //foreach (KeyValuePair<Size, string> Resolution in Resolutions)
                //ResolutionComboBox.Items.Add($"{Resolution.Key.Width}x{Resolution.Key.Height} ({Resolution.Value})");
        }
    }
}