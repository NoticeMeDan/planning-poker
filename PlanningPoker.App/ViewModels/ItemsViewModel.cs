namespace PlanningPoker.App.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using PlanningPoker.Shared;

    public class ItemsViewModel : BaseViewModel
    {
        //TODO: Use API to get and set items.

        //private readonly
        public ObservableCollection<ItemDTO> Items { get; set; }

        //public ICommand AddCommand { get; set; }
        public ICommand LoadCommand { get; set; }

        public ItemsViewModel()
        {
            this.Title = "Items";

            this.Items = new ObservableCollection<ItemDTO>();

            this.LoadCommand = new RelayCommand(async _ => this.ExecuteLoadCommand());
        }

        private static ObservableCollection<ItemDTO> MockData()
        {
            var data = new ObservableCollection<ItemDTO>();

            var item1 = new ItemDTO {Id = 1, Title = "Item_1", Description = "First item"};
            var item2 = new ItemDTO {Id = 2, Title = "Item_2", Description = "Second item"};
            var item3 = new ItemDTO {Id = 3, Title = "Item_3", Description = "Third item"};

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
