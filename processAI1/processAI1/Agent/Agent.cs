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
                      
        public Agent()
        {
          
           effector = new Effector();
           belief = new Belief();     
           
        }
        public void doWork()
        {
            updateBelief(effector.getBoard());
           // belief.getChessBoard().Show();
           // think();

            //Afficher le Board

           
        }

        public Effector getEffector()
        {
            return effector;
        }

        public void updateBelief(Cell[,] board)
        {
            belief.Update(board);
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
            //String[] coord = new String[2];
            //Random rnd = new Random();
            //// coord[0] = mesPieces[rnd.Next(mesPieces.Count)];
            //List<Move> moves = belief.getPossibleMoves();
            //Move randomMove = moves.ElementAt(rnd.Next(moves.Count));
            //coord[0] = randomMove.getInitialPosition().ToString();
            //coord[1] = randomMove.getFinalPosition().ToString();

            MiniMax algo = new MiniMax();
            Move move = algo.getBestMove(belief.getChessBoard(), 3);
            String[] coord = new String[2];
            coord[0] = move.getInitialPosition().ToString();
            coord[1] = move.getFinalPosition().ToString();
            return coord;
        }

       


    }
}
