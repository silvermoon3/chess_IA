using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Board;
using processAI1.Agent;

namespace processAI1
{
    public class Belief
    {
        public static ColorPlayer myColor;
        public static char myCharColor;
        Board.Board board;
        String fenBoard;
        
        public Belief()
        {
            board = new Board.Board();
            board.init_fen(ref Board.Board.start_fen);
        }

        public static void SetMyColor(ColorPlayer _myColor)
        {
            myColor = _myColor;
            if (myColor == ColorPlayer.White)
                myCharColor = 'w';
            else
                myCharColor = 'b';

        }

        public void Update(String _fenBoard)
        {
            fenBoard = _fenBoard;
            board.init_fen(ref fenBoard);
        }

        public Board.Board GetBoard()
        {
            return board;
        }
        
    }
}
