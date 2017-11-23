using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Piece;

namespace processAI1
{
    class Agent
    {

        Belief belief;
        Effector effector;
        static Cell[,] board;



        public Agent()
        {
           effector = new Effector();
           belief = new Belief();
           board = new Cell[8, 8];
           board[0, 0] = new Cell(new Rook(0, 0, true), new Point(0, 0));          
           board[1, 0] = new Cell(new Knight(1, 0, true), new Point(1, 0));
           board[2, 0] = new Cell(new Bishop(2, 0, true), new Point(2, 0));
           board[3, 0] = new Cell(new Queen(3, 0, true), new Point(3, 0));
           board[4, 0] = new Cell(new King(4, 0, true), new Point(4, 0));
           board[5, 0] = new Cell(new Bishop(5, 0, true), new Point(5, 0));
           board[6, 0] = new Cell(new Knight(6, 0, true), new Point(6, 0));
           board[7, 0] = new Cell(new Rook(7, 0, true), new Point(7, 0));
           board[0, 1] = new Cell(new Pawn(0, 1, true), new Point(0, 1));
           board[1, 1] = new Cell(new Pawn(1, 1, true), new Point(1, 1));
           board[2, 1] = new Cell(new Pawn(2, 1, true), new Point(2, 1));
           board[3, 1] = new Cell(new Pawn(3, 1, true), new Point(3, 1));
           board[4, 1] = new Cell(new Pawn(4, 1, true), new Point(4, 1));
           board[5, 1] = new Cell(new Pawn(5, 1, true), new Point(5, 1));
           board[6, 1] = new Cell(new Pawn(6, 1, true), new Point(6, 1));
           board[7, 1] = new Cell(new Pawn(7, 1, true), new Point(7, 1));

            board[0, 7] = new Cell(new Rook(0, 7, false), new Point(0, 7));
            board[1, 7] = new Cell(null /*new Knight(1, 7, false)*/, new Point(1, 7));
            board[2, 7] = new Cell(null /*new Bishop(2, 7, false)*/, new Point(2, 7));
            board[3, 7] = new Cell(/*null*/ new Queen(3, 7, false), new Point(3, 7));
            board[4, 7] = new Cell(/*null*/ new King(4, 7, false), new Point(4, 7));
            board[5, 7] = new Cell(/*null*/ new Bishop(5, 7, false), new Point(5, 7));
            board[6, 7] = new Cell(new Knight(6, 7, false), new Point(6, 7));
            board[7, 7] = new Cell(new Rook(7, 7, false), new Point(7, 7));
            board[0, 6] = new Cell(null, new Point(0, 6));
            board[1, 6] = new Cell(new Pawn(1, 6, false), new Point(1, 6));
            board[2, 6] = new Cell(new Pawn(2, 6, false), new Point(2, 6));
            board[3, 6] = new Cell(null /*new Pawn(3, 6, false)*/, new Point(3, 6));
            board[4, 6] = new Cell(null /*new Pawn(4, 6, false)*/, new Point(4, 6));
            board[5, 6] = new Cell(null /*new Pawn(5, 6, false)*/, new Point(5, 6));
            board[6, 6] = new Cell(new Pawn(6, 6, false), new Point(6, 6));
            board[7, 6] = new Cell(new Pawn(7, 6, false), new Point(7, 6));


            for (int i = 0; i < 8; i++)
                for (int j = 2; j < 6; j++)
                    board[i, j] = new Cell(null, new Point(i, j));
        }
        public void doWork()
        {
            //Update belief
            // belief.Update(effector.getBoard());
            belief.Update(board);

            //Afficher le Board
            for (int i = 7; i >= 0; i--)
            {
                switch (i)
                {
                    case 0: Console.Write("8"); break;
                    case 1: Console.Write( "7"); break;
                    case 2: Console.Write( "6"); break;
                    case 3: Console.Write("5"); break;
                    case 4: Console.Write("4"); break;
                    case 5: Console.Write("3"); break;
                    case 6: Console.Write("2"); break;
                    case 7: Console.Write("1"); break;
                }
                Console.Write("|");
                for (int j = 7; j >= 0; j--)
                {
                    if(board[j, i].getPiece() != null)
                     Console.Write(board[j, i].getPiece().getPiece());
                    else
                    
                        Console.Write("x");
                    
                    if(j == 0)
                    {
                        Console.Write("\n");
                        //retour à la ligne
                    }
                }
            }

            foreach (Point p  in board[1, 6].getPiece().getPossibleMoves(belief))
            {
                Console.WriteLine("Point possible Pawn " + p.getX() + "," + p.getY());
            }
            //foreach (Point p in board[2, 0].getPiece().getPossibleMoves(belief))
            //{
            //    Console.WriteLine("Point possible Fou " + p.getX() + "," + p.getY());
            //}
            foreach (Point p in board[3, 7].getPiece().getPossibleMoves(belief))
            {
                Console.WriteLine("Point possible Dame " + p.getX() + "," + p.getY());
            }

            foreach (Point p in board[4, 7].getPiece().getPossibleMoves(belief))
            {
                Console.WriteLine("Point possible Roi " + p.getX() + "," + p.getY());
            }
            foreach (Point p in board[5, 7].getPiece().getPossibleMoves(belief))
            {
                Console.WriteLine("Point possible Fou " + p.getX() + "," + p.getY());
            }

           



        }

      
    }
}
