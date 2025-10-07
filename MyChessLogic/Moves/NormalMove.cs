namespace MyChessLogic
{
    public class NormalMove(Position from, Position to) : Move
    {
        public override MoveType Type => MoveType.Normal;
        public override Position FromPos { get; } = from;
        public override Position ToPos { get; } = to;

        

        public override void Execute(Board board)
        {
            Piece piece = board[FromPos];
            board[ToPos] = piece;
            board[FromPos] = null;
            piece.HasMoved = true;
        }





    }
}
