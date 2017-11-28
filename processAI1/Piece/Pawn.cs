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
        public Pawn(String pos, bool _isWhite = true): base(pos, _isWhite)
        {

        }
        public override List<Move> getPossibleMoves(Belief belief)
        {
            List<Move> legalMoves = new List<Move>();
            int oneStep = isWhite ? 1 : -1;
            int twoStep = isWhite ? 2 : -2;
            Point p = new Point(position.GetX(), position.GetY() + oneStep);
            if (!belief.isOccupied(p))
            {
                legalMoves.Add(new Move(position, p));
                if (isFirstMove && !belief.isOccupied(position.GetX(), position.GetY() + twoStep))
                {                   
                    legalMoves.Add(new Move(position, new Point(position.GetX(), position.GetY() + twoStep)));

                }
                   
            }

            getPawnAttack(ref legalMoves, belief);
            return legalMoves;
        }

        private void getPawnAttack(ref List<Move> legalMoves, Belief belief)
        {
            int offset = isWhite ? 1 : 1;
            Point right = new Point(position.GetX() + 1, position.GetY() + offset);
            Point left = new Point(position.GetX() - 1, position.GetY() + offset);
            
                if (right.ValidPosition())
                {
                    if (belief.isOccupied(right))
                    {
                        if (!belief.isOccupiedWithMyPiece(right, isWhite))
                        {
                            legalMoves.Add(new Move(position, right));
                        }
                    }
                }
            
           
                if (left.ValidPosition())
                {
                    if (belief.isOccupied(left))
                    {
                         if (!belief.isOccupiedWithMyPiece(left, isWhite))
                        {
                        legalMoves.Add(new Move(position, left));
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
