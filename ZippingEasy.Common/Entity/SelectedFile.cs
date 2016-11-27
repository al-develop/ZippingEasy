using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZippingEasy.Common.Entity
{
    public class SelectedFile
    {
        public int ID { get; set; }
        public string FilePath { get; set; }
        public int ZipProgress { get; set; }
    }
}
