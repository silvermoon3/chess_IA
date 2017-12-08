using processAI1.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Agent
{
    public static class Search
    {
        public static Eval.Table EvaTable = new Eval.Table();
        public static Pawn.Table PawnTable = new Pawn.Table();
        public static DateTime start;
        public static int TotalDepth;
        public static int[] depths;
        static TimeSpan OneMoveAvg = new TimeSpan();


        public static Move algoRoot(Board.Board fakeBoard, int depth, bool isMaximisingPlayer)
        {
            TotalDepth = depth;
            depths = new int[TotalDepth];
            EvaTable = new Eval.Table();
            PawnTable = new Pawn.Table();
            Move bestMoveFound = null;
            int bestScore = Int32.MinValue;
            List<Move> moves = new List<Move>();
            Gen.gen_legals_sort(ref moves, ref fakeBoard, ref EvaTable, ref PawnTable);
            side playingSide = fakeBoard.turn();
            TimeSpan dur;
            List<long> moveSearched = new List<long>();
            foreach (Move mv in moves)
            {
                dur = DateTime.Now - Program.start;
                Console.WriteLine("Move : " + mv + " start search at : " + dur.Milliseconds);
                fakeBoard.move(mv);
                int score;
                score = Min(ref fakeBoard, Score.MIN, Score.MAX, depth - 1);
                fakeBoard.undo();

               if(score > bestScore)
                {
                    bestScore = score;
                    bestMoveFound = mv;
                }
               
                Console.WriteLine("Move : " + mv + " score : " + score);
                Console.WriteLine("current BestMove :" + bestMoveFound);

            }

            return bestMoveFound;
        }

        public static int Max(ref Board.Board fakeBoard, int alpha, int beta, int depth)
        {

            if (depth == 0) return Eval.eval(ref fakeBoard, ref EvaTable, ref PawnTable);
            List<Move> sortMoves = new List<Move>();
            Gen.gen_legals_sort(ref sortMoves,ref fakeBoard,ref EvaTable,ref PawnTable);

            foreach (var move in sortMoves)
            {
                fakeBoard.move(move);
                int score = Min(ref fakeBoard, alpha, beta, depth - 1);
                fakeBoard.undo();
                if (score >= beta)
                {
                    return beta;   // fail hard beta-cutoff
                }
                if (score > alpha)
                    alpha = score; // alpha acts like max in MiniMax

            }

            return alpha;

        }

        public static int Min(ref Board.Board fakeBoard, int alpha, int beta, int depth)
        {
            if (depth == 0)
                return -Eval.eval(ref fakeBoard, ref EvaTable, ref PawnTable);
            List<Move> sortMoves = new List<Move>();
            Gen.gen_legals_sort(ref sortMoves, ref fakeBoard, ref EvaTable, ref PawnTable);

            foreach (var move in sortMoves)
            {
                fakeBoard.move(move);
                int score = Max(ref fakeBoard, alpha, beta, depth - 1);
                fakeBoard.undo();
                if (score <= alpha)
                {
                    return alpha; // fail hard alpha-cutoff
                }
                    
                if (score < beta)
                    beta = score; // beta acts like min in MiniMax

            }

            return beta;
        }


    }
}
