using processAI1.Board;
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
        public override List<Move> getPossibleMoves(ChessBoard game)
        {
            List<Move> legalMoves = new List<Move>();
            if (position.getX() - 2 >= 0)
            {
                if (position.getY() < 7)
                {
                    Point p = new Point(position.getX() - 2, position.getY() + 1);
                    if (p.validPosition() && !game.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }
                if (position.getY() > 0)
                {
                    Point p = new Point(position.getX() - 2, position.getY() - 1);
                    if (p.validPosition() && !game.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }

            }
            if (position.getX() < 6)
            {
                if (position.getY() < 7)
                {
                    Point p = new Point(position.getX() + 2, position.getY() + 1);
                    if (p.validPosition() && !game.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }

                if (position.getY() > 0)
                {
                    Point p = new Point(position.getX() + 2, position.getY() - 1);
                    if (p.validPosition() && !game.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }


            }
            if (position.getY() - 2 >= 0)
            {
                if (position.getX() < 7)
                {
                    Point p = new Point(position.getX() + 1, position.getY() - 2);
                    if (p.validPosition() && !game.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }

                if (position.getX() > 0)
                {
                    Point p = new Point(position.getX() - 1, position.getY() - 2);
                    if (p.validPosition() && !game.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }

            }
            if (position.getY() < 6)
            {
                if (position.getX() < 7)
                {
                    Point p = new Point(position.getX() + 1, position.getY() + 2);
                    if (p.validPosition() && !game.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(new Move(position, p));
                }

                if (position.getX() > 0)
                {
                    Point p = new Point(position.getX() - 1, position.getY() + 2);
                    if (p.validPosition() && !game.isOccupiedWithMyPiece(p, isWhite))
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
