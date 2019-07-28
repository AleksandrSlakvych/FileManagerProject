using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class FileComander
    {
        public FileSystemInfo Info { get; set; }

        public void Start()
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            ListViewWithPath[] pathedListView = new ListViewWithPath[2];

            for (int i = 0; i < pathedListView.Length; i++)
            {
                pathedListView[i] = new ListViewWithPath(this, 1 + 52 * i, 2);
            }

            int currentIndex = 0;

            while (true)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.LeftArrow)
                {
                    currentIndex = 0;
                    pathedListView[0].Active();
                    pathedListView[1].DeActive();
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    currentIndex = 1;
                    pathedListView[0].DeActive();
                    pathedListView[1].Active();
                }

                pathedListView[currentIndex].Update(key);

                for (int i = 0; i < pathedListView.Length; i++)
                {
                    pathedListView[i].Render();
                }

                Lines.TableDraw(0, 24, 104, 1);
                Console.SetCursorPosition(1, 23);
                Console.WriteLine(" COPY-F1| CUT-F2|PASTE-F3| ROOT-F4| LIST OF DISCS-F5| PROPERTIES-F6| RENAME-F7| FIND-F8|NEW FOLDER-F9 ");
            }
        }
    }
}
