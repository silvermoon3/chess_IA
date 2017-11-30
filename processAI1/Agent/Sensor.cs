using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Board.Bitboard;
using processAI1.Board.Chessboard.Piece;

namespace processAI1.Agent
{
    public enum Color { Black,White}

    public class Sensor
    {
        int[] _tabVal = new int[64];
        String[] _tabCoord = new string[] { "a8","b8","c8","d8","e8","f8","g8","h8",
                                           "a7","b7","c7","d7","e7","f7","g7","h7",
                                           "a6","b6","c6","d6","e6","f6","g6","h6",
                                           "a5","b5","c5","d5","e5","f5","g5","h5",
                                           "a4","b4","c4","d4","e4","f4","g4","h4",
                                           "a3","b3","c3","d3","e3","f3","g3","h3",
                                           "a2","b2","c2","d2","e2","f2","g2","h2",
                                           "a1","b1","c1","d1","e1","f1","g1","h1" };

        List<Board.Chessboard.Piece.Piece> _whitePieces = new List<Board.Chessboard.Piece.Piece>();
        List<Board.Chessboard.Piece.Piece> _blackPieces = new List<Board.Chessboard.Piece.Piece>();
        List<String> _reste = new List<String>();

        internal List<Board.Chessboard.Piece.Piece> GetPieces(Boolean isWhite)
        {
            if (isWhite)
                return _whitePieces;
            else
                return _blackPieces;
        }

        public Color Color = Color.White;
        public Sensor()
        {

        }



        public void ReadBoard(List<String> mesPieces, List<String> reste, int[] tabVal)
        {
            this._tabVal = tabVal;
            //first update of my pieces 
            //if (whitePieces.Count == 0)
            //{
            _whitePieces = new List<Board.Chessboard.Piece.Piece>();
                for (int i = 0; i < this._tabVal.Length; i++) 
                {
                    Point p = new Point(_tabCoord[i]);
                    Boolean isWhite = this._tabVal[i] > 0;
                    if (this._tabVal[i] == 21)
                        _whitePieces.Add(new Rook(p.GetX(), p.GetY(), isWhite));
                    else if (this._tabVal[i] == 31)
                        _whitePieces.Add(new Knight(p.GetX(), p.GetY(), isWhite));
                    else if (this._tabVal[i] == 4)
                        _whitePieces.Add(new Bishop(p.GetX(), p.GetY(), isWhite));
                    else if (this._tabVal[i] == 5)
                        _whitePieces.Add(new Queen(p.GetX(), p.GetY(), isWhite));
                    else if (this._tabVal[i] == 6)
                        _whitePieces.Add(new King(p.GetX(), p.GetY(), isWhite));
                    else if (this._tabVal[i] == 32)
                        _whitePieces.Add(new Knight(p.GetX(), p.GetY(), isWhite));
                    else if (this._tabVal[i] == 22)
                        _whitePieces.Add(new Rook(p.GetX(), p.GetY(), isWhite));
                    else if (this._tabVal[i] == 1)
                        _whitePieces.Add(new Pawn(p.GetX(), p.GetY(), isWhite));
                }
            //}
            //else if (mesPieces.Count != whitePieces.Count)
            //{
            //    Boolean somethingToRemove = false;
            //    Piece.Piece toRemove = null;
            //    //enlever pièces qui a été mangée 
            //    foreach (Piece.Piece piece in whitePieces)
            //    {
            //        if (!mesPieces.Contains(whitePieces.Find(p => p.getPosition().equal(piece.getPosition())).getPosition().ToString()))
            //        {
            //            somethingToRemove = true;
            //            toRemove = piece;
            //        }
            //    }
            //    if (somethingToRemove)
            //        whitePieces.Remove(toRemove);

            //}
            //Other piece 
            //if (blackPieces.Count == 0)
            //  {
            _blackPieces = new List<Board.Chessboard.Piece.Piece>();
            for (int i = 0; i < this._tabVal.Length; i++)
            {
                Point p = new Point(_tabCoord[i]);
                Boolean isWhite = this._tabVal[i] > 0;
                if (this._tabVal[i] == -21)
                    _blackPieces.Add(new Rook(p.GetX(), p.GetY(), isWhite));
                else if (this._tabVal[i] == -31)
                    _blackPieces.Add(new Knight(p.GetX(), p.GetY(), isWhite));
                else if (this._tabVal[i] == -4)
                    _blackPieces.Add(new Bishop(p.GetX(), p.GetY(), isWhite));
                else if (this._tabVal[i] == -5)
                    _blackPieces.Add(new Queen(p.GetX(), p.GetY(), isWhite));
                else if (this._tabVal[i] == -6)
                    _blackPieces.Add(new King(p.GetX(), p.GetY(), isWhite));
                else if (this._tabVal[i] == -32)
                    _blackPieces.Add(new Knight(p.GetX(), p.GetY(), isWhite));
                else if (this._tabVal[i] == -22)
                    _blackPieces.Add(new Rook(p.GetX(), p.GetY(), isWhite));
                else if (this._tabVal[i] == -1)
                    _blackPieces.Add(new Pawn(p.GetX(), p.GetY(), isWhite));

            }
            //}
            //else
            //{
            //    //Update only piece which have been moved 
            //}
            //Update of other pieces 
        }
        /*
        public void importFEN(string fenString)
        {
            int charIndex = 0;
            whitePieces = new List<Board.Chessboard.Piece.Piece>();
            blackPieces = new List<Board.Chessboard.Piece.Piece>();
            int boardIndex = 0;
            KeyValuePair<int, int> pair;
            Point p;
            while (fenString[charIndex] != ' ')
            {
                switch (fenString[charIndex++])
                {
                     
                    case 'P':
                        pair = ChessBoardConverter.CoordBitBoardToArray(boardIndex++);
                        p = new Point(pair.Key,pair.Value);
                        blackPieces.Add(new Pawn(p.getX(), p.getY(), false));
                        break;
                    case 'p':
                        pair = ChessBoardConverter.CoordBitBoardToArray(boardIndex++);
                        p = new Point(pair.Key, pair.Value);
                        whitePieces.Add(new Pawn(p.getX(), p.getY(), true));
                        break;
                    case 'N':
                        pair = ChessBoardConverter.CoordBitBoardToArray(boardIndex++);
                        p = new Point(pair.Key, pair.Value);
                        blackPieces.Add(new Knight(p.getX(), p.getY(), false));
                        break;
                    case 'n':
                        pair = ChessBoardConverter.CoordBitBoardToArray(boardIndex++);
                        p = new Point(pair.Key, pair.Value);
                        whitePieces.Add(new Knight(p.getX(), p.getY(), true));
                        break;
                    case 'B':
                        pair = ChessBoardConverter.CoordBitBoardToArray(boardIndex++);
                        p = new Point(pair.Key, pair.Value);
                        blackPieces.Add(new Bishop(p.getX(), p.getY(), false));
                        break;
                    case 'b':
                        pair = ChessBoardConverter.CoordBitBoardToArray(boardIndex++);
                        p = new Point(pair.Key, pair.Value);
                        whitePieces.Add(new Bishop(p.getX(), p.getY(), true));
                        break;
                    case 'R':
                        pair = ChessBoardConverter.CoordBitBoardToArray(boardIndex++);
                        p = new Point(pair.Key, pair.Value);
                        blackPieces.Add(new Rook(p.getX(), p.getY(), false));
                        break;
                    case 'r':
                        pair = ChessBoardConverter.CoordBitBoardToArray(boardIndex++);
                        p = new Point(pair.Key, pair.Value);
                        whitePieces.Add(new Rook(p.getX(), p.getY(), true));
                        break;
                    case 'Q':
                        pair = ChessBoardConverter.CoordBitBoardToArray(boardIndex++);
                        p = new Point(pair.Key, pair.Value);
                        blackPieces.Add(new Queen(p.getX(), p.getY(), false));
                        break;
                    case 'q':
                        pair = ChessBoardConverter.CoordBitBoardToArray(boardIndex++);
                        p = new Point(pair.Key, pair.Value);
                        whitePieces.Add(new Queen(p.getX(), p.getY(), true));
                        break;
                    case 'K':
                        pair = ChessBoardConverter.CoordBitBoardToArray(boardIndex++);
                        p = new Point(pair.Key, pair.Value);
                        blackPieces.Add(new King(p.getX(), p.getY(), false));
                        break;
                    case 'k':
                        pair = ChessBoardConverter.CoordBitBoardToArray(boardIndex++);
                        p = new Point(pair.Key, pair.Value);
                        whitePieces.Add(new King(p.getX(), p.getY(), true));
                        break;
                    case '/':
                        break;
                    case '1':
                        boardIndex++;
                        break;
                    case '2':
                        boardIndex += 2;
                        break;
                    case '3':
                        boardIndex += 3;
                        break;
                    case '4':
                        boardIndex += 4;
                        break;
                    case '5':
                        boardIndex += 5;
                        break;
                    case '6':
                        boardIndex += 6;
                        break;
                    case '7':
                        boardIndex += 7;
                        break;
                    case '8':
                        boardIndex += 8;
                        break;
                    default:
                        break;

                }   
            }
            _color = (fenString[++charIndex] == 'w') ? Color.White : Color.Black;

        }
        */
        public void GetNewBelief()
        {
            //New other piece 

            //New Board State
        }

    }
}
