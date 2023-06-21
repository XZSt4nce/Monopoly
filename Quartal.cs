using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Monopoly
{
    public class Quartal
    {
        private readonly string name;
        private readonly ConsoleColor color;
        private Player owner;
        private int visitors;
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
        private readonly int special;
        private readonly int cost;
        private readonly int noMonopolyRent;
        private readonly int monopolyRent;
        private readonly int house1Rent;
        private readonly int house2Rent;
        private readonly int house3Rent;
        private readonly int house4Rent;
        private readonly int hotelRent;
        private readonly int houseCost;
        /// <summary>
        /// 0 – без домов;
        /// 1 – 1 дом;
        /// 2 – 2 дома;
        /// 3 – 3 дома;
        /// 4 – 4 дома;
        /// 5  отель.
        /// </summary>
        private int level;
        private readonly int pledge;
        private readonly List<Quartal> colorGroup;
        private bool isMantaged;
        private bool? isMonopoly;

        public string Name { get { return name; } }
        public ConsoleColor Color { get { return color; } }
        public Player Owner { get { return owner; } }
        public int Visitors { get { return visitors; } }
        public int Special { get { return special; } }
        public int Cost { get { return cost; } }
        public int NoMonopolyRent { get { return noMonopolyRent; } }
        public int MonopolyRent { get { return monopolyRent; } }
        public int House1Rent { get { return house1Rent; } }
        public int House2Rent { get { return house2Rent; } }
        public int House3Rent { get { return house3Rent; } }
        public int House4Rent { get { return house4Rent; } }
        public int HotelRent { get { return hotelRent; } }
        public int HouseCost { get { return houseCost; } }
        public int Level { get { return level; } }
        public int Pledge { get { return pledge; } }
        public List<Quartal> ColorGroup { get { return colorGroup; } }
        public bool IsMantaged { get { return isMantaged; } }
        public bool? IsMonopoly { get { return isMonopoly; } }

        public Quartal (string _name, ConsoleColor _color, int _cost, int _noMonopolyRent, int _monopolyRent, int _house1Rent, int _house2Rent,
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
            colorGroup = new List<Quartal>();
            isMantaged = false;
            isMonopoly = false;
        }
        public Quartal(string _name, bool isBlack)
        {
            if (isBlack)
            {
                special = 6;
                color = ConsoleColor.Black;
                cost = 200;
                pledge = 100;
            }
            else
            {
                special = 7;

                color = ConsoleColor.Gray;
                cost = 150;
                pledge = 75;
            }
            name = _name;
            owner = null;
            visitors = 0;
            isMantaged = false;
            isMonopoly = null;
        }

        public Quartal (int _special)
        {
            special = _special;
            visitors = 0;
        }

        public void Reset()
        {
            visitors = 0;
            if (Special == 6 || Special == 7)
            {
                isMantaged = false;
                owner = null;
            }
            if (Special == -1)
            {
                owner = null;
                isMantaged = false;
                isMonopoly = null;
            }
        }

        public void InitStart(int playersCount)
        {
            if (special == 0) visitors = playersCount;
        }

        public void AddToGroup(Quartal quartal)
        {
            colorGroup.Add(quartal);
        }

        public void SetOwner(Player player)
        {
            if (player == null) owner = player;
            else
            {
                if (owner == null)
                {
                    player.Pay(cost);
                }
                else
                {
                    owner.DecreaseColor(this);
                    owner.RemoveProperty(this);
                }
                owner = player;
                owner.IncreaseColor(this);
                owner.AddProperty(this);
                if (colorGroup != null)
                {
                    isMonopoly = true;
                    foreach (Quartal quartal in colorGroup)
                    {
                        if (quartal.owner != owner)
                        {
                            isMonopoly = false;
                            break;
                        }
                    }
                    foreach (Quartal quartal in colorGroup)
                    {
                        quartal.isMonopoly = isMonopoly;
                    }
                }
            }
        }

        public void SetOwner(Player player, int bid)
        {
            if (owner == null)
            {
                player.Pay(bid);
            }
            else
            {
                owner.DecreaseColor(this);
                owner.RemoveProperty(this);
            }
            owner = player;
            owner.IncreaseColor(this);
            owner.AddProperty(this);
            if (colorGroup != null)
            {
                isMonopoly = true;
                foreach (Quartal quartal in colorGroup)
                {
                    if (quartal.owner != owner)
                    {
                        isMonopoly = false;
                        break;
                    }
                }
                foreach (Quartal quartal in colorGroup)
                {
                    quartal.isMonopoly = isMonopoly;
                }
            }
        }

        public void Mantage()
        {
            isMantaged = true;
            owner.Receive(Pledge);
        }

        public void Redeem()
        {
            isMantaged = false;
            owner.Pay(pledge + pledge / 10);
        }

        public void Downgrade()
        {
            if (level == 5)
            {
                owner.AddHouses(4);
                owner.DecreaseHotels();
            }
            else
            {
                owner.ReduceHouses(1);
            }
            level--;
            owner.Receive(houseCost / 2);
        }

        public void IncreaseVisitors()
        {
            visitors++;
        }

        public void DecreaseVisitors()
        {
            visitors--;
        }

        public void Upgrade()
        {
            level++;
            if (level == 5)
            {
                owner.ReduceHouses(4);
                owner.IncreaseHotels();
            }
            else
            {
                owner.AddHouses(1);
            }
            owner.Pay(houseCost);
        }
    }
}
