using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class ListViewWithPath
    {
        private ListView view;
        private int x, y;
        private DirectoryInfo currentFolder;
        private FileComander fileComander;

        public ListViewWithPath(FileComander comander, int x, int y)
        {
            this.x = x;
            this.y = y;
            fileComander = comander;
            view = new ListView(x, y + 1, height: 19);
            Console.WindowHeight = 35;
            Console.WindowWidth = 120;
            view.ColumnsWidth = new List<int> { 30, 10, 10 };
            currentFolder = new DirectoryInfo("C:\\");
            view.CurrentState = "C:\\";
            view.Items = GetItems(currentFolder.FullName);
            view.Selected += View_Selected;
            view.TurnBack += View_TurnBack;
            view.ListOfDiscs += View_ListOfDiscs;
            view.NewFolder += View_NewFolder;
            view.Root += View_Root2;
            view.Properties += View_Properties;
            view.Rename += View_Rename;
            view.Copy += View_Copy;
            view.Cut += View_Cut;
            view.Paste += View_Paste;
            view.Find += View_Find;
        }

        private void View_Find(object sender, EventArgs e)
        {
            var listView = (ListView)sender;

            if (listView.Items.Count == 0)
                throw new InvalidOperationException("There is no items in this directory");
            string folderName = UserVoid();
            listView.FindElement(folderName);
            if (listView.Items.Count == 0)
                throw new InvalidOperationException("File not found(");

            Console.SetCursorPosition(1, 25);
            Console.WriteLine("                                                                                               ");
            listView.Clean();
            listView.Items = GetItems(view.CurrentState.ToString());
        }

        private void View_Copy(object sender, EventArgs e)
        {
            var listView = (ListView)sender;
            fileComander.Info = (FileSystemInfo)listView.SelectedItem.State;
        }

        private void CopyItems(string itemInfo, string copInfo)
        {
            if (fileComander.Info is FileInfo)
            {
                var copyinfo = new FileInfo(copInfo);
                var frominfo = fileComander.Info.FullName.ToString();
                File.Copy(frominfo, copInfo + "\\" + itemInfo);
            }
            else if (fileComander.Info is DirectoryInfo)
            {
                var from = new DirectoryInfo(itemInfo.ToString());
                var to = new DirectoryInfo(copInfo);
                to.Create();
                foreach (var file in from.GetFiles())
                    File.Copy(file.FullName, to.FullName + "\\" + file.Name);
                foreach (var dirInfo in from.GetDirectories())
                    CopyItems(dirInfo.FullName, to.FullName + "\\" + dirInfo.Name);
            }
        }

        private void View_Paste(object sender, EventArgs e)
        {
            var listView = (ListView)sender;
            string path = listView.CurrentState.ToString();
            CopyItems(fileComander.Info.FullName.ToString(), path);
            listView.Clean();
            listView.Items = GetItems(view.CurrentState.ToString());
        }

        private void View_Cut(object sender, EventArgs e)
        {
            var listView = (ListView)sender;
            string path = listView.CurrentState.ToString();
            if (fileComander.Info is FileInfo)
            {
                var copyinfo = new FileInfo(path);
                var frominfo = fileComander.Info.FullName.ToString();
                File.Move(frominfo, path + "\\" + fileComander.Info.Name.ToString());
            }
            else if (fileComander.Info is DirectoryInfo)
            {
                var from = new DirectoryInfo(fileComander.Info.FullName.ToString());
                var to = new DirectoryInfo(path);

                foreach (var file in from.GetFiles())
                    File.Move(file.FullName, to.FullName + "\\" + file.Name);
                foreach (var dirInfo in from.GetDirectories())
                    Directory.Move(dirInfo.FullName, to.FullName + "\\" + dirInfo.Name);
            }
            listView.Clean();
            listView.Items = GetItems(view.CurrentState.ToString());
        }


        private void View_Root2(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            var info = view.SelectedItem.State;
            if (info is FileInfo file)
            {
                view.CurrentState = file.Directory.Root;
            }
            else if (info is DirectoryInfo dir)
            {
                view.CurrentState = dir.Root;
            }
            view.Clean();
            view.Items = GetItems(view.CurrentState.ToString());
        }

        private void View_Rename(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            var info = view.SelectedItem.State;
            string folderName = UserVoid();
            if (info is FileInfo file)
            {
                string newFileFullPath = Path.Combine(file.DirectoryName, folderName);
                File.Move(file.FullName, newFileFullPath);
            }
            else if (info is DirectoryInfo dir)
            {
                string newDirFullPath = Path.Combine(dir.Parent.FullName, folderName);
                Directory.Move(dir.FullName, newDirFullPath);
            }

            Console.SetCursorPosition(1, 25);
            Console.WriteLine("                                                                                               ");
            view.Clean();
            view.Items = GetItems(view.CurrentState.ToString());
        }

        private void View_Properties(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            var info = view.SelectedItem.State;

            if (info is FileInfo)
            {
                FileProperty(info);
            }
            else
            {
                FolderProperty(info);
            }
        }

        private void FileProperty(object info)
        {
            var file = info as FileInfo;

            Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
            Console.WriteLine(String.Format("{0,-30}{1,-50}", "Name: ", file.Name).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Parent Directory: ", file.Directory).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Root Directory: ", file.DirectoryName).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Is read only: ", file.IsReadOnly).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Last Read Time: ", file.LastAccessTime).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Last Write Time: ", file.LastWriteTime).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Size: ", file.Length).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
            Console.ReadKey();
        }

        private void FolderProperty(object info)
        {
            var dir = info as DirectoryInfo;

            Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
            Console.WriteLine(String.Format("{0,-30}{1,-50}", "Name: ", dir.Name).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Root Directory: ", dir.FullName).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Parent Directory: ", dir.Parent.Name).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Last Read Time: ", dir.LastAccessTime).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Last Write Time: ", dir.LastWriteTime).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Size: ", dir.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length)).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Files: ", dir.GetFiles().Length).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine(String.Format("{0,-30}{1,-80}", "Folders: ", dir.GetDirectories().Length).PadRight(Console.WindowWidth - 1, ' '));
            Console.WriteLine("=".PadRight(Console.WindowWidth - 1, '='));
            Console.ReadKey();
        }

        private void View_Root(object sender, EventArgs e)
        {
            var listView = (ListView)sender;
            string path = listView.CurrentState.ToString();
            listView.Clean();
            if (path.Contains("C:\\"))
                view.CurrentState = "C:\\";
            else if (path.Contains("D:\\"))
                view.CurrentState = "D:\\";
            else if (path.Contains("E:\\"))
                view.CurrentState = "E:\\";
            else if (path.Contains("F:\\"))
                view.CurrentState = "F:\\";
            else if (path.Contains("G:\\"))
                view.CurrentState = "G:\\";
            listView.Items = GetItems(listView.CurrentState.ToString());
        }

        private void View_NewFolder(object sender, EventArgs e)
        {
            string folderName = UserVoid();
            var listView = (ListView)sender;

            string path = listView.CurrentState + "\\" + folderName;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
                dirInfo.Create();
            Console.SetCursorPosition(1, 25);
            Console.WriteLine("                                                                                                         ");
            listView.Clean();
            listView.Items = GetItems(listView.CurrentState.ToString());
        }

        public string UserVoid()
        {
            Lines.TableDraw(0, 26, 104, 1);
            Console.SetCursorPosition(1, 25);
            Console.WriteLine("Enter name:");
            Console.SetCursorPosition(12, 25);
            string userInput = Console.ReadLine();
            return userInput;

        }

        private static void View_ListOfDiscs(object sender, EventArgs e)
        {
            var listView = (ListView)sender;
            listView.Clean();
            listView.Items = GetDrives();
        }

        private static List<ListViewItem> GetDrives()
        {
            string[] drives = Directory.GetLogicalDrives();
            List<ListViewItem> result = new List<ListViewItem>();

            foreach (var drive in drives)
            {
                result.Add(new ListViewItem(new DirectoryInfo(drive), drive, "<drive>", ""));
            }
            return result;
        }

        private static List<ListViewItem> GetItems(string path)
        {
            return new DirectoryInfo(path).GetFileSystemInfos().Select(f =>
             new ListViewItem(
                 f,
                 f.Name,
                 f is DirectoryInfo dir ? "<dir>" : f.Extension,
                 f is FileInfo file ? ListViewItem.GetSizeofFile(file) : " ")).ToList();
        }

        internal void Render()
        {
            Console.CursorTop = y;
            Console.CursorLeft = x;
            Console.WriteLine(currentFolder.FullName.PadRight(view.ColumnsWidth.Sum()));
            view.Render();
        }

        internal void Update(ConsoleKeyInfo key)
        {
            view.Update(key);
        }

        private void View_Selected(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            var info = view.SelectedItem.State;

            if (info is FileInfo file)
                Process.Start(file.FullName);
            else if (info is DirectoryInfo dir)
            {
                try
                {
                    var items = GetItems(dir.FullName);
                    view.Clean();
                    view.Items = items;
                    currentFolder = dir;
                    view.CurrentState = dir.FullName;
                }
                catch
                {

                }
            }
        }

        private void View_TurnBack(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            string path = Path.GetDirectoryName(view.CurrentState.ToString());
            if (path == null)
                return;
            view.Clean();
            view.Items = GetItems(path);
            view.CurrentState = path;
        }

       
    }
}
