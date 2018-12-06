namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Windows.Input;

    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;

        private readonly Predicate<object> canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void OnCanExecuteChanged(object sender)
        {
            this.CanExecuteChanged?.Invoke(sender, EventArgs.Empty);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
