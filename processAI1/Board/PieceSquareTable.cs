namespace processAI1.Board
{
    public static class PieceSquareTable
    {

        public static readonly int[] Knight_Line = { -4, -2, 0, +1, +1, 0, -2, -4 };
        public static readonly int[] King_Line = { -3, -1, 0, +1, +1, 0, -1, -3 };
        public static readonly int[] King_File = { +1, +2, 0, -2, -2, 0, +2, +1 };
        public static readonly int[] King_Rank= { +1, 0, -2, -4, -6, -8, -10, -12 };

        public static readonly int[] Advance_Rank = { -3, -2, -1, 0, +1, +2, +1, 0 };

        public static int[][][] p_score;//[piece::SIDE_SIZE][Square.SIZE] [Stage.stage::SIZE];

        public static int score(int p12, square sq, Stage.stage stage)
        {
            return p_score[p12][(int)sq][(int)stage];
        }

        public static void init()
        {
            p_score = new int[Piece.SIDE_SIZE][][];
            for (int p12 = 0; p12 < Piece.SIDE_SIZE; p12++)
            {
                p_score[p12] = new int[Square.SIZE][];
                for (int sq = 0; sq < Square.SIZE; sq++)
                {
                    p_score[p12][sq] = new int[Stage.SIZE];
                    p_score[p12][sq][(int)Stage.stage.MG] = 0;
                    p_score[p12][sq][(int)Stage.stage.EG] = 0;
                }
            }

            for (int sq = 0; sq < Square.SIZE; sq++)
            {

                int fl = (int)Square.File((square)sq);
                int rk = (int)Square.Rank((square)sq);

                p_score[(int)side_Piece.WHITE_PAWN][sq][(int)Stage.stage.MG] = 0;
                p_score[(int)side_Piece.WHITE_PAWN][sq][(int)Stage.stage.EG] = 0;

                p_score[(int)side_Piece.WHITE_KNIGHT][sq][(int)Stage.stage.MG] = (Knight_Line[fl] + Knight_Line[rk] + Advance_Rank[rk]) * 4;
                p_score[(int)side_Piece.WHITE_KNIGHT][sq][(int)Stage.stage.EG] = (Knight_Line[fl] + Knight_Line[rk] + Advance_Rank[rk]) * 4;

                p_score[(int)side_Piece.WHITE_BISHOP][sq][(int)Stage.stage.MG] = (King_Line[fl] + King_Line[rk]) * 2;
                p_score[(int)side_Piece.WHITE_BISHOP][sq][(int)Stage.stage.EG] = (King_Line[fl] + King_Line[rk]) * 2;

                p_score[(int)side_Piece.WHITE_ROOK][sq][(int)Stage.stage.MG] = King_Line[fl] * 5;
                p_score[(int)side_Piece.WHITE_ROOK][sq][(int)Stage.stage.EG] = 0;

                p_score[(int)side_Piece.WHITE_QUEEN][sq][(int)Stage.stage.MG] = (King_Line[fl] + King_Line[rk]) * 1;
                p_score[(int)side_Piece.WHITE_QUEEN][sq][(int)Stage.stage.EG] = (King_Line[fl] + King_Line[rk]) * 1;

                p_score[(int)side_Piece.WHITE_KING][sq][(int)Stage.stage.MG] = (King_File[fl] + King_Rank[rk]) * 8;
                p_score[(int)side_Piece.WHITE_KING][sq][(int)Stage.stage.EG] = (King_Line[fl] + King_Line[rk] + Advance_Rank[rk]) * 8;
            }

            for (int pc = (int)piece.PAWN; pc <= (int)piece.KING; pc++)
            {

                int wp = Piece.Make((piece)pc, side.WHITE);
                int bp = Piece.Make((piece)pc, side.BLACK);

                for (int bs = 0; bs < Square.SIZE; bs++)
                {

                    int ws = Square.OppositRank((square)bs);

                    p_score[bp][bs][(int)Stage.stage.MG] = p_score[wp][ws][(int)Stage.stage.MG];
                    p_score[bp][bs][(int)Stage.stage.EG] = p_score[wp][ws][(int)Stage.stage.EG];
                }
            }
        }
    }
}