using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicRandShow
{
    public class DisplayMode
    {
        protected Size PanelSize;

        protected List<string> StrFilePaths;

        protected int DisplayTimes;

        protected int DisImgCounts;

        protected int IntervalTime;

        public DisplayMode()
        {

        }

        public DisplayMode(Size panelSize, List<string> strFileNames, int displayTimes = 1, int disImgCounts = 1, int intervalTime = 2)
        {
            this.PanelSize = panelSize;
            this.StrFilePaths = strFileNames;
            this.DisplayTimes = displayTimes;
            this.DisImgCounts = disImgCounts;
            this.IntervalTime = intervalTime;
        }

        public virtual void Play()
        {

        }
    }
}
