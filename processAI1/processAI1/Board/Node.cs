using processAI1.Piece;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Agent
{
    class Node
    {
        private Move move;
        private int value;


        public Node(Move _move, int _value)
        {
            this.move = _move;
            this.value = _value;
        }
        public Node()
        {
            
        }

        public Move getMove()
        {
            return move;
        }
        public int getValue()
        {
            return value;
        }

        public void setValue(int _value)
        {
            value = _value;
        }

    }
}
