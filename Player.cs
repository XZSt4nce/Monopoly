using System;
using System.Collections.Generic;

namespace Monopoly
{
    public class Player
    {
        private readonly string name;
        private readonly char piece;
        private int balance;
        private int position;
        private readonly List<Quartal> property;
        private int housesCount;
        private int hotelsCount;
        private int yellow;
        private int black;
        private int darkYellow;
        private int darkGreen;
        private int green;
        private int red;
        private int magenta;
        private int gray;
        private int blue;
        private int cyan;
        private int place;
        private int liberation;
        private int escapeAttempts;
        private bool cancelled;
        private bool didBid;
        private bool isPrisoned;
        private bool isBankrupt;

        public string Name { get { return name; } }
        public char Piece { get { return piece; } }
        public int Balance { get { return balance; } }
        public int Position { get { return position; } }
        public List<Quartal> Property { get { return property; } }
        public int HousesCount { get { return housesCount; } }
        public int HotelsCount { get { return hotelsCount; } }
        public int Yellow { get { return yellow; } }
        public int Black { get { return black; } }
        public int DarkYellow { get { return darkYellow; } }
        public int DarkGreen { get { return darkGreen; } }
        public int Green { get { return green; } }
        public int Red { get { return red; } }
        public int Magenta { get {  return magenta; } }
        public int Gray { get { return gray; } }
        public int Blue { get { return blue; } }
        public int Cyan { get { return cyan; } }
        public int Place { get { return place; } }
        public int Liberation { get { return liberation; } }
        public int EscapeAttempts { get { return escapeAttempts; } }
        public bool Cancelled { get { return cancelled; } }
        public bool DidBid { get { return didBid; } }
        public bool IsPrisoned { get { return isPrisoned; } }
        public bool IsBankrupt { get { return isBankrupt; } }

        public Player(string _name, char _piece)
        {
            name = _name;
            piece = _piece;
            balance = 1500;
            position = 0;
            property = new List<Quartal>();
            housesCount = 0;
            hotelsCount = 0;
            yellow = 0;
            black = 0;
            darkYellow = 0;
            darkGreen = 0;
            green = 0;
            red = 0;
            magenta = 0;
            gray = 0;
            blue = 0;
            cyan = 0;
            place = 0;
            liberation = 0;
            escapeAttempts = 0;
            cancelled = false;
            isPrisoned = false;
            isBankrupt = false;
        }

        public void Reset()
        {
            balance = 1500;
            position = 0;
            property.Clear();
            housesCount = 0;
            housesCount = 0;
            yellow = 0;
            black = 0;
            darkYellow = 0;
            darkGreen = 0;
            green = 0;
            red = 0;
            magenta = 0;
            gray = 0;
            blue = 0;
            cyan = 0;
            liberation = 0;
            escapeAttempts = 0;
            isPrisoned = false;
            isBankrupt = false;
        }

        public void IncreasePlace()
        {
            place++;
        }

        public void IncreaseColor(Quartal quartal)
        {
            switch (quartal.Color)
            {
                case ConsoleColor.Black:
                    black++;
                    break;
                case ConsoleColor.Gray:
                    gray++;
                    break;
                case ConsoleColor.Yellow:
                    yellow++;
                    break;
                case ConsoleColor.DarkYellow:
                    darkYellow++;
                    break;
                case ConsoleColor.DarkGreen:
                    darkGreen++;
                    break;
                case ConsoleColor.Green:
                    green++;
                    break;
                case ConsoleColor.Red:
                    red++;
                    break;
                case ConsoleColor.Magenta:
                    magenta++;
                    break;
                case ConsoleColor.Blue:
                    blue++;
                    break;
                case ConsoleColor.Cyan:
                    cyan++;
                    break;
            }
        }

        public void DecreaseColor(Quartal quartal)
        {
            switch (quartal.Color)
            {
                case ConsoleColor.Black:
                    black--;
                    break;
                case ConsoleColor.Gray:
                    gray--;
                    break;
                case ConsoleColor.Yellow:
                    yellow--;
                    break;
                case ConsoleColor.DarkYellow:
                    darkYellow--;
                    break;
                case ConsoleColor.DarkGreen:
                    darkGreen--;
                    break;
                case ConsoleColor.Green:
                    green--;
                    break;
                case ConsoleColor.Red:
                    red--;
                    break;
                case ConsoleColor.Magenta:
                    magenta--;
                    break;
                case ConsoleColor.Blue:
                    blue--;
                    break;
                case ConsoleColor.Cyan:
                    cyan--;
                    break;
            }
        }

        public List<Quartal> Bankrupt(Player player)
        {
            isBankrupt = true;
            List<Quartal> prprt = property;
            foreach (var quartal in prprt)
            {
                for (int i = quartal.Level; i > 0; i--)
                {
                    quartal.Downgrade();
                }
            }

            if (player == null)
            {
                foreach (var quartal in prprt)
                {
                    if (quartal.IsMantaged)
                    {
                        player.Receive(quartal.Pledge + quartal.Pledge / 10);
                        quartal.Redeem();
                    }
                    quartal.SetOwner(null);
                }
                prprt = null;
            }
            else
            {
                player.Receive(balance);
                foreach (var quartal in prprt)
                {
                    quartal.SetOwner(player);
                }
            }
            balance = 0;
            position = -1;
            return prprt;
        }

        public void GetLiberation()
        {
            liberation++;
        }

        public void SpendLiberation()
        {
            liberation--;
            LeavePrison();
        }

        public void Arrest()
        {
            isPrisoned = true;
        }

        public void LeavePrison()
        {
            isPrisoned = false;
            escapeAttempts = 0;
        }

        public void DoEscapeAttempt()
        {
            escapeAttempts++;
        }

        public void AddProperty(Quartal quartal)
        {
            property.Add(quartal);
        }
        
        public void RemoveProperty(Quartal quartal)
        {
            property.Remove(quartal);
        }

        public void Pay (int amount)
        {
            balance -= amount;
        }

        public void Receive (int amount)
        {
            balance += amount;
        }

        public void DoBid()
        {
            didBid = true;
        }

        public void CancelAuction()
        {
            cancelled = true;
        }

        public void EndAuction()
        {
            cancelled = false;
            didBid = false;
        }

        public void AddHouses (int amount)
        {
            housesCount += amount;
        }

        public void ReduceHouses(int amount)
        {
            housesCount -= amount;
        }

        public void IncreaseHotels()
        {
            hotelsCount++;
        }

        public void DecreaseHotels()
        {
            hotelsCount--;
        }

        public void Move(int newPosition)
        {
            position = newPosition;
        }
    }
}
