using processAI1.Agent.BDI;
using processAI1.Board;
using processAI1.Piece;
using System;
using System.Collections.Generic;
using System.Linq;


namespace processAI1.Agent
{
    class Agent
    {
        Belief belief;
        Sensor sensor;
        Intention intention;
        Desire desire;
            
        private Boolean isWhite;
                      
        public Agent()
        {
           sensor = new Sensor();
           belief = new Belief();
           desire = new Desire();
           intention = new Intention();
        }
        public void doWork()
        {
            updateBelief();
              
        }

        public Sensor getEffector()
        {
            return sensor;
        }

        public void updateBelief()
        {
            belief.Update(sensor);
        }

        public void think()
        {
            //Look for the best move
            Random rnd = new Random();
            // coord[0] = mesPieces[rnd.Next(mesPieces.Count)];
            List<Move> moves = belief.getPossibleMoves();
            if (moves.Count != 0)
            {
                Move randomMove = moves.ElementAt(rnd.Next(moves.Count));
                belief.getChessBoard().makeMove(randomMove);
            }
        }

        public String[] getBestMove()
        {
            if (!desire.isOtherPlayerInChess(belief))
            {
                /************************************************************************/
                /************************ RANDOM MOVE ************************/
                /************************************************************************/
                //String[] coord = new String[2];
                //Random rnd = new Random();
                ////// coord[0] = mesPieces[rnd.Next(mesPieces.Count)];
                //List<Move> moves = belief.getPossibleMoves();
                //if (moves.Count != 0)
                //{
                //    Move randomMove = moves.ElementAt(rnd.Next(moves.Count));
                //    coord[0] = randomMove.getInitialPosition().ToString();
                //    coord[1] = randomMove.getFinalPosition().ToString();
                //    Console.WriteLine("coup : " + coord[0] + " " + coord[1]);
                //    //Update first move of Rook or King
                //    belief.updateFirstMove(randomMove.getInitialPosition());
                //    return coord;
                //}

                /************************************************************************/
                /************************ BEST MOVE WITH ONLY PIECE VALUE ************************/
                /************************************************************************/

                //String[] coord = new String[2];
                //Move move = getBestMoveWithPieceValue(belief.getFakeChessBoard());
                //coord[0] = move.getInitialPosition().ToString();
                //coord[1] = move.getFinalPosition().ToString();
                //Console.WriteLine("coup : " + coord[0] + " " + coord[1]);
                ////Update first move of Rook or King
                //belief.updateFirstMove(move.getInitialPosition());
                //return coord;

                /************************************************************************/
                /************************ Simple MiniMax Without alpha beta ************************/
                /************************************************************************/

                String[] coord = new String[2];
                Move move = getBestMoveWithSimpleMinMax(belief.getFakeChessBoard());
                coord[0] = move.getInitialPosition().ToString();
                coord[1] = move.getFinalPosition().ToString();
                Console.WriteLine("coup : " + coord[0] + " " + coord[1]);
                //Update first move of Rook or King
                belief.updateFirstMove(move.getInitialPosition());
                return coord;

            }
            //if (!desire.isOtherPlayerInChess(belief))
            //{
            //    MiniMax algo = new MiniMax();
            //    Move move = algo.getBestMove(belief.getFakeChessBoard(), 3);
            //    String[] coord = new String[2];
            //    if (!move.IWantARoque())
            //    {
            //        if (!intention.willIBeInCheck(belief, move))
            //        {
            //            belief.updateFirstMove(move.getInitialPosition());
            //            coord[0] = move.getInitialPosition().ToString();
            //            coord[1] = move.getFinalPosition().ToString();
            //        }

            //    }
            //else
            //{
            //    coord[0] = move.getRoque();
            //    //Update piece position for a roque
            //}

            //    return coord;
            //}
            return null;
        }

        public void setColor(Boolean _isWhite)
        {
            isWhite = _isWhite;
        }


        //Best Move with only value for a piece 
        public Move getBestMoveWithPieceValue(ChessBoard fakeBoard)
        {
            Move bestMove = null;
            List<Move> moves = fakeBoard.getAllPossibleMoves();
            int bestValue = Int32.MinValue;
            foreach(Move mv in moves)
            {
                fakeBoard.makeMove(mv, fakeBoard.getPiece(mv.getInitialPosition()));
                int score = -fakeBoard.EvaluateBoardWithPieceValue();
                fakeBoard.undoMove(mv, fakeBoard.getPiece(mv.getFinalPosition()));
                if (score > bestValue)
                {
                    bestValue = score;
                    bestMove = mv;
                }
                    
            }
            return bestMove;
        }

        public Move getBestMoveWithSimpleMinMax(ChessBoard fakeBoard)
        {
            MiniMax algo = new MiniMax();
            return algo.simpleMiniMaxRoot(fakeBoard, 3, true);
        }
       
    }
}
