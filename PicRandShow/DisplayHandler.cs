using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private Point panelXY;

        private List<string> strFilePath;

        private int disPhotoCount;

        private int intervalTime;

        public DisplayHandler(Point sizeXY, List<string> strFilePath, int photoCount = 1, int switchTime = 2)
        {
            this.panelXY = sizeXY;
            this.strFilePath = strFilePath;
            this.disPhotoCount = photoCount;
            this.intervalTime = switchTime;
        }

        public PictureBox[] PhotoPlay(int iCount, DisplayEnum displayMode = DisplayEnum.Single)
        {


            switch (displayMode)
            {
                case DisplayEnum.Single:
                    return PlayBySingle();
                case DisplayEnum.Multiple:
                    return PlayByMultiple();
                case DisplayEnum.Random:
                    return PlayByRandom();
                case DisplayEnum.Sequence:
                    return PlayBySequence(iCount);
                case DisplayEnum.SingleMove:
                    return PlayBySingleMove();
                default:
                    return PlayBySingle();
            }
        }

        private PictureBox[] PlayBySequence(int iCount)
        {
            int iImagePosition = iCount;
            if (iCount > this.strFilePath.Count)
            {
                iImagePosition = iCount % this.strFilePath.Count;
            }

            Image photo = Image.FromFile(this.strFilePath[iImagePosition]);

            int photoWidthX = photo.Width;
            int photoHeightY = photo.Height;

            int panelWidthX = this.panelXY.X;
            int panelHeightY = this.panelXY.Y;

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

            WritingOutput.ShowMessage($"坐标:[{widthX},{heightY}]  {this.strFilePath[iImagePosition]}");

            return new PictureBox[] { pb };
        }

        private PictureBox[] PlayBySingle()
        {
            Image photo = Image.FromFile(this.strFilePath.First());

            int photoWidth = photo.Width;
            int photoHeight = photo.Height;

            int panelWidth = this.panelXY.X;
            int panelHeight = this.panelXY.Y;

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

            WritingOutput.ShowMessage($"坐标:[{pointX},{pointY}]  {this.strFilePath.First()}");
            return new PictureBox[] { pb };
        }

        private PictureBox[] PlayByMultiple()
        {
            List<string> picLoactionInfos = new List<string>();
            int panelWidthX = this.panelXY.X;
            int panelHeightY = this.panelXY.Y;

            List<PictureBox> listPb = new List<PictureBox>();
            for (int i = 0; i < this.disPhotoCount; i++)
            {
                Random ra = new Random();
                
                Image photo = Image.FromFile(this.strFilePath[ra.Next(0, this.strFilePath.Count)]);
                int photoWidth = photo.Width;
                int photoHeight = photo.Height;

                int pointX = 0;
                int pointY = 0;

                PictureBox pb = new PictureBox();
                string picLocation;
                if (photoWidth >= panelWidthX || photoHeight >= panelHeightY)
                {
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.Size = new Size(400, 600);
                    pointX = ra.Next(0, panelWidthX - 400);
                    pointY = ra.Next(0, panelHeightY - 600);
                    picLocation = $"{pointX}|{pointY}|{400}|{600}";
                }
                else
                {
                    pb.SizeMode = PictureBoxSizeMode.AutoSize;
                    pointX = ra.Next(0, panelWidthX - photoWidth);
                    pointY = ra.Next(0, panelHeightY - photoHeight);
                    picLocation = $"{pointX}|{pointY}|{photoWidth}|{photoHeight}";
                }

                if (picLoactionInfos.Count > 0)
                {
                    if (!this.IsFullDispaly(picLoactionInfos, picLocation))
                    {
                        WritingOutput.ShowMessage($"坐标:[{pointX},{pointY}]  {this.strFilePath[i]} 与之前图片位置冲突");
                        continue;
                    }
                }

                WritingOutput.ShowMessage($"坐标:[{pointX},{pointY}]  {this.strFilePath[i]}");

                pb.Location = new Point(pointX, pointY);
                pb.Image = photo;

                listPb.Add(pb);
                picLoactionInfos.Add(picLocation);
            }

            return listPb.ToArray();
        }

        private PictureBox[] PlayByRandom()
        {
            int panelWidthX = this.panelXY.X;
            int panelHeightY = this.panelXY.Y;
            Random ra = new Random();

            string file = this.strFilePath[ra.Next(0, this.strFilePath.Count)];
            Image photo = Image.FromFile(file);
            int photoWidthX = photo.Width;
            int photoHeightY = photo.Height;

            PictureBox pb = new PictureBox();
            int widthX = 0;
            int heightY = 0;
            if (photoWidthX >= panelWidthX || photoHeightY >= panelHeightY)
            {
                WritingOutput.ShowMessage($"图片{file}分辨率超过窗口超限");
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

            return new PictureBox[] { pb };
        }

        private PictureBox[] PlayBySingleMove()
        {
            Image photo = Image.FromFile(this.strFilePath.First());

            int photoWidth = photo.Width;
            int photoHeight = photo.Height;

            int panelWidth = this.panelXY.X;
            int panelHeight = this.panelXY.Y;

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

            WritingOutput.ShowMessage($"坐标:[{pointX},{pointY}]  {this.strFilePath.First()}");
            return new PictureBox[] { pb };
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

        /// <summary>
        /// 判断图片落点是否在界面内
        /// </summary>
        /// <param name="pbLocationInfo">"pointX|pointY|panelWidth|panelHeight|imgWidth|imgHeight"</param>
        /// <returns>是否在界面内</returns>
        private bool IsImageInPanel(string pbLocationInfo)
        {
            bool isImageInPanel = false;

            string[] locationInfo = pbLocationInfo.Split('|');

            int pointX = int.Parse(locationInfo[0]);
            int pointY = int.Parse(locationInfo[1]);

            int panelWidth = int.Parse(locationInfo[2]);
            int panelHeight = int.Parse(locationInfo[3]);

            int imgWidth = int.Parse(locationInfo[4]);
            int imgHeight = int.Parse(locationInfo[5]);

            if (pointX + imgWidth <= panelWidth && pointY + imgHeight <= panelHeight)
            {
                isImageInPanel = true;
            }

            return isImageInPanel;
        }
    }
}
