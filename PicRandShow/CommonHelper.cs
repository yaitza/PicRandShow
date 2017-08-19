using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicRandShow
{
    static class CommonHelper
    {
        public static DisplayEnum ToDisplayEnum(string displayMode)
        {
            switch(displayMode.ToLower())
            {
                case "single":
                    return DisplayEnum.Single;
                case "multiple":
                    return DisplayEnum.Multiple;
                case "random":
                    return DisplayEnum.Random;
                default:
                    return DisplayEnum.NotExist;
            }
            
        }
    }
}
