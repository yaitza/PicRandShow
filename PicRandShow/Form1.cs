using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace PicRandShow
{
    public partial class MainForm : Form
    {
        private string filePath;

        private int picCount;

        private int intervalTime;

        private DisplayEnum displayMode;

        private int displayTimes;

        public MainForm()
        {
            InitializeComponent();

            this.filePath = ConfigurationSettings.AppSettings["FilePath"];
            this.picCount = int.Parse(ConfigurationSettings.AppSettings["PicturesCount"]);
            this.intervalTime = int.Parse(ConfigurationSettings.AppSettings["IntervalTime"]);
            this.displayMode = CommonHelper.ToDisplayEnum(ConfigurationSettings.AppSettings["DisplayMode"]);
            this.displayTimes = int.Parse(ConfigurationSettings.AppSettings["DisplayTimes"]);

            WritingOutput.ShowMethod += this.OutputLabel;


            Thread th = new Thread(DisplayPhotos);
            th.Start();

        }

        public void DisplayPhotos()
        {
            var photoHandler = new FileAnalysis(this.filePath);
            var names = photoHandler.GetAllFile();

            if (names.Count == 0)
            {
                WritingOutput.ShowMethod($"{this.filePath}下无任何图片。");
                return;
            }

            for(int i=0; i<this.displayTimes; i++)
            {
                try
                {
                    DisplayHandler dh = new DisplayHandler(new Point(this.panel.Width, this.panel.Height), names, this.picCount, this.intervalTime);
                    PictureBox[] pbArray = dh.PhotoPlay(i, this.displayMode);
                    this.AddPictureBox(pbArray);
                    Thread.Sleep(1000 * this.intervalTime);
                    this.DeletePictureBox(pbArray);
                }catch(Exception ex)
                {
                    WritingOutput.ShowMethod(ex.Message);
                    Thread.Sleep(1000 * this.intervalTime);
                    continue;
                }
            }
            WritingOutput.ShowMethod("图片展示完毕。");
        }

        #region 参考
        //public void ShowPictures(int pics, int sec, string[] names)
        //{
        //    for (int i = 0; i < pics; i++)
        //    {
        //        Random rd = new Random();
        //        Thread th = new Thread(new ParameterizedThreadStart(PicPlay));
        //        th.Start(names[rd.Next(0, 450)]);
        //        Thread.Sleep(1000 * sec);
        //    }
        //}

        //public void PicPlay(object file)
        //{
        //    string fileName = (string)file;
        //    int pWidthX = this.panel.Width;
        //    int pheightY = this.panel.Height;
        //    Random rd = new Random();

        //    Image photo = Image.FromFile(fileName);
        //    int phWidthX = photo.Width;  //照片宽度像素值
        //    int phHeightY = photo.Height;//照片高度像素值

        //    PictureBox picbox = new PictureBox();
        //    picbox.SizeMode = PictureBoxSizeMode.AutoSize;

        //    picbox.Location = new Point(pWidthX / 2 - phWidthX / 2, pheightY / 2 - phHeightY / 2);
        //    picbox.Image = photo;
        //    picbox.BringToFront();
        //    this.AddPictureBox(new PictureBox[] { picbox });
        //}
        #endregion

        private delegate void PanelAddPictureBox(PictureBox[] pb);

        public void AddPictureBox(PictureBox[] pb)
        {
            if (this.panel.InvokeRequired)
            {
                PanelAddPictureBox papb = new PanelAddPictureBox(AddPictureBox);
                this.Invoke(papb, new object[] { pb });
            }
            else
            {
                this.panel.Controls.AddRange(pb);
                this.panel.Refresh();
            }
        }

        private delegate void PanelDeletePictureBox(PictureBox[] pb);

        public void DeletePictureBox(PictureBox[] pb)
        {
            if (this.panel.InvokeRequired)
            {
                PanelDeletePictureBox pdpb = new PanelDeletePictureBox(DeletePictureBox);
                this.Invoke(pdpb, new object[] { pb });
            }
            else
            {
                foreach(PictureBox pictureBox in pb)
                {
                    this.panel.Controls.Remove(pictureBox);
                    pictureBox.Image.Dispose();
                    pictureBox.Dispose();
                }
                this.panel.Refresh();
            }
        }

        private delegate void DisplayMessage(string msg);


        private void OutputLabel(string msg)
        {
            if (this.labelOutput.InvokeRequired)
            {
                DisplayMessage dm = new DisplayMessage(OutputLabel);
                this.Invoke(dm, new object[] { msg});
            }
            else
            {
                this.labelOutput.Text = $"{msg} {Environment.NewLine}";
                this.labelOutput.Focus();
            }
        }
    }
}
