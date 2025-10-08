using MyChessLogic;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyChessUI
{

    public partial class PromotionMenu : UserControl
    {

        public event Action<PiecesType> PieceSelected;

        public PromotionMenu(Player player)
        {
            InitializeComponent();

            QueenImg.Source = Images.GetImage(player, PiecesType.Queen);
            KnightImg.Source = Images.GetImage(player, PiecesType.Knight);
            BishopImg.Source = Images.GetImage(player, PiecesType.Bishop);
            RookImg.Source = Images.GetImage(player, PiecesType.Rook);
        }

        private void QueenImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected?.Invoke(PiecesType.Queen);
        }

        private void BishopImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected?.Invoke(PiecesType.Knight);
        }

        private void KnightImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected?.Invoke(PiecesType.Bishop);
        }

        private void RookImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceSelected?.Invoke(PiecesType.Rook);
        }
    }
}
