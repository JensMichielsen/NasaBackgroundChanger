using Microsoft.Win32;
using NasaBackgroundWPF.Enums;
using NasaBackgroundWPF.Gateways;
using NasaBackgroundWPF.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NasaBackgroundWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notifyIcon = null;

        public MainWindow()
        {
            Logger.WriteToFile($"Program started {DateTime.Now}");
            InitializeComponent();
            Installer.SetAutaticStartup();
        }

        protected override void OnInitialized(EventArgs e)
        {
            Logger.WriteToFile($"Program initializing {DateTime.Now}");
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Text = "Nasa's Background Changer";
            notifyIcon.Click += new EventHandler(_OnNotifyIconLeftClick);
            notifyIcon.DoubleClick += new EventHandler(_OnNotifyIconLeftClick);
            notifyIcon.ContextMenu = _CreateContextMenu();
            notifyIcon.Icon = Properties.Resources.nasa_icon;
            Loaded += _OnLoaded;
        }

        protected override void OnClosing(CancelEventArgs cancelArgs)
        {
            // TODO: Are you sure you want to exit the program? 
        }

        private void _OnLoaded(object sender, RoutedEventArgs e)
        {
            _HideProgram();
            notifyIcon.Visible = true;
            new DisplayChanger().StartBackgroundChangeCycle(new TimeSpan(24, 0, 0), new TimeSpan(16,21,0));

            Logger.WriteToFile($"Program loaded {DateTime.Now}");
        }

        private System.Windows.Forms.ContextMenu _CreateContextMenu()
        {
            System.Windows.Forms.MenuItem closeMenuItem = new System.Windows.Forms.MenuItem();
            closeMenuItem.Index = 0;
            closeMenuItem.Text = "E&xit";
            closeMenuItem.Click += new EventHandler(_OnContextMenuCloseClick);
            System.Windows.Forms.ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { closeMenuItem });
            return contextMenu;
        }

        private void _OnNotifyIconLeftClick(object sender, EventArgs e)
        {
            // Enable the line below when MainWindow is fixed.
            // _ShowProgram();
        }

        private void _OnContextMenuCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void _HideProgram()
        {
            Hide();
            WindowState = WindowState.Minimized;
            ShowInTaskbar = false;
        }

        private void _ShowProgram()
        {
            WindowState = WindowState.Normal;
            ShowInTaskbar = true;
        }

        
    }
}
