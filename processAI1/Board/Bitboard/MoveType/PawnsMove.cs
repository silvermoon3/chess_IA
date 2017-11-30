using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1.Board.Bitboard.MoveType
{
    class PawnsMove: BitMove
    {
        public PawnsMove(BitBoard bitBoard) : base(bitBoard)
        {
        }


        public override List<Move> PossibleMoves(bool isWhite)
        {
            List<Move> moves = new List<Move>();
            moves.AddRange(PossiblePush(isWhite));
            moves.AddRange(PossibleDblPush(isWhite));
            moves.AddRange(PossibleAttackLeft(isWhite));
            moves.AddRange(PossibleAttackRight(isWhite));
            return moves;
        }

        public ulong PossiblePushBitBoard(Boolean isWhite)
        {
            ulong rank = !isWhite ? Rank1 : Rank8;
            ulong pawnsBitBoard = !isWhite ? BitBoard.BlackPawns : BitBoard.WhitePawns;
            Direction d = !isWhite ? Direction.S : Direction.N;

            ulong pawnMoves = MoveOne(pawnsBitBoard & ~rank, d) & BitBoard.GetEmpty();

            return pawnMoves;

        }
        
        public List<Move> PossiblePush(Boolean isWhite)
        {
            
            int d = isWhite ? -1 : 1;
            // move one if target pos empty and start not rank 8
            ulong pawnMoves = PossiblePushBitBoard(isWhite);
            return ListFromTargetPos(pawnMoves, 0, d);
        }

        public List<Move> PossibleDblPush(Boolean isWhite)
        {
            ulong rank = !isWhite ? Rank1 : Rank8;
            ulong dblRank = !isWhite ? Rank5 : Rank4;
            ulong pawnsBitBoard = !isWhite ? BitBoard.BlackPawns : BitBoard.WhitePawns;
            Direction dir = !isWhite ? Direction.S : Direction.N;
            int d = isWhite ? -1 : 1;

            // move one if target pos empty and start not rank 8
            ulong pawnMoves = PossiblePushBitBoard(isWhite);
            
            // move two and check for rank4
            pawnMoves = MoveOne(pawnMoves & ~rank, dir) & dblRank & BitBoard.GetEmpty(); 
            return ListFromTargetPos(pawnMoves, 0, 2 * d);

        }

        public List<Move> PossibleAttackRight(Boolean isWhite)
        {
            ulong rank = !isWhite ? Rank1 : Rank8;
            ulong file = !isWhite ? HFile : AFile;
            ulong pawnsBitBoard = !isWhite ? BitBoard.BlackPawns : BitBoard.WhitePawns;
            Direction dir = !isWhite ? Direction.SW : Direction.NE;
            int d = isWhite ? -1 : 1;

            ulong pawnMoves = MoveOne(pawnsBitBoard & ~rank, dir) & ~file & BitBoard.GetOther(isWhite);
            return ListFromTargetPos(pawnMoves, 1*d, 1*d);
        }

        public List<Move> PossibleAttackLeft(Boolean isWhite)
        {
            ulong rank = !isWhite ? Rank1 : Rank8;
            ulong file = !isWhite ? HFile : AFile;
            ulong pawnsBitBoard = !isWhite ? BitBoard.BlackPawns : BitBoard.WhitePawns;
            Direction dir = !isWhite ? Direction.SE : Direction.NW;
            int d = isWhite ? -1 : 1;

            ulong pawnMoves = MoveOne(pawnsBitBoard & ~rank, dir) & ~file & BitBoard.GetOther(isWhite);
            return ListFromTargetPos(pawnMoves, -1, 1);
        }



        


    }
}
