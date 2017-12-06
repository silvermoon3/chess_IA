using System;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;

namespace processAI1.Board
{
    public class Move
    {
        public square from;
        public square to;
        public piece pieceMoving;
        public piece capturedPiece;
        public piece promotion;
        public bool isCastle;
        public Move()
        {
            
        }
        public Move(square f, square t, piece pc, piece cp, piece pp = piece.NONE)
        {
            Debug.Assert(pc != piece.NONE);
            Debug.Assert(pp == piece.NONE || pc == piece.PAWN);
            this.from = f;
            this.to = t;
            this.pieceMoving = pc;
            this.capturedPiece = cp;
            this.promotion = pp;
        }

        public static int MakeFlags(piece pc, piece cp, piece pp = piece.NONE)
        {
            return ((int)pc << 6) | ((int)cp << 3) |(int) pp;
        }

        public static Move CreateMove(square f, square t, piece pc, piece cp, piece pp = piece.NONE,Boolean isCastle = false)
        {
            Debug.Assert(pc != piece.NONE);
            Debug.Assert(pp == piece.NONE || pc == piece.PAWN);

            Move m = new Move();

            m.from = f;
            m.to = t;
            m.pieceMoving = pc;
            m.capturedPiece = cp;
            m.promotion = pp;
            m.isCastle = isCastle;

            return m;
        }

        public square GetFrom()
        {
             
            return from;
        }

        public square GetTo()
        {
            return to;
        }

        public piece GetPieceMoving()
        {
            return pieceMoving;
        }

        public piece GetCapturedPiece()
        {
            return capturedPiece;
        }

        public piece GetPromoted()
        {
            return promotion;
        }

        public bool GetIsCastle()
        {
            return isCastle;
        }

        public override string ToString()
        {
            return ToCan();
        }

        public string ToCan()
        {

            string s = "";

            s += Square.ToString(this.GetFrom());
            s += Square.ToString(this.GetTo());

            if (this.GetPromoted() != piece.NONE)
            {
                s += Char.ToLower(processAI1.Board.Piece.ToChar(this.GetPromoted()));
            }

            return s;
        }
        public static Move Make(square f, square t, piece pp, ref Board bd) {

            piece pc = bd.getSquare(f);
            piece cp = bd.getSquare(t);

            if (pc == piece.PAWN && t == bd.ep_sq()) {
                cp = piece.PAWN;
            }

            if (pc == piece.PAWN && Square.IsPromotion(t) && pp == piece.NONE) { // not needed
                pp = piece.QUEEN;
            }

            return CreateMove(f, t, pc, cp, pp);
        }

        public static Move from_string(ref string s, ref Board bd) {

            Debug.Assert(s.Length>= 4);
            string from = s.Substring(0, 2);
            string to = s.Substring(2, 2);
            square f = Square.FromString(ref from );
            square t = Square.FromString(ref to );
            piece pp = (s.Length > 4) ? Piece.FromChar(Char.ToUpper(s[4])) : piece.NONE;

            return Move.Make(f, t, pp, ref bd);
        }
}
}