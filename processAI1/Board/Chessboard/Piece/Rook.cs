using System;
using System.Collections.Generic;

namespace processAI1.Board.Chessboard.Piece
{
    class Rook : Board.Chessboard.Piece.Piece
    {
        public Rook(int x, int y, Boolean isWhite) : base(x, y, isWhite)
        {

        }
        public Rook(String pos, bool isWhite = true): base(pos, isWhite)
        {

        }

        public override List<Move> GetPossibleMoves(ChessBoard game)
        {
            List<Move> legalMoves = new List<Move>();
            int left = (int)Position.GetX() - 8 > 0 ? (int)Position.GetX() - 8 : 0;
            int right = (int)Position.GetX() + 8 < 7 ? (int)Position.GetX() + 8 : 7;
            //vers la droite
            if(Position.GetX() < 7)
            {
                for (int i = Position.GetX() + 1; i <= 7; i++)
                {
                    Point p = new Point(i, Position.GetY());
                    if (!game.IsOccupied(p) && p.ValidPosition())
                        legalMoves.Add(new Move(Position, p));
                    else
                    {
                        if (!game.IsOccupiedWithMyPiece(p, IsWhite) && p.ValidPosition())
                            legalMoves.Add(new Move(Position, p));
                        break;
                    }
                }
            }
            //vers la gauche 
            if (Position.GetX() > 0)
            {

                for (int i = Position.GetX() - 1; i >= 0; i--)
                {
                    Point p = new Point(i, Position.GetY());
                    if (!game.IsOccupied(p) && p.ValidPosition())
                        legalMoves.Add(new Move(Position, p));
                    else
                    {
                        if (!game.IsOccupiedWithMyPiece(p, IsWhite) && p.ValidPosition())
                            legalMoves.Add(new Move(Position, p));
                        break;
                    }
                }
            }

            //En haut 
            if (Position.GetY() < 7)
                for (int i = Position.GetY() + 1; i <= 7; i++)
                {
                    Point p = new Point(Position.GetX(), i);
                    if (!game.IsOccupied(p) && p.ValidPosition())
                        legalMoves.Add(new Move(Position, p));
                    else
                    {
                        if (!game.IsOccupiedWithMyPiece(p, IsWhite) && p.ValidPosition())
                            legalMoves.Add(new Move(Position, p));
                        break;
                    }
                }

            //En bas 
            if (Position.GetY() > 0)
                for (int i = Position.GetY() - 1; i >= 0; i--)
                {
                    Point p = new Point(Position.GetX(), i);
                    if (!game.IsOccupied(p) && p.ValidPosition())
                        legalMoves.Add(new Move(Position, p));
                    else
                    {
                        if (!game.IsOccupiedWithMyPiece(p, IsWhite) && p.ValidPosition())
                            legalMoves.Add(new Move(Position, p));
                        break;
                    }
                }
          
                
           
            return legalMoves;
        }

        public override String ToString()
        {
            return IsWhite ? "r" : "R";
        }

    }
}
