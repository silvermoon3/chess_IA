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

        public override List<Point> getPossibleMoves(Belief belief)
        {
            List<Point> legalMoves = new List<Point>();

            // Right and left
           
            int left = (int)position.getX() - 1 > 0 ? (int)position.getX() - 1 : 0;
            int right = (int)position.getX() + 1 < 7 ? (int)position.getX() + 1 : 7;

            for (int i = (int)position.getX() - 1; i >= left; i--)
            {
                Point p = new Point(i, position.getY());
                if (!belief.isOccupiedWithMyPiece(p, isWhite))
                    legalMoves.Add(p);
                break;
            }
            for (int i = (int)position.getX() + 1; i <= right; i++)
            {
                Point p = new Point(i, position.getY());
                if (!belief.isOccupiedWithMyPiece(p, isWhite))
                    legalMoves.Add(p);
                break;
            }

            // Up and down
            int up = (int)position.getY() + 1 < 7 ? (int)position.getY() + 1 : 7;
            int down = (int)position.getY() - 1 > 0 ? (int)position.getY() - 1 : 0;
            for (int i = (int)position.getY() + 1; i <= up; i++)
            {

                Point p = new Point(position.getX(), i);
                if (belief.isOccupied(p))
                {
                    if (!belief.isOccupiedWithMyPiece(p, isWhite))
                    {
                        legalMoves.Add(p);
                        break;
                    }
                    break;
                }
                else
                {
                    legalMoves.Add(p);
                }

              

            }
            for (int i = (int)position.getY() - 1; i >= down; i--)
            {

                Point p = new Point(position.getX(), i);
                if (belief.isOccupied(p))
                {
                    if (!belief.isOccupiedWithMyPiece(p, isWhite))
                    {
                        legalMoves.Add(p);
                        break;
                    }
                    break;
                }
                else
                {
                    legalMoves.Add(p);
                }

            }


            //diagonales
            Point rightUp = new Point(position.getX() + 1, position.getY() + 1);
            Point rightDown = new Point(position.getX() + 1, position.getY() - 1);
            Point leftUp = new Point(position.getX() - 1, position.getY() + 1);
            Point leftDown = new Point(position.getX() - 1, position.getY() - 1);

            if (rightUp.validPosition() && !belief.isOccupiedWithMyPiece(rightUp, isWhite))
            {
                legalMoves.Add(rightUp);
            }
            if (rightDown.validPosition() && !belief.isOccupiedWithMyPiece(rightDown, isWhite))
            {
                legalMoves.Add(rightDown);
            }
            if (leftUp.validPosition() && !belief.isOccupiedWithMyPiece(leftUp, isWhite))
            {
                legalMoves.Add(leftUp);
            }
            if (leftDown.validPosition() && !belief.isOccupiedWithMyPiece(leftDown, isWhite))
            {
                legalMoves.Add(leftDown);
            }

            return legalMoves;
        }
        public override String getPiece()
        {
            return isWhite ? "k" : "K";
        }

    }
}
