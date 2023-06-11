namespace Monopoly
{
    public class Player
    {
        public string name;
        public char piece;
        public int balance;
        public int position;
        public Quartal[] property;
        public int propertyCount;
        public int yellow;
        public int black;
        public int darkYellow;
        public int darkGreen;
        public int green;
        public int red;
        public int magenta;
        public int gray;
        public int blue;
        public int cyan;
        public int liberation;
        public int houses;
        public int hotels;
        public int escapeAttempts;
        public bool prisoned;
        public bool bankrupt;
        public Player(string _name, char _piece)
        {
            name = _name;
            piece = _piece;
            balance = 1500;
            position = 0;
            property = new Quartal[40];
            propertyCount = 0;
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
            prisoned = false;
            bankrupt = false;
        }
    }
}
