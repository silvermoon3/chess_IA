namespace processAI1.Board.Bitboard
{
    public interface IChessBoard
    {

        void InitStartingBoard();

        void AllPossibleMoves();

        void PossibleMovesForPiece(string piecePosition);

        void MakeMove();

        void DrawBoard();

    }
}