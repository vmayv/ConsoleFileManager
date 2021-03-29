using System;
using System.IO;
using System.Linq;

namespace ConsoleFileManager
{
    partial class Program
    {
        static int currentPage;
        static int pagesCount;

        static void WriteDirectories(string path, int columnWidth, int depth)
        {
            WriteDirectories(path, columnWidth, 0, depth, 1, 2);
        }
        static void WriteDirectories(string path, int columnWidth, int depthStart, int depthEnd, int column, int line)
        {
            try
            {
                if (depthStart == depthEnd)
                {
                    return;
                }
                if (depthStart == 0)
                {
                    Console.SetCursorPosition(column, line++);
                    Console.WriteLine($"[..]");
                }
                var separator = "├";
                var directories = Directory.GetDirectories(path);
                foreach (var dir in directories)
                {
                    var dirname = separator + Path.GetFileName(dir);
                    dirname = dirname.Length > columnWidth - 1 ? dirname.Substring(0, columnWidth - 1) : dirname;
                    Console.SetCursorPosition(column, line++);
                    Console.WriteLine(dirname.PadRight(columnWidth - 1 - column));
                    WriteDirectories(dir, columnWidth, depthStart + 1, depthEnd, column + 1, line);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }
        }

        private static void WriteFiles(string path, int columnWidth, int countElementsOnPage)
        {
            try
            {
                var files = Directory.GetFiles(path);
                pagesCount = files.Length / countElementsOnPage;
                var filesOnPage = files.Skip(currentPage * countElementsOnPage).Take(countElementsOnPage);
                var startRow = headerHeight;
                var processedRows = 0;

                foreach (var file in filesOnPage)
                {
                    if (processedRows == countElementsOnPage)
                    {
                        return;
                    }
                    var info = new FileInfo(file);
                    //TODO: 25 ??
                    var name = info.Name.Length > 25 ? info.Name.Substring(0, 25) : info.Name;
                    var line = FormatColumns(
                        GetScreenWidth() / 2,
                        name,
                        $"{BytesToString(info.Length)} {info.LastWriteTime.ToShortDateString()} {info.LastWriteTime.ToShortTimeString()}");
                    Console.SetCursorPosition(columnWidth, startRow++);
                    Console.WriteLine(line);
                    processedRows++;
                }
            }
            catch (DirectoryNotFoundException)
            {
            }
        }

        static void WritePanel()
        {
            //Console.SetCursorPosition();
        }

        static void WriteBorders()
        {
            //todo: header height
            for (int i = 2; i < GetScreenHeight() - headerHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("║");
                Console.SetCursorPosition(GetScreenWidth() / 2 - 1, i);
                Console.Write("║");
                Console.SetCursorPosition(GetScreenWidth() - 1, i);
                Console.WriteLine("║");
            }

            for (int i = 0; i < GetScreenWidth(); i++)
            {
                Console.SetCursorPosition(i, GetScreenHeight() - headerHeight);
                Console.Write("═");
            }
            Console.SetCursorPosition(0, 1);
            Console.WriteLine("╔");

            Console.SetCursorPosition(GetScreenWidth() - 1, 1);
            Console.WriteLine("╗");

            Console.SetCursorPosition(0, GetScreenHeight() - headerHeight);
            Console.WriteLine("╚");

            Console.SetCursorPosition(GetScreenWidth() - 1, GetScreenHeight() - headerHeight);
            Console.WriteLine("╝");

            Console.SetCursorPosition(GetScreenWidth() / 2 - 1, GetScreenHeight() - headerHeight);
            Console.WriteLine("╩");

            Console.SetCursorPosition(GetScreenWidth() / 2 - 1, 1);
            Console.WriteLine("╦");
        }

        static string FormatColumns(int windowWidth, params string[] values)
        {
            windowWidth--;
            if (values.Length == 1)
            {
                return values[0].PadLeft(windowWidth);
            }
            var stringLength = values.Sum(e => e.Length);
            var spacesLength = (windowWidth - stringLength) / (values.Length - 1);
            var remainingSpacesCount = windowWidth - (spacesLength / 2 * 2 * (values.Length - 1) + stringLength);
            var remainingSpacesBegin = "".PadLeft((int)Math.Ceiling(remainingSpacesCount / 2.0));
            var remainingSpacesEnd = "".PadLeft((int)Math.Floor(remainingSpacesCount / 2.0));
            var row = "";
            var spaces = "".PadLeft(spacesLength / 2);
            foreach (var item in values.Skip(1).Take(values.Length - 2))
            {
                row += spaces + item + spaces;
            }
            row = values[0] + remainingSpacesBegin + spaces + row + spaces + remainingSpacesEnd + values[values.Length - 1];
            row = row.PadRight(windowWidth);
            return row;
        }
        static string FormatColumns(params string[] values)
        {
            return FormatColumns(GetScreenWidth(), values);
        }

        static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }
}
