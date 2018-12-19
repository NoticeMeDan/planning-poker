namespace PlanningPoker.App.Views.Session
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using OpenJobScheduler;
    using PlanningPoker.App.ViewModels;
    using Xamarin.Forms;

    public partial class Lobby : ContentPage
    {
        private LobbyViewModel lobbyViewModel;
        private JobScheduler jobScheduler;

        public Lobby(string sessionKey)
        {
            this.InitializeComponent();

            this.BindingContext = this.lobbyViewModel =
               (Application.Current as App)?.Container.GetRequiredService<LobbyViewModel>();
            this.lobbyViewModel.Key = sessionKey;
            this.lobbyViewModel.Title = sessionKey;
        }

        private async Task BeginSessionClicked()
        {
            this.jobScheduler.Stop();
            this.lobbyViewModel.StopFetchingUsers.Execute(null);
            await this.Navigation.PushModalAsync(new NavigationPage(new Session(this.lobbyViewModel.Key)));
        }

        protected override void OnAppearing()
        {
            this.lobbyViewModel.Users.Clear();
            this.lobbyViewModel.Items.Clear();
            this.lobbyViewModel.GetUsersCommand.Execute(null);
            this.StartCheckSessionThread();
            base.OnAppearing();
        }

        private void StartCheckSessionThread()
        {
            this.jobScheduler = new JobScheduler(TimeSpan.FromSeconds(2), new Action(async () => { await this.CheckSessionStatus(); }));
            this.jobScheduler.Start();
        }

        private async Task CheckSessionStatus()
        {
            if (await this.lobbyViewModel.CheckSessionStatus() != null)
            {
                await this.BeginSessionClicked();
            }
        }
    }
}
