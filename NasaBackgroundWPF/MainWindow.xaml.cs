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
            InitializeComponent();
            _HideProgram();

            var apiGateway = new NasaApiGateway();
            var displayChanger = new DisplayChanger();

            var imageResult = apiGateway.GetPictureOfTheDay();
            var imagePath = apiGateway.DownloadPicture(imageResult);

            displayChanger.ChangeDisplay(imagePath, BackgroundStyles.Fill);
        }

        protected override void OnInitialized(EventArgs e)
        {
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
            notifyIcon.Visible = true;
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
