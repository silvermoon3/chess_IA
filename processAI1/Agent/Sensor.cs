using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Agent
{
    public enum Color { Black,White}

    public class Sensor
    {

        bool isWhiteTurn;


        public Sensor()
        {
            isWhiteTurn = true;
        }       

        //Show the board 

        public void ShowBoard(int[] tabVal)
        {
            //OK 
            Console.Write("Belief: \n");
            for (int i=0; i<tabVal.Length; i++)
            {
                Console.Write(tabVal[i]);
                if((i + 1) % 8 == 0) Console.Write("\n");

            }
            
        }


        //Transform the board into fen 
        public String GetFenBoard(int[] tabVal)
        {        
            String fenBoard = "";
            int count = 0;
            for (int i = 0; i < tabVal.Length; i++)
            {                
                if (tabVal[i] == 1)
                {
                    if (count != 0)
                    {
                        fenBoard += count;
                        count = 0;
                    }
                    fenBoard += 'P';
                }                   
                if (tabVal[i] == -1)
                {
                    if (count != 0)
                    {
                        fenBoard += count;
                        count = 0;
                    }
                    fenBoard += 'p';
                }               
                 if (tabVal[i] == 21 || tabVal[i] == 22)
                {
                    if (count != 0)
                    {
                        fenBoard += count;
                        count = 0;
                    }
                    fenBoard += 'R';
                }                
                 if (tabVal[i] == -21 || tabVal[i] == -22)
                {
                    if (count != 0)
                    {
                        fenBoard += count;
                        count = 0;
                    }
                    fenBoard += 'r';
                }               
                 if (tabVal[i] == 31 || tabVal[i] == 32)
                {
                    if (count != 0)
                    {
                        fenBoard += count;
                        count = 0;
                    }
                    fenBoard += 'N';
                }               
                 if (tabVal[i] == -31 || tabVal[i] == -32)
                {
                    if (count != 0)
                    {
                        fenBoard += count;
                        count = 0;
                    }
                    fenBoard += 'n';
                }              
                 if (tabVal[i] == 4)
                {
                    if (count != 0)
                    {
                        fenBoard += count;
                        count = 0;
                    }
                    fenBoard += 'B';
                }                
                 if (tabVal[i] == -4)
                {
                    if (count != 0)
                    {
                        fenBoard += count;
                        count = 0;
                    }
                    fenBoard += 'b';
                }               
                 if (tabVal[i] == 5)
                {
                    if (count != 0)
                    {
                        fenBoard += count;
                        count = 0;
                    }
                    fenBoard += 'Q';
                }              
                 if (tabVal[i] == -5)
                {
                    if (count != 0)
                    {
                        fenBoard += count;
                        count = 0;
                    }
                    fenBoard += 'q';
                }               
                 if (tabVal[i] == 6)
                {
                    if (count != 0)
                    {
                        fenBoard += count;
                        count = 0;
                    }
                    fenBoard += 'K';
                }                
                 if (tabVal[i] == -6)
                {
                    if (count != 0)
                    {
                        fenBoard += count;
                        count = 0;
                    }
                    fenBoard += 'k';
                }
                 if (tabVal[i] == 0)
                {
                    count++;
                }
                //Fin de la ligne 
                if ((i+1) % 8 ==0 )
                {
                    if(count != 0)
                    {
                        fenBoard += count;
                        count = 0;
                    }
                   if(i != tabVal.Length - 1)
                    {
                        fenBoard += '/';
                        count = 0;
                    }
                       
                }
            }            
            fenBoard += " " + Belief.myCharColor + " KQkq -" ;
            Console.WriteLine(fenBoard);
            return fenBoard;
        }

        private char getColorTurn()
        {
            if (isWhiteTurn)
                return 'w';
            else
                return 'b';
        }
    }
}
