using System;
using System.Collections.Generic;
using processAI1.Board.Bitboard.MoveType;
using processAI1.Board.Chessboard.Piece;

namespace processAI1.Board.Bitboard
{
    
    public class BitBoard : IChessBoard
    {
        public struct Coord
        {
            public int X, Y;
            public Coord(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        public enum ChessSquare
        {
            A1, B1, C1, D1, E1, F1, G1, H1,
            A2, B2, C2, D2, E2, F2, G2, H2,
            A3, B3, C3, D3, E3, F3, G3, H3,
            A4, B4, C4, D4, E4, F4, G4, H4,
            A5, B5, C5, D5, E5, F5, G5, H5,
            A6, B6, C6, D6, E6, F6, G6, H6,
            A7, B7, C7, D7, E7, F7, G7, H7,
            A8, B8, C8, D8, E8, F8, G8, H8
        };
        
        // get enum from bitPos coord[bitPos] bitpos = 0 ... 63
        public readonly Coord[] GetCoord = new Coord[]
        {
            new Coord(x: 0,y: 0),new Coord(x: 1,y: 0),new Coord(x: 2,y: 0),new Coord(x: 3,y: 0),new Coord(x: 4,y: 0),new Coord(x: 5,y: 0),new Coord(x: 6,y: 0),new Coord(x: 7,y: 0),
            new Coord(x: 0,y: 1),new Coord(x: 1,y: 1),new Coord(x: 2,y: 1),new Coord(x: 3,y: 1),new Coord(x: 4,y: 1),new Coord(x: 5,y: 1),new Coord(x: 6,y: 1),new Coord(x: 7,y: 1),
            new Coord(x: 0,y: 2),new Coord(x: 1,y: 2),new Coord(x: 2,y: 2),new Coord(x: 3,y: 2),new Coord(x: 4,y: 2),new Coord(x: 5,y: 2),new Coord(x: 6,y: 2),new Coord(x: 7,y: 2),
            new Coord(x: 0,y: 3),new Coord(x: 1,y: 3),new Coord(x: 2,y: 3),new Coord(x: 3,y: 3),new Coord(x: 4,y: 3),new Coord(x: 5,y: 3),new Coord(x: 6,y: 3),new Coord(x: 7,y: 3),
            new Coord(x: 0,y: 4),new Coord(x: 1,y: 4),new Coord(x: 2,y: 4),new Coord(x: 3,y: 4),new Coord(x: 4,y: 4),new Coord(x: 5,y: 4),new Coord(x: 6,y: 4),new Coord(x: 7,y: 4),
            new Coord(x: 0,y: 5),new Coord(x: 1,y: 5),new Coord(x: 2,y: 5),new Coord(x: 3,y: 5),new Coord(x: 4,y: 5),new Coord(x: 5,y: 5),new Coord(x: 6,y: 5),new Coord(x: 7,y: 5),
            new Coord(x: 0,y: 6),new Coord(x: 1,y: 6),new Coord(x: 2,y: 6),new Coord(x: 3,y: 6),new Coord(x: 4,y: 6),new Coord(x: 5,y: 6),new Coord(x: 6,y: 6),new Coord(x: 7,y: 6),
            new Coord(x: 0,y: 7),new Coord(x: 1,y: 7),new Coord(x: 2,y: 7),new Coord(x: 3,y: 7),new Coord(x: 4,y: 7),new Coord(x: 5,y: 7),new Coord(x: 6,y: 7),new Coord(x: 7,y: 7)
        };

        //ChessSquare[y][x] 
        private readonly ChessSquare[][] _arrayToChess = new[]
        {
            new[]{ChessSquare.A1, ChessSquare.B1, ChessSquare.C1, ChessSquare.D1, ChessSquare.E1, ChessSquare.F1, ChessSquare.G1, ChessSquare.H1},
            new[]{ChessSquare.A2, ChessSquare.B2, ChessSquare.C2, ChessSquare.D2, ChessSquare.E2, ChessSquare.F2, ChessSquare.G2, ChessSquare.H2},
            new[]{ChessSquare.A3, ChessSquare.B3, ChessSquare.C3, ChessSquare.D3, ChessSquare.E3, ChessSquare.F3, ChessSquare.G3, ChessSquare.H3},
            new[]{ChessSquare.A4, ChessSquare.B4, ChessSquare.C4, ChessSquare.D4, ChessSquare.E4, ChessSquare.F4, ChessSquare.G4, ChessSquare.H4},
            new[]{ChessSquare.A5, ChessSquare.B5, ChessSquare.C5, ChessSquare.D5, ChessSquare.E5, ChessSquare.F5, ChessSquare.G5, ChessSquare.H5},
            new[]{ChessSquare.A6, ChessSquare.B6, ChessSquare.C6, ChessSquare.D6, ChessSquare.E6, ChessSquare.F6, ChessSquare.G6, ChessSquare.H6},
            new[]{ChessSquare.A7, ChessSquare.B7, ChessSquare.C7, ChessSquare.D7, ChessSquare.E7, ChessSquare.F7, ChessSquare.G7, ChessSquare.H7},
            new[]{ChessSquare.A8, ChessSquare.B8, ChessSquare.C8, ChessSquare.D8, ChessSquare.E8, ChessSquare.F8, ChessSquare.G8, ChessSquare.H8}};

        
        //WHITES
        public ulong WhiteKing { get; private set; }
        public ulong WhiteQueens { get; private set; }
        public ulong WhiteRooks { get; private set; }
        public ulong WhiteBishops { get; private set; }
        public ulong WhiteKnights { get; private set; }
        public ulong WhitePawns { get; private set; }
        
        //blacks
        public ulong BlackKing { get; private set; }
        public ulong BlackQueens { get; private set; }
        public ulong BlackRooks { get; private set; }
        public ulong BlackBishops { get; private set; }
        public ulong BlackKnights { get; private set; }
        public ulong BlackPawns { get; private set; }

        public ulong EnPassant { get; private set; }
        //store function for moving pieces
        public readonly BitMove Move;


        public BitBoard()
        {
            Move = new PawnsMove(this);
        }

        private readonly char[][] _chessBoardStarter = new[]
        {
            new[]{'R','N','B','Q','K','B','N','R'},
            new[]{'P','P','P','P','P','P','P','P'},
            new[]{' ',' ',' ',' ',' ',' ',' ',' '},
            new[]{' ',' ',' ',' ',' ',' ',' ',' '},
            new[]{' ',' ',' ',' ',' ',' ',' ',' '},
            new[]{' ',' ',' ',' ',' ',' ',' ',' '},
            new[]{'p','p','p','p','p','p','p','p'},
            new[]{'r','n','b','q','k','b','n','r'}};
        
        public void InitStartingBoard()
        {

            this.ArrayToBitboards(chessBoard: _chessBoardStarter);
        }

        public void MakeMove(Move move)
        {
            throw new NotImplementedException();
        }

        public void UndoMove(Move move)
        {
            throw new NotImplementedException();
        }

        public void UpdateChessBoard(List<Piece> myPieces, List<Piece> otherPieces)
        {
            throw new NotImplementedException();
        }

        public void UpdateBoard()
        {
            throw new NotImplementedException();
        }

        public bool IsOccupied(Point p)
        {
            throw new NotImplementedException();
        }

        public bool IsOccupied(int x, int y)
        {
            throw new NotImplementedException();
        }

        public bool IsOccupiedWithMyPiece(Point p, bool isWhite)
        {
            throw new NotImplementedException();
        }

        public List<Move> GetAllPossibleMoves(bool isWhite = true)
        {
            throw new NotImplementedException();
        }

        public List<Move> GetAllMovesForCurrentPlayer(bool isWhite = true)
        {
            throw new NotImplementedException();
        }

        public bool AmIInCheck()
        {
            throw new NotImplementedException();
        }

        public int Evaluate(int evaluateOption, bool isWhiteTurn)
        {
            throw new NotImplementedException();
        }

        public int EvaluateBoardWithPieceValue()
        {
            throw new NotImplementedException();
        }

        public ulong GetWhite()
        {
            return WhiteBishops | WhiteKing | WhiteKnights | WhitePawns | WhiteQueens | WhiteRooks;
        }
        public ulong GetBlack()
        {
            return BlackBishops | BlackKing | BlackKnights | BlackPawns | BlackQueens | BlackRooks;
        }

        public ulong GetOther(Boolean isWhite)
        {
            if (!isWhite)
                return GetWhite();
            else
                return GetBlack();
        }

        public ulong GetOccupied()
        {
            return GetBlack() | GetWhite();
        }
        public ulong GetEmpty()
        {
            return ~GetOccupied();
        }
        
        public void ArrayToBitboards(char[][] chessBoard)
        {

            for (int x = 0; x< 8; x++)
            {
                for (int y = 0; y< 8; y++)
                {
                    int bitBoardCoord = (int) this._arrayToChess[y][x];

                    switch (chessBoard[y][x])
                    {
                        case 'R':
                            WhiteRooks |= 1UL << bitBoardCoord;
                            break;
                        case 'N':
                            WhiteKnights |= 1UL << bitBoardCoord;
                            break;
                        case 'B':
                            WhiteBishops |= 1UL << bitBoardCoord;
                            break;
                        case 'K':
                            WhiteKing |= 1UL << bitBoardCoord;
                            break;
                        case 'Q':
                            WhiteQueens |= 1UL << bitBoardCoord;
                            break;
                        case 'P':
                            WhitePawns |= 1UL << bitBoardCoord;
                            break;
                        case 'r':
                            BlackRooks |= 1UL << bitBoardCoord;
                            break;
                        case 'n':
                            BlackKnights |= 1UL << bitBoardCoord;
                            break;
                        case 'b':
                            BlackBishops |= 1UL << bitBoardCoord;
                            break;
                        case 'k':
                            BlackKing |= 1UL << bitBoardCoord;
                            break;
                        case 'q':
                            BlackQueens |= 1UL << bitBoardCoord;
                            break;
                        case 'p':
                            BlackPawns |= 1UL << bitBoardCoord;
                            break;
                    }
                }
            }
        }

        public override string ToString()
        {
            string chessBoardString = "";
            for (int y = 7; y >= 0; y--)
                {

                    for (int x = 0; x < 8; x++)
                    {
                    int pos = (int)this._arrayToChess[y][x];
                    char tempSquare = ' ';

                    if (((WhitePawns >> pos) & 1) == 1) { tempSquare = 'P'; }
                    if (((WhiteKnights >> pos) & 1) == 1) { tempSquare = 'N'; }
                    if (((WhiteBishops >> pos) & 1) == 1) { tempSquare = 'B'; }
                    if (((WhiteRooks >> pos) & 1) == 1) { tempSquare = 'R'; }
                    if (((WhiteQueens >> pos) & 1) == 1) { tempSquare = 'Q'; }
                    if (((WhiteKing >> pos) & 1) == 1) { tempSquare = 'K'; }
                    if (((BlackPawns >> pos) & 1) == 1) { tempSquare = 'p'; }
                    if (((BlackKnights >> pos) & 1) == 1) { tempSquare = 'n'; }
                    if (((BlackBishops >> pos) & 1) == 1) { tempSquare = 'b'; }
                    if (((BlackRooks >> pos) & 1) == 1) { tempSquare = 'r'; }
                    if (((BlackQueens >> pos) & 1) == 1) { tempSquare = 'q'; }
                    if (((BlackKing >> pos) & 1) == 1) { tempSquare = 'k'; }

                    chessBoardString += "[" + tempSquare+ "]";

                }
                    chessBoardString += "\n";

            }

            return chessBoardString;
        }


    }
}
