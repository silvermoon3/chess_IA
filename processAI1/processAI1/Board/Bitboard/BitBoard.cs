using System;

namespace processAI1.Board.Bitboard
{
    
    public class BitBoard : IChessBoard
    {
        
        //WHITES
        private UInt64 _whiteKing=0;
        private UInt64 _whiteQueens=0;
        private UInt64 _whiteRooks=0;
        private UInt64 _whiteBishops=0;
        private UInt64 _whiteKnights=0;
        private UInt64 _whitePawns=0;

        //blacks
        private UInt64 _blackKing = 0;
        private UInt64 _blackQueens = 0;
        private UInt64 _blackRooks = 0;
        private UInt64 _blackBishops = 0;
        private UInt64 _blackKnights = 0;
        private UInt64 _blackPawns = 0;



        private BitMove _move = new BitMove();

        char[][] chessBoardStarter = new char[][]{
            new char[]{'R','N','B','Q','K','B','N','R'},
            new char[]{'P','P','P','P','P','P','P','P'},
            new char[]{' ',' ',' ',' ',' ',' ',' ',' '},
            new char[]{' ',' ',' ',' ',' ',' ',' ',' '},
            new char[]{' ',' ',' ',' ',' ',' ',' ',' '},
            new char[]{' ',' ',' ',' ',' ',' ',' ',' '},
            new char[]{'p','p','p','p','p','p','p','p'},
            new char[]{'r','n','b','q','k','b','n','r'}};


        public void InitStartingBoard()
        {

            this.ArrayToBitboards(chessBoardStarter);
            UInt64 empty = _whiteBishops | _whiteKing | _whiteKnights | _whitePawns | _whiteQueens | _whiteRooks;
            empty |= _blackBishops | _blackKing | _blackKnights | _blackPawns | _blackQueens | _blackRooks;
            empty = ~empty;
            this.DrawArray();
            _whitePawns =  _move.WSinglePushTargets(_whitePawns,empty);
            this.DrawArray();
        }

        public void AllPossibleMoves()
        {
            throw new NotImplementedException();
        }

        public void DrawBoard()
        {
            throw new NotImplementedException();
        }
        
        public void MakeMove()
        {
            throw new NotImplementedException();
        }

        public void PossibleMovesForPiece(string piecePosition)
        {
            throw new NotImplementedException();
        }


        

        public void ArrayToBitboards(char[][] chessBoard)
        {

            for (int x = 0; x< 8; x++)
            {
                for (int y = 0; y< 8; y++)
                {
                    int bitBoardCoord = ChessBoardConverter.CoordArrayToBitBoard(x,y);

                    switch (chessBoard[x][y])
                    {
                            
                        case 'R': //White rook
                            _whiteRooks |= 1UL << bitBoardCoord;
                            break;
                        case 'N': //White knight
                            _whiteKnights |= 1UL << bitBoardCoord;
                            break;
                        case 'B': //White Bishop
                            _whiteBishops |= 1UL << bitBoardCoord;
                            break;
                        case 'K': //White King
                            _whiteKing |= 1UL << bitBoardCoord;
                            break;
                        case 'Q': //White queen
                            _whiteQueens |= 1UL << bitBoardCoord;
                            break;
                        case 'P': //White pawn
                            _whitePawns |= 1UL << bitBoardCoord;
                            break;
                        case 'r': //White rook
                            _blackRooks |= 1UL << bitBoardCoord;
                            break;
                        case 'n': //White knight
                            _blackKnights |= 1UL << bitBoardCoord;
                            break;
                        case 'b': //White Bishop
                            _blackBishops |= 1UL << bitBoardCoord;
                            break;
                        case 'k': //White King
                            _blackKing |= 1UL << bitBoardCoord;
                            break;
                        case 'q': //White queen
                            _blackQueens |= 1UL << bitBoardCoord;
                            break;
                        case 'p': //White pawn
                            _blackPawns |= 1UL << bitBoardCoord;
                            break;

                    }
                }
            }
        }


        public void DrawArray()
        {
            char[][] chessBoard =new char[8][];

            for (int x = 0; x < 8; x++)
            {
                chessBoard[x]= new char[8];

                for (int y = 0; y < 8; y++)
                {
                    int pos =  ChessBoardConverter.CoordArrayToBitBoard(x,y);
                    chessBoard[x][y] = '_';

                    if (((_whitePawns >> pos) & 1) == 1) { chessBoard[x][y] = 'P'; }
                    if (((_whiteKnights >> pos) & 1) == 1) { chessBoard[x][y] = 'N'; }
                    if (((_whiteBishops >> pos) & 1) == 1) { chessBoard[x][y] = 'B'; }
                    if (((_whiteRooks >> pos) & 1) == 1) { chessBoard[x][y] = 'R'; }
                    if (((_whiteQueens >> pos) & 1) == 1) { chessBoard[x][y] = 'Q'; }
                    if (((_whiteKing >> pos) & 1) == 1) { chessBoard[x][y] = 'K'; }
                    if (((_blackPawns >> pos) & 1) == 1) { chessBoard[x][y] = 'p'; }
                    if (((_blackKnights >> pos) & 1) == 1) { chessBoard[x][y] = 'n'; }
                    if (((_blackBishops >> pos) & 1) == 1) { chessBoard[x][y] = 'b'; }
                    if (((_blackRooks >> pos) & 1) == 1) { chessBoard[x][y] = 'r'; }
                    if (((_blackQueens >> pos) & 1) == 1) { chessBoard[x][y] = 'q'; }
                    if (((_blackKing >> pos) & 1) == 1) { chessBoard[x][y] = 'k'; }
                    
                }

            }
            for (int i=7;i>=0;i--) {
                Console.WriteLine(chessBoard[i]);
            }
            Console.WriteLine("");
        }

       
    }
}
