using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1
{
    class Belief
    {
       static Square[,] board = new Square[8, 8];
        
        public Square getCase(int colonne, int rangee)
        {
            return board[colonne, rangee];
        }
        public Square getCase(Position position)
        {
            return board[position.getX() , position.getY()];
        }
        
        public  void Update(Square[,] newBoard)
        {
            board = newBoard;
        }

        public static Boolean isOccupied(Position position)
        {
            return board[position.getX(), position.getY()].getPiece() != null;
        }

        public static Boolean isOccupied(int colonne, int rangee)
        {
            return board[colonne, rangee].getPiece() != null;
        }
    }
}
