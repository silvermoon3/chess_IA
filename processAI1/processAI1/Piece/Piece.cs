using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Piece
{
    public abstract class Piece
    {
        public bool isWhite { get; set; }
        protected Point position { get; set; }
        protected bool isFirstMove { get; set; }
        
        public Piece(int x, int y, bool _isWhite = true)
        {
            position = new Point(x, y);
            isWhite = _isWhite;
            isFirstMove = true;
        }

        public virtual List<Point> getPossibleMoves(Belief belief)
        {
            return new List<Point>();
        }
                   
        public virtual String getPiece()
        {
            return "";
        }
       
    }
}
