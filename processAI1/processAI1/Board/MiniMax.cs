using processAI1.Board;
using processAI1.Piece;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Agent
{
    class MiniMax
    {
        private int evaluateOption;
        private Node bestNode = null;
        private int depth = -1;     
        // Minimax AI constructor
        public MiniMax()
        {
            this.evaluateOption = 1;
        }
        
        public Move getBestMove(ChessBoard game, int depth)
        {           
           
            this.depth = depth;
            alphaBetaMax(game, new Node(), Int32.MinValue, Int32.MaxValue, depth);
            return bestNode.getMove();
        }


        private int alphaBetaMax(ChessBoard game, Node node, int alpha, int beta, int depthLeft)
        {
            List<Move> possibleMoves = game.getAllPossibleMoves();
            if (depthLeft == 0 || possibleMoves.Count == 0)
                return game.evaluate(evaluateOption, true);

            foreach (Move mv in possibleMoves)
            {
                game.makeMove(mv);
                Node n = new Node(mv, 0);
                int score = alphaBetaMin(game, n, alpha, beta, depthLeft - 1);
                n.setValue(score);
                game.undoMove(mv);
               
                if (depthLeft == depth)
                {
                    if (bestNode != null)
                    {
                        if (n.getValue() > bestNode.getValue())
                            bestNode = n;
                    }
                    else
                        bestNode = n;
                }
                if (score >= beta)
                {
                    return beta;
                }
                if (score > alpha)
                {
                    alpha = score;
                }

            }
            return alpha;


        }

        private int alphaBetaMin(ChessBoard game, Node node, int alpha, int beta, int depth)
        {
            List<Move> possibleMoves = game.getAllPossibleMoves();
            if (depth == 0 || possibleMoves.Count == 0)
                return game.evaluate(evaluateOption, false);


            //sorting 
            foreach (Move mv in possibleMoves)
            {
                game.makeMove(mv);
                Node n = new Node(mv, 0);
                int score = alphaBetaMax(game, node, alpha, beta, depth - 1);
                n.setValue(score);
                game.undoMove(mv);
                if (score <= alpha)
                {
                    return alpha;
                }
                if (score < beta)
                {
                    beta = score;
                }
            }
            return beta;
        }


                
    }
}
