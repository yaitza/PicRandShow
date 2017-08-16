using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Forms.VisualStyles;

namespace PicRandShow
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Thread th = new Thread(test);
            th.Start();

        }

        public void test()
        {
#pragma warning disable 618
            String str = ConfigurationSettings.AppSettings["FilePath"];
            int picCount = int.Parse(ConfigurationSettings.AppSettings["PicturesCount"]);
            int switchTime = int.Parse(ConfigurationSettings.AppSettings["SwitchTime"]);
#pragma warning restore 618
            var t = new FileAnalysis(str);
            var names = t.GetAllFile();

            ShowPictures(picCount, switchTime, names.ToArray());
        }

        public void ShowPictures(int pics, int sec, string[] names)
        {
            for (int i = 0; i < pics; i++)
            {
                Random rd = new Random();
                Thread th = new Thread(new ParameterizedThreadStart(PicPlay));
                th.Start(names[rd.Next(0, 450)]);
                Thread.Sleep(1000 * sec);

            }
        }

        public void PicPlay(object file)
        {
            string fileName = (string)file;
            int width = this.Width;
            int height = this.Height;
            Random rd = new Random();

            PictureBox pic = new PictureBox();
            pic.SizeMode = PictureBoxSizeMode.AutoSize;
            pic.Location = new Point(rd.Next(0, height), rd.Next(0, width));
            Bitmap bm = new Bitmap(fileName);
            pic.Image = bm;
            pic.BringToFront();
            this.AddPictureBox(pic);

        }

        private delegate void PanelAddPictureBox(PictureBox pb);

        public void AddPictureBox(PictureBox pb)
        {
            if (this.panel.InvokeRequired)
            {
                PanelAddPictureBox papb = new PanelAddPictureBox(AddPictureBox);
                this.Invoke(papb, new object[] { pb });
            }
            else
            {
                this.panel.Controls.Add(pb);
                this.panel.Refresh();
            }
        }
    }
}
