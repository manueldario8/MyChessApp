namespace MyChessLogic
{
    public class Pawn: Piece
    {
        public override PiecesType Type =>  PiecesType.Pawn;
        public override Player Color { get; }

        private readonly Direction forward;

        public Pawn(Player color) 
        {
            Color = color;

        if (color == Player.White)
            {
            forward = Direction.North;
            }
        else if (color == Player.Black)
            {
                forward = Direction.South;
            }
        }
        public override Piece Copy()
        {
            Pawn copy = new(Color)
            {
                HasMoved = HasMoved
            };
            return copy;
        }

        private static bool CanMoveTo(Position pos, Board board) 
        {
            return Board.IsInside(pos) && board.IsEmpty(pos);
        }
        private bool CanCaptureAt(Position pos, Board board)
        {
            if (!Board.IsInside(pos) || board.IsEmpty(pos))
            {
                return false;
            }
            return board[pos].Color != Color;
        }

        private static IEnumerable<Move> PromotionMoves(Position from, Position to)
        {
            yield return new PawnPromotion(from, to, PiecesType.Queen);
            yield return new PawnPromotion(from, to, PiecesType.Knight);
            yield return new PawnPromotion(from, to, PiecesType.Bishop);
            yield return new PawnPromotion(from, to, PiecesType.Rook);
        }



        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            Position oneMovePos = from + forward;

            if (CanMoveTo(oneMovePos, board))
            {

                if (oneMovePos.Row == 0 || oneMovePos.Row == 7)
                {
                    foreach (Move promMove in PromotionMoves(from, oneMovePos))
                    {
                        yield return promMove;
                    }
                }
                else
                {
                    yield return new NormalMove(from, oneMovePos);
                }

                    

                Position twoMovePos = oneMovePos + forward;

                if (!HasMoved && CanMoveTo(twoMovePos, board))
                {
                    yield return new NormalMove(from, twoMovePos);
                }
            }
        }

        private IEnumerable<Move> DiagonalMoves(Position from, Board board)
        {
            foreach (Direction dir in new Direction[] {Direction.East, Direction.West })
            {
                Position to = from + forward + dir;

                if (CanCaptureAt(to, board))
                {
                    if (to.Row == 0 || to.Row == 7)
                    {
                        foreach (Move promMove in PromotionMoves(from, to))
                        {
                            yield return promMove;
                        }
                    }
                    else
                    {
                        yield return new NormalMove(from, to);
                    }
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return ForwardMoves(from, board).Concat(DiagonalMoves(from, board));
        }

        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            return DiagonalMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PiecesType.King;
            });
        }
    }
}
