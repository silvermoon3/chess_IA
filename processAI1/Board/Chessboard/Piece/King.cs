using System;
using System.Collections.Generic;

namespace processAI1.Board.Chessboard.Piece
{
    class King : Piece
    {
       
        public King(int x, int y, Boolean isWhite): base(x,y,isWhite)
        {

        }
        public King(String pos, bool isWhite = true): base(pos, isWhite)
        {

        }

        public override List<Move> GetPossibleMoves(ChessBoard game)
        {
            List<Move> legalMoves = new List<Move>();

            // Right and left
           
            int left = (int)Position.GetX() - 1 > 0 ? (int)Position.GetX() - 1 : 0;
            int right = (int)Position.GetX() + 1 < 7 ? (int)Position.GetX() + 1 : 7;

            for (int i = (int)Position.GetX() - 1; i >= left; i--)
            {
                Point p = new Point(i, Position.GetY());
                if (!game.IsOccupiedWithMyPiece(p, IsWhite))
                    legalMoves.Add(new Move(Position,p));
                break;
            }
            for (int i = (int)Position.GetX() + 1; i <= right; i++)
            {
                Point p = new Point(i, Position.GetY());
                if (!game.IsOccupiedWithMyPiece(p, IsWhite))
                    legalMoves.Add(new Move(Position, p));
                break;
            }

            // Up and down
          
            int up = (int)Position.GetY() + 1 < 7 ? (int)Position.GetY() + 1 : 7;
            int down = (int)Position.GetY() - 1 > 0 ? (int)Position.GetY() - 1 : 0;
            for (int i = (int)Position.GetY() + 1; i <= up; i++)
            {

                Point p = new Point(Position.GetX(), i);
                if (game.IsOccupied(p))
                {
                    if (!game.IsOccupiedWithMyPiece(p, IsWhite))
                    {
                        legalMoves.Add(new Move(Position, p));
                        break;
                    }
                    else
                        break;
                }
                else
                {
                    legalMoves.Add(new Move(Position, p));
                }

              

            }
            for (int i = (int)Position.GetY() - 1; i >= down; i--)
            {

                Point p = new Point(Position.GetX(), i);
                if (game.IsOccupied(p))
                {
                    if (!game.IsOccupiedWithMyPiece(p, IsWhite))
                    {
                        legalMoves.Add(new Move(Position, p));
                        break;
                    }
                    else
                        break;
                }
                else
                {
                    legalMoves.Add(new Move(Position, p));
                }

            }


            //diagonales
            Point rightUp = new Point(Position.GetX() + 1, Position.GetY() + 1);
            Point rightDown = new Point(Position.GetX() + 1, Position.GetY() - 1);
            Point leftUp = new Point(Position.GetX() - 1, Position.GetY() + 1);
            Point leftDown = new Point(Position.GetX() - 1, Position.GetY() - 1);

            if (rightUp.ValidPosition() && !game.IsOccupiedWithMyPiece(rightUp, IsWhite))
            {
                legalMoves.Add(new Move(Position,rightUp));
            }
            if (rightDown.ValidPosition() && !game.IsOccupiedWithMyPiece(rightDown, IsWhite))
            {
                legalMoves.Add(new Move(Position, rightDown));
            }
            if (leftUp.ValidPosition() && !game.IsOccupiedWithMyPiece(leftUp, IsWhite))
            {
                legalMoves.Add(new Move(Position, leftUp));
            }
            if (leftDown.ValidPosition() && !game.IsOccupiedWithMyPiece(leftDown, IsWhite))
            {
                legalMoves.Add(new Move(Position, leftDown));
            }
            GetRoque(game, ref legalMoves);

            return legalMoves;
        }

        private void GetRoque(ChessBoard belief, ref List<Move> legalMoves)
        {
            if (IsWhite)
            {
                //petit roque
                if (Belief.FirstMoveOfKing && !belief.IsOccupied(5,0) && !belief.IsOccupied(6, 0) && belief.GetCase(7,0).GetPiece() is Rook && Belief.FirstMoveOfRightRook)
                {
                    //legalMoves.Add(new Move("petit roque"));
                    legalMoves.Add(new Move(Position, new Point(6,0)));
                }
                //grand roque
                if (Belief.FirstMoveOfKing && !belief.IsOccupied(1, 0) && !belief.IsOccupied(2, 0) && !belief.IsOccupied(3, 0) && belief.GetCase(1, 0).GetPiece() is Rook && Belief.FirstMoveOfLeftRook)
                {
                    //legalMoves.Add(new Move("grand roque"));
                    legalMoves.Add(new Move(Position, new Point(2, 0)));
                }
            }
            else
            {
                //petit roque
                if (Belief.FirstMoveOfKing && !belief.IsOccupied(5, 7) && !belief.IsOccupied(6, 7) && belief.GetCase(7, 7).GetPiece() is Rook && Belief.FirstMoveOfRightRook)
                {
                    //legalMoves.Add(new Move("petit roque"));
                    legalMoves.Add(new Move(Position, new Point(6, 7)));
                }
                //grand roque
                if (Belief.FirstMoveOfKing && !belief.IsOccupied(1, 7) && !belief.IsOccupied(2, 7) && !belief.IsOccupied(3, 7) && belief.GetCase(1, 7).GetPiece() is Rook && Belief.FirstMoveOfLeftRook)
                {
                    //legalMoves.Add(new Move("grand roque"));
                    legalMoves.Add(new Move(Position, new Point(2, 7)));
                }

            }
           
        }
        public override String ToString()
        {
            return IsWhite ? "k" : "K";
        }

    }
}
