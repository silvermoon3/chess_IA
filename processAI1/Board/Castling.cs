using System;
using processAI1.Board.Bitboard;

namespace processAI1.Board
{
    public static class Castling
    {

        public struct Info
        {
            public square kf;
            public square kt;
            public square rf;
            public square rt;
        };

        public static readonly Info[] info = {      
            new Info(){ kf = square.E1,kt = square.G1, rf = square.H1, rt = square.F1 },
            new Info(){ kf = square.E1,kt = square.C1, rf = square.A1, rt = square.D1 },
            new Info(){ kf = square.E8,kt = square.G8, rf = square.H8, rt = square.F8 },
            new Info(){ kf = square.E8,kt = square.C8, rf = square.A8, rt = square.D8 },
        };

    public static int[] flags_mask= new int[Square.SIZE];
    public static UInt64[] flags_key= new UInt64[1 << 4];

        public static int index(side sd, int wg)
        {
            return (int)sd * 2 + wg;
        }

        public static side side(int index)
    {
        return (side)(index / 2);
    }

    public static bool flag(int flags, int index)
    {
        //TODO ASSERT et voir si ok
        //assert(index < 4);
        return ((flags >> index) & 1 )== 1;
    }

    public static void set_flag(ref int flags, int index)
    {
        //TODO ASSERT
        //assert(index < 4);
        flags |= 1 << index;
    }

    public static UInt64 flags_key_debug(int flags)
    {

        UInt64 key = 0;

        for (int index = 0; index < 4; index++)
        {
        if (flag(flags, index))
        {
        key ^= Hash.FlagKey(index);
        }
        }

        return key;
    }

    public static void init()
    {

    for (int sq = 0; sq < Square.SIZE; sq++)
    {
    flags_mask[sq] = 0;
    }

    set_flag(ref flags_mask[(int)square.E1], 0);
    set_flag(ref flags_mask[(int)square.E1], 1);
    set_flag(ref flags_mask[(int)square.H1], 0);
    set_flag(ref flags_mask[(int)square.A1], 1);

    set_flag(ref flags_mask[(int)square.E8], 2);
    set_flag(ref flags_mask[(int)square.E8], 3);
    set_flag(ref flags_mask[(int)square.H8], 2);
    set_flag(ref flags_mask[(int)square.A8], 3);

    for (int flags = 0; flags < (1 << 4); flags++)
    {
    flags_key[flags] = flags_key_debug(flags);
    }
    }

    }
}