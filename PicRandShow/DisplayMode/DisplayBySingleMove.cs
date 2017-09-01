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
                for (int step = 0; step < PanelSize.Width - 5; step = step + 5)
                {
                    var pb = this.DisplayImgs(i, step);

                    PictureBoxOperator.AddPictureBox(pb);
                    Thread.Sleep(500 * this.IntervalTime);
                    PictureBoxOperator.AddPictureBox(this.DisplayImgs(i, step + 5));
                    PictureBoxOperator.DeletePictureBox(pb);
                }
            }

        }

        private PictureBox[] DisplayImgs(int iCount, int iStep)
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
                pb.Location = new Point(iStep, panelHeight / 2 - 300);
            }
            else
            {
                pb.SizeMode = PictureBoxSizeMode.AutoSize;
                pb.Location = new Point(iStep, panelHeight / 2 - photoHeight / 2);

            }

            pb.Image = photo;
            pb.BackColor = Color.Transparent;

            WritingOutput.ShowMessage($"坐标:[{0},{0}]  {this.StrFilePaths.First()}");
            return new PictureBox[] { pb };
        }


    }
}