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
        Cell[,] board = new Cell[8, 8];
        List<Piece.Piece> myPieces = new List<Piece.Piece>();
        
        public Belief()
        {
            
        }
                       
        public void Update(Cell[,] _board )
        {
            board = _board;    
        }
        
        public Boolean isOccupied(Point p)
        {
            return board[p.GetX(), p.GetY()].isOccupied;
        }

        public Boolean isOccupied(int x, int y)
        {
            return board[x,y].isOccupied;
        }

        public Boolean isOccupiedWithMyPiece(Point p, Boolean isWhite)
        {
            if(isOccupied(p.GetX(), p.GetY()))
                 return board[p.GetX(), p.GetY()].getPiece().isWhite == isWhite;
            return false;
        }

        public Cell getCase(int x, int y)
        {
            return board[x, y];
        }

        public List<Move> getPossibleMoves()
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    if (board[i, j].isOccupied)
                        if (board[i, j].getPiece().isWhite && (board[i, j].getPiece() is Knight || board[i, j].getPiece() is Bishop))
                            moves.AddRange(board[i, j].getPiece().getPossibleMoves(this));
            }

            return moves;
        }



        




    }
}
