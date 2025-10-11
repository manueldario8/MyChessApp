using MyChessLogic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyChessUI
{
    public partial class StartGameCard : UserControl
    {
        public StartGameCard()
        {
            InitializeComponent();
        }

        private void WhiteSide_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            Board.BottomPlayer = Player.White;

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MenuContainer.Content = null;
                mainWindow.StartGameWithBottomPlayer();  
            }
        }

        private void BlackSide_MouseDown(object sender, MouseButtonEventArgs e)
        {            
            Board.BottomPlayer = Player.Black;
          
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MenuContainer.Content = null;
                mainWindow.StartGameWithBottomPlayer();
            }
        }
    }
}
