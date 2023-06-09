using System;
using System.Linq;

namespace Monopoly
{
    public class Monopoly
    {
        static public int[] balances = new int[8];
        static public int players;
        static public int remaining;
        static public int[] positions = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        static public int[] liberation = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        static public bool[] prisoned = new bool[] { false, false, false, false, false, false, false, false };
        static public bool[] bankrupt = new bool[] { false, false, false, false, false, false, false, false };
        static public int[] houses = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        static public int[] hotels = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        static public Quartal[] quartals = new Quartal[40]
        {
            new Quartal(0),
            new Quartal("Старая дорога", ConsoleColor.Yellow, 60, 2, 4, 10, 30, 90, 160, 250, 50, 30),
            new Quartal(1),
            new Quartal("Главная дорога", ConsoleColor.Yellow, 60, 4, 8, 20, 60, 180, 320, 450, 50, 30),
            /// Налог 10% или 200
            new Quartal(8),
            new Quartal("Западный морской порт", 200, 25, 50, 100, 200, 100),
            new Quartal("Аквапарк", ConsoleColor.DarkYellow, 100, 6, 12, 30, 90, 270, 400, 550, 50, 50),
            new Quartal(2),
            new Quartal("Городской парк", ConsoleColor.DarkYellow, 100, 6, 12, 30, 90, 270, 400, 550, 50, 50),
            new Quartal("Гоночный курорт", ConsoleColor.DarkYellow, 120, 8, 16, 40, 100, 300, 450, 600, 50, 60),
            new Quartal(3),
            new Quartal("Спальный район", ConsoleColor.DarkGreen, 140, 10, 20, 50, 150, 450, 625, 750, 100, 70),
            new Quartal("Электрическая компания", 150),
            new Quartal("Деловой квартал", ConsoleColor.DarkGreen, 140, 10, 20, 50, 150, 450, 625, 750, 100, 70),
            new Quartal("Торговая площадь", ConsoleColor.DarkGreen, 160, 12, 24, 60, 180, 500, 700, 900, 100, 80),
            new Quartal("Северный морской порт", 200, 25, 50, 100, 200, 100),
            new Quartal("Улица Пушкина", ConsoleColor.Green, 180, 14, 28, 70, 2000, 550, 750, 950, 100, 90),
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
            new Quartal("Водопроводная компания", 150),
            new Quartal("Сотовая связь", ConsoleColor.Magenta, 280, 24, 48, 120, 360, 850, 1025, 1200, 150, 140),
            new Quartal(5),
            new Quartal("Морские перевозки", ConsoleColor.Blue, 300, 26, 52, 130, 390, 900, 1100, 1275, 200, 150),
            new Quartal("Железная дорога", ConsoleColor.Blue, 300, 26, 52, 130, 390, 900, 1100, 1275, 200, 150),
            new Quartal(1),
            new Quartal("Авиакомпания", ConsoleColor.Blue, 320, 28, 56, 150, 450, 100, 1200, 1400, 200, 160),
            new Quartal("Южный морской порт", 200, 25, 50, 100, 200, 100),
            new Quartal(2),
            new Quartal("Курортная зона", ConsoleColor.Cyan, 350, 22, 44, 110, 330, 800, 975, 1150, 150, 130),
            new Quartal(9),
            new Quartal("Гостиничный комплекс", ConsoleColor.Cyan, 400, 22, 44, 110, 330, 800, 975, 1150, 150, 130),
        };
        static public readonly Random rnd = new Random();
        static public void Print()
        {

        }
        static void Treasury(int player)
        {
            int ticket = rnd.Next(16);
            switch (ticket)
            {
                // SetCursorPosition + WriteLine
                case 0:
                    balances[player] += 25;
                    break;
                case 1:
                    balances[player] -= 50;
                    break;
                case 2:
                    positions[player] = 1;
                    break;
                case 3:
                    balances[player] -= 50;
                    break;
                case 4:
                    liberation[player]++;
                    break;
                case 5:
                    positions[player] = 10;
                    prisoned[player] = true;
                    break;
                case 6:
                    balances[player] -= 100;
                    break;
                case 7:
                    // -10 || chance
                    break;
                case 8:
                    balances[player] += 25;
                    break;
                case 9:
                    balances[player] += 25;
                    break;
                case 10:
                    balances[player] += 50;
                    break;
                case 11:
                    for (int i = 0; i < players; i++)
                    {
                        balances[i] -= 10;
                        balances[player] += 10;
                    }
                    break;
                case 12:
                    balances[player] += 200;
                    break;
                case 13:
                    balances[player] += 100;
                    break;
                case 14:
                    balances[player] += 10;
                    break;
                case 15:
                    balances[player] += 100;
                    break;
            }
        }

        static void Chance(int player)
        {
            int ticket = rnd.Next(16);
            switch (ticket)
            {
                // SetCursorPosition + WriteLine
                case 0:
                    balances[player] -= 40 * houses[player] + 115 * hotels[player];
                    break;
                case 1:
                    positions[player] = 10;
                    prisoned[player] = true;
                    break;
                case 2:
                    balances[player] -= 20;
                    break;
                case 3:
                    if (positions[player] < 3) positions[player] += 37;
                    else positions[player] -= 3;
                    break;
                case 4:
                    liberation[player]++;
                    break;
                case 5:
                    balances[player] += 150;
                    break;
                case 6:
                    balances[player] += 50;
                    break;
                case 7:
                    if (positions[player] > 6) balances[player] += 200;
                    positions[player] = 6;
                    break;
                case 8:
                    balances[player] += 200;
                    positions[player] = 0;
                    break;
                case 9:
                    balances[player] -= 15;
                    break;
                case 10:
                    if (positions[player] > 15) balances[player] += 200;
                    positions[player] = 15;
                    break;
                case 11:
                    balances[player] += 100;
                    break;
                case 12:
                    positions[player] = 39;
                    break;
                case 13:
                    if (positions[player] > 24) balances[player] += 200;
                    positions[player] = 24;
                    break;
                case 14:
                    balances[player] -= 25 * houses[player] + 100 * hotels[player];
                    break;
                case 15:
                    balances[player] -= 150;
                    break;
            }
        }

        static void Main()
        {
            Console.Title = "Монополия";
            while (true)
            {
                Console.Write("Введите количество игроков: ");
                try
                {
                    players = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Неправильный ввод! Попробуйте ещё раз");
                    continue;
                }
                if (players < 3)
                {
                    Console.WriteLine("Слишком мало игроков! Попробуйте ещё раз");
                    continue;
                }
                if (players > 8)
                {
                    Console.WriteLine("Слишком много игроков! Попробуйте ещё раз");
                    continue;
                }
                break;
            }
            string[] names = new string[players];
            
            for (int i = 0; i < players; i++)
            {
                balances[i] = 1500;
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
            char[] figures = new char[players];
            for (int i = 0; i < players; i++)
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
            int dice1, dice2;
            remaining = players;
            int doubles = 0;
            /*while (remaining > 1)
            {
                for (int i = 0; i < players; i++)
                {
                    if (bankrupt[i]) continue;
                    dice1 = rnd.Next(1, 7);
                    dice2 = rnd.Next(1, 7);
                    positions[i] += dice1 + dice2;
                    if (dice1 == dice2)
                    {
                        if (doubles == 2)
                        {
                            positions[i] = 10;
                            prisoned[i] = true;
                            doubles = 0;
                        }
                        else
                        {
                            i--;
                            doubles++;
                        }
                    }
                    else doubles = 0;
                }
            }*/
        }
    }
}
