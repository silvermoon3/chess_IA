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
        private Point initialPosition;
        private Point finalPosition;

      
        public Move(Point _initialPosition, Point _finalPosition)
        {
            initialPosition = _initialPosition;
            finalPosition = _finalPosition;           
        }
        public Move()
        {
            initialPosition = new Point();
            finalPosition = new Point();
        }
        public Point getInitialPosition()
        {
            return initialPosition;
        }
        public Point getFinalPosition()
        {
            return finalPosition;
        }

       



    }
}
