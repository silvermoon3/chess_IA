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

        public static Move algoRoot(Board.Board fakeBoard, int depth, Boolean isMaximisingPlayer)
        {
            DateTime start = DateTime.Now; //Pour calculer le temps de réponse de l'IA
            Move bestMoveFound = null;
            int bestScore = Int32.MinValue;
            List<Move> moves = new List<Move>();
            Gen.gen_legals(ref moves, ref fakeBoard);
            foreach (Move mv in moves)
            {
                fakeBoard.move(mv);
                //with simple minimax
                // int score = simpleMiniMax(fakeBoard, depth - 1, !isMaximisingPlayer);
                //with alpha beta
                int score = alphaBeta(fakeBoard, depth - 1, -10000, 10000, !isMaximisingPlayer);
                fakeBoard.undo();
                TimeSpan dur = DateTime.Now - start;
                if (score >= bestScore)
                {
                    bestScore = score;
                    bestMoveFound = mv;
                }
                //On renvoie le meilleur coup trouvé après 200 ms
                if (dur.TotalMilliseconds >= 200)
                {
                    return bestMoveFound;
                }
            }
                      
            return bestMoveFound;
        }

        private static int simpleMiniMax(Board.Board fakeBoard, int depth, Boolean isMaximisingPlayer)
        {
            if (depth == 0)
            {
                int bestScore = Eval.CompEval(ref fakeBoard, fakeBoard.turn());
                Console.WriteLine("bestscore : " + bestScore);
                return bestScore ;
            }
                

            List<Move> moves = new List<Move>();
            Gen.gen_legals(ref moves, ref fakeBoard);
            if (isMaximisingPlayer)
            {
                int bestScore = Int32.MinValue;
                foreach (Move mv in moves)
                {
                    fakeBoard.move(mv);
                    bestScore = Math.Max(bestScore, simpleMiniMax(fakeBoard, depth - 1, !isMaximisingPlayer));
                    
                    fakeBoard.undo();
                }
                return bestScore;
            }
            else
            {
                int bestScore = Int32.MaxValue;
                foreach (Move mv in moves)
                {
                    fakeBoard.move(mv);
                    bestScore = Math.Min(bestScore, simpleMiniMax(fakeBoard, depth - 1, !isMaximisingPlayer));
                    fakeBoard.undo();
                }
                return bestScore;

            }
        }



        /******** ALPHA BETA PRUNNING ********/
        private static int alphaBeta(Board.Board fakeBoard, int depth, int alpha, int beta, Boolean isMaximisingPlayer)
        {
            if (depth == 0)
            {
                int bestScore = -Eval.CompEval(ref fakeBoard, fakeBoard.turn());
                //Console.WriteLine("bestscore : " + bestScore);
                return bestScore;
            }


            List<Move> moves = new List<Move>();
            Gen.gen_legals(ref moves, ref fakeBoard);
            if (isMaximisingPlayer)
            {
                int bestScore = Int32.MinValue;
                foreach (Move mv in moves)
                {
                    fakeBoard.move(mv);
                    bestScore = Math.Max(bestScore, alphaBeta(fakeBoard, depth - 1, alpha, beta, !isMaximisingPlayer));
                    fakeBoard.undo();
                    alpha = Math.Max(alpha, bestScore);
                    if (beta <= alpha)
                        return bestScore;
                }
                return bestScore;
            }
            else
            {
                int bestScore = Int32.MaxValue;
                foreach (Move mv in moves)
                {
                    fakeBoard.move(mv);
                    bestScore = Math.Min(bestScore, alphaBeta(fakeBoard, depth - 1, alpha, beta, !isMaximisingPlayer));
                    fakeBoard.undo();
                    beta = Math.Min(beta, bestScore);
                    if (beta <= alpha)
                        return bestScore;
                }
                return bestScore;

            }
        }

    }
}
