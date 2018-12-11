namespace PlanningPoker.App.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Models;
    using Shared;

    // This class contains data until repositories is setup
    public class ItemsViewModel : BaseViewModel
    {
        // TODO: Use API to get and set items.
        private readonly ISessionRepository sessionRepo;
        private readonly string SessionTitle;
        private string title;
        private string description;

        public ItemsViewModel(ISessionRepository sessionRepo)
        {
            this.SessionTitle = "New session";

            this.sessionRepo = sessionRepo;

            this.Items = new List<ItemCreateUpdateDTO>();

            this.LoadCommand = new RelayCommand(_ => this.ExecuteLoadCommand());
            this.SaveCommand = new RelayCommand(_ => this.ExecuteSaveCommand());
            this.CreateSessionCommand = new RelayCommand(async _ => await this.ExecuteCreateSessionCommand());
        }

        public List<ItemCreateUpdateDTO> Items { get; set; }

        public ICommand LoadCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand CreateSessionCommand { get; }

        public async Task ExecuteCreateSessionCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            var toCreate = new SessionCreateUpdateDTO
            {
                Items = this.Items
            };

            var result = await this.sessionRepo.CreateAsync(toCreate);
            Debug.WriteLine(result);
            this.IsBusy = false;
        }

        private string Title
        {
            get => this.title;
            set => this.SetProperty(ref this.title, value);
        }

        private string Description
        {
            get => this.description;
            set => this.SetProperty(ref this.description, value);
        }

        private void ExecuteSaveCommand()
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

            this.Title = string.Empty;
            this.Description = string.Empty;
            this.LoadCommand.Execute(null);

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

            var items = this.Items;

            foreach (var item in items)
            {
                this.Items.Add(item);
            }

            this.IsBusy = false;
        }

    }
}
