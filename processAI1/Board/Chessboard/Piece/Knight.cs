using System;
using System.Collections.Generic;

namespace processAI1.Board.Chessboard.Piece
{
    class Knight:Piece
    {
        public Knight(int x, int y, Boolean isWhite): base(x,y,isWhite)
        {

        }
        public Knight(String pos, bool isWhite = true): base(pos, isWhite)
        {

        }
        public override List<Move> GetPossibleMoves(ChessBoard game)
        {
            List<Move> legalMoves = new List<Move>();
            if (Position.GetX() - 2 >= 0)
            {
                if (Position.GetY() < 7)
                {
                    Point p = new Point(Position.GetX() - 2, Position.GetY() + 1);
                    if (p.ValidPosition() && !game.IsOccupiedWithMyPiece(p, IsWhite))
                        legalMoves.Add(new Move(Position, p));
                }
                if (Position.GetY() > 0)
                {
                    Point p = new Point(Position.GetX() - 2, Position.GetY() - 1);
                    if (p.ValidPosition() && !game.IsOccupiedWithMyPiece(p, IsWhite))
                        legalMoves.Add(new Move(Position, p));
                }

            }
            if (Position.GetX() < 6)
            {
                if (Position.GetY() < 7)
                {
                    Point p = new Point(Position.GetX() + 2, Position.GetY() + 1);
                    if (p.ValidPosition() && !game.IsOccupiedWithMyPiece(p, IsWhite))
                        legalMoves.Add(new Move(Position, p));
                }

                if (Position.GetY() > 0)
                {
                    Point p = new Point(Position.GetX() + 2, Position.GetY() - 1);
                    if (p.ValidPosition() && !game.IsOccupiedWithMyPiece(p, IsWhite))
                        legalMoves.Add(new Move(Position, p));
                }


            }
            if (Position.GetY() - 2 >= 0)
            {
                if (Position.GetX() < 7)
                {
                    Point p = new Point(Position.GetX() + 1, Position.GetY() - 2);
                    if (p.ValidPosition() && !game.IsOccupiedWithMyPiece(p, IsWhite))
                        legalMoves.Add(new Move(Position, p));
                }

                if (Position.GetX() > 0)
                {
                    Point p = new Point(Position.GetX() - 1, Position.GetY() - 2);
                    if (p.ValidPosition() && !game.IsOccupiedWithMyPiece(p, IsWhite))
                        legalMoves.Add(new Move(Position, p));
                }

            }
            if (Position.GetY() < 6)
            {
                if (Position.GetX() < 7)
                {
                    Point p = new Point(Position.GetX() + 1, Position.GetY() + 2);
                    if (p.ValidPosition() && !game.IsOccupiedWithMyPiece(p, IsWhite))
                        legalMoves.Add(new Move(Position, p));
                }

                if (Position.GetX() > 0)
                {
                    Point p = new Point(Position.GetX() - 1, Position.GetY() + 2);
                    if (p.ValidPosition() && !game.IsOccupiedWithMyPiece(p, IsWhite))
                        legalMoves.Add(new Move(Position, p));
                }

            }
            return legalMoves;
        }
        public override String ToString()
        {
            return IsWhite ? "n" : "N";
        }
    }
}
