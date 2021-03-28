﻿using System;
using System.Collections.Generic;

namespace ConsoleFileManager
{
    public partial class Program
    {
      

        static void Main(string[] args)
        {
            InitializeConfig();
            Console.SetWindowSize(GetScreenWidth(), GetScreenHeight());
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            
            while (true)
            {
                Console.Clear();
                var headerRow = FormatColumns(GetCurrentDirectory(), DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
                Console.WriteLine(headerRow);
                Console.WriteLine(new string('═', GetScreenWidth() - 1));
                WriteSeparator();
                WriteDirectories(GetCurrentDirectory(), GetScreenWidth() / 2, GetEnclosureLevel());
                WriteFiles(GetCurrentDirectory(), Console.WindowWidth / 2, GetCountElementsOnPage());
                Console.SetCursorPosition(0, GetScreenHeight() - 1);

                var inputString = Console.ReadLine();
                var commands = parseInputString(inputString);
                ExecuteCommand(commands);
            }
        }
    }
}
