using System;
using System.Collections.Generic;

namespace processAI1.Board.Chessboard.Piece
{
    class Pawn: Piece
    {
        Boolean _moved;

        public Pawn(int x, int y, bool isWhite = true):base(x,y,isWhite){ }
        public Pawn(String pos, bool isWhite = true): base(pos, isWhite)
        {

        }
        public Boolean IsFirstMove()
        {
            if (IsWhite)
                return Position.GetY() == 1;
            else
                return Position.GetY() == 6;
        }
        public override List<Move> GetPossibleMoves(ChessBoard game)
        {
            List<Move> legalMoves = new List<Move>();
            int oneStep = IsWhite ? 1 : -1;
            int twoStep = IsWhite ? 2 : -2;
            Point p = new Point(Position.GetX(), Position.GetY() + oneStep);
            if (!game.IsOccupied(p) && p.ValidPosition())
            {
                if (p.GetY() == 7 || p.GetY() == 0)
                {
                    //Promotion
                }
                else
                {
                    legalMoves.Add(new Move(Position, p));
                }
                if (IsFirstMove() && !game.IsOccupied(Position.GetX(), Position.GetY() + twoStep))
                {                   
                    legalMoves.Add(new Move(Position, new Point(Position.GetX(), Position.GetY() + twoStep)));

                }
                   
            }

            GetPawnAttack(ref legalMoves, game);
            return legalMoves;
        }

        private void GetPawnAttack(ref List<Move> legalMoves, ChessBoard belief)
        {
            int offset = IsWhite ? 1 : 1;
            Point right = new Point(Position.GetX() + 1, Position.GetY() + offset);
            Point left = new Point(Position.GetX() - 1, Position.GetY() + offset);
            
                if (right.ValidPosition())
                {
                    if (belief.IsOccupied(right))
                    {
                        if (!belief.IsOccupiedWithMyPiece(right, IsWhite))
                        {
                            legalMoves.Add(new Move(Position, right));
                        }
                    }
                }
            
           
                if (left.ValidPosition())
                {
                    if (belief.IsOccupied(left))
                    {
                         if (!belief.IsOccupiedWithMyPiece(left, IsWhite))
                        {
                        legalMoves.Add(new Move(Position, left));
                         }
                    }
                }
            
        }


        public override String ToString()
        {
            return IsWhite ? "p" : "P";
        }

    }
}
