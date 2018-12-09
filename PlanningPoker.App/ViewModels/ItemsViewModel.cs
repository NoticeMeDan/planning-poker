namespace PlanningPoker.App.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using PlanningPoker.App.ViewModels.Interfaces;
    using PlanningPoker.Shared;

    // This class contains data until repositories is setup
    public class ItemsViewModel : BaseViewModel
    {
        // TODO: Use API to get and set items.
        // private readonly
        public ObservableCollection<ItemDTO> Items { get; }

        // public ICommand AddCommand { get; set; }
        public ICommand LoadCommand { get; }

        public ItemsViewModel()
        {
            this.Title = "Items";

            this.Items = new ObservableCollection<ItemDTO>();

            this.LoadCommand = new RelayCommand(_ => this.ExecuteLoadCommand());
        }

        private static ObservableCollection<ItemDTO> MockData()
        {
            var data = new ObservableCollection<ItemDTO>();

            var item1 = new ItemDTO { Id = 1, Title = "Item_1", Description = "First item" };
            var item2 = new ItemDTO { Id = 2, Title = "Item_2", Description = "Second item" };
            var item3 = new ItemDTO { Id = 3, Title = "Item_3", Description = "Third item" };

            data.Add(item1);
            data.Add(item2);
            data.Add(item3);

            return data;
        }

        private void ExecuteLoadCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            this.Items.Clear();

            var items = MockData();

            foreach (var item in items)
            {
                this.Items.Add(item);
            }

            this.IsBusy = false;
        }
    }
}
