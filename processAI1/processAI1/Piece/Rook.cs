using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1
{
    class Rook: Piece
    {

        override
       public List<Position> getPossibleMoves(Position currentPosition, Square[,] board)
        {
            List<Position> legalMoves = new List<Position>();
            //Right
            if (currentPosition.getX() < 7)
                for (int i= currentPosition.getX() + 1 ; i<= 7; i++)
                {
                    if (!Belief.isOccupied(i, currentPosition.getY()))
                        legalMoves.Add(new Position(i, currentPosition.getY()));

                else
                {
                    
                    /*if (e.getCase(i, position.rangee).piece.info.couleur != joueur.couleur)
                        mouvements.Add(e.getCase(i, position.rangee));
                    break;*/
                }
            }

            if (currentPosition.getX() > 0)
                for (int i = currentPosition.getX() - 1; i >= 0; i--)
                {
                    if(!Belief.isOccupied(i, currentPosition.getY()))
                        legalMoves.Add(new Position(i, currentPosition.getY()));
                     else
                     {
                    //if (e.getCase(i, position.rangee).piece.info.couleur != joueur.couleur)
                    //    mouvements.Add(e.getCase(i, position.rangee));
                    //break;
                     }

             }   

            if (currentPosition.getY() < 7)
                for (int i = currentPosition.getY() + 1; i <= 7; i++)
                {
                    if (!Belief.isOccupied(currentPosition.getX(), i))
                        legalMoves.Add(new Position(currentPosition.getX(), i));                    
                    else
                    {
                        //if (e.getCase(position.colonne, i).piece.info.couleur != joueur.couleur)
                        //    mouvements.Add(e.getCase(position.colonne, i));
                        //break;
                    }
                }
            if (currentPosition.getY() > 0)
                for (int i = currentPosition.getY() - 1; i >= 0; i--)
                {
                    if (!Belief.isOccupied(currentPosition.getX(), i))
                        legalMoves.Add(new Position(currentPosition.getX(), i));
                    else
                    {
                        //if (e.getCase(position.colonne, i).piece.info.couleur != joueur.couleur)
                        //    mouvements.Add(e.getCase(position.colonne, i));
                        //break;
                    }
                }


            return null;
        }

        public List<Position> allLegalMoves(Position currentPosition, Square[,] board)
        {
            List<Position> legalMoves = new List<Position>();
            foreach (Position p in legalMoves)
                if (!ocuppiedOrOnPath(currentPosition, p, board))
                    legalMoves.Remove(p);

            return legalMoves;

        }
        override
        public Boolean ocuppiedOrOnPath(Position currentPosition, Position desination, Square[,] board)
        {
            return false;
        }

    }
}
