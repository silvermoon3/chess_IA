using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Piece
{
    class Bishop : Piece
    {              
        public Bishop(int x, int y, Boolean isWhite) : base(x, y, isWhite)
        {

        }
        public override List<Point> getPossibleMoves(Belief belief)
        {
            List<Point> legalMoves = new List<Point>();
            for (int i = (int)position.getY() - 1, step = 1; step + position.getX() <= 7 && step <= 8 && i >= 0; step++, i--)
            {
                Point p = new Point(position.getX() + step, i);
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
            for (int i = (int)position.getY() - 1, step = 1; position.getX() - step >= 0 && step <= 8 && i >= 0; step++, i--)
            {
                Point p = new Point(position.getX() - step, i);
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
            for (int i = (int)position.getY() + 1, step = 1; step + position.getX() <= 7 && step <= 8 && i < 8; step++, i++)
            {
                Point p = new Point(position.getX() + step, i);
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
            for (int i = (int)position.getY() + 1, step = 1; position.getX() - step >= 0 && step <= 8 && i < 8; step++, i++)
            {
                Point p = new Point(position.getX() - step, i);
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

            return legalMoves;
        }

        public override String getPiece()
        {
            return isWhite ? "b" : "B";
        }
    }
}
