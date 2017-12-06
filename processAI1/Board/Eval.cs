using System;
using System.Diagnostics;
using processAI1.Board;

namespace processAI1.Board
{
    public static class Eval
    {
        struct Entry
        {
            public UInt32 plock;
            public int eval;
        };

        public class Table
        {
            const int BITS = 16;
            const int SIZE = 1 << BITS;
            const int MASK = SIZE - 1;

            Entry[] p_table = new Entry[SIZE];


            public void clear()
            {
                for (int index = 0; index < SIZE; index++)
                {
                    p_table[index].plock = 0;
                    p_table[index].eval = 0;
                }
            }

            public int eval(ref Board bd, ref Pawn.Table pawn_table)
            {
                // NOTE: score for white

                UInt64 key = bd.eval_key();

                int index = (int) Hash.Index(key) & MASK;
                UInt32 plock = Hash.Lock(key);

                Entry entry = p_table[index];

                if (entry.plock == plock)
                {
                    return entry.eval;
                }

                int eval = comp_eval(ref bd, ref pawn_table);

                entry.plock = plock;
                entry.eval = eval;

                return eval;
            }
        };

        public class Attack_Info
        {
            public UInt64[] piece_attacks = new UInt64[Square.SIZE];
            public UInt64[] all_attacks = new UInt64[Square.SIZE];
            public UInt64[] multiple_attacks = new UInt64[Square.SIZE];

            public UInt64[][] ge_pieces = new UInt64[Side.SIZE][];

            public UInt64[][] lt_attacks = new UInt64[Side.SIZE][];
            public UInt64[][] le_attacks = new UInt64[Side.SIZE][];

            public UInt64[] king_evasions = new UInt64[Side.SIZE];

            public UInt64 pinned;

            public Attack_Info()
            {
                for (int i = 0; i < Side.SIZE; i++)
                {
                    ge_pieces[i] = new UInt64[Piece.SIZE];
                    lt_attacks[i] = new UInt64[Piece.SIZE];
                    le_attacks[i] = new UInt64[Piece.SIZE];
                }
            }
        };

        public static readonly int[] attack_weight = {0, 4, 4, 2, 1, 4, 0};
        public static readonly int[] attacked_weight = {0, 1, 1, 2, 4, 8, 0};

        public static int[] mob_weight = new int[32];
        public static int[] dist_weight = new int[8]; // for king-passer distance

        public static UInt64 small_centre, medium_centre, large_centre;
        public static UInt64 centre_0, centre_1;

        public static UInt64[] side_area = new UInt64[Side.SIZE];
        public static UInt64[][] king_area = new UInt64[Side.SIZE][]; //[Square.SIZE];

        public static void comp_attacks(ref Attack_Info ai, ref Board bd)
        {
            // prepare for adding defended opponent pieces

            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                UInt64 b = 0;

                for (int pc = (int) piece.KING; pc >= (int) piece.KNIGHT; pc--)
                {
                    b |= bd.getPieceBit((piece) pc, (side) sd);
                    ai.ge_pieces[(int) sd][(int) pc] = b;
                }

                ai.ge_pieces[(int) sd][(int) piece.BISHOP] =
                    ai.ge_pieces[(int) sd][(int) piece.KNIGHT]; // minors are equal
            }

            // pawn attacks

            {
                piece pc = (int) piece.PAWN;

                for (int sd = 0; sd < Side.SIZE; sd++)
                {
                    UInt64 b = Attack.pawn_attacks_from((side) sd, ref bd);

                    ai.lt_attacks[(int) sd][(int) pc] = 0; // not needed
                    ai.le_attacks[(int) sd][(int) pc] = b;
                    ai.all_attacks[(int) sd] = b;
                }
            }

            // piece attacks

            ai.multiple_attacks[(int) side.WHITE] = 0;
            ai.multiple_attacks[(int) side.BLACK] = 0;

            for (int pc = (int) piece.KNIGHT; pc <= (int) piece.KING; pc++)
            {
                int lower_piece = (pc == (int) piece.BISHOP) ? (int) piece.PAWN : pc - 1; // HACK: direct access
                Debug.Assert(lower_piece >= (int) piece.PAWN && lower_piece < pc);

                for (int sd = 0; sd < Side.SIZE; sd++)
                {
                    ai.lt_attacks[(int) sd][(int) pc] = ai.le_attacks[(int) sd][lower_piece];
                }

                for (int sd = 0; sd < Side.SIZE; sd++)
                {
                    for (UInt64 fs = bd.getPieceBit((piece) pc, (side) sd); fs != 0; fs = Bit.Rest(fs))
                    {
                        square sq = (square) Bit.First(fs);

                        UInt64 ts = Attack.piece_attacks_from((piece) pc, sq, ref bd);
                        ai.piece_attacks[(int) sq] = ts;

                        ai.multiple_attacks[(int) sd] |= ts & ai.all_attacks[(int) sd];
                        ai.all_attacks[(int) sd] |= ts;
                    }

                    ai.le_attacks[(int) sd][(int) pc] = ai.all_attacks[(int) sd];
                    Debug.Assert((ai.le_attacks[(int) sd][(int) pc] & ai.lt_attacks[(int) sd][(int) pc]) ==
                                 ai.lt_attacks[(int) sd][(int) pc]);

                    if (pc == (int) piece.BISHOP)
                    {
                        // minors are equal
                        ai.le_attacks[(int) sd][(int) piece.KNIGHT] = ai.le_attacks[(int) sd][(int) piece.BISHOP];
                    }
                }
            }

            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                square king = bd.king((side) sd);
                UInt64 ts = Attack.pseudo_attacks_from(piece.KING, (side) sd, king);
                ai.king_evasions[(int) sd] =
                    ts & ~bd.GetSide((side) sd) & ~ai.all_attacks[(int) Side.Opposit((side) sd)];
            }

            // pinned pieces

            ai.pinned = 0;

            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                square sq = bd.king((side) sd);
                ai.pinned |= bd.GetSide((side) sd) & Attack.pinned_by(sq, Side.Opposit((side) sd), ref bd);
            }
        }

        public static int mul_shift(int a, int b, int c)
        {
            int bias = 1 << (c - 1);
            return (a * b + bias) >> c;
        }

        public static readonly int[] passed_weight = new int[8] {0, 0, 0, 2, 6, 12, 20, 0};

        public static int passed_score(int sc, rank rk)
        {
            return mul_shift(sc, passed_weight[(int) rk], 4);
        }

        public static int mobility_score(UInt64 ts)
        {
            int mob = Bit.Count(ts);
            return mul_shift(20, mob_weight[mob], 8);
        }

        public static int attack_mg_score(piece pc, side sd, UInt64 ts)
        {
            int c0 = Bit.Count(ts & centre_0);
            int c1 = Bit.Count(ts & centre_1);
            int sc = c1 * 2 + c0;

            sc += Bit.Count(ts & side_area[(int) Side.Opposit(sd)]);

            return (sc - 4) * attack_weight[(int) pc] / 2;
        }

        public static int attack_eg_score(piece pc, side sd, UInt64 ts, ref Pawn.Info pi)
        {
            return Bit.Count(ts & pi.target[(int) sd]) * attack_weight[(int) pc] * 4;
        }

        public static int capture_score(piece pc, side sd, UInt64 ts, ref Board bd, ref Attack_Info ai)
        {
            int sc = 0;

            for (UInt64 b = ts & bd.pieces(Side.Opposit(sd)); b != 0; b = Bit.Rest(b))
            {
                square t = (square) Bit.First(b);

                int cp = (int) bd.getSquare(t);
                sc += attacked_weight[cp];
                if (Bit.IsSet(ai.pinned, t)) sc += attacked_weight[cp] * 2;
            }

            return attack_weight[(int) pc] * sc * 4;
        }

        public static int shelter_score(square sq, side sd, ref Board bd, ref Pawn.Info pi)
        {
            if (Square.Rank(sq, sd) > rank.RANK_2)
            {
                return 0;
            }

            int s0 = pi.shelter[(int) Square.File(sq)][(int) sd];

            int s1 = 0;

            for (int wg = 0; wg < Wing.SIZE; wg++)
            {
                int index = Castling.index(sd, wg);

                if (Castling.flag(bd.flags(), index))
                {
                    int fl = Wing.shelter_file[wg];
                    s1 = Math.Max(s1, (pi.shelter[fl][(int) sd]));
                }
            }

            if (s1 > s0)
            {
                return (s0 + s1) / 2;
            }
            else
            {
                return s0;
            }
        }

        public static int king_score(int sc, int n)
        {
            int weight = 256 - (256 >> n);
            return mul_shift(sc, weight, 8);
        }

        public static int eval_outpost(square sq, side sd, ref Board bd, ref Pawn.Info pi)
        {
            Debug.Assert(Square.Rank(sq, sd) >= rank.RANK_5);

            side xd = Side.Opposit(sd);

            int weight = 0;

            if (Bit.IsSet(pi.safe, sq))
            {
                // safe square
                weight += 2;
            }

            if (bd.square_is(Square.Stop(sq, sd), (int) piece.PAWN, xd))
            {
                // shielded
                weight++;
            }

            if (Pawn.is_attacked(sq, sd, ref bd))
            {
                // defended
                weight++;
            }

            return weight - 2;
        }

        public static bool passer_is_unstoppable(square sq, side sd, ref Board bd)
        {
            if (!Material.lone_king(Side.Opposit(sd), ref bd)) return false;

            file fl = Square.File(sq);
            UInt64 front = Bit.File(fl) & Bit.Front(sq, sd);

            if ((bd.all() & front) != 0)
            {
                // path not free
                return false;
            }

            if (Pawn.square_distance(bd.king(Side.Opposit(sd)), sq, sd) >= 2)
            {
                // opponent king outside square
                return true;
            }

            if ((front & ~Attack.pseudo_attacks_from(piece.KING, sd, bd.king(sd))) == 0)
            {
                // king controls promotion path
                return true;
            }

            return false;
        }

        public static int eval_passed(square sq, side sd, ref Board bd, ref Attack_Info ai)
        {
            file fl = Square.File(sq);
            side xd = Side.Opposit(sd);

            int weight = 4;

            // bplocker

            if (bd.getSquare(Square.Stop(sq, sd)) != piece.NONE)
            {
                weight--;
            }

            // free path

            UInt64 front = Bit.File(fl) & Bit.Front(sq, sd);
            UInt64 rear = Bit.File(fl) & Bit.Rear(sq, sd);

            if ((bd.all() & front) == 0)
            {
                bool major_behind = false;
                UInt64 majors = bd.getPieceBit(piece.ROOK, xd) | bd.getPieceBit(piece.QUEEN, xd);

                for (UInt64 b = majors & rear; b != 0; b = Bit.Rest(b))
                {
                    square f = (square) Bit.First(b);

                    if (Attack.line_is_empty(f, sq, ref bd))
                    {
                        major_behind = true;
                    }
                }

                if (!major_behind && (ai.all_attacks[(int) xd] & front) == 0)
                {
                    weight += 2;
                }
            }

            return weight;
        }

        public static int eval_pawn_cap(side sd, ref Board bd, ref Attack_Info ai)
        {
            UInt64 ts = Attack.pawn_attacks_from(sd, ref bd);

            int sc = 0;

            for (UInt64 b = ts & bd.pieces(Side.Opposit(sd)); b != 0; b = Bit.Rest(b))
            {
                square t = (square) Bit.First(b);

                piece cp = bd.getSquare(t);
                if (cp == piece.KING) continue;

                sc += (int) Piece.Value(cp) - 50;
                if (Bit.IsSet(ai.pinned, t)) sc += ((int) Piece.Value(cp) - 50) * 2;
            }

            return sc / 10;
        }

        public static int eval_pattern(ref Board bd)
        {
            int eval = 0;

            // fianchetto

            if (bd.square_is(square.B2, piece.BISHOP, side.WHITE)
                && bd.square_is(square.B3, piece.PAWN, side.WHITE)
                && bd.square_is(square.C2, piece.PAWN, side.WHITE))
            {
                eval += 20;
            }

            if (bd.square_is(square.G2, piece.BISHOP, side.WHITE)
                && bd.square_is(square.G3, piece.PAWN, side.WHITE)
                && bd.square_is(square.F2, piece.PAWN, side.WHITE))
            {
                eval += 20;
            }

            if (bd.square_is(square.B7, piece.BISHOP, side.BLACK)
                && bd.square_is(square.B6, piece.PAWN, side.BLACK)
                && bd.square_is(square.C7, piece.PAWN, side.BLACK))
            {
                eval -= 20;
            }

            if (bd.square_is(square.G7, piece.BISHOP, side.BLACK)
                && bd.square_is(square.G6, piece.PAWN, side.BLACK)
                && bd.square_is(square.F7, piece.PAWN, side.BLACK))
            {
                eval -= 20;
            }

            return eval;
        }

        public static bool has_minor(side sd, ref Board bd)
        {
            return bd.count(piece.KNIGHT, sd) + bd.count(piece.BISHOP, sd) != 0;
        }

        public static int draw_mul(side sd, ref Board bd, ref Pawn.Info pi)
        {
            side xd = Side.Opposit(sd);

            int[] pawn = new int[Side.SIZE];
            pawn[(int) side.WHITE] = bd.count(piece.PAWN, side.WHITE);
            pawn[(int) side.BLACK] = bd.count(piece.PAWN, side.BLACK);

            int force = Material.force(sd, ref bd) - Material.force(xd, ref bd);

            // rook-file pawns

            if (Material.lone_king_or_bishop(sd, ref bd) && pawn[(int) sd] != 0)
            {
                UInt64 b = bd.getPieceBit(piece.BISHOP, sd);

                if ((bd.getPieceBit(piece.PAWN, sd) & ~Bit.File(file.FILE_A)) == 0
                    && (bd.getPieceBit((int) piece.PAWN, xd) & Bit.File(file.FILE_B)) == 0)
                {
                    square prom = (sd == (int) side.WHITE) ? square.A8 : square.A1;

                    if (Square.Distance(bd.king(xd), prom) <= 1)
                    {
                        if (b == 0 || !Square.SameColour((square) Bit.First(b), prom))
                        {
                            return 1;
                        }
                    }
                }

                if ((bd.getPieceBit((int) piece.PAWN, sd) & ~Bit.File(file.FILE_H)) == 0
                    && (bd.getPieceBit((int) piece.PAWN, xd) & Bit.File(file.FILE_G)) == 0)
                {
                    square prom = (sd == (int) side.WHITE) ? square.H8 : square.H1;

                    if (Square.Distance(bd.king(xd), prom) <= 1)
                    {
                        if (b == 0 || !Square.SameColour((square) Bit.First(b), prom))
                        {
                            return 1;
                        }
                    }
                }
            }

            if (pawn[(int) sd] == 0 && Material.lone_king_or_minor(sd, ref bd))
            {
                return 1;
            }

            if (pawn[(int) sd] == 0 && Material.two_knights(sd, ref bd))
            {
                return 2;
            }

            if (pawn[(int) sd] == 0 && force <= 1)
            {
                return 2;
            }

            if (pawn[(int) sd] == 1 && force == 0 && has_minor(xd, ref bd))
            {
                return 4;
            }

            if (pawn[(int) sd] == 1 && force == 0)
            {
                square king = bd.king(xd);
                square pawnSquare = (square) Bit.First(bd.getPieceBit(piece.PAWN, sd));
                square stop = Square.Stop(pawnSquare, sd);

                if (king == stop || (Square.Rank(pawnSquare, sd) <= rank.RANK_6 && king == Square.Stop(stop, sd)))
                {
                    return 4;
                }
            }

            if (pawn[(int) sd] == 2 && pawn[(int) xd] >= 1 && force == 0 && has_minor(xd, ref bd) &&
                (bd.getPieceBit((int) piece.PAWN, sd) & pi.passed) == 0)
            {
                return 8;
            }

            if (Material.lone_bishop((int) side.WHITE, ref bd) && Material.lone_bishop(side.BLACK, ref bd) &&
                Math.Abs(pawn[(int) side.WHITE] - pawn[(int) side.BLACK]) <= 2)
            {
                // opposit-colour bishops

                square wb = (square) Bit.First(bd.getPieceBit(piece.BISHOP, side.WHITE));
                square bb = (square) Bit.First(bd.getPieceBit(piece.BISHOP, side.BLACK));

                if (!Square.SameColour(wb, bb))
                {
                    return 8;
                }
            }

            return 16;
        }

        public static int my_distance(square f, square t, int weight)
        {
            int dist = Square.Distance(f, t);
            return mul_shift(dist_weight[dist], weight, 8);
        }

        public static int check_number(piece pc, side sd, UInt64 ts, square king, ref Board bd)
        {
            Debug.Assert(pc != piece.KING);

            side xd = Side.Opposit(sd);
            UInt64 checks = ts & ~bd.GetSide(sd) & Attack.pseudo_attacks_to(pc, sd, king);

            if (!Piece.IsSlider(pc))
            {
                return Bit.Count(checks);
            }

            int n = 0;

            UInt64 b = checks & Attack.pseudo_attacks_to(piece.KING, xd, king); // contact checks
            n += Bit.Count(b) * 2;
            checks &= ~b;

            for (UInt64 bit = checks; bit != 0; bit = Bit.Rest(bit))
            {
                square t = (square) Bit.First(bit);

                if (Attack.line_is_empty(t, king, ref bd))
                {
                    n++;
                }
            }

            return n;
        }

        public static int comp_eval(ref Board bd, ref Pawn.Table pawn_table)
        {
            // NOTE: score for white

            Attack_Info ai = new Attack_Info();
            comp_attacks(ref ai, ref bd);

            Pawn.Info pi = pawn_table.info(ref bd);

            int eval = 0;
            int mg = 0;
            int eg = 0;

            int[] shelter = new int[Side.SIZE];

            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                shelter[(int) sd] = shelter_score(bd.king((side) sd), (side) sd, ref bd, ref pi);
            }

            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                side xd = Side.Opposit((side) sd);

                square my_king = bd.king((side) sd);
                square op_king = bd.king(xd);

                UInt64 target = ~(bd.getPieceBit((int) piece.PAWN, (side) sd) | Attack.pawn_attacks_from(xd, ref bd));

                int king_n = 0;
                int king_power = 0;

                // pawns

                {
                    UInt64 fs = bd.getPieceBit((int) piece.PAWN, (side) sd);

                    UInt64 front = (sd == (int) side.WHITE) ? Bit.Front(rank.RANK_3) : Bit.Rear(rank.RANK_6);

                    for (UInt64 b = fs & pi.passed & front; b != 0; b = Bit.Rest(b))
                    {
                        square sq = (square) Bit.First(b);

                        rank rk = Square.Rank((square) sq, (side) sd);

                        if (passer_is_unstoppable(sq, (side) sd, ref bd))
                        {
                            int weight = Math.Max(rk - rank.RANK_3, 0);
                            Debug.Assert(weight >= 0 && weight < 5);

                            eg += ((int) Piece.QUEEN_VALUE - Piece.PAWN_VALUE) * weight / 5;
                        }
                        else
                        {
                            int sc = eval_passed(sq, (side) sd, ref bd, ref ai);

                            int sc_mg = sc * 20;
                            int sc_eg = sc * 25;

                            square stop = Square.Stop(sq, (side) sd);
                            sc_eg -= my_distance(my_king, stop, 10);
                            sc_eg += my_distance(op_king, stop, 20);

                            mg += passed_score(sc_mg, rk);
                            eg += passed_score(sc_eg, rk);
                        }
                    }

                    eval += Bit.Count(Attack.pawn_moves_from((side) sd, ref bd) & bd.empty()) * 4 -
                            bd.count((int) piece.PAWN, (side) sd) * 2;

                    eval += eval_pawn_cap((side) sd, ref bd, ref ai);
                }

                // pieces

                for (int pc = (int) piece.KNIGHT; pc <= (int) piece.KING; pc++)
                {
                    int p12 = (int) Piece.Make((piece) pc, (side) sd); // for PST

                    {
                        int n = bd.count((piece) pc, (side) sd);
                        mg += n * Material.score((piece) pc, Stage.stage.MG);
                        eg += n * Material.score((piece) pc, Stage.stage.EG);
                    }
                    
                    for (UInt64 b = bd.getPieceBit((piece) pc, (side) sd); b != 0; b = Bit.Rest(b))
                    {
                        square sq = (square) Bit.First(b);

                        file fl = Square.File(sq);
                        rank rk = Square.Rank(sq, (side) sd);

                        // compute safe attacks

                        UInt64 ts_all = ai.piece_attacks[(int) sq];
                        UInt64 ts_pawn_safe = ts_all & target;

                        UInt64 safe = ~ai.all_attacks[(int) xd] | ai.multiple_attacks[(int) sd];

                        if (Piece.IsSlider((piece) pc))
                        {
                            // battery support

                            UInt64 bishops = bd.getPieceBit(piece.BISHOP, (side) sd) |
                                             bd.getPieceBit(piece.QUEEN, (side) sd);
                            UInt64 rooks = bd.getPieceBit(piece.ROOK, (side) sd) |
                                           bd.getPieceBit(piece.QUEEN, (side) sd);

                            UInt64 support = 0;
                            support |= bishops & Attack.pseudo_attacks_to(piece.BISHOP, (side) sd, sq);
                            support |= rooks & Attack.pseudo_attacks_to(piece.ROOK, (side) sd, sq);

                            for (UInt64 bit = ts_all & support; bit != 0; bit = Bit.Rest(bit))
                            {
                                square f = (square) Bit.First(b);
                                Debug.Assert(Attack.line_is_empty(f, sq, ref bd));
                                safe |= Attack.Behind[(int) f][(int) sq];
                            }
                        }

                        UInt64 ts_safe = ts_pawn_safe & ~ai.lt_attacks[(int) xd][(int) pc] & safe;

                        mg += PieceSquareTable.score(p12, sq, Stage.stage.MG);
                        eg += PieceSquareTable.score(p12, sq, Stage.stage.EG);

                        if (pc == (int) piece.KING)
                        {
                            eg += mobility_score( ts_safe);
                        }
                        else
                        {
                            eval += mobility_score(ts_safe);
                        }

                        if (pc != (int) piece.KING)
                        {
                            mg += attack_mg_score((piece) pc, (side) sd, ts_pawn_safe);
                        }

                        eg += attack_eg_score((piece) pc, (side) sd, ts_pawn_safe, ref pi);

                        eval += capture_score((piece) pc, (side) sd,
                            ts_all & (ai.ge_pieces[(int) xd][(int) pc] | target), ref bd, ref ai);

                        if (pc != (int) piece.KING)
                        {
                            eval += check_number((piece) pc, (side) sd, ts_safe, op_king, ref bd) *
                                    Material.power((piece) pc) * 6;
                        }

                        if (pc != (int) piece.KING && (ts_safe & king_area[(int) xd][(int) op_king]) != 0)
                        {
                            // king attack
                            king_n++;
                            king_power += Material.power((piece) pc);
                        }

                        if (Piece.IsMinor((piece) pc) && rk >= rank.RANK_5 && rk <= rank.RANK_6 &&
                            fl >= file.FILE_C && fl <= file.FILE_F)
                        {
                            // outpost
                            eval += eval_outpost(sq, (side) sd, ref bd, ref pi) * 5;
                        }

                        if (Piece.IsMinor((piece) pc) && rk >= rank.RANK_5 &&
                            !Bit.IsSet(ai.all_attacks[(int) (side) sd], sq))
                        {
                            // loose minor
                            mg -= 10;
                        }

                        if (Piece.IsMinor((piece) pc) && rk >= rank.RANK_3 && rk <= rank.RANK_4 &&
                            bd.square_is(Square.Stop(sq, (side) sd), (int) piece.PAWN, (side) sd))
                        {
                            // shielded minor
                            mg += 10;
                        }

                        if (pc == (int) piece.ROOK)
                        {
                            // open file

                            int sc = pi.open[(int) fl][(int) sd];

                            UInt64 minors = bd.getPieceBit(piece.KNIGHT, xd) | bd.getPieceBit(piece.BISHOP, xd);
                            if (true && sc >= 10 && (minors & Bit.File(fl) & ~target) != 0)
                            {
                                // bplocked by minor
                                sc = 5;
                            }

                            eval += sc - 10;

                            if (sc >= 10 && Math.Abs(Square.File(op_king) - fl) <= 1)
                            {
                                // open file on king
                                int weight = (Square.File(op_king) == fl) ? 2 : 1;
                                mg += sc * weight / 2;
                            }
                        }

                        if (pc == (int) piece.ROOK && rk == rank.RANK_7)
                        {
                            // 7th rank

                            UInt64 pawns = bd.getPieceBit((int) piece.PAWN, xd) & Bit.Rank(Square.Rank(sq));

                            if (Square.Rank(op_king, (side) sd) >= rank.RANK_7 || pawns != 0)
                            {
                                mg += 10;
                                eg += 20;
                            }
                        }

                        if (pc == (int) piece.KING)
                        {
                            // king out

                            int dl = (pi.left_file - 1) - fl;
                            if (dl > 0) eg -= dl * 20;

                            int dr = fl - (pi.right_file + 1);
                            if (dr > 0) eg -= dr * 20;
                        }
                    }
                }

                if (bd.count(piece.BISHOP, (side) sd) >= 2)
                {
                    mg += 30;
                    eg += 50;
                }

                mg += shelter[(int) sd];
                mg += mul_shift(king_score(king_power * 30, king_n), 32 - shelter[(int) xd], 5);

                eval = -eval;
                mg = -mg;
                eg = -eg;
            }

            mg += pi.mg;
            eg += pi.eg;

            eval += eval_pattern(ref bd);

            eval += Material.interpolation(mg, eg, ref bd);

            if (eval != 0)
            {
                // draw multiplier
                side winner = (eval > 0) ? side.WHITE : side.BLACK;
                eval = mul_shift(eval, draw_mul(winner, ref bd, ref pi), 4);
            }

            Debug.Assert(eval >= Score.EVAL_MIN && eval <= Score.EVAL_MAX);
            return eval;
        }

        public static int eval(ref Board bd, ref Table table, ref Pawn.Table pawn_table)
        {
            return Score.side_score(table.eval(ref bd, ref pawn_table), bd.turn());
        }

        public static void init()
        {
            small_centre = 0;
            medium_centre = 0;
            large_centre = 0;

            for (int sq = 0; sq < Square.SIZE; sq++)
            {
                file fl = Square.File((square) sq);
                rank rk = Square.Rank((square) sq);

                if (fl >= file.FILE_D && fl <= file.FILE_E && rk >= rank.RANK_4 && rk <= rank.RANK_5)
                {
                    Bit.Set(ref small_centre, (square) sq);
                }

                if (fl >= file.FILE_C && fl <= file.FILE_F && rk >= rank.RANK_3 && rk <= rank.RANK_6)
                {
                    Bit.Set(ref medium_centre, (square) sq);
                }

                if (fl >= file.FILE_B && fl <= file.FILE_G && rk >= rank.RANK_2 && rk <= rank.RANK_7)
                {
                    Bit.Set(ref large_centre, (square) sq);
                }
            }

            large_centre &= ~medium_centre;
            medium_centre &= ~small_centre;

            centre_0 = small_centre | large_centre;
            centre_1 = small_centre | medium_centre;

            side_area[(int) side.WHITE] = 0;
            side_area[(int) side.BLACK] = 0;

            for (int sq = 0; sq < Square.SIZE; sq++)
            {
                if (Square.Rank((square) sq) <= rank.RANK_4)
                {
                    Bit.Set(ref side_area[(int) side.WHITE], (square) sq);
                }
                else
                {
                    Bit.Set(ref side_area[(int) side.BLACK], (square) sq);
                }
            }
            king_area[(int) side.WHITE] = new UInt64[Square.SIZE];
            king_area[(int)side.BLACK] = new UInt64[Square.SIZE];

            for (int ks = 0; ks < Square.SIZE; ks++)
            {
                king_area[(int) side.WHITE][ks] = 0;
                king_area[(int) side.BLACK][ks] = 0;

                for (int asv = 0;
                    asv < Square.SIZE;
                    asv
                        ++)
                {
                    int df = Square.File((square) asv) - Square.File((square) ks);
                    int dr = Square.Rank((square) asv) - Square.Rank((square) ks);

                    if (Math.Abs(df) <= 1 && dr >= -1 && dr <= +2)
                    {
                        Bit.Set(ref king_area[(int) side.WHITE][ks], (square) asv);
                    }

                    if (Math.Abs(df) <= 1 && dr >= -2 && dr <= +1)
                    {
                        Bit.Set(ref king_area[(int) side.BLACK][ks], (square) asv);
                    }
                }
            }

            for (int i = 0; i < 32; i++)
            {
                double x = (double) i * 0.5;
                double y = 1.0 - Math.Exp(-x);
                mob_weight[i] = Util.round(y * 512.0) - 256;
            }

            for (int i = 0; i < 8; i++)
            {
                double x = (double) i - 3.0;
                double y = 1.0 / (1.0 + Math.Exp(-x));
                dist_weight[i] = Util.round(y * 7.0 * 256.0);
            }
        }
    }
}