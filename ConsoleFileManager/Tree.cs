using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFileManager
{
    partial class Program
    {
        static int currentPage;
        static int pagesCount;

        static void WriteDirectories(string path, int columnWidth, int depth)
        {
            WriteDirectories(path, columnWidth, 0, depth);
        }
        static void WriteDirectories(string path, int columnWidth, int depthStart, int depthEnd)
        {
            if (depthStart == depthEnd) return;
            var separator = "|–";
            var directories = Directory.GetDirectories(path);
            foreach (var dir in directories)
            {
                var dirname = separator.PadLeft(depthStart + separator.Length) + Path.GetFileName(dir);
                dirname = dirname.Length > columnWidth - 1 ? dirname.Substring(0, columnWidth - 1) : dirname;

                Console.WriteLine(dirname.PadRight(columnWidth));
                WriteDirectories(dir, columnWidth, depthStart + 1, depthEnd);
            }
        }

        private static void WriteFiles(string path, int columnWidth, int countElementsOnPage)
        {
            var files = Directory.GetFiles(path);
            pagesCount = files.Length / countElementsOnPage;
            var filesOnPage = files.Skip(currentPage * countElementsOnPage).Take(countElementsOnPage);
            var startRow = 1;
            var processedRows = 0;
            foreach (var file in filesOnPage)
            {
                if(processedRows == countElementsOnPage)
                {
                    return;
                }
                var info = new FileInfo(file);
                //TODO: 15 ??
                var name = info.Name.Length > 15 ? info.Name.Substring(0, 15) : info.Name;
                var line = FormatColumns(
                    Console.WindowWidth / 2,
                    name,
                    BytesToString(info.Length),
                    info.LastWriteTime.ToShortDateString(),
                    info.LastWriteTime.ToShortTimeString());
                Console.SetCursorPosition(columnWidth, startRow++);
                Console.WriteLine(line);
                processedRows++;
            }
        }


        static string FormatColumns(int windowWidth, params string[] values)
        {
            var stringLength = values.Sum(e => e.Length);
            var spacesLength = (windowWidth - stringLength) / (values.Length - 1);
            var remainingSpacesCount = windowWidth - (spacesLength / 2 * 2 * (values.Length - 1) + stringLength);
            var remainingSpacesBegin = "".PadLeft((int)Math.Ceiling(remainingSpacesCount / 2.0));
            var remainingSpacesEnd = "".PadLeft((int)Math.Floor(remainingSpacesCount / 2.0));
            var row = "";
            var spaces = "".PadLeft(spacesLength / 2);
            foreach (var item in values.Skip(1).Take(values.Length-2))
            {
                row += spaces + item + spaces;
            }
            row = values[0] + remainingSpacesBegin + spaces + row + spaces + remainingSpacesEnd + values[values.Length -1];
            row = row.PadRight(windowWidth);
            return row;
        }
        static string FormatColumns(params string[] values)
        {
            return FormatColumns(Console.WindowWidth, values);
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
