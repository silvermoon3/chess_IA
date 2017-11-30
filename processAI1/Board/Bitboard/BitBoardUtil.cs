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
        public const UInt64 DiagonalAtoH = 0x8040201008040201;
        public const UInt64 DiagonalHtoA = 0x0102040810204080;
        public const UInt64 LightSquares = 0x55AA55AA55AA55AA;
        public const UInt64 DarkSquares = 0xAA55AA55AA55AA55;

        public enum Direction
        {
            N, Ne, E, Se, S, Sw, W, Nw
        }

        static int[] _index64 = new[]{
        0, 47,  1, 56, 48, 27,  2, 60,
        57, 49, 41, 37, 28, 16,  3, 61,
        54, 58, 35, 52, 50, 42, 21, 44,
        38, 32, 29, 23, 17, 11,  4, 62,
        46, 55, 26, 59, 40, 36, 15, 53,
        34, 51, 20, 43, 31, 22, 10, 45,
        25, 39, 14, 33, 19, 30,  9, 24,
        13, 18,  8, 12,  7,  6,  5, 63
        };

    /**
     * bitScanForward
     * @author Kim Walisch (2012)
     * @param bb bitboard to scan
     * @precondition bb != 0
     * @return index (0..63) of least significant one bit
     */
    public static int BitScanForward(UInt64 bb)
    {
    const UInt64 debruijn64 = 0x03f79d71b4cb0a89UL;
    if(bb == 0) throw new Exception();
        return _index64[((bb ^ (bb - 1)) * debruijn64) >> 58];
    }

    }
}
