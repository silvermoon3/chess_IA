using System;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;
using processAI1.Board.Bitboard;

namespace processAI1.Board
{
    public class Move
    {
        public int from;
        public int to;
        public int pieceMoving;
        public int capturedPiece;
        public int promotion;

        public Move()
        {
            
        }
        public Move(int f, int t, int pc, int cp, int pp = (int)Piece.piece.NONE)
        {
            Debug.Assert(f < Square.SIZE);
            Debug.Assert(t < Square.SIZE);
            Debug.Assert(pc < Piece.SIZE);
            Debug.Assert(cp < Piece.SIZE);
            Debug.Assert(pp < Piece.SIZE);

            Debug.Assert(pc != (int)Piece.piece.NONE);
            Debug.Assert(pp == (int)Piece.piece.NONE || pc == (int)Piece.piece.PAWN);

            this.from = f;
            this.to = t;
            this.pieceMoving = pc;
            this.capturedPiece = cp;
            this.promotion = pp;
        }

        public static int MakeFlags(int pc, int cp, int pp = (int)processAI1.Board.Piece.piece.NONE)
        {

            if (pc >= processAI1.Board.Piece.SIZE || cp >= processAI1.Board.Piece.SIZE || pp >= Piece.SIZE)
                throw new Exception("notPiece");

            return (pc << 6) | (cp << 3) | pp;
        }

        public static Move Make(int f, int t, int pc, int cp, int pp = (int)Piece.piece.NONE)
        {
            
            Debug.Assert(f < Square.SIZE);
            Debug.Assert(t < Square.SIZE);
            Debug.Assert(pc < Piece.SIZE);
            Debug.Assert(cp < Piece.SIZE);
            Debug.Assert(pp < Piece.SIZE);

            Debug.Assert(pc != (int)Piece.piece.NONE);
            Debug.Assert(pp == (int)Piece.piece.NONE || pc == (int)Piece.piece.PAWN);

            Move m = new Move();

            m.from = f;
            m.to = t;
            m.pieceMoving = pc;
            m.capturedPiece = cp;
            m.promotion = pp;

            return m;
        }

        public int GetFrom()
        {
             
            return from;
        }

        public int GetTo()
        {
            return to;
        }

        public int GetPieceMoving()
        {
            return pieceMoving;
        }

        public int GetCapturedPiece()
        {
            return capturedPiece;
        }

        public int GetPromoted()
        {
            return promotion;
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

            if (this.GetPromoted() != (int) processAI1.Board.Piece.piece.NONE)
            {
                s += Char.ToLower(processAI1.Board.Piece.ToChar(this.GetPromoted()));
            }

            return s;
        }
        public static Move Make(int f, int t, int pp, ref Board bd) {

            int pc = bd.square(f);
            int cp = bd.square(t);

            if (pc ==(int) Piece.piece.PAWN && t == bd.ep_sq()) {
                cp = (int)Piece.piece.PAWN;
            }

            if (pc == (int)Piece.piece.PAWN && Square.IsPromotion(t) && pp == (int)Piece.piece.NONE) { // not needed
                pp = (int)Piece.piece.QUEEN;
            }

            return Make(f, t, pc, cp, pp);
        }

        public static Move from_string(ref string s, ref Board bd) {

            Debug.Assert(s.Length>= 4);
            string from = s.Substring(0, 2);
            string to = s.Substring(2, 2);
            int f = Square.FromString(ref from );
            int t = Square.FromString(ref to );
            int pp = (s.Length > 4) ? Piece.FromChar(Char.ToUpper(s[4])) : (int)Piece.piece.NONE;

            return Move.Make(f, t, pp, ref bd);
        }
}
}