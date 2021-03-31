using System;
using System.Collections.Generic;
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
            var startRow = headerHeight;
            WriteDirectories(path, columnWidth, 0, depth, 1, ref startRow);
        }
        /// <summary>
        /// Вывод списка директорий
        /// </summary>
        /// <param name="path"></param>
        /// <param name="columnWidth"></param>
        /// <param name="depthStart"></param>
        /// <param name="depthEnd"></param>
        /// <param name="column"></param>
        /// <param name="line"></param>
        static void WriteDirectories(string path, int columnWidth, int depthStart, int depthEnd, int column, ref int line)
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
                if (line == GetScreenHeight() - headerHeight)
                {
                    return;
                }
                var dirname = separator + Path.GetFileName(dir);
                dirname = dirname.Length > columnWidth - 3 ? dirname.Substring(0, columnWidth - 3) : dirname;
                Console.SetCursorPosition(column, line++);
                Console.WriteLine(dirname.PadRight(columnWidth - 1 - column));
                WriteDirectories(dir, columnWidth, depthStart + 1, depthEnd, column + 1, ref line);
            }
        }

        /// <summary>
        /// Вывод списка файлов
        /// </summary>
        /// <param name="path"></param>
        /// <param name="columnWidth"></param>
        /// <param name="countElementsOnPage"></param>
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
        /// <summary>
        /// Отрисовка панели
        /// </summary>
        static void WritePanel()
        {
            var drive = DriveInfo.GetDrives().FirstOrDefault(e => e.Name == Path.GetPathRoot(GetCurrentDirectory()));
            Console.SetCursorPosition(1, panelStartRow);
            Console.WriteLine($"Статистика диска {drive.Name}");
            Console.SetCursorPosition(1, panelStartRow + 1);
            Console.WriteLine($"Общий объем: {BytesToString(drive.TotalSize)}");
            Console.SetCursorPosition(1, panelStartRow + 2);
            Console.WriteLine($"Доступно: {BytesToString(drive.TotalFreeSpace)}");
            Console.SetCursorPosition(1, panelStartRow + 3);

            Console.SetCursorPosition(GetScreenWidth() / 2, panelStartRow);
            Console.WriteLine($"Текущая страница: {currentPage + 1} из {pagesCount + 1}");
            Console.SetCursorPosition(GetScreenWidth() / 2, panelStartRow + 1);
            var errorMesage = error == string.Empty ? "" : $"Ошибка: {error}";
            if (errorMesage.Length > GetScreenWidth() / 2)
            {
                errorMesage = errorMesage.Substring(0, GetScreenWidth() / 2 - 1);
            }
            Console.WriteLine(errorMesage);
            Console.SetCursorPosition(GetScreenWidth() / 2, panelStartRow + 2);
            //Console.WriteLine($"Статистика папки {GetCurrentDirectory()}");

        }

        /// <summary>
        /// Отрисовка границ
        /// </summary>
        static void WriteBorders()
        {
            for (int i = panelStartRow; i < GetScreenHeight() - 1; i++)
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
                Console.SetCursorPosition(i, panelStartRow - 1);
                Console.Write("═");
                Console.SetCursorPosition(i, headerHeight - 1);
                Console.Write("═");
                Console.SetCursorPosition(i, GetScreenHeight() - 2);
                Console.Write("═");
            }
            Console.SetCursorPosition(0, panelStartRow - 1);
            Console.WriteLine("╔");

            Console.SetCursorPosition(GetScreenWidth() - 1, panelStartRow - 1);
            Console.WriteLine("╗");

            Console.SetCursorPosition(0, GetScreenHeight() - 2);
            Console.WriteLine("╚");

            Console.SetCursorPosition(GetScreenWidth() - 1, GetScreenHeight() - 2);
            Console.WriteLine("╝");

            Console.SetCursorPosition(GetScreenWidth() / 2 - 1, GetScreenHeight() - 2);
            Console.WriteLine("╩");

            Console.SetCursorPosition(GetScreenWidth() / 2 - 1, headerHeight - 1);
            Console.WriteLine("╦");
        }

        /// <summary>
        /// Форматирование колонок
        /// </summary>
        /// <param name="windowWidth"></param>
        /// <param name="values"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Для вывода размера файлов
        /// </summary>
        /// <param name="byteCount"></param>
        /// <returns></returns>
        static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"{Math.Sign(byteCount) * num} {suf[place]}";
        }


    }
}
