using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZippingEasy.Common.Utils;
using log4net;
using System.IO.Compression;

namespace ZippingEasy.Logic
{
    public class ZipLogic : IZipLogic
    {
        public ZipLogic()
        {
        }

        public async Task<Result> Zip(string sourcePath, string destinationPath, CompressionLevel compressionLevel)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(sourcePath))
                    {
                        return new Result("directory does not exist", ResultStatus.Warning);
                    }

                    // zipping uses System.IO.Compression.FileSystem.ZipFile
                    IList<string> subfolders = this.GetSubfolders(sourcePath).ToList();
                    foreach (var srcFolder in subfolders)
                    {
                        string sourceFolderName = srcFolder.Split('\\').Last();
                        string destFolder = $"{destinationPath}\\{sourceFolderName}.zip";
                        ZipFile.CreateFromDirectory(srcFolder, destFolder, compressionLevel, false);
                    }

                    return new Result("zipping successfull", ResultStatus.Success);
                }
                catch (Exception ex)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    errorMessage.AppendLine("Error occured while zipping progress");
                    errorMessage.AppendLine(ex.Message);
                    LogManager.GetLogger(typeof(ZipLogic)).Error(errorMessage.ToString());
                    return new Result(errorMessage.ToString(), ResultStatus.Error);
                }
            });

            return new Result("zipping successfull", ResultStatus.Success);
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
