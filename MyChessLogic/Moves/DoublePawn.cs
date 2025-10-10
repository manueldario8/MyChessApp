namespace MyChessLogic
{
    public class DoublePawn (Position from, Position to) : Move
    {
        public override MoveType Type => MoveType.DoublePawn;
        public override Position FromPos { get; } = from;
        public override Position ToPos { get; } = to;

        private readonly Position skippedPos = new((from.Row + to.Row) / 2, from.Column);

        public override bool Execute(Board board)
        {
            Player player = board[FromPos].Color;
            board.SetPawnSkipPosition(player, skippedPos);
            new NormalMove(FromPos, ToPos).Execute(board);

            return true;
        }
    }
}
