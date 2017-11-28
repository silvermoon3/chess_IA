using processAI1.Piece;
using System;
using System.Collections.Generic;
using System.Linq;


namespace processAI1.Agent
{
    class Agent
    {

        Belief belief;
        Effector effector;
        private Boolean isWhite;
                      
        public Agent()
        {
          
           effector = new Effector();
           belief = new Belief();     
           
        }
        public void doWork()
        {
            updateBelief();
           // belief.getChessBoard().Show();
           // think();

            //Afficher le Board

           
        }

        public Effector getEffector()
        {
            return effector;
        }

        public void updateBelief()
        {
            belief.Update(effector);
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
            String[] coord = new String[2];
            Random rnd = new Random();
            //// coord[0] = mesPieces[rnd.Next(mesPieces.Count)];
            List<Move> moves = belief.getPossibleMoves();
            if(moves.Count != 0)
            {
                Move randomMove = moves.ElementAt(rnd.Next(moves.Count));
                coord[0] = randomMove.getInitialPosition().ToString();
                coord[1] = randomMove.getFinalPosition().ToString();
                Console.WriteLine("coup : " + coord[0] + " " + coord[1]);
                belief.updatePiecePosition(randomMove.getFinalPosition(), randomMove.getInitialPosition());
            }
            
            //MiniMax algo = new MiniMax();
            //Move move = algo.getBestMove(belief.getChessBoard(), 3);
            //String[] coord = new String[2];
            //if (!move.IWantARoque())
            //{
            //    belief.updatePiecePosition(move.getFinalPosition(), move.getInitialPosition());
            //    coord[0] = move.getInitialPosition().ToString();
            //    coord[1] = move.getFinalPosition().ToString();
              
            //}
            //else
            //{
            //    coord[0] = move.getRoque();
            //    //Update piece position for a roque
            //}
            
              return coord;
        }

        public void setColor(Boolean _isWhite)
        {
            isWhite = _isWhite;
        }
    }
}
