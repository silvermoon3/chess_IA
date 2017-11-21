using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1
{
    abstract class Piece
    {
        colorPlayer colorPlayer;
        public Piece(colorPlayer _colorPlayer)
        {
            this.colorPlayer = _colorPlayer;
            //info = InfoPiece.GetInfo(joueur.couleur, type);

        }
        public abstract List<Position> getPossibleMoves(Position currentPosition, Square[,] board);
        public abstract Boolean ocuppiedOrOnPath(Position currentPosition, Position desination, Square[,] board);

    }
}
