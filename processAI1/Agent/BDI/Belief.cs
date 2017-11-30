using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Board;
using processAI1.Agent;
using processAI1.Board.Chessboard;
using processAI1.Board.Chessboard.Piece;

namespace processAI1
{
    public class Belief
    {
        ChessBoard _board;
        ChessBoard _fakeBoard;
        List<Board.Chessboard.Piece.Piece> _myPieces = new List<Board.Chessboard.Piece.Piece>();
        List<Board.Chessboard.Piece.Piece> _otherPieces = new List<Board.Chessboard.Piece.Piece>();
        public Boolean IsWhite;

        //Bolean for move
        public static Boolean FirstMoveOfKing;
        public static Boolean FirstMoveOfRightRook;
        public static Boolean FirstMoveOfLeftRook;

        public Belief()
        {
            _board = new ChessBoard();
            _fakeBoard = new ChessBoard();
            FirstMoveOfKing = true;
            FirstMoveOfRightRook = true;
            FirstMoveOfLeftRook = true;
            //this.isWhite = _isWhite;
        }
        
        public void Update(Sensor sensor)
        {
            IsWhite = sensor.Color == Color.White ? true : false;
            //Update of the board           
            _myPieces = sensor.GetPieces(IsWhite);
            _otherPieces = sensor.GetPieces(!IsWhite);           
            _board.UpdateChessBoard(_myPieces, _otherPieces);
            //Update of FakeBoard for MiniMax 
            _fakeBoard.UpdateChessBoard(_myPieces, _otherPieces);

        }
       
        public List<Move> GetPossibleMoves()
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    if (_board.GetCase(i, j).IsOccupied)
                        if (_board.GetCase(i, j).GetPiece().IsWhite /*&& board.getCase(i, j).ToString() is Pawn*/)
                            moves.AddRange(_board.GetCase(i, j).GetPiece().GetPossibleMoves(_board));
            }
            return moves;
        }
        public List<Move> GetPossibleMovesForAPlayer(Boolean isWhite)
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    if (_board.GetCase(i, j).IsOccupied)
                        if (_board.GetCase(i, j).GetPiece().IsWhite == isWhite)
                            moves.AddRange(_board.GetCase(i, j).GetPiece().GetPossibleMoves(_board));
            }
            return moves;
        }

        public Cell GetCase(int x, int y)
        {
            return _board.GetCase(x, y);
        }

        public Cell GetCase(Point p)
        {
            return _board.GetCase(p.GetX(), p.GetY());
        }
        public ChessBoard GetChessBoard()
        {
            return _board;
        }
        public ChessBoard GetFakeChessBoard()
        {
            return _fakeBoard;
        }

        public void UpdateFirstMove(Point oldPos)
        {

           if(GetCase(oldPos).GetPiece() is Rook)
            {
                //Right rook
                if (oldPos.GetX() == 0 && FirstMoveOfLeftRook)
                    FirstMoveOfLeftRook = false;

                //Left rook
                if (oldPos.GetX() == 7 && FirstMoveOfLeftRook)
                    FirstMoveOfLeftRook = false;
            }

            if (GetCase(oldPos).GetPiece() is King && FirstMoveOfKing)
                  FirstMoveOfKing = false;



            //Piece.Piece pieceToChange = myPieces.Find(piece => piece.getPosition().equal(oldPos));
            ////if (pieceToChange.firstMove())
            ////{
            ////    pieceToChange.setFirstMove(false);
            ////}
            //pieceToChange.setPosition(newPos);

            //board.updateChessBoard(myPieces, otherPieces);

            //remove Piece at oldPos
            //myPieces.RemoveAll(piece => piece.getPosition().equal(oldPos));

            //add Piece at new Pos
            // myPieces.Add(pieceToChange);

        }
    }
}
