﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;

namespace processAI1
{
    public class CustomProtocol : IComunicationProtocol
    {
        string rep = "";
        string mutex = "";
        string mutexStart = "";
        public ColorPlayer WhatIsMyColor()
        {
            string inputString;
            Console.WriteLine("What is my color ?  w / b");
            inputString = Console.ReadLine() ?? "";
            if ("w".Equals(inputString))
            {
                rep = "repAI1";
                mutex = "mutexAI1";
                mutexStart = "mutexStartAI1";
                return ColorPlayer.White;

            }   
               

            else if ("b".Equals(inputString))
            {
                rep = "repAI2";
                mutex = "mutexAI2";
                mutexStart = "mutexStartAI2";
                return ColorPlayer.Black;
            }
            
            else
            {
                Console.WriteLine("Error, please enter a correct color player ");
                return WhatIsMyColor();
            }
           
        }

        public void Run()
        {
            try
            {
                bool stop = false;
                int[] tabVal = new int[64];
                String value;
                String[] coord = new String[] { "", "", "" };
                String[] tabCoord = new string[] { "a8","b8","c8","d8","e8","f8","g8","h8",
                                                   "a7","b7","c7","d7","e7","f7","g7","h7",
                                                   "a6","b6","c6","d6","e6","f6","g6","h6",
                                                   "a5","b5","c5","d5","e5","f5","g5","h5",
                                                   "a4","b4","c4","d4","e4","f4","g4","h4",
                                                   "a3","b3","c3","d3","e3","f3","g3","h3",
                                                   "a2","b2","c2","d2","e2","f2","g2","h2",
                                                   "a1","b1","c1","d1","e1","f1","g1","h1" };
                Agent.Agent agent = new Agent.Agent();



                while (!stop)
                {

                    using (var mmf = MemoryMappedFile.OpenExisting("plateau"))
                    {
                        using (var mmf2 = MemoryMappedFile.OpenExisting(rep))
                        {
                            Mutex mutexStartAi1 = Mutex.OpenExisting(mutexStart);
                            Mutex mutexAi1 = Mutex.OpenExisting(mutex);
                            mutexAi1.WaitOne();
                            mutexStartAi1.WaitOne();


                            using (var accessor = mmf.CreateViewAccessor())
                            {
                                ushort size = accessor.ReadUInt16(0);
                                byte[] buffer = new byte[size];
                                accessor.ReadArray(0 + 2, buffer, 0, buffer.Length);

                                value = ASCIIEncoding.ASCII.GetString(buffer);
                                if (value == "stop") stop = true;
                                else
                                {
                                    String[] substrings = value.Split(',');
                                    for (int i = 0; i < substrings.Length; i++)
                                    {
                                        tabVal[i] = Convert.ToInt32(substrings[i]);
                                    }
                                }
                            }
                            if (!stop)
                            {

                                /******************************************************************************************************/
                                /***************************************** ECRIRE LE CODE DE L'IA *************************************/
                                /******************************************************************************************************/

                                /***************************************** BOUCLE IA *************************************/
                                //Sensor action
                                for (int i = 0; i < tabVal.Length; i++)
                                {
                                    Console.Write(tabVal[i]);
                                    if ((i + 1) % 8 == 0) Console.Write("\n");
                                }

                                //agent.DrawBelief(tabVal);
                                String fenBoard = agent.ReadWithSensor(tabVal);                           

                                //update belief
                                agent.UpdateBelief(fenBoard);

                                //Think and update the best move to do
                                agent.Think();

                                /***************************************** FIN BOUCLE IA *************************************/

                                //result 
                                if (agent.GetBestMove()[0] != "echec")
                                {
                                    String[] result = agent.GetBestMove();
                                    coord[0] = result[0];
                                    coord[1] = result[1];
                                    coord[2] = result[2];
                                }
                                
                              
                                //List<String> mesPieces = new List<String>();
                                //for (int i = 0; i < tabVal.Length; i++)
                                //{
                                //    if (tabVal[i] > 0) mesPieces.Add(tabCoord[i]);
                                //}

                                //List<String> reste = new List<String>();
                                //for (int i = 0; i < tabVal.Length; i++)
                                //{
                                //    if (tabVal[i] <= 0) reste.Add(tabCoord[i]);
                                //}

                                //a.GetEffector().ReadBoard(mesPieces, reste, tabVal);
                                //a.DoWork();
                                //Random rnd = new Random();
                                // coord[0] = mesPieces[rnd.Next(mesPieces.Count)];
                                //String[] result = a.GetBestMove();
                                //coord[0] = result[0];
                                //coord[1] = result[1];
                                //coord[1] = tabCoord[rnd.Next(reste.Count)];
                                //coord[2] = "P";


                                /********************************************************************************************************/
                                /********************************************************************************************************/
                                /********************************************************************************************************/

                                using (var accessor = mmf2.CreateViewAccessor())
                                {
                                    value = coord[0];
                                    for (int i = 1; i < coord.Length; i++)
                                    {
                                        value += "," + coord[i];
                                    }
                                    byte[] buffer = ASCIIEncoding.ASCII.GetBytes(value);
                                    accessor.Write(0, (ushort)buffer.Length);
                                    accessor.WriteArray(0 + 2, buffer, 0, buffer.Length);
                                }
                            }
                            mutexAi1.ReleaseMutex();
                            mutexStartAi1.ReleaseMutex();
                        }
                    }

                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Memory-mapped file does not exist. Run Process A first.");
                Console.ReadLine();
            }
        }
    }
}