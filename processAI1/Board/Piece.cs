using System;

namespace processAI1.Board
{
    public static class Piece
    {
        public const int SIZE = 7;
        public const int SIDE_SIZE = 12;

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

        enum side_Piece
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

        const int PAWN_VALUE = 100;
        const int KNIGHT_VALUE = 325;
        const int BISHOP_VALUE = 325;
        const int ROOK_VALUE = 500;
        const int QUEEN_VALUE = 975;
        const int KING_VALUE = 10000; // for SEE

        public static readonly int[] value = new []{ PAWN_VALUE, KNIGHT_VALUE, BISHOP_VALUE, ROOK_VALUE, QUEEN_VALUE, KING_VALUE, 0 };

        const string Char = "PNBRQK?";
        const string Fen_Char = "PpNnBbRrQqKk";



        public static bool IsMinor(int pc)
        {
            if(pc >= SIZE)
                throw new Exception("int does not represent a piece");
            return pc == (int)piece.KNIGHT || pc == (int)piece.BISHOP;
        }

        public static bool IsMajor(int pc)
        {
            if (pc >= SIZE)
                throw new Exception("int does not represent a piece");
            return pc == (int)piece.ROOK || pc == (int)piece.QUEEN;
        }

        public static bool IsSlider(int pc)
        {
            if (pc >= SIZE)
                throw new Exception("int does not represent a piece");
            return pc >= (int)piece.BISHOP && pc <= (int)piece.QUEEN;
        }

        public static int Score(int pc)
        {

            if (pc >= SIZE)
                throw new Exception("int does not represent a piece");
            if (pc == (int)piece.NONE)
                throw new Exception("piece is none");

            return pc;
        }

        public static int Value(int pc)
        {
            if (pc >= SIZE)
                throw new Exception("int does not represent a piece");
            return value[pc];
        }

        public static int Make(int pc, int sd)
        {
            if (pc >= SIZE)
                throw new Exception("int does not represent a piece");
            if(pc == (int)piece.NONE)
                throw new Exception("int does not represent a piece");
            if (sd >= Side.SIZE)
                throw new Exception("not a side");

            return (pc << 1) | sd;
        }

        public static int PieceType(int p12)
        {
            if(p12 >= SIDE_SIZE)
                throw new Exception("it does not represent a piece from side");
            return p12 >> 1;
        }

        public static int PieceSide(int p12)
        {
            if (p12 >= SIDE_SIZE)
                throw new Exception("it does not represent a piece from side");
            return p12 & 1;
        }

        public static int FromChar(char c)
        {
        return Char.IndexOf(c);
        }

        public static char ToChar(int pc)
        {
            if (pc >= SIZE)
                throw new Exception("int does not represent a piece");

            return Char[pc];
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