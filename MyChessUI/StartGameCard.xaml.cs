using MyChessLogic;
using System.Windows;
using System.Windows.Controls;

namespace MyChessUI
{
    public partial class StartGameCard : UserControl
    {
        public bool IsVsComputer { get; private set; } = false;
        public string SelectedSide { get; private set; } = "White";
        public int SelectedDifficulty => cbDifficulty.SelectedIndex;

        public StartGameCard()
        {
            InitializeComponent();

            cbDifficulty.Items.Add("Easy");
            cbDifficulty.Items.Add("Medium");
            cbDifficulty.Items.Add("Hard");
            cbDifficulty.SelectedIndex = 0;

            cbDifficulty.IsEnabled = false; 
        }

        private void RbHuman_Checked(object sender, RoutedEventArgs e)
        {
            IsVsComputer = false;
            if (cbDifficulty != null)
                cbDifficulty.IsEnabled = false;
        }

        private void RbComputer_Checked(object sender, RoutedEventArgs e)
        {
            IsVsComputer = true;
            if (cbDifficulty != null)
                cbDifficulty.IsEnabled = false;
        }

        private void WhitePlayer_Checked(object sender, RoutedEventArgs e)
        {
            SelectedSide = "White";
        }

        private void BlackPlayer_Checked(object sender, RoutedEventArgs e)
        {
            SelectedSide = "Black";
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                Board.BottomPlayer = SelectedSide == "White" ? Player.White : Player.Black;

                mainWindow.MenuContainer.Content = null;
                mainWindow.SetVsComputer(IsVsComputer);
                mainWindow.StartGameWithBottomPlayer();
            }
        }
    }
}
