using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class ListView
    {
        private int selectedIndex;
        private int prevSelectedIndex;
        private bool wasPainted;
        private int x, y, height, scroll;
        public List<ListViewItem> newItems { get; set; }
        public List<ListViewItem> Items { get { return newItems; } set { scroll = 0; newItems = value; } }
        public List<int> ColumnsWidth { get; set; }
        public ListViewItem SelectedItem => Items[selectedIndex];
        public object CurrentState { get; internal set; }
        public bool Focused { get; set; }
        public bool isCut = false;

        public ListView(int x, int y, int height)
        {
            this.height = height;
            this.x = x;
            this.y = y;
        }

        public void FindElement(string nameoffile)
        {
            try
            {
                string[] allFoundFiles = Directory.GetFiles(CurrentState.ToString(), nameoffile, SearchOption.AllDirectories);
                Clean();
                Items.Clear();
                for (int i = 0; i < allFoundFiles.Length; i++)
                {
                    object dc = new object();
                    if (nameoffile.Contains("."))
                        Items.Add(new ListViewItem(dc, dc.ToString()));
                }
            }
            catch
            {
                throw new InvalidOperationException("This directory have folders without access");
            }
        }

        public void Clean()
        {
            selectedIndex = prevSelectedIndex = 0;
            wasPainted = false;
            for (int i = 0; i < Math.Min(height, Items.Count); i++)
            {
                Console.CursorLeft = x;
                Console.CursorTop = i + y;
                Items[i].Clean(ColumnsWidth, i, x, y);
            }
        }

        public void Render()
        {
            for (int i = 0; i < Math.Min(height, Items.Count); i++)
            {
                int elementIndex = i + scroll;

                if (wasPainted)
                {
                    if (elementIndex != selectedIndex && elementIndex != prevSelectedIndex)
                        continue;
                }

                var item = Items[elementIndex];
                var saveForeground = Console.ForegroundColor;
                var saveBackground = Console.BackgroundColor;
                if (elementIndex == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;

                }
                Console.CursorLeft = x;
                Console.CursorTop = i + y;
                item.Render(ColumnsWidth, i, x, y);
                Console.ForegroundColor = saveForeground;
                Console.BackgroundColor = saveBackground;
            }
            wasPainted = true;
        }

        public void Update(ConsoleKeyInfo key)
        {
            prevSelectedIndex = selectedIndex;
            if (key.Key == ConsoleKey.DownArrow && selectedIndex + 1 < Items.Count)
                selectedIndex++;
            else if (key.Key == ConsoleKey.UpArrow && selectedIndex - 1 >= 0)
                selectedIndex--;

            if (selectedIndex >= height + scroll)
            {
                scroll++;
                wasPainted = false;
            }
            else if (selectedIndex < scroll)
            {
                scroll--;
                wasPainted = false;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                TurnBack(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                Selected(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.F1)
            {
                Copy(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.F2)
            {
                isCut = true;
                Copy(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.F3)
            {
                if (!isCut)
                    Paste(this, EventArgs.Empty);
                else
                    Cut(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.F4)
            {
                Root(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.F5)
            {
                ListOfDiscs(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.F6)
            {
                Properties(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.F7)
            {
                Rename(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.F8)
            {
                Find(this, EventArgs.Empty);
            }
            else if (key.Key == ConsoleKey.F9)
            {
                NewFolder(this, EventArgs.Empty);
            }
        }

        public event EventHandler Selected;
        public event EventHandler TurnBack;
        public event EventHandler ListOfDiscs;
        public event EventHandler NewFolder;
        public event EventHandler Root;
        public event EventHandler Properties;
        public event EventHandler Copy;
        public event EventHandler Cut;
        public event EventHandler Paste;
        public event EventHandler Rename;
        public event EventHandler Find;
    }
}
