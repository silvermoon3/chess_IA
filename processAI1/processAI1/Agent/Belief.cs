using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Piece;
using processAI1.Board;

namespace processAI1
{
    public class Belief
    {
        Board.ChessBoard board;
        List<Piece.Piece> myPieces = new List<Piece.Piece>();
        
        public Belief()
        {
            board = new ChessBoard();
        }
                       
        public void Update(Cell[,] _board )
        {
            board.updateBoard(_board);    
        }
        
        public Boolean isOccupied(Point p)
        {
            return board.isOccupied(p); ;
        }

        public Boolean isOccupied(int x, int y)
        {
            return board.isOccupied(x, y);
        }

        public Boolean isOccupiedWithMyPiece(Point p, Boolean isWhite)
        {
            if(isOccupied(p.getX(), p.getY()))
                 return board.getCase(p.getX(), p.getY()).getPiece().isWhite == isWhite;
            return false;
        }

       
        public List<Move> getPossibleMoves()
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    if (board.getCase(i, j).isOccupied)
                        if (board.getCase(i, j).getPiece().isWhite)
                            moves.AddRange(board.getCase(i, j).getPiece().getPossibleMoves(board));
            }

            return moves;
        }
        public Cell getCase(int x, int y)
        {
            return board.getCase(x, y);
        }
        public ChessBoard getChessBoard()
        {
            return board;
        }
    }
}
