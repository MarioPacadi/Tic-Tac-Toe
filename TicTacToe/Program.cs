using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            //Mario Pacadi
            Board board = new Board();

            Title();
            board.Load_Players();

            int current_player = 0;
            do
            {
                Console.Clear();

                Title();

                Board_and_Players(board,current_player);

                Controls();

                if (board.Select_Position(current_player))  //current player must switch when player selected their position
                    if (++current_player == board.Contestants.Count()) current_player = 0; //after changing to next player checks of out of range

            } while (!board.CheckForVictory());

            Victory(board, current_player);

        }

        private static void Title()
        {
            Console.WriteLine(" --------------------");
            Console.WriteLine(" |   Tic-Tac-Toe    |");
            Console.WriteLine(" --------------------\n");
        }

        private static void Board_and_Players(Board board,int current_player)
        {
            board.List_of_players();

            Console.WriteLine($"\n Current_player{current_player + 1}: {board.Contestants[current_player]}");

            Console.WriteLine(board);
        }

        private static void Controls()
        {
            Console.WriteLine("  Controls   or  Arrow keys     ");
            Console.WriteLine("    [W]             [UP]        ");
            Console.WriteLine(" [A][S][D]   [LEFT][DOWN][RIGHT]\n");
            Console.WriteLine("         Confirm");
            Console.WriteLine("   [ENTER]  or  [SPACE]");

        }

        private static void Victory(Board board,int current_player)
        {
            if (++current_player == board.Contestants.Count()) current_player=0;
            Console.Clear();
            Console.WriteLine(" --------------------");
            Console.WriteLine(" |     Victory      |");
            Console.WriteLine(" --------------------\n");
            Console.WriteLine(board);
            Console.WriteLine($"\nPLAYER{current_player + 1}: {board.Contestants[current_player]} is victorious");
        }

    }
}
