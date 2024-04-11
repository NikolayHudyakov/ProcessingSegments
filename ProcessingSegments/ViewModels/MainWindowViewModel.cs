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
    internal class MainWindowViewModel(IModel model, IObjectProviderService<List<Point>> pointsProviderService) : ViewModel
    {
        private readonly IModel _model = model;
        private readonly IObjectProviderService<List<Point>> _pointsProviderService = pointsProviderService;

        private static readonly RectangularSection _rectangularSection = new()
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
            LineSmoothness = 0
        };

        private readonly Rectangle _rectangle = new();
        
        private bool _pointerPressed;

        private readonly ICommand? _loadPointsCommand;
        private readonly ICommand? _calcIncludedPointsCommand;
        private readonly ICommand? _pointerPressedCommand;
        private readonly ICommand? _pointerMoveCommand;
        private readonly ICommand? _pointerReleasedCommand;

        #region Commands
        public ICommand LoadPointsCommand => _loadPointsCommand ?? new RelayCommand(LoadPointsAsync);
        public ICommand CalcIncludedPointsCommand => _calcIncludedPointsCommand ?? new RelayCommand(CalcIncludedPointsAsync);
        public ICommand PointerPressedCommand => _pointerPressedCommand ?? new RelayCommand(PointerPressed);
        public ICommand PointerMoveCommand => _pointerMoveCommand ?? new RelayCommand(PointerMove);
        public ICommand PointerReleasedCommand => _pointerReleasedCommand ?? new RelayCommand(PointerReleased);
        #endregion

        #region Properties
        public static string Title => "Обработка отрезков";

        public ObservableCollection<RectangularSection> Sections { get; private init; } = [];

        public ObservableCollection<ISeries> Series { get; private init; } = [];
        #endregion

        private async void LoadPointsAsync()
        {
            List<Point>? points = await Task.Run(_pointsProviderService.GetObject); ;

            if (points == null)
                return;

            _loadedLineSeries.Values = points;
            Series.Add(_loadedLineSeries);
        }

        private async void CalcIncludedPointsAsync()
        {
            if (_loadedLineSeries.Values == null)
                return;

            IEnumerable<IEnumerable<Point>> lineSeriesIncludedRectangle =
                    await Task.Run(() => _model.GetPointsIncludedRectangle(_loadedLineSeries.Values, _rectangle));

            Series.Clear();

            Series.Add(_loadedLineSeries);

            foreach (IEnumerable<Point> points in lineSeriesIncludedRectangle)
                Series.Add(new LineSeries<Point>
                {
                    Values = points,
                    Stroke = new SolidColorPaint(SKColors.Red, 3),
                    Fill = null,
                    Mapping = (point, index) => new Coordinate(point.X, point.Y),
                    LineSmoothness = 0
                });
        }
        
        #region DrawRectangle
        private LvcPointD GetCoordinatePointer(PointerCommandArgs args)
        {
            var chart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;
            return chart.ScalePixelsToData(args.PointerPosition);
        }

        private void PointerPressed(object? obj)
        {
            if(Sections.Count == 0)
                Sections.Add(_rectangularSection); 

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
