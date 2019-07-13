using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{

    public enum SideOfConsole
    {
        LeftSide, RightSide, Central
    }

    public class Lines
    {
        public static char vertical = '|';
        public static char horizontal = '-';
        public static string topL = "|-";
        public static string topR = "-|";
        public static string botL = "|-";
        public static string botR = "-|";

        // отрисовка горизонтальной линии

        public static void HorizontLineDraw(int x, int y, int length, ConsoleColor color = ConsoleColor.DarkYellow)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(new string(horizontal, length));
        }

        // отрисовка вертикальной линии

        public static void VerticalLineDraw(int x, int y, int length, ConsoleColor color = ConsoleColor.DarkYellow)
        {
            Console.ForegroundColor = color;
            for (int i = 0; i < length; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(vertical);
            }
        }

        // отрисовка рамки

        public static void TableDraw(int x, int y, int width, int height, ConsoleColor color = ConsoleColor.DarkYellow)
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
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

    class InputOutput
    {
        // отрисовка формы для заполнения

        public static void IOFormDraw(int x, int y, int width, int height, ConsoleColor color = ConsoleColor.Gray)
        {
            string s = new string(' ', width);
            Console.BackgroundColor = color;
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(s);
            }
        }
    }

    class AppElement
    {
        private int x, y, width, height;
        private string text, resultstring;
        private ConsoleColor textColor;
        private ConsoleColor backgroundColor;
        private bool wasDrawn = false, isActive = false;

        // создание строкового элемента

        public AppElement(int x, int y, int w, int h, string txt, SideOfConsole style = SideOfConsole.Central, ConsoleColor txtColor = ConsoleColor.Red, ConsoleColor bgColor = ConsoleColor.DarkGreen)
        {
            this.x = x;
            this.y = y;
            width = w;
            height = h;
            backgroundColor = bgColor;
            textColor = txtColor;
            text = txt;
            if (style == SideOfConsole.Central)
            {
                resultstring = new string(' ', (width - text.Length) / 2);
                resultstring = resultstring + text + resultstring;
            }
            else if (style == SideOfConsole.RightSide)
            {
                resultstring = new string(' ', width - text.Length);
                resultstring += text;
            }
            else
            {
                resultstring = new string(' ', width - text.Length);
                resultstring = text + resultstring;
            }
        }

        // активация элемента

        public void ElementActivate()
        {
            wasDrawn = true;
            isActive = true;
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = backgroundColor;
            Console.BackgroundColor = textColor;
            Console.Write(resultstring);
        }

        // деактивация элемента

        public void ElementDeactivate()
        {
            wasDrawn = true;
            isActive = false;
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(resultstring);
        }

        // изменение цвета элемента

        public void SelectorOfColor(ConsoleColor color)
        {
            textColor = color;
            if (wasDrawn && isActive)
                ElementActivate();
            else if (wasDrawn)
                ElementDeactivate();
        }

        // создание пустого элемента

        public void EmptyTextElement()
        {
            if (wasDrawn)
            {
                Console.BackgroundColor = backgroundColor;
                Console.SetCursorPosition(x, y);
                Console.Write(new string(' ', resultstring.Length));
            }
        }
    }


    
}
