using processAI1.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Piece
{
    class Knight:Piece
    {
        public Knight(int x, int y, Boolean isWhite): base(x,y,isWhite)
        {

        }
        public Knight(String pos, bool _isWhite = true): base(pos, _isWhite)
        {

        }
        public override List<Move> getPossibleMoves(ChessBoard game)
        {
            List<Move> legalMoves = new List<Move>();
            for (int i = (int)position.getY() - 1, step = 1; step + position.getX() <= 7 && step <= 8 && i >= 0; step++, i--)
            {
                Point p = new Point(position.getX() + step, i);
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
            for (int i = (int)position.getY() - 1, step = 1; position.getX() - step >= 0 && step <= 8 && i >= 0; step++, i--)
            {
                Point p = new Point(position.getX() - step, i);
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
            for (int i = (int)position.getY() + 1, step = 1; step + position.getX() <= 7 && step <= 8 && i < 8; step++, i++)
            {
                Point p = new Point(position.getX() + step, i);
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
            for (int i = (int)position.getY() + 1, step = 1; position.getX() - step >= 0 && step <= 8 && i < 8; step++, i++)
            {
                Point p = new Point(position.getX() - step, i);
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

            return legalMoves;
        }
        public override String getPiece()
        {
            return isWhite ? "n" : "N";
        }
    }
}
