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
        public Point(String pos)
        {
            this.x = getPointFromString(pos).getX();
            this.y = getPointFromString(pos).getY();
        }
        public Point()
        {
            this.x = 0;
            this.y = 0;
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

        override
        public string ToString()
        {
            String posX = "";
            int newY = y + 1;

            if (y == 9)
                Console.WriteLine("stop");
            switch (getX())
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

    
        public Boolean equal(Point p)
        {
            return this.x == p.getX() && this.y == p.getY();
        }

        public Point getPointFromString(String pos)
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
