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
            start = DateTime.Now; //Pour calculer le temps de réponse de l'IA
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
                //with simple minimax
                // int score = simpleMiniMax(fakeBoard, depth - 1, !isMaximisingPlayer);
                //with alpha beta
                score = NegaMax(ref fakeBoard, -Score.MAX, -Score.MIN, depth - 1);
                fakeBoard.undo();

               if(score > bestScore)
                {
                    bestScore = score;
                    bestMoveFound = mv;
                }

                dur = DateTime.Now - Program.start;
                OneMoveAvg =new TimeSpan( moveSearched.Count > 0? (long)moveSearched.Average():0);
                moveSearched.Add(dur.Ticks);
                Console.WriteLine("Average : "+ OneMoveAvg.Milliseconds);
                for (int i=0; i < depths.Length;i++)
                {
                    Console.WriteLine(" depth "+i+" : "+ depths[i]);
                }
                Console.WriteLine("Move : " + mv + " temps : " + dur.TotalMilliseconds + " score : " + score);
                Console.WriteLine("current BestMove :" + bestMoveFound);
                //On renvoie le meilleur coup trouvé après 200 ms

            }

            return bestMoveFound;
        }
        public static int NegaMax(ref Board.Board fakeBoard, int alpha, int beta, int depthleft)
        {
            depths[(TotalDepth - 1) - depthleft]++;
            TimeSpan dur = DateTime.Now - Program.start;
            
            if (depthleft == 0)
                return Eval.eval(ref fakeBoard, ref EvaTable, ref PawnTable);
            List<Move> moves = new List<Move>();
            Gen.gen_legals_sort(ref moves, ref fakeBoard, ref EvaTable, ref PawnTable);
           
            foreach (Move mv in moves)
            {
                int score;
                fakeBoard.move(mv); 
                score = -NegaMax(ref fakeBoard,-beta, -alpha, depthleft - 1);
                fakeBoard.undo();
                if (score >= beta)
                    return beta;   //  fail hard beta-cutoff
                if (score > alpha)
                    alpha = score; // alpha acts like max in MiniMax
            }
            return alpha;
        }

       

    }
}
