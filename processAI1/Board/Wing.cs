using processAI1.Board.Bitboard;

namespace processAI1.Board
{
    public static  class Wing
    {
        public const int SIZE = 2;

        public enum wing {
            KING,
            QUEEN,
        };

    public static readonly int[] shelter_file =new int[SIZE]{ (int)file.FILE_G, (int) file.FILE_B }; 

    }
}