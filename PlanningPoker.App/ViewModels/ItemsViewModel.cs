using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PlanningPoker.App.Models;
using Xamarin.Forms;

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
        private readonly IItemRepository itemRepo;
        public ObservableCollection<ItemDTO> Items { get; set; }
        public string Session { get; }

        // public ICommand AddCommand { get; set; }
        public ICommand LoadCommand { get; }
        public ICommand SaveCommand { get; }

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public ItemsViewModel()
        {
            this.Session = "New session";

            this.Items = new ObservableCollection<ItemDTO>();

            this.LoadCommand = new RelayCommand(_ => this.ExecuteLoadCommand());
            this.SaveCommand = new RelayCommand(_ => this.ExecuteSaveCommand());
        }

        private void ExecuteSaveCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            var toCreate = new ItemDTO()
            {
                Title = Title,
                Description = Description
            };

            Items.Add(toCreate);

            Title = string.Empty;
            Description = string.Empty;
            this.LoadCommand.Execute(null);

            IsBusy = false;
        }

        private void ExecuteLoadCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            this.Items.Clear();

            var items = Items;

            foreach (var item in items)
            {
                this.Items.Add(item);
            }

            this.IsBusy = false;
        }
    }
}
