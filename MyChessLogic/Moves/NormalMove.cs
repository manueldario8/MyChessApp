namespace MyChessLogic
{
    public class NormalMove(Position from, Position to) : Move
    {
        public override MoveType Type => MoveType.Normal;
        public override Position FromPos { get; } = from;
        public override Position ToPos { get; } = to;

        

        public override bool Execute(Board board)
        {
            Piece piece = board[FromPos];
            bool capture = !board.IsEmpty(ToPos);

            board[ToPos] = piece;
            board[FromPos] = null;
            piece.HasMoved = true;

            return capture || piece.Type == PiecesType.Pawn;
        }





    }
}
