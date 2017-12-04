using System;
using System.Diagnostics;

namespace processAI1.Board
{
    public static class Square
    {

        public const int FILE_SIZE = 8;
        public const int RANK_SIZE = 8;
        public const int SIZE = FILE_SIZE * RANK_SIZE;

        public enum file
        {
            FILE_A,
            FILE_B,
            FILE_C,
            FILE_D,
            FILE_E,
            FILE_F,
            FILE_G,
            FILE_H,
        };

        public enum rank
        {
            RANK_1,
            RANK_2,
            RANK_3,
            RANK_4,
            RANK_5,
            RANK_6,
            RANK_7,
            RANK_8,
        };

        public enum square{
            NONE = -1,
            A1, A2, A3, A4, A5, A6, A7, A8,
            B1, B2, B3, B4, B5, B6, B7, B8,
            C1, C2, C3, C4, C5, C6, C7, C8,
            D1, D2, D3, D4, D5, D6, D7, D8,
            E1, E2, E3, E4, E5, E6, E7, E8,
            F1, F2, F3, F4, F5, F6, F7, F8,
            G1, G2, G3, G4, G5, G6, G7, G8,
            H1, H2, H3, H4, H5, H6, H7, H8,
        };

        public enum inc {
            INC_LEFT = -8,
            INC_RIGHT = +8,
        };

        public const int CASTLING_DELTA = 16;
        public const int DOUBLE_PAWN_DELTA = 2;

        public static int Make(int fl, int rk)
        {
            if(fl >= 8)
                throw new Exception("fl >= 8");
            if (rk >= 8)
                throw new Exception("rk >= 8");

            return (fl << 3) | rk;
        }

        public static int Make(int fl, int rk, int sd)
        {

            if (fl >= 8)
                throw new Exception("fl >= 8");
            if (rk >= 8)
                throw new Exception("rk >= 8");

            return Make(fl, (rk ^ -sd) & 7);
        }

        public static int File(int sq)
        {
            return sq >> 3;
        }

        public static int Rank(int sq)
        {
            return sq & 7;
        }

        public static int Rank(int sq, int sd)
        {
            return (sq ^ -sd) & 7;
        }

        public static int OppositFile(int sq)
        {
            return sq ^ 070;
        }

        public static int OppositRank(int sq)
        {
            return sq ^ 007;
        }

        public static bool IsPromotion(int sq)
        {
            int rk = Rank(sq);
            return rk == (int)rank.RANK_1 || rk == (int)rank.RANK_8;
        }

        public static int Colour(int sq)
        {
            return ((sq >> 3) ^ sq) & 1;
        }

        public static bool SameColour(int s0, int s1)
        {
            int diff = s0 ^ s1;
            return (((diff >> 3) ^ diff) & 1) == 0;
        }

        public static bool SameLine(int s0, int s1)
        {
            return File(s0) == File(s1) || Rank(s0) == Rank(s1);
        }

        public static int FileDistance(int s0, int s1)
        {
            return Math.Abs(File(s1) - File(s0));
        }

        public static int RankDistance(int s0, int s1)
        {
            return Math.Abs(Rank(s1) - Rank(s0));
        }

        public static int Distance(int s0, int s1)
        {
            return Math.Max(FileDistance(s0, s1), RankDistance(s0, s1));
        }

        public static int PawnInc(int sd)
        {
            return (sd == Side.WHITE) ? +1 : -1;
        }

        public static int Stop(int sq, int sd)
        {
            return sq + PawnInc(sd);
        }

        public static int Promotion(int sq, int sd)
        {
            return Make(File(sq), (int)rank.RANK_8, sd);
        }

        public static bool IsValid88(int s88)
        {
            return (s88 & 0x88) == 0;
        }

        public static int To88(int sq)
        {
            return sq + (sq & ~7);
        }

        public static int From88(int s88)
        {
            Debug.Assert(IsValid88(s88));

            return (s88 + (s88 & 7)) >> 1;
        }

        public static int FromFen(int sq)
        {
            return Make(sq & 7, (sq >> 3) ^ 7);
        }

        public static int FromString(ref string s) {
            
            if (s.Length != 2)
                throw new Exception("invalid 88 expression");

            return Make(s[0] - 'a', s[1] - '1');
        }

        public static String ToString(int sq)
        {

            string s="";
            s += (char)('a' + File(sq));
            s += (char)('1' + Rank(sq));

            return s;
        }
}
}