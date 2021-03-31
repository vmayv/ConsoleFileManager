using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleFileManager
{
    public partial class Program
    {
        private static string[] _directories;
        private static string[] _files;
        private static string error = "";

        /// <summary>
        /// Парсер входной строки
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static List<string> parseInputString(string inputString)
        {
            List<string> commands = new List<string>();
            string currentString = "";
            bool isInsideQuotes = false;
            char[] input = inputString.ToCharArray();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '"') // если символ кавычка, то переворачиваем флаг и переходим к следующей итерации
                {
                    isInsideQuotes = !isInsideQuotes;
                    continue;
                }
                if (input[i] == ' ' && !isInsideQuotes) // если символ пробел и не внутри кавычек, то текущую строку добавляем в список, сбрасываем значение в "" и переходим к следующей итерации
                {
                    commands.Add(currentString);
                    currentString = "";
                    continue;
                }
                currentString += input[i];
            }
            commands.Add(currentString);
            return commands;
        }

        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="commands"></param>
        static void ExecuteCommand(List<string> commands)
        {
            var command = commands[0];
            var arguments = commands.Skip(1).ToList();
            switch (command)
            {
                case "exit":
                    Exit();
                    break;
                case "ls":
                    ListDirectory(arguments);
                    break;
                case "cd":
                    ChangeDirectory(arguments);
                    break;
                case "cp":
                    Copy(arguments);
                    break;
                case "rm":
                    Remove(arguments);
                    break;
                case "/paging":
                    SetPaging(arguments);
                    break;
                case "/enclosure":
                    SetEnclosureLevel(arguments);
                    break;
                case "np":
                    NextPage();
                    break;
                case "pp":
                    PreviousPage();
                    break;
                case "sp":
                    SetPage(arguments);
                    break;
                case "touch":
                    CreateFile(arguments);
                    break;
                case "mkdir":
                    CreateDirectory(arguments);
                    break;
                case "cat":
                    PrintFile(arguments);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Выход
        /// </summary>
        private static void Exit()
        {
            System.Environment.Exit(0);
        }

        /// <summary>
        /// Вывод содержимого файла
        /// </summary>
        /// <param name="arguments"></param>
        private static void PrintFile(List<string> arguments)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            string filename = GetAbsolutePath(arguments[0]);
            if (File.Exists(filename))
            {
                Console.Clear();
                var data = File.ReadAllText(filename);
                Console.WriteLine(data);
                Console.ReadKey();
            }
            else
            {
                return;
            }
            Console.BackgroundColor = ConsoleColor.Black;

        }

        /// <summary>
        /// Создать файл
        /// </summary>
        /// <param name="arguments"></param>
        private static void CreateFile(List<string> arguments)
        {
            string filename = GetAbsolutePath(arguments[0]);
            if (File.Exists(filename))
            {
                return;
            }
            else
            {
                File.Create(filename).Close();
            }
        }

        /// <summary>
        /// Создать папку
        /// </summary>
        /// <param name="arguments"></param>
        private static void CreateDirectory(List<string> arguments)
        {
            string directory = GetAbsolutePath(arguments[0]);
            if (Directory.Exists(directory))
            {
                return;
            }
            else
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Следующая страница в списке файлов
        /// </summary>
        private static void NextPage()
        {
            if (currentPage == pagesCount)
            {
                return;
            }
            else
            {
                currentPage++;
            }
        }

        /// <summary>
        /// Предыдущая страница в списке файлов
        /// </summary>
        private static void PreviousPage()
        {
            if (currentPage == 0)
            {
                return;
            }
            else
            {
                currentPage--;
            }
        }

        /// <summary>
        /// Явно задать страницу в списке файлов
        /// </summary>
        /// <param name="arguments"></param>
        private static void SetPage(List<string> arguments)
        {
            int number = Convert.ToInt32(arguments[0]);
            if (number < 1 || number > pagesCount)
            {
                throw new Exception($"Страница {number} не найдена");
            }
            currentPage = number - 1;
        }

        /// <summary>
        /// Поменять директорию
        /// </summary>
        /// <param name="arguments"></param>
        private static void ChangeDirectory(List<string> arguments)
        {
            var path = GetAbsolutePath(arguments[0]);
            if (Directory.Exists(path))
            {
                SetCurrentDirectory(path);
                currentPage = 0;
            }
        }

        /// <summary>
        /// Установить уровень вложенности каталогов
        /// </summary>
        /// <param name="arguments"></param>
        private static void SetEnclosureLevel(List<string> arguments)
        {
            SetEnclosureLevel(Convert.ToInt32(arguments[0]));
        }

        /// <summary>
        /// Установить значение для пагинации списка файлов
        /// </summary>
        /// <param name="arguments"></param>
        private static void SetPaging(List<string> arguments)
        {
            SetCountElementsOnPage(Convert.ToInt32(arguments[0]));
        }

        /// <summary>
        /// Удалить каталог или файл
        /// </summary>
        /// <param name="arguments"></param>
        private static void Remove(List<string> arguments)
        {
            var source = GetAbsolutePath(arguments[0]);
            if (File.Exists(source))
            {
                File.Delete(source);
            }
            if (Directory.Exists(source))
            {
                Directory.Delete(source, true);
            }
        }

        /// <summary>
        /// Скопировать файл
        /// </summary>
        /// <param name="arguments"></param>
        private static void Copy(List<string> arguments)
        {
            var source = GetAbsolutePath(arguments[0]);
            var destination = GetAbsolutePath(arguments[1]);

            if (File.Exists(source))
            {
                if (Directory.Exists(destination))
                {
                    destination = Path.Combine(destination, Path.GetFileName(source));
                }
                File.Copy(source, destination);
            }
            if (Directory.Exists(source))
            {
                CopyDirectory(source, destination);
            }
        }

        /// <summary>
        /// Вывести список директорий
        /// </summary>
        /// <param name="arguments"></param>
        private static void ListDirectory(List<string> arguments)
        {
            var path = Path.Combine(GetCurrentDirectory(), arguments[0]);
            var enclosureLevel = arguments.Count == 3 ? Convert.ToInt32(arguments[2]) : GetEnclosureLevel();
            _directories = Directory.GetDirectories(path);
            _files = Directory.GetFiles(path);

        }

        /// <summary>
        /// Скопировать директорию
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        static void CopyDirectory(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Директория не существует или не может быть найдена"
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
     
            Directory.CreateDirectory(destDirName);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                CopyDirectory(subdir.FullName, tempPath);
            }

        }

        /// <summary>
        /// Получить абсолютный путь
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static string GetAbsolutePath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            return Path.GetFullPath(Path.Combine(GetCurrentDirectory(), path));
        }
    }
}
