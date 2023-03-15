using System;
using System.IO;

namespace SSInventory.Share.Ultilities
{
    public static class FileHelper
    {
        public static string GetValidFileName(string uploadDirectory, string oldFileName, int maxFileLength)
        {
            var fileName = RemoveInvalidChars(oldFileName);
            if (fileName.Length > maxFileLength)
            {
                fileName = TrimFileName(fileName, maxFileLength);
            }
            if (File.Exists(Path.Combine(uploadDirectory, fileName)))
            {
                var number = 1;
                var fileInfo = new FileInfo(fileName);
                while (File.Exists(Path.Combine(uploadDirectory, fileName)))
                {
                    fileName = $"{Path.GetFileNameWithoutExtension(fileName)}_({number}){fileInfo.Extension}";
                    number++;
                    fileName = TrimFileName(fileName, maxFileLength);

                    // failed after retry to get valid file name 100 times, simply use NewGuid
                    if (number == 100)
                    {
                        fileName = $"{Guid.NewGuid()}_{Guid.NewGuid()}{fileInfo.Extension}";
                        break;
                    }
                }
            }

            return fileName;
        }

        public static string TrimFileName(string fileName, int expectedLength)
        {
            if (fileName.Length > expectedLength)
            {
                var fileInfo = new FileInfo(fileName);
                var fileNameWithoutExtionsion = Path.GetFileNameWithoutExtension(fileName);
                fileName = fileNameWithoutExtionsion.Substring(0, 20) + " ..." + fileNameWithoutExtionsion.Substring(fileNameWithoutExtionsion.Length - 20, 20) + fileInfo.Extension;
            }
            return fileName;
        }

        private static string RemoveInvalidChars(string fileName)
        {
            fileName = fileName.Replace(" ", "_");
            fileName = fileName.Replace("%", "_");
            fileName = fileName.Replace("'", "");
            fileName = fileName.Replace("Ã¦", "æ");
            fileName = fileName.Replace("Ã¸", "ø");
            fileName = fileName.Replace("Ã¥", "å");
            fileName = fileName.Replace("Ã¼", "ü");
            return fileName;
        }
    }
}
