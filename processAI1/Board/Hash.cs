using System;
using processAI1.Board.Bitboard;

namespace processAI1.Board
{
    public static class Hash
    {

        const int TURN = Piece.SIDE_SIZE * Square.SIZE;
        const int CASTLE = TURN + 1;
        const int EN_PASSANT = CASTLE + 4;
        const int SIZE = EN_PASSANT + 8;

        public static UInt64[] p_rand = new UInt64[SIZE];

        public static UInt64 Rand64()
        {

            UInt64 rand = 0;

            for (int i = 0; i < 4; i++)
            {
                rand = (rand << 16) | (UInt64)Util.rand_int(1 << 16);
            }

            return rand;
        }

        public static UInt64 RandKey(int index)
        {
            //TODO ASSERT
            //assert(index >= 0 && index < SIZE);
            return p_rand[index];
        }

        public static UInt64 PieceKey(int p12, int sq)
        {
            return RandKey(p12 * Square.SIZE + sq);
        }

        public static UInt64 TurnKey(int turn)
        {
            return (turn == Side.WHITE) ? 0 : RandKey(TURN);
        }

        public static UInt64 TurnFlip()
        {
            return RandKey(TURN);
        }

        public static UInt64 FlagKey(int flag)
        {
            //TODO ASSERT
            //assert(flag < 4);
            return RandKey(CASTLE + flag);
        }

        public static UInt64 EnPassantKey(int sq)
        {
            return (sq == (int)Square.square.NONE) ? 0 : RandKey(EN_PASSANT + Square.File(sq));
        }

        public static Int64 Index(UInt64 key)
        {
            return (Int64)key;
        }

        public static UInt32 Lock(UInt64 key) {
            return (UInt32)(key >> 32);
        }

        public static void Init()
        {
            for (int i = 0; i < SIZE; i++)
            {
                p_rand[i] = Rand64();
            }
        }
}
}