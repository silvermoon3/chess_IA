using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Board.Bitboard
{
    static class BitBoardUtil
    {
        public const UInt64 AFile = 0x0101010101010101;
        public const UInt64 HFile = 0x8080808080808080;
        public const UInt64 Rank1 = 0x00000000000000FF;
        public const UInt64 Rank8 = 0xFF00000000000000;
        public const UInt64 DiagonalAToH = 0x8040201008040201;
        public const UInt64 DiagonalHToA = 0x0102040810204080;
        public const UInt64 LightSquares = 0x55AA55AA55AA55AA;
        public const UInt64 DarkSquares = 0xAA55AA55AA55AA55;

        public enum direction
        {
            N, NE, E, SE, S, SW, W, NW
        }

        

    }
}
