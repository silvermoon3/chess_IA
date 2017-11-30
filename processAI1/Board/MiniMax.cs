using processAI1.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Board.Chessboard;

namespace processAI1.Agent
{
    class MiniMax
    {
        private int _evaluateOption;
        private Node _bestNode = null;
        private int _depth = -1;     
        // Minimax AI constructor
        public MiniMax()
        {
            this._evaluateOption = 1;
        }
        
        public Move GetBestMove(ChessBoard game, int depth)
        {
            this._depth = depth;
            AlphaBetaMax(game, new Node(), Int32.MinValue, Int32.MaxValue, depth);
            return _bestNode.GetMove();
        }

        public Move SimpleMiniMaxRoot(ChessBoard fakeBoard, int depth, Boolean isMaximisingPlayer)
        {
            Move bestMoveFound = null;
            int bestScore = Int32.MinValue;
            List<Move> moves = fakeBoard.GetAllPossibleMoves(isMaximisingPlayer);
            foreach(Move mv in moves)
            {
                fakeBoard.MakeMove(mv);
                int score = SimpleMiniMax(fakeBoard, depth - 1, !isMaximisingPlayer);
                fakeBoard.UndoMove(mv);
                if(score >= bestScore)
                {
                    bestMoveFound = mv;
                    bestScore = score;
                }
            }
            return bestMoveFound;

        }

        private int SimpleMiniMax(ChessBoard fakeBoard, int depth, Boolean isMaximisingPlayer)
        {
            if (depth == 0)
                return -fakeBoard.EvaluateBoardWithPieceValue();
            List<Move> moves = fakeBoard.GetAllPossibleMoves();
            if (isMaximisingPlayer)
            {
                int bestScore = Int32.MinValue;
                foreach(Move mv in moves)
                {
                    fakeBoard.MakeMove(mv);
                    bestScore = Math.Max(bestScore, SimpleMiniMax(fakeBoard, depth - 1, !isMaximisingPlayer));
                    fakeBoard.UndoMove(mv);
                }
                return bestScore;
            }
            else
            {
                int bestScore = Int32.MaxValue;
                foreach (Move mv in moves)
                {
                    fakeBoard.MakeMove(mv);
                    bestScore = Math.Min(bestScore, SimpleMiniMax(fakeBoard, depth - 1, !isMaximisingPlayer));
                    fakeBoard.UndoMove(mv);
                }
                return bestScore;

            }
        }

        private int AlphaBetaMax(ChessBoard game, Node node, int alpha, int beta, int depthLeft)
        {
            List<Move> possibleMoves = game.GetAllPossibleMoves();
            if (depthLeft == 0 || possibleMoves.Count == 0)
                return game.Evaluate(_evaluateOption, true);

            foreach (Move mv in possibleMoves)
            {
                game.MakeMove(mv);
                Node n = new Node(mv, 0);
                int score = AlphaBetaMin(game, n, alpha, beta, depthLeft - 1);
                n.SetValue(score);
                game.UndoMove(mv);
               
                if (depthLeft == _depth)
                {
                    if (_bestNode != null)
                    {
                        if (n.GetValue() > _bestNode.GetValue())
                            _bestNode = n;
                    }
                    else
                        _bestNode = n;
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

        private int AlphaBetaMin(ChessBoard game, Node node, int alpha, int beta, int depth)
        {
            List<Move> possibleMoves = game.GetAllPossibleMoves();
            if (depth == 0 || possibleMoves.Count == 0)
                return -game.EvaluateBoardWithPieceValue();

            //sorting 
            foreach (Move mv in possibleMoves)
            {
                game.MakeMove(mv);
                Node n = new Node(mv, 0);
                int score = AlphaBetaMax(game, node, alpha, beta, depth - 1);
                n.SetValue(score);
                game.UndoMove(mv);
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
