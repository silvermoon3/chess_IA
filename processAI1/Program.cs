using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using processAI1.Agent;
using System.Windows.Forms;
using processAI1.Board;

namespace processAI1
{
    
    public enum ColorPlayer { White, Black };
    class Program
    {
        
        static void Main(string[] args)
        {
            Bit.Init();
            Material.init();
            Hash.Init();
            Castling.init();
            Attack.init();
            PieceSquareTable.init();
            Pawn.init();
            Eval.init();
            chooseProtocol();        
        }

        static void chooseProtocol()
        {
            string inputString;
            Console.WriteLine("Choix du protocole: \n  " +
                                 "Plateforme de base : 1 \n  " +
                                 "UCI protocol : uci ");
            inputString = Console.ReadLine() ?? "";
            if ("1".Equals(inputString))
            {
                CustomProtocol protocol = new CustomProtocol();
                Console.WriteLine("Plateforme de base lancée ");
                Belief.SetMyColor(protocol.WhatIsMyColor());
                protocol.Run();

            }
            else if ("uci".Equals(inputString))
            {
                IComunicationProtocol protocol = new UciProtocol();
                Console.WriteLine("UCI protocol lancé ");
                protocol.Run();
            }            
            else
            {
                Console.WriteLine("Error, please choose a protocol");
                chooseProtocol();
            }
        }
       

    }
}
