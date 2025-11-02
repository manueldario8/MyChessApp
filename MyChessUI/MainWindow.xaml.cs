using MyChessLogic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;


namespace MyChessUI
{
    
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8,8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Dictionary<Position, Move> moveCache = [];

        
        private bool vsComputer = true;
        private Player computerPlayer;
        private readonly Random random = new();


        internal GameState gameState;
        private Position selectedPos = null;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();
            StartGameCard startCard = new();
            MenuContainer.Content = startCard;
        }

        public void SetVsComputer(bool isComputer)
        {
            vsComputer = isComputer;
        }


        public void StartGameWithBottomPlayer()
        {
            gameState = new GameState(Board.BottomPlayer, Board.Initial());
            computerPlayer = (Board.BottomPlayer == Player.White)
                ? Player.Black
                : Player.White;


            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);

            if (vsComputer && gameState.CurrentPlayer == computerPlayer)
            {
                MakeComputerMove();
            }
        }
        private void InitializeBoard()
        {


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++) 
                {
                    Image image = new();
                    pieceImages[i,j] = image;
                    PieceGrid.Children.Add(image);

                    Rectangle hightlight = new();
                    highlights[i, j] = hightlight;
                    HighLightGrid.Children.Add(hightlight);
                
                }
            }
        }
        internal void DrawBoard(Board board)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece piece = board[i, j];
                    pieceImages[i, j].Source = Images.GetImage(piece);
                }
            }

        }
        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMenuOnScreen())
            {
                return;
            }

            Point point = e.GetPosition(BoardGrid);
            Position pos = ToSquarePosition(point);

            if (selectedPos == null)
            {
                OnFromPositionSelected(pos);
            }
            else 
            {
                OnToPositionSelected(pos);
            }
        }
        private Position ToSquarePosition(Point point)
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)(point.Y / squareSize);
            int column = (int)(point.X / squareSize);
            return new Position(row, column);
        }
        private void OnFromPositionSelected(Position pos)
        {
            IEnumerable<Move> moves = gameState.LegalMovesForPiece(pos);

            if (moves.Any())
            {
                selectedPos = pos;
                CacheMoves(moves);
                ShowHighlights();   
            }
        }
        private void OnToPositionSelected(Position pos)
        {
            selectedPos = null;
            HideHighlights();

            if(moveCache.TryGetValue(pos, out Move move))
            {
                if (move.Type == MoveType.PawnPromotion)
                {
                    HandlePromotion(move.FromPos, move.ToPos);
                }
                else 
                {  HandleMove(move);
                }
            }
        }
        private void HandlePromotion(Position from, Position to)
        {
            pieceImages[to.Row, to.Column].Source = Images.GetImage(gameState.CurrentPlayer, PiecesType.Pawn);
            pieceImages[from.Row, from.Column].Source = null;

            PromotionMenu promMenu = new(gameState.CurrentPlayer);
            MenuContainer.Content = promMenu;

            promMenu.PieceSelected += type =>
            {
                MenuContainer.Content = null;
                Move promMove = new PawnPromotion(from, to, type);
                HandleMove(promMove);
            };
        }    
        private void HandleMove(Move move)
        { 

            gameState.MakeMove(move);

            ClearHighlights();
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
            UpdateCheckHighlight();

            if (gameState.IsGameOver())
            {
                ShowGameOver();
            }

            if (vsComputer && gameState.CurrentPlayer == computerPlayer && !gameState.IsGameOver())
            {
                MakeComputerMove();
            }

        }
        private async void MakeComputerMove()
        {
            await Task.Delay(700);
            IEnumerable<Move> moves = gameState.AllLegalMovesFor(computerPlayer);

            if (!moves.Any())
                return;

            Move move = moves.ElementAt(random.Next(moves.Count()));
            

            HandleMove(move);
        }
        public void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();

            foreach (Move move in moves) 
            {
                moveCache[move.ToPos] = move;
            }
        }
        public void ShowHighlights()
        {
            Color color = System.Windows.Media.Color.FromArgb(150,125,255,125);

            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = new SolidColorBrush(color);
            }
        }
        private void HideHighlights()
        {
            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = Brushes.Transparent;
            }
        }
        private void ShowCheckHighlight(Position kingPos)
        {
            Color red = Color.FromArgb(150, 255, 0, 0);
            highlights[kingPos.Row, kingPos.Column].Fill = new SolidColorBrush(red);
        }   
        private void UpdateCheckHighlight()
        {
            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    highlights[i, j].Fill = Brushes.Transparent;
                }
            }
            if (gameState.Board.IsInCheck(Player.White))
            {
                Position whiteKingPos = gameState.Board.KingPosition(Player.White);
                ShowCheckHighlight(whiteKingPos);
            }
            if (gameState.Board.IsInCheck(Player.Black))
            {
                Position blackKingPos = gameState.Board.KingPosition(Player.Black);
                ShowCheckHighlight(blackKingPos);
            }
        }
        private void ClearHighlights()
        {

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    highlights[i, j].Fill = Brushes.Transparent;
                }
            }
        }
        internal void SetCursor(Player player)
        {
            if (player == Player.White) 
            {
                Cursor = ChessCursors.WhiteCursor;
            }

            else
            {
                Cursor = ChessCursors.BlackCursor;
            }
        }
        private bool IsMenuOnScreen()
        {
            return MenuContainer.Content != null;
        }
        private void ShowGameOver()
        {
            GameOverMenu gameOverMenu = new(gameState);
            MenuContainer.Content = gameOverMenu;

            gameOverMenu.OptionSelected += option =>
            {
                if (option == Option.Restart)
                {
                    MenuContainer.Content = null;
                    RestarGame();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            };

        }
        private void ShowStartGameCard()
        {
            StartGameCard startCard = new();
            MenuContainer.Content = startCard;
        }
        private void RestarGame()
        {
            ClearHighlights();
            selectedPos = null;
            moveCache.Clear();
            ShowStartGameCard();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsMenuOnScreen() && e.Key == Key.Escape)
            {
                ShowPauseMenu();
            }
        }
        private void ShowPauseMenu()
        {
            PauseMenu pauseMenu = new();
            MenuContainer.Content = pauseMenu;

            pauseMenu.OptionSelected += option =>
            {
                MenuContainer.Content = null;

                if (option == Option.Restart)
                {
                    RestarGame();
                }
                else if (option == Option.Exit)
                {
                    Application.Current.Shutdown();
                }
            };
        }



    }
}