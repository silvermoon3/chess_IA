using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Piece
{
    class Queen : Piece
    {
        public Queen(int x, int y, Boolean isWhite) : base(x, y, isWhite)
        {

        }
        public Queen(String pos, bool _isWhite = true): base(pos, _isWhite)
        {

        }

        public override List<Move> getPossibleMoves(Belief belief)
        {
            Rook r = new Rook(position.getX(), position.getY(), isWhite);
            Bishop b = new Bishop(position.getX(), position.getY(), isWhite);
            return r.getPossibleMoves(belief).Union(b.getPossibleMoves(belief)).ToList();
           
        }
        public override String getPiece()
        {
            return isWhite ? "q" : "Q";
        }
    }
}
