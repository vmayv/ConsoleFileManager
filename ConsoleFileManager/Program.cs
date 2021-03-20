using System;
using System.Collections.Generic;

namespace ConsoleFileManager
{
    public partial class Program
    {
      

        static void Main(string[] args)
        {
            var inputString = Console.ReadLine();
            var commands = parseInputString(inputString);
            ExecuteCommand(commands);
        }
    }
}
