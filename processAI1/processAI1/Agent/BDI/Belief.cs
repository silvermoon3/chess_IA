using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Piece;
using processAI1.Board;
using processAI1.Agent;

namespace processAI1
{
    public class Belief
    {
        Board.ChessBoard board;
        Board.ChessBoard fakeBoard;
        List<Piece.Piece> myPieces = new List<Piece.Piece>();
        List<Piece.Piece> otherPieces = new List<Piece.Piece>();
        Boolean isWhite;

        //Bolean for move
        public static Boolean firstMoveOfKing;
        public static Boolean firstMoveOfRightRook;
        public static Boolean firstMoveOfLeftRook;

        public Belief()
        {
            board = new ChessBoard();
            fakeBoard = new ChessBoard();
            firstMoveOfKing = true;
            firstMoveOfRightRook = true;
            firstMoveOfLeftRook = true;
            //this.isWhite = _isWhite;
        }
        
        public void Update(Sensor sensor)
        {
            //Update of the board           
            myPieces = sensor.getMyPieces();
            otherPieces = sensor.getOtherPieces();           
            board.updateChessBoard(myPieces, otherPieces);
            //Update of FakeBoard for MiniMax 
            fakeBoard.updateChessBoard(myPieces, otherPieces);

        }
       
        public List<Move> getPossibleMoves()
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    if (board.getCase(i, j).isOccupied)
                        if (board.getCase(i, j).getPiece().isWhite /*&& board.getCase(i, j).getPiece() is Pawn*/)
                            moves.AddRange(board.getCase(i, j).getPiece().getPossibleMoves(board));
            }
            return moves;
        }
        public List<Move> getPossibleMovesForAPlayer(Boolean isWhite)
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    if (board.getCase(i, j).isOccupied)
                        if (board.getCase(i, j).getPiece().isWhite == isWhite)
                            moves.AddRange(board.getCase(i, j).getPiece().getPossibleMoves(board));
            }
            return moves;
        }

        public Cell getCase(int x, int y)
        {
            return board.getCase(x, y);
        }

        public Cell getCase(Point p)
        {
            return board.getCase(p.getX(), p.getY());
        }
        public ChessBoard getChessBoard()
        {
            return board;
        }
        public ChessBoard getFakeChessBoard()
        {
            return fakeBoard;
        }

        public void updateFirstMove(Point oldPos)
        {

           if(getCase(oldPos).getPiece() is Rook)
            {
                //Right rook
                if (oldPos.getX() == 0 && firstMoveOfLeftRook)
                    firstMoveOfLeftRook = false;

                //Left rook
                if (oldPos.getX() == 7 && firstMoveOfLeftRook)
                    firstMoveOfLeftRook = false;
            }

            if (getCase(oldPos).getPiece() is King && firstMoveOfKing)
                  firstMoveOfKing = false;



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
