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

        public override List<Move> getPossibleMoves(Belief belief)
        {
            List<Move> legalMoves = new List<Move>();

            // Right and left
           
            int left = (int)position.GetX() - 1 > 0 ? (int)position.GetX() - 1 : 0;
            int right = (int)position.GetX() + 1 < 7 ? (int)position.GetX() + 1 : 7;

            for (int i = (int)position.GetX() - 1; i >= left; i--)
            {
                Point p = new Point(i, position.GetY());
                if (!belief.isOccupiedWithMyPiece(p, isWhite))
                    legalMoves.Add(new Move(position,p));
                break;
            }
            for (int i = (int)position.GetX() + 1; i <= right; i++)
            {
                Point p = new Point(i, position.GetY());
                if (!belief.isOccupiedWithMyPiece(p, isWhite))
                    legalMoves.Add(new Move(position, p));
                break;
            }

            // Up and down
            int up = (int)position.GetY() + 1 < 7 ? (int)position.GetY() + 1 : 7;
            int down = (int)position.GetY() - 1 > 0 ? (int)position.GetY() - 1 : 0;
            for (int i = (int)position.GetY() + 1; i <= up; i++)
            {

                Point p = new Point(position.GetX(), i);
                if (belief.isOccupied(p))
                {
                    if (!belief.isOccupiedWithMyPiece(p, isWhite))
                    {
                        legalMoves.Add(new Move(position, p));
                        break;
                    }
                    break;
                }
                else
                {
                    legalMoves.Add(new Move(position, p));
                }

              

            }
            for (int i = (int)position.GetY() - 1; i >= down; i--)
            {

                Point p = new Point(position.GetX(), i);
                if (belief.isOccupied(p))
                {
                    if (!belief.isOccupiedWithMyPiece(p, isWhite))
                    {
                        legalMoves.Add(new Move(position, p));
                        break;
                    }
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
            {
                legalMoves.Add(new Move(position, leftDown));
            }
            getRoque(belief, ref legalMoves);

            return legalMoves;
        }

        private void getRoque(Belief belief, ref List<Move> legalMoves)
        {
            if (isWhite)
            {
                //petit roque
                if (isFirstMove && !belief.isOccupied(5,0) && !belief.isOccupied(6, 0) && belief.getCase(7,0).getPiece() is Rook && belief.getCase(7, 0).getPiece().isFirstMove)
                {
                    legalMoves.Add(new Move(position, new Point(6,0)));
                }
                //grand roque
                if (isFirstMove && !belief.isOccupied(1, 0) && !belief.isOccupied(2, 0) && !belief.isOccupied(3, 0) && belief.getCase(1, 0).getPiece() is Rook && belief.getCase(1, 0).getPiece().isFirstMove)
                {
                    legalMoves.Add(new Move(position, new Point(2, 0)));
                }
            }
            else
            {
                //petit roque
                if (isFirstMove && !belief.isOccupied(5, 7) && !belief.isOccupied(6, 7) && belief.getCase(7, 7).getPiece() is Rook && belief.getCase(7, 7).getPiece().isFirstMove)
                {
                    legalMoves.Add(new Move(position, new Point(6, 7)));
                }
                //grand roque
                if (isFirstMove && !belief.isOccupied(1, 7) && !belief.isOccupied(2, 7) && !belief.isOccupied(3, 7) && belief.getCase(1, 7).getPiece() is Rook && belief.getCase(1, 7).getPiece().isFirstMove)
                {
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
