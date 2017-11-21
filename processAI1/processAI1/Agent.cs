using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1
{
    class Agent
    {

        Belief belief;
        Effector effector;
        colorPlayer color;

        public Agent(colorPlayer _color)
        {
            this.color = _color;
        }
        public void doWork()
        {
            //Update belief
            belief.Update(effector.getBoard());

        }
    }
}
