using System;
using System.ComponentModel;
using System.Diagnostics;
using processAI1.Board.Bitboard;

namespace processAI1.Board
{
    public static class Attack
    {

        public class Attacks
        {
            public int size;
            // 2 
            public int[] square;
            public UInt64 avoid;
            public UInt64 pinned;

            public Attacks()
            {
                square = new int[2];
            }
        };
        //
        public static readonly int[] Pawn_Move= new int[Side.SIZE]{ +1, -1 };
        public static readonly int[][] Pawn_Attack = new int[Side.SIZE][]{ new[]{ -15, +17 }, new[]{ -17, +15 } };

        public static readonly int[] Knight_Inc = { -33, -31, -18, -14, +14, +18, +31, +33, 0 };
        public static readonly int[] Bishop_Inc= { -17, -15, +15, +17, 0 };
        public static readonly int[] Rook_Inc= { -16, -1, +1, +16, 0 };
        public static readonly int[] Queen_Inc= { -17, -16, -15, -1, +1, +15, +16, +17, 0 };
        

        public static int[][] Piece_Inc = { null, Knight_Inc, Bishop_Inc, Rook_Inc, Queen_Inc, Queen_Inc, null };
        //Side.SIZE Square.SIZE
        public static UInt64[][] Pawn_Moves;

        public static UInt64[][] Pawn_Attacks;
        //Piece.SIZE Square.SIZE
        public static UInt64[][] Piece_Attacks;

        public static UInt64[][] Blockers;

        //Square.SIZE Square.SIZE
        public static UInt64[][] Between;

        public static UInt64[][] Behind;



        public static void init()
        {

            Pawn_Moves = new UInt64[Side.SIZE][];
            Pawn_Attacks = new UInt64[Side.SIZE][];
            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                Pawn_Moves[sd] = new UInt64[Square.SIZE];
                Pawn_Attacks[sd] = new UInt64[Square.SIZE];
                for (int sq = 0; sq < Square.SIZE; sq++)
                {
                    Pawn_Moves[sd][sq] = pawn_moves_debug(sd, sq);
                    Pawn_Attacks[sd][sq] = pawn_attacks_debug(sd, sq);
                }
            }

            Piece_Attacks = new UInt64[Piece.SIZE][];
            Blockers = new UInt64[Piece.SIZE][];
            for (int pc = (int)Piece.piece.KNIGHT; pc <= (int)Piece.piece.KING; pc++)
            {
                Piece_Attacks[pc] = new UInt64[Square.SIZE];
                Blockers[pc] = new UInt64[Square.SIZE];
                for (int sq = 0; sq < Square.SIZE; sq++)
                {
                    Piece_Attacks[pc][sq] = piece_attacks_debug(pc, sq);
                    Blockers[pc][sq] = blockers_debug(pc, sq);
                }
            }

            Between = new UInt64[Square.SIZE][];
            Behind = new UInt64[Square.SIZE][];
            for (int f = 0; f < Square.SIZE; f++)
            {
                Between[f] = new UInt64[Square.SIZE];
                Behind[f] = new UInt64[Square.SIZE];
                for (int t = 0; t < Square.SIZE; t++)
                {
                    Between[f][t] = between_debug(f, t);
                    Behind[f][t] = behind_debug(f, t);
                }
            }
        }




        public static bool line_is_empty(int f, int t, ref Board bd) {
           return (bd.all() & Between[f][t]) == 0;
        }

    public static UInt64 ray(int f, int t)
    {
        return Between[f][t] | Behind[f][t]; // HACK: t should be included
    }

        public static bool pawn_move(int sd, int f, int t, ref Board bd)
    {
        Debug.Assert(sd < Side.SIZE);
        return Bit.IsSet(Pawn_Moves[sd][f], t) && line_is_empty(f, t, ref bd);
    }

        public static bool pawn_attack(int sd, int f, int t)
    {
        Debug.Assert(sd < Side.SIZE);
            return Bit.IsSet(Pawn_Attacks[sd][f], t);
    }

        public static bool piece_attack(int pc, int f, int t, ref Board bd)
    {
        Debug.Assert(pc != (int)Piece.piece.PAWN);
        return Bit.IsSet(Piece_Attacks[pc][f], t) && line_is_empty(f, t, ref bd);
    }

        public static bool attack(int pc, int sd, int f, int t, ref Board bd)
    {
        Debug.Assert(sd < Side.SIZE);
        if (pc == (int)Piece.piece.PAWN)
        {
            return pawn_attack(sd, f, t);
        }
        else
        {
            return piece_attack(pc, f, t,ref bd);
        }
    }

        public static UInt64 pawn_moves_from(int sd, ref Board bd)
    { // for pawn mobility

        Debug.Assert(sd < Side.SIZE);

        UInt64 fs = bd.piece((int)Piece.piece.PAWN, sd);

        if (sd == Side.WHITE)
        {
            return fs << 1;
        }
        else
        {
            return fs >> 1;
        }
    }

        public static UInt64 pawn_moves_to(int sd, UInt64 ts, ref Board bd)
    {

        Debug.Assert(sd < Side.SIZE);
        Debug.Assert((bd.all() & ts) == 0);

        UInt64 pawns = bd.piece((int)Piece.piece.PAWN, sd);
        UInt64 empty = bd.empty();

        UInt64 fs = 0;

        if (sd == Side.WHITE)
        {
            fs |= (ts >> 1);
            fs |= (ts >> 2) & (empty >> 1) & Bit.Rank((int)Square.rank.RANK_2);
        }
        else
        {
            fs |= (ts << 1);
            fs |= (ts << 2) & (empty << 1) & Bit.Rank((int)Square.rank.RANK_7);
        }

        return pawns & fs;
    }

        public static UInt64 pawn_attacks_from(int sd, ref Board bd)
    {

        Debug.Assert(sd < Side.SIZE);

        UInt64 fs = bd.piece((int)Piece.piece.PAWN, sd);

        if (sd == Side.WHITE)
        {
            return (fs >> 7) | (fs << 9);
        }
        else
        {
            return (fs >> 9) | (fs << 7);
        }
    }

        public static UInt64 pawn_attacks_tos(int sd, UInt64 ts)
    {

        Debug.Assert(sd < Side.SIZE);

        if (sd == Side.WHITE)
        {
            return (ts >> 9) | (ts << 7);
        }
        else
        {
            return (ts >> 7) | (ts << 9);
        }
    }

        public static UInt64 pawn_attacks_from(int sd, int f)
    {
        Debug.Assert(sd < Side.SIZE);
        return Pawn_Attacks[sd][f];
    }

        public static UInt64 pawn_attacks_to(int sd, int t)
    {
        Debug.Assert(sd < Side.SIZE);
        return pawn_attacks_from(Side.Opposit(sd), t);
    }

        public static UInt64 piece_attacks_from(int pc, int f, ref Board bd)
    {

        Debug.Assert(pc != (int)Piece.piece.PAWN);

        UInt64 ts = Piece_Attacks[pc][f];

        for (UInt64 b = bd.all() & Blockers[pc][f]; b != 0; b = Bit.Rest(b))
        {
            int sq = Bit.First(b);
            ts &= ~Behind[f][sq];
        }

        return ts;
    }

        public static UInt64 piece_attacks_to(int pc, int t, ref Board bd)
    {
        Debug.Assert(pc != (int)Piece.piece.PAWN);
        return piece_attacks_from(pc, t,ref bd);
    }

        public static UInt64 piece_moves_from(int pc, int sd, int f, ref Board bd)
    {
        if (pc == (int)Piece.piece.PAWN)
        {
            Debug.Assert(false); // TODO: blockers
            return Pawn_Moves[sd][f];
        }
        else
        {
            return piece_attacks_from(pc, f,ref bd);
        }
    }

        public static UInt64 attacks_from(int pc, int sd, int f, ref Board bd)
    {
        if (pc == (int)Piece.piece.PAWN)
        {
            return Pawn_Attacks[sd][f];
        }
        else
        {
            return piece_attacks_from(pc, f,ref bd);
        }
    }

        public static UInt64 attacks_to(int pc, int sd, int t, ref Board bd)
    {
        return attacks_from(pc, Side.Opposit(sd), t,ref bd); // HACK for pawns
    }

        public static UInt64 pseudo_attacks_from(int pc, int sd, int f)
    {
        if (pc == (int)Piece.piece.PAWN)
        {
            return Pawn_Attacks[sd][f];
        }
        else
        {
            return Piece_Attacks[pc][f];
        }
    }

        public static UInt64 pseudo_attacks_to(int pc, int sd, int t)
    {
        return pseudo_attacks_from(pc, Side.Opposit(sd), t); // HACK for pawns
    }

        public static UInt64 slider_pseudo_attacks_to(int sd, int t, ref Board bd)
    {

        Debug.Assert(sd < Side.SIZE);

        UInt64 b = 0;
        b |= bd.piece((int)Piece.piece.BISHOP, sd) & Piece_Attacks[(int)Piece.piece.BISHOP][t];
        b |= bd.piece((int)Piece.piece.ROOK, sd) & Piece_Attacks[(int)Piece.piece.ROOK][t];
        b |= bd.piece((int)Piece.piece.QUEEN, sd) & Piece_Attacks[(int)Piece.piece.QUEEN][t];

        return b;
    }

        public static bool attack_behind(int f, int t, int sd, ref Board bd)
    {

        Debug.Assert(bd.square(t) != (int)Piece.piece.NONE);

        UInt64 behind = Behind[f][t];
        if (behind == 0) return false;

        for (UInt64 b = slider_pseudo_attacks_to(sd, t,ref bd) & behind; b != 0; b = Bit.Rest(b))
        {

            int sq = Bit.First(b);

            if (Bit.Single(bd.all() & Between[sq][f]))
            {
                return true;
            }
        }

        return false;
    }

        public static bool is_attacked(int t, int sd, ref Board bd)
    {

        // non-sliders

        if ((bd.piece((int)Piece.piece.PAWN, sd) & Pawn_Attacks[Side.Opposit(sd)][t]) != 0)
        { // HACK
            return true;
        }

        if ((bd.piece((int)Piece.piece.KNIGHT, sd) & Piece_Attacks[(int)Piece.piece.KNIGHT][t]) != 0)
        {
            return true;
        }

        if ((bd.piece((int)Piece.piece.KING, sd) & Piece_Attacks[(int)Piece.piece.KING][t]) != 0)
        {
            return true;
        }

        // sliders

        for (UInt64 b = slider_pseudo_attacks_to(sd, t,ref bd); b != 0; b = Bit.Rest(b))
        {

            int f = Bit.First(b);

            if ((bd.all() & Between[f][t]) == 0)
            {
                return true;
            }
        }

        return false;
    }

        public static UInt64 pinned_by(int t, int sd, ref Board bd)
    {

        UInt64 pinned = 0;

        for (UInt64 b = slider_pseudo_attacks_to(sd, t,ref bd); b != 0; b = Bit.Rest(b))
        {

            int f = Bit.First(b);

            UInt64 bb = bd.all() & Between[f][t];

            if (bb != 0 && Bit.Single(bb))
            {
                pinned |= bb;
            }
        }

        return pinned;
    }

        public static void init_attacks(ref Attacks attacks, int sd, ref Board bd)
    { // for strictly-legal moves

        int atk = Side.Opposit(sd);
        int def = sd;

        int t = bd.king(def);

        attacks.size = 0;
        attacks.avoid = 0;
        attacks.pinned = 0;

        // non-sliders

        {
            UInt64 b = 0;
            b |= bd.piece((int)Piece.piece.PAWN, atk) & Pawn_Attacks[def][t]; // HACK
            b |= bd.piece((int)Piece.piece.KNIGHT, atk) & Piece_Attacks[(int)Piece.piece.KNIGHT][t];

            if (b != 0)
            {
                Debug.Assert(Bit.Single(b));
                Debug.Assert(attacks.size < 2);
                attacks.square[attacks.size++] = Bit.First(b);
            }
        }

        // sliders

        for (UInt64 b = slider_pseudo_attacks_to(atk, t,ref bd); b != 0; b = Bit.Rest(b))
        {

            int f = Bit.First(b);

            UInt64 bb = bd.all() & Between[f][t];

            if (bb == 0)
            {
                Debug.Assert(attacks.size < 2);
                attacks.square[attacks.size++] = f;
                attacks.avoid |= ray(f, t);
            }
            else if (Bit.Single(bb))
            {
                attacks.pinned |= bb;
            }
        }
    }

        public static void init_attacks(ref Attacks  attacks, ref Board bd)
    {
        init_attacks(ref attacks, bd.turn(),ref bd);
    }

        public static bool is_legal(ref Board bd)
    {

        int atk = bd.turn();
        int def = Side.Opposit(atk);

        return !is_attacked(bd.king(def), atk,ref bd);
    }

        public static bool is_in_check(ref Board bd)
    {

        int atk = bd.turn();
        int def = Side.Opposit(atk);

        return is_attacked(bd.king(atk), def,ref bd);
    }

        public static UInt64 pawn_moves_debug(int sd, int sq)
    {

        Debug.Assert(sd < Side.SIZE);

        UInt64 b = 0UL;

        int f = Square.To88(sq);
        int inc = Pawn_Move[sd];

        int t = f + inc;

        if (Square.IsValid88(t))
        {
            Bit.Set(ref b, Square.From88(t));
        }

        if (Square.Rank(sq, sd) == (int)Square.rank.RANK_2)
        {
            t += inc;
            Debug.Assert(Square.IsValid88(t));
            Bit.Set(ref b, Square.From88(t));
        }

        return b;
    }

        public static UInt64 pawn_attacks_debug(int sd, int sq)
    {

        Debug.Assert(sd < Side.SIZE);

        UInt64 b = 0;

        int f = Square.To88(sq);

        for (int dir = 0; dir < 2; dir++)
        {
            int t = f + Pawn_Attack[sd][dir];
            if (Square.IsValid88(t))
            {
                Bit.Set(ref b, Square.From88(t));
            }
        }

        return b;
    }

        public static UInt64 piece_attacks_debug(int pc, int sq)
    {

        Debug.Assert(pc != (int)Piece.piece.PAWN);

        UInt64 b = 0;

        int f = Square.To88(sq);

        for (int dir = 0; true; dir++)
        {

            int inc = Piece_Inc[pc][dir];
            if (inc == 0) break;

            if (Piece.IsSlider(pc))
            {

                for (int t = f + inc; Square.IsValid88(t); t += inc)
                {
                    Bit.Set(ref b, Square.From88(t));
                }

            }
            else
            {

                int t = f + inc;

                if (Square.IsValid88(t))
                {
                    Bit.Set(ref b, Square.From88(t));
                }
            }
        }

        return b;
    }

        public static int delta_inc(int f, int t)
    {

        for (int dir = 0; dir < 8; dir++)
        {

            int inc = Queen_Inc[dir];

            for (int sq = f + inc; Square.IsValid88(sq); sq += inc)
            {
                if (sq == t)
                {
                    return inc;
                }
            }
        }

        return 0;
    }

        public static UInt64 between_debug(int f, int t)
    {

        f = Square.To88(f);
        t = Square.To88(t);

        UInt64 b = 0;

        int inc = delta_inc(f, t);

        if (inc != 0)
        {
            for (int sq = f + inc; sq != t; sq += inc)
            {
                Bit.Set(ref b, Square.From88(sq));
            }
        }

        return b;
    }

        public static UInt64 behind_debug(int f, int t)
    {

        f = Square.To88(f);
        t = Square.To88(t);

        UInt64 b = 0;

        int inc = delta_inc(f, t);

        if (inc != 0)
        {
            for (int sq = t + inc; Square.IsValid88(sq); sq += inc)
            {
                Bit.Set(ref b, Square.From88(sq));
            }
        }

        return b;
    }

        public static UInt64 blockers_debug(int pc, int f)
    {

        Debug.Assert(pc != (int)Piece.piece.PAWN);

        UInt64 b = 0;

        UInt64 attacks = piece_attacks_debug(pc, f);

        for (UInt64 bb = attacks; bb != 0; bb = Bit.Rest(bb))
        {
            int sq = Bit.First(bb);
            if ((attacks & behind_debug(f, sq)) != 0)
            {
                Bit.Set(ref b, sq);
            }
        }

        return b;
    }

}
}