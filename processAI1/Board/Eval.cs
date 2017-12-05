namespace processAI1.Board
{
    public static class Eval
    {

        public static int CompEval(ref Board bd, side s)
        {
            return MaterialEval(ref bd, s) + MobilityEval(ref bd,s);
        }
        public static int MaterialEval(ref Board bd, side s)
        {
            int score = 0;
            for (int pc = 0; pc < Piece.SIDE_SIZE; pc++)
            {
                if (Piece.PieceSide(pc) == s)
                {
                    score += bd.count(Piece.PieceType(pc), s) * Material.score(Piece.PieceType(pc), Stage.stage.MG);
                }
                else
                {
                    score -= bd.count(Piece.PieceType(pc), Side.Opposit(s)) * Material.score(Piece.PieceType(pc), Stage.stage.MG);
                }
                    
            }
            
            return score;
        }

        public static int MobilityEval(ref Board bd, side s)
        {
            int score = 0;
            for (int sq = 0; sq < Square.SIZE; sq++)
            {
                piece pc = bd.getSquare((square) sq);
                if(pc == piece.NONE)
                    continue;
                if (bd.square_is((square) sq, pc, s))
                    score += PieceSquareTable.score(Piece.Make(pc, s), (square) sq, Stage.stage.MG);
                else
                    score -= PieceSquareTable.score(Piece.Make(pc, s), (square)sq, Stage.stage.MG);
            }
            return score;

        }
    }
}