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

        protected static int[] pawnTable = {
         0,  0,  0,  0, 0,  0,  0, 0,
        50,  50,  50,  50,  50,  50,  50,  50,
        10,  10,  20,  30,  30,  20,  10,  10,
        5,  5,  10,  25,  25,  10,  5,  5,
        0,  0,  0,  20,  20,  0,  0,  0,
        5, -5, -10,  0,  0, -10, -5,  5,
        5,  10, 10,  -20, -20,  10,  10,  5,
        0,  0,  0,  0,  0,  0,  0,  0};


      
        // Placement Precedence for all Knights
        protected static int[] knightTable = {
          -50,-40,-30,-30,-30,-30,-40,-50,
          -40,-20,  0,  0,  0,  0,-20,-40,
          -30,  0, 10, 15, 15, 10,  0,-30,
          -30,  5, 15, 20, 20, 15,  5,-30,
          -30,  0, 15, 20, 20, 15,  0,-30,
          -30,  5, 10, 15, 15, 10,  5,-30,
          -40,-20,  0,  5,  5,  0,-20,-40,
          -50,-40,-30,-30,-30,-30,-40,-50 };

        protected static int[] bishopTable = {
          -20,-10,-10,-10,-10,-10,-10,-20,
          -10,  0,  0,  0,  0,  0,  0,-10,
          -10,  0,  5, 10, 10,  5,  0,-10,
          -10,  5,  5, 10, 10,  5,  5,-10,
          -10,  0, 10, 10, 10, 10,  0,-10,
          -10, 10, 10, 10, 10, 10, 10,-10,
          -10,  5,  0,  0,  0,  0,  5,-10,
          -20,-10,-10,-10,-10,-10,-10,-20 };

        protected static int[] rookTable = {
            0,  0,  0,  0,  0,  0,  0,  0,
            5, 10, 10, 10, 10, 10, 10,  5,
           -5,  0,  0,  0,  0,  0,  0, -5,
           -5,  0,  0,  0,  0,  0,  0, -5,
           -5,  0,  0,  0,  0,  0,  0, -5,
           -5,  0,  0,  0,  0,  0,  0, -5,
           -5,  0,  0,  0,  0,  0,  0, -5,
            0,  0,  0,  5,  5,  0,  0,  0 };

        protected static int[] queenTable = {
          -20,-10,-10, -5, -5,-10,-10,-20,
          -10,  0,  0,  0,  0,  0,  0,-10,
          -10,  0,  5,  5,  5,  5,  0,-10,
           -5,  0,  5,  5,  5,  5,  0, -5,
            0,  0,  5,  5,  5,  5,  0, -5,
          -10,  5,  5,  5,  5,  5,  0,-10,
          -10,  0,  5,  0,  0,  0,  0,-10,
          -20,-10,-10, -5, -5,-10,-10,-20 };

        protected static int[] kingTable = {
          -30,-40,-40,-50,-50,-40,-40,-30,
          -30,-40,-40,-50,-50,-40,-40,-30,
          -30,-40,-40,-50,-50,-40,-40,-30,
          -30,-40,-40,-50,-50,-40,-40,-30,
          -20,-30,-30,-40,-40,-30,-30,-20,
          -10,-20,-20,-20,-20,-20,-20,-10,
           20, 20,  0,  0,  0,  0, 20, 20,
           20, 30, 10,  0,  0, 10, 30, 20 };

        static int MAX_DEPTH;

        // Minimax AI constructor
        public MiniMax()
        {
          
        }


        public Move getBestMove(ChessBoard game, int depth)
        {           
           
            this.depth = depth;
            alphaBetaMax(game, Int32.MinValue, Int32.MaxValue, depth);
            return bestNode.getMove();
        }



        private int alphaBetaMax(ChessBoard game, int alpha, int beta, int depthLeft)
        {
            List<Move> possibleMoves = game.getAllPossibleMoves();
            if (depthLeft == 0 || possibleMoves.Count == 0)
                return evaluateBoard(game);

           
            foreach (Move mv in possibleMoves)
            {
                game.makeMove(mv);
                int score = alphaBetaMin(game, alpha, beta, depthLeft - 1);
                game.undoMove(mv);
                Node node = new Node(mv, score);
            if (depthLeft == depth)
                {
                    if (bestNode != null)
                    {
                        if (node.getValue() > bestNode.getValue())
                            bestNode = node;
                    }
                    else
                        bestNode = node;
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

        private int alphaBetaMin(ChessBoard game, int alpha, int beta, int depthLeft)
        {
            List<Move> possibleMoves = game.getAllPossibleMoves();
            if (depthLeft == 0 || possibleMoves.Count == 0)
                return evaluateBoard(game);


            //sorting 
            foreach (Move mv in possibleMoves)
            {
                game.makeMove(mv);
                int score = alphaBetaMax(game, alpha, beta, depthLeft - 1);
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
        public int getPieceValue(Piece.Piece piece)
        {
            if (piece is Pawn)
            {
                return 10;
            }
            else if (piece is Rook)
            {
                return 50;
            }
            else if (piece is Bishop)
            {
                return 30;
            }
            else if (piece is Knight)
            {
                return 30;
            }
            else if (piece is Queen)
            {
                return 90;
            }
            else if (piece is King)
            {
                return 900;
            }
            return 0;

        }

        public int getPieceValue(Piece.Piece piece, int x, int y )
        {
            if (piece == null)
                return 0;
           if(piece is Pawn)
            {
                if (piece.isWhite)                
                    return pawnTable[8 * (7 - y + x)];
                
                else
                    return pawnTable[8 + (8 * x ) - (1 + y)];
            }
            else if (piece is Rook)
            {
                if (piece.isWhite)
                    return rookTable[8 * (7 - y + x)];

                else
                    return rookTable[8 + (8 * x) - (1 + y)];
            }
            else if (piece is Bishop)
            {
                if (piece.isWhite)
                    return bishopTable[8 * (7 - y + x)];

                else
                    return bishopTable[8 + (8 * x) - (1 + y)];
            }
            else if (piece is Knight)
            {
                if (piece.isWhite)
                    return knightTable[8 * (7 - y + x)];

                else
                    return knightTable[8 + (8 * x) - (1 + y)];
            }
            else if (piece is Queen)
            {
                if (piece.isWhite)
                    return queenTable[8 * (7 - y + x)];

                else
                    return queenTable[8 + (8 * x) - (1 + y)];
            }
            else if (piece is King)
            {
                if (piece.isWhite)
                    return kingTable[8 * (7 - y + x)];

                else
                    return kingTable[8 + (8 * x) - (1 + y)];
            }


            return 0;
        }
        private int evaluateBoard(ChessBoard game)
        {
            int total = 0;
            for(int i=0; i<8; i++)
            {
                for(int j=0; j<8; j++)
                {
                    total += getPieceValue(game.getCase(i, j).getPiece());
                }
            }
            return total;
               
        }

    }
}
