namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
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
        private string title;
        private string description;
        private JoinCommand joinCommander;

        public ObservableCollection<ItemCreateUpdateDTO> Items { get; set; }

        public ICommand AddItemCommand { get; }

        public ICommand LoadCommand { get; }

        public SessionCreateViewModel(ISessionClient client)
        {
            this.client = client;

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

        private void JoinSession()
        {
            if (this.Key == null)
            {
                return;
            }

            var user = this.CreateScrumMaster();

            this.joinCommander = new JoinCommand(this.client, this.Key, user);

            this.joinCommander.Join.Execute(null);
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
                this.JoinSession();
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
