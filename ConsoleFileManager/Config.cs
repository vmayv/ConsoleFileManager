using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFileManager
{
    public partial class Program
    {
        /// <summary>
        /// Получить текущую директорию из свойств
        /// </summary>
        /// <returns></returns>
        static string GetCurrentDirectory() => Properties.Settings.Default.CurrentDirectory;
        /// <summary>
        /// Установить текущую директорию в свойства
        /// </summary>
        /// <param name="directory"></param>
        static void SetCurrentDirectory(string directory)
        {
            Properties.Settings.Default.CurrentDirectory = directory;
            Properties.Settings.Default.Save();
        }
        /// <summary>
        /// Получить высоту экрана
        /// </summary>
        /// <returns></returns>
        static int GetScreenHeight() => Properties.Settings.Default.ScreenHeight;
        /// <summary>
        /// Получить ширину экрана
        /// </summary>
        /// <returns></returns>
        static int GetScreenWidth() => Properties.Settings.Default.ScreenWidth;
        /// <summary>
        /// Получить уровень вложенности каталогов
        /// </summary>
        /// <returns></returns>
        static int GetEnclosureLevel() => Properties.Settings.Default.EnclosureLevel;
        /// <summary>
        /// Задать уровень вложенности каталогов
        /// </summary>
        /// <param name="count"></param>
        static void SetEnclosureLevel(int count)
        {
            Properties.Settings.Default.EnclosureLevel = count;
            Properties.Settings.Default.Save();
        }
        /// <summary>
        /// Получить количество элементов на странице
        /// </summary>
        /// <returns></returns>
        static int GetCountElementsOnPage() => Properties.Settings.Default.CountElementsOnPage;
        /// <summary>
        /// Задать количество элементов на странице
        /// </summary>
        /// <param name="count"></param>
        static void SetCountElementsOnPage(int count)
        {
            Properties.Settings.Default.CountElementsOnPage = count;
            Properties.Settings.Default.Save();
        }
        /// <summary>
        /// Первоначальная инициализация конфига
        /// </summary>
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
            if(GetCurrentDirectory() == "")
            {
                SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            }
        }

        /*static void ApplyConfig()
        {
            Console.SetWindowSize(Properties.Settings.Default.ScreenHeight, Properties.Settings.Default.ScreenWidth);
        }*/

    }
}
