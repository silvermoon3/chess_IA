using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Piece;
using processAI1.Board;

namespace processAI1
{
    public class Belief
    {
        Board.ChessBoard board;
        List<Piece.Piece> myPieces = new List<Piece.Piece>();
        List<Piece.Piece> otherPieces = new List<Piece.Piece>();

        public Belief()
        {
            board = new ChessBoard();

        }

      

        public void Update(Effector effector)
        {
            if (myPieces.Count == 0)
                myPieces = effector.getMyPieces();
            otherPieces = effector.getOtherPieces();           
            board.updateChessBoard(myPieces, otherPieces);

        }
       
        public List<Move> getPossibleMoves()
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    if (board.getCase(i, j).isOccupied)
                        if (board.getCase(i, j).getPiece().isWhite && board.getCase(i, j).getPiece() is Knight)
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

        public void updatePiecePosition(Point newPos, Point oldPos)
        {
            Piece.Piece pieceToChange = myPieces.Find(piece => piece.getPosition().equal(oldPos));
            if (pieceToChange.firstMove())
            {
                pieceToChange.setFirstMove(false);
            }
            pieceToChange.setPosition(newPos);

            board.updateChessBoard(myPieces, otherPieces);

            //remove Piece at oldPos
            //myPieces.RemoveAll(piece => piece.getPosition().equal(oldPos));

            //add Piece at new Pos
            // myPieces.Add(pieceToChange);

        }
    }
}
