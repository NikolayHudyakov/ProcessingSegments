using LiveChartsCore;
using LiveChartsCore.Drawing;
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

        private LineSeries<Point> _loadedLineSeries = new()
        {
            Stroke = new SolidColorPaint(SKColors.Lime),
            Fill = null,
            Mapping = (point, index) => new Coordinate(point.X, point.Y),
            LineSmoothness = 0,

        };
        private LineSeries<Point> _includedRectangleLineSeries = new()
        {
            Stroke = new SolidColorPaint(SKColors.Red, 2),
            Fill = null,
            Mapping = (point, index) => new Coordinate(point.X, point.Y),
            LineSmoothness = 0,

        };
        private RectangularSection _rectangularSection = new()
        {
            Stroke = new SolidColorPaint
            {
                Color = SKColors.Orange,
                StrokeThickness = 2,
                PathEffect = new DashEffect([10, 10])
            }
        };

        private IEnumerable<Point>? _points;
        private IEnumerable<Point>? _pointsIncludedRectangle;
        private readonly Rectangle _rectangle = new ();
        private bool _pointerPressed;

        private readonly ICommand? _loadPointsCommand;
        private readonly ICommand? _calcIncludedPointsCommand;
        private readonly ICommand? _pointerPressedCommand;
        private readonly ICommand? _pointerMoveCommand;
        private readonly ICommand? _pointerReleasedCommand;

        #region Commands
        public ICommand LoadPointsCommand => _loadPointsCommand ?? new RelayCommand(LoadPoints);
        public ICommand CalcIncludedPointsCommand => _calcIncludedPointsCommand ?? new RelayCommand(CalcIncludedPoints);
        public ICommand PointerPressedCommand => _pointerPressedCommand ?? new RelayCommand(PointerPressed);
        public ICommand PointerMoveCommand => _pointerMoveCommand ?? new RelayCommand(PointerMove);
        public ICommand PointerReleasedCommand => _pointerReleasedCommand ?? new RelayCommand(PointerReleased);
        #endregion

        #region Properties
        public static string Title => "Обработка отрезков";

        public ISeries[] Series => [ _loadedLineSeries, _includedRectangleLineSeries ];

        public RectangularSection[] Sections => [ _rectangularSection ];
        #endregion

        private void LoadPoints()
        {
            List<Point>? points = _pointsProviderService.GetObject();

            if (points == null) 
                return;

            _points = points;
            _loadedLineSeries.Values = _points;
            OnPropertyChanget(nameof(Series));
        }

        private void CalcIncludedPoints()
        {
            if (_points == null)
                return;
            
            _pointsIncludedRectangle = _model.GetPointsIncludedRectangle(_points, _rectangle);
            _includedRectangleLineSeries.Values = _pointsIncludedRectangle;

            OnPropertyChanget(nameof(Series));
        }
        
        #region DrawRectangle
        private LvcPointD GetCoordinatePointer(PointerCommandArgs args)
        {
            var chart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;
            return chart.ScalePixelsToData(args.PointerPosition);
        }

        private void PointerPressed(object? obj)
        {
            _pointerPressed = true;

            if (obj is not PointerCommandArgs args) return;

            LvcPointD scaledPoint = GetCoordinatePointer(args);

            _rectangle.Xi = scaledPoint.X;
            _rectangle.Yi = scaledPoint.Y;
            _rectangle.Xj = scaledPoint.X;
            _rectangle.Yj = scaledPoint.Y;

            _rectangularSection.Xi = scaledPoint.X;
            _rectangularSection.Yi = scaledPoint.Y;
            _rectangularSection.Xj = scaledPoint.X;
            _rectangularSection.Yj = scaledPoint.Y;
        }

        private void PointerMove(object? obj)
        {
            if (!_pointerPressed || obj is not PointerCommandArgs args) return;

            LvcPointD scaledPoint = GetCoordinatePointer(args);

            _rectangle.Xj = scaledPoint.X;
            _rectangle.Yj = scaledPoint.Y;

            _rectangularSection.Xj = scaledPoint.X;
            _rectangularSection.Yj = scaledPoint.Y;

            OnPropertyChanget(nameof(Sections));
        }

        private void PointerReleased(object? obj) => _pointerPressed = false;
        #endregion
    }
}
