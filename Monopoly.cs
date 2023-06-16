using Monopoly.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows.Media;

// ToDo:
// отображать на поле владельцев улиц;
// отображать уровень улиц;
// отображать недвижимость игрока.
namespace Monopoly
{
    public class Monopoly
    {
        static public int playersCount;
        static public int remaining;
        static public List<int> treasuries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        static public List<int> chances = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        static public bool musicMuted = false;
        static readonly int sleep = 400;
        static int dice1, dice2;
        static int doubles = 0;
        static bool doubled = false;
        static readonly SoundPlayer musicPlayer = new SoundPlayer(Resources.music);
        static readonly MediaPlayer moneyPlayer = new MediaPlayer();
        static readonly MediaPlayer policePlayer = new MediaPlayer();
        static readonly MediaPlayer paperPlayer = new MediaPlayer();
        static readonly MediaPlayer happyPlayer = new MediaPlayer();
        static readonly MediaPlayer sadPlayer = new MediaPlayer();
        static public Player[] players;
        static public Quartal[] quartals = new Quartal[40]
        {
            new Quartal(0),
            new Quartal("Старая дорога", ConsoleColor.Yellow, 60, 2, 4, 10, 30, 90, 160, 250, 50, 30),
            new Quartal(1),
            new Quartal("Главное шоссе", ConsoleColor.Yellow, 60, 4, 8, 20, 60, 180, 320, 450, 50, 30),
            new Quartal(8),
            new Quartal("Западный морской порт", 200, 25, 50, 100, 200, 100),
            new Quartal("Аквапарк", ConsoleColor.DarkYellow, 100, 6, 12, 30, 90, 270, 400, 550, 50, 50),
            new Quartal(2),
            new Quartal("Городской парк", ConsoleColor.DarkYellow, 100, 6, 12, 30, 90, 270, 400, 550, 50, 50),
            new Quartal("Горнолыжный курорт", ConsoleColor.DarkYellow, 120, 8, 16, 40, 100, 300, 450, 600, 50, 60),
            new Quartal(3),
            new Quartal("Спальный район", ConsoleColor.DarkGreen, 140, 10, 20, 50, 150, 450, 625, 750, 100, 70),
            new Quartal("Электрическая компания"),
            new Quartal("Деловой квартал", ConsoleColor.DarkGreen, 140, 10, 20, 50, 150, 450, 625, 750, 100, 70),
            new Quartal("Торговая площадь", ConsoleColor.DarkGreen, 160, 12, 24, 60, 180, 500, 700, 900, 100, 80),
            new Quartal("Северный морской порт", 200, 25, 50, 100, 200, 100),
            new Quartal("Улица Пушкина", ConsoleColor.Green, 180, 14, 28, 70, 200, 550, 750, 950, 100, 90),
            new Quartal(1),
            new Quartal("Проспект Мира", ConsoleColor.Green, 180, 14, 28, 70, 200, 550, 750, 950, 100, 90),
            new Quartal("Проспект Победы", ConsoleColor.Green, 200, 16, 32, 80, 220, 600, 800, 1000, 100, 100),
            new Quartal(4),
            new Quartal("Бар", ConsoleColor.Red, 220, 18, 36, 90, 250, 700, 875, 1050, 150, 110),
            new Quartal(2),
            new Quartal("Ночной клуб", ConsoleColor.Red, 220, 18, 36, 90, 250, 700, 875, 1050, 150, 110),
            new Quartal("Ресторан", ConsoleColor.Red, 240, 20, 40, 100, 300, 750, 925, 1100, 150, 120),
            new Quartal("Восточный морской порт", 200, 25, 50, 100, 200, 100),
            new Quartal("Компьютеры", ConsoleColor.Magenta, 260, 22, 44, 110, 330, 800, 975, 1150, 150, 130),
            new Quartal("Интернет", ConsoleColor.Magenta, 260, 22, 44, 110, 330, 800, 975, 1150, 150, 130),
            new Quartal("Водопроводная станция"),
            new Quartal("Сотовая связь", ConsoleColor.Magenta, 280, 24, 48, 120, 360, 850, 1025, 1200, 150, 140),
            new Quartal(5),
            new Quartal("Морские перевозки", ConsoleColor.Blue, 300, 26, 52, 130, 390, 900, 1100, 1275, 200, 150),
            new Quartal("Железная дорога", ConsoleColor.Blue, 300, 26, 52, 130, 390, 900, 1100, 1275, 200, 150),
            new Quartal(1),
            new Quartal("Авиакомпания", ConsoleColor.Blue, 320, 28, 56, 150, 450, 1000, 1200, 1400, 200, 160),
            new Quartal("Южный морской порт", 200, 25, 50, 100, 200, 100),
            new Quartal(2),
            new Quartal("Курортная зона", ConsoleColor.Cyan, 350, 35, 70, 175, 500, 1100, 1300, 1500, 200, 175),
            new Quartal(9),
            new Quartal("Гостиничный комплекс", ConsoleColor.Cyan, 400, 50, 100, 200, 600, 1400, 1700, 2000, 200, 200),
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
            int column = 187;
            int row = 50;
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
            Console.Write("┌───────┐   ┌───────┐");
            Console.SetCursorPosition(column, row + 1);
            Console.Write("│       │   │       │");
            Console.SetCursorPosition(column, row + 2);
            Console.Write("│       │   │       │");
            Console.SetCursorPosition(column, row + 3);
            Console.Write("│       │   │       │");
            Console.SetCursorPosition(column, row + 4);
            Console.Write("└───────┘   └───────┘");
            Console.SetCursorPosition(165, row + 5);
            Console.Write("                                                                 ");

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
            Console.Write("║                                                                                                                             ║");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("             ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("║");
            Console.WriteLine("   ║    Южный    ║                                                                                                                             ║  Северный   ║");
            Console.WriteLine("   ║ морской порт║                                                                                                                             ║ морской порт║");
            Console.WriteLine("   ║     200$    ║                                                                                                                             ║     200$    ║");
            Console.WriteLine("   ╠═════════════╣                                                                                                                             ╠═════════════╣");
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
            PrintTitle();
        }

        /// <summary>
        /// Вывод таблицы игроков с подсветкой ходящего игрока
        /// </summary>
        /// <param name="turn">Номер ходящего игрока</param>
        static public void PrintPlayers(int turn)
        {
            int addWidth, addHeight;
            ConsoleColor background;
            Player player;
            for (int i = 0; i < playersCount; i++)
            {
                player = players[i];
                addWidth = 34 * (i % 2);
                addHeight = 12 * (i / 2);
                if (player.bankrupt) background = ConsoleColor.Gray;
                else background = ConsoleColor.White;
                Console.SetCursorPosition(165 + addWidth, 2 + addHeight);
                Console.BackgroundColor = background;
                Console.Write("╔═════════════════════════════╗");
                Console.SetCursorPosition(165 + addWidth, 3 + addHeight);
                Console.Write("║");
                if (i == turn) Console.BackgroundColor = ConsoleColor.Yellow;
                if (player.canselled) Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write($"{player.name} {player.piece}");
                for (int j = 0; j < 25 - player.name.Length - Convert.ToString(player.balance).Length; j++)
                {
                    Console.Write(" ");
                }
                Console.Write($"{player.balance}$ ");
                Console.BackgroundColor = background;
                Console.Write("║");
                Console.SetCursorPosition(165 + addWidth, 4 + addHeight);
                Console.Write("╠═════════════════════════════╣");
                Console.SetCursorPosition(165 + addWidth, 5 + addHeight);
                Console.Write($"║ Карты освобождения: {player.liberation}");
                for (int j = 0; j < 8 - Convert.ToString(player.liberation).Length; j++)
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
                Console.Write($" {player.yellow}/3 ");
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Write("Тёмно-жёлтая");
                Console.BackgroundColor = background;
                Console.Write($" {player.darkYellow}/3 ║");
                Console.SetCursorPosition(165 + addWidth, 8 + addHeight);
                Console.Write("║ ");
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("Зелёная");
                Console.BackgroundColor = background;
                Console.Write($" {player.green}/3   ");
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.Write("Пурпурная");
                Console.BackgroundColor = background;
                Console.Write($" {player.magenta}/3 ║");
                Console.SetCursorPosition(165 + addWidth, 9 + addHeight);
                Console.Write("║ ");
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("Синяя");
                Console.BackgroundColor = background;
                Console.Write($" {player.blue}/3 ");
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write("Тёмно-зелёная");
                Console.BackgroundColor = background;
                Console.Write($" {player.darkGreen}/3 ║");
                Console.SetCursorPosition(165 + addWidth, 10 + addHeight);
                Console.Write("║ ");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("Красная");
                Console.BackgroundColor = background;
                Console.Write($" {player.red}/3     ");
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.Write("Голубая");
                Console.BackgroundColor = background;
                Console.Write($" {player.cyan}/2 ║");
                Console.SetCursorPosition(165 + addWidth, 11 + addHeight);
                Console.Write("║ ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Чёрная");
                Console.BackgroundColor = background;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($" {player.black}/4        ");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("Серая");
                Console.BackgroundColor = background;
                Console.Write($" {player.gray}/2 ║");
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
        static public void PrintPieces(int oldPosition, int newPosition, char _piece)
        {
            int oldVisitors = quartals[oldPosition].visitors;
            int newVisitors = quartals[newPosition].visitors;
            int prisonedVisitors = players.Where(p => p.prisoned).Count();
            Player player = players[0];
            foreach (var p in players)
            {
                if (p.piece == _piece)
                {
                    player = p;
                    break;
                }
            }
            int oldPiecesPtr = 0;
            int newPiecesPtr = 1;
            int prisonerPiecesPtr = 0;
            char[] oldPieces = new char[oldVisitors];
            char[] newPieces = new char[newVisitors];
            char[] prisonerPieces = new char[prisonedVisitors];
            newPieces[0] = _piece;
            for (int i = 0; i < playersCount; i++)
            {
                if (players[i].position == oldPosition && players[i].piece != _piece && !players[i].prisoned) oldPieces[oldPiecesPtr++] = players[i].piece;
                if (players[i].position == newPosition && players[i].piece != _piece && !players[i].prisoned) newPieces[newPiecesPtr++] = players[i].piece;
                if (players[i].prisoned) prisonerPieces[prisonerPiecesPtr++] = players[i].piece;
            }

            int column, row;
            if (oldPosition <= 10)
            {
                if (oldPosition == 0)
                {
                    row = 3;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    for (int i = 0; i < oldVisitors; i++)
                    {
                        column = 7 + i;
                        Console.SetCursorPosition(column, row);
                        Console.Write(" ");
                    }
                    for (int i = 0; i < oldVisitors; i++)
                    {
                        column = 7 + i;
                        Console.SetCursorPosition(column, row);
                        Console.Write(oldPieces[i]);
                    }
                }
                else if (oldPosition == 10)
                {
                    row = 5;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    for (int i = 0; i < prisonedVisitors; i++)
                    {
                        column = 147 + i;
                        Console.SetCursorPosition(column, row);
                        Console.Write(" ");
                    }
                    for (int i = 0; i < prisonedVisitors; i++)
                    {
                        column = 147 + i;
                        Console.SetCursorPosition(column, row);
                        Console.Write(prisonerPieces[i]);
                    }
                    row = 3;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    for (int i = 0; i < oldVisitors; i++)
                    {
                        column = 147 + i;
                        Console.SetCursorPosition(column, row);
                        Console.Write(" ");
                    }
                    for (int i = 0; i < oldVisitors; i++)
                    {
                        column = 147 + i;
                        Console.SetCursorPosition(column, row);
                        Console.Write(oldPieces[i]);
                    }
                }
                else
                {
                    row = 0;
                    for (int i = 0; i < oldVisitors; i++)
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
                }
                Console.BackgroundColor = ConsoleColor.White;
            }
            else if (oldPosition < 20)
            {
                for (int i = 0; i < oldVisitors; i++)
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
                for (int i = 0; i < oldVisitors; i++)
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
                for (int i = 0; i < oldVisitors; i++)
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

            if (newPosition != -1)
            {
                if (newPosition <= 10)
                {
                    if (newPosition == 0)
                    {
                        player.balance += 200;
                        row = 3;
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        for (int i = 0; i < newVisitors; i++)
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
                    }
                    else if (newPosition == 10)
                    {
                        row = 5;
                        Console.BackgroundColor = ConsoleColor.Gray;
                        for (int i = 0; i < prisonedVisitors; i++)
                        {
                            column = 147 + i;
                            Console.SetCursorPosition(column, row);
                            Console.Write(" ");
                        }
                        for (int i = 0; i < prisonedVisitors; i++)
                        {
                            column = 147 + i;
                            Console.SetCursorPosition(column, row);
                            Console.Write(prisonerPieces[i]);
                        }
                        row = 3;
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        for (int i = 0; i < newVisitors; i++)
                        {
                            column = 147 + i;
                            Console.SetCursorPosition(column, row);
                            Console.Write(" ");
                        }
                        for (int i = 0; i < newVisitors; i++)
                        {
                            column = 147 + i;
                            Console.SetCursorPosition(column, row);
                            Console.Write(newPieces[i]);
                        }
                    }
                    else
                    {
                        row = 0;
                        for (int i = 0; i < newVisitors; i++)
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
                    }
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else if (newPosition < 20)
                {
                    for (int i = 0; i < newVisitors; i++)
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
                    for (int i = 0; i < newVisitors; i++)
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
                    for (int i = 0; i < newVisitors; i++)
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
        }

        static public void PrintTraders(int marker, Player[] traders)
        {
            int column = 25, row = 27;
            bool additionalRow = false;
            int charactersCount = -2;
            foreach (Player trader in traders)
            {
                if (charactersCount + trader.name.Length + 2 > 107) additionalRow = true;
                else charactersCount += trader.name.Length + 2;
            }
            int spaces, left, right, i;
            spaces = 106 - charactersCount;
            left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
            right = Convert.ToInt32(Math.Floor(spaces / 2.0));

            Console.SetCursorPosition(column, row++);
            Console.Write("╔═════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                               ВЫБЕРИТЕ ИГРОКА                                               ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("╠═════════════════════════════════════════════════════════════════════════════════════════════════════════════╣");
            Console.SetCursorPosition(column, row++);
            Console.Write("║ ");
            for (i = 0; i < left; i++)
            {
                Console.Write(" ");
            }
            for (i = 0; i < traders.Length - 1; i++)
            {
                Player trader = traders[i];
                if (i == marker) Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Write($"{trader.piece}{trader.name}");
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(" ");
            }
            if (additionalRow)
            {
                for (i = 0; i < right; i++)
                {
                    Console.Write(" ");
                }
                Console.Write("║");
                Console.SetCursorPosition(column, row++);
                Player trader = traders[traders.Length - 1];
                int spaces2, left2, right2;
                spaces2 = 106 - trader.name.Length;
                left2 = Convert.ToInt32(Math.Ceiling(spaces2 / 2.0));
                right2 = Convert.ToInt32(Math.Floor(spaces2 / 2.0));
                Console.Write("║ ");
                for (i = 0; i < left2; i++)
                {
                    Console.Write(" ");
                }
                if (traders.Length - 1 == marker) Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Write($"{trader.piece}{trader.name}");
                Console.BackgroundColor = ConsoleColor.White;
                for (i = 0; i < right2; i++)
                {
                    Console.Write(" ");
                }
                Console.Write(" ║");
            }
            else
            {
                Player trader = traders[traders.Length - 1];
                if (i == marker) Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Write($"{trader.piece}{trader.name}");
                Console.BackgroundColor = ConsoleColor.White;
                for (i = 0; i < right; i++)
                {
                    Console.Write(" ");
                }
                Console.Write(" ║");
            }
            Console.SetCursorPosition(column, row++);
            Console.Write("╚═════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
        }

        static public void PrintProperty(
            Player player, Player trader,
            List<Quartal> playerProperty, List<Quartal> traderProperty,
            int playerMoney, int traderMoney,
            int marker1, int marker2,
            bool ownProperty, bool? category,
            int playerLiberationCount, int traderLiberationCount)
        {
            string[] give = new string[10];
            string[] take = new string[10];
            for (int i = 0; i < 10; i++)
            {
                give[i] = "";
                take[i] = "";
            }
            int gRows = 0;
            int tRows = 0;

            if (playerProperty.Count() == 0)
            {
                if (playerMoney > 0)
                {
                    give[0] = $"{playerMoney}$";
                    gRows = 1;
                }
            }
            else
            {
                gRows = 1;
                for (int i = 0; i < playerProperty.Count() - 1; i++)
                {
                    if (give[gRows - 1].Length + playerProperty[i].name.Length + 2 > 89) gRows++;
                    give[gRows - 1] += $"{playerProperty[i].name}, ";
                }
                if (give[gRows - 1].Length + playerProperty[playerProperty.Count() - 1].name.Length > 89) gRows++;
                give[gRows - 1] += playerProperty[playerProperty.Count() - 1].name;
                if (playerMoney > 0)
                {
                    if (give[gRows - 1].Length + 2 > 89) gRows++;
                    give[gRows - 1] += " и";
                    if (give[gRows - 1].Length + Convert.ToString(playerMoney).Length + 2 > 89) gRows++;
                    give[gRows - 1] += $"{playerMoney}$";
                }
            }

            if (traderProperty.Count() == 0)
            {
                if (traderMoney > 0)
                {
                    take[0] = $"{traderMoney}$";
                    tRows = 1;
                }
            }
            else
            {
                tRows = 1;
                for (int i = 0; i < traderProperty.Count() - 1; i++)
                {
                    if (take[tRows - 1].Length + traderProperty[i].name.Length + 2 > 89) tRows++;
                    take[tRows - 1] += $"{traderProperty[i].name}, ";
                }
                if (take[tRows - 1].Length + traderProperty[traderProperty.Count() - 1].name.Length > 89) tRows++;
                take[tRows - 1] += traderProperty[traderProperty.Count() - 1].name;
                if (traderMoney > 0)
                {
                    if (take[tRows - 1].Length + 2 > 89) tRows++;
                    take[tRows - 1] += " и";
                    if (take[tRows - 1].Length + Convert.ToString(traderMoney).Length + 2 > 89) tRows++;
                    take[tRows - 1] += $" {traderMoney}$";
                }
            }

            int column = 19, row = 17;
            Console.SetCursorPosition(column, row++);
            Console.Write("╔═══════════════════════════════════════════════════════════════════════════════════════════╦═════════════════════════════╗");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                         ВЫ ОТДАЁТЕ:                                       ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║═══════════════════════════════════════════════════════════════════════════════════════════╣                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                             ЗА:                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("║                                                                                           ║                             ║");
            Console.SetCursorPosition(column, row++);
            Console.Write("╚═══════════════════════════════════════════════════════════════════════════════════════════╩═════════════════════════════╝");
            int gRow = 19;
            int tRow = 31;
            for (int i = 0; i < gRows; i++)
            {
                Console.SetCursorPosition(column + 2, gRow++);
                Console.Write(give[i]);
            }
            for (int i = 0; i < tRows; i++)
            {
                Console.SetCursorPosition(column + 2, tRow++);
                Console.Write(take[i]);
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            if (category == true)
            {
                Console.SetCursorPosition(column + 93, 29);
                Console.Write("<");
                Console.SetCursorPosition(column + 121, 29);
                Console.Write(">");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(column + 93, 40);
                Console.Write("<");
                Console.SetCursorPosition(column + 121, 40);
                Console.Write(">");
                Console.SetCursorPosition(column + 93, 39);
                Console.Write("<");
                Console.SetCursorPosition(column + 121, 39);
                Console.Write(">");
            }
            else if (category == false)
            {
                Console.SetCursorPosition(column + 93, 40);
                Console.Write("<");
                Console.SetCursorPosition(column + 121, 40);
                Console.Write(">");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(column + 93, 29);
                Console.Write("<");
                Console.SetCursorPosition(column + 121, 29);
                Console.Write(">");
                Console.SetCursorPosition(column + 93, 39);
                Console.Write("<");
                Console.SetCursorPosition(column + 121, 39);
                Console.Write(">");
            }
            else
            {
                Console.SetCursorPosition(column + 93, 39);
                Console.Write("<");
                Console.SetCursorPosition(column + 121, 39);
                Console.Write(">");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(column + 93, 40);
                Console.Write("<");
                Console.SetCursorPosition(column + 121, 40);
                Console.Write(">");
                Console.SetCursorPosition(column + 93, 29);
                Console.Write("<");
                Console.SetCursorPosition(column + 121, 29);
                Console.Write(">");
            }
            Console.SetCursorPosition(column + 93, 18);
            int spaces, left, right;
            if (ownProperty)
            {
                Console.Write("        ВАШЕ ИМУЩЕСТВО       ");
                if (marker1 == -1)
                {
                    PrintCard(null, 113, 20);
                }
                else
                {
                    PrintCard(player.property[marker1], 113, 20);
                }
                Console.SetCursorPosition(column + 94, 38);
                Console.Write("          Карточки         ");
                Console.SetCursorPosition(column + 94, 39);
                Console.Write($"      освобождения: {playerLiberationCount}      ");
                Console.SetCursorPosition(column + 94, 40);
                spaces = 26 - Convert.ToString(playerMoney).Length;
                left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                for (int i = 0; i < left; i++) Console.Write(" ");
                Console.Write($"{playerMoney}$");
                for (int i = 0; i < right; i++) Console.Write(" ");
            }
            else
            {
                spaces = 19 - trader.name.Length;
                left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                for (int i = 0; i < left; i++) Console.Write(" ");
                Console.Write($"ИМУЩЕСТВО {trader.name}");
                for (int i = 0; i < right; i++) Console.Write(" ");
                if (marker2 == -1)
                {
                    PrintCard(null, 113, 20);
                }
                else
                {
                    PrintCard(trader.property[marker2], 113, 20);
                }
                Console.SetCursorPosition(column + 94, 38);
                Console.Write("          Карточки         ");
                Console.SetCursorPosition(column + 94, 39);
                Console.Write($"      освобождения: {traderLiberationCount}      ");
                Console.SetCursorPosition(column + 94, 40);
                spaces = 26 - Convert.ToString(traderMoney).Length;
                left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                for (int i = 0; i < left; i++) Console.Write(" ");
                Console.Write($"{traderMoney}$");
                for (int i = 0; i < right; i++) Console.Write(" ");
            }
        }

        /// <summary>
        /// Предложение обмена от игрока1 к игроку2
        /// </summary>
        /// <param name="player">Игрок, который предлагает обмен</param>
        /// <returns></returns>
        static public void Trade(Player player)
        {
            Player[] traders = players.Where(trader => !trader.Equals(player)).ToArray();
            int marker = 0;
            bool trading = true;
            while (trading)
            {
                ClearMenu();
                Console.SetCursorPosition(165, 50);
                Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                Console.SetCursorPosition(165, 51);
                Console.Write("║                             Обмен                             ║");
                Console.SetCursorPosition(165, 52);
                Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                Console.SetCursorPosition(165, 53);
                Console.Write("║ AD – Выбрать игрока           Enter – Предложить игроку обмен ║");
                Console.SetCursorPosition(165, 54);
                if (musicMuted)
                {
                    Console.Write("║ Backspace – Отменить сделку               M – Включить музыку ║");
                }
                else
                {
                    Console.Write("║ Backspace – Отменить сделку              M – Заглушить музыку ║");
                }
                Console.SetCursorPosition(165, 55);
                Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                PrintTraders(marker, traders);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.A:
                        marker--;
                        if (marker == -1) marker = 0;
                        PrintTraders(marker, traders);
                        break;
                    case ConsoleKey.D:
                        marker++;
                        if (marker == traders.Length) marker--;
                        PrintTraders(marker, traders);
                        break;
                    case ConsoleKey.Enter:
                        bool trading2 = true;
                        bool ownProperty = true;
                        Player trader = traders[marker];
                        List<Quartal> playerProperty = new List<Quartal>();
                        List<Quartal> traderProperty = new List<Quartal>();
                        int yourMoney = 0;
                        int traderMoney = 0;
                        bool? category = true;
                        int marker1 = -1;
                        int marker2 = -1;
                        int playerPropertyCount = player.property.Count(p => p != null);
                        int traderPropertyCount = trader.property.Count(p => p != null);
                        int playerLiberationCount = 0;
                        int traderLiberationCount = 0;

                        for (int i = 0; i < playerPropertyCount; i++)
                        {
                            if (player.property[i].level == 0) marker1 = i;
                        }
                        for (int i = 0; i < traderPropertyCount; i++)
                        {
                            if (trader.property[i].level == 0) marker2 = i;
                        }
                        
                        Console.SetCursorPosition(165, 53);
                        Console.Write("║ AD – Выбрать улицу/количество денег WS – Выбрать тип имущества║");
                        Console.SetCursorPosition(165, 54);
                        Console.Write("║ Enter – Подтвердить сделку         Пробел – Подтвердить улицу ║");
                        Console.SetCursorPosition(165, 55);
                        Console.Write("║ Esc – Отменить сделку                       Backspace – Назад ║");
                        while (trading2)
                        {
                            Console.SetCursorPosition(165, 56);
                            if (musicMuted)
                            {
                                Console.Write("║                                           M – Включить музыку ║");
                            }
                            else
                            {
                                Console.Write("║                                          M – Заглушить музыку ║");
                            }
                            Console.SetCursorPosition(167, 56);
                            if (ownProperty) Console.Write($"P – Выбрать имущество \"{trader.name}\"");
                            else Console.Write("P – Выбрать Ваше имущество");
                            Console.SetCursorPosition(165, 57);
                            Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                            PrintProperty(player,
                                          trader,
                                          playerProperty,
                                          traderProperty,
                                          yourMoney,
                                          traderMoney,
                                          marker1,
                                          marker2,
                                          ownProperty,
                                          category,
                                          playerLiberationCount,
                                          traderLiberationCount);
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.A:
                                    if (category == true)
                                    {
                                        bool notFound = true;
                                        if (ownProperty)
                                        {
                                            if (marker1 != -1)
                                            {
                                                for (int i = marker1 - 1; i > 0 && notFound; i--)
                                                {
                                                    if (player.property[i].level == 0)
                                                    {
                                                        marker1 = i;
                                                        notFound = false;
                                                    }
                                                }
                                                for (int i = playerPropertyCount - 1; i > marker1 && notFound; i--)
                                                {
                                                    if (player.property[i].level == 0)
                                                    {
                                                        marker1 = i;
                                                        notFound = false;
                                                    }
                                                }
                                            }
                                        }
                                        else if (category == false)
                                        {
                                            if (marker2 != -1)
                                            {
                                                for (int i = marker2 - 1; i > 0 && notFound; i--)
                                                {
                                                    if (trader.property[i].level == 0)
                                                    {
                                                        marker2 = i;
                                                        notFound = false;
                                                    }
                                                }
                                                for (int i = traderPropertyCount - 1; i > marker2 && notFound; i--)
                                                {
                                                    if (trader.property[i].level == 0)
                                                    {
                                                        marker2 = i;
                                                        notFound = false;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (ownProperty)
                                            {
                                                if (playerLiberationCount != 0) playerLiberationCount--;
                                            }
                                            else
                                            {
                                                if (traderLiberationCount != 0) traderLiberationCount--;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ownProperty)
                                        {
                                            yourMoney--;
                                            if (yourMoney == -1) yourMoney = 0;
                                        }
                                        else
                                        {
                                            traderMoney--;
                                            if (traderMoney == -1) traderMoney = 0;
                                        }
                                    }
                                    break;
                                case ConsoleKey.D:
                                    if (category == true)
                                    {
                                        bool notFound = true;
                                        if (ownProperty)
                                        {
                                            for (int i = marker1 + 1; i < playerPropertyCount && notFound; i++)
                                            {
                                                if (player.property[i].level == 0)
                                                {
                                                    marker1 = i;
                                                    notFound = false;
                                                }
                                            }
                                            for (int i = 0; i < marker1 && notFound; i++)
                                            {
                                                if (player.property[i].level == 0)
                                                {
                                                    marker1 = i;
                                                    notFound = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int i = marker2 + 1; i < traderPropertyCount && notFound; i++)
                                            {
                                                if (trader.property[i].level == 0)
                                                {
                                                    marker2 = i;
                                                    notFound = false;
                                                }
                                            }
                                            for (int i = 0; i < marker2 && notFound; i++)
                                            {
                                                if (trader.property[i].level == 0)
                                                {
                                                    marker2 = i;
                                                    notFound = false;
                                                }
                                            }
                                        }
                                    }
                                    else if (category == false)
                                    {
                                        if (ownProperty)
                                        {
                                            yourMoney++;
                                            if (yourMoney > player.balance) yourMoney = player.balance;
                                        }
                                        else
                                        {
                                            traderMoney++;
                                            if (traderMoney > trader.balance) traderMoney = trader.balance;
                                        }
                                    }
                                    else
                                    {
                                        if (ownProperty)
                                        {
                                            if (playerLiberationCount < trader.liberation) playerLiberationCount++;
                                        }
                                        else
                                        {
                                            if (traderLiberationCount < trader.liberation) traderLiberationCount++;
                                        }
                                    }
                                    break;
                                case ConsoleKey.W:
                                    if (category == false) category = null;
                                    else if (category == null) category = true;
                                    break;
                                case ConsoleKey.S:
                                    if (category == true) category = null;
                                    else if (category == null) category = false;
                                    break;
                                case ConsoleKey.Enter:
                                    int index = 0;
                                    for (; index < players.Length; index++)
                                    {
                                        if (players[index] == trader) break;
                                    }
                                    PrintPlayers(index);
                                    ClearMenu();
                                    Console.SetCursorPosition(165, 50);
                                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                    Console.SetCursorPosition(165, 51);
                                    int spaces, left, right;
                                    spaces = 34 - trader.name.Length;
                                    left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                                    right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                                    Console.Write("║ ");
                                    for (int i = 0; i < left; i++) Console.Write(" ");
                                    Console.Write($"{trader.name}, примите ли Вы эту сделку?");
                                    for (int i = 0; i < right; i++) Console.Write(" ");
                                    Console.Write(" ║");
                                    Console.SetCursorPosition(165, 52);
                                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                    Console.SetCursorPosition(165, 53);
                                    Console.Write("║ Enter – Принять сделку                  Esc – Отменить сделку ║");
                                    while (trading)
                                    {
                                        Console.SetCursorPosition(165, 54);
                                        if (musicMuted)
                                        {
                                            Console.Write("║                      M – Включить музыку                      ║");
                                        }
                                        else
                                        {
                                            Console.Write("║                      M – Заглушить музыку                     ║");
                                        }
                                        Console.SetCursorPosition(165, 55);
                                        Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                        switch (Console.ReadKey(true).Key)
                                        {
                                            case ConsoleKey.Enter:
                                                player.balance -= yourMoney;
                                                trader.balance -= traderMoney;
                                                player.property = player.property.Concat(traderProperty).Except(playerProperty).ToList<Quartal>();
                                                trader.property = trader.property.Concat(playerProperty).Except(traderProperty).ToList<Quartal>();
                                                foreach (Quartal p in playerProperty)
                                                {
                                                    p.owner = trader;
                                                    IncreaseColor(p, trader);
                                                    DecreaseColor(p, player);
                                                }
                                                foreach (Quartal p in traderProperty)
                                                {
                                                    p.owner = player;
                                                    IncreaseColor(p, player);
                                                    DecreaseColor(p, trader);
                                                }
                                                trading = false;
                                                trading2 = false;
                                                break;
                                            case ConsoleKey.Escape:
                                                trading = false;
                                                trading2 = false;
                                                break;
                                            case ConsoleKey.M:
                                                if (musicMuted)
                                                {
                                                    musicMuted = false;
                                                    musicPlayer.PlayLooping();
                                                }
                                                else
                                                {
                                                    musicMuted = true;
                                                    musicPlayer.Stop();
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case ConsoleKey.Spacebar:
                                    if (category == true)
                                    {
                                        if (ownProperty)
                                        {
                                            if (marker1 != -1)
                                            {
                                                Quartal quartal = player.property[marker1];
                                                if (playerProperty.Contains(quartal))
                                                {
                                                    playerProperty.Remove(quartal);
                                                }
                                                else
                                                {
                                                    playerProperty.Add(quartal);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (marker2 != -1)
                                            {
                                                Quartal quartal = trader.property[marker2];
                                                if (traderProperty.Contains(quartal))
                                                {
                                                    traderProperty.Remove(quartal);
                                                }
                                                else
                                                {
                                                    traderProperty.Add(quartal);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case ConsoleKey.Escape:
                                    ClearMenu();
                                    PrintTitle();
                                    trading = false;
                                    trading2 = false;
                                    break;
                                case ConsoleKey.Backspace:
                                    PrintTitle();
                                    trading2 = false;
                                    break;
                                case ConsoleKey.P:
                                    ownProperty = !ownProperty;
                                    break;
                                case ConsoleKey.M:
                                    if (musicMuted)
                                    {
                                        musicMuted = false;
                                        musicPlayer.PlayLooping();
                                    }
                                    else
                                    {
                                        musicMuted = true;
                                        musicPlayer.Stop();
                                    }
                                    break;
                            }
                        }
                        break;
                    case ConsoleKey.M:
                        if (musicMuted)
                        {
                            musicMuted = false;
                            musicPlayer.PlayLooping();
                        }
                        else
                        {
                            musicMuted = true;
                            musicPlayer.Stop();
                        }
                        break;
                    case ConsoleKey.Backspace:
                        ClearMenu();
                        PrintTitle();
                        trading = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Метод вывода карточки города посередине поля
        /// </summary>
        /// <param name="quartal">Город, карточка которого, будет выводиться</param>
        static public void PrintCard(Quartal quartal)
        {
            int row, i, spaces, left, right;
            string part1 = "", part2 = "";
            switch (quartal.special) {
                case 6:
                    row = 23;
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╔═════════════════════════╗");
                    for (i = 0; quartal.name[i] != ' '; i++)
                    {
                        part1 += quartal.name[i];
                    }
                    for (i++; i < quartal.name.Length; i++)
                    {
                        part2 += quartal.name[i];
                    }
                    Console.SetCursorPosition(69, row++);
                    spaces = 25 - part1.Length;
                    left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                    right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                    Console.Write("║");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    for (i = 0; i < left; i++) Console.Write(" ");
                    Console.Write(part1);
                    for (i = 0; i < right; i++) Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("║");
                    Console.SetCursorPosition(69, row++);
                    spaces = 25 - part2.Length;
                    left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                    right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                    Console.Write("║");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    for (i = 0; i < left; i++) Console.Write(" ");
                    Console.Write(part2);
                    for (i = 0; i < right; i++) Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╠═════════════════════════╣");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║   ПРАВО СОБСТВЕННОСТИ   ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║                       $ ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ 1 порт               25 ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ 2 порта              50 ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ 3 порта             100 ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ 4 порта             200 ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╠═════════════════════════╣");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ Закладная           100 ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╚═════════════════════════╝");
                    break;
                case 7:
                    row = 22;
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╔═════════════════════════╗");
                    spaces = 25 - quartal.name.Length;
                    left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                    right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║");
                    Console.BackgroundColor = ConsoleColor.Gray;
                    for (int j = 0; j < left; j++) Console.Write(" ");
                    Console.Write(quartal.name);
                    for (int j = 0; j < right; j++) Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╠═════════════════════════╣");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║   ПРАВО СОБСТВЕННОСТИ   ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ Если у игрока одно      ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ \"Коммунальное           ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ предприятие\", аренда    ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ составляет 4 кратную    ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ сумму очков, выпавших   ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ на кубиках.             ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ Если два, то аренда     ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ 10 кратна сумме очков   ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ на кубиках.             ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╠═════════════════════════╣");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ Закладная           75$ ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╚═════════════════════════╝");
                    break;
                default:
                    row = 20;
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╔═════════════════════════╗");
                    spaces = 25 - quartal.name.Length;
                    left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                    right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║");
                    Console.BackgroundColor = quartal.color;
                    for (int j = 0; j < left; j++) Console.Write(" ");
                    Console.Write(quartal.name);
                    for (int j = 0; j < right; j++) Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╠═════════════════════════╣");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║   ПРАВО СОБСТВЕННОСТИ   ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║                       $ ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ Без цветовой группы ");
                    if (quartal.noMonopolyRent < 100) Console.Write(" ");
                    if (quartal.noMonopolyRent < 10) Console.Write(" ");
                    Console.Write($"{quartal.noMonopolyRent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ С цветовой группой  ");
                    if (quartal.monopolyRent < 100) Console.Write(" ");
                    if (quartal.monopolyRent < 10) Console.Write(" ");
                    Console.Write($"{quartal.monopolyRent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ 1 дом               ");
                    if (quartal.house1Rent < 100) Console.Write(" ");
                    Console.Write($"{quartal.house1Rent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ 2 дома              ");
                    if (quartal.house2Rent < 100) Console.Write(" ");
                    Console.Write($"{quartal.house2Rent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ 3 дома             ");
                    if (quartal.house3Rent < 1000) Console.Write(" ");
                    if (quartal.house3Rent < 100) Console.Write(" ");
                    Console.Write($"{quartal.house3Rent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ 4 дома             ");
                    if (quartal.house4Rent < 1000) Console.Write(" ");
                    if (quartal.house4Rent < 100) Console.Write(" ");
                    Console.Write($"{quartal.house4Rent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ Отель              ");
                    if (quartal.hotelRent < 1000) Console.Write(" ");
                    if (quartal.hotelRent < 100) Console.Write(" ");
                    Console.Write($"{quartal.hotelRent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╠═════════════════════════╣");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ Постройка дома      ");
                    if (quartal.houseCost < 100) Console.Write(" ");
                    Console.Write($"{quartal.houseCost} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ Постройка отеля     ");
                    if (quartal.houseCost < 100) Console.Write(" ");
                    Console.Write($"{quartal.houseCost} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║                 +4 дома ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ Закладная           ");
                    if (quartal.pledge < 100) Console.Write(" ");
                    Console.Write($"{quartal.pledge} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╚═════════════════════════╝");
                    break;
            }
        }

        /// <summary>
        /// Метод вывода карточки города, начиная с указанных строки и столбца
        /// </summary>
        /// <param name="quartal">Город, карточка которого, будет выводиться</param>
        /// <param name="column">Столбец, начиная с котороого будет выводиться карточка</param>
        /// <param name="row">Строка, начиная с которой будет выводиться карточка</param>
        static public void PrintCard(Quartal quartal, int column, int row)
        {
            int i, spaces, left, right;
            string part1 = "", part2 = "";
            if (quartal == null)
            {
                Console.SetCursorPosition(column, row++);
                Console.Write("╔═════════════════════════╗");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║      ИГРОК НЕ ИМЕЕТ     ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║        ИМУЩЕСТВА        ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("║                         ║");
                Console.SetCursorPosition(column, row++);
                Console.Write("╚═════════════════════════╝");
            }
            else
            {
                switch (quartal.special)
                {
                    case 6:
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╔═════════════════════════╗");
                        for (i = 0; quartal.name[i] != ' '; i++)
                        {
                            part1 += quartal.name[i];
                        }
                        for (i++; i < quartal.name.Length; i++)
                        {
                            part2 += quartal.name[i];
                        }
                        Console.SetCursorPosition(column, row++);
                        spaces = 25 - part1.Length;
                        left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                        right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                        Console.Write("║");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        for (i = 0; i < left; i++) Console.Write(" ");
                        Console.Write(part1);
                        for (i = 0; i < right; i++) Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("║");
                        Console.SetCursorPosition(column, row++);
                        spaces = 25 - part2.Length;
                        left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                        right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                        Console.Write("║");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        for (i = 0; i < left; i++) Console.Write(" ");
                        Console.Write(part2);
                        for (i = 0; i < right; i++) Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╠═════════════════════════╣");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║   ПРАВО СОБСТВЕННОСТИ   ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║                       $ ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ 1 порт               25 ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ 2 порта              50 ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ 3 порта             100 ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ 4 порта             200 ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╠═════════════════════════╣");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ Закладная           100 ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╚═════════════════════════╝");
                        break;
                    case 7:
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╔═════════════════════════╗");
                        spaces = 25 - quartal.name.Length;
                        left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                        right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        for (int j = 0; j < left; j++) Console.Write(" ");
                        Console.Write(quartal.name);
                        for (int j = 0; j < right; j++) Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╠═════════════════════════╣");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║   ПРАВО СОБСТВЕННОСТИ   ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ Если у игрока одно      ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ \"Коммунальное           ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ предприятие\", аренда    ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ составляет 4 кратную    ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ сумму очков, выпавших   ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ на кубиках.             ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ Если два, то аренда     ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ 10 кратна сумме очков   ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ на кубиках.             ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╠═════════════════════════╣");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ Закладная           75$ ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╚═════════════════════════╝");
                        break;
                    default:
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╔═════════════════════════╗");
                        spaces = 25 - quartal.name.Length;
                        left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                        right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║");
                        Console.BackgroundColor = quartal.color;
                        for (int j = 0; j < left; j++) Console.Write(" ");
                        Console.Write(quartal.name);
                        for (int j = 0; j < right; j++) Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╠═════════════════════════╣");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║   ПРАВО СОБСТВЕННОСТИ   ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║                       $ ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ Без цветовой группы ");
                        if (quartal.noMonopolyRent < 100) Console.Write(" ");
                        if (quartal.noMonopolyRent < 10) Console.Write(" ");
                        Console.Write($"{quartal.noMonopolyRent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ С цветовой группой  ");
                        if (quartal.monopolyRent < 100) Console.Write(" ");
                        if (quartal.monopolyRent < 10) Console.Write(" ");
                        Console.Write($"{quartal.monopolyRent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ 1 дом               ");
                        if (quartal.house1Rent < 100) Console.Write(" ");
                        Console.Write($"{quartal.house1Rent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ 2 дома              ");
                        if (quartal.house2Rent < 100) Console.Write(" ");
                        Console.Write($"{quartal.house2Rent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ 3 дома             ");
                        if (quartal.house3Rent < 1000) Console.Write(" ");
                        if (quartal.house3Rent < 100) Console.Write(" ");
                        Console.Write($"{quartal.house3Rent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ 4 дома             ");
                        if (quartal.house4Rent < 1000) Console.Write(" ");
                        if (quartal.house4Rent < 100) Console.Write(" ");
                        Console.Write($"{quartal.house4Rent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ Отель              ");
                        if (quartal.hotelRent < 1000) Console.Write(" ");
                        if (quartal.hotelRent < 100) Console.Write(" ");
                        Console.Write($"{quartal.hotelRent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╠═════════════════════════╣");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ Постройка дома      ");
                        if (quartal.houseCost < 100) Console.Write(" ");
                        Console.Write($"{quartal.houseCost} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ Постройка отеля     ");
                        if (quartal.houseCost < 100) Console.Write(" ");
                        Console.Write($"{quartal.houseCost} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║                 +4 дома ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ Закладная           ");
                        if (quartal.pledge < 100) Console.Write(" ");
                        Console.Write($"{quartal.pledge} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╚═════════════════════════╝");
                        break;
                }
            }
        }

        /// <summary>
        /// Печатает надпись "Monopoly" посередине игрового поля
        /// </summary>
        static public void PrintTitle()
        {
            int column = 18, row = 16;
            for (int i = 0; i < 11; i++)
            {
                Console.SetCursorPosition(column, row++);
                Console.Write("                                                                                                                             ");
            }
            Console.SetCursorPosition(column, row++);
            Console.Write("                   ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" __    __     ______     __   __     ______     ______   ______     __         __  __   ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("                  ");
            Console.SetCursorPosition(column, row++);
            Console.Write("                   ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("/\\ \"-./  \\   /\\  __ \\   /\\ \"-.\\ \\   /\\  __ \\   /\\  == \\ /\\  __ \\   /\\ \\       /\\ \\_\\ \\  ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("                  ");
            Console.SetCursorPosition(column, row++);
            Console.Write("                   ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\\ \\ \\-./\\ \\  \\ \\ \\/\\ \\  \\ \\ \\-.  \\  \\ \\ \\/\\ \\  \\ \\  _-/ \\ \\ \\/\\ \\  \\ \\ \\____  \\ \\____ \\ ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("                  ");
            Console.SetCursorPosition(column, row++);
            Console.Write("                   ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" \\ \\_\\ \\ \\_\\  \\ \\_____\\  \\ \\_\\\\\"\\_\\  \\ \\_____\\  \\ \\_\\    \\ \\_____\\  \\ \\_____\\  \\/\\_____\\");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("                  ");
            Console.SetCursorPosition(column, row++);
            Console.Write("                   ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("  \\/_/  \\/_/   \\/_____/   \\/_/ \\/_/   \\/_____/   \\/_/     \\/_____/   \\/_____/   \\/_____/");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("                  ");
            for (int i = 0; i < 10; i++)
            {
                Console.SetCursorPosition(column, row++);
                Console.Write("                                                                                                                             ");
            }
        }

        /// <summary>
        /// Очищает место, где должно быть меню (заполняет пробелами)
        /// </summary>
        static public void ClearMenu()
        {
            Console.SetCursorPosition(165, 50);
            Console.Write("                                                                 ");
            Console.SetCursorPosition(165, 51);
            Console.Write("                                                                 ");
            Console.SetCursorPosition(165, 52);
            Console.Write("                                                                 ");
            Console.SetCursorPosition(165, 53);
            Console.Write("                                                                 ");
            Console.SetCursorPosition(165, 54);
            Console.Write("                                                                 ");
            Console.SetCursorPosition(165, 55);
            Console.Write("                                                                 ");
            Console.SetCursorPosition(165, 56);
            Console.Write("                                                                 ");
            Console.SetCursorPosition(165, 57);
            Console.Write("                                                                 ");
        }

        static public void PrintRealty(Player player)
        {

        }

        static public void IncreaseColor(Quartal quartal, Player player)
        {
            switch (quartal.color)
            {
                case ConsoleColor.Black:
                    player.black++;
                    break;
                case ConsoleColor.Gray:
                    player.gray++;
                    break;
                case ConsoleColor.Yellow:
                    player.yellow++;
                    break;
                case ConsoleColor.DarkYellow:
                    player.darkYellow++;
                    break;
                case ConsoleColor.DarkGreen:
                    player.darkGreen++;
                    break;
                case ConsoleColor.Green:
                    player.green++;
                    break;
                case ConsoleColor.Red:
                    player.red++;
                    break;
                case ConsoleColor.Magenta:
                    player.magenta++;
                    break;
                case ConsoleColor.Blue:
                    player.blue++;
                    break;
                case ConsoleColor.Cyan:
                    player.cyan++;
                    break;
            }
        }

        static public void DecreaseColor(Quartal quartal, Player player)
        {
            switch (quartal.color)
            {
                case ConsoleColor.Black:
                    player.black--;
                    break;
                case ConsoleColor.Gray:
                    player.gray--;
                    break;
                case ConsoleColor.Yellow:
                    player.yellow--;
                    break;
                case ConsoleColor.DarkYellow:
                    player.darkYellow--;
                    break;
                case ConsoleColor.DarkGreen:
                    player.darkGreen--;
                    break;
                case ConsoleColor.Green:
                    player.green--;
                    break;
                case ConsoleColor.Red:
                    player.red--;
                    break;
                case ConsoleColor.Magenta:
                    player.magenta--;
                    break;
                case ConsoleColor.Blue:
                    player.blue--;
                    break;
                case ConsoleColor.Cyan:
                    player.cyan--;
                    break;
            }
        }

        static public int Rent(Quartal quartal, int dicePoints)
        {
            bool isMonopoly = false;
            Player owner = quartal.owner;
            switch (quartal.color)
            {
                case ConsoleColor.Yellow:
                    if (owner.yellow == 2) isMonopoly = true;
                    break;
                case ConsoleColor.DarkYellow:
                    if (owner.darkYellow == 3) isMonopoly = true;
                    break;
                case ConsoleColor.DarkGreen:
                    if (owner.darkGreen == 3) isMonopoly = true;
                    break;
                case ConsoleColor.Green:
                    if (owner.green == 3) isMonopoly = true;
                    break;
                case ConsoleColor.Red:
                    if (owner.red == 3) isMonopoly = true;
                    break;
                case ConsoleColor.Magenta:
                    if (owner.magenta == 3) isMonopoly = true;
                    break;
                case ConsoleColor.Blue:
                    if (owner.blue == 3) isMonopoly = true;
                    break;
                case ConsoleColor.Cyan:
                    if (owner.cyan == 2) isMonopoly = true;
                    break;
            }

            if (isMonopoly)
            {
                if (quartal.color == ConsoleColor.Black)
                {
                    switch (owner.black)
                    {
                        case 1:
                            return 25;
                        case 2:
                            return 50;
                        case 3:
                            return 100;
                        default:
                            return 200;
                    }
                }
                else if (quartal.color == ConsoleColor.Gray)
                {
                    if (owner.gray == 1) return 4 * dicePoints;
                    else return 10 * dicePoints;
                }
                else
                {
                    switch (quartal.level)
                    {
                        case 0:
                            return quartal.house1Rent;
                        case 1:
                            return quartal.house1Rent;
                        case 2:
                            return quartal.house2Rent;
                        case 3:
                            return quartal.house3Rent;
                        case 4:
                            return quartal.house4Rent;
                        default:
                            return quartal.hotelRent;
                    }
                }
            }
            else return quartal.noMonopolyRent;
        }

        static public void BuyOrAuctionOrRent(Player player, Quartal quartal, int turn)
        {
            PrintCard(quartal);
            if (quartal.owner == player)
            {
                ClearMenu();
                Console.SetCursorPosition(165, 50);
                Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                Console.SetCursorPosition(165, 51);
                Console.Write("║                Купить или выставить на аукцион?               ║");
                Console.SetCursorPosition(165, 52);
                Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                Console.SetCursorPosition(165, 53);
                Console.Write("║ ");
                if (quartal.cost > player.balance) Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Пробел – Купить");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("                  Enter – Выставить на аукцион ║");
                Console.SetCursorPosition(165, 54);
                if (musicMuted)
                {
                    Console.Write("║                      M – Включить музыку                      ║");
                }
                else
                {
                    Console.Write("║                      M – Заглушить музыку                     ║");
                }
                Console.SetCursorPosition(165, 55);
                Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                while (true)
                {
                    
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Spacebar:
                            if (quartal.cost <= player.balance)
                            {
                                Purchase(player, quartal);
                                PrintTitle();
                                PrintPlayers(turn);
                            }
                            break;
                        case ConsoleKey.Enter:
                            PrintTitle();
                            Auction(quartal);
                            break;
                        case ConsoleKey.M:
                            if (musicMuted)
                            {
                                musicMuted = false;
                                musicPlayer.PlayLooping();
                                Console.SetCursorPosition(165, 54);
                                Console.Write("║                      M – Заглушить музыку                     ║");
                            }
                            else
                            {
                                musicMuted = true;
                                musicPlayer.Stop();
                                Console.SetCursorPosition(165, 54);
                                Console.Write("║                      M – Включить музыку                      ║");
                            }
                            break;
                    }
                }
            }
            else
            {
                if (!quartal.isMantaged)
                {
                    ClearMenu();
                    int rent = Rent(quartal, dice1 + dice2);
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║");
                    Console.SetCursorPosition(185, 51);
                    Console.Write($"Заплатите ренту: {rent}");
                    Console.SetCursorPosition(229, 51);
                    Console.Write("║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 53);
                    Console.Write("║ ");
                    if (rent > player.balance) Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"Пробел – Заплатить {rent}");
                    Console.ForegroundColor = ConsoleColor.Black;
                    if (musicMuted)
                    {
                        Console.SetCursorPosition(211, 53);
                        Console.Write("M – Включить музыку ║");
                    }
                    else
                    {
                        Console.SetCursorPosition(210, 53);
                        Console.Write("M – Заглушить музыку ║");
                    }
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Enter:
                            if (quartal.cost <= player.balance)
                            {
                                player.balance -= rent;
                                PrintTitle();
                                PrintPlayers(turn);
                            }
                            break;
                        case ConsoleKey.M:
                            if (musicMuted)
                            {
                                musicMuted = false;
                                musicPlayer.PlayLooping();
                            }
                            else
                            {
                                musicMuted = true;
                                musicPlayer.Stop();
                            }
                            break;
                    }
                }
            }
        }

        static public bool ThrowDices(Player player, int turn)
        {
            ClearMenu();
            dice1 = rnd.Next(1, 7);
            dice2 = rnd.Next(1, 7);
            if (player.prisoned)
            {
                if (dice1 != dice2)
                {
                    PrintDices(dice1, dice2, 0);
                    player.escapeAttempts++;
                    Thread.Sleep(1500);
                    if (player.escapeAttempts == 3)
                    {
                        Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                        Console.SetCursorPosition(165, 51);
                        Console.Write("║                              МЕНЮ                             ║");
                        Console.SetCursorPosition(165, 52);
                        Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                        Console.SetCursorPosition(165, 53);
                        Console.Write("║ ");
                        if (player.balance < 50) Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("P – Заплатить 50$ за выход");
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("      Tab – Посмотреть недвижимость ║");
                        Console.SetCursorPosition(165, 54);
                        if (musicMuted)
                        {
                            Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                        }
                        Console.SetCursorPosition(165, 55);
                        Console.Write("║                 End – Объявить себя банкротом                 ║");
                        Console.SetCursorPosition(165, 56);
                        Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                        bool notEntered = true;
                        while (notEntered)
                        {
                            while (Console.KeyAvailable)
                            {
                                switch (Console.ReadKey().Key) {
                                    case ConsoleKey.T:
                                        Trade(player);
                                        break;
                                    case ConsoleKey.Tab:
                                        PrintRealty(player);
                                        break;
                                    case ConsoleKey.M:
                                        if (musicMuted)
                                        {
                                            musicMuted = false;
                                            musicPlayer.PlayLooping();
                                            Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                                        }
                                        else
                                        {
                                            musicMuted = true;
                                            musicPlayer.Stop();
                                            Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                                        }
                                        break;
                                    case ConsoleKey.P:
                                        if (player.prisoned)
                                        {
                                            player.prisoned = false;
                                            player.balance -= 50;
                                            Menu(player, turn);
                                        }
                                        break;
                                    case ConsoleKey.End:
                                        player.Bankrupt(null);
                                        int liberation = player.liberation;
                                        for (int i = 0; i < liberation; i++)
                                        {
                                            if (treasuries.Count == 15)
                                            {
                                                treasuries.Add(4);
                                                player.liberation--;
                                            }
                                            else
                                            {
                                                chances.Add(4);
                                                player.liberation--;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    return false;
                }
                player.prisoned = false;
                player.escapeAttempts = 0;
            }
            if (dice1 == dice2)
            {
                doubles++;
                if (doubles == 3)
                {
                    player.prisoned = true;
                    quartals[10].visitors++;
                    PrintPieces(30, 10, player.piece);
                    quartals[30].visitors--;
                    player.position = 10;
                    doubles = 0;
                    policePlayer.Position = TimeSpan.Zero;
                    policePlayer.Play();
                    return false;
                }
                else
                {
                    doubled = true;
                }
            }
            else doubles = 0;
            PrintDices(dice1, dice2, doubles);
            return true;
        }

        /// <summary>
        /// Вывод меню действий
        /// </summary>
        /// <param name="player">Ходящий игрок</param>
        static public void Menu(Player player, int turn, bool skip = false)
        {
            bool turnNotEnded = true;
            if (skip)
            {
                ClearMenu();
                Console.SetCursorPosition(165, 50);
                Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                Console.SetCursorPosition(165, 51);
                Console.Write("║                              МЕНЮ                             ║");
                Console.SetCursorPosition(165, 52);
                Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                Console.SetCursorPosition(165, 53);
                if (doubled)
                {
                    Console.Write("║ Пробел – Бросить кубики         Tab – Посмотреть недвижимость ║");
                }
                else
                {
                    Console.Write("║ Enter – Закончить ход           Tab – Посмотреть недвижимость ║");
                }
                Console.SetCursorPosition(165, 54);
                if (musicMuted)
                {
                    Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                }
                else
                {
                    Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                }
                Console.SetCursorPosition(165, 55);
                if (player.prisoned)
                {
                    Console.Write("║                   P – Заплатить 50$ за выход                  ║");
                    Console.SetCursorPosition(165, 56);
                }
                Console.Write("╚═══════════════════════════════════════════════════════════════╝");
            }
            while (turnNotEnded)
            {
                while (Console.KeyAvailable)
                {
                    ConsoleKey key;
                    if (skip) key = ConsoleKey.Spacebar;
                    else key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.Spacebar:
                            turnNotEnded = ThrowDices(player, turn);
                            int oldPosition = player.position;
                            int newPosition = (oldPosition + dice1 + dice2) % 40;
                            if (!player.prisoned)
                            {
                                for (int j = oldPosition; j != newPosition;)
                                {
                                    int next = (j + 1) % 40;
                                    quartals[next].visitors++;
                                    PrintPieces(j, next, player.piece);
                                    quartals[j].visitors--;
                                    Thread.Sleep(sleep);
                                    j = next;
                                }
                                Quartal quartal = quartals[newPosition];
                                player.position = newPosition;
                                if (quartal.special == 1)
                                {
                                    Quartal newQuartal = Treasury(player);
                                    if (player.prisoned)
                                    {
                                        turnNotEnded = false;
                                        break;
                                    }
                                    if (newQuartal != null)
                                    {
                                        BuyOrAuctionOrRent(player, newQuartal, turn);
                                        Menu(player, turn);
                                        turnNotEnded = false;
                                        break;
                                    }
                                }
                                else if (quartal.special == 2)
                                {
                                    Quartal newQuartal = Chance(player);
                                    if (player.prisoned) break;
                                    if (newQuartal != null)
                                    {
                                        BuyOrAuctionOrRent(player, newQuartal, turn);
                                        Menu(player, turn);
                                        break;
                                    }
                                }
                                else if (quartal.special == 3)
                                {
                                    while (turnNotEnded)
                                    {
                                        ClearMenu();
                                        Console.SetCursorPosition(165, 50);
                                        Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                        Console.SetCursorPosition(165, 51);
                                        Console.Write("║      Вам пришла мысль проведать знакомого. Вы посетитель      ║");
                                        Console.SetCursorPosition(165, 52);
                                        Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                        Console.SetCursorPosition(165, 53);
                                        if (doubled)
                                        {
                                            Console.Write("║ Пробел – Бросить кубики         Tab – Посмотреть недвижимость ║");
                                        }
                                        else
                                        {
                                            Console.Write("║ Enter – Закончить ход           Tab – Посмотреть недвижимость ║");
                                        }
                                        Console.SetCursorPosition(165, 54);
                                        if (musicMuted)
                                        {
                                            Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                                        }
                                        else
                                        {
                                            Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                                        }
                                        Console.SetCursorPosition(165, 55);
                                        Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                        switch (Console.ReadKey(true).Key)
                                        {
                                            case ConsoleKey.Spacebar:
                                                if (doubled) Menu(player, turn, true);
                                                break;
                                            case ConsoleKey.Enter:
                                                turnNotEnded = false;
                                                break;
                                            case ConsoleKey.Tab:
                                                PrintRealty(player);
                                                break;
                                            case ConsoleKey.T:
                                                Trade(player);
                                                break;
                                            case ConsoleKey.M:
                                                if (musicMuted)
                                                {
                                                    musicMuted = false;
                                                    musicPlayer.PlayLooping();
                                                    Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                                                }
                                                else
                                                {
                                                    musicMuted = true;
                                                    musicPlayer.Stop();
                                                    Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                                                }
                                                break;
                                        }
                                    }
                                }
                                else if (quartal.special == 4)
                                {
                                    ClearMenu();
                                    Console.SetCursorPosition(165, 50);
                                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                    Console.SetCursorPosition(165, 51);
                                    Console.Write("║             Ваша остановка! Бесплатная парковка               ║");
                                    Console.SetCursorPosition(165, 52);
                                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                    Console.SetCursorPosition(165, 53);
                                    Console.Write("║ ");
                                    if (quartal.cost > player.balance) Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.Write("Пробел – Купить");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.Write("                  Enter – Выставить на аукцион ║");
                                    Console.SetCursorPosition(165, 54);
                                    if (musicMuted)
                                    {
                                        Console.Write("║                      M – Включить музыку                      ║");
                                    }
                                    else
                                    {
                                        Console.Write("║                      M – Заглушить музыку                     ║");
                                    }
                                    Console.SetCursorPosition(165, 55);
                                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                    switch (Console.ReadKey(true).Key)
                                    {
                                        case ConsoleKey.Spacebar:
                                            if (quartal.cost <= player.balance) 
                                            {
                                                Purchase(player, quartal);
                                                PrintPlayers(turn);
                                                while (true)
                                                {
                                                    ClearMenu();
                                                    Console.SetCursorPosition(165, 50);
                                                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                                    Console.SetCursorPosition(165, 51);
                                                    Console.Write("║                              МЕНЮ                             ║");
                                                    Console.SetCursorPosition(165, 52);
                                                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                                    Console.SetCursorPosition(165, 53);
                                                    if (doubled)
                                                    {
                                                        Console.Write("║ Пробел – Бросить кубики         Tab – Посмотреть недвижимость ║");
                                                    }
                                                    else
                                                    {
                                                        Console.Write("║ Enter – Закончить ход           Tab – Посмотреть недвижимость ║");
                                                    }
                                                    Console.SetCursorPosition(165, 54);
                                                    if (musicMuted)
                                                    {
                                                        Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                                                    }
                                                    else
                                                    {
                                                        Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                                                    }
                                                    Console.SetCursorPosition(165, 55);
                                                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                                    switch (Console.ReadKey(true).Key)
                                                    {
                                                        case ConsoleKey.Spacebar:
                                                            if (doubled) goto DBL;
                                                            break;
                                                        case ConsoleKey.Enter:
                                                            if (!doubled) goto END;
                                                            break;
                                                        case ConsoleKey.Tab:
                                                            PrintRealty(player);
                                                            break;
                                                        case ConsoleKey.T:
                                                            Trade(player);
                                                            break;
                                                        case ConsoleKey.M:
                                                            if (musicMuted)
                                                            {
                                                                musicMuted = false;
                                                                musicPlayer.PlayLooping();
                                                            }
                                                            else
                                                            {
                                                                musicMuted = true;
                                                                musicPlayer.Stop();
                                                            }
                                                            break;
                                                    }
                                                }
                            else if (quartal.special == 8)
                            {
                                // Todo: Заплатить 10% от нынешнего баланса или 200$
                            }
                            else if (quartal.special == 9)
                            {
                                while (true)
                                {
                                    Console.SetCursorPosition(165, 50);
                                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                    Console.SetCursorPosition(165, 51);
                                    Console.Write("║ В этот прекрасный день стоит пройтись по магазинам. С вас 100$║");
                                    Console.SetCursorPosition(165, 52);
                                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                    Console.SetCursorPosition(165, 53);
                                    Console.Write("║ ");
                                    if (player.balance < 100) Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.Write("Enter – Заплатить 100$");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.Write("            Tab – Посмотреть недвижимость ║");
                                    Console.SetCursorPosition(165, 54);
                                    if (musicMuted)
                                    {
                                        Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                                    }
                                    else
                                    {
                                        Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                                    }
                                    Console.SetCursorPosition(165, 55);
                                    Console.Write("║                 End – Объявить себя банкротом                 ║");
                                    Console.SetCursorPosition(165, 56);
                                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                    while (turnNotEnded)
                                    {
                                        while (Console.KeyAvailable)
                                        {
                                            switch (Console.ReadKey(true).Key)
                                            {
                                                case ConsoleKey.Enter:
                                                    if (player.balance >= 100)
                                                    {
                                                        player.balance -= 100;
                                                        Menu(player, turn, true);
                                                        turnNotEnded = false;
                                                    }
                                                    break;
                                                case ConsoleKey.Tab:
                                                    PrintRealty(player);
                                                    break;
                                                case ConsoleKey.T:
                                                    Trade(player);
                                                    break;
                                                case ConsoleKey.M:
                                                    if (musicMuted)
                                                    {
                                                        musicMuted = false;
                                                        musicPlayer.PlayLooping();
                                                        Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                                                    }
                                                    else
                                                    {
                                                        musicMuted = true;
                                                        musicPlayer.Stop();
                                                        Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                                                    }
                                                    break;
                                                case ConsoleKey.End:
                                                    player.Bankrupt(null);
                                                    int liberation = player.liberation;
                                                    for (int i = 0; i < liberation; i++)
                                                    {
                                                        if (treasuries.Count == 15)
                                                        {
                                                            treasuries.Add(4);
                                                            player.liberation--;
                                                        }
                                                        else
                                                        {
                                                            chances.Add(4);
                                                            player.liberation--;
                                                        }
                                                    }
                                                    turnNotEnded = false;
                                                    break;
                                            }
                                        }
                                    }
                                }
                                if (doubled) Menu(player, turn, true);
                                turnNotEnded = false;
                            }
                            break;
                        case ConsoleKey.T:
                            Trade(player);
                            break;
                        case ConsoleKey.Tab:
                            PrintRealty(player);
                            break;
                        case ConsoleKey.M:
                            if (musicMuted)
                            {
                                musicMuted = false;
                                musicPlayer.PlayLooping();
                                Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                            }
                            else
                            {
                                musicMuted = true;
                                musicPlayer.Stop();
                                Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                            }
                            break;
                        case ConsoleKey.P:
                            if (player.prisoned)
                            {
                                player.prisoned = false;
                                player.balance -= 50;
                                Menu(player, turn, true);
                            }
                            break;
                    }
                }
            }
            PrintTitle();
        }

        /// <summary>
        /// Покупка игроком конкретной улицы
        /// </summary>
        /// <param name="player">Покупатель, игрок</param>
        /// <param name="quartal">Покупаемая улица</param>
        static public void Purchase(Player player, Quartal quartal)
        {
            moneyPlayer.Position = TimeSpan.Zero;
            moneyPlayer.Play();
            player.balance -= quartal.cost;
            player.property.Add(quartal);
            quartal.owner = player;
            IncreaseColor(quartal, player);
        }

        /// <summary>
        /// Выставление улицы на аукцион. Игроки по очереди повышают ставки, начиная со стоимости улицы. Те, у кого нет денег на ставку выше, аннулируют ставку автоматически
        /// </summary>
        /// <param name="quartal">Выставляемая на аукцион улица</param>
        static public void Auction(Quartal quartal)
        {
            int spaces, left, right;
            int bid = quartal.cost;
            int participants = playersCount;
            Player buyer = null;
            for (int i = 0; i < playersCount; i++)
            {
                if (players[i].balance < bid) players[i].canselled = true;
            }
            do
            {
                for (int i = 0; i < playersCount; i++)
                {
                    Player participant = players[i];
                    if (participant.balance < bid) participant.canselled = true;
                    if (participant.canselled) continue;
                    Console.SetCursorPosition(69, 26);
                    Console.Write("╔══════════════════════╗");
                    Console.SetCursorPosition(69, 27);
                    spaces = 22 - quartal.name.Length;
                    left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                    right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                    Console.Write("║");
                    for (int j = 0; j < left; j++) Console.Write(" ");
                    if (quartal.special == 6)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (quartal.special == 7) Console.BackgroundColor = ConsoleColor.Gray;
                    else Console.BackgroundColor = quartal.color;
                    Console.Write(quartal.name);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    for (int j = 0; j < right; j++) Console.Write(" ");
                    Console.Write("║");
                    Console.SetCursorPosition(69, 28);
                    Console.Write("╠══════════════════════╣");
                    Console.SetCursorPosition(69, 29);
                    Console.Write("║ ");
                    if (quartal.cost < 100) Console.Write(" ");
                    Console.Write($"Начальная цена: {quartal.cost}$");
                    Console.Write(" ║");
                    PrintPlayers(i);
                    Console.SetCursorPosition(69, 30);
                    Console.Write("║");
                    if (buyer == null)
                    {
                        Console.Write("                      ");
                    }
                    else
                    {
                        spaces = 22 - buyer.name.Length;
                        left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                        right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                        for (int j = 0; j < left; j++) Console.Write(" ");
                        Console.Write(buyer.name);
                        for (int j = 0; j < right; j++) Console.Write(" ");
                    }
                    Console.Write("║");
                    Console.SetCursorPosition(69, 31);
                    Console.Write("║ Ставка: ");
                    for (int j = 0; j < 11 - Convert.ToString(bid).Length; j++) Console.Write(" ");
                    Console.Write($"{bid}$ ║");
                    Console.SetCursorPosition(69, 32);
                    Console.Write("╚══════════════════════╝");

                    if (participants > 1)
                    {
                        Console.SetCursorPosition(165, 50);
                        Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                        Console.SetCursorPosition(165, 51);
                        Console.Write("║                            АУКЦИОН                            ║");
                        Console.SetCursorPosition(165, 52);
                        Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                        Console.SetCursorPosition(165, 53);
                        Console.Write("║ ");
                        if (participant.balance >= 1) Console.Write("Tab – Поставить 1$");
                        else Console.Write("                  ");
                        if (participant.balance >= 10) Console.Write("                      Enter – Поставить 10$ ║");
                        else Console.Write("                                            ║");
                        Console.SetCursorPosition(165, 54);
                        Console.Write("║ ");
                        if (participant.balance >= 100) Console.Write("Пробел – Поставить 100$");
                        else Console.Write("                       ");
                        Console.Write("       Backspace – Аннулировать ставку ║");
                        Console.SetCursorPosition(165, 55);
                        Console.Write("╚═══════════════════════════════════════════════════════════════╝");

                        while (!participant.didBid)
                        {
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.Tab:
                                    if (participant.balance >= 1)
                                    {
                                        bid++;
                                        buyer = participant;
                                        participant.didBid = true;
                                        goto End;
                                    }
                                    break;
                                case ConsoleKey.Enter:
                                    if (participant.balance >= 10)
                                    {
                                        bid += 10;
                                        buyer = participant;
                                        participant.didBid = true;
                                        goto End;
                                    }
                                    break;
                                case ConsoleKey.Spacebar:
                                    if (participant.balance >= 100)
                                    {
                                        bid += 100;
                                        buyer = participant;
                                        participant.didBid = true;
                                        goto End;
                                    }
                                    break;
                                case ConsoleKey.Backspace:
                                    participants--;
                                    participant.canselled = true;
                                    goto End;
                            }
                        }
                    }
                    else
                    {
                        if (participant.didBid)
                        {
                            buyer = participant;
                        }
                        else
                        {
                            Console.SetCursorPosition(165, 50);
                            Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                            Console.SetCursorPosition(165, 51);
                            Console.Write("║                            АУКЦИОН                            ║");
                            Console.SetCursorPosition(165, 52);
                            Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                            Console.SetCursorPosition(165, 53);
                            Console.Write("║ Enter – Купить за                Esc – Отказаться от сделки   ║");

                            bool notChoosen = true;
                            while (notChoosen)
                            {
                                Console.SetCursorPosition(165, 54);
                                if (musicMuted)
                                {
                                    Console.Write("║                      M – Включить музыку                      ║");
                                }
                                else
                                {
                                    Console.Write("║                      M – Заглушить музыку                     ║");
                                }
                                Console.SetCursorPosition(165, 55);
                                Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                Console.SetCursorPosition(185, 53);
                                Console.Write($"{bid}$");
                                switch (Console.ReadKey(true).Key)
                                {
                                    case ConsoleKey.Enter:
                                        //but
                                        //notChoosen = true;
                                        break;
                                    case ConsoleKey.Escape:
                                        notChoosen = true;
                                        break;
                                    case ConsoleKey.M:
                                        if (musicMuted)
                                        {
                                            musicMuted = false;
                                            musicPlayer.PlayLooping();
                                        }
                                        else
                                        {
                                            musicMuted = true;
                                            musicPlayer.Stop();
                                        }
                                        break;
                                }
                            }
                        }
                        
                    }
                }
            End:;
            } while (participants > 1);

            if (buyer != null)
            {
                quartal.owner = buyer;
                buyer.property.Add(quartal);
                buyer.balance -= bid;
            }
            for (int i = 0; i < playersCount; i++)
            {
                players[i].canselled = false;
                players[i].didBid = false;
            }
            PrintTitle();
        }

        /// <summary>
        /// Вытаскивание карты "Казна"
        /// </summary>
        /// <param name="player">Номер ходящего игрока</param>
        static public Quartal Treasury(Player player)
        {
            int card = treasuries[0];
            bool notEntered;
            treasuries.RemoveAt(0);
            treasuries.Add(card);
            switch (card)
            {
                case 0:
                case 8:
                    ClearMenu();
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║              Выгодная продажа акций! Получите 25$             ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Получить 25$                      M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Получить 25$                     M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance += 25;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 1:
                    ClearMenu();
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║            Пора платить за страховку! Заплатите 50$           ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Заплатить 50$                     M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Заплатить 50$                    M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance -= 50;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 2:
                    ClearMenu();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                   Вернитесь на Старую дорогу                  ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Перейти                           M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Перейти                          M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                for (int i = player.position; i > 1; i--)
                                {
                                    quartals[i - 1].visitors++;
                                    PrintPieces(i, i - 1, player.piece);
                                    quartals[i].visitors--;
                                }
                                player.position = 1;
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return quartals[1];
                case 3:
                    ClearMenu();
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║         У вас простуда! Заплатите за услуги доктора 50$       ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Заплатить 50$                     M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Заплатить 50$                    M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance -= 50;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 4:
                    ClearMenu();
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                 Начальник тюрьмы Ваш должник!                 ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("║            Получите карточку освобождения от тюрьмы           ║");
                    Console.SetCursorPosition(165, 53);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 55);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 54);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Получить                          M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Получить                         M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                paperPlayer.Position = TimeSpan.Zero;
                                paperPlayer.Play();
                                player.liberation++;
                                treasuries.Remove(4);
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 5:
                    quartals[10].visitors++;
                    PrintPieces(player.position, 10, player.piece);
                    quartals[player.position].visitors--;
                    player.position = 10;
                    player.prisoned = true;
                    policePlayer.Position = TimeSpan.Zero;
                    policePlayer.Play();
                    return null;
                case 6:
                    ClearMenu();
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║       Вы сломали ногу! Заплатите за услуги доктора 100$       ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Заплатить 100$                    M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Заплатить 100$                   M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance -= 100;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 7:
                    ClearMenu();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║           Заплатите штраф 10$ или возьмите \"Шанс\"           ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("║ Пробел – Заплатить штраф 10$         Enter – Взять \"Шанс\" ║");
                    Console.SetCursorPosition(165, 55);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 54);
                        if (musicMuted)
                        {
                            Console.Write("║                      M – Включить музыку                      ║");
                        }
                        else
                        {
                            Console.Write("║                      M – Заглушить музыку                     ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Spacebar:
                                player.balance -= 10;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                break;
                            case ConsoleKey.Enter:
                                return Chance(player);
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 9:
                    ClearMenu();
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                Возмещение налога! Получите 25$                ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Получить 25$                      M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Получить 25$                     M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance += 25;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 10:
                    ClearMenu();
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║            Выгодная продажа облигаций! Получите 50$           ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Получить 50$                      M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Получить 50$                     M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance += 50;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 11:
                    ClearMenu();
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║       У вас день рождения! Получите 10$ с каждого игрока      ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Получить                          M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Получить                         M – Заглушить музыку ║");
                        }
                        Console.SetCursorPosition(184, 53);
                        Console.Write($"{10 * (playersCount - 1)}$");
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                for (int i = 0; i < playersCount; i++)
                                {
                                    players[i].balance -= 10;
                                    player.balance += 10;
                                }
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 12:
                    ClearMenu();
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║         Банковская ошибка в Вашу пользу! Получите 200$        ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Получить 200$                     M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Получить 200$                    M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance += 200;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 13:
                    ClearMenu();
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║         На Ваше имя подписали завещание! Получите 100$        ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Получить 100$                     M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Получить 100$                    M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance += 100;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 14:
                    ClearMenu();
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║    Вы заняли второе место на конкурсе красоты! Получите 10$   ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Получить 10$                      M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Получить 10$                     M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance += 10;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                default:
                    ClearMenu();
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                   Сбор ренты! Получите 100$                   ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Получить 100$                     M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Получить 100$                    M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance += 100;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break; 
                        }
                    }
                    return null;
            }
        }

        /// <summary>
        /// Вытаскивание карты "Шанс"
        /// </summary>
        /// <param name="player">Номер ходящего игрока</param>
        static public Quartal Chance(Player player)
        {
            int card = chances[0];
            int payment;
            bool notEntered;
            chances.RemoveAt(0);
            chances.Add(card);
            int position = player.position;
            switch (card)
            {
                case 0:
                    ClearMenu();
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                      Сбор на ремонт улиц!                     ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("║       Заплатите 40$ за каждый дом и 115$ за каждый отель      ║");
                    Console.SetCursorPosition(165, 53);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 55);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    payment = 40 * player.housesCount + 115 * player.hotelsCount;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 54);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Заплатить                         M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Заплатить                         M – Заглушить музыку ║");
                        }
                        Console.SetCursorPosition(165, 54);
                        Console.Write($"{payment}$");
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance -= payment;
                                if (payment > 0)
                                {
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                }
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 1:
                    quartals[10].visitors++;
                    PrintPieces(position, 10, player.piece);
                    quartals[position].visitors--;
                    player.position = 10;
                    player.prisoned = true;
                    policePlayer.Position = TimeSpan.Zero;
                    policePlayer.Play();
                    return null;
                case 2:
                    ClearMenu();
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║       Штраф за вождение в нетрезвом виде! Заплатите 20$       ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Заплатить 20$                      M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Заплатить 20$                    M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance -= 20;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 3:
                    ClearMenu();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                  Вернитесь на три поля назад                  ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Вернуться                         M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Вернуться                       M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                for (int j = position; j != (position + 37) % 40;)
                                {
                                    int next = (position + 1) % 40;
                                    quartals[next].visitors++;
                                    PrintPieces(j, next, player.piece);
                                    quartals[j].visitors--;
                                    Thread.Sleep(sleep);
                                    j = next;
                                }
                                player.position = (position + 37) % 40;
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return quartals[(position + 37) % 40];
                case 4:
                    ClearMenu();
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                 Начальник тюрьмы Ваш должник!                 ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("║            Получите карточку освобождения от тюрьмы           ║");
                    Console.SetCursorPosition(165, 53);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 55);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 54);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Получить                          M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Получить                         M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                paperPlayer.Position = TimeSpan.Zero;
                                paperPlayer.Play();
                                player.liberation++;
                                chances.Remove(4);
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 5:
                    ClearMenu();
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                  Возврат займа! Получите 150$                 ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Получить 150$                     M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Получить 150$                    M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance += 150;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 6:
                    ClearMenu();
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║           Пришли банковские дивиденды! Получите 50$           ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Получить 50$                      M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Получить 50$                     M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance += 50;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 7:
                    ClearMenu();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                   Отправляйтесь в Аквапарк!                   ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("║             Если Вы пройдёте Старт, получите 200$             ║");
                    Console.SetCursorPosition(165, 53);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 55);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 54);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Отправиться в Аквапарк            M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Отправиться в Аквапарк           M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                position = player.position;
                                quartals[6].visitors++;
                                PrintPieces(position, 6, player.piece);
                                quartals[position].visitors--;
                                if (position > 6)
                                {
                                    player.balance += 200;
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                }
                                player.position = 6;
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return quartals[6];
                case 8:
                    ClearMenu();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                       Пройдите на Старт                       ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Пройти на Старт                   M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Пройти на Старт                  M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                position = player.position;
                                quartals[0].visitors++;
                                PrintPieces(position, 0, player.piece);
                                quartals[position].visitors--;
                                player.position = 0;
                                player.balance += 200;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 9:
                    ClearMenu();
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║          Штраф за превышение скорости! Заплатите 15$          ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Заплатить 15$                      M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Заплатить 15$                    M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance -= 15;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 10:
                    ClearMenu();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║             Отправляйтесь в Северный морской порт!            ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("║             Если Вы пройдёте Старт, получите 200$             ║");
                    Console.SetCursorPosition(165, 53);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("║          Enter – Отправиться в Северный морской порт          ║");
                    Console.SetCursorPosition(165, 56);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 55);
                        if (musicMuted)
                        {
                            Console.Write("║                      M – Включить музыку                      ║");
                        }
                        else
                        {
                            Console.Write("║                      M – Заглушить музыку                     ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                position = player.position;
                                quartals[15].visitors++;
                                PrintPieces(position, 15, player.piece);
                                quartals[position].visitors--;
                                if (position > 15)
                                {
                                    player.balance += 200;
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                }
                                player.position = 15;
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return quartals[15];
                case 11:
                    ClearMenu();
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║        Вы выиграли чемпионат по шахматам! Получите 100$       ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Получить 200$                     M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Получить 200$                    M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance += 100;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                case 12:
                    ClearMenu();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║             Отправляйтесь в Гостиничный комплекс!             ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Отправиться в Гостиничный комплекс M – Включить музыку║");
                        }
                        else
                        {
                            Console.Write("║Enter – Отправиться в Гостиничный косплекс M – Заглушить музыку║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                position = player.position;
                                quartals[39].visitors++;
                                PrintPieces(position, 39, player.piece);
                                quartals[position].visitors--;
                                player.position = 39;
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return quartals[39];
                case 13:
                    ClearMenu();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                   Отправляйтесь в Ресторан!                   ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("║             Если Вы пройдёте Старт, получите 200$             ║");
                    Console.SetCursorPosition(165, 53);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 55);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 54);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Отправиться в Ресторан            M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Отправиться в Ресторан           M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                position = player.position;
                                quartals[24].visitors++;
                                PrintPieces(position, 24, player.piece);
                                quartals[position].visitors--;
                                if (position > 24)
                                {
                                    player.balance += 200;
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                }
                                player.position = 24;
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return quartals[24];
                case 14:
                    ClearMenu();
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                      Капитальный ремонт!                      ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("║       Заплатите 25$ за каждый дом и 100$ за каждый отель      ║");
                    Console.SetCursorPosition(165, 53);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 55);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    payment = 25 * player.housesCount + 100 * player.hotelsCount;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 54);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Заплатить                         M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Заплатить                         M – Заглушить музыку ║");
                        }
                        Console.SetCursorPosition(165, 54);
                        Console.Write($"{payment}$");
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance -= payment;
                                if (payment > 0)
                                {
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                }
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
                default:
                    ClearMenu();
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║            Оплата курсов водителей! Заплатите 150$            ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        Console.SetCursorPosition(165, 53);
                        if (musicMuted)
                        {
                            Console.Write("║ Enter – Заплатить 150$                    M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ Enter – Заплатить 150$                   M – Заглушить музыку ║");
                        }
                        switch (Console.ReadKey().Key)
                        {
                            case ConsoleKey.Enter:
                                player.balance -= 150;
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.PlayLooping();
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Stop();
                                    musicMuted = true;
                                }
                                break;
                        }
                    }
                    return null;
            }
        }

        static void Main()
        {
            // Window
            Console.Title = "Монополия";
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetBufferSize(Console.LargestWindowWidth, 110);
            Console.CursorVisible = false;

            // Sounds
            musicPlayer.PlayLooping();
            moneyPlayer.Open(new System.Uri(@"C:\Users\Class_Student\source\repos\Monopoly\Resources\money.wav"));
            policePlayer.Open(new System.Uri(@"C:\Users\Class_Student\source\repos\Monopoly\Resources\police.wav"));
            paperPlayer.Open(new System.Uri(@"C:\Users\Class_Student\source\repos\Monopoly\Resources\paper.wav"));
            happyPlayer.Open(new System.Uri(@"C:\Users\Class_Student\source\repos\Monopoly\Resources\happy.wav"));
            sadPlayer.Open(new System.Uri(@"C:\Users\Class_Student\source\repos\Monopoly\Resources\sad.wav"));

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
                bool prevSpace = false;
                string trimmedName = "";
                foreach (char character in name)
                {
                    if (character == ' ')
                    {
                        if (prevSpace) continue;
                        else prevSpace = true;
                    }
                    else prevSpace = false;
                    trimmedName += character;
                }
                if (names.Contains(trimmedName))
                {
                    Console.WriteLine("Кто-то из игроков уже выбрал это имя! Введите другое");
                    i--;
                    continue;
                }
                if (trimmedName.Length < 2)
                {
                    Console.WriteLine("Слишком короткое имя! Введите другое");
                    i--;
                    continue;
                }
                if (trimmedName.Length > 16)
                {
                    Console.WriteLine("Слишком длинное имя! Введите другое");
                    i--;
                    continue;
                }
                names[i] = trimmedName;
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
            PrintTitle();
            while (remaining > 1)
            {
                for (int i = 0; i < playersCount; i++)
                {
                    if (players[i].bankrupt) continue;
                    PrintPlayers(i);
                    Menu(players[i], i);
                }
            }
        }
    }
}