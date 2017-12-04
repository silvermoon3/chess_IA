using processAI1.Agent.BDI;
using processAI1.Board;
using System;
using System.Collections.Generic;
using System.Linq;


namespace processAI1.Agent
{
    class Agent
    {
        Belief _belief;
        Sensor _sensor;
        Intention _intention;
        Desire _desire;
                      
        public Agent()
        {
           _sensor = new Sensor();
           _belief = new Belief();
           _desire = new Desire();
           _intention = new Intention();
        }
        public void DoWork()
        {
            UpdateBelief();
              
        }

        public Sensor GetEffector()
        {
            return _sensor;
        }

        public void UpdateBelief()
        {
          
        }

        public void Think()
        {
            //Look for the best move
            Random rnd = new Random();
            // coord[0] = mesPieces[rnd.Next(mesPieces.Count)];

            /*    Exploration      */
            Search search = new Search();

        }

      



        public void DrawBelief()
        {
           
        }

        public void ImportFen(string input)
        {
            //sensor.importFEN(input);
            this.UpdateBelief();
        }
    }
}
