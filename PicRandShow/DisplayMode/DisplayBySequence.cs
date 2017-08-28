using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PicRandShow
{
    public class DisplayBySequence : DisplayMode
    {

        public DisplayBySequence(Size panelSize, List<string> strFileNames, int displayTimes = 1, int disImgCounts = 1, int intervalTime = 2) :
            base(panelSize, strFileNames, displayTimes, disImgCounts, intervalTime)
        {
        }

        public override void Play()
        {
            for (int i = 0; i < this.DisplayTimes; i++)
            {
                var pictureBoxes = this.DisplayImgs(i);
                PictureBoxOperator.AddPictureBox(pictureBoxes);
                Thread.Sleep(1000 * this.IntervalTime);
                PictureBoxOperator.DeletePictureBox(pictureBoxes);
            }

        }

        private PictureBox[] DisplayImgs(int iCount)
        {
            int iImagePosition = iCount;
            if (iCount > this.StrFilePaths.Count)
            {
                iImagePosition = iCount % this.StrFilePaths.Count;
            }

            Image photo = Image.FromFile(this.StrFilePaths[iImagePosition]);

            int photoWidthX = photo.Width;
            int photoHeightY = photo.Height;

            int panelWidthX = this.PanelSize.Width;
            int panelHeightY = this.PanelSize.Height;

            Random ra = new Random();
            int widthX = 0;
            int heightY = 0;
            PictureBox pb = new PictureBox();

            if (panelWidthX < photoWidthX || panelHeightY < photoHeightY)
            {
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

            WritingOutput.ShowMessage($"坐标:[{widthX},{heightY}]  {this.StrFilePaths[iImagePosition]}");

            return new PictureBox[] { pb };
        }
    }
}