using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicRandShow
{
    static class WritingOutput
    {
        public delegate void ShowMessageHandler(string msg);

        public static ShowMessageHandler ShowMethod { get; set; }

        public static void ShowMessage(string msg)
        {
            if (ShowMethod != null)
            {
                ShowMethod.Invoke(msg);
            }
        }
    }
}
