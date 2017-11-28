using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Board.Bitboard
{
    class BitMove
    {

        public const UInt64 AFile = 0x0101010101010101;
        public const UInt64 HFile = 0x8080808080808080;
        public const UInt64 Rank1 = 0x00000000000000FF;
        public const UInt64 Rank2 = 0x000000000000FF00;
        public const UInt64 Rank3 = 0x0000000000FF0000;
        public const UInt64 Rank4 = 0x00000000FF000000;
        public const UInt64 Rank5 = 0x000000FF00000000;
        public const UInt64 Rank6 = 0x0000FF0000000000;
        public const UInt64 Rank7 = 0x00FF000000000000;
        public const UInt64 Rank8 = 0xFF00000000000000;
        public const UInt64 DiagonalAtoH = 0x8040201008040201;
        public const UInt64 DiagonalHtoA = 0x0102040810204080;
        public const UInt64 LightSquares = 0x55AA55AA55AA55AA;
        public const UInt64 DarkSquares = 0xAA55AA55AA55AA55;

        public enum direction
        {
            N, NE, E, SE, S, SW, W, NW
        }

        public UInt64 WSinglePushTargets(UInt64 wpawns, UInt64 empty)
        {
            return MoveOne(wpawns,direction.N) & empty;
        }

        public UInt64 WDblPushTargets(UInt64 wpawns, UInt64 empty)
        {
            UInt64 singlePushs = WSinglePushTargets(wpawns, empty);
            return MoveOne(singlePushs,direction.N) & empty & Rank4;
        }

        public UInt64 WPawnsAble2Push(UInt64 wpawns, UInt64 empty)
        {
            return MoveOne(empty,direction.S) & wpawns;
        }

        public UInt64 WPawnsAble2DblPush(UInt64 wpawns, UInt64 empty)
        {
            UInt64 emptyRank3 = MoveOne(empty & Rank4,direction.S) & empty;
            return WPawnsAble2Push(wpawns, emptyRank3);
        }

        // Black pawns
        public UInt64 BSinglePushTargets(UInt64 bpawns, UInt64 empty)
        {
            return MoveOne(bpawns,direction.S) & empty;
        }

        public UInt64 BDoublePushTargets(UInt64 bpawns, UInt64 empty)
        {
            
            UInt64 singlePushs = BSinglePushTargets(bpawns, empty);
            return MoveOne(singlePushs,direction.S) & empty & Rank5;
        }

        public UInt64 BPawnsAble2Push(UInt64 bpawns, UInt64 empty)
        {
            return MoveOne(empty, direction.N) & bpawns;
        }

        public UInt64 BPawnsAble2DblPush(UInt64 bpawns, UInt64 empty)
        {
            UInt64 emptyRank3 = MoveOne(empty & Rank5, direction.N) & empty;
            return BPawnsAble2Push(bpawns, emptyRank3);
        }



        //Compass Rose
        public static UInt64 MoveOne(UInt64 bitboard, direction d)
        {
            switch (d)
            {
                case direction.N:
                    return bitboard << 8;
                    break;
                case direction.NE:
                    return (bitboard << 9) & ~AFile;
                    break;
                case direction.E:
                    return (bitboard << 1) & ~AFile;
                    break;
                case direction.SE:
                    return (bitboard >> 7) & ~AFile;
                    break;
                case direction.S:
                    return bitboard >> 8;
                    break;
                case direction.SW:
                    return (bitboard >> 9) & ~HFile;
                    break;
                case direction.W:
                    return (bitboard >> 1) & ~HFile;
                    break;
                case direction.NW:
                    return (bitboard << 7) & ~HFile;
                    break;
                default:
                    throw new Exception("direction invalide");
                    break;
            }
        }
    }
}
