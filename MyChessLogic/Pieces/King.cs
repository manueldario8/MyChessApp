using MyChessLogic;

public class King(Player color) : Piece
{
    public override PiecesType Type => PiecesType.King;
    public override Player Color { get; } = color;
    
    private static readonly Direction[] dirs =
    [
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West,
            Direction.NorthEast,
            Direction.SouthEast,
            Direction.SouthEast,
            Direction.SouthWest
    ];
    public static bool AllEmpty(IEnumerable<Position> positions, Board board)
    {
        return positions.All(pos => board.IsEmpty(pos));
    }

    private bool CanCastleKingSide(Position from, Board board)
    {
        if (HasMoved) { return false; }
        Position rookPos = new(from.Row, 7);
        Position[] betweenPositions = new Position[] { new(from.Row, 5), new(from.Row, 6) };

        return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
    }

    private bool CanCastleQueenSide(Position from, Board board)
    {
        if (HasMoved) { return false; }
        Position rookPos = new(from.Row, 0);
        Position[] betweenPositions = [new(from.Row, 1), new(from.Row, 2), new(from.Row, 3)];

        return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
    }



    private static bool IsUnmovedRook(Position pos, Board board)
    {
        if (board.IsEmpty(pos))
        {
            return false;
        }

        Piece piece = board[pos];
        return piece.Type == PiecesType.Rook && !piece.HasMoved;
    }


    
    public override Piece Copy()
    {
        King copy = new(Color)
        {
            HasMoved = HasMoved
        };
        return copy;
    }

    private IEnumerable<Position> MovePositions(Position from, Board board)
    {
        foreach (Direction dir in dirs)
        {
            Position to = from + dir;

            if (!Board.IsInside(to))
            {
                continue;
            }

            if (board.IsEmpty(to) || board[to].Color != Color)
            {
                yield return to;
            }
        }
    }


    public override IEnumerable<Move> GetMoves(Position from, Board board)
    {
        foreach (Position to in MovePositions(from, board))
        {
            yield return new NormalMove(from, to);
        }

        if (CanCastleKingSide(from, board))
        {
            yield return new Castle(MoveType.CastleKS, from); 
        }

        if (CanCastleQueenSide(from, board))
        {
            yield return new Castle(MoveType.CastleQS, from);
        }

    }

    public override bool CanCaptureOpponentKing(Position from, Board board)
    {
        return MovePositions(from, board).Any(to =>
        {

            Piece piece = board[to];
            return piece != null && piece.Type == PiecesType.King;

        });
    }
}
