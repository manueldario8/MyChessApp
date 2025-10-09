namespace MyChessLogic
{
    public class Counting
    {
        private readonly Dictionary<PiecesType, int> whiteCount = new();
        private readonly Dictionary<PiecesType, int> blackCount = new();

        public int totalCount { get; private set; }

        public Counting()
        {
            foreach (PiecesType type in Enum.GetValues(typeof(PiecesType)))
            {
                whiteCount[type] = 0;
                blackCount[type] = 0;
            }
        }

        public void Increment(Player color, PiecesType type)
        {
            if (color == Player.White)
            {
                whiteCount[type]++;
            }
            else if (color == Player.Black)
            {
                blackCount[type]++;
            }

            totalCount++;
        }

        public int White(PiecesType type)
        {
            return whiteCount[type];
        }
        public int Black(PiecesType type)
        {
            return blackCount[type];
        }


    }
}
