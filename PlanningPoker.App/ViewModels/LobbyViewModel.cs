namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using OpenJobScheduler;
    using PlanningPoker.App.Models;
    using PlanningPoker.Shared;

    public class LobbyViewModel : BaseViewModel
    {
        private readonly ISessionClient repository;
        private bool loading;

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

        private void ExecuteKillThread()
        {
            this.JobScheduler.Stop();
        }

        private void ExecuteGetUsersCommand()
        {
            this.JobScheduler = new JobScheduler(TimeSpan.FromSeconds(2), new Action(async () => { await this.FetchUsers(); }));
            this.JobScheduler.Start();
        }

        public async Task FetchUsers()
        {
            if (this.loading)
            {
                return;
            }

            this.loading = true;

            var session = await this.repository.GetByKeyAsync(this.Key);

            if (this.Users.Count < 1)
            {
                this.UpdateItemCollection(session.Items);
            }

            if (session != null)
            {
                this.UpdateUserCollection(session.Users);
            }
            else
            {
                this.Users.Clear();
                this.Title = "No session found...";
                this.JobScheduler.Stop();
            }

            this.loading = false;
        }

        public void UpdateItemCollection(List<ItemDTO> items)
        {
            items.ForEach(i =>
            {
                this.Items.Add(i);
            });
        }

        public void UpdateUserCollection(ICollection<UserDTO> users)
        {
            this.Users.Clear();

            users.ToList().ForEach(u =>
            {
                    this.Users.Add(u);
            });
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
