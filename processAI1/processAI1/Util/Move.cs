using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace processAI1.Piece
{
    public class Move
    {
      private Point finalPosition { get; set; }
      private Point initialPosition { get; set; }

        public Move(Point _initialPosition, Point _finalPosition)
        {
            initialPosition = _initialPosition;
            finalPosition = _finalPosition;
           // IsCastlingMove = false;
        }

      


    }
}
