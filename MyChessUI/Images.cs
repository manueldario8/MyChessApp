using MyChessLogic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyChessUI
{
    public static class Images
    {
        private static readonly Dictionary<PiecesType, ImageSource> whiteSource = new()
        {
            {PiecesType.Pawn,LoadImage("Assets/PawnW.png") },
            {PiecesType.King,LoadImage("Assets/KingW.png") },
            {PiecesType.Queen,LoadImage("Assets/QueenW.png") },
            {PiecesType.Bishop,LoadImage("Assets/BishopW.png") },
            {PiecesType.Knight,LoadImage("Assets/KnightW.png") },
            {PiecesType.Rook,LoadImage("Assets/RookW.png") }
        };
        private static readonly Dictionary<PiecesType, ImageSource> blackSource = new()
        {
            {PiecesType.Pawn,LoadImage("Assets/PawnB.png") },
            {PiecesType.King,LoadImage("Assets/KingB.png") },
            {PiecesType.Queen,LoadImage("Assets/QueenB.png") },
            {PiecesType.Bishop,LoadImage("Assets/BishopB.png") },
            {PiecesType.Knight,LoadImage("Assets/KnightB.png") },
            {PiecesType.Rook,LoadImage("Assets/RookB.png") }
        };


        private static ImageSource LoadImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        public static ImageSource GetImage(Player color, PiecesType type)
        {
            return color switch
            {
                Player.White => whiteSource[type],
                Player.Black => blackSource[type],
                _ => null
            };
        }

        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null) return null;
            return GetImage(piece.Color, piece.Type);

            //piece == null ? null : GetImage(piece.Color, piece.Type);
        }


    }
}
