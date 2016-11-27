using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZip;
using ZippingEasy.Common.Utils;
using log4net;

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

            // set settings for zipping
            StringBuilder libraryPathBuilder = new StringBuilder();
            libraryPathBuilder.Append(AppDomain.CurrentDomain.BaseDirectory);
            if (Environment.Is64BitOperatingSystem)
            {
                libraryPathBuilder.Append("x64\\7z.dll");
            }
            else
            {
                libraryPathBuilder.Append("x86\\7z.dll");
            }

            if (!File.Exists(libraryPathBuilder.ToString()))
            {
                LogManager.GetLogger(typeof(ZipLogic)).Error("7z.dll was not found");
                return new Result("7z.dll was not found", ResultStatus.Error);
            }
            SevenZipCompressor.SetLibraryPath(libraryPathBuilder.ToString());
            SevenZipCompressor compressor = SetCompressionSettings(compressionLevel);

            // zipping            
            IList<string> subfolders = this.GetSubfolders(sourcePath).ToList();
            foreach(var srcFolder in subfolders)
            {
                string sourceFolderName = srcFolder.Split('\\').Last();
                string destFolder = $"{destinationPath}\\{sourceFolderName}.zip";
                compressor.CompressDirectory(srcFolder, destFolder);
            }
            //File.Move($"{sourcePath}\\{sourceFolderName}.zip", $"{destinationPath}\\{sourceFolderName}.zip");

            return null;
        }

        private SevenZipCompressor SetCompressionSettings(CompressionLevel compressionLevel)
        {
            return new SevenZipCompressor()
            {                
                CompressionLevel = compressionLevel,
                CompressionMethod = CompressionMethod.Default,
                CompressionMode = CompressionMode.Create
            };
        }

        public IEnumerable<string> GetSubfolders(string sourcePath)
        {
            try
            {
                if (!Directory.Exists(sourcePath))
                    return new List<string>();

                IEnumerable<string> subfolders = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)
                                                          .Where(f => !Directory.EnumerateDirectories(f).Any());
                return subfolders;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(ZipLogic)).Warn($"Error on getting subfolders:\n{ex.Message}");
                return new List<string>();
            }
        }
    }
}
