namespace PlanningPoker.App.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Models;
    using Shared;
    using Xamarin.Forms;

    // This class contains data until repositories is setup
    public class SessionCreateViewModel : BaseViewModel
    {
        private readonly ISessionClient client;
        private readonly ISettings settings;
        private string title;
        private string description;
        private JoinHelper joinHelper;

        public ObservableCollection<ItemCreateUpdateDTO> Items { get; set; }

        public ICommand AddItemCommand { get; }

        public ICommand LoadCommand { get; }

        public SessionCreateViewModel(ISessionClient client, ISettings settings)
        {
            this.client = client;

            this.settings = settings;

            this.BaseTitle = "Items";

            this.Items = new ObservableCollection<ItemCreateUpdateDTO>();

            this.AddItemCommand = new RelayCommand(_ => this.ExecuteAddItemCommand());
            this.LoadCommand = new Command(() => this.ExecuteLoadCommand());
        }

        private UserCreateDTO CreateScrumMaster()
        {
            return new UserCreateDTO
            {
                IsHost = true,
                Nickname = "ScrumMaster"
            };
        }

        private async Task JoinSession()
        {
            if (this.Key == null)
            {
                return;
            }

            var user = this.CreateScrumMaster();

            this.joinHelper = new JoinHelper(this.client, this.Key, user);

            await this.joinHelper.Join();

            Application.Current.Properties["token"] = this.joinHelper.Token;
        }

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

        public async Task CreateSession()
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

            var result = await this.client.CreateAsync(toCreate);

            if (result != null)
            {
                this.Key = result.SessionKey;
                await this.JoinSession();
            }

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
