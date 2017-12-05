using System;
using System.ComponentModel;
using System.Diagnostics;
using processAI1.Board.Bitboard;

namespace processAI1.Board
{
    public class Attacks
    {
        public int size;

        // 2 
        public square[] square;

        public UInt64 avoid;
        public UInt64 pinned;

        public Attacks()
        {
            square = new square[2];
        }
    };

    public static class Attack
    {
        

        //Possible Moves of Each piece by 0x88 coordinates offsets

        // Pawn Move direction by sides
        public static readonly int[] Pawn_Move = new int[Side.SIZE] {+1, -1};

        //Pawn Move direction for Captures Left and Right By sides
        public static readonly int[][] Pawn_Attack = new int[Side.SIZE][] {new[] {-15, +17}, new[] {-17, +15}};

        //Other pieces moves offsets
        public static readonly int[] Knight_Inc = {-33, -31, -18, -14, +14, +18, +31, +33, 0};

        public static readonly int[] Bishop_Inc = {-17, -15, +15, +17, 0};
        public static readonly int[] Rook_Inc = {-16, -1, +1, +16, 0};
        public static readonly int[] Queen_Inc = {-17, -16, -15, -1, +1, +15, +16, +17, 0};

        //all moves directions for better looping
        public static int[][] Piece_Inc = {null, Knight_Inc, Bishop_Inc, Rook_Inc, Queen_Inc, Queen_Inc, null};


        /// <summary>
        /// Pre-calculated moves and attacks for pawns [Side][Square]
        /// https://chessprogramming.wikispaces.com/On+an+empty+Board
        /// </summary>
        public static UInt64[][] Pawn_Moves;

        public static UInt64[][] Pawn_Attacks;


        /// <summary>
        /// Pre-calculated moves and attacks for all pieces [PieceType][Square]
        /// https://chessprogramming.wikispaces.com/On+an+empty+Board
        /// </summary>
        public static UInt64[][] Piece_Attacks;

        /// <summary>
        /// Pre-calculated Blockers for pieces except pawns [PieceType][Square]
        /// https://chessprogramming.wikispaces.com/Blockers+and+Beyond
        /// </summary>
        public static UInt64[][] Blockers;


        public static UInt64[][] Between;

        public static UInt64[][] Behind;


        /// <summary>
        /// Init all pre-calculated attacks and moves bitboards
        /// </summary>
        public static void init()
        {
            //pawns moves and attacks bitboards init
            Pawn_Moves = new UInt64[Side.SIZE][];
            Pawn_Attacks = new UInt64[Side.SIZE][];
            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                Pawn_Moves[(int)sd] = new UInt64[Square.SIZE];
                Pawn_Attacks[(int)sd] = new UInt64[Square.SIZE];
                for (int sq = 0; sq < Square.SIZE; sq++)
                {
                    Pawn_Moves[(int)sd][sq] = pawn_moves_debug((side)sd, (square)sq);
                    Pawn_Attacks[(int)sd][sq] = pawn_attacks_debug((side)sd, (square)sq);
                }
            }

            //Other pieces attacks and Blockers init
            Piece_Attacks = new UInt64[Piece.SIZE][];
            Blockers = new UInt64[Piece.SIZE][];
            for (int pc = (int)piece.KNIGHT; pc <= (int)piece.KING; pc++)
            {
                Piece_Attacks[(int)pc] = new UInt64[Square.SIZE];
                Blockers[(int)pc] = new UInt64[Square.SIZE];
                for (int sq = 0; sq < Square.SIZE; sq++)
                {
                    Piece_Attacks[(int)pc][sq] = piece_attacks_debug((piece)pc, (square)sq);
                    Blockers[(int)pc][sq] = blockers_debug((piece)pc, (square)sq);
                }
            }

            Between = new UInt64[Square.SIZE][];
            Behind = new UInt64[Square.SIZE][];
            for (int f = 0; f < Square.SIZE; f++)
            {
                Between[(int)f] = new UInt64[Square.SIZE];
                Behind[(int)f] = new UInt64[Square.SIZE];
                for (int t = 0; t < Square.SIZE; t++)
                {
                    Between[(int)f][(int)t] = between_debug( (square)f, (square)t);
                    Behind[(int)f][(int)t] = behind_debug( (square)f, (square)t);
                }
            }
        }

        /// <summary>
        /// Check if line between the to and from square is empty
        /// </summary>
        /// <param name="f">int from : 0...63</param>
        /// <param name="t">square to : 0...63</param>
        /// <param name="bd">ref board</param>
        /// <returns></returns>
        public static bool line_is_empty(square f, square t, ref Board bd)
        {
            return (bd.all() & Between[(int)f][(int)t]) == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f">int From: 0...63</param>
        /// <param name="t">square to: 0....63</param>
        /// <returns></returns>
        public static UInt64 ray(square f, square t)
        {
            return Between[(int)f][(int)t] | Behind[(int)f][(int)t]; // HACK: t should be included
        }

        /// <summary>
        /// check if it's a pawn move
        /// </summary>
        /// <param name="sd">int side: 0...1</param>
        /// <param name="f">int From: 0...63</param>
        /// <param name="t">square to 0...63</param>
        /// <param name="bd">ref board</param>
        /// <returns></returns>
        public static bool pawn_move(side sd, square f, square t, ref Board bd)
        {
            return Bit.IsSet(Pawn_Moves[(int)sd][(int)f], t) && line_is_empty(f, t, ref bd);
        }

        /// <summary>
        /// check if pawn attack
        /// </summary>
        /// <param name="sd">int side: 0...1</param>
        /// <param name="f">int From: 0...63</param>
        /// <param name="t">square to 0...63</param>
        /// <returns></returns>
        public static bool pawn_attack(side sd, square f, square t)
        {
            return Bit.IsSet(Pawn_Attacks[(int)sd][(int)f], t);
        }

        /// <summary>
        /// check if piece attack
        /// </summary>
        /// <param name="pc">type piece : 0...6</param>
        /// <param name="f">int from : 0...63</param>
        /// <param name="t">square to : 0...63</param>
        /// <param name="bd">ref board</param>
        /// <returns></returns>
        public static bool piece_attack(piece pc, square f, square t, ref Board bd)
        {
            Debug.Assert(pc != (int)piece.PAWN);
            return Bit.IsSet(Piece_Attacks[(int)pc][(int)f], t) && line_is_empty(f, t, ref bd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="sd"></param>
        /// <param name="f"></param>
        /// <param name="t"></param>
        /// <param name="bd"></param>
        /// <returns></returns>
        public static bool attack(piece pc, side sd, square f, square t, ref Board bd)
        {

            if (pc == (int)piece.PAWN)
            {
                return pawn_attack(sd, f, t);
            }
            else
            {
                return piece_attack(pc, f, t, ref bd);
            }
        }

        public static UInt64 pawn_moves_from(side sd, ref Board bd)
        {
            // for pawn mobility

            UInt64 fs = bd.getPieceBit((int)piece.PAWN, sd);

            if (sd == (int)side.WHITE)
            {
                return fs << 1;
            }
            else
            {
                return fs >> 1;
            }
        }

        public static UInt64 pawn_moves_to(side sd, UInt64 ts, ref Board bd)
        {
            Debug.Assert((bd.all() & ts) == 0);

            UInt64 pawns = bd.getPieceBit((int)piece.PAWN, sd);
            UInt64 empty = bd.empty();

            UInt64 fs = 0;

            if (sd == (int)side.WHITE)
            {
                fs |= (ts >> 1);
                fs |= (ts >> 2) & (empty >> 1) & Bit.Rank(rank.RANK_2);
            }
            else
            {
                fs |= (ts << 1);
                fs |= (ts << 2) & (empty << 1) & Bit.Rank(rank.RANK_7);
            }

            return pawns & fs;
        }

        public static UInt64 pawn_attacks_from(side sd, ref Board bd)
        {
            UInt64 fs = bd.getPieceBit((int)piece.PAWN, sd);

            if (sd == (int)side.WHITE)
            {
                return (fs >> 7) | (fs << 9);
            }
            else
            {
                return (fs >> 9) | (fs << 7);
            }
        }

        public static UInt64 pawn_attacks_tos(side sd, UInt64 ts)
        {
            if (sd == (int)side.WHITE)
            {
                return (ts >> 9) | (ts << 7);
            }
            else
            {
                return (ts >> 7) | (ts << 9);
            }
        }

        public static UInt64 pawn_attacks_from(side sd, square f)
        {
            return Pawn_Attacks[(int)sd][(int)f];
        }

        public static UInt64 pawn_attacks_to(side sd, square t)
        {
            return pawn_attacks_from(Side.Opposit(sd), t);
        }

        public static UInt64 piece_attacks_from(piece pc, square f, ref Board bd)
        {
            Debug.Assert(pc != piece.PAWN);

            UInt64 ts = Piece_Attacks[(int)pc][(int)f];

            for (UInt64 b = bd.all() & Blockers[(int)pc][(int)f]; b != 0; b = Bit.Rest(b))
            {
                int sq = Bit.First(b);
                ts &= ~Behind[(int)f][sq];
            }

            return ts;
        }

        public static UInt64 piece_attacks_to(piece pc, square t, ref Board bd)
        {
            Debug.Assert(pc != (int)piece.PAWN);
            return piece_attacks_from(pc, t, ref bd);
        }

        public static UInt64 piece_moves_from(piece pc, side sd, square f, ref Board bd)
        {
            if (pc == (int)piece.PAWN)
            {
                Debug.Assert(false); // TODO: blockers
                return Pawn_Moves[(int)sd][(int)f];
            }
            else
            {
                return piece_attacks_from(pc, f, ref bd);
            }
        }

        public static UInt64 attacks_from(piece pc, side sd, square f, ref Board bd)
        {
            if (pc == (int)piece.PAWN)
            {
                return Pawn_Attacks[(int)sd][(int)f];
            }
            else
            {
                return piece_attacks_from(pc, f, ref bd);
            }
        }

        public static UInt64 attacks_to(piece pc, side sd, square t, ref Board bd)
        {
            return attacks_from(pc, Side.Opposit(sd), t, ref bd); // HACK for pawns
        }

        public static UInt64 pseudo_attacks_from(piece pc, side sd, square f)
        {
            if (pc == (int)piece.PAWN)
            {
                return Pawn_Attacks[(int)sd][(int)f];
            }
            else
            {
                return Piece_Attacks[(int)pc][(int)f];
            }
        }

        public static UInt64 pseudo_attacks_to(piece pc, side sd, square t)
        {
            return pseudo_attacks_from(pc, Side.Opposit(sd), t); // HACK for pawns
        }

        public static UInt64 slider_pseudo_attacks_to(side sd, square t, ref Board bd)
        {
            UInt64 b = 0;
            b |= bd.getPieceBit(piece.BISHOP, sd) & Piece_Attacks[(int)piece.BISHOP][(int)t];
            b |= bd.getPieceBit(piece.ROOK, sd) & Piece_Attacks[(int)piece.ROOK][(int)t];
            b |= bd.getPieceBit(piece.QUEEN, sd) & Piece_Attacks[(int)piece.QUEEN][(int)t];

            return b;
        }

        public static bool attack_behind(square f, square t, side sd, ref Board bd)
        {
            Debug.Assert(bd.getSquare(t) != piece.NONE);

            UInt64 behind = Behind[(int)f][(int)t];
            if (behind == 0) return false;

            for (UInt64 b = slider_pseudo_attacks_to(sd, t, ref bd) & behind; b != 0; b = Bit.Rest(b))
            {
                int sq = Bit.First(b);

                if (Bit.Single(bd.all() & Between[sq][(int)f]))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool is_attacked(square t, side sd, ref Board bd)
        {
            // non-sliders

            if ((bd.getPieceBit(piece.PAWN, sd) & Pawn_Attacks[(int)Side.Opposit(sd)][(int)t]) != 0)
            {
                // HACK
                return true;
            }

            if ((bd.getPieceBit(piece.KNIGHT, sd) & Piece_Attacks[(int)piece.KNIGHT][(int)t]) != 0)
            {
                return true;
            }

            if ((bd.getPieceBit(piece.KING, sd) & Piece_Attacks[(int)piece.KING][(int)t]) != 0)
            {
                return true;
            }

            // sliders

            for (UInt64 b = slider_pseudo_attacks_to(sd, t, ref bd); b != 0; b = Bit.Rest(b))
            {
                int f = Bit.First(b);

                if ((bd.all() & Between[(int)f][(int)t]) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static UInt64 pinned_by(square t, side sd, ref Board bd)
        {
            UInt64 pinned = 0;

            for (UInt64 b = slider_pseudo_attacks_to(sd, t, ref bd); b != 0; b = Bit.Rest(b))
            {
                int f = Bit.First(b);

                UInt64 bb = bd.all() & Between[(int)f][(int)t];

                if (bb != 0 && Bit.Single(bb))
                {
                    pinned |= bb;
                }
            }

            return pinned;
        }

        public static void init_attacks(ref Attacks attacks, side sd, ref Board bd)
        {
            // for strictly-legal moves

            //attacking side
            side atk = Side.Opposit(sd);
            //defending side
            side def = sd;

            //king of defending side position (int bit 0....63]
            square t = bd.king(def);

            attacks.size = 0;
            attacks.avoid = 0;
            attacks.pinned = 0;

            // non-sliders (pawn and k

            {
                UInt64 b = 0;
                b |= bd.getPieceBit(piece.PAWN, atk) & Pawn_Attacks[(int)def][(int)t]; // HACK
                b |= bd.getPieceBit(piece.KNIGHT, atk) & Piece_Attacks[(int)piece.KNIGHT][(int)t];

                if (b != 0)
                {
                    Debug.Assert(Bit.Single(b));
                    Debug.Assert(attacks.size < 2);
                    attacks.square[attacks.size++] = (square)Bit.First(b);
                }
            }

            // sliders

            for (UInt64 b = slider_pseudo_attacks_to(atk, t, ref bd); b != 0; b = Bit.Rest(b))
            {
                int f = Bit.First(b);

                UInt64 bb = bd.all() & Between[(int)f][(int)t];

                if (bb == 0)
                {
                    Debug.Assert(attacks.size < 2);
                    attacks.square[attacks.size++] = (square)f;
                    attacks.avoid |= ray( (square)f, (square)t);
                }
                else if (Bit.Single(bb))
                {
                    attacks.pinned |= bb;
                }
            }
        }

        public static void init_attacks(ref Attacks attacks, ref Board bd)
        {
            init_attacks(ref attacks, bd.turn(), ref bd);
        }

        public static bool is_legal(ref Board bd)
        {
            side atk = bd.turn();
            side def = Side.Opposit(atk);

            return !is_attacked(bd.king(def), atk, ref bd);
        }

        public static bool is_in_check(ref Board bd)
        {
            side atk = bd.turn();
            side def = Side.Opposit(atk);

            return is_attacked(bd.king(atk), def, ref bd);
        }

        public static UInt64 pawn_moves_debug(side sd, square sq)
        {
            UInt64 b = 0UL;

            int f = Square.To88(sq);
            int inc = Pawn_Move[(int)sd];

            int t = f + inc;

            if (Square.IsValid88(t))
            {
                Bit.Set(ref b, (square) Square.From88(t));
            }

            if (Square.Rank(sq, sd) == rank.RANK_2)
            {
                t += inc;
                Debug.Assert(Square.IsValid88(t));
                Bit.Set(ref b, (square) Square.From88(t));
            }

            return b;
        }

        public static UInt64 pawn_attacks_debug(side sd, square sq)
        {
            UInt64 b = 0;

            int f = Square.To88(sq);

            for (int dir = 0; dir < 2; dir++)
            {
                int t = f + Pawn_Attack[(int)sd][dir];
                if (Square.IsValid88(t))
                {
                    Bit.Set(ref b, (square) Square.From88(t));
                }
            }

            return b;
        }

        public static UInt64 piece_attacks_debug(piece pc, square sq)
        {
            Debug.Assert(pc != (int)piece.PAWN);

            UInt64 b = 0;

            int f = Square.To88(sq);

            for (int dir = 0; true; dir++)
            {
                int inc = Piece_Inc[(int)pc][dir];
                if (inc == 0) break;

                if (Piece.IsSlider(pc))
                {
                    for (int t = f + inc; Square.IsValid88(t); t += inc)
                    {
                        Bit.Set(ref b, (square) Square.From88(t));
                    }
                }
                else
                {
                    int t = f + inc;

                    if (Square.IsValid88(t))
                    {
                        Bit.Set(ref b, (square) Square.From88(t));
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

        public static UInt64 between_debug(square f, square t)
        {
            int f88 = Square.To88(f);
            int t88 = Square.To88(t);

            UInt64 b = 0;

            int inc = delta_inc( f88, t88);

            if (inc != 0)
            {
                for (int sq = f88 + inc; sq != t88; sq += inc)
                {
                    Bit.Set(ref b, (square) Square.From88(sq));
                }
            }

            return b;
        }

        public static UInt64 behind_debug(square f, square t)
        {
            int f88 = Square.To88(f);
            int t88 = Square.To88(t);

            UInt64 b = 0;

            int inc = delta_inc(f88, t88);

            if (inc != 0)
            {
                for (int sq = t88 + inc; Square.IsValid88(sq); sq += inc)
                {
                    Bit.Set(ref b, (square) Square.From88(sq));
                }
            }

            return b;
        }

        public static UInt64 blockers_debug(piece pc, square f)
        {
            Debug.Assert(pc != (int)piece.PAWN);

            UInt64 b = 0;

            UInt64 attacks = piece_attacks_debug(pc, f);

            for (UInt64 bb = attacks; bb != 0; bb = Bit.Rest(bb))
            {
                int sq = Bit.First(bb);
                if ((attacks & behind_debug(f, (square)sq)) != 0)
                {
                    Bit.Set(ref b, (square) sq);
                }
            }

            return b;
        }
    }
}