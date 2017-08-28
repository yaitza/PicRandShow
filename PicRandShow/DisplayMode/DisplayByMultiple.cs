using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace PicRandShow
{
    public class DisplayByMultiple : DisplayMode
    {
        public DisplayByMultiple(Size panelSize, List<string> strFileNames, int displayTimes = 1, int disImgCounts = 1, int intervalTime = 2) : base(panelSize, strFileNames, displayTimes, disImgCounts, intervalTime)
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
            List<string> picLoactionInfos = new List<string>();
            int panelWidthX = this.PanelSize.Width;
            int panelHeightY = this.PanelSize.Height;

            List<PictureBox> listPb = new List<PictureBox>();
            for (int i = 0; i < this.DisImgCounts; i++)
            {
                Random ra = new Random();

                Image photo = Image.FromFile(this.StrFilePaths[ra.Next(0, this.StrFilePaths.Count)]);
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
                        WritingOutput.ShowMessage($"坐标:[{pointX},{pointY}]  {this.StrFilePaths[i]} 与之前图片位置冲突");
                        photo.Dispose();
                        photo = null;
                        pb.Dispose();
                        pb = null;
                        continue;
                    }
                }

                WritingOutput.ShowMessage($"坐标:[{pointX},{pointY}]  {this.StrFilePaths[i]}");

                pb.Location = new Point(pointX, pointY);
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
    }
}