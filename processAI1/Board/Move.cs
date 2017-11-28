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
        private Point InitialPosition { get; set; }
        private Point FinalPosition { get; set; }

       

        public Move(Point initialPosition, Point finalPosition)
        {
            InitialPosition = initialPosition;
            FinalPosition = finalPosition;           
        }
        public Move()
        {
            InitialPosition = new Point();
            FinalPosition = new Point();
        }
        public Point GetInitialPosition()
        {
            return InitialPosition;
        }
        public Point GetFinalPosition()
        {
            return FinalPosition;
        }




    }
}
