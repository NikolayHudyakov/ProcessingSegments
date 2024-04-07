using ProcessingSegments.Models.Interfaces;

namespace ProcessingSegments.Models
{
    internal class Model : IModel
    {
        public IEnumerable<Point> GetPointsIncludedRectangle(IEnumerable<Point> points, Rectangle rectangle)
        {
            List<Point> listPoints = new(points);

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
                point.X >= rectangle.Xi && point.Y >= rectangle.Yi && 
                point.X <= rectangle.Xj && point.Y <= rectangle.Yj;

        }
    }
}
