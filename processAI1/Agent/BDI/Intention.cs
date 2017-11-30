
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Board;

namespace processAI1.Agent.BDI
{
    class Intention
    {

        public Intention()
        {

        }

        //Check if I will be in check if I do this move 
        public Boolean WillIBeInCheck(Belief belief, Move mv)
        {
            //Is my king in check if I do this move ? 
            belief.GetFakeChessBoard().MakeMove(mv);
            belief.GetFakeChessBoard().AmIInCheck();
            belief.GetFakeChessBoard().UndoMove(mv);
            return false;
        }
        
    }
}
