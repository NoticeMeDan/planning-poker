namespace PlanningPoker.App.ViewModels
{
    using System.Windows.Input;
    using Shared;

    // This class contains data until repositories is setup
    public class ItemViewModel : BaseViewModel
    {
        public ICommand SaveCommand { get; set; }

        // TODO: Use API to get and set items.
        // private readonly
        private ItemsViewModel itemsViewModel;

        private string TitleText { get; set; }

        private string DescriptionText { get; set; }

        // public ICommand AddCommand { get; set; }

        public ItemViewModel()
        {
            this.Title = "New Item";

            this.itemsViewModel = new ItemsViewModel();

            this.SaveCommand = new RelayCommand(_ => this.ExecuteSaveCommand());
        }

        private void ExecuteSaveCommand()
        {
            var newItem = new ItemDTO
            {
                Title = this.TitleText,
                Description = this.DescriptionText
            };

            this.itemsViewModel.Items.Add(newItem);
            this.OnPropertyChanged($"TitleText");
            this.OnPropertyChanged($"DescriptionText");
        }
    }
}
