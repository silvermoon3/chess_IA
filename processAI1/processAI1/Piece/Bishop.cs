using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1
{
    class Bishop : Piece
    {
        
        public Bishop(colorPlayer colorPlayer): base(colorPlayer) { }

        override
        public List<Position> getPossibleMoves(Position currentPosition, Square[,] board)
        {
            //
            return null;
        }
        override
        public Boolean ocuppiedOrOnPath(Position currentPosition, Position desination, Square[,] board)
        {
            return false;
        }

    }
}
