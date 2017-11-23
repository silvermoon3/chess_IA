using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Piece
{
    public class Cell
    {
        public Piece Piece;
        public Point Position;

        public Cell(Piece _piece, Point _position)
        {
            Piece = _piece;
            Position = _position;

        }

        public Piece getPiece()
        {
            return Piece;
        }
    }
}
