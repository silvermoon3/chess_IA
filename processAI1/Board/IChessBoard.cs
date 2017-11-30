using System;
using System.Collections.Generic;
using System.Linq;
using processAI1.Board.Chessboard;
using processAI1.Board.Chessboard.Piece;

namespace processAI1.Board
{
    public interface IChessBoard
    {

        void InitStartingBoard();

        void MakeMove(Move move);

        void UndoMove(Move move);

        void UpdateChessBoard(List<Chessboard.Piece.Piece> myPieces, List<Chessboard.Piece.Piece> otherPieces);


        void UpdateBoard();

        Boolean IsOccupied(Point p);


        Boolean IsOccupied(int x, int y);

        Boolean IsOccupiedWithMyPiece(Point p, Boolean isWhite);

        List<Move> GetAllPossibleMoves(Boolean isWhite = true);

        List<Move> GetAllMovesForCurrentPlayer(Boolean isWhite = true);

        Boolean AmIInCheck();

        int Evaluate(int evaluateOption, Boolean isWhiteTurn);

        //Simple evaluation
        int EvaluateBoardWithPieceValue();
        
    }
}