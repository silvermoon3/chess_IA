namespace processAI1.Board.Chessboard
{
    public class Cell
    {
        private Board.Chessboard.Piece.Piece _piece;
        private Point _position;
        public bool IsOccupied { get; set; }

        public Cell(Board.Chessboard.Piece.Piece piece, Point position)
        {
            this._piece = piece;
            this._position = position;
            IsOccupied = piece != null;            

        }

        public Board.Chessboard.Piece.Piece GetPiece()
        {
            return _piece;
        }
        public Point GetPosition()
        {
            return _position;
        }

        public void SetPiece(Board.Chessboard.Piece.Piece piece)
        {
            this._piece = piece;
            IsOccupied = piece != null;
        }

      
    }
}
