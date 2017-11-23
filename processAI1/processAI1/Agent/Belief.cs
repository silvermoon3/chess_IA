using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Piece;

namespace processAI1
{
    public class Belief
    {
        Cell[,] board;
        
        public Belief()
        {
            board = new Cell[8, 8];
        }

        public  Cell getCase(int colonne, int rangee)
        {
            return board[colonne, rangee];
        }
        public  Cell getCase(Point position)
        {
            return board[position.getX() , position.getY()];
        }
               
        public  void Update(Cell[,] newBoard)
        {
            board = newBoard;
        }

        public  Boolean isOccupied(Point position)
        {
            return board[position.getX(), position.getY()].getPiece() != null;
        }

        public  Boolean isOccupied(int colonne, int rangee)
        {
            return board[colonne, rangee].getPiece() != null;
        }

        public  Boolean isOccupiedWithMyPiece(int colonne, int rangee, bool isWhite)
        {
            if (isOccupied(colonne, rangee))
                return getCase(colonne, rangee).getPiece().isWhite == isWhite;

            return false;          

        }

        public  Boolean isOccupiedWithMyPiece(Point position, bool isWhite)
        {
            if (isOccupied(position))
                return getCase(position).getPiece().isWhite == isWhite;

            return false;


        }
    }
}
