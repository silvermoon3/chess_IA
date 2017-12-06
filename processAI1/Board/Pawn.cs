using System;
using System.Diagnostics;

namespace processAI1.Board
{
    public static class Pawn
    {
        public class Info
        {
            // 80 bytes; TODO: merge some bitboards and/or file info?
            public int[][] open = new int[Square.FILE_SIZE][];

            public int[][] shelter = new int[Square.FILE_SIZE][];
            public UInt64 passed;
            public UInt64[] target = new UInt64[Side.SIZE];
            public UInt64 safe;
            public UInt32 plock;
            public int mg;
            public int eg;
            public file left_file;
            public file right_file;

            public Info()
            {
                for (int i = 0; i < Square.FILE_SIZE; i++)
                {
                    open[i] = new int [Side.SIZE];
                    shelter[i] = new int[Side.SIZE];
                }
            }
        };


        public class Table
        {
            const int BITS = 12;
            const int SIZE = 1 << BITS;
            const int MASK = SIZE - 1;

            Info[] p_table = new Info[SIZE];

            public void clear()
            {
                Info info = new Info();
                clear_info(ref info);

                for (int index = 0; index < SIZE; index++)
                {
                    p_table[index] = info;
                }
            }

            void clear_fast()
            {
                for (int index = 0; index < SIZE; index++)
                {
                    p_table[index].plock = 1; // board w/o pawns has key 0!
                }
            }

            public Info info(ref Board bd)
            {
                UInt64 key = bd.pawn_key();

                int index = (int) (Hash.Index(key) & MASK);
                UInt32 plock = Hash.Lock(key);
                if (p_table[index] == null)
                    p_table[index] = new Info();
                Info entry = p_table[index];
                
                if (entry.plock != plock)
                {
                    entry.plock = plock;
                    comp_info(ref entry, ref bd);
                }

                return entry;
            }
        };


        public static UInt64[][] passed_me= new UInt64[Square.SIZE][];
        
        public static UInt64[][] passed_opp = new UInt64[Square.SIZE][];

        public static bool is_passed(square sq, side sd, ref Board bd) {
            return (bd.getPieceBit(piece.PAWN, Side.Opposit(sd)) & passed_opp[(int)sq][(int)sd]) == 0UL
                   && (bd.getPieceBit(piece.PAWN, sd) & passed_me[(int)sq][(int)sd]) == 0;
        }

        public static int square_distance(square ks, square ps, side sd)
        {
            square prom = Square.Promotion(ps, sd);
            return Square.Distance(ks, prom) - Square.Distance(ps, prom);
        }


    public static UInt64[] duo = new UInt64[Square.SIZE];

        public static void clear_info(ref Info info)
        {
            info.passed = 0;
            info.safe = 0;
            info.plock = 1; // board w/o pawns has key 0!
            info.mg = 0;
            info.eg = 0;
            info.left_file = file.FILE_A;
            info.right_file = file.FILE_H;

            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                info.target[sd] = 0;
            }

            for (int fl = 0; fl < Square.FILE_SIZE; fl++)
            {
                for (int sd = 0; sd < Side.SIZE; sd++)
                {
                    info.open[fl][sd] = 0;
                    info.shelter[fl][sd] = 0;
                }
            }
        }

        public static bool is_empty(square sq, ref Board bd)
        {
            return bd.getSquare(sq) != piece.PAWN;
        }

        public static bool is_attacked(square sq, side sd, ref Board bd)
        {
            return (bd.getPieceBit(piece.PAWN, sd) & Attack.pawn_attacks_to(sd, sq)) != 0;
        }

        public static bool is_controlled(square sq, side sd, ref Board bd)
        {
            UInt64 attackers = bd.getPieceBit(piece.PAWN, sd) & Attack.pawn_attacks_to(sd, sq);
            UInt64 defenders = bd.getPieceBit(piece.PAWN, Side.Opposit(sd)) &
                               Attack.pawn_attacks_to(Side.Opposit(sd), sq);

            return Bit.Count(attackers) > Bit.Count(defenders);
        }

        public static bool is_safe(square sq, side sd, ref Board bd)
        {
            return is_empty(sq, ref bd) && !is_controlled(sq, Side.Opposit(sd),ref bd);
        }

        public static UInt64 potential_attacks(square sq, side sd, ref Board bd)
        {
            int pInc = Square.PawnInc(sd);

            UInt64 attacks = Attack.pawn_attacks_from(sd, sq);

            for (sq += pInc; !Square.IsPromotion(sq) && is_safe(sq, sd, ref bd); sq += pInc)
            {
                attacks |= Attack.pawn_attacks_from(sd, sq);
            }

            return attacks;
        }

        public static bool is_duo(square sq, side sd, ref Board bd)
        {
            return (bd.getPieceBit(piece.PAWN, sd) & duo[(int)sq]) != 0;
        }

        public static bool is_isolated(square sq, side sd, ref Board bd)
        {
            file fl = Square.File(sq);
            UInt64 files = Bit.Files(fl) & ~Bit.File(fl);

            return (bd.getPieceBit(piece.PAWN, sd) & files) == 0;
        }

        public static bool is_weak(square sq, side sd, ref Board bd)
        {
            file fl = Square.File(sq);
            rank rk = Square.Rank(sq, sd);

            UInt64 pawns = bd.getPieceBit(piece.PAWN, sd);
            int pInc = Square.PawnInc(sd);

            // already fine?

            if ((pawns & duo[(int)sq]) != 0)
            {
                return false;
            }

            if (is_attacked(sq, sd,ref bd))
            {
                return false;
            }

            // can advance next to other pawn in one move?

            square s1 = sq + pInc;
            square s2 = s1 + pInc;

            if ((pawns & duo[(int)s1]) != 0 && is_safe(s1, sd, ref bd))
            {
                return false;
            }

            if (rk == rank.RANK_2 && (pawns & duo[(int)s2]) != 0 && is_safe(s1, sd, ref bd) && is_safe(s2, sd, ref bd))
            {
                return false;
            }

            // can be defended in one move?

            if (fl != file.FILE_A)
            {
                square s0 = sq + (int)inc.INC_LEFT;
                s1 = s0 - pInc;
                s2 = s1 - pInc;
                square s3 = s2 - pInc;
                
                if (s2 > square.NONE && (int)s2 < Square.SIZE && bd.square_is(s2, piece.PAWN, sd) && is_safe(s1, sd, ref bd))
                {
                    return false;
                }
                
                if (rk == rank.RANK_5 && bd.square_is(s3, piece.PAWN, sd) && is_safe(s2, sd, ref bd) &&
                    is_safe(s1, sd, ref bd))
                {
                    return false;
                }
            }

            if (fl != file.FILE_H)
            {
                square s0 = sq + (int)inc.INC_RIGHT;
                s1 = s0 - pInc;
                s2 = s1 - pInc;
                square s3 = s2 - pInc;
                
                if (s2 > square.NONE && (int)s2 < Square.SIZE && bd.square_is(s2, piece.PAWN, sd) && is_safe(s1, sd, ref bd))
                {
                    return false;
                }

                if (rk == rank.RANK_5 && bd.square_is(s3, piece.PAWN, sd) && is_safe(s2, sd, ref bd) &&
                    is_safe(s1, sd, ref bd))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool is_doubled(square sq, side sd, ref Board bd)
        {
            file fl = Square.File(sq);
            return (bd.getPieceBit(piece.PAWN, sd) & Bit.File(fl) & Bit.Rear(sq, sd)) != 0;
        }

        public static bool is_bplocked(square sq, side sd, ref Board bd)
        {
            return !is_safe(Square.Stop(sq, sd), sd, ref bd) && !is_attacked(sq, Side.Opposit(sd), ref bd);
        }

        public static int shelter_file(file fl, side sd, ref Board bd)
        {
           

            if (false)
            {
            }
            else if (bd.square_is(Square.Make(fl, rank.RANK_2, sd), piece.PAWN, sd))
            {
                return 2;
            }
            else if (bd.square_is(Square.Make(fl, rank.RANK_3, sd), piece.PAWN, sd))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static int shelter_files(file fl, side sd, ref Board bd)
        {
            file fl_left = (fl == file.FILE_A) ? fl + 1 : fl - 1;
            file fl_right = (fl == file.FILE_H) ? fl - 1 : fl + 1;

            int sc = shelter_file(fl, sd, ref bd) * 2 + shelter_file(fl_left, sd, ref bd) + shelter_file(fl_right, sd, ref bd);
            Debug.Assert(sc >= 0 && sc <= 8);

            return sc;
        }

        public static void comp_info(ref Info info, ref Board bd)
        {
            info.passed = 0;
            info.safe = 0;

            info.mg = 0;
            info.eg = 0;

            info.left_file = file.FILE_H + 1;
            info.right_file = file.FILE_A - 1;

            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                info.target[sd] = 0;
            }

            UInt64 weak = 0;
            UInt64 strong = 0;
            UInt64[] safe = new UInt64[Side.SIZE] {
                ~0UL, ~0UL
            }
            ;

            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                int p12 = Piece.Make(piece.PAWN,(side) sd);

                strong |= bd.getPieceBit(piece.PAWN, (side)sd) & Attack.pawn_attacks_from((side)sd, ref bd); // defended pawns

                {
                    int n = bd.count(piece.PAWN, (side)sd);
                    info.mg += n * Material.score(piece.PAWN, Stage.stage.MG);
                    info.eg += n * Material.score(piece.PAWN, Stage.stage.EG);
                }

                for (UInt64 b = bd.getPieceBit(piece.PAWN, (side)sd); b != 0; b = Bit.Rest(b))
                {
                    square sq =(square) Bit.First(b);

                    file fl = Square.File(sq);
                    rank rk = Square.Rank(sq, (side)sd);

                    if (fl < info.left_file) info.left_file = fl;
                    if (fl > info.right_file) info.right_file = fl;

                    info.mg += PieceSquareTable.score(p12, sq, Stage.stage.MG);
                    info.eg += PieceSquareTable.score(p12, sq, Stage.stage.EG);

                    if (is_isolated(sq, (side)sd, ref bd))
                    {
                        Bit.Set(ref weak, sq);

                        info.mg -= 10;
                        info.eg -= 20;
                    }
                    else if (is_weak(sq, (side)sd, ref bd))
                    {
                        Bit.Set(ref weak, sq);

                        info.mg -= 5;
                        info.eg -= 10;
                    }

                    if (is_doubled(sq, (side)sd, ref bd))
                    {
                        info.mg -= 5;
                        info.eg -= 10;
                    }

                    if (is_passed(sq,(side) sd, ref bd))
                    {
                        Bit.Set(ref info.passed, sq);

                        info.mg += 10;
                        info.eg += 20;

                        if (rk >= rank.RANK_5)
                        {
                            square stop= Square.Stop(sq, (side)sd);
                            if (is_duo(sq, (side)sd, ref bd) && rk <= rank.RANK_6)
                                stop += Square.PawnInc((side)sd); // stop one line "later" for duos
                            Bit.Set(ref info.target[(int)Side.Opposit((side)sd)], stop);
                        }
                    }

                    safe[(int)Side.Opposit((side)sd)] &= ~potential_attacks(sq, (side)sd, ref bd);
                }

                for (int fl = 0; fl < Square.FILE_SIZE; fl++)
                {
                    info.shelter[fl][sd] = shelter_files((file)fl, (side)sd, ref bd) * 4;
                }

                info.mg = -info.mg;
                info.eg = -info.eg;
            }

            weak &= ~strong; // defended doubled pawns are not weak
            Debug.Assert((weak & strong) == 0);

            info.target[(int)side.WHITE] |= bd.getPieceBit(piece.PAWN, side.BLACK) & weak;
            info.target[(int)side.BLACK] |= bd.getPieceBit(piece.PAWN, side.WHITE) & weak;

            info.safe = (safe[(int)side.WHITE] & Bit.Front(rank.RANK_4))
                        | (safe[(int)side.BLACK] & Bit.Rear(rank.RANK_5));

            if (info.left_file > info.right_file)
            {
                // no pawns
                info.left_file = file.FILE_A;
                info.right_file = file.FILE_H;
            }

            Debug.Assert(info.left_file <= info.right_file);

            // file "openness"

            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                for (int fl = 0; fl < Square.FILE_SIZE; fl++)
                {
                    UInt64 file = Bit.File((file)fl);

                    int open;

                    if (false)
                    {
                    }
                    else if ((bd.getPieceBit(piece.PAWN,(side) sd) & file) != 0)
                    {
                        open = 0;
                    }
                    else if ((bd.getPieceBit(piece.PAWN, Side.Opposit((side)sd)) & file) == 0)
                    {
                        open = 4;
                    }
                    else if ((strong & file) != 0)
                    {
                        open = 1;
                    }
                    else if ((weak & file) != 0)
                    {
                        open = 3;
                    }
                    else
                    {
                        open = 2;
                    }

                    info.open[fl][sd] = open * 5;
                }
            }
        }

        public static void init()
        {
            for (int sq = 0; sq < Square.SIZE; sq++)
            {
                passed_me[(int) sq] = new UInt64[Side.SIZE];
                passed_opp[(int)sq] = new UInt64[Side.SIZE];
                file fl = Square.File((square)sq);
                rank rk = Square.Rank((square)sq);

                passed_me[(int)sq][(int)side.WHITE] = Bit.File(fl) & Bit.Front(rk);
                passed_me[(int)sq][(int)side.BLACK] = Bit.File(fl) & Bit.Rear(rk);

                passed_opp[(int)sq][(int)side.WHITE] = Bit.Files(fl) & Bit.Front(rk);
                passed_opp[(int)sq][(int)side.BLACK] = Bit.Files(fl) & Bit.Rear(rk);

                UInt64 b = 0;
                if (fl != file.FILE_A) Bit.Set(ref b, (square)(sq + (int)inc.INC_LEFT));
                if (fl != file.FILE_H) Bit.Set(ref b, (square)(sq + (int)inc.INC_RIGHT));
                duo[(int)sq] = b;
            }
        }
    }
}