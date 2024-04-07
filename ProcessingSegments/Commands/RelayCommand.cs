namespace ProcessingSegments.Commands
{
    internal class RelayCommand : Command
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute; 
        }

        public RelayCommand(Action execute, Predicate<object?>? canExecute = null)
        {
            _execute = (parameter) => execute();
            _canExecute = canExecute;
        }

        public RelayCommand(Func<Task> execute, Predicate<object?>? canExecute = null)
        {
            _execute = (parameter) => execute();
            _canExecute = canExecute;
        }

        public RelayCommand(Func<object?, Task> execute, Predicate<object?>? canExecute = null)
        {
            _execute = (parameter) => execute(parameter);
            _canExecute = canExecute;
        }

        public override void Execute(object? parameter) => _execute(parameter);

        public override bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
    }
}
