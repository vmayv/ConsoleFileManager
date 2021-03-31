using System;
using System.Collections.Generic;

namespace ConsoleFileManager
{
    public partial class Program
    {

        static int panelStartRow = 2;
        static int headerHeight = 6;
        static void Main(string[] args)
        {
            // Инициализируем конфиг и задаём параметры
            InitializeConfig();
            Console.SetWindowSize(GetScreenWidth(), GetScreenHeight());
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "ConsoleFileManager";

            while (true)
            {
                var exceptionWasThrown = false;
                try
                {
                    Console.Clear();
                    var headerRow = FormatColumns(" " + GetCurrentDirectory(), DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
                    Console.WriteLine(headerRow);
                    WriteBorders();
                    WriteDirectories(GetCurrentDirectory(), GetScreenWidth() / 2, GetEnclosureLevel());
                    WriteFiles(GetCurrentDirectory(), Console.WindowWidth / 2, GetCountElementsOnPage());
                }
                catch (Exception ex)
                {
                    exceptionWasThrown = true;
                    error = ex.Message;
                }
                finally
                {
                    if (!exceptionWasThrown)
                    {
                        error = "";
                    };
                    WritePanel();
                    Console.SetCursorPosition(0, GetScreenHeight() - 1);
                    var inputString = Console.ReadLine();
                    var commands = parseInputString(inputString);
                    ExecuteCommand(commands);
                }
            }
        }
    }
}
