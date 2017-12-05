namespace processAI1.Board
{
    public enum side
    {
        WHITE,
        BLACK
    }

    public static class Side
    {
        public const int SIZE = 2;

        public static side Opposit(side side)
        {

            return (side)((int)side ^ 1);
        }
        
    }

}