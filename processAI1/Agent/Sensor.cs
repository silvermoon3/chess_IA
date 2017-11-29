using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using processAI1.Piece;

namespace processAI1.Agent
{
    public class Sensor
    {
        int[] tabVal = new int[64];
        String[] tabCoord = new string[] { "a8","b8","c8","d8","e8","f8","g8","h8",
                                           "a7","b7","c7","d7","e7","f7","g7","h7",
                                           "a6","b6","c6","d6","e6","f6","g6","h6",
                                           "a5","b5","c5","d5","e5","f5","g5","h5",
                                           "a4","b4","c4","d4","e4","f4","g4","h4",
                                           "a3","b3","c3","d3","e3","f3","g3","h3",
                                           "a2","b2","c2","d2","e2","f2","g2","h2",
                                           "a1","b1","c1","d1","e1","f1","g1","h1" };

        List<Piece.Piece> myPieces = new List<Piece.Piece>();
        List<Piece.Piece> otherPieces = new List<Piece.Piece>();
        List<String> reste = new List<String>();
        public Sensor()
        {

        }

        public List<Piece.Piece> getMyPieces()
        {
            return myPieces;
        }

        public List<Piece.Piece> getOtherPieces()
        {
            return otherPieces;
        }


        public void readBoard(List<String> mesPieces, List<String> reste, int[] _tabVal)
        {
            tabVal = _tabVal;
            //first update of my pieces 
            //if (myPieces.Count == 0)
            //{
            myPieces = new List<Piece.Piece>();
                for (int i = 0; i < tabVal.Length; i++)
                {
                    Point p = new Point(tabCoord[i]);
                    Boolean isWhite = tabVal[i] > 0;
                    if (tabVal[i] == 21)
                        myPieces.Add(new Rook(p.getX(), p.getY(), isWhite));
                    else if (tabVal[i] == 31)
                        myPieces.Add(new Knight(p.getX(), p.getY(), isWhite));
                    else if (tabVal[i] == 4)
                        myPieces.Add(new Bishop(p.getX(), p.getY(), isWhite));
                    else if (tabVal[i] == 5)
                        myPieces.Add(new Queen(p.getX(), p.getY(), isWhite));
                    else if (tabVal[i] == 6)
                        myPieces.Add(new King(p.getX(), p.getY(), isWhite));
                    else if (tabVal[i] == 32)
                        myPieces.Add(new Knight(p.getX(), p.getY(), isWhite));
                    else if (tabVal[i] == 22)
                        myPieces.Add(new Rook(p.getX(), p.getY(), isWhite));
                    else if (tabVal[i] == 1)
                        myPieces.Add(new Pawn(p.getX(), p.getY(), isWhite));
                }
            //}
            //else if (mesPieces.Count != myPieces.Count)
            //{
            //    Boolean somethingToRemove = false;
            //    Piece.Piece toRemove = null;
            //    //enlever pièces qui a été mangée 
            //    foreach (Piece.Piece piece in myPieces)
            //    {
            //        if (!mesPieces.Contains(myPieces.Find(p => p.getPosition().equal(piece.getPosition())).getPosition().ToString()))
            //        {
            //            somethingToRemove = true;
            //            toRemove = piece;
            //        }
            //    }
            //    if (somethingToRemove)
            //        myPieces.Remove(toRemove);

            //}
            //Other piece 
            //if (otherPieces.Count == 0)
            //  {
            otherPieces = new List<Piece.Piece>();
            for (int i = 0; i < tabVal.Length; i++)
            {
                Point p = new Point(tabCoord[i]);
                Boolean isWhite = tabVal[i] > 0;
                if (tabVal[i] == -21)
                    otherPieces.Add(new Rook(p.getX(), p.getY(), isWhite));
                else if (tabVal[i] == -31)
                    otherPieces.Add(new Knight(p.getX(), p.getY(), isWhite));
                else if (tabVal[i] == -4)
                    otherPieces.Add(new Bishop(p.getX(), p.getY(), isWhite));
                else if (tabVal[i] == -5)
                    otherPieces.Add(new Queen(p.getX(), p.getY(), isWhite));
                else if (tabVal[i] == -6)
                    otherPieces.Add(new King(p.getX(), p.getY(), isWhite));
                else if (tabVal[i] == -32)
                    otherPieces.Add(new Knight(p.getX(), p.getY(), isWhite));
                else if (tabVal[i] == -22)
                    otherPieces.Add(new Rook(p.getX(), p.getY(), isWhite));
                else if (tabVal[i] == -1)
                    otherPieces.Add(new Pawn(p.getX(), p.getY(), isWhite));

            }
            //}
            //else
            //{
            //    //Update only piece which have been moved 
            //}
            //Update of other pieces 
        }

        public void getNewBelief()
        {
            //New other piece 

            //New Board State
        }

    }
}
