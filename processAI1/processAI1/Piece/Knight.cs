using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1
{
    class Knight:Piece
    {
        public Knight(colorPlayer colorPlayer): base(colorPlayer) { }
        override
       public List<Position> getPossibleMoves(Position currentPosition, Square[,] board)
        {
            List<Position> legalMoves = new List<Position>();
           // if (currentPosition.getX() < 7 && currentPosition.getY() < 6 && (!Belief.isOccupied(currentPosition.getX() + 1, currentPosition.getY() + 2) || Belief.isOccupied(currentPosition.getX() + 1, currentPosition.getY() + 2).piece.info.couleur != joueur.couleur))
           //     legalMoves.Add(new Position(currentPosition.getX() + 1, currentPosition.getY() + 2));

            //if (currentPosition.colonne < 7 && currentPosition.rangee > 1 && (!e.getCase(currentPosition.colonne + 1, position.rangee - 2).occupée || e.getCase(position.colonne + 1, position.rangee - 2).piece.info.couleur != joueur.couleur))
            //    mouvements.Add(e.getCase(position.colonne + 1, position.rangee - 2));

            //if (currentPosition.colonne < 6 && currentPosition.rangee < 7 && (!e.getCase(currentPosition.colonne + 2, position.rangee + 1).occupée || e.getCase(position.colonne + 2, position.rangee + 1).piece.info.couleur != joueur.couleur))
            //    mouvements.Add(e.getCase(position.colonne + 2, position.rangee + 1));
            //if (currentPosition.colonne < 6 && currentPosition.rangee > 0 && (!e.getCase(currentPosition.colonne + 2, position.rangee - 1).occupée || e.getCase(position.colonne + 2, position.rangee - 1).piece.info.couleur != joueur.couleur))
            //    mouvements.Add(e.getCase(position.colonne + 2, position.rangee - 1));

            //if (currentPosition.colonne > 0 && currentPosition.rangee > 1 && (!e.getCase(position.colonne - 1, position.rangee - 2).occupée || e.getCase(position.colonne - 1, position.rangee - 2).piece.info.couleur != joueur.couleur))
            //    mouvements.Add(e.getCase(position.colonne - 1, position.rangee - 2));
            //if (position.colonne > 0 && position.rangee < 6 && (!e.getCase(position.colonne - 1, position.rangee + 2).occupée || e.getCase(position.colonne - 1, position.rangee + 2).piece.info.couleur != joueur.couleur))
            //    mouvements.Add(e.getCase(position.colonne - 1, position.rangee + 2));

            //if (position.colonne > 1 && position.rangee < 7 && (!e.getCase(position.colonne - 2, position.rangee + 1).occupée || e.getCase(position.colonne - 2, position.rangee + 1).piece.info.couleur != joueur.couleur))
            //    mouvements.Add(e.getCase(position.colonne - 2, position.rangee + 1));
            //if (position.colonne > 1 && position.rangee > 0 && (!e.getCase(position.colonne - 2, position.rangee - 1).occupée || e.getCase(position.colonne - 2, position.rangee - 1).piece.info.couleur != joueur.couleur))
            //    mouvements.Add(e.getCase(position.colonne - 2, position.rangee - 1));
            return legalMoves;
        }
        override
        public Boolean ocuppiedOrOnPath(Position currentPosition, Position desination, Square[,] board)
        {
            return false;
        }
    }
}
