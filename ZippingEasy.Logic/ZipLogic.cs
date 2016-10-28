using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZip;
using ZippingEasy.Common.Utils;

namespace ZippingEasy.Logic
{
    public class ZipLogic : IZipLogic
    {
        public ZipLogic()
        {

        }

        public async Task<Result> Zip(string sourcePath, string destinationPath, CompressionLevel compressionLevel)
        {
            if (!Directory.Exists(sourcePath))
            {
                return new Result("directory does not exist", ResultStatus.Warning);
            }

            SevenZipCompressor compressor = new SevenZipCompressor();
            compressor.CompressionLevel = compressionLevel;
            compressor.CompressionMethod = CompressionMethod.Default;
            compressor.CompressionMode = CompressionMode.Create;

            string[] sourceFolderName = sourcePath.Split('\\');

            compressor.CompressDirectory(sourcePath, $"{sourceFolderName}.zip");

            File.Move($"{sourcePath}\\{sourceFolderName}.zip", $"{destinationPath}\\{sourceFolderName}.zip");

            return null;
        }

        public IEnumerable<string> GetSubfolders(string sourcePath)
        {
            IEnumerable<string> subfolders = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)
                                                      .Where(f => !Directory.EnumerateDirectories(f).Any());
            return subfolders;
        }
    }
}
