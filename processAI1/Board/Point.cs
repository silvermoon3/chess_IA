using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1
{
    public class Point
    {
        int _x;
        int _y;

        public Point(int x, int y)
        {
            this._x = x;
            this._y = y;
        }
        public Point(String pos)
        {
            this._x = GetPointFromString(pos).GetX();
            this._y = GetPointFromString(pos).GetY();
        }
        public Point()
        {
            this._x = 0;
            this._y = 0;
        }

        public int GetX()
        {
            return this._x;        
        }

        public int GetY()
        {
            return this._y;
        }

        public Boolean ValidPosition()
        {
            return this._x >= 0 && this._x <= 7 && this._y >= 0 && this._y <= 7;
        }

        override
        public string ToString()
        {
            String posX = "";
            int newY = _y + 1;
           
            switch (GetX())
            {
                case 0:
                    posX = "a";break;
                case 1:
                    posX = "b";break;
                case 2:
                    posX = "c";break;
                case 3:
                    posX = "d";break;
                case 4:
                    posX = "e"; break;
                case 5:
                    posX = "f"; break;
                case 6:
                    posX = "g"; break;
                case 7:
                    posX = "h"; break;

            }
            return posX + newY;
        }

        public Point GetPointFromString(String pos)
        {
            int x = 0;
            int y = Int32.Parse(pos.Substring(1));

            switch (pos[0])
            {
                case 'a':
                    x = 0;
                    break;
                case 'b':
                    x = 1;
                    break;
                case 'c':
                    x = 2;
                    break;
                case 'd':
                    x = 3;
                    break;
                case 'e':
                    x = 4;
                    break;
                case 'f':
                    x = 5;
                    break;
                case 'g':
                    x = 6;
                    break;
                case 'h':
                    x = 7;
                    break;
            }

            return new Point(x,y-1);             

        }
    }
}
