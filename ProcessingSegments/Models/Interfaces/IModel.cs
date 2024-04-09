namespace ProcessingSegments.Models.Interfaces
{
    public interface IModel
    {
        public IEnumerable<Point> GetPointsIncludedRectangle(IEnumerable<Point> points, Rectangle rectangle);
    }

    public record Point(double X, double Y);

    public class Rectangle()
    {
        public double Xi { get; set; }
        public double Yi { get; set; }
        public double Xj { get; set; }
        public double Yj { get; set; }
    };
}
