using System;

namespace Monopoly
{
    public class Quartal
    {
        public readonly string name;
        public readonly string color;
        public Player owner;
        public int visitors;
        /// <summary>
        /// Специальное обозначение улицы.
        /// -1 – обычная улица
        /// 0 – старт;
        /// 1 – казна;
        /// 2 – шанс;
        /// 3 – тюрьма;
        /// 4 – парковка;
        /// 5 – арест;
        /// 6 – порт;
        /// 7 – коммунальное предприятие;
        /// 8 – налог;
        /// 9 – дорогая покупка.
        /// </summary>
        public readonly int special;
        public readonly int cost;
        public readonly int noMonopolyRent;
        public readonly int monopolyRent;
        public readonly int house1Rent;
        public readonly int house2Rent;
        public readonly int house3Rent;
        public readonly int house4Rent;
        public readonly int hotelRent;
        public readonly int houseCost;
        /// <summary>
        /// Уровень прокачки.
        /// 0 – без домов;
        /// 1 – 1 дом;
        /// 2 – 2 дома;
        /// 3 – 3 дома;
        /// 4 – 4 дома;
        /// 5  отель.
        /// </summary>
        public int level;
        /// <summary>
        /// Сумма денег, полученных при заложении улицы
        /// </summary>
        public readonly int pledge;
        /// <summary>
        /// Является ли улица заложенной
        /// </summary>
        public bool isMantaged;
        public Quartal (string _name, string _color, int _cost, int _noMonopolyRent, int _monopolyRent, int _house1Rent, int _house2Rent,
                       int _house3Rent, int _house4Rent, int _hotelRent, int _houseCost, int _pledge)
        {
            name = _name;
            special = -1;
            color = _color;
            owner = null;
            visitors = 0;
            level = 0;
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
            isMantaged = false;
        }
        public Quartal(string _name, int _cost, int _house1Rent, int _house2Rent,
                       int _house3Rent, int _house4Rent, int _pledge)
        {
            name = _name;
            special = 6;
            color = "Black";
            owner = null;
            cost = _cost;
            house1Rent = _house1Rent;
            house2Rent = _house2Rent;
            house3Rent = _house3Rent;
            house4Rent = _house4Rent;
            pledge = _pledge;
            visitors = 0;
            isMantaged = false;
        }
        public Quartal (int _special)
        {
            special = _special;
            visitors = 0;
        }
        public Quartal(string _name, int _cost, int _pledge)
        {
            name = _name;
            cost = _cost;
            pledge = _pledge;
            isMantaged = false;
            special = 7;
            visitors = 0;
        }
    }
}
