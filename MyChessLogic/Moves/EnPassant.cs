namespace MyChessLogic.Moves
{
    public class EnPassant(Position from, Position to) : Move
    {
        public override MoveType Type => MoveType.EnPassant;
        public override Position FromPos { get; } = from;
        public override Position ToPos { get; } = to;
        private readonly Position capturePos = new(from.Row,to.Column);

        public override void Execute(Board board)
        {
            new NormalMove(FromPos, ToPos).Execute(board);
            board[capturePos] = null;
        }
    }
}
