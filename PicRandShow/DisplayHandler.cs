using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
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

        public PictureBox[] PhotoPlay(int iCount, DisplayEnum displayMode = DisplayEnum.Single)
        {
            switch (displayMode)
            {
                case DisplayEnum.Single:
                    return PlaySingle();
                case DisplayEnum.Multiple:
                    return PlayMultiple();
                case DisplayEnum.Random:
                    return PlayRandom();
                case DisplayEnum.Sequence:
                    return PlaySequence(iCount);
                default:
                    return PlaySingle();
            }
        }

        private PictureBox[] PlaySequence(int iCount)
        {
            int iImagePosition = iCount;
            if (iCount > this.strFilePath.Count)
            {
                iImagePosition = iCount - this.strFilePath.Count - 1;
            }

            Image photo = Image.FromFile(this.strFilePath[iImagePosition]);

            int photoWidthX = photo.Width;
            int photoHeightY = photo.Height;

            int panelWidthX = this.formXY.X;
            int panelHeightY = this.formXY.Y;

            Random ra = new Random();
            int widthX = ra.Next(0, Math.Abs(panelWidthX - 400));
            int heightY = ra.Next(0, Math.Abs(panelHeightY - 600));
            WritingOutput.ShowMessage($"坐标:[{widthX},{heightY}]  {this.strFilePath[iImagePosition]}");
            PictureBox pb = new PictureBox();
            pb.Location = new Point(widthX, heightY);

            if (panelWidthX < photoWidthX || panelHeightY < photoHeightY)
            {
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Size = new Size(400, 600);
            }
            else
            {
                pb.SizeMode = PictureBoxSizeMode.AutoSize;
            }
            
            pb.Image = photo;

            return new PictureBox[] { pb };

            throw new NotImplementedException();
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
            WritingOutput.ShowMessage($"坐标:[{widthX},{heightY}]  {this.strFilePath.First()}");
            PictureBox pb = new PictureBox();
            pb.Location = new Point(widthX, heightY);
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            pb.Image = photo;

            return new PictureBox[] { pb };
        }

        private PictureBox[] PlayMultiple()
        {
            List<string> picLoactionInfos = new List<string>();
            int panelWidthX = this.formXY.X;
            int panelHeightY = this.formXY.Y;

            List<PictureBox> listPb = new List<PictureBox>();
            for (int i = 0; i < this.disPhotoCount; i++)
            {
                Random ra = new Random();

                Image photo = Image.FromFile(this.strFilePath[ra.Next(0, this.strFilePath.Count)]);
                int photoWidthX = photo.Width;
                int photoHeightY = photo.Height;

                if (photoWidthX >= panelWidthX || photoHeightY >= panelHeightY)
                {
                    continue;
                }

                int widthX = ra.Next(0, panelWidthX - photoWidthX);
                int heightY = ra.Next(0, panelHeightY - photoHeightY);

                string picLocation = $"{widthX}|{heightY}|{photoWidthX}|{photoHeightY}";

                if (picLoactionInfos.Count >= 1)
                {
                    if (!this.IsFullDispaly(picLoactionInfos, picLocation))
                    {
                        WritingOutput.ShowMessage($"坐标:[{widthX},{heightY}]  {this.strFilePath[i]} 与之前图片位置冲突");
                        continue;
                    }
                }

                WritingOutput.ShowMessage($"坐标:[{widthX},{heightY}]  {this.strFilePath[i]}");
                PictureBox pb = new PictureBox();
                pb.Location = new Point(widthX, heightY);
                pb.SizeMode = PictureBoxSizeMode.AutoSize;
                pb.Image = photo;

                listPb.Add(pb);
                picLoactionInfos.Add(picLocation);
            }

            return listPb.ToArray();
        }

        /// <summary>
        /// 判断图片是否重叠
        /// </summary>
        /// <param name="picLocations">已添加图片位置信息</param>
        /// <param name="location">新添加图片位置信息</param>
        /// <returns>是否重叠</returns>
        private bool IsFullDispaly(List<string> picLocations, string location)
        {
            List<bool> isInEveryPicRegion = new List<bool>();

            string[] imageLocInfo = location.Split('|');
            int imgX = int.Parse(imageLocInfo[0]);
            int imgY = int.Parse(imageLocInfo[1]);
            int imgWidth = int.Parse(imageLocInfo[2]);
            int imgHeight = int.Parse(imageLocInfo[3]);
            Point[] imgPoints = {
                    new Point(imgX, imgY),
                    new Point(imgX + imgWidth, imgY),
                    new Point(imgX + imgWidth, imgY + imgHeight),
                    new Point(imgX, imgY + imgHeight)
                };

            Region imgRegion = new Region();
            GraphicsPath imgPolygon = new GraphicsPath();
            imgPolygon.Reset();
            imgPolygon.AddPolygon(imgPoints);
            imgRegion.MakeEmpty();
            imgRegion.Union(imgPolygon);

            foreach (string picLocation in picLocations)
            {
                string[] picLocInfo = picLocation.Split('|');
                int picX = int.Parse(picLocInfo[0]);
                int picY = int.Parse(picLocInfo[1]);
                int picWidth = int.Parse(picLocInfo[2]);
                int picHeight = int.Parse(picLocInfo[3]);
                Point[] picPoints = {
                    new Point(picX, picY),
                    new Point(picX + picWidth, picY),
                    new Point(picX + picWidth, picY + picHeight),
                    new Point(picX, picY + picHeight)
                };

                Region picRegion = new Region();
                GraphicsPath picPolygon = new GraphicsPath();
                picPolygon.Reset();
                picPolygon.AddPolygon(picPoints);
                picRegion.MakeEmpty();
                picRegion.Union(picPolygon);

                bool isInPicRegion = true;
                foreach (Point imgPoint in imgPoints)
                {
                    //WritingOutput.ShowMessage($"Point:{imgPoint.X},{imgPoint.Y}[{!picRegion.IsVisible(imgPoint)}]在区域[{picX},{picY}][{picX + picWidth},{picY + picHeight}]");
                    isInPicRegion = isInPicRegion && !picRegion.IsVisible(imgPoint);
                    //Thread.Sleep(1000 * 3);

                }

                foreach (Point picPoint in picPoints)
                {
                    isInPicRegion = isInPicRegion && !imgRegion.IsVisible(picPoint);
                }

                isInEveryPicRegion.Add(isInPicRegion);
            }

            bool isAddThePic = true;
            foreach (bool isInRegion in isInEveryPicRegion)
            {
                isAddThePic = isAddThePic && isInRegion;
            }

            return isAddThePic;

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
            WritingOutput.ShowMessage($"坐标:[{widthX},{heightY}]  {file}");

            PictureBox pb = new PictureBox();
            pb.Location = new Point(widthX, heightY);
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            pb.Image = photo;

            return new PictureBox[] { pb };
        }
    }
}
