using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using processAI1.Board;
using processAI1.Board.Bitboard;

namespace processAI1
{
    public class UciProtocol : IComunicationProtocol
    {
        const string Enginename = "Deep Elie";
        public Board.Board b;
        public void Run()
        {
            string inputString;
            // Déclaration du thread
            b = new Board.Board();
            b.init_fen(ref Board.Board.start_fen);

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
                b.init_fen(ref Board.Board.start_fen);
            }
            else if (input.Contains("fen"))
            {
                input = input.Substring(4);
                b.init_fen(ref input);
            }
            if (input.Contains("moves"))
            {
                
                input = input.Substring(input.IndexOf("moves") + 6);
                while (input.Length > 0)
                {
                    
                    string moves = input.Substring(0,4);
                    Move m = Move.from_string(ref moves, ref b);
                    b.move(m);
                    input = input.Substring(input.IndexOf(' ') + 1);

                    
                    //TODO player to move list all possibles moves

                }
            }
        }
        public void InputGo()
        {
            InputPrint();
            List<Move> m = new List<Move>();
            Gen.gen_legals(ref m, ref b);
            Console.WriteLine("bestmove " + m[0]);
            //Console.WriteLine("bestmove " + move.GetInitialPosition().ToString()+move.GetFinalPosition().ToString());
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
            Console.WriteLine(b.turn());
            Console.WriteLine(b);
            
        }
    }
}