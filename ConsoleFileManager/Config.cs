using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFileManager
{
    public partial class Program
    {
        static void SaveCurrentDirectory(string currDir)
        {
            Properties.Settings.Default.CurrentDirectory = currDir;
            Properties.Settings.Default.Save();
        }
        static string GetCurrentDirectory() => Properties.Settings.Default.CurrentDirectory;
        static void SetCurrentDirectory(string directory)
        {
            Properties.Settings.Default.CurrentDirectory = directory;
            Properties.Settings.Default.Save();
        }
        static int GetScreenHeight() => Properties.Settings.Default.ScreenHeight;
        static int GetScreenWidth() => Properties.Settings.Default.ScreenWidth;
        static int GetEnclosureLevel() => Properties.Settings.Default.EnclosureLevel;
        static void SetEnclosureLevel(int count)
        {
            Properties.Settings.Default.EnclosureLevel = count;
            Properties.Settings.Default.Save();
        }
        static int GetCountElementsOnPage() => Properties.Settings.Default.CountElementsOnPage;
        static void SetCountElementsOnPage(int count)
        {
            Properties.Settings.Default.CountElementsOnPage = count;
            Properties.Settings.Default.Save();
        }
        static void InitializeConfig()
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

        /*static void ApplyConfig()
        {
            Console.SetWindowSize(Properties.Settings.Default.ScreenHeight, Properties.Settings.Default.ScreenWidth);
        }*/

    }
}
