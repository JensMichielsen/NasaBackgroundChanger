using Newtonsoft.Json.Linq;
using NasaBackgroundWPF.Models;
using System.Drawing;
using System.IO;
using System.Net;

namespace NasaBackgroundWPF.Gateways
{
    class NasaApiGateway
    {
        private WebClient _WebClient;
        private const string path = "https://api.nasa.gov/planetary/apod?api_key=fvYiQxVR7OSIcrw6ibzYCvmbtc79VCFvMeQLucsM";

        public NasaApiGateway()
        {
            _WebClient = new WebClient();
        }

        public PictureInfo GetPictureOfTheDay()
        {
            Stream data = _WebClient.OpenRead(path);
            StreamReader reader = new StreamReader(data);
            string result = reader.ReadToEnd();
            data.Close();
            reader.Close();

            JObject jsonObj = JObject.Parse(result);
            return jsonObj.ToObject<PictureInfo>();
        }

        public string DownloadPicture(PictureInfo pictureInfo)
        {
            Stream s = new WebClient().OpenRead(pictureInfo.HdUrl);

            Image img = Image.FromStream(s);
            string tempPath = Path.Combine(Path.GetTempPath(), $"{pictureInfo.Title}.jpg");
            img.Save(tempPath, System.Drawing.Imaging.ImageFormat.Jpeg);

            return tempPath;
        }
    }
}