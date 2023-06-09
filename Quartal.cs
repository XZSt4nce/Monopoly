using System;

namespace Monopoly
{
    public class Quartal
    {
        public readonly string name;
        public readonly ConsoleColor color;
        public int owner;
        public readonly int special;
        /* 
         * 0 – start
         * 1 – treasury
         * 2 – chance
         * 3 – jail
         * 4 – parking
         * 5 – arrest
         * 6 – port
         * 7 – municipal
         * 8 – tax
         * 9 – purchase
         */
        public readonly int cost;
        public readonly int noMonopolyRent;
        public readonly int monopolyRent;
        public readonly int house1Rent;
        public readonly int house2Rent;
        public readonly int house3Rent;
        public readonly int house4Rent;
        public readonly int hotelRent;
        public readonly int houseCost;
        public readonly int pledge;
        public Quartal (string _name, ConsoleColor _color, int _cost, int _noMonopolyRent, int _monopolyRent, int _house1Rent, int _house2Rent,
                       int _house3Rent, int _house4Rent, int _hotelRent, int _houseCost, int _pledge)
        {
            name = _name;
            special = -1;
            color = _color;
            owner = -1;
            cost = _cost;
            noMonopolyRent = _noMonopolyRent;
            monopolyRent = _monopolyRent;
            house1Rent = _house1Rent;
            house2Rent = _house2Rent;
            house3Rent = _house3Rent;
            house4Rent = _house4Rent;
            hotelRent = _hotelRent;
            houseCost = _houseCost;
            pledge = _pledge;
        }
        public Quartal(string _name, int _cost, int _house1Rent, int _house2Rent,
                       int _house3Rent, int _house4Rent, int _pledge)
        {
            name = _name;
            special = 6;
            color = ConsoleColor.Black;
            owner = -1;
            cost = _cost;
            house1Rent = _house1Rent;
            house2Rent = _house2Rent;
            house3Rent = _house3Rent;
            house4Rent = _house4Rent;
            pledge = _pledge;
        }
        public Quartal (int _special)
        {
            special = _special;
        }
        public Quartal(string _name, int _cost)
        {
            name = _name;
            cost = _cost;
            special = 7;
        }
    }
}
