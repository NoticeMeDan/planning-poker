namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using OpenJobScheduler;
    using PlanningPoker.App.Models;
    using PlanningPoker.Shared;
    using Xamarin.Forms;

    public class LobbyViewModel : BaseViewModel
    {
        private readonly ISessionClient repository;
        private bool loading;
        private SessionDTO session;

        private string key;
        private string title;

        public JobScheduler JobScheduler { get; set; }

        public ObservableCollection<UserDTO> Users { get; set; }

        public ObservableCollection<ItemDTO> Items { get; set; }

        public bool IsHost { get; set; }

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
            var currentItem = await this.repository.GetCurrentItem(this.Key);
            if (currentItem != null)
            {
                this.loading = true;
            }

            return currentItem;
        }

        public async Task FetchUsers()
        {
            if (this.loading)
            {
                return;
            }

            this.loading = true;

            this.session = await this.repository.GetByKeyAsync(this.Key);

            if (this.Items.Count < 1)
            {
                await this.CheckUserIsHost(this.session);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    this.UpdateItemCollection(this.session.Items);

                    this.UpdateUserCollection(this.session.Users);
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            });

            this.loading = false;
        }

        public void UpdateItemCollection(List<ItemDTO> items)
        {
            if (this.Items.Count < 1)
            {
                items.ForEach(i =>
                {
                    this.Items.Add(i);
                });
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

        private void ExecuteKillThread()
        {
            if (this.JobScheduler != null)
            {
                this.JobScheduler.Stop();
            }
        }

        private void ExecuteGetUsersCommand()
        {
            this.JobScheduler = new JobScheduler(TimeSpan.FromSeconds(2), new Action(async () => { await this.FetchUsers(); }));
            this.JobScheduler.Start();
        }

        private async Task CheckUserIsHost(SessionDTO session)
        {
            var userState = await this.repository.WhoAmI(session.SessionKey);
            var user = session.Users.ToList().Where(u => u.Id == userState.Id).Select(u => u).FirstOrDefault();
            Debug.WriteLine("IsHOST: " + user.IsHost);
            if (user != null)
            {
                this.IsHost = user.IsHost;
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
    }
}
