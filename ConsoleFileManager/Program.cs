using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConsoleFileManager
{
    class Program
    {
        static void SaveCurrentDirectory(string currDir)
        {
            Properties.Settings.Default.CurrentDirectory = currDir;
            Properties.Settings.Default.Save();
        }
        static string ReadCurrentDirectory()
        {
            return Properties.Settings.Default.CurrentDirectory;
        }
        static void CheckAndWriteConfig()
        {
            if (Properties.Settings.Default.ScreenHeight == 0 || Properties.Settings.Default.ScreenWidth == 0)
            {

                Console.WriteLine("Введите размер окна программы по высоте:");
                Properties.Settings.Default.ScreenHeight = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите размер окна программы по ширине:");
                Properties.Settings.Default.ScreenWidth = Convert.ToInt32(Console.ReadLine());
                Properties.Settings.Default.Save();               
            }
            if (Properties.Settings.Default.CountElementsOnPage == 0)
            {

                Console.WriteLine("Введите количество отображаемых элементов на странице:");
                Properties.Settings.Default.CountElementsOnPage = Convert.ToInt32(Console.ReadLine());
                Properties.Settings.Default.Save();
            }
            if (Properties.Settings.Default.EnclosureLevel == 0)
            {

                Console.WriteLine("Введите количество уровней вложенности каталогов:");
                Properties.Settings.Default.EnclosureLevel = Convert.ToInt32(Console.ReadLine());
                Properties.Settings.Default.Save();
            }
        }

        static void ApplyConfig()
        {
            Console.SetWindowSize(Properties.Settings.Default.ScreenHeight, Properties.Settings.Default.ScreenWidth);
        }

        static List<string> parseInputString(string inputString)
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

        static void Main(string[] args)
        {
            string x = Console.ReadLine();
            foreach (string y in parseInputString(x))
            {
                Console.WriteLine(y);
            }
            Console.ReadKey();
        }
    }
}
