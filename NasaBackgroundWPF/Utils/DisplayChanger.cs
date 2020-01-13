using Microsoft.Win32;
using NasaBackgroundWPF.Enums;
using NasaBackgroundWPF.Gateways;
using System;
using System.Runtime.InteropServices;
using System.Timers;

namespace NasaBackgroundWPF.Utils
{
    public class DisplayChanger
    {
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private Timer _Timer = new Timer();
        private NasaApiGateway _ApiGateway = new NasaApiGateway();

        private TimeSpan _BackgroundChangeCycleLength;

        public void StartBackgroundChangeCycle(TimeSpan backgroundChangeCycleLength, TimeSpan activationTime)
        {
            _GetImageAndSetAsBackground();

            Logger.WriteToFile("Cycle started at " + DateTime.Now);

            _BackgroundChangeCycleLength = backgroundChangeCycleLength;
            TimeSpan now = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));

            TimeSpan timeLeftUntilFirstRun = ((_BackgroundChangeCycleLength - now) + activationTime);
            if (timeLeftUntilFirstRun.TotalHours > 24) timeLeftUntilFirstRun -= new TimeSpan(24, 0, 0);    // Deducts a day from the schedule so it will run today.

            _Timer.Interval = timeLeftUntilFirstRun.TotalMilliseconds;
            _Timer.Elapsed += new ElapsedEventHandler(_OnElapsedTime);
            _Timer.Start();

            Logger.WriteToFile("Hours remaining till next update: " + _Timer.Interval / 3600000 + " h");
        }

        private void _OnElapsedTime(object source, ElapsedEventArgs e)
        {
            _GetImageAndSetAsBackground();
            _Timer.Interval = _BackgroundChangeCycleLength.TotalMilliseconds;
        }

        public bool _GetImageAndSetAsBackground()
        {
            Logger.WriteToFile("Changing background at" + DateTime.Now);
            Logger.WriteToFile("Hours remaining till next update: " + _Timer.Interval / 3600000);

            try
            {
                var imageResult = _ApiGateway.GetPictureOfTheDay();
                var imagePath = _ApiGateway.DownloadPicture(imageResult);

                _SetRegistryForNewBackground(imagePath, BackgroundStyles.Fill);
            }
            catch (Exception exception)
            {
                Logger.WriteToFile("Error: " + exception);
            }

            return true;
        }

        public void _SetRegistryForNewBackground(string tempPath, BackgroundStyles style)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (style == BackgroundStyles.Fill)
            {
                key.SetValue(@"WallpaperStyle", 10.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == BackgroundStyles.Fit)
            {
                key.SetValue(@"WallpaperStyle", 6.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == BackgroundStyles.Span) // Windows 8 or newer only!
            {
                key.SetValue(@"WallpaperStyle", 22.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == BackgroundStyles.Stretch)
            {
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == BackgroundStyles.Tile)
            {
                key.SetValue(@"WallpaperStyle", 0.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }
            if (style == BackgroundStyles.Center)
            {
                key.SetValue(@"WallpaperStyle", 0.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0,
                tempPath,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

        }
    }
}
