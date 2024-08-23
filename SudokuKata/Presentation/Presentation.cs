using System;
using System.Text;

namespace SudokuKata
{
    public abstract class Presentation
    {
        public static void PrintToConsole(int[] board, string label)
        {
            Console.WriteLine($"{label}\n{ToPrintableString(board)}");
        }


        private static string ToPrintableString(int[] board)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < board.Length; i++)
            {
                sb.Append(board[i]);
                if ((i + 1) % 9 == 0) sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}