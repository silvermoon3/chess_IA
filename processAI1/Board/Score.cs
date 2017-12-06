using System.Diagnostics;

namespace processAI1.Board
{
    public static class Score
    {
        public const int NONE = -10000;
        public const int MIN = -9999;
        public const int EVAL_MIN = -8999;
        public const int EVAL_MAX = +8999;
        public const int MAX = +9999;
        public const int MATE = +10000;

        public const int FLAGS_NONE = 0;
        public const int FLAGS_LOWER = 1 << 0;
        public const int FLAGS_UPPER = 1 << 1;
        public const int FLAGS_EXACT = FLAGS_LOWER | FLAGS_UPPER;

        public static bool is_mate(int sc)
        {
            return sc < EVAL_MIN || sc > EVAL_MAX;
        }

        public static int signed_mate(int sc)
        {
            if (sc < EVAL_MIN)
            {
                // -MATE
                return -(MATE + sc) / 2;
            }
            else if (sc > EVAL_MAX)
            {
                // +MATE
                return (MATE - sc + 1) / 2;
            }
            else
            {
                Debug.Assert(false);
                return 0;
            }
        }

        public static int side_score(int sc, side sd)
        {
            return (sd == side.WHITE) ? +sc : -sc;
        }

        public static int from_trans(int sc, int ply)
        {
            if (sc < EVAL_MIN)
            {
                return sc + ply;
            }
            else if (sc > EVAL_MAX)
            {
                return sc - ply;
            }
            else
            {
                return sc;
            }
        }

        public static int to_trans(int sc, int ply)
        {
            if (sc < EVAL_MIN)
            {
                return sc - ply;
            }
            else if (sc > EVAL_MAX)
            {
                return sc + ply;
            }
            else
            {
                return sc;
            }
        }

        public static int flags(int sc, int alpha, int beta)
        {
            int flags = FLAGS_NONE;
            if (sc > alpha) flags |= FLAGS_LOWER;
            if (sc < beta) flags |= FLAGS_UPPER;

            return flags;
        }
    }
}