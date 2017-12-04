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
        private bool makeARoque;
        private String roque;
        public bool promotion;
        public bool pieceCapturee;

        public Move(Point _initialPosition, Point _finalPosition)
        {
            initialPosition = _initialPosition;
            finalPosition = _finalPosition;
            
            makeARoque = false;
        }

        public Move(String roque)
        {
            makeARoque = true;
            this.roque = roque;

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
        public Boolean IWantARoque()
        {
            return makeARoque;
        }

        public String getRoque()
        {
            return roque;

        }
      





    }
}
