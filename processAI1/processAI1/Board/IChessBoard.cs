using System;
using System.Management.Instrumentation;
using processAI1.Piece;

namespace processAI1.Board
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