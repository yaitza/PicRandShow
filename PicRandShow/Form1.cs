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

namespace PicRandShow
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            this.test();
        }

        public void test()
        {
            String str = ConfigurationSettings.AppSettings["filePath"];
            var t = new FileAnalysis(str);
            var names = t.GetAllFile();
            //this.pictureBox.Image = new System.Drawing.Bitmap(names[300]);

            //foreach (string file in names)
            //{
            //    this.pictureBox.Image =new  System.Drawing.Bitmap(file);
            //    Thread.Sleep(1000);
            //}
            ShowPictures(5, 1, names.ToArray());
        }

        public void ShowPictures(int pics, int sec, string[] names)
        {
            //while (true)
            {
                for (int i = 0; i < pics; i++)
                {
                    Random rd = new Random();
                    string fileName = names[rd.Next(0, 400)];
                    int width = this.Width;
                    int height = this.Height;


                    PictureBox pic = new PictureBox();
                    pic.Size = new Size(100, 50);
                    pic.SizeMode = PictureBoxSizeMode.AutoSize;
                    pic.Location = new Point(rd.Next(0, height), rd.Next(0, width));
                    Bitmap bm = new Bitmap(fileName);
                    pic.Image = bm;
                    this.Controls.Add(pic);
                    //Thread th = new Thread(new ParameterizedThreadStart(PicPlay));
                    //th.Start(names[i]);
                    this.Refresh();

                }

                Thread.Sleep(1000 * 5);
                //this.Controls.Clear();
                this.Refresh();
                Thread.Sleep(1000 * 3);
            }

        }

        public void PicPlay(object file)
        {
            string fileName = (string)file;
            int width = this.Width;
            int height = this.Height;
            Random rd = new Random();


            PictureBox pic = new PictureBox();
            pic.Size = new Size(50, 50);
            pic.Location = new Point(rd.Next(0, height), rd.Next(0, width));
            Bitmap bm = new Bitmap(fileName);
            pic.Image = bm;
            this.Controls.Add(pic);

        }
    }
}
