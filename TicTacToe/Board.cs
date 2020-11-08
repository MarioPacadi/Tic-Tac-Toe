using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Board
    {
        //###### CONSTRUCTORS ######

        public Board()
        {
            Number_of_players = 2; //default
            N=Number_of_players+1; //if 2 players n=3, if 3 players n=4 etc...
        }

        //###### VARIABLES ######
        public static int PosX { get; set; }
        public static int PosY { get; set; }
        public static int Number_of_players { get; set; }
        private static int N { get; set; } //length
        public List<Player> Contestants { get; set; }

        private static List<Player> Field = new List<Player>(N*N); //radi list s capacity

        //###### METHODS ######

        private static Player ReadPlayer(int i, List<Player> players)
        {
            Player p;
            do
            {
                Console.Write($"Type in a letter to be {i}. players choosen symbol: ");
                p = Player.Parse(Console.ReadKey().KeyChar, players); //use ReadKey.KeyChar

                if (p == null)
                {
                    Console.WriteLine("  You cant use that symbol!");
                }
                else Console.WriteLine();

            } while (p == null);

            return p;
        }
        public void Load_Players()
        {
            List<Player> players = new List<Player>();

            for (int i = 1; i <= Number_of_players; i++)
            {
                players.Add(ReadPlayer(i, players));
            }

            Contestants = players;
        }

        //##### Methods for checking #####

        public bool Select_Position(int current_player)
        {
            //Check in case of TIE
            if (AllFieldsFull())
            {
                Console.WriteLine("\n    TIE ");
                ResetField();
                return false;
            }

            //Controls to move
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.W: case ConsoleKey.UpArrow: { if (--PosX < 0) PosX = N - 1; break; }
                case ConsoleKey.S: case ConsoleKey.DownArrow: { if (++PosX > N - 1) PosX = 0; break; }
                case ConsoleKey.A: case ConsoleKey.LeftArrow: { if (--PosY < 0) PosY = N - 1; break; }
                case ConsoleKey.D: case ConsoleKey.RightArrow: { if (++PosY > N - 1) PosY = 0; break; }
                case ConsoleKey.Spacebar: case ConsoleKey.Enter: { if (TryToChangeSymbol(current_player)) return true; break; } //change symbol if possible
                //default: return false; must be out of switch because program doesnt allow it
            }
            return false;
        }
        private bool TryToChangeSymbol(int current_player)
        {
            if (CheckIfHasSymbol(current_player)) //if area already has symbol then dont change it
            {
                Console.WriteLine($"That area has already been taken");
                Console.WriteLine($"Please wait 5 seconds to select another area...");
                Thread.Sleep(5000);
                return false;
            }
            else
            {
                Field[PosY * N + PosX].Symbol = Contestants[current_player].Symbol;
                return true;
            }
        }
        private bool CheckIfHasSymbol(int current_player)
        {
            for (int i = 0; i < Contestants.Count(); i++)
            {
                if (Field[PosY * N + PosX].Symbol == Contestants[i].Symbol) return true;
            }
            return false;
        }
        private bool AllFieldsFull()
        {
            int count=0;
            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                {
                    if (Field[y * N + x].Symbol != '-') count++;
                }
            }

            return (count==N*N) ? true : false;
        }
        public virtual bool CheckForVictory()
        {
            //Horizontal check
            for (int x = 0; x < N; x++)
                if (Field[0 * N + x].Equals(Field[1 * N + x]) && Field[1 * N + x].Equals(Field[2 * N + x])) return true;

            //Vertical check
            for (int y = 0; y < N; y++)
                if (Field[y * N + 0].Equals(Field[y * N + 1]) && Field[y * N + 1].Equals(Field[y * N + 2])) return true;

            //Diagonal check
            if (Field[0].Equals(Field[4]) && Field[4].Equals(Field[8])) return true;
            else if (Field[2].Equals(Field[4]) && Field[4].Equals(Field[6])) return true;

            return false;
        }

        //##### Method for reset #####
        private static void ResetField()
        {
            Console.WriteLine("Wait 5 sec for field to reset...");
            Thread.Sleep(5000);
            Field.Clear();
            EmptyBoard();
        }

        //##### Methods for write #####
        public virtual void List_of_players()
        {
            Console.Write(" ");
            for (int i = 0; i < Contestants.Count(); i++)
            {
                Console.Write($"Player{i + 1}: {Contestants[i]}");
                if (i < Contestants.Count() - 1) Console.Write(" and ");
            }
            Console.WriteLine();
        }
        private static void EmptyBoard()
        {
            for (int i = 1; i <= N*N; i++) Field.Add(new Player('-'));
        }
        private static string Line(int pos)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" |");
            for (int i = 0; i < N; i++)
            {
                if (i==pos) //check which column
                {
                    sb.Append("_---_|");
                }
                else sb.Append("_____|");
            }
            sb.Append("\n");

            return sb.ToString();
        }
        private static string EmptyLine() 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" |");
            for (int i = 0; i<N; i++)
            {
                sb.Append("     |");
            }
            sb.Append("\n");

            return sb.ToString();
        }
        private static string Roof()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("  ");
            for (int i = 0; i < N; i++)
            {
                sb.Append("______");
            }
            sb.Length--; //removes last char to look better
            sb.Append("\n");

            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb=new StringBuilder(); //similar to StringStream

            EmptyBoard();
            sb.Append("\n");
            sb.Append(Roof());
            for (int x = 0; x < N; x++)
            {
                sb.Append(EmptyLine());
                for (int y = 0; y <N; y++)
                {
                    if (y == 0) sb.Append(" |");

                    sb.Append($"  {Field[y * N + x].Symbol}  |");
                }
                sb.Append("\n");
                if(PosX==x) sb.Append(Line(PosY)); //checks in which row it is then sends position to which column it is
                else sb.Append(Line(-1)); //not selected
            }

            return sb.ToString();
        }

    }
}
