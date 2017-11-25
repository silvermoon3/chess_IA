using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Piece;

namespace processAI1
{
    class Effector
    {
        int[] tabVal = new int[64];
        String[] tabCoord = new string[] { "a8","b8","c8","d8","e8","f8","g8","h8",
                                           "a7","b7","c7","d7","e7","f7","g7","h7",
                                           "a6","b6","c6","d6","e6","f6","g6","h6",
                                           "a5","b5","c5","d5","e5","f5","g5","h5",
                                           "a4","b4","c4","d4","e4","f4","g4","h4",
                                           "a3","b3","c3","d3","e3","f3","g3","h3",
                                           "a2","b2","c2","d2","e2","f2","g2","h2",
                                           "a1","b1","c1","d1","e1","f1","g1","h1" };
        public Cell[,] getBoard()
        {
            Cell[,] board = new Cell[8, 8];
            for (int i = 0; i<8; i++)
            {
                for (int j = 0; j < 8; j++)
                    board[i, j] = null;
            }

            for (int i = 0; i < tabVal.Length; i++)
            {
                Point p = new Point(tabCoord[i]);
                Boolean isWhite = tabVal[i] > 0;
                if (Math.Abs(tabVal[i]) == 21)
                    board[p.getX(), p.getY()] = new Cell(new Rook(p.getX(), p.getY(), isWhite), p);
                else if (Math.Abs(tabVal[i]) == 31)
                    board[p.getX(), p.getY()] = new Cell(new Bishop(p.getX(), p.getY(), isWhite), p);
                else if (Math.Abs(tabVal[i]) == 4)
                    board[p.getX(), p.getY()] = new Cell(new Knight(p.getX(), p.getY(), isWhite), p);
                else if (Math.Abs(tabVal[i]) == 5)
                    board[p.getX(), p.getY()] = new Cell(new Queen(p.getX(), p.getY(), isWhite), p);
                else if (Math.Abs(tabVal[i]) == 6)
                    board[p.getX(), p.getY()] = new Cell(new King(p.getX(), p.getY(), isWhite), p);
                else if (Math.Abs(tabVal[i]) == 32)
                    board[p.getX(), p.getY()] = new Cell(new Bishop(p.getX(), p.getY(), isWhite), p);
                else if (Math.Abs(tabVal[i]) == 22)
                    board[p.getX(), p.getY()] = new Cell(new Rook(p.getX(), p.getY(), isWhite), p);
                else if (Math.Abs(tabVal[i]) == 1)
                    board[p.getX(), p.getY()] = new Cell(new Pawn(p.getX(), p.getY(), isWhite), p);
                else
                {
                    board[p.getX(), p.getY()] = new Cell(null, p);
                }
                  
            }
            return board;
        }

      

        public void setTabVal(int[] _tabVal)
        {
            tabVal = _tabVal;
        }

     

    
       
    }
}
