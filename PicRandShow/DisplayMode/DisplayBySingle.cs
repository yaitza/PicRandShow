using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PicRandShow
{
    public class DisplayBySingle : DisplayMode
    {
        public DisplayBySingle(Size panelSize, List<string> strFileNames, int displayTimes = 1, int disImgCounts = 1, int intervalTime = 2) :
            base(panelSize, strFileNames, displayTimes, disImgCounts, intervalTime)
        {
        }

        public override void Play()
        {
            for (int i = 0; i < this.DisplayTimes; i++)
            {
                var pictureBoxes = this.DisplayImgs();
                PictureBoxOperator.AddPictureBox(pictureBoxes);
                Thread.Sleep(1000 * this.IntervalTime);
                PictureBoxOperator.DeletePictureBox(pictureBoxes);
            }

        }

        private PictureBox[] DisplayImgs()
        {
            Image photo = Image.FromFile(this.StrFilePaths.First());

            int photoWidth = photo.Width;
            int photoHeight = photo.Height;

            int panelWidth = this.PanelSize.Width;
            int panelHeight = this.PanelSize.Height;

            PictureBox pb = new PictureBox();

            Random ra = new Random();
            int pointX = 0;
            int pointY = 0;

            if (photoWidth > panelWidth || photoHeight > panelHeight)
            {
                pb.Location = new Point(pointX, pointY);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Size = new Size(400, 600);
                pointX = ra.Next(0, Math.Abs(panelWidth - 400));
                pointY = ra.Next(0, Math.Abs(panelHeight - 600));
            }
            else
            {
                pb.SizeMode = PictureBoxSizeMode.AutoSize;
                pointX = ra.Next(0, Math.Abs(panelWidth - photoWidth));
                pointY = ra.Next(0, Math.Abs(panelHeight - photoHeight));
            }

            pb.Location = new Point(pointX, pointY);
            pb.Image = photo;

            WritingOutput.ShowMessage($"坐标:[{pointX},{pointY}]  {this.StrFilePaths.First()}");
            return new PictureBox[] { pb };
        }
    }
}