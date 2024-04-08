using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using ProcessingSegments.Commands;
using ProcessingSegments.Models.Interfaces;
using ProcessingSegments.Services.Interfaces;
using SkiaSharp;
using System.Windows.Input;

namespace ProcessingSegments.ViewModels
{
    internal class MainWindowViewModel(IModel model, IObjectProviderService<List<Point>> pointsProviderService) : ViewModel
    {
        private readonly IModel _model = model;
        private readonly IObjectProviderService<List<Point>> _pointsProviderService = pointsProviderService;

        private IEnumerable<Point>? _points;
        private IEnumerable<Point>? _pointsIncludedRectangle;
        private readonly Rectangle _rectangle = new ();

        #region Commands
        public ICommand LoadPointsCommand => new RelayCommand(LoadPoints);
        public ICommand CalcIncludedPointsCommand => new RelayCommand(CalcIncludedPoints);
        public ICommand PointerPressedCommand => new RelayCommand(PointerPressed);
        public ICommand PointerMoveCommand => new RelayCommand(PointerMove);
        #endregion

        #region Properties
        public static string Title => "Обработка отрезков";

        public ISeries[] Series =>
        [
            new LineSeries<Point>
            {
                Values = _points,
                Stroke = new SolidColorPaint(SKColors.Lime),
                Fill = null,
                Mapping = (point, index) => new Coordinate(point.X, point.Y),
                LineSmoothness = 0,

            },
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
        #endregion

        private void LoadPoints()
        {
            _points = _pointsProviderService.GetObject();
            OnPropertyChanget(nameof(Series));
        }

        private void CalcIncludedPoints()
        {
            if (_points == null)
                return;

            _pointsIncludedRectangle = _model.GetPointsIncludedRectangle(_points, _rectangle);
            OnPropertyChanget(nameof(Series));
        }

        private void PointerPressed(object? obj)
        {
            if (obj is not PointerCommandArgs args) return;

            var chart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;
            var scaledPoint = chart.ScalePixelsToData(args.PointerPosition);

            _rectangle.Xi = scaledPoint.X;
            _rectangle.Yi = scaledPoint.Y;
            _rectangle.Xj = scaledPoint.X;
            _rectangle.Yj = scaledPoint.Y;
            OnPropertyChanget(nameof(Sections));
        }

        private void PointerMove(object? obj)
        {
            if (obj is not PointerCommandArgs args) return;

            var chart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;
            var scaledPoint = chart.ScalePixelsToData(args.PointerPosition);

            _rectangle.Xj = scaledPoint.X;
            _rectangle.Yj = scaledPoint.Y;
            OnPropertyChanget(nameof(Sections));
        }
    }
}
