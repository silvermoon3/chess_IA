using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Board.Chessboard;
using processAI1.Board.Chessboard.Piece;

namespace processAI1.Board
{
    class Rating
    {
       
        static int[][] _pawnBoard= {  //attribute to http://chessprogramming.wikispaces.com/Simplified+evaluation+function
            new int[]{ 0,  0,  0,  0,  0,  0,  0,  0},
            new int[]{50, 50, 50, 50, 50, 50, 50, 50},
            new int[]{10, 10, 20, 30, 30, 20, 10, 10},
            new int[]{ 5,  5, 10, 25, 25, 10,  5,  5},
            new int[]{ 0,  0,  0, 20, 20,  0,  0,  0},
            new int[]{ 5, -5,-10,  0,  0,-10, -5,  5},
            new int[]{ 5, 10, 10,-20,-20, 10, 10,  5},
            new int[]{ 0,  0,  0,  0,  0,  0,  0,  0}};

    static int[][] _rookBoard = {
            new int[]{ 0,  0,  0,  0,  0,  0,  0,  0},
            new int[]{ 5, 10, 10, 10, 10, 10, 10,  5},
            new int[] {-5,  0,  0,  0,  0,  0,  0, -5},
            new int[]{-5,  0,  0,  0,  0,  0,  0, -5},
            new int[]{-5,  0,  0,  0,  0,  0,  0, -5},
            new int[]{-5,  0,  0,  0,  0,  0,  0, -5},
            new int[]{-5,  0,  0,  0,  0,  0,  0, -5},
            new int[]{ 0,  0,  0,  5,  5,  0,  0,  0}};

    static int[][] _knightBoard={
            new int[]{-50,-40,-30,-30,-30,-30,-40,-50},
            new int[]{-40,-20,  0,  0,  0,  0,-20,-40},
            new int[]{-30,  0, 10, 15, 15, 10,  0,-30},
            new int[]{-30,  5, 15, 20, 20, 15,  5,-30},
            new int[]{-30,  0, 15, 20, 20, 15,  0,-30},
            new int[]{-30,  5, 10, 15, 15, 10,  5,-30},
            new int[]{-40,-20,  0,  5,  5,  0,-20,-40},
            new int[]{-50,-40,-30,-30,-30,-30,-40,-50}};

    static int[][] _bishopBoard ={
            new int[]{-20,-10,-10,-10,-10,-10,-10,-20},
            new int[]{-10,  0,  0,  0,  0,  0,  0,-10},
            new int[]{-10,  0,  5, 10, 10,  5,  0,-10},
            new int[]{-10,  5,  5, 10, 10,  5,  5,-10},
            new int[]{-10,  0, 10, 10, 10, 10,  0,-10},
            new int[]{-10, 10, 10, 10, 10, 10, 10,-10},
            new int[]{-10,  5,  0,  0,  0,  0,  5,-10},
            new int[]{-20,-10,-10,-10,-10,-10,-10,-20}};

    static int[][] _queenBoard ={
            new int[]{-20,-10,-10, -5, -5,-10,-10,-20},
            new int[]{-10,  0,  0,  0,  0,  0,  0,-10},
            new int[]{-10,  0,  5,  5,  5,  5,  0,-10},
            new int[]{ -5,  0,  5,  5,  5,  5,  0, -5},
            new int[]{  0,  0,  5,  5,  5,  5,  0, -5},
            new int[]{-10,  5,  5,  5,  5,  5,  0,-10},
            new int[]{-10,  0,  5,  0,  0,  0,  0,-10},
            new int[]{-20,-10,-10, -5, -5,-10,-10,-20}};

    static int[][] _kingMidBoard ={
            new int[]{-30,-40,-40,-50,-50,-40,-40,-30},
            new int[]{-30,-40,-40,-50,-50,-40,-40,-30},
            new int[]{-30,-40,-40,-50,-50,-40,-40,-30},
            new int[]{-30,-40,-40,-50,-50,-40,-40,-30},
            new int[]{-20,-30,-30,-40,-40,-30,-30,-20},
            new int[]{-10,-20,-20,-20,-20,-20,-20,-10},
            new int[]{ 20, 20,  0,  0,  0,  0, 20, 20},
            new int[]{ 20, 30, 10,  0,  0, 10, 30, 20}};

    static int[][] _kingEndBoard ={
            new int[]{-50,-40,-30,-20,-20,-30,-40,-50},
            new int[]{-30,-20,-10,  0,  0,-10,-20,-30},
            new int[]{-30,-10, 20, 30, 30, 20,-10,-30},
            new int[]{-30,-10, 30, 40, 40, 30,-10,-30},
            new int[]{-30,-10, 30, 40, 40, 30,-10,-30},
            new int[]{-30,-10, 20, 30, 30, 20,-10,-30},
            new int[]{-30,-30,  0,  0,  0,  0,-30,-30},
            new int[]{-50,-30,-30,-30,-30,-30,-30,-50}};

    public static int RateBoard(ChessBoard b, Boolean isWhiteTurn)
        {
            int result = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (isWhiteTurn)
                    {
                        if (b.IsOccupied(i, j))
                        { //checks if board square has a piece
                            Chessboard.Piece.Piece p = b.GetCase(i, j).GetPiece();
                            if (p.IsWhite)
                            {
                                if (p is Pawn)
                                {
                                    result += _pawnBoard[i][j];
                                }
                                else if (p is Rook)
                                {
                                    result += _rookBoard[i][j];
                                }
                                else if (p is Bishop)
                                {
                                    result += _bishopBoard[i][j];
                                }
                                else if (p is Knight)
                                {
                                    result += _knightBoard[i][j];
                                }
                                else if (p is Queen)
                                {
                                    result += _queenBoard[i][j];
                                }
                                else if (p is King)
                                {
                                    result += _kingMidBoard[i][j];
                                }
                            }
                        }
                    }
                    else
                    {
                        if (b.IsOccupied(i, j))
                        { //checks if board square has a piece
                            Chessboard.Piece.Piece p = b.GetCase(i, j).GetPiece();
                            if (!p.IsWhite)
                            {
                                if (p is Pawn)
                                {
                                    result += _pawnBoard[7 - i][7 - j];
                                }
                                else if (p is Rook)
                                {
                                    result += _rookBoard[7 - i][7 - j];
                                }
                                else if (p is Bishop)
                                {
                                    result += _bishopBoard[7 - i][7 - j];
                                }
                                else if (p is Knight)
                                {
                                    result += _knightBoard[7 - i][7 - j];
                                }
                                else if (p is Queen)
                                {
                                    result += _queenBoard[7 - i][7 - j];
                                }
                                else if (p is King)
                                {
                                    result += _kingMidBoard[7 - i][7 - j];
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
