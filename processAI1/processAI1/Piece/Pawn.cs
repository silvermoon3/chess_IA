using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1
{
    class Pawn: Piece
    {
        Boolean firstMove = true;
       
        public Pawn(colorPlayer colorPlayer): base(colorPlayer) { }
        override
       public List<Position> getPossibleMoves(Position currentPosition, Square[,] board)
        {
            List<Position> legalMoves = new List<Position>();
            if (firstMove)
            {
                //We can jump 2 case
                legalMoves.Add(new Position(currentPosition.getX(), currentPosition.getY()+2));

            }
            //Only one case
            legalMoves.Add(new Position(currentPosition.getX(), currentPosition.getY() + 1));

            foreach (Position pos in legalMoves)
                if (!ocuppiedOrOnPath(currentPosition, pos, board))
                    legalMoves.Remove(pos);

            Position positionUpRight = new Position(currentPosition.getX() + 1, currentPosition.getY() + 1);
            if (ocuppiedOrOnPath(currentPosition, positionUpRight, board))
                legalMoves.Add(positionUpRight);

            Position positionUpLeft = new Position(currentPosition.getX() + 1, currentPosition.getY() + 1);
            if (ocuppiedOrOnPath(currentPosition, positionUpLeft, board))
                legalMoves.Add(positionUpLeft);




            return null;
        }
        override
        public Boolean ocuppiedOrOnPath(Position currentPosition, Position desination, Square[,] board)
        {
            return false;
        }

    }
}
