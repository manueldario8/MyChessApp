namespace MyChessLogic
{
    public class Bishop(Player color) : Piece
    {
        public override PiecesType Type => PiecesType.Bishop;
        public override Player Color { get; } = color;

        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.NorthEast,
            Direction.NorthWest,
            Direction.SouthEast,
            Direction.SouthWest
        };


        public override Piece Copy()
        {
            Bishop copy = new(Color)
            {
                HasMoved = HasMoved
            };
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirs(from, board, dirs).Select(to=> new NormalMove(from, to));  
        }


    }
}
