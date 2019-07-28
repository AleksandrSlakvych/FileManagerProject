using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
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
                Console.WriteLine(ListViewFunc.GetStringWithLenght(columns[i], columnsWidth[i]));

            }
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
