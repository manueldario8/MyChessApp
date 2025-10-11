namespace MyChessLogic
{
    public class PawnPromotion(Position from, Position to, PiecesType newType) : Move
    {
        public override MoveType Type => MoveType.PawnPromotion;
        public override Position FromPos { get; } = from;
        public override Position ToPos { get; } = to;

        private readonly PiecesType newType = newType;

        private Piece CreatePromotionPiece(Player color)
        {
            return newType switch
            {
                PiecesType.Knight => new Knight(color),
                PiecesType.Bishop => new Bishop(color),
                PiecesType.Rook => new Rook(color),
                _ => new Queen(color)
            };
        }

        public override bool Execute(Board board)
        {
            Piece pawn = board[FromPos];
            board[FromPos] = null;

            Piece promotionPiece = CreatePromotionPiece(pawn.Color);
            promotionPiece.HasMoved = true;
            board[ToPos] = promotionPiece;


            return true;
        }









    }
}
