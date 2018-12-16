namespace PlanningPoker.App.Views.Session
{
    using System;
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

            this.viewModel.Key = sessionKey;
            this.viewModel.Title = sessionKey;
        }

        private void BeginSessionClicked(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new Session());
        }

        protected override void OnAppearing()
        {
            this.viewModel.GetUsersCommand.Execute(null);
            base.OnAppearing();
        }
    }
}
