using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicRandShow
{
    class DisplayHandler
    {
        private Point formXY;

        private List<string> strFilePath;

        private int disPhotoCount;

        private int intervalTime;

        public DisplayHandler(Point sizeXY, List<string> strFilePath, int photoCount = 1, int switchTime = 2)
        {
            this.formXY = sizeXY;
            this.strFilePath = strFilePath;
            this.disPhotoCount = photoCount;
            this.intervalTime = switchTime;
        }

        public PictureBox[] PhotoPlay(DisplayEnum displayMode = DisplayEnum.Single)
        {
            switch (displayMode)
            {
                case DisplayEnum.Single:
                    return PlaySingle();
                case DisplayEnum.Multiple:
                    return PlayMultiple();
                case DisplayEnum.Random:
                    return PlayRandom();
                default:
                    return PlaySingle();
            }
        }

        private PictureBox[] PlaySingle()
        {
            Image photo = Image.FromFile(this.strFilePath.First());

            int photoWidthX = photo.Width;
            int photoHeightY = photo.Height;

            int panelWidthX = this.formXY.X;
            int panelHeightY = this.formXY.Y;

            Random ra = new Random();
            int widthX = ra.Next(0, panelWidthX - photoWidthX);
            int heightY = ra.Next(0, panelHeightY - photoHeightY);
            WritingOutput.ShowMessage($"[{widthX},{heightY}]  {this.strFilePath.First()}");
            PictureBox pb = new PictureBox();
            pb.Location = new Point(widthX, heightY);
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            pb.Image = photo;

            return new PictureBox[] { pb };
        }

        private PictureBox[] PlayMultiple()
        {
            int panelWidthX = this.formXY.X;
            int panelHeightY = this.formXY.Y;

            List<PictureBox> listPB = new List<PictureBox>();
            for (int i = 0; i < this.disPhotoCount; i++)
            {
                Random ra = new Random();

                Image photo = Image.FromFile(this.strFilePath[i]);
                int photoWidthX = photo.Width;
                int photoHeightY = photo.Height;

                if (photoWidthX >= panelWidthX || photoHeightY >= panelHeightY)
                {
                    continue;
                }

                int widthX = ra.Next(0, panelWidthX - photoWidthX);
                int heightY = ra.Next(0, panelHeightY - photoHeightY);
                WritingOutput.ShowMessage($"[{widthX},{heightY}]  {this.strFilePath[i]}");
                PictureBox pb = new PictureBox();
                pb.Location = new Point(widthX, heightY);
                pb.SizeMode = PictureBoxSizeMode.AutoSize;
                pb.Image = photo;

                listPB.Add(pb);
            }

            return listPB.ToArray();
        }

        private PictureBox[] PlayRandom()
        {
            int panelWidthX = this.formXY.X;
            int panelHeightY = this.formXY.Y;

            Random ra = new Random();

            string file = this.strFilePath[ra.Next(0, this.strFilePath.Count)];
            Image photo = Image.FromFile(file);
            int photoWidthX = photo.Width;
            int photoHeightY = photo.Height;

            if (photoWidthX >= panelWidthX || photoHeightY >= panelHeightY)
            {
                //WritingOutput.ShowMessage($"图片{file}分辨率超过窗口超限");
                throw new Exception($"图片{file}分辨率超过窗口超限");
            }

            int widthX = ra.Next(0, panelWidthX - photoWidthX);
            int heightY = ra.Next(0, panelHeightY - photoHeightY);
            WritingOutput.ShowMessage($"[{widthX},{heightY}]  {file}");

            PictureBox pb = new PictureBox();
            pb.Location = new Point(widthX, heightY);
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            pb.Image = photo;

            return new PictureBox[] { pb };
        }
    }
}
