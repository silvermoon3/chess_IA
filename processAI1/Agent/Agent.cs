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
            

            //Afficher le Board
            for (int i = 7; i >= 0; i--)
            {
                switch (i)
                {
                    case 0: Console.Write("8"); break;
                    case 1: Console.Write("7"); break;
                    case 2: Console.Write("6"); break;
                    case 3: Console.Write("5"); break;
                    case 4: Console.Write("4"); break;
                    case 5: Console.Write("3"); break;
                    case 6: Console.Write("2"); break;
                    case 7: Console.Write("1"); break;
                }
                Console.Write("|");
                for (int j = 7; j >= 0; j--)
                {
                    if(belief.isOccupied(i,j))
                     Console.Write(belief.getCase(i, j).getPiece().getPiece());
                    else
                    
                        Console.Write("x");
                    
                    if(j == 0)
                    {
                        Console.Write("\n");
                        //retour à la ligne
                    }
                }
            }          
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
        }

        public String[] getBestMove()
        {
            String[] coord = new String[2];
            Random rnd = new Random();
            // coord[0] = mesPieces[rnd.Next(mesPieces.Count)];
            List<Move> moves = belief.getPossibleMoves();
            Move randomMove = moves.ElementAt(rnd.Next(moves.Count));
            coord[0] = randomMove.GetInitialPosition().ToString();
            coord[1] = randomMove.GetFinalPosition().ToString();

            return coord;
        }

        private Move bestMove(int depth, Boolean isMaximisingPlayer)
        {            
            List<Move> moves = belief.getPossibleMoves();
            int[] bestresults = new int[moves.Count];
            Move bestMoveFound = new Move();
            foreach(Move mv in moves)
            {
                
            }
            return bestMoveFound;
        }

        private Move MiniMax(int depth, int alpha, int beta, Boolean isMaximisingPlayer)
        {

            return null;
        }


     


    }
}
