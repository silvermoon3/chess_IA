namespace processAI1.Board
{
    public static class Side
    {
        public const int SIZE = 2;
        public const int WHITE = 0;
        public const int BLACK = 1;

        public static int Opposit(int side)
        {
            return side ^ 1;
        }
        
    }

}