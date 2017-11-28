using System;
using System.Linq;
using System.Threading;
using processAI1.Board;
using processAI1.Board.Bitboard;

namespace processAI1
{
    public class UciProtocol : IComunicationProtocol
    {
        const string Enginename = "Deep Elie";
        IChessBoard chessBoard = new BitBoard();
        Thread agentThread;
        Agent.Agent agent;
        public void Run()
        {
            string inputString;
            // Déclaration du thread
            
            agent = new Agent.Agent();
            chessBoard.InitStartingBoard();
            do
            {
                inputString = Console.ReadLine() ?? "";

                if ("uci".Equals(inputString))
                {
                    InputUci();
                }
                else if (inputString.StartsWith("setoption"))
                {
                    InputSetOption(inputString);
                }
                else if ("isready".Equals(inputString))
                {
                    InputIsReady();
                }
                else if ("ucinewgame".Equals(inputString))
                {
                    InputUciNewGame();
                }
                else if (inputString.StartsWith("position"))
                {
                    InputPosition(inputString);
                }
                else if (inputString.StartsWith("go"))
                {
                    InputGo();
                }
                else if ("print".Equals(inputString))
                {
                    InputPrint();
                }

            } while (inputString != "quit");

            

        }

        public void InputUci()
        {
            Console.WriteLine("id name " + Enginename);
            Console.WriteLine("id author MA_PE_GO");
            //options go here
            Console.WriteLine("uciok");
            
        }
        public void InputSetOption(String inputString)
        {
            //set options
        }
        public void InputIsReady()
        {
            Console.WriteLine("readyok");
        }
        public void InputUciNewGame()
        {
            //add code here
        }
        public void InputPosition(string input)
        {
            input = input.Substring(9) + " ";
            if (input.Contains("startpos "))
            {
                input = input.Substring(9);
                
            }
            else if (input.Contains("fen"))
            {
                input = input.Substring(4);
                //TODO board generation implementation
                //agent.ImportFen(input);
            }
            if (input.Contains("moves"))
            {
                input = input.Substring(input.IndexOf("moves") + 6);
                while (input.Length > 0)
                {
                    string moves;

                    //TODO player to move list all possibles moves

                }
            }
        }
        public void InputGo()
        {
            //TODO go command
        }
        public String MoveToAlgebra(String move)
        {
            //TODO convert UCI Protocol string move to move of AI
            return null;
        }
        public void AlgebraToMove(String input, String moves)
        {
            //TODO convert move of AI to UCI Protocol string move
            
        }

        public void InputPrint()
        {
            //TODO InputPrint
        }
    }
}