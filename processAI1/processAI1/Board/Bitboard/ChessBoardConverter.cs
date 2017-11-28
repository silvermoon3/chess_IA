using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Board.Bitboard
{
    static class ChessBoardConverter
    {

        enum enumSquare
        {
            a1, b1, c1, d1, e1, f1, g1, h1,
            a2, b2, c2, d2, e2, f2, g2, h2,
            a3, b3, c3, d3, e3, f3, g3, h3,
            a4, b4, c4, d4, e4, f4, g4, h4,
            a5, b5, c5, d5, e5, f5, g5, h5,
            a6, b6, c6, d6, e6, f6, g6, h6,
            a7, b7, c7, d7, e7, f7, g7, h7,
            a8, b8, c8, d8, e8, f8, g8, h8
        };


        /*                           x
         *     8             (0;0)--------7
         *     |                |
         * rows|           =>  y|
         *     |                |
         *     1a----------h    7
         * 
         * convert position of regular chess (a8 ... b7 ... etc)  to array position (i,j) from top right corner
         * */
        public static KeyValuePair<int,int> CoordChessToArray(string position)
        {
            char column = position[0];
            char row = position[1];

            int x = column - 'a';
            int y = 8 - int.Parse(row.ToString()); // Chess 1 => Array 7 and Chess 8 => Array 0

            return new KeyValuePair<int,int>(x, y);
        }

        /**
         *  convert array position to chess position
         * */
        public static string CoordArrayToChess(int x,int y)
        {
            string position = "";

            position += 'a' + x;
            position += (8 - y).ToString();

            return position;
        }

        // 0 .... 63 => (0;7) .... (7;0)
        public static int CoordArrayToBitBoard(int x, int y)
        {
            return 8 * x + y ;
        }

        public static KeyValuePair<int, int> CoordBitBoardToArray(int nb)
        {
            int x = nb % 8;
            int y = 8 - (nb+1 / 8);

            return new KeyValuePair<int, int>(x, y);
        }

    }
}
