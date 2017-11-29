using processAI1.Board;
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

        public override List<Move> getPossibleMoves(ChessBoard game)
        {
<<<<<<< HEAD:processAI1/Piece/Queen.cs
            Rook r = new Rook(position.GetX(), position.GetY(), isWhite);
            Bishop b = new Bishop(position.GetX(), position.GetY(), isWhite);
            return r.getPossibleMoves(belief).Union(b.getPossibleMoves(belief)).ToList();
=======
            Rook r = new Rook(position.getX(), position.getY(), isWhite);
            Bishop b = new Bishop(position.getX(), position.getY(), isWhite);
            return r.getPossibleMoves(game).Union(b.getPossibleMoves(game)).ToList();
>>>>>>> MiniMax:processAI1/Piece/Queen.cs
           
        }
        public override String getPiece()
        {
            return isWhite ? "q" : "Q";
        }
    }
}
