using System;
using System.Collections.Generic;

namespace ConsoleFileManager
{
    public partial class Program
    {
      

        static void Main(string[] args)
        {
            while (true)
            {
                var inputString = Console.ReadLine();
                var commands = parseInputString(inputString);
                ExecuteCommand(commands);
            }
        }
    }
}
