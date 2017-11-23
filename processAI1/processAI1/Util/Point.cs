using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1
{
    public class Point
    {
        int x;
        int y;

        public Point(int _x, int _y)
        {
            this.x = _x;
            this.y = _y;
        }

        public int getX()
        {
            return this.x;        
        }

        public int getY()
        {
            return this.y;
        }

        public Boolean validPosition()
        {
            return this.x >= 0 && this.x <= 7 && this.y >= 0 && this.y <= 7;
        }

    }
}
