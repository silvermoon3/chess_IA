using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace processAI1.Board
{
    public class Move
    {
        private Point _initialPosition;
        private Point _finalPosition;
        private bool _makeARoque;
        private String _roque;

        public Move(Point initialPosition, Point finalPosition)
        {
            this._initialPosition = initialPosition;
            this._finalPosition = finalPosition;
            _makeARoque = false;
        }

        public Move(String roque)
        {
            _makeARoque = true;
            this._roque = roque;

        }
        public Move()
        {
            _initialPosition = new Point();
            _finalPosition = new Point();
        }
        public Point GetInitialPosition()
        {
            return _initialPosition;
        }
        public Point GetFinalPosition()
        {
            return _finalPosition;
        }
        public Boolean WantARoque()
        {
            return _makeARoque;
        }

        public String GetRoque()
        {
            return _roque;

        }

        public override string ToString()
        {
            return "" + _initialPosition + _finalPosition;
        }




    }
}
