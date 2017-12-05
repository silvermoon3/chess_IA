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

        public static Move simpleMiniMaxRoot(Board.Board fakeBoard, int depth, Boolean isMaximisingPlayer)
        {
            Move bestMoveFound = null;
            int bestScore = Int32.MinValue;
            List<Move> moves = new List<Move>();
            Gen.gen_legals(ref moves, ref fakeBoard);
            foreach (Move mv in moves)
            {
                fakeBoard.move(mv);
                int score = simpleMiniMax(fakeBoard, depth - 1, !isMaximisingPlayer);
                fakeBoard.undo();
                if (score >= bestScore)
                {
                    bestScore = score;
                    bestMoveFound = mv;
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
    }
}
