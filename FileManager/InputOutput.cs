using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    public class Lines
    {
        public static char vertical = '|';
        public static char horizontal = '-';
        public static string topL = "|-";
        public static string topR = "-|";
        public static string botL = "|-";
        public static string botR = "-|";

        // отрисовка рамки

        public static void TableDraw(int x, int y, int width, int height, ConsoleColor color = ConsoleColor.DarkYellow)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(topL);
            Console.Write(new string(horizontal, width - 4));
            Console.Write(topR);
            for (int i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(vertical);
                Console.SetCursorPosition(x + width - 1, y + i);
                Console.Write(vertical);
            }
            Console.SetCursorPosition(x, y + height - 1);
            Console.Write(botL);
            Console.Write(new string(horizontal, width - 4));
            Console.Write(botR);
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
        }
    }
}
