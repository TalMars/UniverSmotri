using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverSmotri.Settings
{
    public static class Settings
    {
        private static string _qualityStreamVideo;

        public static string QualityStreamVideo
        {
            get { return _qualityStreamVideo; }
            set { _qualityStreamVideo = value; }
        }

        private static string _qualityYouTubeVideo;

        public static string QualityYouTubeVideo
        {
            get { return _qualityYouTubeVideo; }
            set { _qualityYouTubeVideo = value; }
        }

    }
}
