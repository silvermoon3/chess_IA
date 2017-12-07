using processAI1.Agent.BDI;
using processAI1.Board;
using System;
using System.Collections.Generic;
using System.Linq;


namespace processAI1.Agent
{
    class Agent
    {
        Belief _belief;
        Sensor _sensor;
        Intention _intention;
        Desire _desire;
        Move _bestMoveFound;
       

        public Agent()
        {
           _sensor = new Sensor();
           _belief = new Belief();
           _desire = new Desire();
           _intention = new Intention();
           _bestMoveFound = null;
          
        }
              

        public void UpdateBelief(string fenInput)
        {
            _belief.Update(fenInput);
        }

        public void Think()
        {
            /***** Think with Minimax ****/
            _bestMoveFound = Search.algoRoot(_belief.GetBoard(), 9);
            Console.WriteLine("bestmove " + _bestMoveFound);

        }

        public String[] GetBestMove()
        {
            if (_bestMoveFound == null)
            {
                //King is in check but we need to force it to move for plateforme
              // _bestMoveFound =  Gen.force_move_king();
              
            }
                

            String[] bestMove = new String[3];
            
            bestMove[0] = _bestMoveFound.from.ToString();
            bestMove[1] = _bestMoveFound.to.ToString();
            if (_bestMoveFound.isCastle)
            {
                bestMove[0] = Castling.WhichCastle(_bestMoveFound);
            }
        
            if (_bestMoveFound.GetPromoted() != piece.NONE)
            {
               

                if (_bestMoveFound.GetPromoted() == piece.ROOK)
                {
                    bestMove[2] = "TG";
                }
                else if(_bestMoveFound.GetPromoted() == piece.KNIGHT)
                {
                    bestMove[2] = "CG";
                }
                else if (_bestMoveFound.GetPromoted() == piece.BISHOP)
                {
                    bestMove[2] = "F";
                }
                else if (_bestMoveFound.GetPromoted() == piece.QUEEN)
                {
                    bestMove[2] = "D";
                }
            }
          
                
       
            return bestMove;
        }

        public String ReadWithSensor(int[] tabVal)
        {
          return  _sensor.GetFenBoard(tabVal);
        }
        
        public void DrawBelief(int[] tabVal)
        {
            _sensor.ShowBoard(tabVal);
        }
     

        public void ImportFen(string input)
        {
            //sensor.importFEN(input);
            this.UpdateBelief(input);
        }
    }
}
