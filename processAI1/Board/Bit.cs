using System;

namespace processAI1.Board
{
    public static class Bit
    {
        private static readonly int[] index = {
           0,  1,  2,  7,  3, 13,  8, 19,
           4, 25, 14, 28,  9, 34, 20, 40,
           5, 17, 26, 38, 15, 46, 29, 48,
          10, 31, 35, 54, 21, 50, 41, 57,
          63,  6, 12, 18, 24, 27, 33, 39,
          16, 37, 45, 47, 30, 53, 49, 56,
          62, 11, 23, 32, 36, 44, 52, 55,
          61, 22, 43, 51, 60, 42, 59, 58,
       };

        //8
        private static UInt64[] _pLeft = new UInt64[8];

        private static UInt64[] _pRight = new UInt64[8];
        private static UInt64[] _pFront = new UInt64[8];
        private static UInt64[] _pRear = new UInt64[8];

        // Side.size 8
        private static UInt64[][] _pSideFront = new UInt64[Side.SIZE][];

        private static UInt64[][] _pSideRear = new UInt64[Side.SIZE][];

        public static UInt64 MakeBit(square pos)
        {
            return 1UL << (int)pos;
        }



        public static void Init()
        {

            {
                UInt64 bf = 0;
                UInt64 br = 0;

                for (int i = 0; i < 8; i++)
                {
                    _pLeft[i] = bf;
                    _pRear[i] = br;
                    bf |= File((file)i);
                    br |= Rank((rank)i);
                }
            }

            {
                UInt64 bf = 0;
                UInt64 br = 0;

                for (int i = 7; i >= 0; i--)
                {
                    _pRight[i] = bf;
                    _pFront[i] = br;
                    bf |= File((file)i);
                    br |= Rank((rank)i);
                }
            }
            _pSideFront[(int)side.WHITE] = new UInt64[8];
            _pSideFront[(int)side.BLACK] = new UInt64[8];
            _pSideRear[(int)side.WHITE] = new UInt64[8];
            _pSideRear[(int)side.BLACK] = new UInt64[8];

            for (int rk = 0; rk < 8; rk++)
            {
                _pSideFront[(int)side.WHITE][(int)rk] = Front((rank)rk);
                _pSideFront[(int)side.BLACK][(int)rk] = Rear((rank)rk);
                _pSideRear[(int)side.WHITE][(int)rk] = Rear((rank)rk);
                _pSideRear[(int)side.BLACK][(int)rk] = Front((rank)rk);
            }
        }


        public static void Set(ref UInt64 b, square pos)
        {
            b |= MakeBit(pos);
        }


        public static void Clear(ref UInt64 b, square pos)
        {
            b &= ~MakeBit(pos);
        }

        public static bool IsSet(UInt64 b, square pos)
        {
            return (b & MakeBit(pos)) != 0;
        }


        public static int First(UInt64 b)
        {
            
            return index[((b & (~b + 1)) * 0x218A392CD3D5DBFUL) >> (64 - 6)];
        }

        public static UInt64 Rest(UInt64 b)
        {
            if (b == 0)
                throw new Exception("b == 0");
            return b & (b - 1);
        }

        public static int Count(UInt64 b)
        {
            b = b - ((b >> 1) & 0x5555555555555555UL);
            b = (b & 0x3333333333333333UL) + ((b >> 2) & 0x3333333333333333UL);
            b = (b + (b >> 4)) & 0x0F0F0F0F0F0F0F0FUL;
            b = (b * 0x0101010101010101UL) >> 56;
            return (int) b;
        }

        public static bool Single(UInt64 b)
        {
            if (b == 0)
                throw new Exception("b == 0");

            return Rest(b) == 0;
        }

        public static UInt64 File(file fl)
        {

            return 0xFFUL << ((int)fl * 8);
        }

        public static UInt64 Rank(rank rk)
        {
            return 0x0101010101010101UL << (int)rk;
        }


        public static UInt64 Files(file fl)
        {
            UInt64 file = Bit.File(fl);
            return (file << 8) | file | (file >> 8);
        }

        public static UInt64 Left(file fl)
        {
            return _pLeft[(int)fl];
        }

        public static UInt64 Right(file fl)
        {
            return _pRight[(int)fl];
        }

        public static UInt64 Front(rank rk)
        {
            return _pFront[(int)rk];
        }

        public static UInt64 Rear(rank rk)
        {
            return _pRear[(int)rk];
        }

        public static UInt64 Front(square sq, side sd)
        {
            rank rk = Square.Rank(sq);
            return _pSideFront[(int)sd][(int)rk];
        }

        public static UInt64 Rear(square sq, side sd)
        {
            rank rk = Square.Rank(sq);
            return _pSideRear[(int)sd][(int)rk];
        }

        
        public static string to_string(UInt64 b)
        {
            string[] s = new string[8];

            for (int sq = Square.SIZE - 1; sq >= 0; sq--)
            {
                if (IsSet(b, (square)sq))
                    s[7 - (int)Square.Rank((square)sq)] += "[X]";
                else
                    s[7 - (int)Square.Rank((square)sq)] += "[ ]";
            }
            return String.Join("\n", s);
        }
    }
}