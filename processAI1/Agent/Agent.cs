using processAI1.Agent.BDI;
using processAI1.Board;
using processAI1.Board.Chessboard.Piece;
using System;
using System.Collections.Generic;
using System.Linq;
using processAI1.Board.Chessboard;


namespace processAI1.Agent
{
    class Agent
    {
        Belief _belief;
        Sensor _sensor;
        Intention _intention;
        Desire _desire;
                      
        public Agent()
        {
           _sensor = new Sensor();
           _belief = new Belief();
           _desire = new Desire();
           _intention = new Intention();
        }
        public void DoWork()
        {
            UpdateBelief();
              
        }

        public Sensor GetEffector()
        {
            return _sensor;
        }

        public void UpdateBelief()
        {
            _belief.Update(_sensor);
        }

        public void Think()
        {
            //Look for the best move
            Random rnd = new Random();
            // coord[0] = mesPieces[rnd.Next(mesPieces.Count)];
            List<Move> moves = _belief.GetPossibleMoves();
            if (moves.Count != 0)
            {
                Move randomMove = moves.ElementAt(rnd.Next(moves.Count));
                _belief.GetChessBoard().MakeMove(randomMove);
            }
        }

        public String[] GetBestMove()
        {
            if (!_desire.IsOtherPlayerInChess(_belief))
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
                Move move = GetBestMoveWithSimpleMinMax();
                coord[0] = move.GetInitialPosition().ToString();
                coord[1] = move.GetFinalPosition().ToString();
                //Console.WriteLine("coup : " + coord[0] + " " + coord[1]);
                //Update first move of Rook or King
                _belief.UpdateFirstMove(move.GetInitialPosition());
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
        


        //Best Move with only value for a piece 
        public Move GetBestMoveWithPieceValue(ChessBoard fakeBoard)
        {
            Move bestMove = null;
            List<Move> moves = fakeBoard.GetAllPossibleMoves();
            int bestValue = Int32.MinValue;
            foreach(Move mv in moves)
            {
                fakeBoard.MakeMove(mv);
                int score = -fakeBoard.EvaluateBoardWithPieceValue();
                fakeBoard.UndoMove(mv);
                if (score > bestValue)
                {
                    bestValue = score;
                    bestMove = mv;
                }
                    
             }
            return bestMove;
        }

        public Move GetBestMoveWithSimpleMinMax()
        {
            ChessBoard fakeBoard = _belief.GetFakeChessBoard();
            MiniMax algo = new MiniMax();
            return algo.SimpleMiniMaxRoot(fakeBoard, 3, _belief.IsWhite);
        }

        public void DrawBelief()
        {
            _belief.GetChessBoard().DrawBoard();
        }

        public void ImportFen(string input)
        {
            //sensor.importFEN(input);
            this.UpdateBelief();
        }
    }
}
