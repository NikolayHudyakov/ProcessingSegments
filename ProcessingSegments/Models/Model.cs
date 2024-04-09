using ProcessingSegments.Models.Interfaces;

namespace ProcessingSegments.Models
{
    public class Model : IModel
    {
        public IEnumerable<Point> GetPointsIncludedRectangle(IEnumerable<Point> points, Rectangle rectangle)
        {
            List<Point> listPoints = new(points);
            Rectangle correctRectangle = TransformToCorrectRectangle(rectangle);

            for (var i = 0; i < listPoints.Count; i++)
            {
                if (Included(listPoints[i]))
                {
                    if(i > 0 && !Included(listPoints[i - 1]))
                        yield return listPoints[i - 1];

                    yield return listPoints[i];

                    if (i < listPoints.Count - 1 && !Included(listPoints[i + 1]))
                        yield return listPoints[i + 1];
                } 
            }

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
