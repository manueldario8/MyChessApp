namespace MyChessLogic
{
    public class Direction(int rowDelta, int columnDelta)
    {

        public readonly static Direction North = new(-1, 0);
        public readonly static Direction South = new(1, 0);
        public readonly static Direction East = new(0, 1);
        public readonly static Direction West = new(0, -1);
        public readonly static Direction NorthEast = North + East;
        public readonly static Direction NorthWest = North + West;
        public readonly static Direction SouthEast = South + East;
        public readonly static Direction SouthWest = South + West;


        public int RowDelta { get; } = rowDelta;
        public int ColumnDelta { get; } = columnDelta;


        public static Direction operator +(Direction a, Direction b) 
        {
            return new Direction(a.RowDelta + b.RowDelta, a.ColumnDelta + b.ColumnDelta);
        }

        public static Direction operator *(int scalar, Direction a) 
        {
            return new Direction(scalar * a.RowDelta, scalar * a.ColumnDelta);
        }

    }
}
