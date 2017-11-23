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
        public override List<Point> getPossibleMoves(Belief belief)
        {
            List<Point> legalMoves = new List<Point>();
            if (position.getX() - 2 >= 0)
            {
                if (position.getY() < 7)
                {
                    Point p = new Point(position.getX() - 2, position.getY() + 1);
                    if (p.validPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(p);
                }
                if (position.getY() > 0)
                {
                    Point p = new Point(position.getX() - 2, position.getY() - 1);
                    if (p.validPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(p);
                }
               
            }
            if (position.getX() < 6)
            {
                if (position.getY() < 7)
                {
                        Point p = new Point(position.getX() + 2, position.getY() + 1);
                        if (p.validPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                            legalMoves.Add(p);
                }
               
                if (position.getY() > 0)
                {
                    Point p = new Point(position.getX() + 2, position.getY() - 1);
                    if (p.validPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(p);
                }
                

            }
            if (position.getY() - 2 >= 0)
            {
                if (position.getX() < 7)
                {
                    Point p = new Point(position.getX() + 1, position.getY() - 2);
                    if (p.validPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(p);
                }
              
                if (position.getX() > 0)
                {
                    Point p = new Point(position.getX() - 1, position.getY() - 2);
                    if (p.validPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(p);
                }
               
            }
            if (position.getY() < 6)
            {
                if (position.getX() < 7)
                {
                    Point p = new Point(position.getX() + 1, position.getY() + 2);
                    if (p.validPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(p);
                }
              
                if (position.getX() > 0)
                {
                    Point p = new Point(position.getX() - 1, position.getY() + 2);
                    if (p.validPosition() && !belief.isOccupiedWithMyPiece(p, isWhite))
                        legalMoves.Add(p);
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
