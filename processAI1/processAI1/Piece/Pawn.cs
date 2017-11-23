using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Piece
{
    class Pawn: Piece
    {
        Boolean moved;

        public Pawn(int x, int y, bool _isWhite = true):base(x,y,_isWhite){ }

        public override List<Point> getPossibleMoves(Belief belief)
        {
            List<Point> legalMoves = new List<Point>();
            int oneStep = isWhite ? 1 : -1;
            int twoStep = isWhite ? 2 : -2;
            if (!belief.isOccupied(position.getX(), position.getY() + oneStep))
            {
                legalMoves.Add(new Point(position.getX(), position.getY() + oneStep));
                if (isFirstMove && !belief.isOccupied(position.getX(), position.getY() + twoStep))
                    legalMoves.Add(new Point(position.getX(), position.getY() + twoStep));
            }

            getPawnAttack(ref legalMoves, belief);
            return legalMoves;
        }

        private void getPawnAttack(ref List<Point> legalMoves, Belief belief)
        {
            int offset = isWhite ? 1 : 1;
            Point right = new Point(position.getX() + 1, position.getY() + offset);
            Point left = new Point(position.getX() - 1, position.getY() + offset);
            
                if (right.validPosition())
                {
                    if (belief.isOccupied(right))
                    {
                        if (!belief.isOccupiedWithMyPiece(right, isWhite))
                        {
                            legalMoves.Add(right);
                        }
                    }
                }
            
           
                if (left.validPosition())
                {
                    if (belief.isOccupied(left))
                    {
                         if (!belief.isOccupiedWithMyPiece(left, isWhite))
                        {
                        legalMoves.Add(left);
                         }
                    }
                }
            
        }
        public override String getPiece()
        {
            return isWhite ? "p" : "P";
        }

    }
}
