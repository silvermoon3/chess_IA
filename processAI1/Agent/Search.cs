using processAI1.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Agent
{
    public static class  Search
    {
        public static Eval.Table EvaTable = new Eval.Table();
        public static Pawn.Table PawnTable = new Pawn.Table();
        public static DateTime start;
        public static int TotalDepth;
        public static Move algoRoot(Board.Board fakeBoard, int depth)
        {
            TotalDepth = depth;
            EvaTable = new Eval.Table();
            PawnTable = new Pawn.Table();
            start = DateTime.Now; //Pour calculer le temps de réponse de l'IA
            Move bestMoveFound = null;
            int bestScore = 0;
            List<Move> moves = new List<Move>();
            Gen.gen_legals_sort(ref moves, ref fakeBoard,ref EvaTable,ref PawnTable);
            TimeSpan dur = DateTime.Now - start;
            foreach (Move mv in moves)
            {
                fakeBoard.move(mv);
                //with simple minimax
                //int score = simpleMiniMax(fakeBoard, depth - 1, !isMaximisingPlayer);
                //with alpha beta
                int score = alphaBeta(fakeBoard, depth - 1, Score.EVAL_MIN, Score.EVAL_MAX);
                fakeBoard.undo();

                if (bestMoveFound == null||(fakeBoard.turn() == side.WHITE && score < bestScore) || (fakeBoard.turn() == side.BLACK && score < bestScore))
                {
                    bestScore = score;
                    bestMoveFound = mv;
                }
                dur = DateTime.Now - start;
                Console.WriteLine("Move :"+mv +"temps : " + dur.TotalMilliseconds+ " score :" + score);
                Console.WriteLine("current BestMove :" + bestMoveFound);
                //On renvoie le meilleur coup trouvé après 200 ms

            }

            return bestMoveFound;
        }

        private static int simpleMiniMax(Board.Board fakeBoard, int depth)
        {
            if (depth == 0)
            {
                int bestScore = Eval.eval(ref fakeBoard,ref EvaTable,ref PawnTable);
                Console.WriteLine("bestscore : " + bestScore);
                return bestScore ;
            }
            TimeSpan dur = DateTime.Now - start;
            Console.WriteLine("temps : " + dur.TotalMilliseconds + " depth:"+(TotalDepth - depth) + " / " + TotalDepth);
            List<Move> moves = new List<Move>();
            Gen.gen_legals(ref moves, ref fakeBoard);
            if (fakeBoard.turn() == side.WHITE)
            {
                int bestScore = Score.EVAL_MIN +1 ;
                foreach (Move mv in moves)
                {
                    fakeBoard.move(mv);
                    bestScore = Math.Max(bestScore, simpleMiniMax(fakeBoard, depth - 1));
                    
                    fakeBoard.undo();
                }
                return bestScore;
            }
            else
            {
                int bestScore = Score.EVAL_MAX -1;
                foreach (Move mv in moves)
                {
                    fakeBoard.move(mv);
                    bestScore = Math.Min(bestScore, simpleMiniMax(fakeBoard, depth - 1));
                    fakeBoard.undo();
                }
                return bestScore;

            }
        }



        /******** ALPHA BETA PRUNNING ********/
        private static int alphaBeta(Board.Board fakeBoard, int depth, int alpha, int beta)
        {
            int bestScore=0;
            TimeSpan dur = DateTime.Now - start;
            if (depth == 0 || dur.TotalMilliseconds >= 1000)
            {
                bestScore = Eval.eval(ref fakeBoard, ref EvaTable, ref PawnTable);
                //Console.WriteLine("bestscore : " + bestScore);
                return bestScore;
            }
            

            List<Move> moves = new List<Move>();
            Gen.gen_legals(ref moves, ref fakeBoard);
            if (fakeBoard.turn() == side.WHITE)
            {
                bestScore = Score.EVAL_MIN +1;
                foreach (Move mv in moves)
                {
                    fakeBoard.move(mv);
                    bestScore = Math.Max(bestScore, alphaBeta(fakeBoard, depth - 1, alpha, beta));
                    fakeBoard.undo();
                    alpha = Math.Max(alpha, bestScore);
                    if (beta <= alpha)
                        return bestScore;
                    
                }
            }
            else
            {
                bestScore = Score.EVAL_MAX-1;
                foreach (Move mv in moves)
                {
                    fakeBoard.move(mv);
                    bestScore = Math.Min(bestScore, alphaBeta(fakeBoard, depth - 1, alpha, beta));
                    fakeBoard.undo();
                    beta = Math.Min(beta, bestScore);
                    if (beta <= alpha)
                        return bestScore;
                }
               
            }

            return bestScore;
        }

    }
}
