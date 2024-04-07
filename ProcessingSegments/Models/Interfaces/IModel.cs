namespace ProcessingSegments.Models.Interfaces
{
    internal interface IModel
    {
        public IEnumerable<Point> GetPointsIncludedRectangle(IEnumerable<Point> points, Rectangle rectangle);
    }

    public record Point(double X, double Y);

    public record Rectangle(double Xi, double Yi, double Xj, double Yj);
}
