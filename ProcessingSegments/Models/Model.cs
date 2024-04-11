using ProcessingSegments.Models.Interfaces;

namespace ProcessingSegments.Models
{
    public class Model : IModel
    {
        public IEnumerable<IEnumerable<Point>> GetPointsIncludedRectangle(IEnumerable<Point> points, Rectangle rectangle)
        {
            Rectangle correctRectangle = TransformToCorrectRectangle(rectangle);
            List<Point> listPoints = new(points);
            List<List<Point>> lineSeries = [];
            List<Point> pointsIncluded = [];
            
            for (var i = 0; i < listPoints.Count; i++)
            {
                if (Included(listPoints[i]) || 
                    i > 0 && Included(listPoints[i - 1]) || 
                    i < listPoints.Count - 1 && Included(listPoints[i + 1]))
                {
                    pointsIncluded.Add(listPoints[i]);
                    continue;
                }

                if (pointsIncluded.Count != 0)
                {
                    lineSeries.Add(pointsIncluded);
                    pointsIncluded = [];
                }   
            }

            if (pointsIncluded.Count != 0)
                lineSeries.Add(pointsIncluded);

            return lineSeries;

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
