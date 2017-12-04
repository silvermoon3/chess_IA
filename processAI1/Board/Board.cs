using System;
using System.Diagnostics;
using System.Linq;
using processAI1.Board.Bitboard;

namespace processAI1.Board
{
    public struct Copy
    {
        public UInt64 key;
        public UInt64 pawn_key;
        public int flags;
        public int ep_sq;
        public int moves;
        public int recap;
        public int phase;
    };

    public struct Undo
    {
        public Copy copy;
        public Move move;
        public int cap_sq;
        public bool castling;
    };


    public class Board
    {
        public static string start_fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -";

        public static int SCORE_NONE = -10000; // HACK because "score::NONE" is defined later

        //Position par type de piece
        UInt64[] p_piece = new UInt64[Piece.SIZE];
        
        //position par side
        UInt64[] p_side = new UInt64[Side.SIZE];
        //representation de toutes les pieces
        UInt64 p_all;

        int[] p_king = new int[Side.SIZE];
        int[] p_count = new int[Piece.SIDE_SIZE];

        int[] p_square = new int[Square.SIZE];
        int p_turn;
        Copy p_copy;

        int p_root;
        int p_sp;
        Undo[] p_stack = new Undo[1024];

        public Board()
        {
            
        }
        public Board(Board bd)
        {
            for (int pc = 0; pc < Piece.SIZE; pc++)
            {
                p_piece[pc] = bd.p_piece[pc];
            }

            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                p_side[sd] = bd.p_side[sd];
                p_king[sd] = bd.p_king[sd];
            }

            p_all = bd.p_all;

            for (int p12 = 0; p12 < Piece.SIDE_SIZE; p12++)
            {
                p_count[p12] = bd.p_count[p12];
            }

            for (int sq = 0; sq < Square.SIZE; sq++)
            {
                p_square[sq] = bd.p_square[sq];
            }

            p_turn = bd.p_turn;
            p_copy = bd.p_copy;

            p_root = bd.p_root;
            p_sp = bd.p_sp;

            for (int sp = 0; sp < bd.p_sp; sp++)
            {
                p_stack[sp] = bd.p_stack[sp];
            }

            Debug.Assert(moves() == bd.moves());
        }


        public UInt64 piece(int pc)
        {
            Util.Assert(pc<Piece.SIZE, "pc<Piece.SIZE");
            Util.Assert(pc != (int)Piece.piece.NONE, "pc != (int)Piece.piece.NONE");
            return p_piece[pc];
        }

        public UInt64 piece(int pc, int sd)
        {
            Util.Assert(pc<Piece.SIZE, "pc<Piece.SIZE");
            Util.Assert(pc !=(int) Piece.piece.NONE, "pc !=(int) Piece.piece.NONE");

            return p_piece[pc] & p_side[sd];
        }

        public int count(int pc, int sd)
        {

            Debug.Assert(pc<Piece.SIZE);
            Debug.Assert(pc != (int)Piece.piece.NONE);

            // return bit::count(piece(pc, sd));
            return p_count[Piece.Make(pc, sd)];
        }

        public UInt64 side(int sd)
        {
            return p_side[sd];
        }

        public UInt64 pieces(int sd)
        {
            return p_side[sd] & ~piece((int) Piece.piece.PAWN, sd);
        }

        public UInt64 all()
        {
            return p_all;
        }

        public UInt64 empty()
        {
            return ~p_all;
        }

        public int square(int sq)
        {
            return p_square[sq];
        }

        public int square_side(int sq)
        {
            Debug.Assert(p_square[sq] != (int)Piece.piece.NONE);
            return (int) ((p_side[Side.BLACK] >> sq) & 1); // HACK: uses Side internals
        }

        public bool square_is(int sq, int pc, int sd)
        {

            Debug.Assert(pc<Piece.SIZE);
            Debug.Assert(pc != (int)Piece.piece.NONE);

            return square(sq) == pc && square_side(sq) == sd;
        }

        public int king(int sd)
        {
            int sq = p_king[sd];
            Debug.Assert(sq == Bit.First(piece((int)Piece.piece.KING, sd)));
            return sq;
        }

        public int turn()
        {
            return p_turn;
        }

        public UInt64 key()
        {
            UInt64 key = p_copy.key;
            key ^= Castling.flags_key[p_copy.flags];
            key ^= Hash.EnPassantKey(p_copy.ep_sq);
            return key;
        }

        public UInt64 pawn_key()
        {
            return p_copy.pawn_key;
        }

        public UInt64 eval_key()
        {
            UInt64 key = p_copy.key;
            key ^= Hash.TurnKey(p_turn); // remove incremental STM
            key ^= Castling.flags_key[p_copy.flags];
            return key;
        }

        public int flags()
        {
            return p_copy.flags;
        }

        public int ep_sq()
        {
            return p_copy.ep_sq;
        }

        public int moves()
        {
            return p_copy.moves;
        }

        public int recap()
        {
            return p_copy.recap;
        }

        public int phase()
        {
            return p_copy.phase;
        }

        public int ply()
        {
            Debug.Assert(p_sp >= p_root);
            return p_sp - p_root;
        }

        public Move last_move()
        {
            return (p_sp == 0) ? null : p_stack[p_sp - 1].move;
        }

        public bool is_draw()
        {
            if (p_copy.moves > 100)
            {
                // TODO: check for mate
                return true;
            }

            UInt64 key = p_copy.key; // HACK: ignores castling flags and e.p. square
            
            Debug.Assert(p_copy.moves <= p_sp);

            for (int i = 4; i < p_copy.moves; i += 2)
            {
                if (p_stack[p_sp - i].copy.key == key)
                {
                    return true;
                }
            }

            return false;
        }

        public void set_root()
        {
            p_root = p_sp;
        }

        public void clear()
        {
            for (int pc = 0; pc < Piece.SIZE; pc++)
            {
                p_piece[pc] = 0;
            }

            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                p_side[sd] = 0;
            }

            for (int sq = 0; sq < Square.SIZE; sq++)
            {
                p_square[sq] = (int)Piece.piece.NONE;
            }

            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                p_king[sd] = (int)Square.square.NONE;
            }

            for (int p12 = 0; p12 < Piece.SIDE_SIZE; p12++)
            {
                p_count[p12] = 0;
            }

            p_turn = Side.WHITE;

            p_copy.key = 0;
            p_copy.pawn_key = 0;
            p_copy.flags = 0;
            p_copy.ep_sq = (int)Square.square.NONE;
            p_copy.moves = 0;
            p_copy.recap = (int)Square.square.NONE;
            p_copy.phase = 0;

            p_root = 0;
            p_sp = 0;
        }

        public void clear_square(int pc, int sd, int sq, bool update_copy)
        {
            Debug.Assert(pc < Piece.SIZE);
            Debug.Assert(pc != (int) Piece.piece.NONE);
            Debug.Assert(sq >= 0 && sq < Square.SIZE);

            Debug.Assert(pc == p_square[sq]);

            Debug.Assert(Bit.IsSet(p_piece[pc], sq));
            Bit.Clear(ref p_piece[pc], sq);

            Debug.Assert(Bit.IsSet(p_side[sd], sq));
            Bit.Clear(ref p_side[sd], sq);

            Debug.Assert(p_square[sq] != (int)Piece.piece.NONE);
            p_square[sq] = (int) Piece.piece.NONE;

            int p12 =Piece.Make(pc, sd);

            Debug.Assert(p_count[p12] != 0);
            p_count[p12]--;

            if (update_copy)
            {
                UInt64 key = Hash.PieceKey(p12, sq);
                p_copy.key ^= key;
                if (pc == (int)Piece.piece.PAWN) p_copy.pawn_key ^= key;

                p_copy.phase -= Material.phase(pc);
            }
        }

        public void set_square(int pc, int sd, int sq, bool update_copy)
        {
            Debug.Assert(pc < Piece.SIZE);
            Debug.Assert(pc != (int)Piece.piece.NONE);
            Debug.Assert(sq >= 0 && sq < Square.SIZE);

            Debug.Assert(!Bit.IsSet(p_piece[pc], sq));
            Bit.Set(ref p_piece[pc], sq);

            Debug.Assert(!Bit.IsSet(p_side[sd], sq));
            Bit.Set(ref p_side[sd], sq);

            Debug.Assert(p_square[sq] == (int)Piece.piece.NONE);
            p_square[sq] = pc;

            if (pc == (int)Piece.piece.KING)
            {
                p_king[sd] = sq;
            }

            int p12 = Piece.Make(pc, sd);

            p_count[p12]++;

            if (update_copy)
            {
                UInt64 key = Hash.PieceKey(p12, sq);
                p_copy.key ^= key;
                if (pc == (int)Piece.piece.PAWN) p_copy.pawn_key ^= key;

                p_copy.phase += Material.phase(pc);
            }
        }
        /// <summary>
        /// Move one piece to another square
        /// </summary>
        /// <param name="pc">piece type</param>
        /// <param name="sd">side</param>
        /// <param name="f">Starting square</param>
        /// <param name="t">ending square </param>
        /// <param name="update_copy"></param>
        public void move_square(int pc, int sd, int f, int t, bool update_copy)
        {
            // TODO
            clear_square(pc, sd, f, update_copy);
            set_square(pc, sd, t, update_copy);
        }

        //Change side
        public void flip_turn()
        {
            p_turn = Side.Opposit(p_turn);
            p_copy.key ^= Hash.TurnFlip();
        }

        // Update p_all var each time p_side is modified
        public void update()
        {
            p_all = p_side[Side.WHITE] | p_side[Side.BLACK];
        }

        public bool can_castle(int index)
        {
            int sd = Castling.side(index);

            return square_is(Castling.info[index].kf, (int)Piece.piece.KING, sd)
                   && square_is(Castling.info[index].rf, (int)Piece.piece.ROOK, sd);
        }

        public bool pawn_is_attacked(int sq, int sd)
        {
            int fl = Square.File(sq);
            sq -= Square.PawnInc(sd);

            return (fl != (int)Square.file.FILE_A && square_is(sq + (int)Square.inc.INC_LEFT, (int)Piece.piece.PAWN, sd))
                   || (fl != (int)Square.file.FILE_H && square_is(sq + (int)Square.inc.INC_RIGHT, (int)Piece.piece.PAWN, sd));
        }

        public void init_fen(ref string s)
        {
            clear();

            int pos = 0;

            // piece placement

            int sq = 0;

            while (pos < s.Length)
            {
                Debug.Assert(sq <= Square.SIZE);

                char c = s[pos++];

                if (false)
                {
                }
                else if (c == ' ')
                {
                    break;
                }
                else if (c == '/')
                {
                    continue;
                }
                else if (Char.IsDigit(c))
                {
                    sq += c - '0';
                }
                else
                {
                    // assume piece

                    int p12 = Piece.FromFen(c);
                    int pc = Piece.PieceType(p12);
                    int sd = Piece.PieceSide(p12);
                    set_square(pc, sd, Square.FromFen(sq), true);
                    sq++;
                }
            }

            Debug.Assert(sq == Square.SIZE);

            // turn

            p_turn = Side.WHITE;

            if (pos < s.Length)
            {
                p_turn = "wb".IndexOf(s[pos++]);

                if (pos < s.Length)
                {
                    Debug.Assert(s[pos] == ' ');
                    pos++;
                }
            }

            p_copy.key ^= Hash.TurnKey(p_turn);

            // castling flags

            p_copy.flags = 0;

            if (pos < s.Length)
            {
                // read from FEN

                while (pos < s.Length)
                {
                    char c = s[pos++];
                    if (c == ' ') break;
                    if (c == '-') continue;

                    int index = "KQkq".IndexOf(c);

                    if (can_castle(index))
                    {
                        Castling.set_flag(ref p_copy.flags, index);
                    }
                }
            }
            else
            {
                // guess from position

                for (int index = 0; index < 4; index++)
                {
                    if (can_castle(index))
                    {
                        Castling.set_flag(ref p_copy.flags, index);
                    }
                }
            }

            // en-passant square

            p_copy.ep_sq = (int) Square.square.NONE;

            if (pos < s.Length)
            {
                // read from FEN

                string ep_string = "";

                while (pos < s.Length)
                {
                    char c = s[pos++];
                    if (c == ' ') break;

                    ep_string += c;
                }

                if (ep_string != "-")
                {
                    sq = Square.FromString(ref ep_string);

                    if (pawn_is_attacked(sq, p_turn))
                    {
                        p_copy.ep_sq = sq;
                    }
                }
            }

            update();
        }

        public void move(Move mv)
        {
            Debug.Assert(mv != null);

            int sd = p_turn;
            int xd = Side.Opposit(sd);

            int f = mv.GetFrom();
            int t = mv.GetTo();

            int pc = mv.GetPieceMoving();
            int cp = mv.GetCapturedPiece();
            int pp = mv.GetPromoted();

            Debug.Assert(p_square[f] == pc);
            Debug.Assert(square_side(f) == sd);

            Debug.Assert(p_sp < 1024);
            int poss = p_sp++;

            p_stack[poss].copy = p_copy;
            p_stack[poss].move = mv;
            p_stack[poss].castling = false;

            p_copy.moves++;
            p_copy.recap = (int)Square.square.NONE;

            // capture

            Debug.Assert(cp != (int)Piece.piece.KING);

            if (pc == (int)Piece.piece.PAWN && t == p_copy.ep_sq)
            {
                int cap_sq = t - Square.PawnInc(sd);
                Debug.Assert(p_square[cap_sq] == cp);
                Debug.Assert(cp == (int)Piece.piece.PAWN);

                p_stack[poss].cap_sq = cap_sq;

                clear_square(cp, xd, cap_sq, true);
            }
            else if (cp != (int)Piece.piece.NONE)
            {
                Debug.Assert(p_square[t] == cp);
                Debug.Assert(square_side(t) == xd);

                p_stack[poss].cap_sq = t;

                clear_square(cp, xd, t, true);
            }
            else
            {
                Debug.Assert(p_square[t] == cp);
            }

            // promotion

            if (pp != (int)Piece.piece.NONE)
            {
                Debug.Assert(pc == (int)Piece.piece.PAWN);
                clear_square((int)Piece.piece.PAWN, sd, f, true);
                set_square(pp, sd, t, true);
            }
            else
            {
                move_square(pc, sd, f, t, true);
            }

            // castling rook

            if (pc == (int)Piece.piece.KING && Math.Abs(t - f) == Square.CASTLING_DELTA)
            {
                p_stack[poss].castling = true;

                int wg = (t > f) ? (int)Wing.wing.KING : (int)Wing.wing.QUEEN;
                int index = Castling.index(sd, wg);

                Debug.Assert(Castling.flag(p_copy.flags, index));

                Debug.Assert(f == Castling.info[index].kf);
                Debug.Assert(t == Castling.info[index].kt);

                move_square((int)Piece.piece.ROOK, sd, Castling.info[index].rf, Castling.info[index].rt, true);
            }

            // turn

            flip_turn();

            // castling flags

            p_copy.flags &= ~Castling.flags_mask[f];
            p_copy.flags &= ~Castling.flags_mask[t];

            // en-passant square

            p_copy.ep_sq = (int)Square.square.NONE;

            if (pc == (int)Piece.piece.PAWN &&  Math.Abs(t - f) == Square.DOUBLE_PAWN_DELTA)
            {
                int sq = (f + t) / 2;
                if (pawn_is_attacked(sq, xd))
                {
                    p_copy.ep_sq = sq;
                }
            }

            // move counter

            if (cp != (int)Piece.piece.NONE || pc == (int)Piece.piece.PAWN)
            {
                p_copy.moves = 0; // conversion;
            }

            // recapture

            if (cp != (int)Piece.piece.NONE || pp != (int)Piece.piece.NONE)
            {
                p_copy.recap = t;
            }

            update();
        }

        public void undo()
        {
            Debug.Assert(p_sp > 0);
            int poss = --p_sp;

            Move mv = p_stack[poss].move;

            int f = mv.GetFrom();
            int t = mv.GetTo();

            int pc = mv.GetPieceMoving();
            int cp = mv.GetCapturedPiece();
            int pp = mv.GetPromoted();

            int xd = p_turn;
            int sd = Side.Opposit(xd);

            Debug.Assert(p_square[t] == pc || p_square[t] == pp);
            Debug.Assert(square_side(t) == sd);

            // castling rook

            if (p_stack[poss].castling)
            {
                int wg = (t > f) ? (int)Wing.wing.KING : (int)Wing.wing.QUEEN;
                int index = Castling.index(sd, wg);

                Debug.Assert(f == Castling.info[index].kf);
                Debug.Assert(t == Castling.info[index].kt);

                move_square((int)Piece.piece.ROOK, sd, Castling.info[index].rt, Castling.info[index].rf, false);
            }

            // promotion

            if (pp != (int)Piece.piece.NONE)
            {
                Debug.Assert(pc == (int)Piece.piece.PAWN);
                clear_square(pp, sd, t, false);
                set_square((int)Piece.piece.PAWN, sd, f, false);
            }
            else
            {
                move_square(pc, sd, t, f, false);
            }

            // capture

            if (cp != (int)Piece.piece.NONE)
            {
                set_square(cp, xd, p_stack[poss].cap_sq, false);
            }

            flip_turn();
            p_copy = p_stack[poss].copy;

            update();
        }

        public void move_null()
        {
            Debug.Assert(p_sp < 1024);
            int poss = p_sp++;

            p_stack[poss].move = null;

            p_stack[poss].copy = p_copy;

            flip_turn();
            p_copy.ep_sq = (int)Square.square.NONE;
            p_copy.moves = 0; // HACK: conversion
            p_copy.recap = (int)Square.square.NONE;

            update();
        }

        public void undo_null()
        {
            Debug.Assert(p_sp > 0);
            int poss = --p_sp;

            Debug.Assert(p_stack[poss].move == null);

            flip_turn();
            p_copy = p_stack[poss].copy;

            update();
        }

        public override string ToString()
        {
            string[] s= new string[8];
            
            for (int sq = Square.SIZE - 1; sq >= 0; sq--)
            {
                Boolean pieceFound = false;
                for (int sd = 0; sd < Side.SIZE; sd++)
                {
                    for (int pc = 0; pc < Piece.SIZE-1; pc++)
                    {
                        if (s[7-Square.Rank(sq)] == null)
                            s[7-Square.Rank(sq)] = "";
                        if (square_is(sq, pc, sd))
                        {
                            pieceFound = true;
                            s[7-Square.Rank(sq)] += "[" + Piece.ToFen(Piece.Make(pc, sd)) + "]";
                        }
                           
                        
                    }
                }
                if (!pieceFound)
                    s[7-Square.Rank(sq)]+= "[" + Piece.ToChar((int)Piece.piece.NONE)+ "]";
            }
            return String.Join("\n", s);
        }
    }
}