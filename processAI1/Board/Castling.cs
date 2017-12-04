using System;
using processAI1.Board.Bitboard;

namespace processAI1.Board
{
    public static class Castling
    {

        public struct Info
        {
            public int kf;
            public int kt;
            public int rf;
            public int rt;
        };

        public static readonly Info[] info = {      
            new Info(){ kf = (int)Square.square.E1,kt = (int)Square.square.G1, rf = (int)Square.square.H1, rt = (int)Square.square.F1 },
            new Info(){ kf = (int)Square.square.E1,kt = (int)Square.square.C1, rf = (int)Square.square.A1, rt = (int)Square.square.D1 },
            new Info(){ kf = (int)Square.square.E8,kt = (int)Square.square.G8, rf = (int)Square.square.H8, rt = (int)Square.square.F8 },
            new Info(){ kf = (int)Square.square.E8,kt = (int)Square.square.C8, rf = (int)Square.square.A8, rt = (int)Square.square.D8 },
        };

    public static int[] flags_mask= new int[Square.SIZE];
    public static UInt64[] flags_key= new UInt64[1 << 4];

    public static int index(int sd, int wg)
    {
        return sd * 2 + wg;
    }

    public static int side(int index)
    {
        return index / 2;
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

    set_flag(ref flags_mask[(int)Square.square.E1], 0);
    set_flag(ref flags_mask[(int)Square.square.E1], 1);
    set_flag(ref flags_mask[(int)Square.square.H1], 0);
    set_flag(ref flags_mask[(int)Square.square.A1], 1);

    set_flag(ref flags_mask[(int)Square.square.E8], 2);
    set_flag(ref flags_mask[(int)Square.square.E8], 3);
    set_flag(ref flags_mask[(int)Square.square.H8], 2);
    set_flag(ref flags_mask[(int)Square.square.A8], 3);

    for (int flags = 0; flags < (1 << 4); flags++)
    {
    flags_key[flags] = flags_key_debug(flags);
    }
    }

    }
}