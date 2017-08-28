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
                var pb = this.DisplayImgs(i);
                for (int step = 0; step < PanelSize.Width; step++)
                {
                    PictureBoxOperator.AddPictureBox(pb);
                    Thread.Sleep(500);
                    PictureBoxOperator.DeletePictureBox(pb);
                }
            }

        }

        private PictureBox[] DisplayImgs(int iCount)
        {
            Image photo = Image.FromFile(this.StrFilePaths[iCount]);

            int photoWidth = photo.Width;
            int photoHeight = photo.Height;

            int panelWidth = this.PanelSize.Width;
            int panelHeight = this.PanelSize.Height;

            PictureBox pb = new PictureBox();


            if (photoWidth > panelWidth || photoHeight > panelHeight)
            {
                pb.Location = new Point(0, 0);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Size = new Size(400, 600);
            }
            else
            {
                pb.SizeMode = PictureBoxSizeMode.AutoSize;

            }

            pb.Location = new Point(0, 0);
            pb.Image = photo;

            WritingOutput.ShowMessage($"坐标:[{0},{0}]  {this.StrFilePaths.First()}");
            return new PictureBox[] { pb };
        }


    }
}