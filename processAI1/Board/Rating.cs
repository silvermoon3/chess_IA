using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Board
{
    class Rating
    {
       
        static int[][] pawnBoard= {  //attribute to http://chessprogramming.wikispaces.com/Simplified+evaluation+function
            new int[]{ 0,  0,  0,  0,  0,  0,  0,  0},
            new int[]{50, 50, 50, 50, 50, 50, 50, 50},
            new int[]{10, 10, 20, 30, 30, 20, 10, 10},
            new int[]{ 5,  5, 10, 25, 25, 10,  5,  5},
            new int[]{ 0,  0,  0, 20, 20,  0,  0,  0},
            new int[]{ 5, -5,-10,  0,  0,-10, -5,  5},
            new int[]{ 5, 10, 10,-20,-20, 10, 10,  5},
            new int[]{ 0,  0,  0,  0,  0,  0,  0,  0}};

    static int[][] rookBoard = {
            new int[]{ 0,  0,  0,  0,  0,  0,  0,  0},
            new int[]{ 5, 10, 10, 10, 10, 10, 10,  5},
            new int[] {-5,  0,  0,  0,  0,  0,  0, -5},
            new int[]{-5,  0,  0,  0,  0,  0,  0, -5},
            new int[]{-5,  0,  0,  0,  0,  0,  0, -5},
            new int[]{-5,  0,  0,  0,  0,  0,  0, -5},
            new int[]{-5,  0,  0,  0,  0,  0,  0, -5},
            new int[]{ 0,  0,  0,  5,  5,  0,  0,  0}};

    static int[][] knightBoard={
            new int[]{-50,-40,-30,-30,-30,-30,-40,-50},
            new int[]{-40,-20,  0,  0,  0,  0,-20,-40},
            new int[]{-30,  0, 10, 15, 15, 10,  0,-30},
            new int[]{-30,  5, 15, 20, 20, 15,  5,-30},
            new int[]{-30,  0, 15, 20, 20, 15,  0,-30},
            new int[]{-30,  5, 10, 15, 15, 10,  5,-30},
            new int[]{-40,-20,  0,  5,  5,  0,-20,-40},
            new int[]{-50,-40,-30,-30,-30,-30,-40,-50}};

    static int[][] bishopBoard ={
            new int[]{-20,-10,-10,-10,-10,-10,-10,-20},
            new int[]{-10,  0,  0,  0,  0,  0,  0,-10},
            new int[]{-10,  0,  5, 10, 10,  5,  0,-10},
            new int[]{-10,  5,  5, 10, 10,  5,  5,-10},
            new int[]{-10,  0, 10, 10, 10, 10,  0,-10},
            new int[]{-10, 10, 10, 10, 10, 10, 10,-10},
            new int[]{-10,  5,  0,  0,  0,  0,  5,-10},
            new int[]{-20,-10,-10,-10,-10,-10,-10,-20}};

    static int[][] queenBoard ={
            new int[]{-20,-10,-10, -5, -5,-10,-10,-20},
            new int[]{-10,  0,  0,  0,  0,  0,  0,-10},
            new int[]{-10,  0,  5,  5,  5,  5,  0,-10},
            new int[]{ -5,  0,  5,  5,  5,  5,  0, -5},
            new int[]{  0,  0,  5,  5,  5,  5,  0, -5},
            new int[]{-10,  5,  5,  5,  5,  5,  0,-10},
            new int[]{-10,  0,  5,  0,  0,  0,  0,-10},
            new int[]{-20,-10,-10, -5, -5,-10,-10,-20}};

    static int[][] kingMidBoard ={
            new int[]{-30,-40,-40,-50,-50,-40,-40,-30},
            new int[]{-30,-40,-40,-50,-50,-40,-40,-30},
            new int[]{-30,-40,-40,-50,-50,-40,-40,-30},
            new int[]{-30,-40,-40,-50,-50,-40,-40,-30},
            new int[]{-20,-30,-30,-40,-40,-30,-30,-20},
            new int[]{-10,-20,-20,-20,-20,-20,-20,-10},
            new int[]{ 20, 20,  0,  0,  0,  0, 20, 20},
            new int[]{ 20, 30, 10,  0,  0, 10, 30, 20}};

    static int[][] kingEndBoard ={
            new int[]{-50,-40,-30,-20,-20,-30,-40,-50},
            new int[]{-30,-20,-10,  0,  0,-10,-20,-30},
            new int[]{-30,-10, 20, 30, 30, 20,-10,-30},
            new int[]{-30,-10, 30, 40, 40, 30,-10,-30},
            new int[]{-30,-10, 30, 40, 40, 30,-10,-30},
            new int[]{-30,-10, 20, 30, 30, 20,-10,-30},
            new int[]{-30,-30,  0,  0,  0,  0,-30,-30},
            new int[]{-50,-30,-30,-30,-30,-30,-30,-50}};

    public static int rateBoard(ChessBoard b, Boolean isWhiteTurn)
        {
            int result = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (isWhiteTurn)
                    {
                        if (b.isOccupied(i, j))
                        { //checks if board square has a piece
                            Piece.Piece p = b.getCase(i, j).getPiece();
                            if (p.isWhite)
                            {
                                if (p is Piece.Pawn)
                                {
                                    result += pawnBoard[i][j];
                                }
                                else if (p is Piece.Rook)
                                {
                                    result += rookBoard[i][j];
                                }
                                else if (p is Piece.Bishop)
                                {
                                    result += bishopBoard[i][j];
                                }
                                else if (p is Piece.Knight)
                                {
                                    result += knightBoard[i][j];
                                }
                                else if (p is Piece.Queen)
                                {
                                    result += queenBoard[i][j];
                                }
                                else if (p is Piece.King)
                                {
                                    result += kingMidBoard[i][j];
                                }
                            }
                        }
                    }
                    else
                    {
                        if (b.isOccupied(i, j))
                        { //checks if board square has a piece
                            Piece.Piece p = b.getCase(i, j).getPiece();
                            if (!p.isWhite)
                            {
                                if (p is Piece.Pawn)
                                {
                                    result += pawnBoard[7 - i][7 - j];
                                }
                                else if (p is Piece.Rook)
                                {
                                    result += rookBoard[7 - i][7 - j];
                                }
                                else if (p is Piece.Bishop)
                                {
                                    result += bishopBoard[7 - i][7 - j];
                                }
                                else if (p is Piece.Knight)
                                {
                                    result += knightBoard[7 - i][7 - j];
                                }
                                else if (p is Piece.Queen)
                                {
                                    result += queenBoard[7 - i][7 - j];
                                }
                                else if (p is Piece.King)
                                {
                                    result += kingMidBoard[7 - i][7 - j];
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
