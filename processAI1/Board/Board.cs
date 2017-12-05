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
        public square ep_sq;
        public int moves;
        public square recap;
        public int phase;
    };

    public struct Undo
    {
        public Copy copy;
        public Move move;
        public square cap_sq;
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

        square[] p_king = new square[Side.SIZE];
        int[] p_count = new int[Piece.SIDE_SIZE];

        piece[] p_square = new piece[Square.SIZE];
        side p_turn;
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
                p_side[(int)sd] = bd.p_side[(int)sd];
                p_king[(int)sd] = bd.p_king[(int)sd];
            }

            p_all = bd.p_all;

            for (int p12 = 0; p12 < Piece.SIDE_SIZE; p12++)
            {
                p_count[p12] = bd.p_count[p12];
            }

            for (int sq = 0; sq < Square.SIZE; sq++)
            {
                p_square[(int)sq] = bd.p_square[(int)sq];
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


        public UInt64 getPieceBit(piece pc)
        {
            Util.Assert(pc != piece.NONE, "pc != piece.NONE");
            return p_piece[(int)pc];
        }

        public UInt64 getPieceBit(piece pc, side sd)
        {
            Util.Assert(pc != piece.NONE, "pc !=(int) piece.NONE");

            return p_piece[(int)pc] & p_side[(int)sd];
        }

        public int count(piece pc, side sd)
        {
            
            Debug.Assert(pc != piece.NONE);

            // return bit::count(piece(pc, sd));
            return p_count[Piece.Make(pc,sd)];
        }

        public UInt64 GetSide(side sd)
        {
            return p_side[(int)sd];
        }

        public UInt64 pieces(side sd)
        {
            return p_side[(int)sd] & ~getPieceBit((int) piece.PAWN, sd);
        }

        public UInt64 all()
        {
            return p_all;
        }

        public UInt64 empty()
        {
            return ~p_all;
        }

        public piece getSquare(square sq)
        {
            return (piece)p_square[(int)sq];
        }

        public side square_side(square sq)
        {
            Debug.Assert(p_square[(int)sq] != piece.NONE);
            return (side)((p_side[(int)side.BLACK] >> (int)sq) & 1); // HACK: uses Side internals
        }

        public bool square_is(square sq, piece pc, side sd)
        {
            
            Debug.Assert(pc != piece.NONE);

            return getSquare(sq) == pc && square_side(sq) == sd;
        }

        public square king(side sd)
        {
            square sq = p_king[(int)sd];
            Debug.Assert((int)sq == Bit.First(getPieceBit(piece.KING, sd)));
            return sq;
        }

        public side turn()
        {
            return p_turn;
        }

        public UInt64 key()
        {
            UInt64 key = p_copy.key;
            key ^= Castling.flags_key[(int)p_copy.flags];
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
            key ^= Castling.flags_key[(int)p_copy.flags];
            return key;
        }

        public int flags()
        {
            return p_copy.flags;
        }

        public square ep_sq()
        {
            return p_copy.ep_sq;
        }

        public int moves()
        {
            return p_copy.moves;
        }

        public square recap()
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
                p_square[(int)sq] = piece.NONE;
            }

            for (int sd = 0; sd < Side.SIZE; sd++)
            {
                p_king[sd] = square.NONE;
            }

            for (int p12 = 0; p12 < Piece.SIDE_SIZE; p12++)
            {
                p_count[p12] = 0;
            }

            p_turn = side.WHITE;

            p_copy.key = 0;
            p_copy.pawn_key = 0;
            p_copy.flags = 0;
            p_copy.ep_sq = square.NONE;
            p_copy.moves = 0;
            p_copy.recap = square.NONE;
            p_copy.phase = 0;

            p_root = 0;
            p_sp = 0;
        }

        public void clear_square(piece pc, side sd, square sq, bool update_copy)
        {
            Debug.Assert(pc != piece.NONE);
            Debug.Assert(sq != square.NONE);

            Debug.Assert(pc == p_square[(int)sq]);

            Debug.Assert(Bit.IsSet(p_piece[(int)pc], sq));
            Bit.Clear(ref p_piece[(int)pc], sq);

            Debug.Assert(Bit.IsSet(p_side[(int)sd], sq));
            Bit.Clear(ref p_side[(int)sd], sq);

            Debug.Assert(p_square[(int)sq] != piece.NONE);
            p_square[(int)sq] = piece.NONE;

            int p12 =Piece.Make(pc,sd);

            Debug.Assert(p_count[p12] != 0);
            p_count[p12]--;

            if (update_copy)
            {
                UInt64 key = Hash.PieceKey(p12, sq);
                p_copy.key ^= key;
                if (pc == (int)piece.PAWN) p_copy.pawn_key ^= key;

                p_copy.phase -= Material.phase(pc);
            }
        }

        public void set_square(piece pc, side sd, square sq, bool update_copy)
        {
            Debug.Assert(pc != piece.NONE);
            Debug.Assert(sq != square.NONE);

            Debug.Assert(!Bit.IsSet(p_piece[(int)pc], sq));
            
            Bit.Set(ref p_piece[(int)pc], (square) sq);

            Debug.Assert(!Bit.IsSet(p_side[(int)sd], sq));
            Bit.Set(ref p_side[(int)sd], (square) sq);

            Debug.Assert(p_square[(int)sq] == piece.NONE);
            p_square[(int)sq] = pc;

            if (pc == piece.KING)
            {
                p_king[(int)sd] = sq;
            }

            int p12 = Piece.Make(pc,sd);

            p_count[p12]++;

            if (update_copy)
            {
                UInt64 key = Hash.PieceKey(p12, sq);
                p_copy.key ^= key;
                if (pc == (int)piece.PAWN) p_copy.pawn_key ^= key;

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
        public void move_square(piece pc, side sd, square f, square t, bool update_copy)
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
            p_all = p_side[(int)side.WHITE] | p_side[(int)side.BLACK];
        }

        public bool can_castle(int index)
        {
            side sd = Castling.side(index);

            return square_is(Castling.info[index].kf, piece.KING, sd)
                   && square_is(Castling.info[index].rf, piece.ROOK, sd);
        }

        public bool pawn_is_attacked(square sq, side sd)
        {
            file fl = Square.File(sq);
            sq -= Square.PawnInc(sd);

            return (fl != file.FILE_A && square_is(sq + (int)inc.INC_LEFT, (int)piece.PAWN, sd))
                   || (fl != file.FILE_H && square_is(sq + (int)inc.INC_RIGHT, (int)piece.PAWN, sd));
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
                    piece pc = Piece.PieceType(p12);
                    side sd = Piece.PieceSide(p12);
                    set_square(pc, sd, Square.FromFen(sq), true);
                    sq++;
                }
            }

            Debug.Assert(sq == Square.SIZE);

            // turn

            p_turn = side.WHITE;

            if (pos < s.Length)
            {
                p_turn = (side)("wb".IndexOf(s[pos++]));

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

            p_copy.ep_sq = square.NONE;

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
                    sq = (int)Square.FromString(ref ep_string);

                    if (pawn_is_attacked((square)sq, p_turn))
                    {
                        p_copy.ep_sq = (square)sq;
                    }
                }
            }

            update();
        }

        public void move(Move mv)
        {
            Debug.Assert(mv != null);

            side sd = p_turn;
            side xd = Side.Opposit(sd);

            square f = mv.GetFrom();
            square t = mv.GetTo();

            piece pc = mv.GetPieceMoving();
            piece cp = mv.GetCapturedPiece();
            piece pp = mv.GetPromoted();

            Debug.Assert(p_square[(int)f] == pc);
            Debug.Assert(square_side(f) == sd);

            Debug.Assert(p_sp < 1024);
            int poss = p_sp++;

            p_stack[poss].copy = p_copy;
            p_stack[poss].move = mv;
            p_stack[poss].castling = false;

            p_copy.moves++;
            p_copy.recap = square.NONE;

            // capture

            Debug.Assert(cp != piece.KING);

            if (pc == (int)piece.PAWN && t == p_copy.ep_sq)
            {
                square cap_sq =(square) (int)t - Square.PawnInc(sd);
                Debug.Assert(p_square[(int)cap_sq] == cp);
                Debug.Assert(cp == (int)piece.PAWN);

                p_stack[poss].cap_sq = cap_sq;

                clear_square(cp, xd, cap_sq, true);
            }
            else if (cp != piece.NONE)
            {
                Debug.Assert(p_square[(int)t] == cp);
                Debug.Assert(square_side(t) == xd);

                p_stack[poss].cap_sq = t;

                clear_square(cp, xd, t, true);
            }
            else
            {
                Debug.Assert(p_square[(int)t] == cp);
            }

            // promotion

            if (pp != piece.NONE)
            {
                Debug.Assert(pc == (int)piece.PAWN);
                clear_square((int)piece.PAWN, sd, f, true);
                set_square(pp, sd, t, true);
            }
            else
            {
                move_square(pc, sd, f, t, true);
            }

            // castling rook

            if (pc == piece.KING && Math.Abs(t - f) == Square.CASTLING_DELTA)
            {
                p_stack[poss].castling = true;

                int wg = (t > f) ? (int)Wing.wing.KING : (int)Wing.wing.QUEEN;
                int index = Castling.index(sd, wg);

                Debug.Assert(Castling.flag(p_copy.flags, index));

                Debug.Assert(f == Castling.info[index].kf);
                Debug.Assert(t == Castling.info[index].kt);

                move_square(piece.ROOK, sd, Castling.info[index].rf, Castling.info[index].rt, true);
            }

            // turn

            flip_turn();

            // castling flags

            p_copy.flags &= ~Castling.flags_mask[(int)f];
            p_copy.flags &= ~Castling.flags_mask[(int)t];

            // en-passant square

            p_copy.ep_sq = square.NONE;

            if (pc == (int)piece.PAWN &&  Math.Abs(t - f) == Square.DOUBLE_PAWN_DELTA)
            {
                square sq = (square)(((int)f + (int)t) / 2);
                if (pawn_is_attacked(sq, xd))
                {
                    p_copy.ep_sq = sq;
                }
            }

            // move counter

            if (cp != piece.NONE || pc == piece.PAWN)
            {
                p_copy.moves = 0; // conversion;
            }

            // recapture

            if (cp != piece.NONE || pp != piece.NONE)
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

            square f = mv.GetFrom();
            square t = mv.GetTo();

            piece pc = mv.GetPieceMoving();
            piece cp = mv.GetCapturedPiece();
            piece pp = mv.GetPromoted();

            side xd = p_turn;
            side sd = Side.Opposit(xd);

            Debug.Assert(p_square[(int)t] == pc || p_square[(int)t] == pp);
            Debug.Assert(square_side(t) == sd);

            // castling rook

            if (p_stack[poss].castling)
            {
                int wg = (t > f) ? (int)Wing.wing.KING : (int)Wing.wing.QUEEN;
                int index = Castling.index(sd, wg);

                Debug.Assert(f == Castling.info[index].kf);
                Debug.Assert(t == Castling.info[index].kt);

                move_square(piece.ROOK, sd, Castling.info[index].rt, Castling.info[index].rf, false);
            }

            // promotion

            if (pp != piece.NONE)
            {
                Debug.Assert(pc == (int)piece.PAWN);
                clear_square(pp, sd, t, false);
                set_square((int)piece.PAWN, sd, f, false);
            }
            else
            {
                move_square(pc, sd, t, f, false);
            }

            // capture

            if (cp != piece.NONE)
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
            p_copy.ep_sq = square.NONE;
            p_copy.moves = 0; // HACK: conversion
            p_copy.recap = square.NONE;

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
                        if (s[7-(int)Square.Rank((square)sq)] == null)
                            s[7-(int)Square.Rank((square)sq)] = "";
                        if (square_is((square)sq, (piece)pc, (side)sd))
                        {
                            pieceFound = true;
                            s[7 - (int)Square.Rank((square)sq)] += "[" + Piece.ToFen(Piece.Make((piece)pc, (side)sd)) + "]";
                        }
                           
                        
                    }
                }
                if (!pieceFound)
                    s[7 - (int)Square.Rank((square)sq)] += "[" + Piece.ToChar(piece.NONE) + "]";
            }
            return String.Join("\n", s);
        }


    }
}