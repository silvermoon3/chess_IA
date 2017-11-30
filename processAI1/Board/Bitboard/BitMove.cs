using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace processAI1.Board.Bitboard
{
    public enum Direction
    {
        N, NE, E, SE, S, SW, W, NW
    }

    public static class DirectionMethods
    {
        public static Direction addSubDir(this Direction d,Direction sd)
        {
            Direction newdir;
            if (d.ToString().Length == 1 && sd.ToString().Length == 1) 
                Direction.TryParse(d.ToString() + sd.ToString(),out newdir);
            else
                throw new Exception("invalide direction pour jointure");

            return newdir;
        }

        private static char invert(char d)
        {
            char inverseDir;
            switch (d)
            {
                case 'N':
                    inverseDir = 'S';
                    break;
                case 'S':
                    inverseDir = 'N';
                    break;
                case 'E':
                    inverseDir = 'W';
                    break;
                case 'W':
                    inverseDir = 'E';
                    break;
                default:
                    throw new Exception("dir invalide");
            }
            return inverseDir;

        }
        public static Direction InverseOf(this Direction d)
        {
            string inverseDir = "" + invert(d.ToString()[0]);

            if (d.ToString().Length == 2)
            {
                inverseDir += invert(d.ToString()[1]);
            }
            Direction.TryParse(inverseDir,out Direction dir);

            return dir;

        }
    }

    public abstract class BitMove
    {

        public BitBoard BitBoard;
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

       

        public BitMove(BitBoard bitBoard)
        {
            BitBoard = bitBoard;
        }

      
        public abstract List<Move> PossibleMoves(Boolean isWhite);

        public List<Move> ListFromTargetPos(ulong targetPosBitboard, int startoffSetX, int startoffSetY)
        {
            List<Move> list = new List<Move>();
            ulong possibility = targetPosBitboard & ~(targetPosBitboard - 1);

            while (possibility != 0)
            {
                int index = BitBoardUtil.BitScanForward(possibility);

                list.Add(
                    new Move(
                        new Point(BitBoard.GetCoord[index].X + startoffSetX, (BitBoard.GetCoord[index].Y + startoffSetY)),
                        new Point(BitBoard.GetCoord[index].X, (BitBoard.GetCoord[index].Y))
                    ));

                targetPosBitboard &= ~possibility;
                possibility = targetPosBitboard & ~(targetPosBitboard - 1);
            }
            return list;
        }
        //Compass Rose
        public static UInt64 MoveOne(UInt64 bitboard, Direction d)
        {
            switch (d)
            {
                case Direction.N:
                    return bitboard << 8;
                case Direction.NE:
                    return (bitboard << 9) & ~AFile;
                case Direction.E:
                    return (bitboard << 1) & ~AFile;
                case Direction.SE:
                    return (bitboard >> 7) & ~AFile;
                case Direction.S:
                    return bitboard >> 8;
                case Direction.SW:
                    return (bitboard >> 9) & ~HFile;
                case Direction.W:
                    return (bitboard >> 1) & ~HFile;
                case Direction.NW:
                    return (bitboard << 7) & ~HFile;
                default:
                    throw new Exception("direction invalide");
            }
        }
    }
}
