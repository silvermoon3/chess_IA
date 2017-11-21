using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1
{
    class Queen : Piece
    {
        override
        public List<Position> getPossibleMoves(Position currentPosition, Square[,] board)
        {
            return allLegalMoves(currentPosition, board);
        }

        public List<Position> allLegalMoves(Position currentPosition, Square[,] board)
        {
            List<Position> legalMoves = new List<Position>();
            Rook r = new Rook();
            legalMoves.AddRange(r.getPossibleMoves(currentPosition, board));
            Bishop b = new Bishop();
            legalMoves.AddRange(b.getPossibleMoves(currentPosition, board));
            
            return legalMoves;
        }

        override
        public Boolean ocuppiedOrOnPath(Position currentPosition, Position desination, Square[,] board)
        {
            return false;
        }
    }
}
