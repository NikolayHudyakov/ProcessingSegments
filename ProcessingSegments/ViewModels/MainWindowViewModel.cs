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
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProcessingSegments.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IModel _model;
        private readonly IObjectProviderService<List<Point>> _pointsProviderService;

        private readonly RectangularSection _rectangularSection = new()
        {
            Stroke = new SolidColorPaint
            {
                Color = SKColors.Orange,
                StrokeThickness = 2,
                PathEffect = new DashEffect([10, 10])
            }
        };
        private readonly LineSeries<Point> _loadedLineSeries = new()
        {
            Stroke = new SolidColorPaint(SKColors.Lime),
            Fill = null,
            Mapping = (point, index) => new Coordinate(point.X, point.Y),
            LineSmoothness = 0,

        };
        private readonly LineSeries<Point> _includedRectangleLineSeries = new()
        {
            Stroke = new SolidColorPaint(SKColors.Red, 2),
            Fill = null,
            Mapping = (point, index) => new Coordinate(point.X, point.Y),
            LineSmoothness = 0,

        };

        private readonly Rectangle _rectangle = new();
        
        private bool _pointerPressed;

        private readonly ICommand? _loadPointsCommand;
        private readonly ICommand? _calcIncludedPointsCommand;
        private readonly ICommand? _pointerPressedCommand;
        private readonly ICommand? _pointerMoveCommand;
        private readonly ICommand? _pointerReleasedCommand;

        public MainWindowViewModel(IModel model, IObjectProviderService<List<Point>> pointsProviderService)
        {
            _model = model;
            _pointsProviderService = pointsProviderService;

            Sections = [_rectangularSection];
            Series = [_loadedLineSeries];
        }

        #region Commands
        public ICommand LoadPointsCommand => _loadPointsCommand ?? new RelayCommand(LoadPoints);
        public ICommand CalcIncludedPointsCommand => _calcIncludedPointsCommand ?? new RelayCommand(CalcIncludedPoints);
        public ICommand PointerPressedCommand => _pointerPressedCommand ?? new RelayCommand(PointerPressed);
        public ICommand PointerMoveCommand => _pointerMoveCommand ?? new RelayCommand(PointerMove);
        public ICommand PointerReleasedCommand => _pointerReleasedCommand ?? new RelayCommand(PointerReleased);
        #endregion

        #region Properties
        public static string Title => "Обработка отрезков";

        public List<RectangularSection> Sections { get; private init; }

        public ObservableCollection<ISeries> Series { get; private init; }
        #endregion

        private void LoadPoints()
        {
            List<Point>? points = _pointsProviderService.GetObject();

            if (points == null)
                return;

            _loadedLineSeries.Values = points;
        }

        private void CalcIncludedPoints()
        {
            if (_loadedLineSeries.Values == null)
                return;

            IEnumerable<IEnumerable<Point>> _pointsIncludedRectangle = _model.GetPointsIncludedRectangle(_loadedLineSeries.Values, _rectangle);
            foreach (IEnumerable<Point> lineSeries in _pointsIncludedRectangle)
            {
                Series.Add(new LineSeries<Point> 
                { 
                    Values = lineSeries,
                    Stroke = new SolidColorPaint(SKColors.Red),
                    Fill = null,
                    Mapping = (point, index) => new Coordinate(point.X, point.Y),
                    LineSmoothness = 0,
                });
            }
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

            _rectangularSection.Xi = scaledPoint.X;
            _rectangularSection.Yi = scaledPoint.Y;
        }

        private void PointerMove(object? obj)
        {
            if (!_pointerPressed || obj is not PointerCommandArgs args) return;

            LvcPointD scaledPoint = GetCoordinatePointer(args);

            _rectangularSection.Xj = scaledPoint.X;
            _rectangularSection.Yj = scaledPoint.Y;
        }

        private void PointerReleased(object? obj)
        {
            _pointerPressed = false;

            _rectangle.Xi = _rectangularSection.Xi ?? 0;
            _rectangle.Yi = _rectangularSection.Yi ?? 0; 
            _rectangle.Xj = _rectangularSection.Xj ?? 0;
            _rectangle.Yj = _rectangularSection.Yj ?? 0;
        }
        #endregion
    }
}
