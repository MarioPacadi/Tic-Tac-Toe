using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Player
    {

        public Player(char symbol) => Symbol=symbol;

        public char Symbol { get; set; }

        public override string ToString() => $"{Symbol}";

        public override bool Equals(object obj) => (obj is Player other) ? char.ToUpper(Symbol) == char.ToUpper(other.Symbol) && Symbol!='-' && other.Symbol!='-' : false;

        public override int GetHashCode() => Symbol.GetHashCode() * 31;

        public static Player Parse(char s, List<Player> players)
        {
            //check if the same symbol exists in list of players
            foreach (Player p in players)
            {
                if (p.Equals(new Player(s))) return null;
            }

            //check if symbol is letter
            return (char.IsLetter(s)) ? new Player(s) : null;
        }

    }
}
