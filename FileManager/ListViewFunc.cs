using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class ListViewFunc
    {
        // возвращает имя файла в формате с обрезанием

        public static string GetStringWithLenght(string v1, int maxLenght)
        {
            if (v1.Length < maxLenght)
                return v1.PadRight(maxLenght, ' ');

            else
                return v1.Substring(0, maxLenght - 5) + "[...]";

        }

        // возвращает строку с папкой

        public static string GetDirectoryWithLenght(string directoryName, int length)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryName);
            return GetStringWithLenght(directory.Name, length - 20) + "  " + GetStringWithLenght("<dir>", 8) + new string(' ', 10);
        }

        // возвращает размер файла/папки в формате

        public static string GetSizeofFile(FileInfo fileLength)
        {
            if (fileLength.Length < 10240) return fileLength.ToString() + " Byte";
            else if (fileLength.Length < 1024 * 1024 * 10) return (fileLength.Length / 1024).ToString() + " KB";
            else if (fileLength.Length < (Int64)1024 * 1024 * 1024 * 10) return (fileLength.Length / 1024 / 1024).ToString() + " MB";
            else if (fileLength.Length < (Int64)1024 * 1024 * 1024 * 1024 * 10) return (fileLength.Length / 1024 / 1024 / 1024).ToString() + " GB";
            else return (fileLength.Length / 1024 / 1024 / 1024 / 1024).ToString() + " TB";
        }

        // возвращает строку с файлом

        public static string GetFileWithLenght(string fileName, int length)
        {
            FileInfo file = new FileInfo(fileName);

            string strFullFileInfo = GetStringWithLenght(file.Name, length - 20) + "  ";
            if (file.Extension.Length > 1)
                strFullFileInfo += GetStringWithLenght(file.Extension.Substring(1), 8);
            else
                strFullFileInfo += GetStringWithLenght(" ", 8);
            return strFullFileInfo;
        }

        // находит файлы по запросу (рекурсивный метод)

        public static List<FileInfo> FindListOfFiles(DirectoryInfo directory, string request, List<FileInfo> finding = null)
        {
            if (finding == null)
                finding = new List<FileInfo>();

            request = request.ToLower();
            FileInfo[] files = null;
            files = directory.GetFiles();

            if (files != null)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.ToLower().IndexOf(request) != -1)
                        finding.Add(files[i]);
                }
            }

            DirectoryInfo[] directories = null;
            directories = directory.GetDirectories();

            if (directories != null)
            {
                for (int i = 0; i < directories.Length; i++)
                    FindListOfFiles(directories[i], request, finding);
            }
            return finding;
        }

        // находит папки по запросу (рекурсивный метод)

        public static List<DirectoryInfo> FindListOfDirectories(DirectoryInfo directory, string request, List<DirectoryInfo> finding = null)
        {
            if (finding == null)
                finding = new List<DirectoryInfo>();

            request = request.ToLower();
            DirectoryInfo[] directories = null;
            directories = directory.GetDirectories();

            if (directories != null)
            {
                for (int i = 0; i < directories.Length; i++)
                {
                    if (directories[i].Name.ToLower().IndexOf(request) != -1)
                        finding.Add(directories[i]);

                    FindListOfDirectories(directories[i], request, finding);
                }
            }
            return finding;
        }
    }
}
