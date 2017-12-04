using System;
using System.Collections.Generic;
using System.Diagnostics;
using processAI1.Board;
using processAI1.Board.Bitboard;

namespace processAI1.Board
{
    public static class Gen
    {
       
        public static void add_pawn_move(ref List<Move> ml, int f, int t,  ref Board  bd)
        {
            Debug.Assert(bd.square(f) == (int)Piece.piece.PAWN);

            int pc = bd.square(f);
            int cp = bd.square(t);

            if (Square.IsPromotion(t))
            {
                ml.Add(Move.Make(f, t, pc, cp, (int)(int)Piece.piece.QUEEN));
                ml.Add(Move.Make(f, t, pc, cp, (int)Piece.piece.KNIGHT));
                ml.Add(Move.Make(f, t, pc, cp, (int)Piece.piece.ROOK));
                ml.Add(Move.Make(f, t, pc, cp, (int)Piece.piece.BISHOP));
            }
            else
            {
                ml.Add(Move.Make(f, t, pc, cp));
            }
        }

        public static void add_piece_move(ref List<Move> ml, int f, int t,  ref Board  bd)
        {
            Debug.Assert(bd.square(f) != (int)Piece.piece.PAWN);
            ml.Add(Move.Make(f, t, bd.square(f), bd.square(t)));
        }

        public static void add_move(ref List<Move> ml, int f, int t,  ref Board  bd)
        {
            if (bd.square(f) == (int)Piece.piece.PAWN)
            {
                add_pawn_move(ref ml, f, t, ref bd);
            }
            else
            {
                add_piece_move(ref ml, f, t, ref bd);
            }
        }

        public static void add_piece_moves_from(ref List<Move> ml, int f, UInt64 ts, ref Board  bd)
        {
            int pc = bd.square(f);

            for (UInt64 b = Attack.piece_attacks_from(pc, f, ref bd) & ts; b != 0; b = Bit.Rest(b))
            {
                int t = Bit.First(b);
                add_piece_move(ref ml, f, t, ref bd);
            }
        }

        public static void add_captures_to(ref List<Move> ml, int sd, int t, ref Board  bd)
        {
            for (int pc = (int)Piece.piece.PAWN; pc <= (int)Piece.piece.KING; pc++)
            {
                for (UInt64 b = bd.piece(pc, sd) & Attack.attacks_to(pc, sd, t, ref bd); b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);
                    add_move(ref ml, f, t, ref bd);
                }
            }
        }

        public static void add_captures_to_no_king(ref List<Move> ml, int sd, int t, ref Board  bd)
        {
            // for evasions

            for (int pc =(int)Piece.piece.PAWN; pc <= (int) (int)Piece.piece.QUEEN; pc++)
            {
                // skip king
                for (UInt64 b = bd.piece(pc, sd) & Attack.attacks_to(pc, sd, t, ref bd); b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);
                    add_move(ref ml, f, t, ref bd);
                }
            }
        }

        public static void add_pawn_captures(ref List<Move> ml, int sd, UInt64 ts, ref Board  bd)
        {
            UInt64 pawns = bd.piece((int)Piece.piece.PAWN, sd);
            ts &= bd.side(Side.Opposit(sd)); // not needed

            if (sd == Side.WHITE)
            {
                for (UInt64 b = (ts << 7) & pawns; b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);
                    int t = f - 7;
                    add_pawn_move(ref ml, f, t,ref  bd);
                }

                for (UInt64 b = (ts >> 9) & pawns; b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);
                    int t = f + 9;
                    add_pawn_move(ref ml, f, t,ref  bd);
                }
            }
            else
            {
                for (UInt64 b = (ts << 9) & pawns; b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);
                    int t = f - 9;
                    add_pawn_move(ref ml, f, t,ref  bd);
                }

                for (UInt64 b = (ts >> 7) & pawns; b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);
                    int t = f + 7;
                    add_pawn_move(ref ml, f, t,ref  bd);
                }
            }
        }

        public static void add_promotions(ref List<Move> ml, int sd, UInt64 ts, ref Board  bd)
        {
            UInt64 pawns = bd.piece((int)Piece.piece.PAWN, sd);

            if (sd == Side.WHITE)
            {
                for (UInt64 b = pawns & (ts >> 1) & Bit.Rank((int)Square.rank.RANK_7); b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);
                    int t = f + 1;
                    Debug.Assert(bd.square(t) == (int)Piece.piece.NONE);
                    Debug.Assert(Square.IsPromotion(t));
                    add_pawn_move(ref ml, f, t,ref  bd);
                }
            }
            else
            {
                for (UInt64 b = pawns & (ts << 1) & Bit.Rank((int)Square.rank.RANK_2); b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);
                    int t = f - 1;
                    Debug.Assert(bd.square(t) == (int)Piece.piece.NONE);
                    Debug.Assert(Square.IsPromotion(t));
                    add_pawn_move(ref ml, f, t,ref  bd);
                }
            }
        }

        public static void add_promotions(ref List<Move> ml, int sd, ref Board  bd)
        {
            add_promotions(ref ml, sd, bd.empty(),ref bd);
        }

        public static void add_pawn_quiets(ref List<Move> ml, int sd, UInt64 ts, ref Board  bd)
        {
            UInt64 pawns = bd.piece((int)Piece.piece.PAWN, sd);
            UInt64 empty = bd.empty();

            if (sd == Side.WHITE)
            {
                for (UInt64 b = pawns & (ts >> 1) & ~Bit.Rank((int)Square.rank.RANK_7); b != 0; b = Bit.Rest(b))
                {
                    // don't generate promotions
                    int f = Bit.First(b);
                    int t = f + 1;
                    Debug.Assert(bd.square(t) == (int)Piece.piece.NONE);
                    Debug.Assert(!Square.IsPromotion(t));
                    add_pawn_move(ref ml, f, t,ref  bd);
                }

                for (UInt64 b = pawns & (ts >> 2) & (empty >> 1) & Bit.Rank((int)Square.rank.RANK_2); b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);
                    int t = f + 2;
                    Debug.Assert(bd.square(t) == (int)Piece.piece.NONE);
                    Debug.Assert(!Square.IsPromotion(t));
                    add_pawn_move(ref ml, f, t,ref  bd);
                }
            }
            else
            {
                for (UInt64 b = pawns & (ts << 1) & ~Bit.Rank((int)Square.rank.RANK_2); b != 0; b = Bit.Rest(b))
                {
                    // don't generate promotions
                    int f = Bit.First(b);
                    int t = f - 1;
                    Debug.Assert(bd.square(t) == (int)Piece.piece.NONE);
                    Debug.Assert(!Square.IsPromotion(t));
                    add_pawn_move(ref ml, f, t,ref  bd);
                }

                for (UInt64 b = pawns & (ts << 2) & (empty << 1) & Bit.Rank((int)Square.rank.RANK_7); b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);
                    int t = f - 2;
                    Debug.Assert(bd.square(t) == (int)Piece.piece.NONE);
                    Debug.Assert(!Square.IsPromotion(t));
                    add_pawn_move(ref ml, f, t,ref  bd);
                }
            }
        }

        public static void add_pawn_pushes(ref List<Move> ml, int sd, ref Board  bd)
        {
            UInt64 ts = 0;

            if (sd == Side.WHITE)
            {
                ts |= Bit.Rank((int)Square.rank.RANK_7);
                ts |= Bit.Rank((int)Square.rank.RANK_6) & ~Attack.pawn_attacks_from(Side.BLACK,ref bd) &
                      (~bd.piece((int)Piece.piece.PAWN) >> 1); // HACK: direct access
            }
            else
            {
                ts |= Bit.Rank((int)Square.rank.RANK_2);
                ts |= Bit.Rank((int)Square.rank.RANK_3) & ~Attack.pawn_attacks_from(Side.WHITE,ref bd) &
                      (~bd.piece((int)Piece.piece.PAWN) << 1); // HACK: direct access
            }

            add_pawn_quiets(ref ml, sd, ts & bd.empty(),ref bd);
        }

        public static void add_en_passant(ref List<Move> ml, int sd, ref Board  bd)
        {
            int t = bd.ep_sq();

            if (t != (int)Square.square.NONE)
            {
                UInt64 fs = bd.piece((int)Piece.piece.PAWN, sd) & Attack.Pawn_Attacks[Side.Opposit(sd)][t];

                for (UInt64 b = fs; b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);
                    ml.Add(Move.Make(f, t, (int)Piece.piece.PAWN, (int)Piece.piece.PAWN));
                }
            }
        }

        public static bool can_castle(int sd, int wg,  ref Board bd)
        {
            int index = Castling.index(sd, wg);

            if (Castling.flag(bd.flags(), index))
            {
                int kf = Castling.info[index].kf;
                // int kt = Castling.info[index].kt;
                int rf = Castling.info[index].rf;
                int rt = Castling.info[index].rt;

                Debug.Assert(bd.square_is(kf, (int)Piece.piece.KING, sd));
                Debug.Assert(bd.square_is(rf, (int)Piece.piece.ROOK, sd));

                return Attack.line_is_empty(kf, rf,ref bd) && !Attack.is_attacked(rt, Side.Opposit(sd),ref bd);
            }

            return false;
        }

        public static void add_castling(ref List<Move> ml, int sd, ref Board  bd)
        {
            for (int wg = 0; wg < Wing.SIZE; wg++)
            {
                if (can_castle(sd, wg,ref bd))
                {
                    int index = Castling.index(sd, wg);
                    add_piece_move(ref ml, Castling.info[index].kf, Castling.info[index].kt,ref bd);
                }
            }
        }

        public static void add_piece_moves(ref List<Move> ml, int sd, UInt64 ts, ref Board  bd)
        {
            Debug.Assert(ts != 0);

            for (int pc = (int)Piece.piece.KNIGHT; pc <= (int)Piece.piece.KING; pc++)
            {
                for (UInt64 b = bd.piece(pc, sd); b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);
                    add_piece_moves_from(ref ml, f, ts,ref bd);
                }
            }
        }

        public static void add_piece_moves_no_king(ref List<Move> ml, int sd, UInt64 ts, ref Board  bd)
        {
            // for evasions

            Debug.Assert(ts != 0);

            for (int pc = (int)Piece.piece.KNIGHT; pc <= (int)Piece.piece.QUEEN; pc++)
            {
                // skip king
                for (UInt64 b = bd.piece(pc, sd); b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);
                    add_piece_moves_from(ref ml, f, ts,ref bd);
                }
            }
        }

        public static void add_piece_moves_rare(ref List<Move> ml, int sd, UInt64 ts, ref Board  bd)
        {
            // for captures

            Debug.Assert(ts != 0);

            for (int pc = (int)Piece.piece.KNIGHT; pc <= (int)Piece.piece.KING; pc++)
            {
                for (UInt64 b = bd.piece(pc, sd); b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);

                    for (UInt64 bb = Attack.pseudo_attacks_from(pc, sd, f) & ts; bb != 0; bb = Bit.Rest(bb))
                    {
                        int t = Bit.First(bb);

                        if (Attack.line_is_empty(f, t,ref bd))
                        {
                            add_piece_move(ref ml, f, t,ref  bd);
                        }
                    }
                }
            }
        }

        public static void add_captures(ref List<Move> ml, int sd, ref Board  bd)
        {
            UInt64 ts = bd.side(Side.Opposit(sd));

            add_pawn_captures(ref ml, sd, ts,ref bd);
            add_piece_moves_rare(ref ml, sd, ts,ref bd);
            add_en_passant(ref ml, sd,ref bd);
        }

        public static void add_captures_mvv_lva(ref List<Move> ml, int sd, ref Board  bd)
        {
            // unused

            for (int pc = (int)Piece.piece.QUEEN; pc >= (int)Piece.piece.PAWN; pc--)
            {
                for (UInt64 b = bd.piece(pc, Side.Opposit(sd)); b != 0; b = Bit.Rest(b))
                {
                    int t = Bit.First(b);
                    add_captures_to(ref ml, sd, t,ref bd);
                }
            }

            add_en_passant(ref ml, sd,ref bd);
        }

        public static bool is_move(Move mv,  ref Board bd)
        {
            // for TT-move legality

            int sd = bd.turn();

            int f = mv.GetFrom();
            int t = mv.GetTo();

            int pc = mv.GetPieceMoving();
            int cp = mv.GetCapturedPiece();

            if (!(bd.square(f) == pc && bd.square_side(f) == sd))
            {
                return false;
            }

            if (bd.square(t) != (int)Piece.piece.NONE && bd.square_side(t) == sd)
            {
                return false;
            }

            if (pc == (int)Piece.piece.PAWN && t == bd.ep_sq())
            {
                if (cp != (int)Piece.piece.PAWN)
                {
                    return false;
                }
            }
            else if (bd.square(t) != cp)
            {
                return false;
            }

            if (cp == (int)Piece.piece.KING)
            {
                return false;
            }

            if (pc == (int)Piece.piece.PAWN)
            {
                // TODO

                return true;
            }
            else
            {
                // TODO: castling

                // return Attack.piece_attack(pc, f, t,ref bd);

                return true;
            }

            Debug.Assert(false);
        }

        public static bool is_quiet_move(Move mv,  ref Board bd)
        {
            // for killer legality

            int sd = bd.turn();

            int f = mv.GetFrom();
            int t = mv.GetTo();

            int pc = mv.GetPieceMoving();
            Debug.Assert(mv.GetCapturedPiece() == (int)Piece.piece.NONE);
            Debug.Assert(mv.GetPromoted() == (int)Piece.piece.NONE);

            if (!(bd.square(f) == pc && bd.square_side(f) == sd))
            {
                return false;
            }

            if (bd.square(t) != (int)Piece.piece.NONE)
            {
                return false;
            }

            if (pc == (int)Piece.piece.PAWN)
            {
                int inc = Square.PawnInc(sd);

                if (false)
                {
                }
                else if (t - f == inc && !Square.IsPromotion(t))
                {
                    return true;
                }
                else if (t - f == inc * 2 && Square.Rank(f, sd) == (int)Square.rank.RANK_2)
                {
                    return bd.square(f + inc) == (int)Piece.piece.NONE;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // TODO: castling

                return Attack.piece_attack(pc, f, t,ref bd);
            }

            Debug.Assert(false);
        }

        public static void add_quiets(ref List<Move> ml, int sd, ref Board  bd)
        {
            add_castling(ref ml, sd,ref bd);
            add_piece_moves(ref ml, sd, bd.empty(),ref bd);
            add_pawn_quiets(ref ml, sd, bd.empty(),ref bd);
        }

        public static void add_evasions(ref List<Move> ml, int sd, ref Board bd, ref Attack.Attacks  attacks)
        {
            Debug.Assert(attacks.size > 0);

            int king = bd.king(sd);

            add_piece_moves_from(ref ml, king, ~bd.side(sd) & ~attacks.avoid,ref bd);

            if (attacks.size == 1)
            {
                int t = attacks.square[0];

                add_captures_to_no_king(ref ml, sd, t,ref bd);
                add_en_passant(ref ml, sd,ref bd);

                UInt64 ts = Attack.Between[king][t];
                Debug.Assert(Attack.line_is_empty(king, t,ref bd));

                if (ts != 0)
                {
                    add_pawn_quiets(ref ml, sd, ts,ref bd);
                    add_promotions(ref ml, sd, ts,ref bd);
                    add_piece_moves_no_king(ref ml, sd, ts,ref bd);
                }
            }
        }

        public static void add_evasions(ref List<Move> ml, int sd, ref Board  bd)
        {
            Attack.Attacks attacks = new Attack.Attacks();
            Attack.init_attacks(ref attacks, sd,ref bd);
            add_evasions(ref ml, sd, ref bd, ref attacks);
        }

        public static void add_checks(ref List<Move> ml, int sd, ref Board  bd)
        {
            int atk = sd;
            int def = Side.Opposit(sd);

            int king = bd.king(def);
            UInt64 pinned = Attack.pinned_by(king, atk,ref bd);
            UInt64 empty = bd.empty();
            empty &= ~Attack.pawn_attacks_from(Side.Opposit(sd),ref bd); // pawn-safe

            // discovered checks

            for (UInt64 fs = bd.pieces(atk) & pinned; fs != 0; fs = Bit.Rest(fs))
            {
                // TODO: pawns
                int f = Bit.First(fs);
                UInt64 ts = empty & ~Attack.ray(king, f); // needed only for pawns
                add_piece_moves_from(ref ml, f, ts,ref bd);
            }

            // direct checks, pawns

            {
                UInt64 ts = Attack.pseudo_attacks_to((int)Piece.piece.PAWN, sd, king) & empty;

                add_pawn_quiets(ref ml, sd, ts,ref bd);
            }

            // direct checks, knights

            {
                int pc = (int)Piece.piece.KNIGHT;

                UInt64 attacks = Attack.pseudo_attacks_to(pc, sd, king) & empty;

                for (UInt64 b = bd.piece(pc, sd) & ~pinned; b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);

                    UInt64 moves = Attack.pseudo_attacks_from(pc, sd, f);

                    for (UInt64 bb = moves & attacks; bb != 0; bb = Bit.Rest(bb))
                    {
                        int t = Bit.First(bb);
                        add_piece_move(ref ml, f, t,ref  bd);
                    }
                }
            }

            // direct checks, sliders

            for (int pc = (int)Piece.piece.BISHOP; pc <= (int)Piece.piece.QUEEN; pc++)
            {
                UInt64 attacks = Attack.pseudo_attacks_to(pc, sd, king) & empty;

                for (UInt64 b = bd.piece(pc, sd) & ~pinned; b != 0; b = Bit.Rest(b))
                {
                    int f = Bit.First(b);

                    UInt64 moves = Attack.pseudo_attacks_from(pc, sd, f);

                    for (UInt64 bb = moves & attacks; bb != 0; bb = Bit.Rest(bb))
                    {
                        int t = Bit.First(bb);

                        if (Attack.line_is_empty(f, t,ref bd) && Attack.line_is_empty(t, king,ref bd))
                        {
                            add_piece_move(ref ml, f, t,ref  bd);
                        }
                    }
                }
            }
        }

        public static bool is_legal_debug(Move mv, ref Board bd)
        {
            // HACK: duplicate from Move_Type

            bd.move(mv);
            bool b = Attack.is_legal(ref bd);
            bd.undo();

            return b;
        }

        public static void gen_moves_debug(ref List<Move> ml, ref Board  bd)
        {
            ml.Clear();

            int sd = bd.turn();

            if (Attack.is_in_check(ref bd))
            {
                add_evasions(ref ml, sd,ref bd);
            }
            else
            {
                add_captures(ref ml, sd,ref bd);
                add_promotions(ref ml, sd,ref bd);
                add_quiets(ref ml, sd,ref bd);
            }
        }

        public static void filter_legals(ref List<Move> dst, ref List<Move> src, ref Board bd)
        {
            dst.Clear();

            foreach (Move mv in src)
            {
                if (is_legal_debug(mv, ref bd))
                {
                    dst.Add(mv);
                }
            }
        }

        public static void gen_legals(ref List<Move> ml, ref Board bd)
        {
            List<Move> pseudos = new List<Move>();
            gen_moves_debug(ref pseudos,ref bd);

            filter_legals(ref ml, ref pseudos,ref bd);
        }

        public static void gen_legal_evasions(ref List<Move> ml, ref Board bd)
        {
            int sd = bd.turn();

            Attack.Attacks attacks = new Attack.Attacks();
            Attack.init_attacks(ref attacks, sd,ref bd);

            if (attacks.size == 0)
            {
                ml.Clear();
                return;
            }

            List<Move> pseudos = new List<Move>();
            add_evasions(ref pseudos, sd, ref bd, ref attacks);

            filter_legals(ref ml, ref pseudos,ref bd);
        }
    }
}