using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZippingEasy.Logic
{
    public static class Factory
    {
        public static IZipLogic GetZipLogic()
        {
            return new ZipLogic();
        }
    }
}
