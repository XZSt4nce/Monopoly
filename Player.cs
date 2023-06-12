﻿namespace Monopoly
{
    public class Player
    {
        public string name;
        public char piece;
        public int balance;
        public int position;
        public Quartal[] property;
        /// <summary>
        /// Количество построенных домов на улицах игрока
        /// </summary>
        public int housesCount;
        /// <summary>
        /// Количество построенных отелей на улицах игрока
        /// </summary>
        public int hotelsCount;
        /// <summary>
        /// Количество жёлтых улиц игрока
        /// </summary>
        public int yellow;
        /// <summary>
        /// Количество портов игрока
        /// </summary>
        public int black;
        /// <summary>
        /// Количество тёмно-жёлтых улиц игрока
        /// </summary>
        public int darkYellow;
        /// <summary>
        /// Количество тёмно-зелёных улиц игрока
        /// </summary>
        public int darkGreen;
        /// <summary>
        /// Количество зелёных улиц игрока
        /// </summary>
        public int green;
        /// <summary>
        /// Количество красных улиц игрока
        /// </summary>
        public int red;
        /// <summary>
        /// Количество пурпурных улиц игрока
        /// </summary>
        public int magenta;
        /// <summary>
        /// Количество коммунальных предприятий игрока
        /// </summary>
        public int gray;
        /// <summary>
        /// Количество синих улиц игрока
        /// </summary>
        public int blue;
        /// <summary>
        /// Количество голубых улиц игрока
        /// </summary>
        public int cyan;
        /// <summary>
        /// Количество карточек освобождения игрока
        /// </summary>
        public int liberation;
        /// <summary>
        /// Количество попыток выйти из тюрьмы. При выходе из тюрьмы обнуляется
        /// </summary>
        public int escapeAttempts;
        /// <summary>
        /// Аннулировал ли ставку игрок на аукционе. По умолчанию – false
        /// </summary>
        public bool canselled;
        public bool prisoned;
        public bool bankrupt;
        public Player(string _name, char _piece)
        {
            name = _name;
            piece = _piece;
            balance = 1500;
            position = 0;
            property = new Quartal[40];
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
            liberation = 0;
            escapeAttempts = 0;
            canselled = false;
            prisoned = false;
            bankrupt = false;
        }
    }
}