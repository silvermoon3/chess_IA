using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace processAI1
{
    public partial class Form1 : Form
    {
        const int RANGEES = 8;
        const int COLONNES = 8;
        const int CARREAU_TAILLE = 42;
        Color CARREAU_NOIR = Color.FromArgb(189, 117, 53);
        Color CARREAU_BLANC = Color.FromArgb(229, 197, 105);
        // visualisation de l'échiquier
        static PictureBox[,] carreaux = new PictureBox[COLONNES, RANGEES];

        // visualisation des captures
        const int CAPTURES = 15;
        PictureBox[] captures_blancs = new PictureBox[CAPTURES];
        PictureBox[] captures_noirs = new PictureBox[CAPTURES];

        // liste des pieces
        List<Bitmap> piecesBlanches = new List<Bitmap>();
        List<Bitmap> piecesNoires = new List<Bitmap>();

        // géstion du drag & drop
        PictureBox picFrom, picTo;
        Image imgFrom;
        // status de la partie
      
        public Form1()
        {
            InitializeComponent();
            CreationEchiquier();
        }

        void CreationEchiquier()
        {
            // création des carreaux pour l'échiquier
            int idx = 0;
            bool blanc;
            for (int y = 0; y < RANGEES; y++)
            {
                blanc = y % 2 == 0 ? true : false;

                for (int x = 0; x < COLONNES; x++)
                {
                    carreaux[x, y] = new PictureBox();
                    carreaux[x, y].SizeMode = PictureBoxSizeMode.CenterImage;
                    carreaux[x, y].Size = new Size(CARREAU_TAILLE, CARREAU_TAILLE);
                    carreaux[x, y].Left = x * CARREAU_TAILLE + 2;
                    carreaux[x, y].Top = y * CARREAU_TAILLE + 1;
                    carreaux[x, y].Tag = idx++;
                    carreaux[x, y].BackColor = blanc ? CARREAU_BLANC : CARREAU_NOIR;
                    blanc = !blanc;                    
                    pnlEdging.Controls.Add(carreaux[x, y]);
                }
            }

            // création des carreaux pour les piéces capturées
            for (int i = 0; i < CAPTURES; i++)
            {
                captures_blancs[i] = new PictureBox();
                captures_blancs[i].SizeMode = PictureBoxSizeMode.CenterImage;
                captures_blancs[i].Size = new Size(CARREAU_TAILLE, CARREAU_TAILLE);
                captures_blancs[i].Left = i * (CARREAU_TAILLE + 1) + 1;
                captures_blancs[i].Top = 384;
                captures_blancs[i].BackColor = SystemColors.ControlDark;
                captures_blancs[i].Tag = i;

                captures_noirs[i] = new PictureBox();
                captures_noirs[i].SizeMode = PictureBoxSizeMode.CenterImage;
                captures_noirs[i].Size = new Size(CARREAU_TAILLE, CARREAU_TAILLE);
                captures_noirs[i].Left = i * (CARREAU_TAILLE + 1) + 1;
                captures_noirs[i].Top = 384 + CARREAU_TAILLE + 1;
                captures_noirs[i].BackColor = SystemColors.ControlDark;
                captures_noirs[i].Tag = i;

                pnlMain.Controls.Add(captures_blancs[i]);
                pnlMain.Controls.Add(captures_noirs[i]);
            }

            // initialisation des images pour les pièces blanches
            piecesBlanches.Add(Properties.Resource1.King_White);
            piecesBlanches.Add(Properties.Resource1.Queen_White);
            piecesBlanches.Add(Properties.Resource1.Rook_White);
            piecesBlanches.Add(Properties.Resource1.Bishop_White);
            piecesBlanches.Add(Properties.Resource1.Knight_White);
            piecesBlanches.Add(Properties.Resource1.Pawn_White);

            // initialisation des images pour les pièces noires
            piecesNoires.Add(Properties.Resource1.King_Black);
            piecesNoires.Add(Properties.Resource1.Queen_Black);
            piecesNoires.Add(Properties.Resource1.Rook_Black);
            piecesNoires.Add(Properties.Resource1.Bishop_Black);
            piecesNoires.Add(Properties.Resource1.Knight_Black);
            piecesNoires.Add(Properties.Resource1.Pawn_Black);
            for(int i=0; i<8; i++)
            {
                ActualiserCase(i, 0, new Piece.Pawn(i, 1, false));
            }
            ActualiserCase(0, 1, new Piece.Rook(0, 6, false));
            ActualiserCase(1, 1, new Piece.Knight(1, 1, false));
            ActualiserCase(2, 1, new Piece.Bishop(2, 1, false));
            ActualiserCase(3, 1, new Piece.Queen(3, 1, false));
            ActualiserCase(4, 1, new Piece.King(4, 1, false));
            ActualiserCase(5, 1, new Piece.Bishop(5, 1, false));
            ActualiserCase(6, 1, new Piece.Knight(6, 1, false));
            ActualiserCase(7, 1, new Piece.Rook(7, 1, false));
            for (int i = 0; i < 8; i++)
            {
                ActualiserCase(i, 6, new Piece.Pawn(i,6, true));
            }
            ActualiserCase(0, 7, new Piece.Rook(0, 7, true));
            ActualiserCase(1, 7, new Piece.Knight(1, 7, true));
            ActualiserCase(2, 7, new Piece.Bishop(2, 7, true));
            ActualiserCase(3, 7, new Piece.Queen(3, 7, true));
            ActualiserCase(4, 7, new Piece.King(4, 7, true));
            ActualiserCase(5, 7, new Piece.Bishop(5, 7, true));
            ActualiserCase(6, 7, new Piece.Knight(6, 7, true));
            ActualiserCase(7, 7, new Piece.Rook(7, 7, true));


        }

        public static void ActualiserCase(int x, int y, Piece.Piece piece)
        {
            if (piece == null)
                carreaux[x, y].Image = null;
            else if (piece.isWhite)
            {
                if (piece is Piece.Rook)
                    carreaux[x, y].Image = Properties.Resource1.Rook_White;
                if (piece is Piece.Knight)
                    carreaux[x, y].Image = Properties.Resource1.Knight_White;
                if (piece is Piece.King)
                    carreaux[x, y].Image = Properties.Resource1.King_White;
                if (piece is Piece.Pawn)
                    carreaux[x, y].Image = Properties.Resource1.Pawn_White;
                if (piece is Piece.Queen)
                    carreaux[x, y].Image = Properties.Resource1.Queen_White;
                if (piece is Piece.Bishop)
                    carreaux[x, y].Image = Properties.Resource1.Pawn_White;
            }
            else
            {
                if (piece is Piece.Rook)
                    carreaux[x, y].Image = Properties.Resource1.Rook_Black;
                if (piece is Piece.Knight)
                    carreaux[x, y].Image = Properties.Resource1.Knight_Black;
                if (piece is Piece.King)
                    carreaux[x, y].Image = Properties.Resource1.King_Black;
                if (piece is Piece.Pawn)
                    carreaux[x, y].Image = Properties.Resource1.Pawn_Black;
                if (piece is Piece.Queen)
                    carreaux[x, y].Image = Properties.Resource1.Queen_Black;
                if (piece is Piece.Bishop)
                    carreaux[x, y].Image = Properties.Resource1.Pawn_Black;
            }
               
        }
    }
}
