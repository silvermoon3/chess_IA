
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Board;

namespace processAI1.Agent
{
    class Node
    {
        private Move _move;
        private int _value;


        public Node(Move move, int value)
        {
            this._move = move;
            this._value = value;
        }
        public Node()
        {
            
        }

        public Move GetMove()
        {
            return _move;
        }
        public int GetValue()
        {
            return _value;
        }

        public void SetValue(int value)
        {
            this._value = value;
        }

    }
}
