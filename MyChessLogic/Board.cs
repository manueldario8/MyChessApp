using MyChessLogic.Moves;

namespace MyChessLogic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];

        private readonly Dictionary<Player, Position> pawnSkipPositions = new()
        { 
            {Player.White, null },
            {Player.Black, null }
        };

        public Position GetPawnSkipPosition(Player player)
        {
            return pawnSkipPositions[player];
        }

        public void SetPawnSkipPosition(Player player, Position pos)
        {
            pawnSkipPositions[player] = pos;
        }


        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }

        public Piece this[Position pos]
        {
            get { return this[pos.Row, pos.Column]; }
            set { this[pos.Row, pos.Column] = value; }

        }

        public static Board Initial()
        {
            Board board = new();
            board.AddStartPieces();
            return board;

        }

        public void AddStartPieces()
        {
            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0, 2] = new Bishop(Player.Black);
            this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Bishop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);


            this[7, 0] = new Rook(Player.White);
            this[7, 1] = new Knight(Player.White);
            this[7, 2] = new Bishop(Player.White);
            this[7, 3] = new Queen(Player.White);
            this[7, 4] = new King(Player.White);
            this[7, 5] = new Bishop(Player.White);
            this[7, 6] = new Knight(Player.White);
            this[7, 7] = new Rook(Player.White);

            for (int i = 0; i <= 7; i++)
            {
                this[1, i] = new Pawn(Player.Black);
                this[6, i] = new Pawn(Player.White);
            }

        }

        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Position pos = new(r, c);

                    if (!IsEmpty(pos))
                    {
                        yield return pos;
                    }
                }
            }
        }

        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            return PiecePositions().Where(pos => this[pos].Color == player);

        }

        //public Position FindKing(Player player)
        //{
        //    return PiecePositionsFor(player).First(pos => this[pos].Type == PiecesType.King);
        //}

        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.Opponent()).Any(pos =>
            {
                Piece piece = this[pos];
                return piece.CanCaptureOpponentKing(pos, this);
            });
        }
        //public bool IsInCheck(Player player)
        //{
        //    Position kingPos = FindKing(player);
        //    return PiecePositionsFor(player.Opponent()).Any(pos =>
        //    {
        //        Piece piece = this[pos];
        //        return piece.GetMoves(pos, this).Any(move => move.ToPos.Equals(kingPos));
        //    });
        //}


        public Board Copy()
        {
            Board copy = new();

            foreach (Position pos in PiecePositions())
            {
                copy[pos] = this[pos].Copy();
            }

            return copy;
        }

        public Counting CountPieces()
        {
            Counting counting = new();

            foreach (Position pos in PiecePositions())
            {
                Piece piece = this[pos];
                counting.Increment(piece.Color, piece.Type);
            }

            return counting;
        }

        public bool InsufficientMaterial()
        {
            Counting counting = CountPieces();

            return IsKingVsKing(counting) || IsKingAndBishopVsKing(counting) || IsKingAndBishopVsKingAndBishop(counting) || IsKingAndKnightVsKing(counting) || IsKingAndTwoKnightsVsKing(counting);
        }

        private static bool IsKingVsKing(Counting counting)
        {
            return counting.totalCount == 2;
        }

        private static bool IsKingAndBishopVsKing(Counting counting)
        {
            return counting.totalCount == 3 && (counting.White(PiecesType.Bishop) == 1 || counting.Black(PiecesType.Bishop) == 1);
        }

        private bool IsKingAndBishopVsKingAndBishop(Counting counting)
        {
            if (counting.totalCount !=4) return false;
            if (counting.White(PiecesType.Bishop) != 1 || counting.Black(PiecesType.Bishop) != 1) return false;

            Position wBishopPos = FindPiece(Player.White, PiecesType.Bishop);
            Position bBishopPos = FindPiece(Player.Black, PiecesType.Bishop);

            return wBishopPos.SquareColor() == bBishopPos.SquareColor();

        }

        private Position FindPiece(Player color, PiecesType type)
        {
            return PiecePositionsFor(color).First(pos => this[pos].Type == type);
        }

        private static bool IsKingAndKnightVsKing(Counting counting)
        {
            return counting.totalCount == 3 && (counting.White(PiecesType.Knight) == 1 || counting.Black(PiecesType.Knight) == 1);
        }

        private static bool IsKingAndTwoKnightsVsKing(Counting counting)
        {
            return counting.totalCount == 4 && (counting.White(PiecesType.Knight) == 2 || counting.Black(PiecesType.Knight) == 2);
        }

        private bool IsUnmovedKingAndRook(Position kingPos, Position rookPos)
        {
            if (IsEmpty(kingPos) || IsEmpty(rookPos)) return false;

            Piece king = this[kingPos];
            Piece rook = this[rookPos];

            return king.Type == PiecesType.King && rook.Type == PiecesType.Rook && !king.HasMoved && !rook.HasMoved;

        }

        internal bool IsCastleRightKS(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 7)),
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 7)),
                _ => false
            };
        }

        internal bool IsCastleRightQS(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 0)),
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 0)),
                _ => false
            };
        }

        private bool HasPawnInPosition(Player player, Position[] pawnPositions, Position skipPos)
        {
            foreach (Position pos in pawnPositions.Where(IsInside))
            {
                Piece piece = this[pos];

                if (piece == null || piece.Color != player || piece.Type != PiecesType.Pawn)
                {
                    continue;
                }

                EnPassant move = new(pos, skipPos);
                if (move.IsLegal(this)) return true;               
            }

            return false;
        }

        public bool CanCaptureEnPassant(Player player)
        {
            Position skipPos = GetPawnSkipPosition(player.Opponent());

            if (skipPos == null) return false;

            Position[] pawnPositions = player switch
            {
                Player.White => new Position[] { skipPos + Direction.SouthWest, skipPos+ Direction.SouthEast },
                Player.Black => new Position[] { skipPos + Direction.NorthWest, skipPos+ Direction.NorthEast },
                _ => Array.Empty<Position>()
            };

            return HasPawnInPosition(player, pawnPositions, skipPos);
        }

    }
}
