using System;
using System.Collections.Generic;

namespace processAI1.Board.Chessboard.Piece
{
    class Bishop : Piece
    {              
        public Bishop(int x, int y, Boolean isWhite) : base(x, y, isWhite)
        {

        }
        public Bishop(String pos, bool isWhite = true): base(pos, isWhite)
        {
           
        }
        public override List<Move> GetPossibleMoves(ChessBoard game)
        {
            List<Move> legalMoves = new List<Move>();
            for (int i = (int)Position.GetY() - 1, step = 1; step + Position.GetX() <= 7 && step <= 8 && i >= 0; step++, i--)
            {
                Point p = new Point(Position.GetX() + step, i);
                if (p.ValidPosition())
                {
                    if (game.IsOccupied(p))
                    {
                        if (!game.IsOccupiedWithMyPiece(p, IsWhite))
                        {
                            legalMoves.Add(new Move(Position, p));
                            break;
                        }
                        break;
                    }
                    else
                    {
                        legalMoves.Add(new Move(Position, p));

                    }
                }


            }
            for (int i = (int)Position.GetY() - 1, step = 1; Position.GetX() - step >= 0 && step <= 8 && i >= 0; step++, i--)
            {
                Point p = new Point(Position.GetX() - step, i);
                if (p.ValidPosition())
                {
                    if (game.IsOccupied(p))
                    {
                        if (!game.IsOccupiedWithMyPiece(p, IsWhite))
                        {
                            legalMoves.Add(new Move(Position, p));
                            break;
                        }
                        break;
                    }
                    else
                    {
                        legalMoves.Add(new Move(Position, p));
                    }
                }


            }
            for (int i = (int)Position.GetY() + 1, step = 1; step + Position.GetX() <= 7 && step <= 8 && i < 8; step++, i++)
            {
                Point p = new Point(Position.GetX() + step, i);
                if (p.ValidPosition())
                {
                    if (game.IsOccupied(p))
                    {
                        if (!game.IsOccupiedWithMyPiece(p, IsWhite))
                        {
                            legalMoves.Add(new Move(Position, p));
                            break;
                        }
                        break;
                    }
                    else
                    {
                        legalMoves.Add(new Move(Position, p));
                    }
                }



            }
            for (int i = (int)Position.GetY() + 1, step = 1; Position.GetX() - step >= 0 && step <= 8 && i < 8; step++, i++)
            {
                Point p = new Point(Position.GetX() - step, i);
                if (p.ValidPosition())
                {
                    if (game.IsOccupied(p))
                    {
                        if (!game.IsOccupiedWithMyPiece(p, IsWhite))
                        {
                            legalMoves.Add(new Move(Position, p));
                            break;
                        }
                        break;
                    }
                    else
                    {
                        legalMoves.Add(new Move(Position, p));
                    }
                }

            }

            return legalMoves;

        }

        public override String ToString()
        {
            return IsWhite ? "b" : "B";
        }
    }
}
