using processAI1.Board;
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

        public override List<Move> getPossibleMoves(ChessBoard game)
        {
            List<Move> legalMoves = new List<Move>();
            int left = (int)position.getX() - 8 > 0 ? (int)position.getX() - 8 : 0;
            int right = (int)position.getX() + 8 < 7 ? (int)position.getX() + 8 : 7;
            //vers la droite
            if(position.getX() < 7)
            {
                for (int i = position.getX() + 1; i <= 7; i++)
                {
                    Point p = new Point(i, position.getY());
                    if (!game.isOccupied(p))
                        legalMoves.Add(new Move(position, p));
                    else
                    {
                        if (!game.isOccupiedWithMyPiece(p, isWhite) && p.validPosition())
                            legalMoves.Add(new Move(position, p));
                        break;
                    }
                }
            }
            //vers la gauche 
            if (position.getX() > 0)
            {

                for (int i = position.getX() - 1; i >= 0; i--)
                {
                    Point p = new Point(i, position.getY());
                    if (!game.isOccupied(p))
                        legalMoves.Add(new Move(position, p));
                    else
                    {
                        if (!game.isOccupiedWithMyPiece(p, isWhite) && p.validPosition())
                            legalMoves.Add(new Move(position, p));
                        break;
                    }
                }
            }

            //En haut 
            if (position.getY() < 7)
                for (int i = position.getY() + 1; i <= 7; i++)
                {
                    Point p = new Point(position.getX(), i);
                    if (!game.isOccupied(p))
                        legalMoves.Add(new Move(position, p));
                    else
                    {
                        if (!game.isOccupiedWithMyPiece(p, isWhite) && p.validPosition())
                            legalMoves.Add(new Move(position, p));
                        break;
                    }
                }

            //En bas 
            if (position.getY() > 0)
                for (int i = position.getY() - 1; i >= 0; i--)
                {
                    Point p = new Point(position.getX(), i);
                    if (!game.isOccupied(p))
                        legalMoves.Add(new Move(position, p));
                    else
                    {
                        if (!game.isOccupiedWithMyPiece(p, isWhite) && p.validPosition())
                            legalMoves.Add(new Move(position, p));
                        break;
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
