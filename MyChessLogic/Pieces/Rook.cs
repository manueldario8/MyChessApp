using MyChessLogic;

public class Rook(Player color) : Piece
{
    public override PiecesType Type => PiecesType.Rook;
    public override Player Color { get; } = color;

    private static readonly Direction[] dirs = new Direction[]
    {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West
    };

    public override Piece Copy()
    {
        Rook copy = new(Color)
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