using System;

namespace processAI1.Board
{
    public enum piece
    {
        PAWN,
        KNIGHT,
        BISHOP,
        ROOK,
        QUEEN,
        KING,
        NONE,
    };

    public enum side_Piece
    {
        WHITE_PAWN,
        BLACK_PAWN,
        WHITE_KNIGHT,
        BLACK_KNIGHT,
        WHITE_BISHOP,
        BLACK_BISHOP,
        WHITE_ROOK,
        BLACK_ROOK,
        WHITE_QUEEN,
        BLACK_QUEEN,
        WHITE_KING,
        BLACK_KING,
    };

    public static class Piece
    {
        public const int SIZE = 7;
        public const int SIDE_SIZE = 12;

        

        public const int PAWN_VALUE = 100;
        public const int KNIGHT_VALUE = 325;
        public const int BISHOP_VALUE = 325;
        public const int ROOK_VALUE = 500;
        public const int QUEEN_VALUE = 975;
        public const int KING_VALUE = 10000; // for SEE

        public static readonly int[] value = new []{ PAWN_VALUE, KNIGHT_VALUE, BISHOP_VALUE, ROOK_VALUE, QUEEN_VALUE, KING_VALUE, 0 };

        const string Char = "PNBRQK?";
        const string Fen_Char = "PpNnBbRrQqKk";



        public static bool IsMinor(piece pc)
        {
            return pc == piece.KNIGHT || pc == piece.BISHOP;
        }

        public static bool IsMajor(piece pc)
        {
            return pc == piece.ROOK || pc == piece.QUEEN;
        }

        public static bool IsSlider(piece pc)
        {
            return pc >= piece.BISHOP && pc <= piece.QUEEN;
        }

        //TODO pourquoi ?!
        public static int Score(piece pc)
        {
            return (int)pc;
        }

        public static int Value(piece pc)
        {
            return value[(int)pc];
        }

        public static int Make(piece pc, side sd)
        {
            if (pc == piece.NONE)
                throw new Exception("int does not represent a piece");

            return ((int)pc << 1) | (int)sd;
        }

        public static piece PieceType(int p12)
        {
            if(p12 >= SIDE_SIZE)
                throw new Exception("it does not represent a piece from side");
            return (piece)(p12 >> 1);
        }

        public static side PieceSide(int p12)
        {
            if (p12 >= SIDE_SIZE)
                throw new Exception("it does not represent a piece from side");
            return (side)(p12 & 1);
        }

        public static piece FromChar(char c)
        {
        return (piece)Char.IndexOf(c);
        }

        public static char ToChar(piece pc)
        {
            return Char[(int)pc];
        }

        public static int FromFen(char c)
        {
            return Fen_Char.IndexOf(c);
        }

        public static char ToFen(int p12)
        {
            if (p12 >= SIDE_SIZE)
                throw new Exception("it does not represent a piece from side");
            return Fen_Char[p12];
        }
    }
}