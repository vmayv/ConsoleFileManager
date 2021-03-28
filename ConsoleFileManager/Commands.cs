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

        public static List<string> parseInputString(string inputString)
        {
            List<string> commands = new List<string>(); // выходной список
            string currentString = ""; // текущая строка
            bool isInsideQuotes = false; // флаг внутренних кавычек
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
                currentString += input[i]; // собираем строку посимвольно
            }
            commands.Add(currentString); // добавляем строку в список
            return commands;
        }

        static void ExecuteCommand(List<string> commands)
        {
            var command = commands[0];
            var arguments = commands.Skip(1).ToList();
            switch (command)
            {
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
                case "file":
                    ShowFileAttributes(arguments);
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

        private static void PrintFile(List<string> arguments)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            string filename = arguments[0];
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
            Console.BackgroundColor = ConsoleColor.DarkBlue;

        }

        private static void CreateFile(List<string> arguments)
        {
            string filename = arguments[0];
            if (File.Exists(filename))
            {
                return;
            }
            else
            {
                File.Create(filename).Close();
            }
        }

        private static void CreateDirectory(List<string> arguments)
        {
            string directory = arguments[0];
            if (Directory.Exists(directory))
            {
                return;
            }
            else
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static void NextPage()
        {
            if (currentPage == pagesCount - 1)
            {
                return;
            }
            else
            {
                currentPage++;
            }
        }

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

        private static void SetPage(List<string> arguments)
        {
            int number = Convert.ToInt32(arguments[0]);
            currentPage = number - 1;
        }
        //TODO: абсолютные и относительные пути
        private static void ChangeDirectory(List<string> arguments)
        {
            var path = GetAbsolutePath(arguments[0]);
            SetCurrentDirectory(path);
            currentPage = 0;
        }

        private static void SetEnclosureLevel(List<string> arguments)
        {
            SetEnclosureLevel(Convert.ToInt32(arguments[0]));
        }

        private static void SetPaging(List<string> arguments)
        {
            SetCountElementsOnPage(Convert.ToInt32(arguments[0]));
        }

        private static void ShowFileAttributes(List<string> arguments)
        {
            var file = Path.Combine(GetCurrentDirectory(), arguments[0]);
            var extension = Path.GetFileNameWithoutExtension(file);
            var fileInfo = new FileInfo(file);
            var attributes = fileInfo.Attributes.ToString();
            var size = fileInfo.Length;
        }

        private static void Remove(List<string> arguments)
        {
            var source = Path.Combine(GetCurrentDirectory(), arguments[0]);
            if (File.Exists(source))
            {
                File.Delete(source);
            }
            if (Directory.Exists(source))
            {
                Directory.Delete(source, true);
            }
        }

        private static void Copy(List<string> arguments)
        {
            var source = Path.Combine(GetCurrentDirectory(), arguments[0]);
            var destination = Path.Combine(GetCurrentDirectory(), arguments[1]);
            if (File.Exists(source))
            {
                File.Copy(source, destination);
            }
            if (Directory.Exists(source))
            {

                CopyDirectory(source, destination);
            }
        }

        private static void ListDirectory(List<string> arguments)
        {
            var path = Path.Combine(GetCurrentDirectory(), arguments[0]);
            var enclosureLevel = arguments.Count == 3 ? Convert.ToInt32(arguments[2]) : GetEnclosureLevel();
            _directories = Directory.GetDirectories(path);
            _files = Directory.GetFiles(path);

        }

        static void CopyDirectory(string sourceDirName, string destDirName)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                CopyDirectory(subdir.FullName, tempPath);
            }

        }
    
        static string GetAbsolutePath(string path)
        {
            if(Path.IsPathRooted(path)) {
                return path;
            }
            return Path.GetFullPath(Path.Combine(GetCurrentDirectory(), path));
        }
    }
}
