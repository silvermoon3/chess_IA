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
        public Bishop(String pos, bool _isWhite = true): base(pos, _isWhite)
        {
           
        }
        public override List<Move> getPossibleMoves(Belief belief)
        {
            List<Move> legalMoves = new List<Move>();
            if (position.GetX() - 2 >= 0)
            {
                if (position.GetY() < 7)
                {
                    Point p = new Point(position.GetX() - 2, position.GetY() + 1);
                    if (p.ValidPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }
                if (position.GetY() > 0)
                {
                    Point p = new Point(position.GetX() - 2, position.GetY() - 1);
                    if (p.ValidPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }

            }
            if (position.GetX() < 6)
            {
                if (position.GetY() < 7)
                {
                    Point p = new Point(position.GetX() + 2, position.GetY() + 1);
                    if (p.ValidPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }

                if (position.GetY() > 0)
                {
                    Point p = new Point(position.GetX() + 2, position.GetY() - 1);
                    if (p.ValidPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }


            }
            if (position.GetY() - 2 >= 0)
            {
                if (position.GetX() < 7)
                {
                    Point p = new Point(position.GetX() + 1, position.GetY() - 2);
                    if (p.ValidPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }

                if (position.GetX() > 0)
                {
                    Point p = new Point(position.GetX() - 1, position.GetY() - 2);
                    if (p.ValidPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }

            }
            if (position.GetY() < 6)
            {
                if (position.GetX() < 7)
                {
                    Point p = new Point(position.GetX() + 1, position.GetY() + 2);
                    if (p.ValidPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }

                if (position.GetX() > 0)
                {
                    Point p = new Point(position.GetX() - 1, position.GetY() + 2);
                    if (p.ValidPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
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
