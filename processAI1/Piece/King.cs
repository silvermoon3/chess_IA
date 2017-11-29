using processAI1.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Piece
{
    class King : Piece
    {
       
        public King(int x, int y, Boolean isWhite): base(x,y,isWhite)
        {

        }
        public King(String pos, bool _isWhite = true): base(pos, _isWhite)
        {

        }

        public override List<Move> getPossibleMoves(ChessBoard game)
        {
            List<Move> legalMoves = new List<Move>();

            // Right and left
           
            int left = (int)position.GetX() - 1 > 0 ? (int)position.GetX() - 1 : 0;
            int right = (int)position.GetX() + 1 < 7 ? (int)position.GetX() + 1 : 7;

            for (int i = (int)position.GetX() - 1; i >= left; i--)
            {
<<<<<<< HEAD:processAI1/Piece/King.cs
                Point p = new Point(i, position.GetY());
                if (!belief.isOccupiedWithMyPiece(p, isWhite))
=======
                Point p = new Point(i, position.getY());
                if (!game.isOccupiedWithMyPiece(p, isWhite))
>>>>>>> MiniMax:processAI1/Piece/King.cs
                    legalMoves.Add(new Move(position,p));
                break;
            }
            for (int i = (int)position.GetX() + 1; i <= right; i++)
            {
<<<<<<< HEAD:processAI1/Piece/King.cs
                Point p = new Point(i, position.GetY());
                if (!belief.isOccupiedWithMyPiece(p, isWhite))
=======
                Point p = new Point(i, position.getY());
                if (!game.isOccupiedWithMyPiece(p, isWhite))
>>>>>>> MiniMax:processAI1/Piece/King.cs
                    legalMoves.Add(new Move(position, p));
                break;
            }

            // Up and down
<<<<<<< HEAD:processAI1/Piece/King.cs
            int up = (int)position.GetY() + 1 < 7 ? (int)position.GetY() + 1 : 7;
            int down = (int)position.GetY() - 1 > 0 ? (int)position.GetY() - 1 : 0;
            for (int i = (int)position.GetY() + 1; i <= up; i++)
            {

                Point p = new Point(position.GetX(), i);
                if (belief.isOccupied(p))
=======
          
            int up = (int)position.getY() + 1 < 7 ? (int)position.getY() + 1 : 7;
            int down = (int)position.getY() - 1 > 0 ? (int)position.getY() - 1 : 0;
            for (int i = (int)position.getY() + 1; i <= up; i++)
            {

                Point p = new Point(position.getX(), i);
                if (game.isOccupied(p))
>>>>>>> MiniMax:processAI1/Piece/King.cs
                {
                    if (!game.isOccupiedWithMyPiece(p, isWhite))
                    {
                        legalMoves.Add(new Move(position, p));
                        break;
                    }
                    else
                        break;
                }
                else
                {
                    legalMoves.Add(new Move(position, p));
                }

              

            }
            for (int i = (int)position.GetY() - 1; i >= down; i--)
            {

<<<<<<< HEAD:processAI1/Piece/King.cs
                Point p = new Point(position.GetX(), i);
                if (belief.isOccupied(p))
=======
                Point p = new Point(position.getX(), i);
                if (game.isOccupied(p))
>>>>>>> MiniMax:processAI1/Piece/King.cs
                {
                    if (!game.isOccupiedWithMyPiece(p, isWhite))
                    {
                        legalMoves.Add(new Move(position, p));
                        break;
                    }
                    else
                        break;
                }
                else
                {
                    legalMoves.Add(new Move(position, p));
                }

            }


            //diagonales
            Point rightUp = new Point(position.GetX() + 1, position.GetY() + 1);
            Point rightDown = new Point(position.GetX() + 1, position.GetY() - 1);
            Point leftUp = new Point(position.GetX() - 1, position.GetY() + 1);
            Point leftDown = new Point(position.GetX() - 1, position.GetY() - 1);

<<<<<<< HEAD:processAI1/Piece/King.cs
            if (rightUp.ValidPosition() && !belief.isOccupiedWithMyPiece(rightUp, isWhite))
            {
                legalMoves.Add(new Move(position,rightUp));
            }
            if (rightDown.ValidPosition() && !belief.isOccupiedWithMyPiece(rightDown, isWhite))
            {
                legalMoves.Add(new Move(position, rightDown));
            }
            if (leftUp.ValidPosition() && !belief.isOccupiedWithMyPiece(leftUp, isWhite))
            {
                legalMoves.Add(new Move(position, leftUp));
            }
            if (leftDown.ValidPosition() && !belief.isOccupiedWithMyPiece(leftDown, isWhite))
=======
            if (rightUp.validPosition() && !game.isOccupiedWithMyPiece(rightUp, isWhite))
            {
                legalMoves.Add(new Move(position,rightUp));
            }
            if (rightDown.validPosition() && !game.isOccupiedWithMyPiece(rightDown, isWhite))
            {
                legalMoves.Add(new Move(position, rightDown));
            }
            if (leftUp.validPosition() && !game.isOccupiedWithMyPiece(leftUp, isWhite))
            {
                legalMoves.Add(new Move(position, leftUp));
            }
            if (leftDown.validPosition() && !game.isOccupiedWithMyPiece(leftDown, isWhite))
>>>>>>> MiniMax:processAI1/Piece/King.cs
            {
                legalMoves.Add(new Move(position, leftDown));
            }
            getRoque(game, ref legalMoves);

            return legalMoves;
        }

        private void getRoque(ChessBoard belief, ref List<Move> legalMoves)
        {
            if (isWhite)
            {
                //petit roque
                if (Belief.firstMoveOfKing && !belief.isOccupied(5,0) && !belief.isOccupied(6, 0) && belief.getCase(7,0).getPiece() is Rook && Belief.firstMoveOfRightRook)
                {
                    //legalMoves.Add(new Move("petit roque"));
                    legalMoves.Add(new Move(position, new Point(6,0)));
                }
                //grand roque
                if (Belief.firstMoveOfKing && !belief.isOccupied(1, 0) && !belief.isOccupied(2, 0) && !belief.isOccupied(3, 0) && belief.getCase(1, 0).getPiece() is Rook && Belief.firstMoveOfLeftRook)
                {
                    //legalMoves.Add(new Move("grand roque"));
                    legalMoves.Add(new Move(position, new Point(2, 0)));
                }
            }
            else
            {
                //petit roque
                if (Belief.firstMoveOfKing && !belief.isOccupied(5, 7) && !belief.isOccupied(6, 7) && belief.getCase(7, 7).getPiece() is Rook && Belief.firstMoveOfRightRook)
                {
                    //legalMoves.Add(new Move("petit roque"));
                    legalMoves.Add(new Move(position, new Point(6, 7)));
                }
                //grand roque
                if (Belief.firstMoveOfKing && !belief.isOccupied(1, 7) && !belief.isOccupied(2, 7) && !belief.isOccupied(3, 7) && belief.getCase(1, 7).getPiece() is Rook && Belief.firstMoveOfLeftRook)
                {
                    //legalMoves.Add(new Move("grand roque"));
                    legalMoves.Add(new Move(position, new Point(2, 7)));
                }

            }
           
        }
        public override String getPiece()
        {
            return isWhite ? "k" : "K";
        }

    }
}
