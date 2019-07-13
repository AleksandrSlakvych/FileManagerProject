using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    // инф-я о папке , перегрузка оператора + для суммирования размера, кол-ва папок и файлов

    public class DataOfDirectory
    {
        public Int64 size;
        public int countOfFiles;
        public int countOfDirect;

        public DataOfDirectory()
        {
            size = 0;
            countOfFiles = 0;
            countOfDirect = 0;
        }

        public static DataOfDirectory operator +(DataOfDirectory first, DataOfDirectory second)
        {
            DataOfDirectory resultDirectory = new DataOfDirectory();
            resultDirectory.size = first.size + second.size;
            resultDirectory.countOfDirect = first.countOfDirect + second.countOfDirect;
            resultDirectory.countOfFiles = first.countOfFiles + second.countOfFiles;
            return resultDirectory;
        }
    }

    class ListViewItem
    {
        private readonly string[] columns;
        public object State { get; set; }

        public ListViewItem(object state, params string[] columns)
        {
            State = state;
            this.columns = columns;
        }

        internal void Render(List<int> columnsWidth, int elementIndex, int listViewX, int listViewY)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;

                Console.CursorTop = elementIndex + listViewY;
                Console.CursorLeft = listViewX + columnsWidth.Take(i).Sum();
                Console.WriteLine(GetStringWithLenght(columns[i], columnsWidth[i]));

            }
        }

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
            //strFullFileInfo += GetSizeofFile(file.Length);
            return strFullFileInfo;
        }

        // с помощью перегрузки оператора+ возвращает инфо о папке (ко-во папок,файлов и размер)  

        public static DataOfDirectory GetDataOfDirectory(DirectoryInfo directory)
        {
            DataOfDirectory directoryInfo = new DataOfDirectory();
            DirectoryInfo[] directories = null;
            directories = directory.GetDirectories();

            if (directories != null)
            {
                directoryInfo.countOfDirect = directories.Length;
                for (int i = 0; i < directories.Length; i++)
                    directoryInfo += GetDataOfDirectory(directories[i]);
            }

            FileInfo[] files = null;
            files = directory.GetFiles();

            if (files != null)
            {
                directoryInfo.countOfFiles = files.Length;
                for (int i = 0; i < files.Length; i++)
                    directoryInfo.size += files[i].Length;
            }
            return directoryInfo;
        }

        // находит файлы по запросу (рекурсивный метод)

        public static List<object> FindListOfFiles(DirectoryInfo directory, string request, List<object> finding = null)
        {
            if (finding == null)
                finding = new List<object>();

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

        public static List<object> FindListOfDirectories(DirectoryInfo directory, string request, List<object> finding = null)
        {
            if (finding == null)
                finding = new List<object>();

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

        // очистка колонок

        public void Clean(List<int> columnsWidth, int i, int x, int y)
        {
            Console.CursorTop = i + y;
            Console.CursorLeft = x;
            Console.Write(new string(' ', columnsWidth.Sum()));
        }
    }
}
