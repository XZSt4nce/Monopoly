using Monopoly.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading;

namespace Monopoly
{
    public class Monopoly
    {
        static public int playersCount;
        static public int remaining;
        static public int[] treasuries = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        static public int[] chances = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        static public Player[] players;
        static public Quartal[] quartals = new Quartal[40]
        {
            new Quartal(0),
            new Quartal("Старая дорога", "Yellow", 60, 2, 4, 10, 30, 90, 160, 250, 50, 30),
            new Quartal(1),
            new Quartal("Главная дорога", "Yellow", 60, 4, 8, 20, 60, 180, 320, 450, 50, 30),
            new Quartal(8),
            new Quartal("Западный морской порт", 200, 25, 50, 100, 200, 100),
            new Quartal("Аквапарк", "DarkYellow", 100, 6, 12, 30, 90, 270, 400, 550, 50, 50),
            new Quartal(2),
            new Quartal("Городской парк", "DarkYellow", 100, 6, 12, 30, 90, 270, 400, 550, 50, 50),
            new Quartal("Гоночный курорт", "DarkYellow", 120, 8, 16, 40, 100, 300, 450, 600, 50, 60),
            new Quartal(3),
            new Quartal("Спальный район", "DarkGreen", 140, 10, 20, 50, 150, 450, 625, 750, 100, 70),
            new Quartal("Электрическая компания", 150, 75),
            new Quartal("Деловой квартал", "DarkGreen", 140, 10, 20, 50, 150, 450, 625, 750, 100, 70),
            new Quartal("Торговая площадь", "DarkGreen", 160, 12, 24, 60, 180, 500, 700, 900, 100, 80),
            new Quartal("Северный морской порт", 200, 25, 50, 100, 200, 100),
            new Quartal("Улица Пушкина", "Green", 180, 14, 28, 70, 2000, 550, 750, 950, 100, 90),
            new Quartal(1),
            new Quartal("Проспект Мира", "Green", 180, 14, 28, 70, 200, 550, 750, 950, 100, 90),
            new Quartal("Проспект Победы", "Green", 200, 16, 32, 80, 220, 600, 800, 1000, 100, 100),
            new Quartal(4),
            new Quartal("Бар", "Red", 220, 18, 36, 90, 250, 700, 875, 1050, 150, 110),
            new Quartal(2),
            new Quartal("Ночной клуб", "Red", 220, 18, 36, 90, 250, 700, 875, 1050, 150, 110),
            new Quartal("Ресторан", "Red", 240, 20, 40, 100, 300, 750, 925, 1100, 150, 120),
            new Quartal("Восточный морской порт", 200, 25, 50, 100, 200, 100),
            new Quartal("Компьютеры", "Magenta", 260, 22, 44, 110, 330, 800, 975, 1150, 150, 130),
            new Quartal("Интернет", "Magenta", 260, 22, 44, 110, 330, 800, 975, 1150, 150, 130),
            new Quartal("Водопроводная компания", 150, 75),
            new Quartal("Сотовая связь", "Magenta", 280, 24, 48, 120, 360, 850, 1025, 1200, 150, 140),
            new Quartal(5),
            new Quartal("Морские перевозки", "Blue", 300, 26, 52, 130, 390, 900, 1100, 1275, 200, 150),
            new Quartal("Железная дорога", "Blue", 300, 26, 52, 130, 390, 900, 1100, 1275, 200, 150),
            new Quartal(1),
            new Quartal("Авиакомпания", "Blue", 320, 28, 56, 150, 450, 100, 1200, 1400, 200, 160),
            new Quartal("Южный морской порт", 200, 25, 50, 100, 200, 100),
            new Quartal(2),
            new Quartal("Курортная зона", "Cyan", 350, 22, 44, 110, 330, 800, 975, 1150, 150, 130),
            new Quartal(9),
            new Quartal("Гостиничный комплекс", "Cyan", 400, 22, 44, 110, 330, 800, 975, 1150, 150, 130),
        };
        static public readonly Random rnd = new Random();

        /// <summary>
        /// Вывод кубиков с указанными значениями
        /// </summary>
        /// <param name="dice1">Значение первого кубика от 1 до 6</param>
        /// <param name="dice2">Значение второго кубика от 1 до 6</param>
        static public void PrintDices(int dice1, int dice2)
        {
            int column = 3;
            int row = 4;
            Console.SetCursorPosition(column, row);
            Console.WriteLine("┌───────┐   ┌───────┐");
            Console.SetCursorPosition(column, row + 1);
            Console.WriteLine("│       │   │       │");
            Console.SetCursorPosition(column, row + 2);
            Console.WriteLine("│       │   │       │");
            Console.SetCursorPosition(column, row + 3);
            Console.WriteLine("│       │   │       │");
            Console.SetCursorPosition(column, row + 4);
            Console.WriteLine("└───────┘   └───────┘");

            switch (dice1)
            {
                case 1:
                    Console.SetCursorPosition(column + 4, row + 2);
                    Console.Write("▄");
                    break;
                case 2:
                    Console.SetCursorPosition(column + 6, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 3);
                    Console.Write("▄");
                    break;
                case 3:
                    Console.SetCursorPosition(column + 4, row + 2);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 6, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 3);
                    Console.Write("▄");
                    break;
                case 4:
                    Console.SetCursorPosition(column + 6, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 6, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 1);
                    Console.Write("▄");
                    break;
                case 5:
                    Console.SetCursorPosition(column + 6, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 6, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 4, row + 2);
                    Console.Write("▄");
                    break;
                case 6:
                    Console.SetCursorPosition(column + 6, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 6, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 6, row + 2);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 2);
                    Console.Write("▄");
                    break;
            }
            switch (dice2)
            {
                case 1:
                    Console.SetCursorPosition(column + 16, row + 2);
                    Console.Write("▄");
                    break;
                case 2:
                    Console.SetCursorPosition(column + 17, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 3);
                    Console.Write("▄");
                    break;
                case 3:
                    Console.SetCursorPosition(column + 16, row + 2);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 18, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 3);
                    Console.Write("▄");
                    break;
                case 4:
                    Console.SetCursorPosition(column + 18, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 18, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 1);
                    Console.Write("▄");
                    break;
                case 5:
                    Console.SetCursorPosition(column + 18, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 18, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 16, row + 2);
                    Console.Write("▄");
                    break;
                case 6:
                    Console.SetCursorPosition(column + 18, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 18, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 18, row + 2);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 2);
                    Console.Write("▄");
                    break;
            }
            Console.SetCursorPosition(0, row + 6);
        }

        /// <summary>
        /// Вывод кубиков с указанными значениями и подсветкой разного цвета, в зависимости от количества дублей
        /// </summary>
        /// <param name="dice1">Значение первого кубика от 1 до 6</param>
        /// <param name="dice2">Значение второго кубика от 1 до 6</param>
        static public void PrintDices(int dice1, int dice2, int doubles)
        {
            int column = 165;
            int row = 53;
            if (doubles == 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            else if (doubles == 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            else if (doubles == 3)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            Console.SetCursorPosition(column, row);
            Console.WriteLine("┌───────┐   ┌───────┐");
            Console.SetCursorPosition(column, row + 1);
            Console.WriteLine("│       │   │       │");
            Console.SetCursorPosition(column, row + 2);
            Console.WriteLine("│       │   │       │");
            Console.SetCursorPosition(column, row + 3);
            Console.WriteLine("│       │   │       │");
            Console.SetCursorPosition(column, row + 4);
            Console.WriteLine("└───────┘   └───────┘");

            switch (dice1)
            {
                case 1:
                    Console.SetCursorPosition(column + 4, row + 2);
                    Console.Write("▄");
                    break;
                case 2:
                    Console.SetCursorPosition(column + 6, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 3);
                    Console.Write("▄");
                    break;
                case 3:
                    Console.SetCursorPosition(column + 4, row + 2);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 6, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 3);
                    Console.Write("▄");
                    break;
                case 4:
                    Console.SetCursorPosition(column + 6, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 6, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 1);
                    Console.Write("▄");
                    break;
                case 5:
                    Console.SetCursorPosition(column + 6, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 6, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 4, row + 2);
                    Console.Write("▄");
                    break;
                case 6:
                    Console.SetCursorPosition(column + 6, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 6, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 6, row + 2);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 2, row + 2);
                    Console.Write("▄");
                    break;
            }
            switch (dice2)
            {
                case 1:
                    Console.SetCursorPosition(column + 16, row + 2);
                    Console.Write("▄");
                    break;
                case 2:
                    Console.SetCursorPosition(column + 17, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 3);
                    Console.Write("▄");
                    break;
                case 3:
                    Console.SetCursorPosition(column + 16, row + 2);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 18, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 3);
                    Console.Write("▄");
                    break;
                case 4:
                    Console.SetCursorPosition(column + 18, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 18, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 1);
                    Console.Write("▄");
                    break;
                case 5:
                    Console.SetCursorPosition(column + 18, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 18, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 16, row + 2);
                    Console.Write("▄");
                    break;
                case 6:
                    Console.SetCursorPosition(column + 18, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 18, row + 3);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 1);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 18, row + 2);
                    Console.Write("▄");
                    Console.SetCursorPosition(column + 14, row + 2);
                    Console.Write("▄");
                    break;
            }
            Console.ForegroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Вывод игрового поля, улиц
        /// </summary>
        static public void PrintRoadmap()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine();
            Console.WriteLine("   ╔═════════════╦═════════════╦═════════════╦═════════════╦═════════════╦═════════════╦═════════════╦═════════════╦═════════════╦═════════════╦═════════════╗");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write("    СТАРТ    ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("    Налог    ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write("  Посетитель ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write("   ");
            for (int i = 0; i < playersCount; i++)
            {
                Console.Write(players[i].piece);
            }
            for (int i = 0; i < 10 - playersCount; i++)
            {
                Console.Write(" ");
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║Старая дорога║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║Главное шоссе║");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("   с дохода  ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║  Западный   ║  Аквапарк   ║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║  Городской  ║ Горнолыжный ║");
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write("    ─────>   ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║             ║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("    КАЗНА    ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║             ║");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("Заплатите 10%");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║ морской порт║             ║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("     ШАНС    ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║     парк    ║    курорт   ║");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("   ТЮРЬМА    ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write("Получите 200$");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║     60$     ║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║     60$     ║");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("   или 200$  ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║     200$    ║    100$     ║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║     100$    ║     120$    ║");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.WriteLine("   ╠═════════════╬═════════════╩═════════════╩═════════════╩═════════════╩═════════════╩═════════════╩═════════════╩═════════════╩═════════════╬═════════════╣");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║                                                                                                                             ║");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.WriteLine("   ║ Гостиничный ║                                                                                                                             ║   Спальный  ║");
            Console.Write("   ║   комплекс  ║           ");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("???????????????");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("                                                                                                   ║    район    ║");
            Console.Write("   ║     400$    ║           ");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("?╔═══════════╗?");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("                                                                                                   ║     140$    ║");
            Console.Write("   ╠═════════════╣           ");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("?║           ║?");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("                                                                                                   ╠═════════════╣");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("   Дорогая   ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║           ");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("?║   КАЗНА   ║?");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("                                                                                                   ║");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("   покупка   ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║           ");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("?║           ║?");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("                                                                                                   ║Электрическая║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("  Заплатите  ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║           ");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("?╚═══════════╝?");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("                                                                                                   ║   компания  ║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("     100$    ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║           ");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("???????????????");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("                                                                                                   ║     150$    ║");
            Console.WriteLine("   ╠═════════════╣                                                                                                                             ╠═════════════╣");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║                                                                                                                             ║");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.WriteLine("   ║  Курортная  ║                                                                                                                             ║   Деловой   ║");
            Console.WriteLine("   ║    зона     ║                                                                                                                             ║   квартал   ║");
            Console.WriteLine("   ║    350$     ║                                                                                                                             ║     140$    ║");
            Console.WriteLine("   ╠═════════════╣                                                                                                                             ╠═════════════╣");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║                                                                                                                             ║");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║                                                                                                                             ║   Торговая  ║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("    ШАНС     ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║                                                                                                                             ║    площадь  ║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║                                                                                                                             ║     160$    ║");
            Console.WriteLine("   ╠═════════════╣                                                                                                                             ╠═════════════╣");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║                   ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" __    __     ______     __   __     ______     ______   ______     __         __  __   ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("                  ║");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║    Южный    ║                   ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("/\\ \"-./  \\   /\\  __ \\   /\\ \"-.\\ \\   /\\  __ \\   /\\  == \\ /\\  __ \\   /\\ \\       /\\ \\_\\ \\  ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("                  ║  Северный   ║");
            Console.Write("   ║ морской порт║                   ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\\ \\ \\-./\\ \\  \\ \\ \\/\\ \\  \\ \\ \\-.  \\  \\ \\ \\/\\ \\  \\ \\  _-/ \\ \\ \\/\\ \\  \\ \\ \\____  \\ \\____ \\ ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("                  ║ морской порт║");
            Console.Write("   ║     200$    ║                   ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" \\ \\_\\ \\ \\_\\  \\ \\_____\\  \\ \\_\\\\\"\\_\\  \\ \\_____\\  \\ \\_\\    \\ \\_____\\  \\ \\_____\\  \\/\\_____\\");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("                  ║     200$    ║");
            Console.Write("   ╠═════════════╣                   ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("  \\/_/  \\/_/   \\/_____/   \\/_/ \\/_/   \\/_____/   \\/_/     \\/_____/   \\/_____/   \\/_____/");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("                  ╠═════════════╣");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║                                                                                                                             ║");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.WriteLine("   ║ Авиакомпания║                                                                                                                             ║    Улица    ║");
            Console.WriteLine("   ║             ║                                                                                                                             ║   Пушкина   ║");
            Console.WriteLine("   ║     320$    ║                                                                                                                             ║     180$    ║");
            Console.WriteLine("   ╠═════════════╣                                                                                                                             ╠═════════════╣");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║                                                                                                                             ║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║                                                                                                                             ║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("    КАЗНА    ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║                                                                                                                             ║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("    КАЗНА    ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║                                                                                                                             ║");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.WriteLine("   ╠═════════════╣                                                                                                                             ╠═════════════╣");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║                                                                                                     ");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("???????????????");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("         ║");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║   Железная  ║                                                                                                     ");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("?╔═══════════╗?");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("         ║   Проспект  ║");
            Console.Write("   ║    дорога   ║                                                                                                     ");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("?║           ║?");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("         ║     Мира    ║");
            Console.Write("   ║     300$    ║                                                                                                     ");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("?║    ШАНС   ║?");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("         ║     180$    ║");
            Console.Write("   ╠═════════════╣                                                                                                     ");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("?║           ║?");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("         ╠═════════════╣");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║                                                                                                     ");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("?╚═══════════╝?");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("         ║");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║   Морские   ║                                                                                                     ");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("???????????????");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("         ║   Проспект  ║");
            Console.WriteLine("   ║  перевозки  ║                                                                                                                             ║    Победы   ║");
            Console.WriteLine("   ║     300$    ║                                                                                                                             ║     200$    ║");
            Console.WriteLine("   ╠═════════════╬═════════════╦═════════════╦═════════════╦═════════════╦═════════════╦═════════════╦═════════════╦═════════════╦═════════════╬═════════════╣");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║");
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.Write("      P      ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("      Вы     ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║Сотовая связь║Водопроводная║   Интернет  ║  Компьютеры ║  Восточный  ║   Ресторан  ║    Ночной   ║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║     Бар     ║");
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write(" арестованы! ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║             ║   станция   ║             ║             ║ морской порт║             ║     клуб    ║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("     ШАНС    ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║             ║");
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.Write("  Бесплатная ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.Write("   ║");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║     280$    ║     200$    ║     260$    ║     260$    ║     200$    ║     240$    ║     220$    ║");
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("║     220$    ║");
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.Write("   парковка  ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.WriteLine("   ╚═════════════╩═════════════╩═════════════╩═════════════╩═════════════╩═════════════╩═════════════╩═════════════╩═════════════╩═════════════╩═════════════╝");
        }

        /// <summary>
        /// Вывод таблицы игроков с подсветкой ходящего игрока
        /// </summary>
        /// <param name="turn">Номер ходящего игрока</param>
        static void PrintPlayers(int turn)
        {
            int addWidth, addHeight;
            ConsoleColor background;
            for (int i = 0; i < players.Length; i++)
            {
                addWidth = 34 * (i % 2);
                addHeight = 12 * (i / 2);
                if (players[i].bankrupt) background = ConsoleColor.Gray;
                else background = ConsoleColor.White;
                Console.SetCursorPosition(165 + addWidth, 2 + addHeight);
                Console.BackgroundColor = background;
                Console.Write("╔═════════════════════════════╗");
                Console.SetCursorPosition(165 + addWidth, 3 + addHeight);
                Console.Write("║");
                if (i == turn) Console.BackgroundColor = ConsoleColor.Yellow;
                Console.Write($"{players[i].name} {players[i].piece}");
                for (int j = 0; j < 25 - players[i].name.Length - Convert.ToString(players[i].balance).Length; j++)
                {
                    Console.Write(" ");
                }
                Console.Write($"{players[i].balance}$ ");
                Console.BackgroundColor = background;
                Console.Write("║");
                Console.SetCursorPosition(165 + addWidth, 4 + addHeight);
                Console.Write("╠═════════════════════════════╣");
                Console.SetCursorPosition(165 + addWidth, 5 + addHeight);
                Console.Write($"║ Карты освобождения: {players[i].liberation}");
                for (int j = 0; j < 8 - Convert.ToString(players[i].liberation).Length; j++)
                {
                    Console.Write(" ");
                }
                Console.Write("║");
                Console.SetCursorPosition(165 + addWidth, 6 + addHeight);
                Console.Write("║            УЛИЦЫ            ║");
                Console.SetCursorPosition(165 + addWidth, 7 + addHeight);
                Console.Write("║ ");
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.Write("Жёлтая");
                Console.BackgroundColor = background;
                Console.Write($" {players[i].yellow}/3 ");
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Write("Тёмно-жёлтая");
                Console.BackgroundColor = background;
                Console.Write($" {players[i].darkYellow}/3 ║");
                Console.SetCursorPosition(165 + addWidth, 8 + addHeight);
                Console.Write("║ ");
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("Зелёная");
                Console.BackgroundColor = background;
                Console.Write($" {players[i].green}/3   ");
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.Write("Пурпурная");
                Console.BackgroundColor = background;
                Console.Write($" {players[i].magenta}/3 ║");
                Console.SetCursorPosition(165 + addWidth, 9 + addHeight);
                Console.Write("║ ");
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("Синяя");
                Console.BackgroundColor = background;
                Console.Write($" {players[i].blue}/3 ");
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write("Тёмно-зелёная");
                Console.BackgroundColor = background;
                Console.Write($" {players[i].darkGreen}/3 ║");
                Console.SetCursorPosition(165 + addWidth, 10 + addHeight);
                Console.Write("║ ");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("Красная");
                Console.BackgroundColor = background;
                Console.Write($" {players[i].red}/3     ");
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.Write("Голубая");
                Console.BackgroundColor = background;
                Console.Write($" {players[i].cyan}/2 ║");
                Console.SetCursorPosition(165 + addWidth, 11 + addHeight);
                Console.Write("║          ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Чёрная");
                Console.BackgroundColor = background;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($" {players[i].black}/4         ║");
                Console.SetCursorPosition(165 + addWidth, 12 + addHeight);
                Console.Write("╚═════════════════════════════╝");
            }
        }

        /// <summary>
        /// Вывод фигур игроков
        /// </summary>
        /// <param name="oldPosition">Предыдущая позиция игрока</param>
        /// <param name="newPosition">Следующая позиция игрока</param>
        /// <param name="_piece">Фигура игрока</param>
        static void PrintPieces(int oldPosition, int newPosition, char _piece)
        {
            int oldVisitors = quartals[oldPosition].visitors;
            int newVisitors = quartals[newPosition].visitors;
            int oldPiecesPtr = 0;
            int newPiecesPtr = 1;
            char[] oldPieces = new char[oldVisitors];
            char[] newPieces = new char[newVisitors];
            newPieces[0] = _piece;
            bool prisoned = false;
            for (int i = 0; i < playersCount; i++)
            {
                if (players[i].position == oldPosition && players[i].piece != _piece) oldPieces[oldPiecesPtr++] = players[i].piece;
                if (players[i].position == newPosition) newPieces[newPiecesPtr++] = players[i].piece;
                if (players[i].piece == _piece) prisoned = players[i].prisoned;
            }

            int column, row;
            if (oldPosition <= 10)
            {

                if (oldPosition == 0)
                {
                    row = 3;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                }
                else if (oldPosition == 10)
                {
                    if (prisoned)
                    {
                        row = 5;
                        Console.BackgroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        row = 3;
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    }
                }
                else row = 0;
                for (int i = 0; i < 8; i++)
                {
                    column = 7 + i + 14 * oldPosition;
                    Console.SetCursorPosition(column, row);
                    Console.Write(" ");
                }
                for (int i = 0; i < oldVisitors; i++)
                {
                    column = 7 + i + 14 * oldPosition;
                    Console.SetCursorPosition(column, row);
                    Console.Write(oldPieces[i]);
                }
                Console.BackgroundColor = ConsoleColor.White;
            }
            else if (oldPosition < 20)
            {
                for (int i = 0; i < 8; i++)
                {
                    column = 158 + i / 4;
                    row = 7 + (oldPosition - 11) * 5 - 3 * (i / 4) + i;
                    Console.SetCursorPosition(column, row);
                    Console.Write(" ");
                }
                for (int i = 0; i < oldVisitors; i++)
                {
                    column = 158 + i / 4;
                    row = 7 + (oldPosition - 11) * 5 - 3 * (i / 4) + i;
                    Console.SetCursorPosition(column, row);
                    Console.Write(oldPieces[i]);
                }
            }
            else if (oldPosition <= 30)
            {
                if (oldPosition == 20)
                {
                    row = 53;
                    Console.BackgroundColor = ConsoleColor.Cyan;
                }
                else if (oldPosition == 30)
                {
                    row = 52;
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                else row = 57;
                for (int i = 0; i < 8; i++)
                {
                    column = 147 + i - 14 * (oldPosition - 20);
                    Console.SetCursorPosition(column, row);
                    Console.Write(" ");
                }
                for (int i = 0; i < oldVisitors; i++)
                {
                    column = 147 + i - 14 * (oldPosition - 20);
                    Console.SetCursorPosition(column, row);
                    Console.Write(oldPieces[i]);
                }
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    column = 1 - i / 4;
                    row = 47 - (oldPosition - 31) * 5 - 3 * (i / 4) + i;
                    Console.SetCursorPosition(column, row);
                    Console.Write(" ");
                }
                for (int i = 0; i < oldVisitors; i++)
                {
                    column = 1 - i / 4;
                    row = 47 - (oldPosition - 31) * 5 - 3 * (i / 4) + i;
                    Console.SetCursorPosition(column, row);
                    Console.Write(oldPieces[i]);
                }
            }

            if (newPosition <= 10)
            {

                if (newPosition == 0)
                {
                    row = 3;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                }
                else if (newPosition == 10)
                {
                    if (prisoned)
                    {
                        row = 5;
                        Console.BackgroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        row = 3;
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    }
                }
                else row = 0;
                for (int i = 0; i < 8; i++)
                {
                    column = 7 + i + 14 * newPosition;
                    Console.SetCursorPosition(column, row);
                    Console.Write(" ");
                }
                for (int i = 0; i < newVisitors; i++)
                {
                    column = 7 + i + 14 * newPosition;
                    Console.SetCursorPosition(column, row);
                    Console.Write(newPieces[i]);
                }
                Console.BackgroundColor = ConsoleColor.White;
            }
            else if (newPosition < 20)
            {
                for (int i = 0; i < 8; i++)
                {
                    column = 158 + i / 4;
                    row = 7 + (newPosition - 11) * 5 - 3 * (i / 4) + i;
                    Console.SetCursorPosition(column, row);
                    Console.Write(" ");
                }
                for (int i = 0; i < newVisitors; i++)
                {
                    column = 158 + i / 4;
                    row = 7 + (newPosition - 11) * 5 - 3 * (i / 4) + i;
                    Console.SetCursorPosition(column, row);
                    Console.Write(newPieces[i]);
                }
            }
            else if (newPosition <= 30)
            {
                if (newPosition == 20)
                {
                    row = 53;
                    Console.BackgroundColor = ConsoleColor.Cyan;
                }
                else if (newPosition == 30)
                {
                    row = 52;
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                else row = 57;
                for (int i = 0; i < 8; i++)
                {
                    column = 147 + i - 14 * (newPosition - 20);
                    Console.SetCursorPosition(column, row);
                    Console.Write(" ");
                }
                for (int i = 0; i < newVisitors; i++)
                {
                    column = 147 + i - 14 * (newPosition - 20);
                    Console.SetCursorPosition(column, row);
                    Console.Write(newPieces[i]);
                }
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    column = 1 - i / 4;
                    row = 47 - (newPosition - 31) * 5 - 3 * (i / 4) + i;
                    Console.SetCursorPosition(column, row);
                    Console.Write(" ");
                }
                for (int i = 0; i < newVisitors; i++)
                {
                    column = 1 - i / 4;
                    row = 47 - (newPosition - 31) * 5 - 3 * (i / 4) + i;
                    Console.SetCursorPosition(column, row);
                    Console.Write(newPieces[i]);
                }
            }
        }

        /// <summary>
        /// Вывод меню действий
        /// </summary>
        /// <param name="player">Номер ходящего игрока</param>
        static public void Menu(int player)
        { //╬
            Console.SetCursorPosition(165, 50);
            Console.Write("╔═══════════════════════════════════════════════════════════════╗");
            Console.Write("║                              МЕНЮ                             ║");
            Console.Write("╠═══════════════════════════════════════════════════════════════╣");
        }

        /// <summary>
        /// Вытаскивание карты "Казна"
        /// </summary>
        /// <param name="player">Номер ходящего игрока</param>
        static void Treasury(Player player)
        {
            int card = treasuries[0];
            for (var i = 1; i < 16; i++)
            {
                treasuries[i - 1] = treasuries[i];
            }
            treasuries[15] = card;
            switch (card)
            {
                // SetCursorPosition + WriteLine
                case 0:
                    player.balance += 25;
                    break;
                case 1:
                    player.balance -= 50;
                    break;
                case 2:
                    player.balance = 1;
                    break;
                case 3:
                    player.balance -= 50;
                    break;
                case 4:
                    player.liberation++;
                    break;
                case 5:
                    player.position = 10;
                    player.prisoned = true;
                    break;
                case 6:
                    player.balance -= 100;
                    break;
                case 7:
                    // -10 || chance
                    break;
                case 8:
                    player.balance += 25;
                    break;
                case 9:
                    player.balance += 25;
                    break;
                case 10:
                    player.balance += 50;
                    break;
                case 11:
                    for (int i = 0; i < playersCount; i++)
                    {
                        players[i].balance -= 10;
                        player.balance += 10;
                    }
                    break;
                case 12:
                    player.balance += 200;
                    break;
                case 13:
                    player.balance += 100;
                    break;
                case 14:
                    player.balance += 10;
                    break;
                case 15:
                    player.balance += 100;
                    break;
            }
        }

        /// <summary>
        /// Вытаскивание карты "Шанс"
        /// </summary>
        /// <param name="player">Номер ходящего игрока</param>
        static void Chance(Player player)
        {
            int card = chances[0];
            for (var i = 1; i < 16; i++)
            {
                chances[i - 1] = chances[i];
            }
            chances[15] = card;
            switch (card)
            {
                // SetCursorPosition + WriteLine
                case 0:
                    player.balance -= 40 * player.houses + 115 * player.hotels;
                    break;
                case 1:
                    player.position = 10;
                    player.prisoned = true;
                    break;
                case 2:
                    player.balance -= 20;
                    break;
                case 3:
                    if (player.position < 3) player.position += 37;
                    else player.position -= 3;
                    break;
                case 4:
                    player.liberation++;
                    break;
                case 5:
                    player.balance += 150;
                    break;
                case 6:
                    player.balance += 50;
                    break;
                case 7:
                    if (player.position > 6) player.balance += 200;
                    player.position = 6;
                    break;
                case 8:
                    player.balance += 200;
                    player.position = 0;
                    break;
                case 9:
                    player.balance -= 15;
                    break;
                case 10:
                    if (player.position > 15) player.balance += 200;
                    player.position = 15;
                    break;
                case 11:
                    player.balance += 100;
                    break;
                case 12:
                    player.position = 39;
                    break;
                case 13:
                    if (player.position > 24) player.balance += 200;
                    player.position = 24;
                    break;
                case 14:
                    player.balance -= 25 * player.houses + 100 * player.hotels;
                    break;
                case 15:
                    player.balance -= 150;
                    break;
            }
        }

        static void Main()
        {
            // Window
            Console.Title = "Монополия";
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetBufferSize(Console.LargestWindowWidth, 110);

            // Music
            SoundPlayer sndPlayer = new SoundPlayer(Resources.music);
            try
            {
                sndPlayer.PlayLooping();
            }
            catch (System.IO.FileNotFoundException) { }

            // Shuffle
            int cards = 16;
            while (cards > 1)
            {
                int index = rnd.Next(cards--);
                (treasuries[index], treasuries[cards]) = (treasuries[cards], treasuries[index]);
            }
            cards = 16;
            while (cards > 1)
            {
                int index = rnd.Next(cards--);
                (chances[index], chances[cards]) = (chances[cards], chances[index]);
            }

            // Players input
            while (true)
            {
                Console.Write("Введите количество игроков (мин. 3, макс. 8): ");
                try
                {
                    playersCount = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Неправильный ввод! Попробуйте ещё раз");
                    continue;
                }
                if (playersCount < 3)
                {
                    Console.WriteLine("Слишком мало игроков! Попробуйте ещё раз");
                    continue;
                }
                if (playersCount > 8)
                {
                    Console.WriteLine("Слишком много игроков! Попробуйте ещё раз");
                    continue;
                }
                break;
            }

            string[] names = new string[playersCount];
            players = new Player[playersCount];
            remaining = playersCount;
            quartals[0].visitors = playersCount;

            for (int i = 0; i < playersCount; i++)
            {
                Console.Write($"Игрок {i + 1}, введите своё имя (мин. 2 символа, макс. 16 символов): ");
                string name = Console.ReadLine();
                name = name.Trim();
                bool prev_space = false;
                string trimmed_name = "";
                foreach (char character in name)
                {
                    if (character == ' ')
                    {
                        if (prev_space) continue;
                        else prev_space = true;
                    }
                    else prev_space = false;
                    trimmed_name += character;
                }
                if (names.Contains(trimmed_name))
                {
                    Console.WriteLine("Кто-то из игроков уже выбрал это имя! Введите другое");
                    i--;
                    continue;
                }
                if (trimmed_name.Length < 2)
                {
                    Console.WriteLine("Слишком короткое имя! Введите другое");
                    i--;
                    continue;
                }
                if (trimmed_name.Length > 16)
                {
                    Console.WriteLine("Слишком длинное имя! Введите другое");
                    i--;
                    continue;
                }
                names[i] = trimmed_name;
            }

            Console.Clear();
            char[] pieces = new char[] { '♦', '♣', '☻', '↕', '♀', '♫', '☺', '♥' };
            char[] figures = new char[playersCount];
            for (int i = 0; i < playersCount; i++)
            {
                Console.WriteLine($"{names[i]}, выберите фигурку, которой будете играть");
                for (int j = 0; j < pieces.Count(); j++)
                {
                    Console.WriteLine($"{j + 1}. {pieces[j]}");
                }
                while (true)
                {
                    string choice = "" + Console.ReadKey(true).KeyChar;
                    int piece;
                    try
                    {
                        piece = int.Parse(choice);
                    }
                    catch
                    {
                        continue;
                    }
                    if (piece == 0 || piece > pieces.Count())
                    {
                        continue;
                    }
                    piece--;
                    figures[i] = pieces[piece];
                    char[] tmp_pieces = new char[pieces.Count() - 1];
                    int tmp_ptr = 0;
                    for (int j = 0; j < pieces.Count(); j++)
                    {
                        if (piece == j) continue;
                        tmp_pieces[tmp_ptr++] = pieces[j];
                    }
                    pieces = tmp_pieces;
                    break;
                }
                Console.Clear();
            }

            for (int i = 0; i < playersCount; i++) players[i] = new Player(names[i], figures[i]);


            // Queue definition
            int dice1, dice2;
            int[] throws = new int[playersCount];
            for (int i = 0; i < playersCount; i++)
            {
                throws[i] = 0;
            }

            Console.WriteLine("Определяется очередь хода");
            for (int i = 0; i < playersCount; i++)
            {
                Console.WriteLine($"{players[i].name}, бросайте кубики");
                Console.WriteLine("Нажмите пробел, чтобы бросить кубики");
                while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) ;
                dice1 = rnd.Next(1, 7);
                dice2 = rnd.Next(1, 7);
                throws[i] = dice1 + dice2;
                PrintDices(dice1, dice2);
                Console.WriteLine($"Сумма на кубиках: {dice1 + dice2}");
                Console.WriteLine("\nНажмите Enter, чтобы завершить ход");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;
                Console.Clear();
            }

            for (int i = 0; i < playersCount; i++)
            {
                for (int j = 0; j < playersCount - i - 1; j++)
                {
                    if (throws[j] < throws[j + 1])
                    {
                        (throws[j], throws[j + 1]) = ((throws[j + 1], throws[j]));
                        (players[j], players[j + 1]) = ((players[j + 1], players[j]));
                    }
                }
            }

            // Game
            PrintRoadmap();
            int sleep = 300;
            int doubles = 0;
            while (remaining > 1)
            {
                for (int i = 0; i < playersCount; i++)
                {
                    if (players[i].bankrupt) continue;
                    PrintPlayers(i);
                    Menu(i);
                    if (players[i].prisoned)
                    {

                    }
                    else
                    {
                        dice1 = rnd.Next(1, 7);
                        dice2 = rnd.Next(1, 7);
                        int newPosition = (players[i].position + dice1 + dice2) % 40;
                        if (dice1 == dice2)
                        {
                            doubles++;
                            if (doubles == 3)
                            {
                                players[i].position = 10;
                                players[i].prisoned = true;
                                quartals[30].visitors++;
                                PrintPieces(30, 10, players[i].piece);
                                quartals[10].visitors--;
                                doubles = 0;
                            }
                            else
                            {
                                i--;
                            }
                        }
                        else doubles = 0;
                        PrintDices(dice1, dice2, doubles);
                        if (!players[i].prisoned)
                        {
                            if (players[i].position > newPosition)
                            {
                                for (int j = players[i].position; j < 40; j++)
                                {
                                    quartals[(j + 1) % 40].visitors++;
                                    PrintPieces(j, (j + 1) % 40, players[i].piece);
                                    quartals[j].visitors--;
                                    Thread.Sleep(sleep);
                                }
                                for (int j = 0; j < newPosition; j++)
                                {
                                    quartals[j + 1].visitors++;
                                    PrintPieces(j, j + 1, players[i].piece);
                                    quartals[j].visitors--;
                                    Thread.Sleep(sleep);
                                }
                            }
                            else
                            {
                                for (int j = players[i].position; j < newPosition; j++)
                                {
                                    quartals[j + 1].visitors++;
                                    PrintPieces(j, j + 1, players[i].piece);
                                    quartals[j].visitors--;
                                    Thread.Sleep(sleep);
                                }
                            }
                            quartals[newPosition].visitors++;
                            players[i].position = newPosition;
                            if (newPosition == 30)
                            {
                                players[i].position = 10;
                                players[i].prisoned = true;
                                quartals[30].visitors++;
                                PrintPieces(30, 10, players[i].piece);
                                quartals[10].visitors--;
                                doubles = 0;
                            }
                        }
                    }
                    Console.ReadKey(true);
                }
            }
        }
    }
}
