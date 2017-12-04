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

        public Move simpleMiniMaxRoot(ChessBoard fakeBoard, int depth, Boolean isMaximisingPlayer)
        {
            Move bestMoveFound = null;
            int bestScore = Int32.MinValue;
            List<Move> moves = fakeBoard.getAllPossibleMoves();
            foreach(Move mv in moves)
            {
                fakeBoard.makeMove(mv, fakeBoard.getPiece(mv.getInitialPosition()));
                int score = simpleMiniMax(fakeBoard, depth - 1, !isMaximisingPlayer);
                fakeBoard.undoMove(mv, fakeBoard.getPiece(mv.getFinalPosition()));
                if (score >= bestScore)
                {
                    bestScore = score;
                    bestMoveFound = mv;
                }
            }
            return bestMoveFound;

        }

        private int simpleMiniMax(ChessBoard fakeBoard, int depth, Boolean isMaximisingPlayer)
        {
            if (depth == 0)
                return -fakeBoard.EvaluateBoardWithPieceValue();
            List<Move> moves = fakeBoard.getAllPossibleMoves();
            if (isMaximisingPlayer)
            {
                int bestScore = Int32.MinValue;
                foreach(Move mv in moves)
                {
                    fakeBoard.makeMove(mv, fakeBoard.getPiece(mv.getInitialPosition()));
                    bestScore = Math.Max(bestScore, simpleMiniMax(fakeBoard, depth - 1, !isMaximisingPlayer));
                    fakeBoard.undoMove(mv, fakeBoard.getPiece(mv.getFinalPosition()));
                }
                return bestScore;
            }
            else
            {
                int bestScore = Int32.MaxValue;
                foreach (Move mv in moves)
                {
                    fakeBoard.makeMove(mv, fakeBoard.getPiece(mv.getInitialPosition()));
                    bestScore = Math.Min(bestScore, simpleMiniMax(fakeBoard, depth - 1, !isMaximisingPlayer));
                    fakeBoard.undoMove(mv, fakeBoard.getPiece(mv.getFinalPosition()));
                }
                return bestScore;

            }
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
                        if (n.getValue() >= bestNode.getValue())
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
                return -game.EvaluateBoardWithPieceValue();

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
