using System;
using System.Diagnostics;

namespace processAI1.Board
{
    public static class Material
    {
        public const int PAWN_PHASE = 0;
        public const int KNIGHT_PHASE = 1;
        public const int BISHOP_PHASE = 1;
        public const int ROOK_PHASE = 2;
        public const int QUEEN_PHASE = 4;

        public const int TOTAL_PHASE =
            PAWN_PHASE * 16 + KNIGHT_PHASE * 4 + BISHOP_PHASE * 4 + ROOK_PHASE * 4 + QUEEN_PHASE * 2;

        public static readonly int[] p_phase = new int[Piece.SIZE]
            {PAWN_PHASE, KNIGHT_PHASE, BISHOP_PHASE, ROOK_PHASE, QUEEN_PHASE, 0, 0};


        public static readonly int[] p_power = new int[Piece.SIZE] {0, 1, 1, 2, 4, 0, 0};

        public static readonly int[][] p_score = new int[Piece.SIZE][]
        {
            new[] {85, 95}, new[] {325, 325}, new[] {325, 325}, new[] {460, 540}, new[] {975, 975}, new[] {0, 0},
            new[] {0, 0}
        };

        public static int[] phase_weight = new int[TOTAL_PHASE + 1];

        public static int power(piece pc)
        {
            return p_power[(int) pc];
        }

        public static int score(piece pc, Stage.stage stage)
        {
            //Debug.Assert(stage < Stage.SIZE);
            return p_score[(int) pc][(int)stage];
        }

        public static int force(side sd, ref Board bd)
        {
            // for draw eval

            int force = 0;

            for (int pc = (int) piece.KNIGHT; pc <= (int) piece.QUEEN; pc++)
            {
                force += bd.count((piece) pc, sd) * power((piece) pc);
            }

            return force;
        }

        public static bool lone_king(side sd, ref Board bd)
        {
            return bd.count(piece.KNIGHT, sd) == 0
                   && bd.count(piece.BISHOP, sd) == 0
                   && bd.count(piece.ROOK, sd) == 0
                   && bd.count(piece.QUEEN, sd) == 0;
        }

        public static bool lone_bishop(side sd, ref Board bd)
        {
            return bd.count(piece.KNIGHT, sd) == 0
                   && bd.count(piece.BISHOP, sd) == 1
                   && bd.count(piece.ROOK, sd) == 0
                   && bd.count(piece.QUEEN, sd) == 0;
        }

        public static bool lone_king_or_bishop(side sd, ref Board bd)
        {
            return bd.count(piece.KNIGHT, sd) == 0
                   && bd.count(piece.BISHOP, sd) <= 1
                   && bd.count(piece.ROOK, sd) == 0
                   && bd.count(piece.QUEEN, sd) == 0;
        }

        public static bool lone_king_or_minor(side sd, ref Board bd)
        {
            return bd.count(piece.KNIGHT, sd) + bd.count(piece.BISHOP, sd) <= 1
                   && bd.count(piece.ROOK, sd) == 0
                   && bd.count(piece.QUEEN, sd) == 0;
        }

        public static bool two_knights(side sd, ref Board bd)
        {
            return bd.count(piece.KNIGHT, sd) == 2
                   && bd.count(piece.BISHOP, sd) == 0
                   && bd.count(piece.ROOK, sd) == 0
                   && bd.count(piece.QUEEN, sd) == 0;
        }

        public static int interpolation(int mg, int eg, ref Board bd)
        {
            int phase = Math.Min(bd.phase(), TOTAL_PHASE);
            Debug.Assert(phase >= 0 && phase <= TOTAL_PHASE);

            int weight = phase_weight[phase];
            return (mg * weight + eg * (256 - weight) + 128) >> 8;
        }

        public static void init()
        {
            for (int i = 0; i <= TOTAL_PHASE; i++)
            {
                double x = (double) i / (double) (TOTAL_PHASE / 2) - 1.0;
                double y = 1.0 / (1.0 + Math.Exp(-x * 5.0));
                phase_weight[i] = Util.round(y * 256.0);
            }
        }

        public static int phase(piece pc)
        {
            return p_phase[(int) pc];
        }
    }
}