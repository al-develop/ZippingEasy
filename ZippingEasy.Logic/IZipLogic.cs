using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZip;
using ZippingEasy.Common.Utils;

namespace ZippingEasy.Logic
{
    public interface IZipLogic
    {
        Task<Result> Zip(string sourcePath, string destinationPath, CompressionLevel compressionLevel);

        IEnumerable<string> GetSubfolders(string sourcePath);
    }
}
