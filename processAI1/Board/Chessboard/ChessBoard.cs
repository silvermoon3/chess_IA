using System;
using System.Collections.Generic;
using System.Linq;
using processAI1.Board.Chessboard.Piece;

namespace processAI1.Board.Chessboard
{
    public class ChessBoard : IChessBoard
    {
        List<Chessboard.Piece.Piece> _myPieces = new List<Chessboard.Piece.Piece>();
        List<Chessboard.Piece.Piece> _otherPieces = new List<Chessboard.Piece.Piece>();
        public Cell[,] Board = new Cell[8, 8];

        public ChessBoard()
        {
            //init cell
        }

        public ChessBoard(Cell[,] board)
        {
            
        }

        public void MakeMove(Move move)
        {
            Chessboard.Piece.Piece currentPiece = Board[move.GetInitialPosition().GetX(), move.GetInitialPosition().GetY()].GetPiece();
            Board[move.GetFinalPosition().GetX(), move.GetFinalPosition().GetY()].SetPiece(currentPiece);
            Board[move.GetFinalPosition().GetX(), move.GetFinalPosition().GetY()].GetPiece().SetPosition(move.GetFinalPosition());
            Board[move.GetInitialPosition().GetX(), move.GetInitialPosition().GetY()] = new Cell(null, move.GetInitialPosition());
        }

        public void UndoMove(Move move)
        {
            Chessboard.Piece.Piece currentPiece = Board[move.GetFinalPosition().GetX(), move.GetFinalPosition().GetY()].GetPiece();
            Board[move.GetInitialPosition().GetX(), move.GetInitialPosition().GetY()].SetPiece(currentPiece);
            Board[move.GetInitialPosition().GetX(), move.GetInitialPosition().GetY()].GetPiece().SetPosition(move.GetInitialPosition());
            Board[move.GetFinalPosition().GetX(), move.GetFinalPosition().GetY()] = new Cell(null, move.GetFinalPosition());
        }

        public void UpdateChessBoard(List<Chessboard.Piece.Piece> myPieces , List<Chessboard.Piece.Piece> otherPieces)
        {
            this._myPieces = myPieces;
            this._otherPieces = otherPieces;
            UpdateBoard();
        }
        public Cell GetCase(int x, int y)
        {
            return Board[x, y];
        }

        public void UpdateBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    Board[i, j] = new Cell(null, new Point(i, j));
            }

            foreach (Chessboard.Piece.Piece piece in _myPieces)
            {
                Board[piece.GetPosition().GetX(), piece.GetPosition().GetY()] = new Cell(piece, piece.GetPosition());
            }

            foreach (Chessboard.Piece.Piece piece in _otherPieces)
            {
                Board[piece.GetPosition().GetX(), piece.GetPosition().GetY()] = new Cell(piece, piece.GetPosition());
            }
        }

        public Boolean IsOccupied(Point p)
        {
            if(p.ValidPosition())
                return Board[p.GetX(), p.GetY()].IsOccupied;
            return false;
        }

        public Boolean IsOccupied(int x, int y)
        {
         
            return Board[x, y].IsOccupied;
        }

        public Boolean IsOccupiedWithMyPiece(Point p, Boolean isWhite)
        {
            if (IsOccupied(p.GetX(), p.GetY()))
                return Board[p.GetX(), p.GetY()].GetPiece().IsWhite == isWhite;
            return false;
        }

        public List<Move> GetAllPossibleMoves(Boolean isWhite = true)
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    if (GetCase(i, j).IsOccupied)
                        if (GetCase(i, j).GetPiece().IsWhite == isWhite)
                            moves.AddRange(GetCase(i, j).GetPiece().GetPossibleMoves(this));
            }

            return moves;
        }
        public List<Move> GetAllMovesForCurrentPlayer(Boolean isWhite = true)
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    if (GetCase(i, j).IsOccupied)
                        if (GetCase(i, j).GetPiece().IsWhite == isWhite)
                            moves.AddRange(GetCase(i, j).GetPiece().GetPossibleMoves(this));
            }

            return moves;
        }

        public Boolean AmIInCheck()
        {
            Point myKingPosition = _myPieces.Find(p => p is King).GetPosition();
            foreach(Move move in GetAllMovesForCurrentPlayer(false))
            {
                if (move.GetFinalPosition().Equal(myKingPosition))
                    return true;
            }
            return false;
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
                    if (IsOccupied(i, j))
                        Console.Write(GetCase(i, j).GetPiece().ToString());
                    else
                        Console.Write("x");

                    Console.Write("|");
                    if (i == 7) Console.Write("\n");
                }
              
            }

        }

        public int Evaluate(int evaluateOption, Boolean isWhiteTurn)
        {        
            int result = 0;
            switch (evaluateOption)
            {
                case 0:
                    result = EvaluateMovability(isWhiteTurn);
                    break;
                case 1:
                    result = EvaluatePositions();
                    break;
                case 2:
                    result = EvaluateMaterial(isWhiteTurn) + EvaluateMovability(isWhiteTurn);
                    break;
                case 3:
                    result = EvaluateMaterial(isWhiteTurn) + EvaluatePositions();
                    break;
            }
            return result;
        }
        private int EvaluateMaterial(Boolean isWhiteTurn)
        {
            int result = 0;
            int blackScore = 0;
            int whiteScore = 0;

            Boolean isWhiteKing = false;
            Boolean isBlackKing = false;

            int score = 100;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (IsOccupied(i, j))
                    {

                        Chessboard.Piece.Piece pieceToCheck = GetCase(i, j).GetPiece();

                        if (pieceToCheck is Pawn)
                        {
                            score = 100;
                            if (pieceToCheck.IsWhite)
                            {
                                whiteScore += score;
                             
                            }
                            else
                            {
                                blackScore += score;
                            }
                           
                        }
                        else if (pieceToCheck is Rook)
                        {
                            score = 500;
                            if (pieceToCheck.IsWhite)
                            {
                                whiteScore += score;
                               
                            }
                            else
                            {
                                blackScore += score;
                            }
                            
                        }
                        else if (pieceToCheck is Bishop)
                        {
                            score = 330;
                            if (pieceToCheck.IsWhite)
                            {
                                whiteScore += score;
                            }
                            else
                            {
                                blackScore += score;                               
                            }
                            
                        }
                        else if (pieceToCheck is Knight)
                        {
                            score = 320;
                            if (pieceToCheck.IsWhite)
                            {
                                whiteScore += score;
                            }
                            else
                            {
                                blackScore += score;
                            }
                           
                        }
                        else if (pieceToCheck is Queen)
                        {
                            score = 900;
                            if (pieceToCheck.IsWhite)
                            {
                                whiteScore += score;
                            }
                            else
                            {
                                blackScore += score;
                            }
                           
                        }
                        else if (pieceToCheck is King)
                        {
                            score = 20000;
                            if (pieceToCheck.IsWhite)
                            {
                                isWhiteKing = true;
                                whiteScore += score;
                            }
                            else
                            {
                                isBlackKing = true;
                                blackScore += score;

                            }
                        }
                    }
                }
            }

            if (isWhiteTurn)
            {
                result = blackScore - whiteScore;
                if (!isBlackKing)
                    result = Int32.MinValue;

            }
            else
            {
                result = whiteScore - blackScore;
                if (!isWhiteKing)
                    result = Int32.MaxValue;
            }

            return result;
        }

        private int EvaluatePositions()
        {
            int thisScore = Rating.RateBoard(this, true);
            int otherScore = Rating.RateBoard(this, false);

            return thisScore - otherScore;
        }

        private int EvaluateMovability(Boolean isWhiteTurn)
        {
            int thisScore = GetAllMovesForCurrentPlayer().Count();
            isWhiteTurn = !isWhiteTurn;
            int otherScore = GetAllMovesForCurrentPlayer().Count();
            isWhiteTurn = !isWhiteTurn;
            return thisScore - otherScore;
        }


        //Simple evaluation
        public int EvaluateBoardWithPieceValue()
        {
            int score = 0;
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {                   
                     score = score + GetPieceValue(Board[i,j].GetPiece());
                }
            }
          
            return score;
        }
        private int GetPieceValue(Chessboard.Piece.Piece piece)
        {
            if (piece == null)
            {
                return 0;
            }
            else if (piece is Pawn)
            {
                return piece.IsWhite ? -10 : 10;
            }
            else if (piece is Rook)
            {
                return piece.IsWhite ? -50 : 50;
            }
            else if (piece is Knight)
            {
                return piece.IsWhite ? -30 : 30;
            }
            else if (piece is Bishop)
            {
                return piece.IsWhite ? -30 : 30;
            }
            else if (piece is Queen)
            {
                return piece.IsWhite ? -90 : 90;
            }
            else if (piece is King)
            {
                return piece.IsWhite ? -900 : 900;
            }

            return 0;
        }

        public void  DrawBoard()
        {
            for (int x = 0; x < Board.GetLength(0); x++)
            {
                for (int y = 0; y < Board.GetLength(1); y++)
                {
                    if (Board[x, y] != null)
                    {
                        Chessboard.Piece.Piece p = Board[y, x].GetPiece();

                        Console.Write(p != null ? p.ToString() : "_");
                    }
                        
                }
                Console.WriteLine();
            }
        }

        public void InitStartingBoard()
        {
            throw new NotImplementedException();
        }
    }
}
