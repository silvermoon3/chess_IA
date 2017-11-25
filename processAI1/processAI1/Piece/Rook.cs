using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Piece
{
    class Rook : Piece
    {
        public Rook(int x, int y, Boolean isWhite) : base(x, y, isWhite)
        {

        }
        public Rook(String pos, bool _isWhite = true): base(pos, _isWhite)
        {

        }

        public override List<Move> getPossibleMoves(Belief belief)
        {
            List<Move> legalMoves = new List<Move>();
            int left = (int)position.getX() - 8 > 0 ? (int)position.getX() - 8 : 0;
            int right = (int)position.getX() + 8 < 7 ? (int)position.getX() + 8 : 7;
                        
            for (int i = (int)position.getX() - 1; i >= left; i--)
            {
                Point p = new Point(i, position.getY());
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
            for (int i = (int)position.getX() + 1; i <= right; i++)
            {
                Point p = new Point(i, position.getY());
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

            // Up and down
            int up = (int)position.getY() + 8 < 7 ? (int)position.getY() + 8 : 7;
            int down = (int)position.getY() - 8 > 0 ? (int)position.getY() - 8 : 0;
            for (int i = (int)position.getY() + 1; i <= up; i++)
            {
                
                Point p = new Point(position.getX(), i);
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
            for (int i = (int)position.getY() - 1; i >= down; i--)
            {
                
                Point p = new Point(position.getX(), i);
                if (belief.isOccupied(p))
                {
                    if(!belief.isOccupiedWithMyPiece(p, isWhite))
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
            return legalMoves;
        }

        public override String getPiece()
        {
            return isWhite ? "r" : "R";
        }

    }
}
