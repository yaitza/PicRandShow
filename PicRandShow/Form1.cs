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
            PictureBoxOperator.AddPictureBoxMethod += this.AddPictureBox;
            PictureBoxOperator.DeletePictureBoxMethod += this.DeletePictureBox;
            PictureBoxOperator.MoveLocationMethod += this.MoveLocation;

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
            DisplayMode dm;
            switch (displayMode)
            {

                case DisplayEnum.Single:
                    dm = new DisplayBySingle(this.panel.Size, names, this.displayTimes, this.picCount, this.intervalTime);
                    break;
                case DisplayEnum.Sequence:
                    dm = new DisplayBySequence(this.panel.Size, names, this.displayTimes, this.picCount, this.intervalTime);
                    break;
                case DisplayEnum.Random:
                    dm = new DisplayByRandom(this.panel.Size, names, this.displayTimes, this.picCount, this.intervalTime);
                    break;
                case DisplayEnum.Multiple:
                    dm = new DisplayByMultiple(this.panel.Size, names, this.displayTimes, this.picCount, this.intervalTime);
                    break;
                case DisplayEnum.SingleMove:
                    dm = new DisplayBySingleMove(this.panel.Size, names, this.displayTimes, this.picCount, this.intervalTime);
                    break;
                default:
                    dm = new DisplayBySingle(this.panel.Size, names, this.displayTimes, this.picCount, this.intervalTime);
                    break;
            }
            try
            {
                dm.Play();
            }
            catch (Exception ex)
            {
                WritingOutput.ShowMethod(ex.Message);
            }

            WritingOutput.ShowMethod("图片展示完毕。");
        }

        private delegate void PictureBoxMoveLocation(Point point, PictureBox pb);

        public void MoveLocation(Point point, PictureBox pb)
        {
            int iFlags = this.panel.Controls.IndexOf(pb);

            if (this.panel.Controls[iFlags].InvokeRequired)
            {
                PictureBoxMoveLocation pbml = new PictureBoxMoveLocation(MoveLocation);
                this.Invoke(pbml, new object[] { point, pb });
            }
            else
            {
                this.panel.Controls[iFlags].Location = point;
            }
        }


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
                foreach (PictureBox pictureBox in pb)
                {
                    this.panel.Controls.Remove(pictureBox);
                    pictureBox.Image.Dispose();
                    pictureBox.Dispose();
                }
                pb = null;
            }
            GC.Collect();
        }

        private delegate void DisplayMessage(string msg);

        private void OutputLabel(string msg)
        {
            if (this.labelOutput.InvokeRequired)
            {
                DisplayMessage dm = new DisplayMessage(OutputLabel);
                this.Invoke(dm, new object[] { msg });
            }
            else
            {
                this.labelOutput.Text = $"{msg} {Environment.NewLine}";
                this.labelOutput.Focus();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.panel.Size = this.Size;

        }
    }
}
