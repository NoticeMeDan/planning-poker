namespace PlanningPoker.App.Views.Session
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using PlanningPoker.App.ViewModels;
    using Xamarin.Forms;

    public partial class Lobby : ContentPage
    {
        private LobbyViewModel lobbyViewModel;

        public Lobby(string sessionKey)
        {
            this.InitializeComponent();

            this.BindingContext = this.lobbyViewModel =
               (Application.Current as App)?.Container.GetRequiredService<LobbyViewModel>();
            this.lobbyViewModel.Key = sessionKey;
            this.lobbyViewModel.Title = sessionKey;
        }

        private void BeginSessionClicked(object sender, EventArgs e)
        {
            this.lobbyViewModel.StopFetchingUsers.Execute(null);
            this.Navigation.PushModalAsync(new Session());
        }

        protected override void OnAppearing()
        {
            this.lobbyViewModel.Users.Clear();
            this.lobbyViewModel.Items.Clear();
            this.lobbyViewModel.GetUsersCommand.Execute(null);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            this.lobbyViewModel.Users.Clear();
            this.lobbyViewModel.Items.Clear();
            this.lobbyViewModel.JobScheduler.Stop();
            base.OnDisappearing();
        }
    }
}
