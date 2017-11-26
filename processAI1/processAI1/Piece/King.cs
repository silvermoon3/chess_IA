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
           
            int left = (int)position.getX() - 1 > 0 ? (int)position.getX() - 1 : 0;
            int right = (int)position.getX() + 1 < 7 ? (int)position.getX() + 1 : 7;

            for (int i = (int)position.getX() - 1; i >= left; i--)
            {
                Point p = new Point(i, position.getY());
                if (!game.isOccupiedWithMyPiece(p, isWhite))
                    legalMoves.Add(new Move(position,p));
                break;
            }
            for (int i = (int)position.getX() + 1; i <= right; i++)
            {
                Point p = new Point(i, position.getY());
                if (!game.isOccupiedWithMyPiece(p, isWhite))
                    legalMoves.Add(new Move(position, p));
                break;
            }

            // Up and down
            int up = (int)position.getY() + 1 < 7 ? (int)position.getY() + 1 : 7;
            int down = (int)position.getY() - 1 > 0 ? (int)position.getY() - 1 : 0;
            for (int i = (int)position.getY() + 1; i <= up; i++)
            {

                Point p = new Point(position.getX(), i);
                if (game.isOccupied(p))
                {
                    if (!game.isOccupiedWithMyPiece(p, isWhite))
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
            for (int i = (int)position.getY() - 1; i >= down; i--)
            {

                Point p = new Point(position.getX(), i);
                if (game.isOccupied(p))
                {
                    if (!game.isOccupiedWithMyPiece(p, isWhite))
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
            Point rightUp = new Point(position.getX() + 1, position.getY() + 1);
            Point rightDown = new Point(position.getX() + 1, position.getY() - 1);
            Point leftUp = new Point(position.getX() - 1, position.getY() + 1);
            Point leftDown = new Point(position.getX() - 1, position.getY() - 1);

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
