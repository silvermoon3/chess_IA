using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1
{
    class Position
    {
        int x;
        int y;

        public Position(int _x, int _y)
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

    }
}
