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
        public Point position { get; set; }
        public bool isFirstMove { get; set; }
        
        public Piece(int x, int y, bool _isWhite = true)
        {
            position = new Point(x, y);
            isWhite = _isWhite;
            isFirstMove = true;
        }
      

        public Piece(String pos, bool _isWhite = true)
        {
            position = new Point(pos);
            isWhite = _isWhite;
            isFirstMove = true;
        }

        public virtual List<Move> getPossibleMoves(Belief belief)
        {
            return new List<Move>();
        }
                   
        public virtual String getPiece()
        {
            return "";
        }
       
        public void setPosition(Point _newPosition)
        {
            this.position = _newPosition;
        }
    }
}
