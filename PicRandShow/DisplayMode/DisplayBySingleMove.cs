using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PicRandShow
{
    public class DisplayBySingleMove : DisplayMode
    {
        public DisplayBySingleMove(Size panelSize, List<string> strFileNames, int displayTimes = 1, int disImgCounts = 1,
            int intervalTime = 2) :
                base(panelSize, strFileNames, displayTimes, disImgCounts, intervalTime)
        {

        }

        public override void Play()
        {
            for (int i = 0; i < this.DisImgCounts; i++)
            {
                int height = 0;
                var pb = this.DisplayImgs(i, ref height);
                PictureBoxOperator.AddPictureBox(pb);

                for (int step = 0; step < PanelSize.Width - 11; step = step + 10)
                {
                    Point location = new Point(step, height);
                    PictureBoxOperator.MoveLocation(location, pb[0]);
                    WritingOutput.ShowMessage($"坐标:[{location.X},{location.Y}]");
                    Thread.Sleep(50 * this.IntervalTime);
                }
                PictureBoxOperator.DeletePictureBox(pb);
            }
        }

        private PictureBox[] DisplayImgs(int iCount, ref int height)
        {
            Image photo = Image.FromFile(this.StrFilePaths[iCount]);

            int photoWidth = photo.Width;
            int photoHeight = photo.Height;

            int panelWidth = this.PanelSize.Width;
            int panelHeight = this.PanelSize.Height;

            PictureBox pb = new PictureBox();


            if (photoWidth > panelWidth || photoHeight > panelHeight)
            {
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Size = new Size(400, 600);
                height = panelHeight / 2 - 300 / 2;
            }
            else
            {
                pb.SizeMode = PictureBoxSizeMode.AutoSize;
                height = panelHeight / 2 - photoHeight / 2;
            }

            pb.Location = new Point(0, height);
            pb.Image = photo;
            pb.BackColor = Color.Transparent;

            WritingOutput.ShowMessage($"坐标:[{0},{height}]  {this.StrFilePaths[iCount]}");
            return new PictureBox[] { pb };
        }


    }
}