using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasaBackgroundWPF.Models
{
    public class PictureInfo
    {
        public DateTime Date { get; set; }
        public string Explanation { get; set; }
        public string HdUrl { get; set; }
        public string Media_type { get; set; }
        public string Service_version { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}