using processAI1.Piece;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Agent.BDI
{
    class Intention
    {

        public Intention()
        {

        }

        //Check if I will be in check if I do this move 
        public Boolean willIBeInCheck(Belief belief, Move mv)
        {
            //Is my king in check if I do this move ? 
            belief.getFakeChessBoard().makeMove(mv);
            belief.getFakeChessBoard().amIInCheck();
            belief.getFakeChessBoard().undoMove(mv);
            return false;
        }
        
    }
}
