using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1
{
    class King : Piece
    {
        public King(colorPlayer colorPlayer): base(colorPlayer) { }

        override
        public List<Position> getPossibleMoves(Position currentPosition, Square[,] board)
        {
            return allLegalMoves(currentPosition, board);
        }
        

        public List<Position> allLegalMoves(Position currentPosition, Square[,] board)
        {
            List<Position> legalMoves = new List<Position>();
            //up
            legalMoves.Add(new Position(currentPosition.getX(), currentPosition.getY() + 1));

            //upRight
            legalMoves.Add(new Position(currentPosition.getX() + 1, currentPosition.getY() + 1));

            //upLeft
            legalMoves.Add(new Position(currentPosition.getX() - 1, currentPosition.getY() + 1));

            //down
            legalMoves.Add(new Position(currentPosition.getX(), currentPosition.getY() - 1));

            //downRight
            legalMoves.Add(new Position(currentPosition.getX() + 1, currentPosition.getY() - 1));

            //downLeft
            legalMoves.Add(new Position(currentPosition.getX() - 1, currentPosition.getY() - 1));

            //right
            legalMoves.Add(new Position(currentPosition.getX() + 1, currentPosition.getY()));

            //left
            legalMoves.Add(new Position(currentPosition.getX() - 1, currentPosition.getY()));

            foreach (Position p in legalMoves)
                if (ocuppiedOrOnPath(currentPosition, p, board))
                    legalMoves.Remove(p);

            return legalMoves;

        }


        override
        public Boolean ocuppiedOrOnPath(Position currentPosition, Position destinations, Square[,] board)
        {
            //check if there is no piece on the destination
            return Belief.isOccupied(destinations);
          
        }


    }
}
