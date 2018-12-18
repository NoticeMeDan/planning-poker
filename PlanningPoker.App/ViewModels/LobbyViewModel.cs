namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using OpenJobScheduler;
    using PlanningPoker.App.Models;
    using PlanningPoker.Shared;

    public class LobbyViewModel : BaseViewModel
    {
        private readonly ISessionClient repository;
        private bool loading;
        private SessionDTO session;

        public JobScheduler JobScheduler { get; set; }

        private string key;
        private string title;

        public ObservableCollection<UserDTO> Users { get; set; }

        public ObservableCollection<ItemDTO> Items { get; set; }

        public ICommand GetUsersCommand { get; }

        public ICommand StopFetchingUsers { get; }

        public LobbyViewModel(ISessionClient client)
        {
            this.repository = client;
            this.Users = new ObservableCollection<UserDTO>();
            this.Items = new ObservableCollection<ItemDTO>();
            this.GetUsersCommand = new RelayCommand(_ => this.ExecuteGetUsersCommand());
            this.StopFetchingUsers = new RelayCommand(_ => this.ExecuteKillThread());
        }

        public async Task<ItemDTO> CheckSessionStatus()
        {
            return await this.repository.GetCurrentItem(this.Key);
        }

        private void ExecuteKillThread()
        {
            this.JobScheduler.Stop();
        }

        private void ExecuteGetUsersCommand()
        {
            this.JobScheduler = new JobScheduler(TimeSpan.FromSeconds(2), new Action(async () => { await this.FetchUsers(); }));
            this.JobScheduler.Start();
        }

        private async Task FetchUsers()
        {
            if (this.loading)
            {
                return;
            }

            this.loading = true;

            this.session = await this.repository.GetByKeyAsync(this.Key);

            this.UpdateItemCollection(this.session.Items);

            this.UpdateUserCollection(this.session.Users);

            this.loading = false;
        }

        private void UpdateItemCollection(List<ItemDTO> items)
        {
            if (this.Items.Count < 1)
            {
                items.ForEach(i =>
                {
                    this.Items.Add(i);
                });
            }
        }

        private void UpdateUserCollection(ICollection<UserDTO> users)
        {
            if (this.session != null)
            {
                this.Users.Clear();

                users.ToList().ForEach(u =>
                {
                        this.Users.Add(u);
                });
            }
            else
            {
                this.Users.Clear();
                this.Title = "No session found...";
                this.JobScheduler.Stop();
            }
        }

        public string Title
        {
            get => this.title;
            set => this.SetProperty(ref this.title, "Session-key: " + value);
        }

        public string Key
        {
            get => this.key;
            set => this.SetProperty(ref this.key, value);
        }
    }
}
