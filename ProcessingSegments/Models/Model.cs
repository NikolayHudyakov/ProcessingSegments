using ProcessingSegments.Models.Interfaces;

namespace ProcessingSegments.Models
{
    public class Model : IModel
    {
        public IEnumerable<Point> GetPointsIncludedRectangle(IEnumerable<Point> points, Rectangle rectangle)
        {
            List<Point> listPoints = new(points);
            List<Point>lineSeries

            List<Point> points1 = [];
            Rectangle correctRectangle = TransformToCorrectRectangle(rectangle);

            int sequentiallyEmpty = 0;

            for (var i = 0; i < listPoints.Count; i++)
            {
                if (Included(listPoints[i]))
                {
                    points1.Add(listPoints[i]);
                    sequentiallyEmpty = 0;
                    continue;
                }

                if (i > 0 && Included(listPoints[i - 1]))
                    points1.Add(listPoints[i]);

                if (i < listPoints.Count - 1 && Included(listPoints[i + 1]))
                    points1.Add(listPoints[i]);

                sequentiallyEmpty++;

                if (sequentiallyEmpty >= 2)
                {

                }
            }

            return points1;

            bool Included(Point point) =>
                point.X >= correctRectangle.Xi && point.Y >= correctRectangle.Yi && 
                point.X <= correctRectangle.Xj && point.Y <= correctRectangle.Yj;
        }

        private Rectangle TransformToCorrectRectangle(Rectangle rectangle)
        {
            Rectangle correctRectangle = new();

            var correctXi = rectangle.Xi < rectangle.Xj;
            correctRectangle.Xi = correctXi ? rectangle.Xi : rectangle.Xj;
            correctRectangle.Xj = !correctXi ? rectangle.Xi : rectangle.Xj;

            var correctYi = rectangle.Yi < rectangle.Yj;
            correctRectangle.Yi = correctYi ? rectangle.Yi : rectangle.Yj;
            correctRectangle.Yj = !correctYi ? rectangle.Yi : rectangle.Yj;

            return correctRectangle;
        }
    }
}
