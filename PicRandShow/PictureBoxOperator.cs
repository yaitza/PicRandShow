using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicRandShow
{
    static class PictureBoxOperator
    {
        public delegate void AddPictureBoxHandler(PictureBox[] pb);

        public static AddPictureBoxHandler AddPictureBoxMethod { get; set; }

        public static void AddPictureBox(PictureBox[] pb)
        {
            if(AddPictureBoxMethod != null)
            {
                AddPictureBoxMethod.Invoke(pb);
            }
        }

        public delegate void DeletePictureBoxHandler(PictureBox[] pb);

        public static DeletePictureBoxHandler DeletePictureBoxMethod { get; set; }

        public static void DeletePictureBox(PictureBox[] pb)
        {
            if(DeletePictureBoxMethod != null)
            {
                DeletePictureBoxMethod.Invoke(pb);
            }
        }

        public delegate void MoveLocationHandler(Point point, PictureBox pb);

        public static MoveLocationHandler MoveLocationMethod { get; set; }

        public static void MoveLocation(Point point, PictureBox pb)
        {
            if (MoveLocationMethod != null)
            {
                MoveLocationMethod.Invoke(point, pb);
            }
        }
    }
}
