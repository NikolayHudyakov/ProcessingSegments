using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using ProcessingSegments.Commands;
using ProcessingSegments.Models.Interfaces;
using ProcessingSegments.Services.Interfaces;
using SkiaSharp;
using System.Windows.Input;

namespace ProcessingSegments.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IModel _model;
        private readonly IObjectProviderService<List<Point>> _pointsProviderService;

        private List<Point>? _points;

        public MainWindowViewModel(IModel model, IObjectProviderService<List<Point>> pointsProviderService)
        {
            _model = model;
            _pointsProviderService = pointsProviderService;

            //_lineSeries = new LineSeries<Point>
            //{
            //    Values = _serializerPointsService.Obj,
            //    Stroke = new SolidColorPaint(SKColors.Lime),
            //    Fill = null,
            //    Mapping = (point, index) => new Coordinate(point.X, point.Y),
            //    LineSmoothness = 0,

            //};
        }

        private ISeries _lineSeries;

        private Rectangle _rectangle = new(2, 2, 12, 12);

        private IEnumerable<Point>? _pointsIncludedRectangle;

        #region Commands
        public ICommand GetPointsCommand => new RelayCommand(GetPoints);
        public ICommand CalculatePointsCommand => new RelayCommand(CalculatePoints);
        #endregion

        public static string Title => "Обработка отрезков";

        public ISeries[] Series =>
        [
            //new LineSeries<Point>
            //{
            //    Values = _serializerPointsService.Obj,
            //    Stroke = new SolidColorPaint(SKColors.Lime),
            //    Fill = null,
            //    Mapping = (point, index) => new Coordinate(point.X, point.Y),
            //    LineSmoothness = 0,

            //},
            new LineSeries<Point>
            {
                Values = _pointsIncludedRectangle,
                Stroke = new SolidColorPaint(SKColors.Red, 2),
                Fill = null,
                Mapping = (point, index) => new Coordinate(point.X, point.Y),
                LineSmoothness = 0,
            }
            
        ];

        public RectangularSection[] Sections =>
        [
            new RectangularSection
            {
                Xi = _rectangle.Xi,
                Yi = _rectangle.Yi,
                Xj = _rectangle.Yj,
                Yj = _rectangle.Yj,
                Stroke = new SolidColorPaint 
                { 
                    Color = SKColors.Orange, StrokeThickness = 2, PathEffect = new DashEffect([10, 10]) 
                } 
            }
        ];

        private void GetPoints() => _points = _pointsProviderService.GetObject();

        private void CalculatePoints()
        {
            //_pointsIncludedRectangle = _model.GetPointsIncludedRectangle(_serializerPointsService.Obj, _rectangle);
            OnPropertyChanget(nameof(Series));
        }
    }
}
