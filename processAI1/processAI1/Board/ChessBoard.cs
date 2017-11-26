using processAI1.Piece;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Board
{
    public class ChessBoard
    {
        List<Piece.Piece> pieces = new List<Piece.Piece>();
        public Cell[,] board = new Cell[8, 8];

        public ChessBoard()
        {
            //init cell
        }

        public ChessBoard(Cell[,] _board)
        {
            
        }

        public void makeMove(Move _move)
        {
            //board[_move.getFinalPosition().getX(), _move.getFinalPosition().getY()].setPiece(board[_move.getInitialPosition().getX(), _move.getInitialPosition().getY()].getPiece());
            //board[_move.getFinalPosition().getX(), _move.getFinalPosition().getY()].getPiece().setPosition(board[_move.getInitialPosition().getX(), _move.getInitialPosition().getY()].getPiece().getPosition());

            //board[_move.getInitialPosition().getX(), _move.getInitialPosition().getY()].setPiece(null);
            Piece.Piece currentPiece = board[_move.getInitialPosition().getX(), _move.getInitialPosition().getY()].getPiece();

            board[_move.getFinalPosition().getX(), _move.getFinalPosition().getY()].setPiece(currentPiece);
            board[_move.getFinalPosition().getX(), _move.getFinalPosition().getY()].getPiece().setPosition(_move.getFinalPosition());

            board[_move.getInitialPosition().getX(), _move.getInitialPosition().getY()] = new Cell(null, _move.getInitialPosition());

        }


        public void undoMove(Move _move)
        {
            //board[_move.getInitialPosition().getX(), _move.getInitialPosition().getY()].setPiece(board[_move.getFinalPosition().getX(), _move.getFinalPosition().getY()].getPiece());
            //board[_move.getFinalPosition().getX(), _move.getFinalPosition().getY()].setPiece(null);
            Piece.Piece currentPiece = board[_move.getFinalPosition().getX(), _move.getFinalPosition().getY()].getPiece();
            board[_move.getInitialPosition().getX(), _move.getInitialPosition().getY()].setPiece(currentPiece);
            board[_move.getInitialPosition().getX(), _move.getInitialPosition().getY()].getPiece().setPosition(_move.getInitialPosition());

            board[_move.getFinalPosition().getX(), _move.getFinalPosition().getY()] = new Cell(null, _move.getFinalPosition());

        }

        public void updateBoard(Cell[,] board)
        {
            this.board = board;
        }
        public Cell getCase(int x, int y)
        {
            return board[x, y];
        }


        public Boolean isOccupied(Point p)
        {
            return board[p.getX(), p.getY()].isOccupied;
        }

        public Boolean isOccupied(int x, int y)
        {
            return board[x, y].isOccupied;
        }

        public Boolean isOccupiedWithMyPiece(Point p, Boolean isWhite)
        {
            if (isOccupied(p.getX(), p.getY()))
                return board[p.getX(), p.getY()].getPiece().isWhite == isWhite;
            return false;
        }

        public List<Move> getAllPossibleMoves(Boolean _isWhite = true)
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    if (getCase(i, j).isOccupied)
                        if (getCase(i, j).getPiece().isWhite == _isWhite)
                            moves.AddRange(getCase(i, j).getPiece().getPossibleMoves(this));
            }

            return moves;
        }

        public void Show()
        {
            Console.Write("\n\n");
            Console.Write("a b c d e f g h\n");
            Console.Write("+-------------------------------+\n");
            for (int j = 7; j >= 0; --j)             
            {
                switch (j)
                {
                    case 0: Console.Write("8"); break;
                    case 1: Console.Write("7"); break;
                    case 2: Console.Write("6"); break;
                    case 3: Console.Write("5"); break;
                    case 4: Console.Write("4"); break;
                    case 5: Console.Write("3"); break;
                    case 6: Console.Write("2"); break;
                    case 7: Console.Write("1"); break;
                }
                Console.Write("|");
                for (int i = 0; i < 8; ++i)
                {
                    if (isOccupied(i, j))
                        Console.Write(getCase(i, j).getPiece().getPiece());
                    else
                        Console.Write("x");

                   

                    Console.Write("|");
                    if (i == 7) Console.Write("\n");
                }
              
            }

        }
    }
}
