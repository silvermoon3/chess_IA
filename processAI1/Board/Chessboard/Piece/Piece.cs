using System;
using System.Collections.Generic;

namespace processAI1.Board.Chessboard.Piece
{
    public abstract class Piece
    {
        public bool IsWhite { get; set; }
        public Point Position { get; set; }
        
        public Piece(int x, int y, bool isWhite = true)
        {
            Position = new Point(x, y);
            IsWhite = isWhite;

        }
      

        public Piece(String pos, bool isWhite = true)
        {
            Position = new Point(pos);
            IsWhite = isWhite;            
        }

        public virtual List<Move> GetPossibleMoves(ChessBoard game)
        {
            return new List<Move>();
        }
                   
        public override string ToString()
        {
            return "";
        }
       
        public void SetPosition(Point newPosition)
        {
            this.Position = newPosition;
        }

        public Point GetPosition()
        {
            return Position;
        }
        
     
    }
}
