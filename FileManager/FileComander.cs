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
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    currentIndex = 1;
                }

                pathedListView[currentIndex].Update(key);

                for (int i = 0; i < pathedListView.Length; i++)
                {
                    Lines.TableDraw(0 + 52 * i, 1, 52, 22);
                    pathedListView[i].Render();
                }

                Lines.TableDraw(0, 24, 104, 1);
                Console.SetCursorPosition(1, 23);
                Console.WriteLine(" COPY-F1| CUT-F2|PASTE-F3| ROOT-F4| LIST OF DISCS-F5| PROPERTIES-F6| RENAME-F7| FIND-F8|NEW FOLDER-F9 ");
            }
        }
    }
}
