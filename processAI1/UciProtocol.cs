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
        BitBoard _chessBoard = new BitBoard();
        Thread _agentThread;
        Agent.Agent _agent;
        public void Run()
        {
            string inputString;
            // Déclaration du thread
            
            _agent = new Agent.Agent();
            _chessBoard.InitStartingBoard();
            foreach (Move m in _chessBoard.Move.PossibleMoves(false))
            {
                Console.WriteLine(m);
            }
            
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
                _agent.ImportFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1 ");
                
            }
            else if (input.Contains("fen"))
            {
                input = input.Substring(4);
                //TODO board generation implementation
                _agent.ImportFen(input);
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
            string[] move = _agent.GetBestMove();
            Console.WriteLine("bestmove " + move[0]+move[1]);
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
            Console.WriteLine(_chessBoard.ToString());
        }
    }
}