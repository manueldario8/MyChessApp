using MyChessLogic;

public class Queen(Player color) : Piece
{
    public override PiecesType Type => PiecesType.Queen;
    public override Player Color { get; } = color;

    private static readonly Direction[] dirs = new Direction[]
    {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West,
            Direction.NorthEast,
            Direction.SouthEast,
            Direction.NorthWest,
            Direction.SouthWest
    };


    public override Piece Copy()
    {
        Queen copy = new(Color)
        {
            HasMoved = HasMoved
        };
        return copy;
    }
    public override IEnumerable<Move> GetMoves(Position from, Board board)
    {
        return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
    }
}