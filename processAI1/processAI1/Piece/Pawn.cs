using processAI1.Board;
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
        public Boolean isFirstMove()
        {
            if (isWhite)
                return position.getY() == 1;
            else
                return position.getY() == 6;
        }
        public override List<Move> getPossibleMoves(ChessBoard game)
        {
            List<Move> legalMoves = new List<Move>();
            int oneStep = isWhite ? 1 : -1;
            int twoStep = isWhite ? 2 : -2;
            Point p = new Point(position.getX(), position.getY() + oneStep);
            if (!game.isOccupied(p) && p.validPosition())
            {
                if (p.getY() == 7 || p.getY() == 0)
                {
                    //Promotion
                }
                else
                {
                    legalMoves.Add(new Move(position, p));
                }
                if (isFirstMove() && !game.isOccupied(position.getX(), position.getY() + twoStep))
                {                   
                    legalMoves.Add(new Move(position, new Point(position.getX(), position.getY() + twoStep)));

                }
                   
            }

            getPawnAttack(ref legalMoves, game);
            return legalMoves;
        }

        private void getPawnAttack(ref List<Move> legalMoves, ChessBoard belief)
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
                            legalMoves.Add(new Move(position, right));
                        }
                    }
                }
            
           
                if (left.validPosition())
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
