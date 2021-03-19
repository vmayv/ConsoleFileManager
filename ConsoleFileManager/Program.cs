using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFileManager
{
    class Program
    {
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
