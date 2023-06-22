using Monopoly.Properties;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Media;

namespace Monopoly
{
    internal class Monopoly
    {
        static int playersCount;
        static int remaining;
        static List<int> treasuries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        static List<int> chances = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        static bool musicMuted = false;
        static readonly int sleep = 400;
        static int dice1, dice2;
        static int doubles = 0;
        static bool repeat = false;
        static readonly MediaPlayer menuPlayer = new MediaPlayer();
        static readonly MediaPlayer musicPlayer = new MediaPlayer();
        static readonly MediaPlayer victoryPlayer = new MediaPlayer();
        static readonly MediaPlayer moneyPlayer = new MediaPlayer();
        static readonly MediaPlayer policePlayer = new MediaPlayer();
        static readonly MediaPlayer paperPlayer = new MediaPlayer();
        static readonly MediaPlayer happyPlayer = new MediaPlayer();
        static readonly MediaPlayer sadPlayer = new MediaPlayer();
        static readonly MediaPlayer upgradePlayer = new MediaPlayer();
        static readonly MediaPlayer downgradePlayer = new MediaPlayer();
        static readonly MediaPlayer padlockPlayer = new MediaPlayer();
        static readonly MediaPlayer chainPlayer = new MediaPlayer();
        static readonly MediaPlayer cancelPlayer = new MediaPlayer();
        static readonly MediaPlayer turnPlayer = new MediaPlayer();
        static readonly MediaPlayer successPlayer = new MediaPlayer();
        static readonly Random rnd = new Random();
        static Player[] players;
        static Quartal[] quartals = new Quartal[40]
        {
            new Quartal(0),
            new Quartal("Старая дорога", ConsoleColor.Yellow, 60, 2, 4, 10, 30, 90, 160, 250, 50, 30),
            new Quartal(1),
            new Quartal("Главное шоссе", ConsoleColor.Yellow, 60, 4, 8, 20, 60, 180, 320, 450, 50, 30),
            new Quartal(8),
            new Quartal("Западный морской порт", true),
            new Quartal("Аквапарк", ConsoleColor.DarkYellow, 100, 6, 12, 30, 90, 270, 400, 550, 50, 50),
            new Quartal(2),
            new Quartal("Городской парк", ConsoleColor.DarkYellow, 100, 6, 12, 30, 90, 270, 400, 550, 50, 50),
            new Quartal("Горнолыжный курорт", ConsoleColor.DarkYellow, 120, 8, 16, 40, 100, 300, 450, 600, 50, 60),
            new Quartal(3),
            new Quartal("Спальный район", ConsoleColor.DarkGreen, 140, 10, 20, 50, 150, 450, 625, 750, 100, 70),
            new Quartal("Электрическая компания", false),
            new Quartal("Деловой квартал", ConsoleColor.DarkGreen, 140, 10, 20, 50, 150, 450, 625, 750, 100, 70),
            new Quartal("Торговая площадь", ConsoleColor.DarkGreen, 160, 12, 24, 60, 180, 500, 700, 900, 100, 80),
            new Quartal("Северный морской порт", true),
            new Quartal("Улица Пушкина", ConsoleColor.Green, 180, 14, 28, 70, 200, 550, 750, 950, 100, 90),
            new Quartal(1),
            new Quartal("Проспект Мира", ConsoleColor.Green, 180, 14, 28, 70, 200, 550, 750, 950, 100, 90),
            new Quartal("Проспект Победы", ConsoleColor.Green, 200, 16, 32, 80, 220, 600, 800, 1000, 100, 100),
            new Quartal(4),
            new Quartal("Бар", ConsoleColor.Red, 220, 18, 36, 90, 250, 700, 875, 1050, 150, 110),
            new Quartal(2),
            new Quartal("Ночной клуб", ConsoleColor.Red, 220, 18, 36, 90, 250, 700, 875, 1050, 150, 110),
            new Quartal("Ресторан", ConsoleColor.Red, 240, 20, 40, 100, 300, 750, 925, 1100, 150, 120),
            new Quartal("Восточный морской порт", true),
            new Quartal("Компьютеры", ConsoleColor.Magenta, 260, 22, 44, 110, 330, 800, 975, 1150, 150, 130),
            new Quartal("Интернет", ConsoleColor.Magenta, 260, 22, 44, 110, 330, 800, 975, 1150, 150, 130),
            new Quartal("Водопроводная станция", false),
            new Quartal("Сотовая связь", ConsoleColor.Magenta, 280, 24, 48, 120, 360, 850, 1025, 1200, 150, 140),
            new Quartal(5),
            new Quartal("Морские перевозки", ConsoleColor.Blue, 300, 26, 52, 130, 390, 900, 1100, 1275, 200, 150),
            new Quartal("Железная дорога", ConsoleColor.Blue, 300, 26, 52, 130, 390, 900, 1100, 1275, 200, 150),
            new Quartal(1),
            new Quartal("Авиакомпания", ConsoleColor.Blue, 320, 28, 56, 150, 450, 1000, 1200, 1400, 200, 160),
            new Quartal("Южный морской порт", true),
            new Quartal(2),
            new Quartal("Курортная зона", ConsoleColor.Cyan, 350, 35, 70, 175, 500, 1100, 1300, 1500, 200, 175),
            new Quartal(9),
            new Quartal("Гостиничный комплекс", ConsoleColor.Cyan, 400, 50, 100, 200, 600, 1400, 1700, 2000, 200, 200),
        };

        static void PrintDices(int dice1, int dice2)
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

        static void PrintDices(int dice1, int dice2, int doubles)
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

        static void PrintRoadmap()
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
                Console.Write(players[i].Piece);
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
            Console.Write("║     280$    ║     150$    ║     260$    ║     260$    ║     200$    ║     240$    ║     220$    ║");
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

        static void PrintPlayers(int turn)
        {
            int addWidth, addHeight;
            ConsoleColor background;
            Player player;
            for (int i = 0; i < playersCount; i++)
            {
                player = players[i];
                addWidth = 34 * (i % 2);
                addHeight = 12 * (i / 2);
                if (player.IsBankrupt) background = ConsoleColor.Gray;
                else background = ConsoleColor.White;
                Console.SetCursorPosition(165 + addWidth, 2 + addHeight);
                Console.BackgroundColor = background;
                Console.Write("╔═════════════════════════════╗");
                Console.SetCursorPosition(165 + addWidth, 3 + addHeight);
                Console.Write("║");
                if (i == turn) Console.BackgroundColor = ConsoleColor.Yellow;
                if (player.Cancelled) Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write($" {player.Name} {player.Piece}");
                for (int j = 0; j < 24 - player.Name.Length - Convert.ToString(player.Balance).Length; j++)
                {
                    Console.Write(" ");
                }
                Console.Write($"{player.Balance}$ ");
                Console.BackgroundColor = background;
                Console.Write("║");
                Console.SetCursorPosition(165 + addWidth, 4 + addHeight);
                Console.Write("╠═════════════════════════════╣");
                Console.SetCursorPosition(165 + addWidth, 5 + addHeight);
                Console.Write($"║ Карты освобождения: {player.Liberation}");
                for (int j = 0; j < 8 - Convert.ToString(player.Liberation).Length; j++)
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
                Console.Write($" {player.Yellow}/3 ");
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Write("Тёмно-жёлтая");
                Console.BackgroundColor = background;
                Console.Write($" {player.DarkYellow}/3 ║");
                Console.SetCursorPosition(165 + addWidth, 8 + addHeight);
                Console.Write("║ ");
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("Зелёная");
                Console.BackgroundColor = background;
                Console.Write($" {player.Green}/3   ");
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.Write("Пурпурная");
                Console.BackgroundColor = background;
                Console.Write($" {player.Magenta}/3 ║");
                Console.SetCursorPosition(165 + addWidth, 9 + addHeight);
                Console.Write("║ ");
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("Синяя");
                Console.BackgroundColor = background;
                Console.Write($" {player.Blue}/3 ");
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write("Тёмно-зелёная");
                Console.BackgroundColor = background;
                Console.Write($" {player.DarkGreen}/3 ║");
                Console.SetCursorPosition(165 + addWidth, 10 + addHeight);
                Console.Write("║ ");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("Красная");
                Console.BackgroundColor = background;
                Console.Write($" {player.Red}/3     ");
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.Write("Голубая");
                Console.BackgroundColor = background;
                Console.Write($" {player.Cyan}/2 ║");
                Console.SetCursorPosition(165 + addWidth, 11 + addHeight);
                Console.Write("║ ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Чёрная");
                Console.BackgroundColor = background;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($" {player.Black}/4        ");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("Серая");
                Console.BackgroundColor = background;
                Console.Write($" {player.Gray}/2 ║");
                Console.SetCursorPosition(165 + addWidth, 12 + addHeight);
                Console.Write("╚═════════════════════════════╝");
                Console.BackgroundColor = ConsoleColor.White;
            }
        }

        static void PrintPieces(int oldPosition, int newPosition, Player player)
        {
            int oldVisitors = quartals[oldPosition].Visitors - 1;
            int oldPiecesPtr = 0;
            char[] oldPieces = new char[oldVisitors];
            int prisonedVisitors = players.Where(p => p.IsPrisoned).Count();
            int prisonerPiecesPtr = 0;
            char[] prisonerPieces = new char[prisonedVisitors];
            if (newPosition == -1)
            {
                for (int i = 0; i < playersCount; i++)
                {
                    if (players[i].Position == oldPosition && players[i] != player) oldPieces[oldPiecesPtr++] = players[i].Piece;
                    if (players[i].IsPrisoned && players[i] != player) prisonerPieces[prisonerPiecesPtr++] = players[i].Piece;
                }

                int column, row;
                if (oldPosition <= 10)
                {
                    if (oldPosition == 0)
                    {
                        row = 3;
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        for (int i = 0; i <= oldVisitors; i++)
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
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        for (int i = 0; i < 8; i++)
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
                        for (int i = 0; i <= oldVisitors; i++)
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
                        for (int i = 0; i <= oldVisitors; i++)
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
                    for (int i = 0; i <= oldVisitors; i++)
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
                    for (int i = 0; i <= oldVisitors; i++)
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
                    for (int i = 0; i <= oldVisitors; i++)
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
            }
            else
            {
                int newVisitors = quartals[newPosition].Visitors + 1;
                int newPiecesPtr = 1;
                char[] newPieces = new char[newVisitors];
                if (!player.IsPrisoned) newPieces[0] = player.Piece;
                for (int i = 0; i < playersCount; i++)
                {
                    if (players[i].IsPrisoned) prisonerPieces[prisonerPiecesPtr++] = players[i].Piece;
                    else
                    {
                        if (players[i].Position == oldPosition && players[i] != player) oldPieces[oldPiecesPtr++] = players[i].Piece;
                        if (players[i].Position == newPosition) newPieces[newPiecesPtr++] = players[i].Piece;
                    }
                }

                int column, row;
                if (oldPosition <= 10)
                {
                    if (oldPosition == 0)
                    {
                        row = 3;
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        for (int i = 0; i <= oldVisitors; i++)
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
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        for (int i = 0; i < 8; i++)
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
                        for (int i = 0; i <= oldVisitors; i++)
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
                        for (int i = 0; i <= oldVisitors; i++)
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
                    for (int i = 0; i <= oldVisitors; i++)
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
                    for (int i = 0; i <= oldVisitors; i++)
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
                    for (int i = 0; i <= oldVisitors; i++)
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
                            Console.BackgroundColor = ConsoleColor.DarkGray;
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
                player.Move(newPosition);
                quartals[newPosition].IncreaseVisitors();
            }
            quartals[oldPosition].DecreaseVisitors();
        }

        static void PrintTraders(int marker, Player[] traders)
        {
            int column = 25, row = 27;
            bool additionalRow = false;
            int charactersCount = -2;
            foreach (Player trader in traders)
            {
                if (charactersCount + trader.Name.Length + 2 > 107) additionalRow = true;
                else charactersCount += trader.Name.Length + 2;
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
                Console.Write($"{trader.Piece}{trader.Name}");
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
                spaces2 = 106 - trader.Name.Length;
                left2 = Convert.ToInt32(Math.Ceiling(spaces2 / 2.0));
                right2 = Convert.ToInt32(Math.Floor(spaces2 / 2.0));
                Console.Write("║ ");
                for (i = 0; i < left2; i++)
                {
                    Console.Write(" ");
                }
                if (traders.Length - 1 == marker) Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Write($"{trader.Piece}{trader.Name}");
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
                Console.Write($"{trader.Piece}{trader.Name}");
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

        static void PrintProperty(
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
                    if (give[gRows - 1].Length + playerProperty[i].Name.Length + 2 > 89) gRows++;
                    give[gRows - 1] += $"{playerProperty[i].Name}, ";
                }
                if (give[gRows - 1].Length + playerProperty[playerProperty.Count() - 1].Name.Length > 89) gRows++;
                give[gRows - 1] += playerProperty[playerProperty.Count() - 1].Name;
                if (playerMoney > 0)
                {
                    if (give[gRows - 1].Length + 2 > 89) gRows++;
                    give[gRows - 1] += " и";
                    if (give[gRows - 1].Length + Convert.ToString(playerMoney).Length + 2 > 89) gRows++;
                    give[gRows - 1] += $" {playerMoney}$";
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
                    if (take[tRows - 1].Length + traderProperty[i].Name.Length + 2 > 89) tRows++;
                    take[tRows - 1] += $"{traderProperty[i].Name}, ";
                }
                if (take[tRows - 1].Length + traderProperty[traderProperty.Count() - 1].Name.Length > 89) tRows++;
                take[tRows - 1] += traderProperty[traderProperty.Count() - 1].Name;
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
                    PrintCard(player.Property[marker1], 113, 20);
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
                spaces = 19 - trader.Name.Length;
                left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                for (int i = 0; i < left; i++) Console.Write(" ");
                Console.Write($"ИМУЩЕСТВО {trader.Name}");
                for (int i = 0; i < right; i++) Console.Write(" ");
                if (marker2 == -1)
                {
                    PrintCard(null, 113, 20);
                }
                else
                {
                    PrintCard(trader.Property[marker2], 113, 20);
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

        [StructLayout(LayoutKind.Sequential)]
        struct COORD
        {
            public short X;
            public short Y;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool ReadConsoleOutputCharacter
        (
            IntPtr hConsoleOutput,
            [Out] char[] lpCharacter,
            int nLength,
            COORD dwReadCoord,
            out int lpNumberOfCharsRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        static void PrintQuartalLevel(Quartal quartal, int position)
        {
            int row, column;
            if (position <= 10)
            {
                row = 5;
                column = 4 + position * 14;
            }
            else if (position < 20)
            {
                row = 5 + 5 * (position - 10);
                column = 144;
            }
            else if (position <= 30)
            {
                row = 55;
                column = 144 - (position - 20) * 14;
            }
            else
            {
                row = 55 - 5 * (position - 30);
                column = 4;
            }

            COORD coords = new COORD();
            char[] readBuffer = new char[13];
            int charsRead;
            short interval = 1;
            short end = Convert.ToInt16(column + 13);

            Console.BackgroundColor = ConsoleColor.White;
            for (short i = Convert.ToInt16(column); i < column + 13; i++)
            {
                coords.X = i;
                coords.Y = Convert.ToInt16(row);
#pragma warning disable IDE0059 // Ненужное присваивание значения
                ReadConsoleOutputCharacter(GetStdHandle(-11), readBuffer, 1, coords, out charsRead);
#pragma warning restore IDE0059 // Ненужное присваивание значения
                Console.SetCursorPosition(i, row);
                Console.Write(readBuffer[0]);
            }

            if (quartal.IsMantaged)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
            }
            else if (quartal.Level == 0)
            {
                Console.BackgroundColor = ConsoleColor.White;
            }
            else if (quartal.Level == 5)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Green;
                interval = 4;
                end = Convert.ToInt16(column + 1 + 4 * (quartal.Level - 1));
            }

            for (short i = Convert.ToInt16(column); i < end; i += interval)
            {
                coords.X = i;
                coords.Y = Convert.ToInt16(row);
#pragma warning disable IDE0059 // Ненужное присваивание значения
                ReadConsoleOutputCharacter(GetStdHandle(-11), readBuffer, 13, coords, out charsRead);
#pragma warning restore IDE0059 // Ненужное присваивание значения
                Console.SetCursorPosition(i, row);
                Console.Write(readBuffer[0]);
            }
            Console.BackgroundColor = ConsoleColor.White;
        }

        static void PrintOwner(Quartal quartal)
        {
            int row, column;
            int position = Array.FindIndex(quartals, q => q == quartal);
            ConsoleColor bgColor;
            ConsoleColor fgColor = ConsoleColor.Black;
            if (position <= 10)
            {
                row = 2;
                column = 10 + position * 14;
            }
            else if (position < 20)
            {
                row = 2 + 5 * (position - 10);
                column = 150;
            }
            else if (position <= 30)
            {
                row = 52;
                column = 150 - (position - 20) * 14;
            }
            else
            {
                row = 52 - 5 * (position - 30);
                column = 10;
            }

            if (position % 5 == 0)
            {
                bgColor = ConsoleColor.Black;
                fgColor = ConsoleColor.White;
            }
            else if (position < 10)
            {
                if (position < 5) bgColor = ConsoleColor.Yellow;
                else bgColor = ConsoleColor.DarkYellow;
            }
            else if (position < 20)
            {
                if (position < 15)
                {
                    if (position == 12) bgColor = ConsoleColor.Gray;
                    else bgColor = ConsoleColor.DarkGreen;
                }
                else bgColor = ConsoleColor.Green;
            }
            else if (position < 30)
            {
                if (position < 25) bgColor = ConsoleColor.Red;
                else
                {
                    if (position == 28) bgColor = ConsoleColor.Gray;
                    else bgColor = ConsoleColor.Magenta;
                }
            }
            else
            {
                if (position < 35) bgColor = ConsoleColor.Blue;
                else bgColor = ConsoleColor.Cyan;
            }

            Console.SetCursorPosition(column, row);
            Console.BackgroundColor = bgColor;
            Console.ForegroundColor = fgColor;

            if (quartal.Owner == null) Console.Write(" ");
            else Console.Write(quartal.Owner.Piece);

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        static void Redeem(Quartal quartal, int turn)
        {
            PrintTitle();
            PrintCard(quartal);
            PrintPlayers(turn);
            ClearMenu();
            Console.SetCursorPosition(165, 50);
            Console.Write("╔═══════════════════════════════════════════════════════════════╗");
            Console.SetCursorPosition(165, 51);
            Console.Write("║ Эта улица заложена! Выкупить сразу или оплатить частично 10%? ║");
            Console.SetCursorPosition(165, 52);
            Console.Write("╠═══════════════════════════════════════════════════════════════╣");
            Console.SetCursorPosition(165, 53);
            Console.Write("║ R – Выкупить за                             P – Заплатить 10% ║");
            Console.SetCursorPosition(183, 53);
            Console.Write($"{quartal.Pledge + quartal.Pledge / 10}$");
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
            bool notPayed = true;
            while (notPayed)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.R:
                        if (quartal.Owner.Balance >= quartal.Pledge + quartal.Pledge / 10)
                        {
                            quartal.Redeem();
                            PrintQuartalLevel(quartal, Array.FindIndex(quartals, q => q == quartal));
                            notPayed = false;
                        }
                        break;
                    case ConsoleKey.P:
                        if (quartal.Owner.Balance >= quartal.Pledge / 10)
                        {
                            quartal.Owner.Pay(quartal.Pledge / 10);
                            notPayed = false;
                        }
                        break;
                    case ConsoleKey.M:
                        Console.SetCursorPosition(165, 54);
                        if (musicMuted)
                        {
                            musicMuted = false;
                            musicPlayer.Volume = 0.5;
                            Console.Write("║                      M – Заглушить музыку                     ║");
                        }
                        else
                        {
                            musicMuted = true;
                            musicPlayer.Volume = 0;
                            Console.Write("║                      M – Включить музыку                      ║");
                        }
                        break;
                }
            }
            PrintTitle();
            ClearMenu();
        }

        static void Trade(Player player)
        {
            Player[] traders = players.Where(trader => !trader.Equals(player) && !trader.IsBankrupt).ToArray();
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
                PrintTitle();
                PrintTraders(marker, traders);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        if (marker > 0) marker--;
                        PrintTraders(marker, traders);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        if (marker < traders.Length - 1) marker++;
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
                        int playerPropertyCount = player.Property.Count(p => p != null);
                        int traderPropertyCount = trader.Property.Count(p => p != null);
                        int playerLiberationCount = 0;
                        int traderLiberationCount = 0;

                        for (int i = 0; i < playerPropertyCount; i++)
                        {
                            if (player.Property[i].Level == 0) marker1 = i;
                        }
                        for (int i = 0; i < traderPropertyCount; i++)
                        {
                            if (trader.Property[i].Level == 0) marker2 = i;
                        }

                        Console.SetCursorPosition(165, 53);
                        Console.Write("║ AD – Выбрать                       WS – Выбрать тип имущества ║");
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
                            if (ownProperty) Console.Write($"P – Выбрать имущество \"{trader.Name}\"");
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
                                case ConsoleKey.LeftArrow:
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
                                                    if (player.Property[i].Level == 0)
                                                    {
                                                        marker1 = i;
                                                        notFound = false;
                                                    }
                                                }
                                                for (int i = playerPropertyCount - 1; i > marker1 && notFound; i--)
                                                {
                                                    if (player.Property[i].Level == 0)
                                                    {
                                                        marker1 = i;
                                                        notFound = false;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (marker2 != -1)
                                            {
                                                for (int i = marker2 - 1; i > 0 && notFound; i--)
                                                {
                                                    if (trader.Property[i].Level == 0)
                                                    {
                                                        marker2 = i;
                                                        notFound = false;
                                                    }
                                                }
                                                for (int i = traderPropertyCount - 1; i > marker2 && notFound; i--)
                                                {
                                                    if (trader.Property[i].Level == 0)
                                                    {
                                                        marker2 = i;
                                                        notFound = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (category == null)
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
                                    else
                                    {
                                        if (ownProperty)
                                        {
                                            if (yourMoney != 0) yourMoney--;
                                        }
                                        else
                                        {
                                            if (traderMoney != 0) traderMoney--;
                                        }
                                    }
                                    break;
                                case ConsoleKey.RightArrow:
                                case ConsoleKey.D:
                                    if (category == true)
                                    {
                                        bool notFound = true;
                                        if (ownProperty)
                                        {
                                            for (int i = marker1 + 1; i < playerPropertyCount && notFound; i++)
                                            {
                                                if (player.Property[i].Level == 0)
                                                {
                                                    marker1 = i;
                                                    notFound = false;
                                                }
                                            }
                                            for (int i = 0; i < marker1 && notFound; i++)
                                            {
                                                if (player.Property[i].Level == 0)
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
                                                if (trader.Property[i].Level == 0)
                                                {
                                                    marker2 = i;
                                                    notFound = false;
                                                }
                                            }
                                            for (int i = 0; i < marker2 && notFound; i++)
                                            {
                                                if (trader.Property[i].Level == 0)
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
                                            if (yourMoney < player.Balance) yourMoney++;
                                        }
                                        else
                                        {
                                            if (traderMoney < trader.Balance) traderMoney++;
                                        }
                                    }
                                    else
                                    {
                                        if (ownProperty)
                                        {
                                            if (playerLiberationCount < player.Liberation) playerLiberationCount++;
                                        }
                                        else
                                        {
                                            if (traderLiberationCount < trader.Liberation) traderLiberationCount++;
                                        }
                                    }
                                    break;
                                case ConsoleKey.UpArrow:
                                case ConsoleKey.W:
                                    if (category == false) category = null;
                                    else if (category == null) category = true;
                                    break;
                                case ConsoleKey.DownArrow:
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
                                    spaces = 34 - trader.Name.Length;
                                    left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                                    right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                                    Console.Write("║ ");
                                    for (int i = 0; i < left; i++) Console.Write(" ");
                                    Console.Write($"{trader.Name}, примите ли Вы эту сделку?");
                                    for (int i = 0; i < right; i++) Console.Write(" ");
                                    Console.Write(" ║");
                                    Console.SetCursorPosition(165, 52);
                                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                    Console.SetCursorPosition(165, 53);
                                    Console.Write("║ Enter – Принять сделку                  Esc – Отменить сделку ║");
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
                                    while (trading)
                                    {
                                        switch (Console.ReadKey(true).Key)
                                        {
                                            case ConsoleKey.Enter:
                                                successPlayer.Position = TimeSpan.Zero;
                                                successPlayer.Play();
                                                player.Pay(yourMoney);
                                                player.Receive(traderMoney);
                                                trader.Pay(traderMoney);
                                                trader.Receive(yourMoney);
                                                foreach (Quartal p in playerProperty)
                                                {
                                                    p.SetOwner(trader);
                                                    if (p.IsMantaged) Redeem(p, Array.FindIndex(players, m => m == trader));
                                                    PrintOwner(p);
                                                }
                                                foreach (Quartal p in traderProperty)
                                                {
                                                    p.SetOwner(player);
                                                    if (p.IsMantaged) Redeem(p, Array.FindIndex(players, m => m == player));
                                                    PrintOwner(p);
                                                }
                                                trading = false;
                                                trading2 = false;
                                                break;
                                            case ConsoleKey.Escape:
                                                cancelPlayer.Position = TimeSpan.Zero;
                                                cancelPlayer.Play();
                                                trading = false;
                                                trading2 = false;
                                                break;
                                            case ConsoleKey.M:
                                                Console.SetCursorPosition(165, 54);
                                                if (musicMuted)
                                                {
                                                    musicMuted = false;
                                                    musicPlayer.Volume = 0.5;
                                                    Console.Write("║                      M – Заглушить музыку                     ║");
                                                }
                                                else
                                                {
                                                    musicMuted = true;
                                                    musicPlayer.Volume = 0;
                                                    Console.Write("║                      M – Включить музыку                      ║");
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
                                                Quartal quartal = player.Property[marker1];
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
                                                Quartal quartal = trader.Property[marker2];
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
                                        musicPlayer.Volume = 0.5;
                                    }
                                    else
                                    {
                                        musicMuted = true;
                                        musicPlayer.Volume = 0;
                                    }
                                    break;
                            }
                        }
                        break;
                    case ConsoleKey.M:
                        if (musicMuted)
                        {
                            musicMuted = false;
                            musicPlayer.Volume = 0.5;
                        }
                        else
                        {
                            musicMuted = true;
                            musicPlayer.Volume = 0;
                        }
                        break;
                    case ConsoleKey.Backspace:
                        ClearMenu();
                        PrintTitle();
                        trading = false;
                        break;
                }
            }
            PrintTitle();
        }

        static void PrintCard(Quartal quartal)
        {
            PrintTitle();
            int row, i, spaces, left, right;
            string part1 = "", part2 = "";
            switch (quartal.Special)
            {
                case 6:
                    row = 23;
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╔═════════════════════════╗");
                    for (i = 0; quartal.Name[i] != ' '; i++)
                    {
                        part1 += quartal.Name[i];
                    }
                    for (i++; i < quartal.Name.Length; i++)
                    {
                        part2 += quartal.Name[i];
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
                    spaces = 25 - quartal.Name.Length;
                    left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                    right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║");
                    Console.BackgroundColor = ConsoleColor.Gray;
                    for (int j = 0; j < left; j++) Console.Write(" ");
                    Console.Write(quartal.Name);
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
                    spaces = 25 - quartal.Name.Length;
                    left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                    right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║");
                    Console.BackgroundColor = quartal.Color;
                    for (int j = 0; j < left; j++) Console.Write(" ");
                    Console.Write(quartal.Name);
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
                    if (quartal.NoMonopolyRent < 100) Console.Write(" ");
                    if (quartal.NoMonopolyRent < 10) Console.Write(" ");
                    Console.Write($"{quartal.NoMonopolyRent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ С цветовой группой  ");
                    if (quartal.MonopolyRent < 100) Console.Write(" ");
                    if (quartal.MonopolyRent < 10) Console.Write(" ");
                    Console.Write($"{quartal.MonopolyRent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ 1 дом               ");
                    if (quartal.House1Rent < 100) Console.Write(" ");
                    Console.Write($"{quartal.House1Rent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ 2 дома              ");
                    if (quartal.House2Rent < 100) Console.Write(" ");
                    Console.Write($"{quartal.House2Rent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ 3 дома             ");
                    if (quartal.House3Rent < 1000) Console.Write(" ");
                    if (quartal.House3Rent < 100) Console.Write(" ");
                    Console.Write($"{quartal.House3Rent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ 4 дома             ");
                    if (quartal.House4Rent < 1000) Console.Write(" ");
                    if (quartal.House4Rent < 100) Console.Write(" ");
                    Console.Write($"{quartal.House4Rent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ Отель              ");
                    if (quartal.HotelRent < 1000) Console.Write(" ");
                    if (quartal.HotelRent < 100) Console.Write(" ");
                    Console.Write($"{quartal.HotelRent} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╠═════════════════════════╣");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ Постройка дома      ");
                    if (quartal.HouseCost < 100) Console.Write(" ");
                    Console.Write($"{quartal.HouseCost} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ Постройка отеля     ");
                    if (quartal.HouseCost < 100) Console.Write(" ");
                    Console.Write($"{quartal.HouseCost} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║                 +4 дома ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("║ Закладная           ");
                    if (quartal.Pledge < 100) Console.Write(" ");
                    Console.Write($"{quartal.Pledge} ║");
                    Console.SetCursorPosition(69, row++);
                    Console.Write("╚═════════════════════════╝");
                    break;
            }
        }

        static void PrintCard(Quartal quartal, int column, int row)
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
                switch (quartal.Special)
                {
                    case 6:
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╔═════════════════════════╗");
                        for (i = 0; quartal.Name[i] != ' '; i++)
                        {
                            part1 += quartal.Name[i];
                        }
                        for (i++; i < quartal.Name.Length; i++)
                        {
                            part2 += quartal.Name[i];
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
                        spaces = 25 - quartal.Name.Length;
                        left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                        right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║");
                        Console.BackgroundColor = ConsoleColor.Gray;
                        for (int j = 0; j < left; j++) Console.Write(" ");
                        Console.Write(quartal.Name);
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
                        spaces = 25 - quartal.Name.Length;
                        left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                        right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║");
                        Console.BackgroundColor = quartal.Color;
                        for (int j = 0; j < left; j++) Console.Write(" ");
                        Console.Write(quartal.Name);
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
                        if (quartal.NoMonopolyRent < 100) Console.Write(" ");
                        if (quartal.NoMonopolyRent < 10) Console.Write(" ");
                        Console.Write($"{quartal.NoMonopolyRent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ С цветовой группой  ");
                        if (quartal.MonopolyRent < 100) Console.Write(" ");
                        if (quartal.MonopolyRent < 10) Console.Write(" ");
                        Console.Write($"{quartal.MonopolyRent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ 1 дом               ");
                        if (quartal.House1Rent < 100) Console.Write(" ");
                        Console.Write($"{quartal.House1Rent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ 2 дома              ");
                        if (quartal.House2Rent < 100) Console.Write(" ");
                        Console.Write($"{quartal.House2Rent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ 3 дома             ");
                        if (quartal.House3Rent < 1000) Console.Write(" ");
                        if (quartal.House3Rent < 100) Console.Write(" ");
                        Console.Write($"{quartal.House3Rent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ 4 дома             ");
                        if (quartal.House4Rent < 1000) Console.Write(" ");
                        if (quartal.House4Rent < 100) Console.Write(" ");
                        Console.Write($"{quartal.House4Rent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ Отель              ");
                        if (quartal.HotelRent < 1000) Console.Write(" ");
                        if (quartal.HotelRent < 100) Console.Write(" ");
                        Console.Write($"{quartal.HotelRent} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╠═════════════════════════╣");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ Постройка дома      ");
                        if (quartal.HouseCost < 100) Console.Write(" ");
                        Console.Write($"{quartal.HouseCost} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ Постройка отеля     ");
                        if (quartal.HouseCost < 100) Console.Write(" ");
                        Console.Write($"{quartal.HouseCost} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║                 +4 дома ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("║ Закладная           ");
                        if (quartal.Pledge < 100) Console.Write(" ");
                        Console.Write($"{quartal.Pledge} ║");
                        Console.SetCursorPosition(column, row++);
                        Console.Write("╚═════════════════════════╝");
                        break;
                }
            }
        }

        static void PrintTitle()
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

        static void ClearMenu()
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

        static void PrintRealty(Player player)
        {
            var property = new List<Quartal>();
            foreach (Quartal q in quartals)
            {
                if (player.Property.Contains(q)) property.Add(q);
            }
            bool upgradeable = true;
            bool downgradeable = true;
            bool mantageable = true;
            Quartal quartal = property[0];
            upgradeable = true;
            if (quartal.Special == 6 || quartal.Special == 7)
            {
                upgradeable = false;
                downgradeable = false;
                mantageable = !quartal.IsMantaged;
            }
            else
            {
                foreach (Quartal q in quartal.ColorGroup)
                {
                    if (q.IsMantaged)
                    {
                        upgradeable = false;
                        downgradeable = false;
                        mantageable = false;
                    }
                    else
                    {
                        if (q.Level < quartal.Level) upgradeable = false;
                        if (q.Level > quartal.Level) downgradeable = false;
                        if (q.Level > 0 || quartal.IsMantaged) mantageable = false;
                    }
                }
            }
            PrintCard(quartal);
            int quartalIndex = 0;
            ClearMenu();
            Console.SetCursorPosition(165, 50);
            Console.Write("╔═══════════════════════════════════════════════════════════════╗");
            Console.SetCursorPosition(165, 51);
            Console.Write("║                         ВАШЕ ИМУЩЕСТВО                        ║");
            Console.SetCursorPosition(165, 52);
            Console.Write("╠═══════════════════════════════════════════════════════════════╣");
            Console.SetCursorPosition(165, 53);
            Console.Write("║ AD – Переключить имущество                  Backspace – Назад ║");
            Console.SetCursorPosition(165, 54);
            Console.Write("║                                                               ║");
            Console.SetCursorPosition(167, 54);
            if (quartal.Level != 5 && quartal.IsMonopoly == true)
            {
                if (player.Balance < quartal.HouseCost || !upgradeable) Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"Enter – Улучшить за {quartal.HouseCost}$");
            }
            else if (quartal.IsMantaged)
            {
                if (player.Balance < quartal.Pledge + quartal.Pledge / 10) Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"Enter – Выкупить за {quartal.Pledge + quartal.Pledge / 10}$");
            }
            else Console.Write("                        ");
            Console.ForegroundColor = ConsoleColor.Black;
            if (quartal.IsMantaged)
            {
                Console.SetCursorPosition(193, 54);
                Console.Write("                                   ");
            }
            else
            {
                if (quartal.Level == 0)
                {
                    if (quartal.Pledge < 100)
                    {
                        Console.SetCursorPosition(204, 54);
                    }
                    else
                    {
                        Console.SetCursorPosition(203, 54);
                    }
                    if (!mantageable) Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"Пробел – Заложить за {quartal.Pledge}$");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    if (quartal.HouseCost / 2 < 100)
                    {
                        Console.SetCursorPosition(194, 54);
                    }
                    else
                    {
                        Console.SetCursorPosition(193, 54);
                    }
                    if (!downgradeable) Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"Пробел – Снести дом и получить {quartal.HouseCost / 2}$");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
            }
            Console.SetCursorPosition(165, 55);
            if (musicMuted)
            {
                Console.Write("║                      M – Включить музыку                      ║");
            }
            else
            {
                Console.Write("║                      M – Заглушить музыку                     ║");
            }
            Console.SetCursorPosition(165, 56);
            Console.Write("╚═══════════════════════════════════════════════════════════════╝");
            bool notReturned = true;
            int turn = Array.FindIndex(players, p => p == player);
            upgradeable = true;
            downgradeable = true;
            mantageable = true;
            if (quartal.Special == 6 || quartal.Special == 7)
            {
                upgradeable = false;
                downgradeable = false;
                if (quartal.IsMantaged) mantageable = false;
            }
            else
            {
                foreach (Quartal q in quartal.ColorGroup)
                {
                    if (q.IsMantaged)
                    {
                        upgradeable = false;
                        downgradeable = false;
                        mantageable = false;
                    }
                    else
                    {
                        if (q.Level < quartal.Level) upgradeable = false;
                        if (q.Level > quartal.Level) downgradeable = false;
                        if (q.Level > 0) mantageable = false;
                    }
                }
            }
            while (notReturned)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        if (quartalIndex == 0) quartalIndex = property.Count - 1;
                        else quartalIndex--;
                        quartal = property[quartalIndex];
                        upgradeable = true;
                        downgradeable = true;
                        mantageable = true;
                        if (quartal.Special == 6 || quartal.Special == 7)
                        {
                            upgradeable = false;
                            downgradeable = false;
                            mantageable = !quartal.IsMantaged;
                        }
                        else
                        {
                            foreach (Quartal q in quartal.ColorGroup)
                            {
                                if (q.IsMantaged)
                                {
                                    upgradeable = false;
                                    downgradeable = false;
                                }
                                else
                                {
                                    if (q.Level < quartal.Level) upgradeable = false;
                                    if (q.Level > quartal.Level) downgradeable = false;
                                    if (q.Level > 0) mantageable = false;
                                }
                            }
                        }
                        PrintCard(quartal);
                        Console.SetCursorPosition(165, 54);
                        Console.Write("║                                                               ║");
                        Console.SetCursorPosition(167, 54);
                        if (quartal.IsMantaged)
                        {
                            if (player.Balance < quartal.Pledge + quartal.Pledge / 10) Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write($"Enter – Выкупить за {quartal.Pledge + quartal.Pledge / 10}$");
                        }
                        else if (quartal.Level != 5 && quartal.IsMonopoly == true)
                        {
                            if (player.Balance < quartal.HouseCost || !upgradeable) Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write($"Enter – Улучшить за {quartal.HouseCost}$");
                        }
                        else Console.Write("                        ");
                        Console.ForegroundColor = ConsoleColor.Black;
                        if (quartal.IsMantaged)
                        {
                            Console.SetCursorPosition(193, 54);
                            Console.Write("                                   ");
                        }
                        else
                        {
                            if (quartal.Level == 0)
                            {
                                if (quartal.Pledge < 100)
                                {
                                    Console.SetCursorPosition(194, 54);
                                }
                                else
                                {
                                    Console.SetCursorPosition(193, 54);
                                }
                                if (!mantageable) Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write($"          Пробел – Заложить за {quartal.Pledge}$");
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                            else
                            {
                                if (quartal.HouseCost / 2 < 100)
                                {
                                    Console.SetCursorPosition(194, 54);
                                }
                                else
                                {
                                    Console.SetCursorPosition(193, 54);
                                }
                                if (!downgradeable) Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write($"Пробел – Снести дом и получить {quartal.HouseCost / 2}$");
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                        }
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        quartalIndex = (quartalIndex + 1) % property.Count;
                        quartal = property[quartalIndex];
                        upgradeable = true;
                        downgradeable = true;
                        mantageable = true;
                        if (quartal.Special == 6 || quartal.Special == 7)
                        {
                            upgradeable = false;
                            downgradeable = false;
                            mantageable = !quartal.IsMantaged;
                        }
                        else
                        {
                            foreach (Quartal q in quartal.ColorGroup)
                            {
                                if (q.IsMantaged)
                                {
                                    upgradeable = false;
                                    downgradeable = false;
                                }
                                else
                                {
                                    if (q.Level < quartal.Level) upgradeable = false;
                                    if (q.Level > quartal.Level) downgradeable = false;
                                    if (q.Level > 0) mantageable = false;
                                }
                            }
                        }
                        PrintTitle();
                        PrintCard(quartal);
                        Console.SetCursorPosition(165, 54);
                        Console.Write("║                                                               ║");
                        Console.SetCursorPosition(167, 54);
                        if (quartal.IsMantaged)
                        {
                            if (player.Balance < quartal.Pledge + quartal.Pledge / 10) Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write($"Enter – Выкупить за {quartal.Pledge + quartal.Pledge / 10}$");
                        }
                        else if (quartal.Level != 5 && quartal.IsMonopoly == true)
                        {
                            if (player.Balance < quartal.HouseCost || !upgradeable) Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write($"Enter – Улучшить за {quartal.HouseCost}$");
                        }
                        else Console.Write("                        ");
                        Console.ForegroundColor = ConsoleColor.Black;
                        if (quartal.IsMantaged)
                        {
                            Console.SetCursorPosition(193, 54);
                            Console.Write("                                   ");
                        }
                        else
                        {
                            if (quartal.Level == 0)
                            {
                                if (quartal.Pledge < 100)
                                {
                                    Console.SetCursorPosition(194, 54);
                                }
                                else
                                {
                                    Console.SetCursorPosition(193, 54);
                                }
                                if (!mantageable) Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write($"          Пробел – Заложить за {quartal.Pledge}$");
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                            else
                            {
                                if (quartal.HouseCost / 2 < 100)
                                {
                                    Console.SetCursorPosition(194, 54);
                                }
                                else
                                {
                                    Console.SetCursorPosition(193, 54);
                                }
                                if (!downgradeable) Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write($"Пробел – Снести дом и получить {quartal.HouseCost / 2}$");
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (quartal.IsMantaged)
                        {
                            if (player.Balance >= quartal.Pledge + quartal.Pledge / 10)
                            {
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                quartal.Redeem();
                                upgradeable = true;
                                downgradeable = false;
                                mantageable = true;
                                foreach (Quartal q in quartal.ColorGroup)
                                {
                                    if (q.IsMantaged) upgradeable = false;
                                }
                                PrintPlayers(turn);
                                Console.SetCursorPosition(167, 54);
                                if (quartal.Level != 5 && quartal.IsMonopoly == true)
                                {
                                    if (player.Balance < quartal.HouseCost || !upgradeable) Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.Write($"Enter – Улучшить за {quartal.HouseCost}$");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                }
                                else Console.Write("                        ");
                                if (quartal.Pledge < 100)
                                {
                                    Console.SetCursorPosition(204, 54);
                                }
                                else
                                {
                                    Console.SetCursorPosition(203, 54);
                                }
                                Console.Write($"Пробел – Заложить за {quartal.Pledge}$");
                            }
                        }
                        else
                        {
                            if (quartal.Level != 5 && quartal.IsMonopoly == true && quartal.Special != 6 && quartal.Special != 7)
                            {
                                if (player.Balance >= quartal.HouseCost && upgradeable)
                                {
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    upgradePlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                    upgradePlayer.Play();
                                    quartal.Upgrade();
                                    upgradeable = true;
                                    downgradeable = true;
                                    mantageable = false;
                                    foreach (Quartal q in quartal.ColorGroup)
                                    {
                                        if (q.Level < quartal.Level) upgradeable = false;
                                    }
                                    PrintPlayers(turn);
                                    Console.SetCursorPosition(167, 54);
                                    if (quartal.Level != 5 && quartal.IsMonopoly == true)
                                    {
                                        if (player.Balance < quartal.HouseCost || !upgradeable) Console.ForegroundColor = ConsoleColor.DarkGray;
                                        Console.Write($"Enter – Улучшить за {quartal.HouseCost}$");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                    }
                                    else
                                    {
                                        Console.Write("                        ");
                                    }
                                    if (quartal.HouseCost / 2 < 100)
                                    {
                                        Console.SetCursorPosition(194, 54);
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(193, 54);
                                    }
                                    Console.Write($"Пробел – Снести дом и получить {quartal.HouseCost / 2}$");
                                }
                            }
                        }
                        break;
                    case ConsoleKey.Spacebar:
                        if (!quartal.IsMantaged)
                        {
                            if (quartal.Level == 0 && mantageable)
                            {
                                padlockPlayer.Position = TimeSpan.Zero;
                                chainPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Position = TimeSpan.Zero;
                                padlockPlayer.Play();
                                chainPlayer.Play();
                                quartal.Mantage();
                                upgradeable = false;
                                downgradeable = false;
                                mantageable = false;
                                Thread.Sleep(100);
                                moneyPlayer.Play();
                                PrintPlayers(turn);
                                Console.SetCursorPosition(193, 54);
                                Console.Write("                                   ");
                                Console.SetCursorPosition(167, 54);
                                if (player.Balance < quartal.Pledge + quartal.Pledge / 10) Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write($"Enter – Выкупить за {quartal.Pledge + quartal.Pledge / 10}$ ");
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                            else if (downgradeable)
                            {
                                downgradePlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Position = TimeSpan.Zero;
                                downgradePlayer.Play();
                                moneyPlayer.Play();
                                quartal.Downgrade();
                                upgradeable = true;
                                downgradeable = true;
                                mantageable = true;
                                foreach (Quartal q in quartal.ColorGroup)
                                {
                                    if (q.Level > quartal.Level) downgradeable = false;
                                    if (q.Level > 0) mantageable = false;
                                }
                                PrintPlayers(turn);
                                Console.SetCursorPosition(167, 54);
                                if (player.Balance < quartal.HouseCost) Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write($"Enter – Улучшить за {quartal.HouseCost}$ ");
                                Console.ForegroundColor = ConsoleColor.Black;
                                if (quartal.Level == 0)
                                {
                                    if (quartal.Pledge < 100)
                                    {
                                        Console.SetCursorPosition(194, 54);
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(193, 54);
                                    }
                                    if (!mantageable) Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.Write($"          Пробел – Заложить за {quartal.Pledge}$");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                }
                                else
                                {
                                    if (quartal.HouseCost / 2 < 100)
                                    {
                                        Console.SetCursorPosition(194, 54);
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(193, 54);
                                    }
                                    if (!downgradeable) Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.Write($"Пробел – Снести дом и получить {quartal.HouseCost / 2}$ ");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                }
                            }
                        }
                        break;
                    case ConsoleKey.Backspace:
                        notReturned = false;
                        break;
                    case ConsoleKey.M:
                        Console.SetCursorPosition(165, 55);
                        if (musicMuted)
                        {
                            musicMuted = false;
                            musicPlayer.Play();
                            Console.Write("║                      M – Заглушить музыку                     ║");
                        }
                        else
                        {
                            musicMuted = true;
                            musicPlayer.Volume = 0;
                            Console.Write("║                      M – Включить музыку                      ║");
                        }
                        break;
                }
                PrintQuartalLevel(quartal, Array.FindIndex(quartals, q => q == quartal));
            }
            PrintTitle();
        }

        static int Rent(Quartal quartal, int dicePoints)
        {
            Player owner = quartal.Owner;
            if (quartal.IsMonopoly == true)
            {
                switch (quartal.Level)
                {
                    case 0:
                        return quartal.MonopolyRent;
                    case 1:
                        return quartal.House1Rent;
                    case 2:
                        return quartal.House2Rent;
                    case 3:
                        return quartal.House3Rent;
                    case 4:
                        return quartal.House4Rent;
                    default:
                        return quartal.HotelRent;
                }
            }
            else if (quartal.Color == ConsoleColor.Black)
            {
                switch (owner.Black)
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
            else if (quartal.Color == ConsoleColor.Gray)
            {
                if (owner.Gray == 1) return 4 * dicePoints;
                else return 10 * dicePoints;
            }
            else return quartal.NoMonopolyRent;
        }

        static void BuyOrAuctionOrRent(Player player, Quartal quartal, int turn)
        {
            PrintCard(quartal);
            if (quartal.Owner == null)
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
                if (quartal.Cost > player.Balance) Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Пробел – Купить");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("                      A – Выставить на аукцион ║");
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
                bool notEntered = true;
                while (notEntered)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Spacebar:
                            if (quartal.Cost <= player.Balance)
                            {
                                Purchase(player, quartal);
                                PrintTitle();
                                PrintPlayers(turn);
                                notEntered = false;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                        case ConsoleKey.A:
                            PrintTitle();
                            Auction(quartal, turn);
                            PrintPlayers(turn);
                            notEntered = false;
                            break;
                        case ConsoleKey.M:
                            if (musicMuted)
                            {
                                musicMuted = false;
                                musicPlayer.Volume = 0.5;
                                Console.SetCursorPosition(165, 54);
                                Console.Write("║                      M – Заглушить музыку                     ║");
                            }
                            else
                            {
                                musicMuted = true;
                                musicPlayer.Volume = 0;
                                Console.SetCursorPosition(165, 54);
                                Console.Write("║                      M – Включить музыку                      ║");
                            }
                            break;
                    }
                }
                Menu(player, turn);
            }
            else if (quartal.Owner == player)
            {
                Menu(player, turn, false, "Вы дома!");
            }
            else
            {
                if (!quartal.IsMantaged)
                {
                    ClearMenu();
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    int rent = Rent(quartal, dice1 + dice2);
                    string header = $"Заплатите ренту: {rent}$ игроку {quartal.Owner.Name}";
                    PrintMenu(header, rent, player);
                    bool notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                if (rent <= player.Balance)
                                {
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                    player.Pay(rent);
                                    quartal.Owner.Receive(rent);
                                    notEntered = false;
                                }
                                break;
                            case ConsoleKey.T:
                                Trade(player);
                                PrintPlayers(turn);
                                PrintMenu(header, rent, player);
                                PrintCard(quartal);
                                break;
                            case ConsoleKey.Tab:
                                if (player.Property.Count > 0)
                                {
                                    PrintRealty(player);
                                    PrintMenu(header, rent, player);
                                    PrintCard(quartal);
                                }
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicMuted = false;
                                    musicPlayer.Volume = 0.5;
                                }
                                else
                                {
                                    musicMuted = true;
                                    musicPlayer.Volume = 0;
                                }
                                PrintMenu(header, rent, player);
                                break;
                            case ConsoleKey.End:
                                sadPlayer.Position = TimeSpan.Zero;
                                sadPlayer.Play();
                                Bankrupt(player, quartal.Owner);
                                PrintTitle();
                                notEntered = false;
                                break;
                        }
                    }
                    PrintPlayers(turn);
                    Menu(player, turn);
                }
                else
                {
                    Menu(player, turn, false, $"Вы на заложенной собственности игрока {quartal.Owner.Name}");
                }
            }
        }

        static bool ThrowDices(Player player, int turn)
        {
            ClearMenu();
            dice1 = rnd.Next(1, 7);
            dice2 = rnd.Next(1, 7);
            if (player.IsPrisoned)
            {
                if (dice1 != dice2)
                {
                    PrintDices(dice1, dice2, 0);
                    player.DoEscapeAttempt();
                    Thread.Sleep(1500);
                    if (player.EscapeAttempts == 3)
                    {
                        Console.SetCursorPosition(165, 50);
                        Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                        Console.SetCursorPosition(165, 51);
                        Console.Write("║                              МЕНЮ                             ║");
                        Console.SetCursorPosition(165, 52);
                        Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                        Console.SetCursorPosition(165, 53);
                        Console.Write("║ ");
                        if (player.Balance < 50) Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("P – Заплатить 50$ за выход");
                        Console.ForegroundColor = ConsoleColor.Black;
                        if (player.Liberation == 0) Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("          L – Карточка освобождения");
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(" ║");
                        Console.SetCursorPosition(165, 54);
                        Console.Write("║ Tab – Посмотреть недвижимость            T – Предложить обмен ║");
                        Console.SetCursorPosition(165, 55);
                        if (musicMuted)
                        {
                            
                            Console.Write("║ End – Объявить себя банкротом             M – Включить музыку ║");
                        }
                        else
                        {
                            Console.Write("║ End – Объявить себя банкротом            M – Заглушить музыку ║");
                        }
                        Console.SetCursorPosition(165, 56);
                        Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                        bool notEntered = true;
                        while (notEntered)
                        {
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.T:
                                    Trade(player);
                                    PrintPlayers(turn);
                                    Console.SetCursorPosition(165, 50);
                                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                    Console.SetCursorPosition(165, 51);
                                    Console.Write("║                              МЕНЮ                             ║");
                                    Console.SetCursorPosition(165, 52);
                                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                    Console.SetCursorPosition(165, 53);
                                    Console.Write("║ ");
                                    if (player.Balance < 50) Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.Write("P – Заплатить 50$ за выход");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    if (player.Liberation == 0) Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.Write("          L – Карточка освобождения");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.Write(" ║");
                                    Console.SetCursorPosition(165, 54);
                                    Console.Write("║ Tab – Посмотреть недвижимость             T – Предложить обмен ║");
                                    Console.SetCursorPosition(165, 55);
                                    if (musicMuted)
                                    {

                                        Console.Write("║ End – Объявить себя банкротом             M – Включить музыку ║");
                                    }
                                    else
                                    {
                                        Console.Write("║ End – Объявить себя банкротом            M – Заглушить музыку ║");
                                    }
                                    Console.SetCursorPosition(165, 56);
                                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                    break;
                                case ConsoleKey.Tab:
                                    if (player.Property.Count > 0)
                                    {
                                        PrintRealty(player);
                                        Console.SetCursorPosition(165, 50);
                                        Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                        Console.SetCursorPosition(165, 51);
                                        Console.Write("║                              МЕНЮ                             ║");
                                        Console.SetCursorPosition(165, 52);
                                        Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                        Console.SetCursorPosition(165, 53);
                                        Console.Write("║ ");
                                        if (player.Balance < 50) Console.ForegroundColor = ConsoleColor.DarkGray;
                                        Console.Write("P – Заплатить 50$ за выход");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        if (player.Liberation == 0) Console.ForegroundColor = ConsoleColor.DarkGray;
                                        Console.Write("          L – Карточка освобождения");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.Write(" ║");
                                        Console.SetCursorPosition(165, 54);
                                        Console.Write("║ Tab – Посмотреть недвижимость             T – Предложить обмен ║");
                                        Console.SetCursorPosition(165, 55);
                                        if (musicMuted)
                                        {

                                            Console.Write("║ End – Объявить себя банкротом             M – Включить музыку ║");
                                        }
                                        else
                                        {
                                            Console.Write("║ End – Объявить себя банкротом            M – Заглушить музыку ║");
                                        }
                                        Console.SetCursorPosition(165, 56);
                                        Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                    }
                                    break;
                                case ConsoleKey.M:
                                    if (musicMuted)
                                    {
                                        musicMuted = false;
                                        musicPlayer.Volume = 0.5;
                                        Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                                    }
                                    else
                                    {
                                        musicMuted = true;
                                        musicPlayer.Volume = 0;
                                        Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                                    }
                                    break;
                                case ConsoleKey.P:
                                    if (player.Balance >= 50)
                                    {
                                        moneyPlayer.Position = TimeSpan.Zero;
                                        moneyPlayer.Play();
                                        player.Pay(50);
                                        player.LeavePrison();
                                        doubles = 0;
                                        repeat = true;
                                        Menu(player, turn);
                                        notEntered = false;
                                    }
                                    break;
                                case ConsoleKey.L:
                                    if (player.Liberation > 0)
                                    {
                                        paperPlayer.Position = TimeSpan.Zero;
                                        paperPlayer.Play();
                                        SpendLiberation(player);
                                        player.LeavePrison();
                                        doubles = 0;
                                        repeat = true;
                                        Menu(player, turn);
                                        notEntered = false;
                                    }
                                    break;
                                case ConsoleKey.End:
                                    Bankrupt(player, null);
                                    doubles = 0;
                                    notEntered = false;
                                    break;
                            }
                        }
                    }
                    return false;
                }
                
                player.LeavePrison();
            }
            if (dice1 == dice2)
            {
                doubles++;
                if (doubles == 3)
                {
                    policePlayer.Position = TimeSpan.Zero;
                    policePlayer.Play();
                    PrintDices(dice1, dice2, doubles);
                    Thread.Sleep(1500);
                    player.Arrest();
                    PrintPieces(player.Position, 10, player);
                    doubles = 0;
                    return false;
                }
                else
                {
                    repeat = true;
                }
            }
            else doubles = 0;
            PrintDices(dice1, dice2, doubles);
            return true;
        }

        static void PrintMenu(string header, int payment, Player player)
        {
            int spaces = 63 - header.Length;
            int left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
            int right = Convert.ToInt32(Math.Floor(spaces / 2.0));
            int column = 165;
            int row = 50;
            ClearMenu();
            Console.SetCursorPosition(column, row++);
            Console.Write("╔═══════════════════════════════════════════════════════════════╗");
            Console.SetCursorPosition(column, row);
            Console.Write("║");
            for (int i = 0; i < left; i++) Console.Write(" ");
            Console.Write(header);
            for (int i = 0; i < right; i++) Console.Write(" ");
            Console.Write("║");
            row++;
            Console.SetCursorPosition(column, row++);
            Console.Write("╠═══════════════════════════════════════════════════════════════╣");
            if (payment == 0)
            {
                Console.SetCursorPosition(column, row++);
                if (repeat)
                {
                    Console.Write("║ Пробел – Бросить кубики         Tab – Посмотреть недвижимость ║");
                }
                else
                {
                    Console.Write("║ Enter – Закончить ход           Tab – Посмотреть недвижимость ║");
                }
                Console.SetCursorPosition(column, row++);
                if (musicMuted)
                {
                    Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                }
                else
                {
                    Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                }
                if (player.IsPrisoned)
                {
                    Console.SetCursorPosition(column, row++);
                    Console.Write("║ ");
                    if (player.Balance < 50) Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("P – Заплатить 50$ за выход          ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    if (player.Liberation == 0) Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("L – Карточка освобождения");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" ║");
                }
            }
            else if (payment < 0)
            {
                Console.SetCursorPosition(column, row);
                if (musicMuted)
                {
                    Console.Write("║ Enter – Получить                          M – Включить музыку ║");
                }
                else
                {
                    Console.Write("║ Enter – Получить                         M – Заглушить музыку ║");
                }
                Console.SetCursorPosition(column + 19, row++);
                Console.Write($"{-payment}$");
            }
            else
            {
                Console.SetCursorPosition(column, row);
                Console.Write("║                                 Tab – Посмотреть недвижимость ║");
                Console.SetCursorPosition(column + 2, row++);
                if (player.Balance < payment) Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"Enter – Заплатить {payment}$");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(column, row++);
                if (musicMuted)
                {
                    Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                }
                else
                {
                    Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                }
                Console.SetCursorPosition(column, row++);
                Console.Write("║                 End – Объявить себя банкротом                 ║");
            }
            Console.SetCursorPosition(column, row++);
            Console.Write("╚═══════════════════════════════════════════════════════════════╝");
        }

        static void Menu(Player player, int turn, bool skip = false, string header = "МЕНЮ")
        {
            bool turnNotEnded = true;
            if (!skip)
            {
                PrintMenu(header, 0, player);
            }
            while (turnNotEnded && !player.IsBankrupt)
            {
                ConsoleKey key;
                if (skip) key = ConsoleKey.Spacebar;
                else key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.Spacebar:
                        if (!repeat) continue;
                        repeat = false;
                        turnNotEnded = ThrowDices(player, turn);
                        int oldPosition = player.Position;
                        int newPosition = (oldPosition + dice1 + dice2) % 40;
                        if (!player.IsPrisoned && turnNotEnded)
                        {
                            int next;
                            for (int j = oldPosition; j != newPosition;)
                            {
                                next = (j + 1) % 40;
                                PrintPieces(j, next, player);
                                Thread.Sleep(sleep);
                                j = next;
                            }
                            Quartal quartal = quartals[newPosition];
                            Quartal newQuartal;

                            if (newPosition < oldPosition)
                            {
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                player.Receive(200);
                                PrintPlayers(turn);
                            }

                            switch (quartal.Special)
                            {
                                case 0:
                                    happyPlayer.Position = TimeSpan.Zero;
                                    happyPlayer.Play();
                                    Menu(player, turn, false, "Вы пришли на Старт!");
                                    turnNotEnded = false;
                                    break;
                                case 1:
                                    newQuartal = Treasury(player, turn);
                                    PrintPlayers(turn);
                                    if (!player.IsPrisoned)
                                    {
                                        if (newQuartal != null) BuyOrAuctionOrRent(player, newQuartal, turn);
                                        else Menu(player, turn);
                                    }
                                    turnNotEnded = false;
                                    break;
                                case 2:
                                    newQuartal = Chance(player, turn);
                                    PrintPlayers(turn);
                                    if (!player.IsPrisoned)
                                    {
                                        if (newQuartal != null) BuyOrAuctionOrRent(player, newQuartal, turn);
                                        else Menu(player, turn);
                                    }
                                    turnNotEnded = false;
                                    break;
                                case 3:
                                    ClearMenu();
                                    Console.SetCursorPosition(165, 50);
                                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                    Console.SetCursorPosition(165, 51);
                                    Console.Write("║      Вам пришла мысль проведать знакомого. Вы посетитель      ║");
                                    Console.SetCursorPosition(165, 52);
                                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                    Console.SetCursorPosition(165, 53);
                                    if (repeat)
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
                                    while (turnNotEnded)
                                    {
                                        switch (Console.ReadKey(true).Key)
                                        {
                                            case ConsoleKey.Spacebar:
                                                if (repeat)
                                                {
                                                    Menu(player, turn, true);
                                                    turnNotEnded = false;
                                                }
                                                break;
                                            case ConsoleKey.Enter:
                                                if (!repeat) turnNotEnded = false;
                                                break;
                                            case ConsoleKey.Tab:
                                                if (player.Property.Count > 0)
                                                {
                                                    PrintRealty(player);
                                                    ClearMenu();
                                                    Console.SetCursorPosition(165, 50);
                                                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                                    Console.SetCursorPosition(165, 51);
                                                    Console.Write("║      Вам пришла мысль проведать знакомого. Вы посетитель      ║");
                                                    Console.SetCursorPosition(165, 52);
                                                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                                    Console.SetCursorPosition(165, 53);
                                                    if (repeat)
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
                                                }
                                                break;
                                            case ConsoleKey.T:
                                                Trade(player);
                                                PrintPlayers(turn);
                                                ClearMenu();
                                                Console.SetCursorPosition(165, 50);
                                                Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                                Console.SetCursorPosition(165, 51);
                                                Console.Write("║      Вам пришла мысль проведать знакомого. Вы посетитель      ║");
                                                Console.SetCursorPosition(165, 52);
                                                Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                                Console.SetCursorPosition(165, 53);
                                                if (repeat)
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
                                                break;
                                            case ConsoleKey.M:
                                                if (musicMuted)
                                                {
                                                    musicMuted = false;
                                                    musicPlayer.Volume = 0.5;
                                                    Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                                                }
                                                else
                                                {
                                                    musicMuted = true;
                                                    musicPlayer.Volume = 0;
                                                    Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case 4:
                                    ClearMenu();
                                    Console.SetCursorPosition(165, 50);
                                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                    Console.SetCursorPosition(165, 51);
                                    Console.Write("║             Ваша остановка! Бесплатная парковка               ║");
                                    Console.SetCursorPosition(165, 52);
                                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                    Console.SetCursorPosition(165, 53);
                                    if (repeat)
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
                                    while (turnNotEnded)
                                    {
                                        switch (Console.ReadKey(true).Key)
                                        {
                                            case ConsoleKey.Spacebar:
                                                if (repeat)
                                                {
                                                    Menu(player, turn, true);
                                                    turnNotEnded = false;
                                                }
                                                break;
                                            case ConsoleKey.Enter:
                                                if (!repeat)
                                                {
                                                    turnNotEnded = false;
                                                }
                                                break;
                                            case ConsoleKey.Tab:
                                                if (player.Property.Count > 0)
                                                {
                                                    PrintRealty(player);
                                                    ClearMenu();
                                                    Console.SetCursorPosition(165, 50);
                                                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                                    Console.SetCursorPosition(165, 51);
                                                    Console.Write("║             Ваша остановка! Бесплатная парковка               ║");
                                                    Console.SetCursorPosition(165, 52);
                                                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                                    if (repeat)
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
                                                }
                                                break;
                                            case ConsoleKey.T:
                                                Trade(player);
                                                PrintPlayers(turn);
                                                ClearMenu();
                                                Console.SetCursorPosition(165, 50);
                                                Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                                Console.SetCursorPosition(165, 51);
                                                Console.Write("║             Ваша остановка! Бесплатная парковка               ║");
                                                Console.SetCursorPosition(165, 52);
                                                Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                                if (repeat)
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
                                                break;
                                            case ConsoleKey.M:
                                                if (musicMuted)
                                                {
                                                    musicMuted = false;
                                                    musicPlayer.Volume = 0.5;
                                                    Console.Write("║ T – Предложить обмен                     M – Заглушить музыку ║");
                                                }
                                                else
                                                {
                                                    musicMuted = true;
                                                    musicPlayer.Volume = 0;
                                                    Console.Write("║ T – Предложить обмен                      M – Включить музыку ║");
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case 5:
                                    ClearMenu();
                                    policePlayer.Position = TimeSpan.Zero;
                                    policePlayer.Play();
                                    Console.SetCursorPosition(165, 50);
                                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                    Console.SetCursorPosition(165, 51);
                                    Console.Write("║                        Вас арестовали!                        ║");
                                    Console.SetCursorPosition(165, 52);
                                    Console.Write("║                     Отправляйтесь в тюрьму                    ║");
                                    Console.SetCursorPosition(165, 53);
                                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                    Thread.Sleep(1500);
                                    player.Arrest();
                                    PrintPieces(player.Position, 10, player);
                                    turnNotEnded = false;
                                    break;
                                case 6:
                                case 7:
                                case -1:
                                    PrintCard(quartal);
                                    BuyOrAuctionOrRent(player, quartal, turn);
                                    turnNotEnded = false;
                                    break;
                                case 8:
                                    sadPlayer.Position = TimeSpan.Zero;
                                    sadPlayer.Play();
                                    int payment = player.Balance + player.Liberation * 50;
                                    foreach (Quartal q in player.Property)
                                    {
                                        payment += q.Cost;
                                        payment += q.Level * q.HouseCost;
                                    }
                                    payment /= 10;
                                    ClearMenu();
                                    Console.SetCursorPosition(165, 50);
                                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                    Console.SetCursorPosition(165, 51);
                                    Console.Write("║     Налог! Вы заплатите 10% от своего имущества или 200$?     ║");
                                    Console.SetCursorPosition(165, 52);
                                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                    Console.SetCursorPosition(165, 53);
                                    Console.Write("║                                        ");
                                    if (player.Balance < payment) Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.Write("Пробел – Заплатить 10%");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.Write(" ║");
                                    Console.SetCursorPosition(165, 54);
                                    Console.Write("║ T – Предложить обмен           Tab – Посмотреть недвижимость  ║");
                                    Console.SetCursorPosition(167, 53);
                                    if (player.Balance < 200) Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.Write("Enter – Заплатить 200$");
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.SetCursorPosition(165, 55);
                                    if (musicMuted)
                                    {
                                        Console.Write("║ M – Включить музыку             End – Объявить себя банкротом ║");
                                    }
                                    else
                                    {
                                        Console.Write("║ M – Заглушить музыку            End – Объявить себя банкротом ║");
                                    }
                                    Console.SetCursorPosition(165, 56);
                                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                    while (turnNotEnded)
                                    {
                                        switch (Console.ReadKey(true).Key)
                                        {
                                            case ConsoleKey.Enter:
                                                if (player.Balance >= 200)
                                                {
                                                    moneyPlayer.Position = TimeSpan.Zero;
                                                    moneyPlayer.Play();
                                                    player.Pay(200);
                                                    turnNotEnded = false;
                                                }
                                                break;
                                            case ConsoleKey.Spacebar:

                                                if (player.Balance >= payment)
                                                {
                                                    moneyPlayer.Position = TimeSpan.Zero;
                                                    moneyPlayer.Play();
                                                    player.Pay(payment);
                                                    turnNotEnded = false;
                                                }
                                                break;
                                            case ConsoleKey.T:
                                                Trade(player);
                                                PrintPlayers(turn);
                                                ClearMenu();
                                                Console.SetCursorPosition(165, 50);
                                                Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                                Console.SetCursorPosition(165, 51);
                                                Console.Write("║      Налог! Вы заплатите 10% от своего баланса или 200$?      ║");
                                                Console.SetCursorPosition(165, 52);
                                                Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                                Console.SetCursorPosition(165, 53);
                                                Console.Write("║                                        ");
                                                if (player.Balance < payment) Console.ForegroundColor = ConsoleColor.Gray;
                                                Console.Write("Пробел – Заплатить 10%");
                                                Console.ForegroundColor = ConsoleColor.Black;
                                                Console.Write(" ║");
                                                Console.SetCursorPosition(165, 54);
                                                Console.Write("║ T – Предложить обмен           Tab – Посмотреть недвижимость  ║");
                                                Console.SetCursorPosition(167, 53);
                                                if (player.Balance < 200) Console.ForegroundColor = ConsoleColor.Gray;
                                                Console.Write("Enter – Заплатить 200$");
                                                Console.ForegroundColor = ConsoleColor.Black;
                                                Console.SetCursorPosition(165, 55);
                                                if (musicMuted)
                                                {
                                                    Console.Write("║ M – Включить музыку             End – Объявить себя банкротом ║");
                                                }
                                                else
                                                {
                                                    Console.Write("║ M – Заглушить музыку            End – Объявить себя банкротом ║");
                                                }
                                                Console.SetCursorPosition(165, 56);
                                                Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                                break;
                                            case ConsoleKey.Tab:
                                                if (player.Property.Count > 0)
                                                {
                                                    PrintRealty(player);
                                                    ClearMenu();
                                                    Console.SetCursorPosition(165, 50);
                                                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                                    Console.SetCursorPosition(165, 51);
                                                    Console.Write("║      Налог! Вы заплатите 10% от своего баланса или 200$?      ║");
                                                    Console.SetCursorPosition(165, 52);
                                                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                                    Console.SetCursorPosition(165, 53);
                                                    Console.Write("║                                        ");
                                                    if (player.Balance < payment) Console.ForegroundColor = ConsoleColor.Gray;
                                                    Console.Write("Пробел – Заплатить 10%");
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write(" ║");
                                                    Console.SetCursorPosition(165, 54);
                                                    Console.Write("║ T – Предложить обмен           Tab – Посмотреть недвижимость  ║");
                                                    Console.SetCursorPosition(167, 53);
                                                    if (player.Balance < 200) Console.ForegroundColor = ConsoleColor.Gray;
                                                    Console.Write("Enter – Заплатить 200$");
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.SetCursorPosition(165, 55);
                                                    if (musicMuted)
                                                    {
                                                        Console.Write("║ M – Включить музыку             End – Объявить себя банкротом ║");
                                                    }
                                                    else
                                                    {
                                                        Console.Write("║ M – Заглушить музыку            End – Объявить себя банкротом ║");
                                                    }
                                                    Console.SetCursorPosition(165, 56);
                                                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                                }
                                                break;
                                            case ConsoleKey.M:
                                                Console.SetCursorPosition(165, 55);
                                                if (musicMuted)
                                                {
                                                    musicPlayer.Volume = 0.5;
                                                    musicMuted = false;
                                                    Console.Write("║ M – Заглушить музыку            End – Объявить себя банкротом ║");
                                                }
                                                else
                                                {
                                                    musicPlayer.Volume = 0;
                                                    musicMuted = true;
                                                    Console.Write("║ M – Включить музыку             End – Объявить себя банкротом ║");
                                                }
                                                break;
                                            case ConsoleKey.End:
                                                Bankrupt(player, null);
                                                turnNotEnded = false;
                                                break;
                                        }
                                    }
                                    PrintPlayers(turn);
                                    Menu(player, turn);
                                    break;
                                case 9:
                                    sadPlayer.Position = TimeSpan.Zero;
                                    sadPlayer.Play();
                                    header = "В этот прекрасный день стоит пройтись по магазинам. С вас 100$";
                                    PrintMenu(header, 100, player);
                                    while (turnNotEnded)
                                    {
                                        switch (Console.ReadKey(true).Key)
                                        {
                                            case ConsoleKey.Enter:
                                                if (player.Balance >= 100)
                                                {
                                                    player.Pay(100);
                                                    PrintPlayers(turn);
                                                    moneyPlayer.Position = TimeSpan.Zero;
                                                    moneyPlayer.Play();
                                                    Menu(player, turn);
                                                    turnNotEnded = false;
                                                }
                                                break;
                                            case ConsoleKey.Tab:
                                                if (player.Property.Count > 0)
                                                {
                                                    PrintRealty(player);
                                                    PrintMenu(header, 100, player);
                                                }
                                                break;
                                            case ConsoleKey.T:
                                                Trade(player);
                                                PrintPlayers(turn);
                                                PrintMenu(header, 100, player);
                                                break;
                                            case ConsoleKey.M:
                                                if (musicMuted)
                                                {
                                                    musicMuted = false;
                                                    musicPlayer.Volume = 0.5;
                                                }
                                                else
                                                {
                                                    musicMuted = true;
                                                    musicPlayer.Volume = 0;
                                                }
                                                PrintMenu(header, 100, player);
                                                break;
                                            case ConsoleKey.End:
                                                Bankrupt(player, null);
                                                turnNotEnded = false;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (!repeat) turnNotEnded = false;
                        break;
                    case ConsoleKey.T:
                        Trade(player);
                        PrintPlayers(turn);
                        PrintMenu(header, 0, player);
                        break;
                    case ConsoleKey.Tab:
                        if (player.Property.Count > 0)
                        {
                            PrintRealty(player);
                            PrintMenu(header, 0, player);
                        }
                        break;
                    case ConsoleKey.M:
                        if (musicMuted)
                        {
                            musicMuted = false;
                            musicPlayer.Volume = 0.5;
                        }
                        else
                        {
                            musicMuted = true;
                            musicPlayer.Volume = 0;
                        }
                        PrintMenu(header, 0, player);
                        break;
                    case ConsoleKey.P:
                        if (player.IsPrisoned && player.Balance >= 50)
                        {
                            player.Pay(50);
                            player.LeavePrison();
                            turnNotEnded = false;
                        }
                        break;
                    case ConsoleKey.L:
                        if (player.IsPrisoned && player.Liberation > 0)
                        {
                            SpendLiberation(player);
                            player.LeavePrison();
                            turnNotEnded = false;
                        }
                        break;
                }
                PrintTitle();
            }
        }

        static void Purchase(Player player, Quartal quartal)
        {
            moneyPlayer.Position = TimeSpan.Zero;
            moneyPlayer.Play();
            quartal.SetOwner(player);
            PrintOwner(quartal);
        }

        static void Auction(Quartal quartal, int turn)
        {
            int spaces, left, right;
            int bid = quartal.Cost;
            int participants = players.Count();
            Player participant = null;
            Console.SetCursorPosition(69, 26);
            Console.Write("╔══════════════════════╗");
            Console.SetCursorPosition(69, 27);
            spaces = 22 - quartal.Name.Length;
            left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
            right = Convert.ToInt32(Math.Floor(spaces / 2.0));
            Console.Write("║");
            for (int j = 0; j < left; j++) Console.Write(" ");
            if (quartal.Special == 6)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (quartal.Special == 7) Console.BackgroundColor = ConsoleColor.Gray;
            else Console.BackgroundColor = quartal.Color;
            Console.Write(quartal.Name);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int j = 0; j < right; j++) Console.Write(" ");
            Console.Write("║");
            Console.SetCursorPosition(69, 28);
            Console.Write("╠══════════════════════╣");
            Console.SetCursorPosition(69, 29);
            Console.Write("║ ");
            if (quartal.Cost < 100) Console.Write(" ");
            Console.Write($"Начальная цена: {quartal.Cost}$");
            Console.Write(" ║");
            Console.SetCursorPosition(69, 30);
            Console.Write("║                      ║");
            Console.SetCursorPosition(69, 31);
            Console.Write("║ Ставка: ");
            for (int j = 0; j < 11 - Convert.ToString(bid).Length; j++) Console.Write(" ");
            Console.Write($"{bid}$ ║");
            Console.SetCursorPosition(69, 32);
            Console.Write("╚══════════════════════╝");
            int i = turn;
            while (participants > 1)
            {
                i = turn;
                do
                {
                    participant = players[i];
                    if (participant.IsBankrupt)
                    {
                        participant.CancelAuction();
                        participants--;
                    }
                    if (participant.Cancelled) continue;
                    PrintPlayers(i);
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                            АУКЦИОН                            ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 53);
                    Console.Write("║ ");
                    if (participant.Balance < bid + 1) Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("Tab – Поставить 1$");
                    Console.ForegroundColor = ConsoleColor.Black;
                    if (participant.Balance < bid + 10) Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("                      Enter – Поставить 10$ ║");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(165, 54);
                    Console.Write("║ ");
                    if (participant.Balance < bid + 100) Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("Пробел – Поставить 100$");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("       Backspace – Аннулировать ставку ║");
                    Console.SetCursorPosition(165, 55);
                    if (musicMuted)
                    {
                        Console.Write("║                      M – Включить музыку                      ║");
                    }
                    else
                    {
                        Console.Write("║                      M – Заглушить музыку                     ║");
                    }
                    Console.SetCursorPosition(165, 56);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    bool notChoosen = true;
                    while (notChoosen)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Tab:
                                if (participant.Balance >= bid + 1)
                                {
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                    bid++;
                                    spaces = 20 - participant.Name.Length;
                                    left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                                    Console.SetCursorPosition(69 + left, 30);
                                    Console.Write($"{participant.Name} {participant.Piece}");
                                    Console.SetCursorPosition(69, 31);
                                    Console.Write("║ Ставка: ");
                                    for (int j = 0; j < 11 - Convert.ToString(bid).Length; j++) Console.Write(" ");
                                    Console.Write($"{bid}$ ║");
                                    participant.DoBid();
                                    notChoosen = false;
                                }
                                break;
                            case ConsoleKey.Enter:
                                if (participant.Balance >= bid + 10)
                                {
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                    bid += 10;
                                    spaces = 20 - participant.Name.Length;
                                    left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                                    Console.SetCursorPosition(69 + left, 30);
                                    Console.Write($"{participant.Name} {participant.Piece}");
                                    Console.SetCursorPosition(69, 31);
                                    Console.Write("║ Ставка: ");
                                    for (int j = 0; j < 11 - Convert.ToString(bid).Length; j++) Console.Write(" ");
                                    Console.Write($"{bid}$ ║");
                                    participant.DoBid();
                                    notChoosen = false;
                                }
                                break;
                            case ConsoleKey.Spacebar:
                                if (participant.Balance >= bid + 100)
                                {
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                    bid += 100;
                                    spaces = 20 - participant.Name.Length;
                                    left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                                    Console.SetCursorPosition(69 + left, 30);
                                    Console.Write($"{participant.Name} {participant.Piece}");
                                    Console.SetCursorPosition(69, 31);
                                    Console.Write("║ Ставка: ");
                                    for (int j = 0; j < 11 - Convert.ToString(bid).Length; j++) Console.Write(" ");
                                    Console.Write($"{bid}$ ║");
                                    participant.DoBid();
                                    notChoosen = false;
                                }
                                break;
                            case ConsoleKey.Backspace:
                                cancelPlayer.Position = TimeSpan.Zero;
                                cancelPlayer.Play();
                                participants--;
                                participant.CancelAuction();
                                notChoosen = false;
                                break;
                            case ConsoleKey.M:
                                Console.SetCursorPosition(165, 55);
                                if (musicMuted)
                                {
                                    musicMuted = false;
                                    musicPlayer.Volume = 0.5;
                                    Console.Write("║                      M – Заглушить музыку                     ║");
                                }
                                else
                                {
                                    musicMuted = true;
                                    musicPlayer.Volume = 0;
                                    Console.Write("║                      M – Включить музыку                      ║");
                                }
                                break;
                        }
                    }
                    i = (i + 1) % playersCount;
                } while (i != turn && participants > 1);
            }

            foreach (Player p in players)
            {
                if (!p.Cancelled)
                {
                    participant = p;
                    break;
                }
            }

            if (participant.DidBid)
            {
                moneyPlayer.Position = TimeSpan.Zero;
                moneyPlayer.Play();
                quartal.SetOwner(participant, bid);
                PrintOwner(quartal);
                PrintPlayers(i);
                spaces = 54 - participant.Name.Length;
                left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                ClearMenu();
                Console.SetCursorPosition(165, 50);
                Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                Console.SetCursorPosition(165, 51);
                Console.Write("║                            АУКЦИОН                            ║");
                Console.SetCursorPosition(165, 52);
                Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                Console.SetCursorPosition(165, 52);
                Console.Write("║");
                for (i = 0; i < left; i++) Console.Write(" ");
                Console.Write($"Продано: {participant.Name}");
                for (i = 0; i < right; i++) Console.Write(" ");
                Console.Write("║");
                Console.SetCursorPosition(165, 53);
                Console.Write("╚═══════════════════════════════════════════════════════════════╝");
            }
            else
            {
                PrintPlayers(i);
                ClearMenu();
                Console.SetCursorPosition(165, 50);
                Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                Console.SetCursorPosition(165, 51);
                Console.Write("║                            АУКЦИОН                            ║");
                Console.SetCursorPosition(165, 52);
                Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                Console.SetCursorPosition(165, 53);
                Console.Write("║ ");
                if (participant.Balance < bid) Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"Enter – Купить за {bid}$");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(200, 53);
                Console.Write("Esc – Отказаться от сделки   ║");
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

                bool notChoosen = true;
                while (notChoosen)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Enter:
                            if (participant.Balance >= bid)
                            {
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                quartal.SetOwner(participant, bid);
                                PrintOwner(quartal);
                                PrintPlayers(i);
                                spaces = 54 - participant.Name.Length;
                                left = Convert.ToInt32(Math.Ceiling(spaces / 2.0));
                                right = Convert.ToInt32(Math.Floor(spaces / 2.0));
                                ClearMenu();
                                Console.SetCursorPosition(165, 50);
                                Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                Console.SetCursorPosition(165, 51);
                                Console.Write("║                            АУКЦИОН                            ║");
                                Console.SetCursorPosition(165, 52);
                                Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                Console.SetCursorPosition(165, 52);
                                Console.Write("║");
                                for (i = 0; i < left; i++) Console.Write(" ");
                                Console.Write($"Продано: {participant.Name}");
                                for (i = 0; i < right; i++) Console.Write(" ");
                                Console.Write("║");
                                Console.SetCursorPosition(165, 53);
                                Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                notChoosen = false;
                            }
                            break;
                        case ConsoleKey.Escape:
                            cancelPlayer.Position = TimeSpan.Zero;
                            cancelPlayer.Play();
                            ClearMenu();
                            Console.SetCursorPosition(165, 50);
                            Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                            Console.SetCursorPosition(165, 51);
                            Console.Write("║                            АУКЦИОН                            ║");
                            Console.SetCursorPosition(165, 52);
                            Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                            Console.SetCursorPosition(165, 52);
                            Console.Write("║                     Улица осталась у банка                    ║");
                            Console.SetCursorPosition(165, 53);
                            Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                            notChoosen = false;
                            break;
                        case ConsoleKey.M:
                            Console.SetCursorPosition(165, 54);
                            if (musicMuted)
                            {
                                musicMuted = false;
                                musicPlayer.Volume = 0.5;
                                Console.Write("║                      M – Заглушить музыку                     ║");
                            }
                            else
                            {
                                musicMuted = true;
                                musicPlayer.Volume = 0;
                                Console.Write("║                      M – Включить музыку                      ║");
                            }
                            break;
                    }
                }
            }
            Thread.Sleep(1500);

            for (i = 0; i < playersCount; i++)
            {
                players[i].EndAuction();
            }
            PrintTitle();
        }

        static void SpendLiberation(Player player)
        {
            player.SpendLiberation();
            if (treasuries.Count == 15)
            {
                treasuries.Add(4);
            }
            else
            {
                chances.Add(4);
            }
            PrintPlayers(Array.FindIndex(players, p => p == player));
        }

        static void Bankrupt(Player debtor, Player player)
        {
            sadPlayer.Position = TimeSpan.Zero;
            sadPlayer.Play();

            PrintPieces(debtor.Position, -1, debtor);
            var property = debtor.Bankrupt(player);
            if (property != null)
            {
                int playerIndex = Array.FindIndex(players, m => m == player);
                foreach (Quartal p in property)
                {
                    if (p.IsMantaged) Redeem(p, playerIndex);
                }
            }

            remaining--;
            foreach (Player p in players)
            {
                if (p.IsBankrupt) p.IncreasePlace();
            }

            for (int i = 0; i < debtor.Liberation; i++)
            {
                SpendLiberation(debtor);
            }
        }

        static Quartal Treasury(Player player, int turn)
        {
            int card = treasuries[0];
            bool notEntered;
            int payment;
            string header;
            treasuries.RemoveAt(0);
            treasuries.Add(card);
            switch (card)
            {
                case 0:
                case 8:
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    notEntered = true;
                    header = "Выгодная продажа акций! Получите 25$";
                    payment = -25;
                    PrintMenu(header, payment, player);
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                player.Receive(25);
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                        }
                    }
                    return null;
                case 1:
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    header = "Пора платить за страховку! Заплатите 50$";
                    payment = 50;
                    PrintMenu(header, payment, player);
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                if (player.Balance >= 50)
                                {
                                    player.Pay(50);
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                    notEntered = false;
                                }
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                            case ConsoleKey.T:
                                Trade(player);
                                PrintPlayers(turn);
                                PrintMenu(header, payment, player);
                                break;
                            case ConsoleKey.Tab:
                                if (player.Property.Count > 0)
                                {
                                    PrintRealty(player);
                                    PrintMenu(header, payment, player);
                                }
                                break;
                            case ConsoleKey.End:
                                Bankrupt(player, null);
                                notEntered = false;
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
                    Console.SetCursorPosition(165, 53);
                    if (musicMuted)
                    {
                        Console.Write("║ Enter – Вернуться на Старую дорогу        M – Включить музыку ║");
                    }
                    else
                    {
                        Console.Write("║ Enter – Вернуться на Старую дорогу       M – Заглушить музыку ║");
                    }
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                PrintPieces(player.Position, 1, player);
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                Console.SetCursorPosition(165, 53);
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                    Console.Write("║ Enter – Вернуться на Старую дорогу       M – Заглушить музыку ║");
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                    Console.Write("║ Enter – Вернуться на Старую дорогу        M – Включить музыку ║");
                                }
                                break;
                        }
                    }
                    return quartals[1];
                case 3:
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    header = "У вас простуда! Заплатите за услуги доктора 50$";
                    payment = 50;
                    PrintMenu(header, payment, player);
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                if (player.Balance >= 50)
                                {
                                    player.Pay(50);
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                    notEntered = false;
                                }
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                            case ConsoleKey.T:
                                Trade(player);
                                PrintPlayers(turn);
                                PrintMenu(header, payment, player);
                                break;
                            case ConsoleKey.Tab:
                                if (player.Property.Count > 0)
                                {
                                    PrintRealty(player);
                                    PrintMenu(header, payment, player);
                                }
                                break;
                            case ConsoleKey.End:
                                Bankrupt(player, null);
                                notEntered = false;
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
                    Console.SetCursorPosition(165, 54);
                    if (musicMuted)
                    {
                        Console.Write("║ Enter – Получить                          M – Включить музыку ║");
                    }
                    else
                    {
                        Console.Write("║ Enter – Получить                         M – Заглушить музыку ║");
                    }
                    Console.SetCursorPosition(165, 55);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                paperPlayer.Position = TimeSpan.Zero;
                                paperPlayer.Play();
                                player.GetLiberation();
                                treasuries.Remove(4);
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                Console.SetCursorPosition(165, 54);
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                    Console.Write("║ Enter – Получить                         M – Заглушить музыку ║");
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                    Console.Write("║ Enter – Получить                          M – Включить музыку ║");
                                }
                                break;
                        }
                    }
                    return null;
                case 5:
                    ClearMenu();
                    policePlayer.Position = TimeSpan.Zero;
                    policePlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                        Вас арестовали!                        ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("║                     Отправляйтесь в тюрьму                    ║");
                    Console.SetCursorPosition(165, 53);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    Thread.Sleep(1500);
                    player.Arrest();
                    PrintPieces(player.Position, 10, player);
                    return null;
                case 6:
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    header = "Вы сломали ногу! Заплатите за услуги доктора 100$";
                    payment = 100;
                    PrintMenu(header, payment, player);
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                if (player.Balance >= 100)
                                {
                                    player.Pay(100);
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                    notEntered = false;
                                }
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                            case ConsoleKey.T:
                                Trade(player);
                                PrintPlayers(turn);
                                PrintMenu(header, payment, player);
                                break;
                            case ConsoleKey.Tab:
                                if (player.Property.Count > 0)
                                {
                                    PrintRealty(player);
                                    PrintMenu(header, payment, player);
                                }
                                break;
                            case ConsoleKey.End:
                                Bankrupt(player, null);
                                notEntered = false;
                                break;
                        }
                    }
                    return null;
                case 7:
                    ClearMenu();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║            Заплатите штраф 10$ или возьмите \"Шанс\"            ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                    Console.SetCursorPosition(165, 53);
                    Console.Write("║ ");
                    if (player.Balance < 10) Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("Пробел – Заплатить штраф 10$");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("             Enter – Взять \"Шанс\" ║");
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
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Spacebar:
                                if (player.Balance >= 10)
                                {
                                    player.Pay(10);
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                }
                                break;
                            case ConsoleKey.Enter:
                                return Chance(player, turn);
                            case ConsoleKey.M:
                                Console.SetCursorPosition(165, 54);
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                    Console.Write("║                      M – Заглушить музыку                     ║");
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                    Console.Write("║                      M – Включить музыку                      ║");
                                }
                                break;
                        }
                    }
                    return null;
                case 9:
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    notEntered = true;
                    header = "Возмещение налога! Получите 25$";
                    payment = -25;
                    PrintMenu(header, payment, player);
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                player.Receive(25);
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                        }
                    }
                    return null;
                case 10:
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    notEntered = true;
                    header = "Выгодная продажа облигаций! Получите 50$";
                    payment = -50;
                    PrintMenu(header, payment, player);
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                player.Receive(50);
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                        }
                    }
                    return null;
                case 11:
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    notEntered = true;
                    header = "У вас день рождения! Получите 10$ с каждого игрока";
                    payment = -10 * (playersCount - 1);
                    PrintMenu(header, payment, player);
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                foreach (Player p in players)
                                {
                                    if (!p.IsBankrupt)
                                    {
                                        p.Pay(10);
                                        player.Receive(10);
                                    }
                                }
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                        }
                    }

                    for (int i = 0; i < playersCount; i++)
                    {
                        Player debtor = players[i];
                        if (debtor.Balance < 0)
                        {
                            PrintPlayers(i);
                            header = "Вы задолжали своему другу!";
                            payment = -debtor.Balance;
                            PrintMenu(header, payment, debtor);
                            bool notPayed = true;
                            while (notPayed)
                            {
                                switch (Console.ReadKey(true).Key)
                                {
                                    case ConsoleKey.Enter:
                                        notPayed = false;
                                        break;
                                    case ConsoleKey.M:
                                        if (musicMuted)
                                        {
                                            musicPlayer.Volume = 0.5;
                                            musicMuted = false;
                                        }
                                        else
                                        {
                                            musicPlayer.Volume = 0;
                                            musicMuted = true;
                                        }
                                        PrintMenu(header, payment, debtor);
                                        break;
                                    case ConsoleKey.T:
                                        Trade(player);
                                        PrintPlayers(turn);
                                        PrintMenu(header, payment, debtor);
                                        break;
                                    case ConsoleKey.Tab:
                                        if (player.Property.Count > 0)
                                        {
                                            PrintRealty(player);
                                            PrintMenu(header, payment, debtor);
                                        }
                                        break;
                                    case ConsoleKey.End:
                                        Bankrupt(debtor, player);
                                        notPayed = false;
                                        break;
                                }
                            }
                        }
                    }

                    return null;
                case 12:
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    notEntered = true;
                    header = "Банковская ошибка в Вашу пользу! Получите 200$";
                    payment = -200;
                    PrintMenu(header, payment, player);
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                player.Receive(200);
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                        }
                    }
                    return null;
                case 13:
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    notEntered = true;
                    header = "На Ваше имя подписали завещание! Получите 100$";
                    payment = -100;
                    PrintMenu(header, payment, player);
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                player.Receive(100);
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                        }
                    }
                    return null;
                case 14:
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    notEntered = true;
                    header = "Вы заняли второе место на конкурсе красоты! Получите 10$";
                    payment = -10;
                    PrintMenu(header, payment, player);
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                player.Receive(10);
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                        }
                    }
                    return null;
                default:
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    notEntered = true;
                    header = "Сбор ренты! Получите 100$";
                    payment = -100;
                    PrintMenu(header, payment, player);
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                player.Receive(100);
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                        }
                    }
                    return null;
            }
        }

        static Quartal Chance(Player player, int turn)
        {
            int card = chances[0];
            int payment;
            int position = player.Position;
            string header;
            bool notEntered;
            chances.RemoveAt(0);
            chances.Add(card);
            switch (card)
            {
                case 0:
                    notEntered = true;
                    header = "Заплатите 40$ за каждый дом и 115$ за каждый отель";
                    payment = 40 * player.HousesCount + 115 * player.HotelsCount;
                    if (payment > 0)
                    {
                        sadPlayer.Position = TimeSpan.Zero;
                        sadPlayer.Play();
                        PrintMenu(header, payment, player);
                        while (notEntered)
                        {
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.Enter:
                                    if (player.Balance >= payment)
                                    {
                                        player.Pay(payment);
                                        moneyPlayer.Position = TimeSpan.Zero;
                                        moneyPlayer.Play();
                                        notEntered = false;
                                    }
                                    break;
                                case ConsoleKey.M:
                                    if (musicMuted)
                                    {
                                        musicPlayer.Volume = 0.5;
                                        musicMuted = false;
                                    }
                                    else
                                    {
                                        musicPlayer.Volume = 0;
                                        musicMuted = true;
                                    }
                                    PrintMenu(header, payment, player);
                                    break;
                                case ConsoleKey.T:
                                    Trade(player);
                                    PrintPlayers(turn);
                                    PrintMenu(header, payment, player);
                                    break;
                                case ConsoleKey.Tab:
                                    if (player.Property.Count > 0)
                                    {
                                        PrintRealty(player);
                                        PrintMenu(header, payment, player);
                                    }
                                    break;
                                case ConsoleKey.End:
                                    Bankrupt(player, null);
                                    notEntered = false;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        ClearMenu();
                        Console.SetCursorPosition(165, 50);
                        Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                        Console.SetCursorPosition(165, 51);
                        Console.Write("║       Заплатите 40$ за каждый дом и 115$ за каждый отель      ║");
                        Console.SetCursorPosition(165, 52);
                        Console.Write("║                    У вас нет отелей и домов                   ║");
                        Console.SetCursorPosition(165, 53);
                        Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                        Thread.Sleep(1500);
                    }
                    return null;
                case 1:
                    ClearMenu();
                    policePlayer.Position = TimeSpan.Zero;
                    policePlayer.Play();
                    Console.SetCursorPosition(165, 50);
                    Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                    Console.SetCursorPosition(165, 51);
                    Console.Write("║                        Вас арестовали!                        ║");
                    Console.SetCursorPosition(165, 52);
                    Console.Write("║                     Отправляйтесь в тюрьму                    ║");
                    Console.SetCursorPosition(165, 53);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    Thread.Sleep(1500);
                    player.Arrest();
                    PrintPieces(position, 10, player);
                    return null;
                case 2:
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    header = "Штраф за вождение в нетрезвом виде! Заплатите 20$";
                    payment = 20;
                    PrintMenu(header, payment, player);
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                if (player.Balance >= 20)
                                {
                                    player.Pay(20);
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                    notEntered = false;
                                }
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                            case ConsoleKey.T:
                                Trade(player);
                                PrintPlayers(turn);
                                PrintMenu(header, payment, player);
                                break;
                            case ConsoleKey.Tab:
                                if (player.Property.Count > 0)
                                {
                                    PrintRealty(player);
                                    PrintMenu(header, payment, player);
                                }
                                break;
                            case ConsoleKey.End:
                                Bankrupt(player, null);
                                notEntered = false;
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
                    Console.SetCursorPosition(165, 53);
                    if (musicMuted)
                    {
                        Console.Write("║ Enter – Вернуться                         M – Включить музыку ║");
                    }
                    else
                    {
                        Console.Write("║ Enter – Вернуться                        M – Заглушить музыку ║");
                    }
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                
                                for (int j = position; j != position - 3; j--)
                                {
                                    PrintPieces(j, j - 1, player);
                                    Thread.Sleep(sleep);
                                }
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                Console.SetCursorPosition(165, 53);
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                    Console.Write("║ Enter – Вернуться                       M – Заглушить музыку ║");
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                    Console.Write("║ Enter – Вернуться                         M – Включить музыку ║");
                                }
                                break;
                        }
                    }
                    switch (quartals[position - 3].Special)
                    {
                        case 1:
                            return Treasury(player, turn);
                        case 8:
                            sadPlayer.Position = TimeSpan.Zero;
                            sadPlayer.Play();
                            payment = player.Balance + player.Liberation * 50;
                            foreach (Quartal q in player.Property)
                            {
                                payment += q.Cost;
                                payment += q.Level * q.HouseCost;
                            }
                            payment /= 10;
                            ClearMenu();
                            Console.SetCursorPosition(165, 50);
                            Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                            Console.SetCursorPosition(165, 51);
                            Console.Write("║     Налог! Вы заплатите 10% от своего имущества или 200$?     ║");
                            Console.SetCursorPosition(165, 52);
                            Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                            Console.SetCursorPosition(165, 53);
                            Console.Write("║                                        ");
                            if (player.Balance < payment) Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write("Пробел – Заплатить 10%");
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(" ║");
                            Console.SetCursorPosition(165, 54);
                            Console.Write("║ T – Предложить обмен           Tab – Посмотреть недвижимость  ║");
                            Console.SetCursorPosition(167, 53);
                            if (player.Balance < 200) Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write("Enter – Заплатить 200$");
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(165, 55);
                            if (musicMuted)
                            {
                                Console.Write("║ M – Включить музыку             End – Объявить себя банкротом ║");
                            }
                            else
                            {
                                Console.Write("║ M – Заглушить музыку            End – Объявить себя банкротом ║");
                            }
                            Console.SetCursorPosition(165, 56);
                            Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                            bool turnNotEnded = true;
                            while (turnNotEnded)
                            {
                                switch (Console.ReadKey(true).Key)
                                {
                                    case ConsoleKey.Enter:
                                        if (player.Balance >= 200)
                                        {
                                            moneyPlayer.Position = TimeSpan.Zero;
                                            moneyPlayer.Play();
                                            player.Pay(200);
                                            turnNotEnded = false;
                                        }
                                        break;
                                    case ConsoleKey.Spacebar:

                                        if (player.Balance >= payment)
                                        {
                                            moneyPlayer.Position = TimeSpan.Zero;
                                            moneyPlayer.Play();
                                            player.Pay(payment);
                                            turnNotEnded = false;
                                        }
                                        break;
                                    case ConsoleKey.T:
                                        Trade(player);
                                        PrintPlayers(turn);
                                        ClearMenu();
                                        Console.SetCursorPosition(165, 50);
                                        Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                        Console.SetCursorPosition(165, 51);
                                        Console.Write("║      Налог! Вы заплатите 10% от своего баланса или 200$?      ║");
                                        Console.SetCursorPosition(165, 52);
                                        Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                        Console.SetCursorPosition(165, 53);
                                        Console.Write("║                                        ");
                                        if (player.Balance < payment) Console.ForegroundColor = ConsoleColor.Gray;
                                        Console.Write("Пробел – Заплатить 10%");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.Write(" ║");
                                        Console.SetCursorPosition(165, 54);
                                        Console.Write("║ T – Предложить обмен           Tab – Посмотреть недвижимость  ║");
                                        Console.SetCursorPosition(167, 53);
                                        if (player.Balance < 200) Console.ForegroundColor = ConsoleColor.Gray;
                                        Console.Write("Enter – Заплатить 200$");
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.SetCursorPosition(165, 55);
                                        if (musicMuted)
                                        {
                                            Console.Write("║ M – Включить музыку             End – Объявить себя банкротом ║");
                                        }
                                        else
                                        {
                                            Console.Write("║ M – Заглушить музыку            End – Объявить себя банкротом ║");
                                        }
                                        Console.SetCursorPosition(165, 56);
                                        Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                        break;
                                    case ConsoleKey.Tab:
                                        if (player.Property.Count > 0)
                                        {
                                            PrintRealty(player);
                                            ClearMenu();
                                            Console.SetCursorPosition(165, 50);
                                            Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                                            Console.SetCursorPosition(165, 51);
                                            Console.Write("║      Налог! Вы заплатите 10% от своего баланса или 200$?      ║");
                                            Console.SetCursorPosition(165, 52);
                                            Console.Write("╠═══════════════════════════════════════════════════════════════╣");
                                            Console.SetCursorPosition(165, 53);
                                            Console.Write("║                                        ");
                                            if (player.Balance < payment) Console.ForegroundColor = ConsoleColor.Gray;
                                            Console.Write("Пробел – Заплатить 10%");
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.Write(" ║");
                                            Console.SetCursorPosition(165, 54);
                                            Console.Write("║ T – Предложить обмен           Tab – Посмотреть недвижимость  ║");
                                            Console.SetCursorPosition(167, 53);
                                            if (player.Balance < 200) Console.ForegroundColor = ConsoleColor.Gray;
                                            Console.Write("Enter – Заплатить 200$");
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.SetCursorPosition(165, 55);
                                            if (musicMuted)
                                            {
                                                Console.Write("║ M – Включить музыку             End – Объявить себя банкротом ║");
                                            }
                                            else
                                            {
                                                Console.Write("║ M – Заглушить музыку            End – Объявить себя банкротом ║");
                                            }
                                            Console.SetCursorPosition(165, 56);
                                            Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                                        }
                                        break;
                                    case ConsoleKey.M:
                                        Console.SetCursorPosition(165, 55);
                                        if (musicMuted)
                                        {
                                            musicPlayer.Volume = 0.5;
                                            musicMuted = false;
                                            Console.Write("║ M – Заглушить музыку            End – Объявить себя банкротом ║");
                                        }
                                        else
                                        {
                                            musicPlayer.Volume = 0;
                                            musicMuted = true;
                                            Console.Write("║ M – Включить музыку             End – Объявить себя банкротом ║");
                                        }
                                        break;
                                    case ConsoleKey.End:
                                        Bankrupt(player, null);
                                        turnNotEnded = false;
                                        break;
                                }
                            }
                            PrintPlayers(turn);
                            return null;
                    }
                    return quartals[position - 3];
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
                    Console.SetCursorPosition(165, 54);
                    if (musicMuted)
                    {
                        Console.Write("║ Enter – Получить                          M – Включить музыку ║");
                    }
                    else
                    {
                        Console.Write("║ Enter – Получить                         M – Заглушить музыку ║");
                    }
                    Console.SetCursorPosition(165, 55);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                paperPlayer.Position = TimeSpan.Zero;
                                paperPlayer.Play();
                                player.GetLiberation();
                                chances.Remove(4);
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                Console.SetCursorPosition(165, 53);
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                    Console.Write("║ Enter – Получить                         M – Заглушить музыку ║");
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                    Console.Write("║ Enter – Получить                          M – Включить музыку ║");
                                }
                                break;
                        }
                    }
                    return null;
                case 5:;
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    notEntered = true;
                    header = "Возврат займа! Получите 150$";
                    payment = -150;
                    PrintMenu(header, payment, player);
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                player.Receive(150);
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                        }
                    }
                    return null;
                case 6:
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    notEntered = true;
                    header = "Пришли банковские дивиденды! Получите 50$";
                    payment = -50;
                    PrintMenu(header, payment, player);
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                player.Receive(50);
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
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
                    Console.SetCursorPosition(165, 54);
                    if (musicMuted)
                    {
                        Console.Write("║ Enter – Отправиться в Аквапарк            M – Включить музыку ║");
                    }
                    else
                    {
                        Console.Write("║ Enter – Отправиться в Аквапарк           M – Заглушить музыку ║");
                    }
                    Console.SetCursorPosition(165, 55);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                PrintPieces(position, 6, player);
                                if (position > 6)
                                {
                                    player.Receive(200);
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                }
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                Console.SetCursorPosition(165, 54);
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                    Console.Write("║ Enter – Отправиться в Аквапарк           M – Заглушить музыку ║");
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                    Console.Write("║ Enter – Отправиться в Аквапарк            M – Включить музыку ║");
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
                    Console.SetCursorPosition(165, 53);
                    if (musicMuted)
                    {
                        Console.Write("║ Enter – Пройти на Старт                   M – Включить музыку ║");
                    }
                    else
                    {
                        Console.Write("║ Enter – Пройти на Старт                  M – Заглушить музыку ║");
                    }
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                PrintPieces(position, 0, player);
                                player.Receive(200);
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                Console.SetCursorPosition(165, 53);
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                    Console.Write("║ Enter – Пройти на Старт                  M – Заглушить музыку ║");
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                    Console.Write("║ Enter – Пройти на Старт                   M – Включить музыку ║");
                                }
                                break;
                        }
                    }
                    return null;
                case 9:
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    header = "Штраф за превышение скорости! Заплатите 15$";
                    payment = 15;
                    PrintMenu(header, payment, player);
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                if (player.Balance >= 15)
                                {
                                    player.Pay(15);
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                    notEntered = false;
                                }
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                            case ConsoleKey.T:
                                Trade(player);
                                PrintPlayers(turn);
                                PrintMenu(header, payment, player);
                                break;
                            case ConsoleKey.Tab:
                                if (player.Property.Count > 0)
                                {
                                    PrintRealty(player);
                                    PrintMenu(header, payment, player);
                                }
                                break;
                            case ConsoleKey.End:
                                Bankrupt(player, null);
                                notEntered = false;
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
                    Console.SetCursorPosition(165, 55);
                    if (musicMuted)
                    {
                        Console.Write("║                      M – Включить музыку                      ║");
                    }
                    else
                    {
                        Console.Write("║                      M – Заглушить музыку                     ║");
                    }
                    Console.SetCursorPosition(165, 56);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                PrintPieces(player.Position, 15, player);
                                if (position > 15)
                                {
                                    player.Receive(200);
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                }
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                Console.SetCursorPosition(165, 55);
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                    Console.Write("║                      M – Заглушить музыку                     ║");
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                    Console.Write("║                      M – Включить музыку                      ║");
                                }
                                break;
                        }
                    }
                    return quartals[15];
                case 11:
                    happyPlayer.Position = TimeSpan.Zero;
                    happyPlayer.Play();
                    notEntered = true;
                    header = "Вы выиграли чемпионат по шахматам! Получите 100$";
                    payment = -100;
                    PrintMenu(header, payment, player);
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                player.Receive(100);
                                moneyPlayer.Position = TimeSpan.Zero;
                                moneyPlayer.Play();
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
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
                    Console.SetCursorPosition(165, 53);
                    if (musicMuted)
                    {
                        Console.Write("║ Enter – Отправиться в Гостиничный комплекс M – Включить музыку║");
                    }
                    else
                    {
                        Console.Write("║Enter – Отправиться в Гостиничный косплекс M – Заглушить музыку║");
                    }
                    Console.SetCursorPosition(165, 54);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                PrintPieces(player.Position, 39, player);
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                Console.SetCursorPosition(165, 53);
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                    Console.Write("║Enter – Отправиться в Гостиничный косплекс M – Заглушить музыку║");
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                    Console.Write("║ Enter – Отправиться в Гостиничный комплекс M – Включить музыку║");
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
                    Console.SetCursorPosition(165, 54);
                    if (musicMuted)
                    {
                        Console.Write("║ Enter – Отправиться в Ресторан            M – Включить музыку ║");
                    }
                    else
                    {
                        Console.Write("║ Enter – Отправиться в Ресторан           M – Заглушить музыку ║");
                    }
                    Console.SetCursorPosition(165, 55);
                    Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                    notEntered = true;
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                PrintPieces(player.Position, 24, player);
                                if (position > 24)
                                {
                                    player.Receive(200);
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                }
                                notEntered = false;
                                break;
                            case ConsoleKey.M:
                                Console.SetCursorPosition(165, 54);
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                    Console.Write("║ Enter – Отправиться в Ресторан           M – Заглушить музыку ║");
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                    Console.Write("║ Enter – Отправиться в Ресторан            M – Включить музыку ║");
                                }
                                break;
                        }
                    }
                    return quartals[24];
                case 14:
                    notEntered = true;
                    header = "Заплатите 25$ за каждый дом и 100$ за каждый отель";
                    payment = 25 * player.HousesCount + 100 * player.HotelsCount;
                    if (payment > 0)
                    {
                        sadPlayer.Position = TimeSpan.Zero;
                        sadPlayer.Play();
                        PrintMenu(header, payment, player);
                        while (notEntered)
                        {
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.Enter:
                                    if (player.Balance >= payment)
                                    {
                                        player.Pay(payment);
                                        moneyPlayer.Position = TimeSpan.Zero;
                                        moneyPlayer.Play();
                                        notEntered = false;
                                    }
                                    break;
                                case ConsoleKey.M:
                                    if (musicMuted)
                                    {
                                        musicPlayer.Volume = 0.5;
                                        musicMuted = false;
                                    }
                                    else
                                    {
                                        musicPlayer.Volume = 0;
                                        musicMuted = true;
                                    }
                                    PrintMenu(header, payment, player);
                                    break;
                                case ConsoleKey.T:
                                    Trade(player);
                                    PrintPlayers(turn);
                                    PrintMenu(header, payment, player);
                                    break;
                                case ConsoleKey.Tab:
                                    if (player.Property.Count > 0)
                                    {
                                        PrintRealty(player);
                                        PrintMenu(header, payment, player);
                                    }
                                    break;
                                case ConsoleKey.End:
                                    Bankrupt(player, null);
                                    notEntered = false;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        ClearMenu();
                        Console.SetCursorPosition(165, 50);
                        Console.Write("╔═══════════════════════════════════════════════════════════════╗");
                        Console.SetCursorPosition(165, 51);
                        Console.Write("║       Заплатите 25$ за каждый дом и 100$ за каждый отель      ║");
                        Console.SetCursorPosition(165, 52);
                        Console.Write("║                    У вас нет отелей и домов                   ║");
                        Console.SetCursorPosition(165, 53);
                        Console.Write("╚═══════════════════════════════════════════════════════════════╝");
                        Thread.Sleep(1500);
                    }
                    return null;
                default:
                    sadPlayer.Position = TimeSpan.Zero;
                    sadPlayer.Play();
                    notEntered = true;
                    header = "Оплата курсов водителей! Заплатите 150$";
                    payment = 150;
                    PrintMenu(header, payment, player);
                    while (notEntered)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                if (player.Balance >= 150)
                                {
                                    player.Pay(150);
                                    moneyPlayer.Position = TimeSpan.Zero;
                                    moneyPlayer.Play();
                                    notEntered = false;
                                }
                                break;
                            case ConsoleKey.M:
                                if (musicMuted)
                                {
                                    musicPlayer.Volume = 0.5;
                                    musicMuted = false;
                                }
                                else
                                {
                                    musicPlayer.Volume = 0;
                                    musicMuted = true;
                                }
                                PrintMenu(header, payment, player);
                                break;
                            case ConsoleKey.T:
                                Trade(player);
                                PrintPlayers(turn);
                                PrintMenu(header, payment, player);
                                break;
                            case ConsoleKey.Tab:
                                if (player.Property.Count > 0)
                                {
                                    PrintRealty(player);
                                    PrintMenu(header, payment, player);
                                }
                                break;
                            case ConsoleKey.End:
                                Bankrupt(player, null);
                                notEntered = false;
                                break;
                        }
                    }
                    return null;
            }
        }

        static void Registration()
        {
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
            quartals[0].InitStart(playersCount);

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
            char[] pieces = new char[] { '♦', '♣', '☻', '↕', '♀', '♫', '☺', '♥', '¶' };
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
        }

        static void QueueDefinition()
        {
            int dice1, dice2;
            int[] throws = new int[playersCount];
            for (int i = 0; i < playersCount; i++)
            {
                throws[i] = 0;
            }

            Console.WriteLine("Определяется очередь хода");
            for (int i = 0; i < playersCount; i++)
            {
                Console.WriteLine($"{players[i].Name}, бросайте кубики");
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
        }

        static string CreateDirectory()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\MonopolySounds\";
            while (true)
            {
                try
                {
                    Directory.CreateDirectory(path);
                    break;
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine($"У программы нет прав создать папку со звуками по пути: {path}");
                    Console.Write("Пожалуйста, укажите путь для папки: ");
                    path = Console.ReadLine() + @"\MonopolySounds\";
                }
                catch (PathTooLongException)
                {
                    Console.WriteLine($"Слишком длинный путь: {path}");
                    Console.Write("Пожалуйста, укажите путь для папки: ");
                    path = Console.ReadLine() + @"\MonopolySounds\";
                }
                catch (IOException)
                {
                    Console.WriteLine($"Папка по пути {path} доступна только для чтения");
                    Console.Write("Пожалуйста, укажите путь для папки: ");
                    path = Console.ReadLine() + @"\MonopolySounds\";
                }
                catch
                {
                    Console.WriteLine($"Недопустимый путь: {path}");
                    Console.Write("Пожалуйста, укажите путь для папки: ");
                    path = Console.ReadLine() + @"\MonopolySounds\";
                }
            }
            return path;
        }

        static void SoundInit(MediaPlayer media, byte[] resource, string path)
        {
            File.WriteAllBytes(path, resource);
            media.Open(new Uri(path));
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FontInfo
        {
            internal int cbSize;
            internal int FontIndex;
            internal short FontWidth;
            public short FontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FontName;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

        static void SetCurrentFont(short fontSize = 0)
        {
            FontInfo set = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>(),
                FontIndex = 0,
                FontFamily = 54,
                FontName = "Consolas",
                FontWeight = 400,
                FontSize = fontSize
            };
            SetCurrentConsoleFontEx(GetStdHandle(-11), false, ref set);
        }

        static void FontSetup()
        {
            for (short i = 72; i > 4; i--)
            {
                SetCurrentFont(i);
                if (Console.LargestWindowWidth >= 231 && Console.LargestWindowHeight >= 58) break;
            }
        }

        static bool Victory()
        {
            musicPlayer.Stop();
            Player winner;
            victoryPlayer.Play();
            foreach (Player p in players)
            {
                if (!p.IsBankrupt)
                {
                    winner = p;
                    break;
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            int row = 3;
            int column = Convert.ToInt32(Math.Ceiling((Console.LargestWindowWidth - 25) / 2.0));
            Console.SetCursorPosition(column, row++);
            Console.Write("╔═══════════════════════╗");
            Console.SetCursorPosition(column, row++);
            Console.Write("║       Итоги игры      ║");
            for (int i = 0; i < playersCount; i++)
            {
                Console.SetCursorPosition(column, row);
                Console.Write($"║ {i + 1}.                    ║");
                Console.SetCursorPosition(column + 5, row++);
                foreach (Player p in players)
                {
                    if (p.Place == i)
                    {
                        Console.Write($"{players[i].Name} {players[i].Piece}");
                        break;
                    }
                }
            }
            Console.SetCursorPosition(column, row++);
            Console.Write("╚═══════════════════════╝");
            row++;
            Console.SetCursorPosition(column + 4, row++);
            Console.Write("Начать новую игру?");
            Console.SetCursorPosition(column + 8, row++);
            Console.Write("1. Начать");
            Console.SetCursorPosition(column + 8, row++);
            Console.Write("2. Закрыть");
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:
                        victoryPlayer.Stop();
                        menuPlayer.Play();
                        return true;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2:
                        victoryPlayer.Stop();
                        menuPlayer.Play();
                        return false;
                }
            }
        }

        static void Reset()
        {
            foreach (Player p in players) p.Reset();
            foreach (Quartal q in quartals) q.Reset();
            quartals[0].InitStart(playersCount);
            remaining = playersCount;
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main()
        {
            // Настройка окна
            Console.Title = "Монополия";
            FontSetup();
            ShowWindow(GetConsoleWindow(), 3);
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.CursorVisible = false;

            // Группировка улиц
            quartals[1].AddToGroup(quartals[3]);
            quartals[3].AddToGroup(quartals[1]);

            quartals[6].AddToGroup(quartals[8]);
            quartals[6].AddToGroup(quartals[9]);
            quartals[8].AddToGroup(quartals[6]);
            quartals[8].AddToGroup(quartals[9]);
            quartals[9].AddToGroup(quartals[6]);
            quartals[9].AddToGroup(quartals[8]);

            quartals[11].AddToGroup(quartals[13]);
            quartals[11].AddToGroup(quartals[14]);
            quartals[13].AddToGroup(quartals[11]);
            quartals[13].AddToGroup(quartals[14]);
            quartals[14].AddToGroup(quartals[11]);
            quartals[14].AddToGroup(quartals[13]);

            quartals[16].AddToGroup(quartals[18]);
            quartals[16].AddToGroup(quartals[19]);
            quartals[18].AddToGroup(quartals[16]);
            quartals[18].AddToGroup(quartals[19]);
            quartals[19].AddToGroup(quartals[16]);
            quartals[19].AddToGroup(quartals[18]);

            quartals[21].AddToGroup(quartals[23]);
            quartals[21].AddToGroup(quartals[24]);
            quartals[23].AddToGroup(quartals[21]);
            quartals[23].AddToGroup(quartals[24]);
            quartals[24].AddToGroup(quartals[21]);
            quartals[24].AddToGroup(quartals[23]);

            quartals[26].AddToGroup(quartals[27]);
            quartals[26].AddToGroup(quartals[29]);
            quartals[27].AddToGroup(quartals[26]);
            quartals[27].AddToGroup(quartals[29]);
            quartals[29].AddToGroup(quartals[26]);
            quartals[29].AddToGroup(quartals[27]);

            quartals[31].AddToGroup(quartals[32]);
            quartals[31].AddToGroup(quartals[34]);
            quartals[32].AddToGroup(quartals[31]);
            quartals[32].AddToGroup(quartals[34]);
            quartals[34].AddToGroup(quartals[31]);
            quartals[34].AddToGroup(quartals[32]);

            quartals[37].AddToGroup(quartals[39]);
            quartals[39].AddToGroup(quartals[37]);

            // Звуки
            string path = CreateDirectory();

            SoundInit(moneyPlayer, Resources.money, path + "money.mp3");
            SoundInit(policePlayer, Resources.police, path + "police.mp3");
            SoundInit(paperPlayer, Resources.paper, path + "paper.mp3");
            SoundInit(happyPlayer, Resources.happy, path + "happy.mp3");
            SoundInit(sadPlayer, Resources.sad, path + "sad.mp3");
            SoundInit(upgradePlayer, Resources.upgrade, path + "upgrade.mp3");
            SoundInit(downgradePlayer, Resources.downgrade, path + "downgrade.mp3");
            SoundInit(padlockPlayer, Resources.padlock, path + "padlock.mp3");
            SoundInit(chainPlayer, Resources.chain, path + "chain.mp3");
            SoundInit(cancelPlayer, Resources.cancel, path + "cancel.mp3");
            SoundInit(turnPlayer, Resources.turn, path + "turn.mp3");
            SoundInit(successPlayer, Resources.success, path + "success.mp3");
            SoundInit(musicPlayer, Resources.music, path + "music.mp3");
            SoundInit(menuPlayer, Resources.menu, path + "menu.mp3");
            SoundInit(victoryPlayer, Resources.victory, path + "victory.mp3");

            menuPlayer.Volume = 1;
            musicPlayer.Volume = 1;
            victoryPlayer.Volume = 1;

            // Перемешивание колод Казны и Шанса
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

            // Ввод игроков
            menuPlayer.Play();
            Registration();

            // Игра
            while (true)
            {
                QueueDefinition();
                menuPlayer.Stop();
                musicPlayer.Play();
                PrintRoadmap();
                PrintTitle();
                while (remaining > 1)
                {
                    for (int i = 0; i < playersCount && remaining > 1; i++)
                    {
                        if (musicPlayer.Position == musicPlayer.NaturalDuration.TimeSpan)
                        {
                            musicPlayer.Position = TimeSpan.Zero;
                            musicPlayer.Play();
                        }
                        PrintPlayers(i);
                        repeat = true;
                        Menu(players[i], i, false);
                        turnPlayer.Position = TimeSpan.Zero;
                        turnPlayer.Play();
                    }
                }
                if (Victory()) Reset();
                else break;
            }
            Environment.Exit(0);
        }
    }
}