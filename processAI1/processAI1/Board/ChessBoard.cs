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
        List<Piece.Piece> myPieces = new List<Piece.Piece>();
        List<Piece.Piece> otherPieces = new List<Piece.Piece>();
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
            Piece.Piece currentPiece = board[_move.getInitialPosition().getX(), _move.getInitialPosition().getY()].getPiece();
            board[_move.getFinalPosition().getX(), _move.getFinalPosition().getY()].setPiece(currentPiece);
            board[_move.getFinalPosition().getX(), _move.getFinalPosition().getY()].getPiece().setPosition(_move.getFinalPosition());
            board[_move.getInitialPosition().getX(), _move.getInitialPosition().getY()] = new Cell(null, _move.getInitialPosition());
        }

        public void undoMove(Move _move)
        {
            Piece.Piece currentPiece = board[_move.getFinalPosition().getX(), _move.getFinalPosition().getY()].getPiece();
            board[_move.getInitialPosition().getX(), _move.getInitialPosition().getY()].setPiece(currentPiece);
            board[_move.getInitialPosition().getX(), _move.getInitialPosition().getY()].getPiece().setPosition(_move.getInitialPosition());
            board[_move.getFinalPosition().getX(), _move.getFinalPosition().getY()] = new Cell(null, _move.getFinalPosition());
        }

        public void updateChessBoard(List<Piece.Piece> _myPieces , List<Piece.Piece> _otherPieces)
        {
            myPieces = _myPieces;
            otherPieces = _otherPieces;
            updateBoard();
        }
        public Cell getCase(int x, int y)
        {
            return board[x, y];
        }

        public void updateBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    board[i, j] = new Cell(null, new Point(i, j));
            }

            foreach (Piece.Piece piece in myPieces)
            {
                board[piece.getPosition().getX(), piece.getPosition().getY()] = new Cell(piece, piece.getPosition());
            }

            foreach (Piece.Piece piece in otherPieces)
            {
                board[piece.getPosition().getX(), piece.getPosition().getY()] = new Cell(piece, piece.getPosition());
            }
        }

        public Boolean isOccupied(Point p)
        {
            if(p.validPosition())
                return board[p.getX(), p.getY()].isOccupied;
            return false;
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
        public List<Move> getAllMovesForCurrentPlayer(Boolean _isWhite = true)
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

        public Boolean amIInCheck()
        {
            Point myKingPosition = myPieces.Find(p => p is King).getPosition();
            foreach(Move move in getAllMovesForCurrentPlayer(false))
            {
                if (move.getFinalPosition().equal(myKingPosition))
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
                    if (isOccupied(i, j))
                        Console.Write(getCase(i, j).getPiece().getPiece());
                    else
                        Console.Write("x");

                    Console.Write("|");
                    if (i == 7) Console.Write("\n");
                }
              
            }

        }

        public int evaluate(int evaluateOption, Boolean isWhiteTurn)
        {        
            int result = 0;
            switch (evaluateOption)
            {
                case 0:
                    result = evaluateMovability(isWhiteTurn);
                    break;
                case 1:
                    result = evaluatePositions();
                    break;
                case 2:
                    result = evaluateMaterial(isWhiteTurn) + evaluateMovability(isWhiteTurn);
                    break;
                case 3:
                    result = evaluateMaterial(isWhiteTurn) + evaluatePositions();
                    break;
            }
            return result;
        }
        private int evaluateMaterial(Boolean isWhiteTurn)
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
                    if (isOccupied(i, j))
                    {

                        Piece.Piece pieceToCheck = getCase(i, j).getPiece();

                        if (pieceToCheck is Piece.Pawn)
                        {
                            score = 100;
                            if (pieceToCheck.isWhite)
                            {
                                whiteScore += score;
                             
                            }
                            else
                            {
                                blackScore += score;
                            }
                           
                        }
                        else if (pieceToCheck is Piece.Rook)
                        {
                            score = 500;
                            if (pieceToCheck.isWhite)
                            {
                                whiteScore += score;
                               
                            }
                            else
                            {
                                blackScore += score;
                            }
                            
                        }
                        else if (pieceToCheck is Piece.Bishop)
                        {
                            score = 330;
                            if (pieceToCheck.isWhite)
                            {
                                whiteScore += score;
                            }
                            else
                            {
                                blackScore += score;                               
                            }
                            
                        }
                        else if (pieceToCheck is Piece.Knight)
                        {
                            score = 320;
                            if (pieceToCheck.isWhite)
                            {
                                whiteScore += score;
                            }
                            else
                            {
                                blackScore += score;
                            }
                           
                        }
                        else if (pieceToCheck is Piece.Queen)
                        {
                            score = 900;
                            if (pieceToCheck.isWhite)
                            {
                                whiteScore += score;
                            }
                            else
                            {
                                blackScore += score;
                            }
                           
                        }
                        else if (pieceToCheck is Piece.King)
                        {
                            score = 20000;
                            if (pieceToCheck.isWhite)
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

        private int evaluatePositions()
        {
            int thisScore = Rating.rateBoard(this, true);
            int otherScore = Rating.rateBoard(this, false);

            return thisScore - otherScore;
        }

        private int evaluateMovability(Boolean isWhiteTurn)
        {
            int thisScore = getAllMovesForCurrentPlayer().Count();
            isWhiteTurn = !isWhiteTurn;
            int otherScore = getAllMovesForCurrentPlayer().Count();
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
                     score = score + getPieceValue(board[i,j].getPiece());
                }
            }
          
            return score;
        }
        private int getPieceValue(Piece.Piece _piece)
        {
            if (_piece == null)
            {
                return 0;
            }
            else if (_piece is Pawn)
            {
                return _piece.isWhite ? -10 : 10;
            }
            else if (_piece is Rook)
            {
                return _piece.isWhite ? -50 : 50;
            }
            else if (_piece is Knight)
            {
                return _piece.isWhite ? -30 : 30;
            }
            else if (_piece is Bishop)
            {
                return _piece.isWhite ? -30 : 30;
            }
            else if (_piece is Queen)
            {
                return _piece.isWhite ? -90 : 90;
            }
            else if (_piece is King)
            {
                return _piece.isWhite ? -900 : 900;
            }

            return 0;
        }
    }
}
