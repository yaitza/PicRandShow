using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PicRandShow
{
    public class DisplayByRandom : DisplayMode
    {
        public DisplayByRandom(Size panelSize, List<string> strFileNames, int displayTimes = 1, int disImgCounts = 1,
            int intervalTime = 2) :
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
            int panelWidthX = this.PanelSize.Width;
            int panelHeightY = this.PanelSize.Height;
            Random ra = new Random();

            string file = this.StrFilePaths[ra.Next(0, this.StrFilePaths.Count)];
            Image photo = Image.FromFile(file);
            int photoWidthX = photo.Width;
            int photoHeightY = photo.Height;

            PictureBox pb = new PictureBox();
            int widthX = 0;
            int heightY = 0;
            if (photoWidthX >= panelWidthX || photoHeightY >= panelHeightY)
            {
                WritingOutput.ShowMessage($"Info:图片{file}分辨率超过窗口超限");
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Size = new Size(400, 600);
                widthX = ra.Next(0, Math.Abs(panelWidthX - 400));
                heightY = ra.Next(0, Math.Abs(panelHeightY - 600));
            }
            else
            {
                pb.SizeMode = PictureBoxSizeMode.AutoSize;
                widthX = ra.Next(0, Math.Abs(panelWidthX - photoWidthX));
                heightY = ra.Next(0, Math.Abs(panelHeightY - photoHeightY));
            }
            pb.Location = new Point(widthX, heightY);
            pb.Image = photo;

            WritingOutput.ShowMessage($"坐标:[{widthX},{heightY}]  {file}");

            return new PictureBox[] {pb};
        }
    }
}