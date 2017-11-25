using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Piece
{
    public class Cell
    {
        private Piece piece;
        private Point position;
        public bool isOccupied { get; set; }

        public Cell(Piece _piece, Point _position)
        {
            piece = _piece;
            position = _position;
            isOccupied = _piece != null;            

        }

        public Piece getPiece()
        {
            return piece;
        }
        public Point getPosition()
        {
            return position;
        }

        
    }
}
