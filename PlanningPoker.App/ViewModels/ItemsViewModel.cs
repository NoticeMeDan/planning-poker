namespace PlanningPoker.App.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Models;
    using Shared;
    using Xamarin.Forms;

    // This class contains data until repositories is setup
    public class ItemsViewModel : BaseViewModel
    {
        private readonly ISessionClient sessionRepo;
        private string title;
        private string description;

        public ItemsViewModel(ISessionClient sessionRepo)
        {
            this.sessionRepo = sessionRepo;

            this.BaseTitle = "Items";

            this.Items = new ObservableCollection<ItemCreateUpdateDTO>();

            this.AddItemCommand = new RelayCommand(_ => this.ExecuteAddItemCommand());
            this.LoadCommand = new Command(() => this.ExecuteLoadCommand());
            this.CreateSessionCommand = new RelayCommand(async _ => await this.ExecuteCreateSessionCommand());
        }

        public ObservableCollection<ItemCreateUpdateDTO> Items { get; set; }

        public ICommand AddItemCommand { get; }

        public ICommand CreateSessionCommand { get; }

        public ICommand LoadCommand { get; }

        public string Title
        {
            get => this.title;
            set => this.SetProperty(ref this.title, value);
        }

        public string Description
        {
            get => this.description;
            set => this.SetProperty(ref this.description, value);
        }

        public string Key { get; private set; }

        public async Task ExecuteCreateSessionCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            var toCreate = new SessionCreateUpdateDTO
            {
                Items = this.Items.ToList()
            };

            var result = await this.sessionRepo.CreateAsync(toCreate);
            this.Key = result.SessionKey;
            this.IsBusy = false;
        }

        private void ExecuteAddItemCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            var toCreate = new ItemCreateUpdateDTO
            {
                Title = this.Title,
                Description = this.Description
            };

            this.Items.Add(toCreate);

            MessagingCenter.Send(this, "ItemAdded", toCreate);

            this.Title = string.Empty;
            this.Description = string.Empty;

            this.IsBusy = false;
        }

        private void ExecuteLoadCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            this.Items.Clear();

            var items = this.Items.ToList();

            foreach (var item in items)
            {
                this.Items.Add(item);
            }

            this.IsBusy = false;
        }
    }
}
