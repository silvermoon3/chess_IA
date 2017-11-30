using System;
using System.Collections.Generic;
using System.Linq;

namespace processAI1.Board.Chessboard.Piece
{
    class Queen : Board.Chessboard.Piece.Piece
    {
        public Queen(int x, int y, Boolean isWhite) : base(x, y, isWhite)
        {

        }
        public Queen(String pos, bool isWhite = true): base(pos, isWhite)
        {

        }

        public override List<Move> GetPossibleMoves(ChessBoard game)
        {
            Rook r = new Rook(Position.GetX(), Position.GetY(), IsWhite);
            Bishop b = new Bishop(Position.GetX(), Position.GetY(), IsWhite);
            return r.GetPossibleMoves(game).Union(b.GetPossibleMoves(game)).ToList();
           
        }
        public override String ToString()
        {
            return IsWhite ? "q" : "Q";
        }
    }
}
