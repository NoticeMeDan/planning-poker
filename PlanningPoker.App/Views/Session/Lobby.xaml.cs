namespace PlanningPoker.App.Views.Session
{
    using System;
    using System.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;
    using PlanningPoker.App.ViewModels;
    using Xamarin.Forms;

    public partial class Lobby : ContentPage
    {
        private LobbyViewModel viewModel;

        public Lobby(string sessionKey)
        {
            this.InitializeComponent();

            this.BindingContext = this.viewModel =
               (Application.Current as App)?.Container.GetRequiredService<LobbyViewModel>();
            Debug.Write("SessionKey: " + sessionKey);
            this.viewModel.Key = sessionKey;
            this.viewModel.Title = sessionKey;
            Debug.Write("SessionKey vm: " + this.viewModel.Key);
        }

        private void BeginSessionClicked(object sender, EventArgs e)
        {
            this.viewModel.StopFetchingUsers.Execute(null);
            this.Navigation.PushModalAsync(new Session());
        }

        protected override void OnAppearing()
        {
            this.viewModel.Users.Clear();
            this.viewModel.GetUsersCommand.Execute(null);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            this.viewModel.JobScheduler.Stop();
            base.OnDisappearing();
        }
    }
}
