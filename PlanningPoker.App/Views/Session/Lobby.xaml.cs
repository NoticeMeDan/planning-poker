namespace PlanningPoker.App.Views.Session
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using PlanningPoker.App.ViewModels;
    using Xamarin.Forms;

    public partial class Lobby : ContentPage
    {
        private LobbyViewModel _vm;

        public Lobby(string sessionKey)
        {
            this.InitializeComponent();

            this.BindingContext = this._vm =
               (Application.Current as App)?.Container.GetRequiredService<LobbyViewModel>();

            this._vm.Key = sessionKey;
            this._vm.Title = sessionKey;
        }

        private void BeginSessionClicked(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new Session());
        }

        protected override void OnAppearing()
        {
            this._vm.GetUsersCommand.Execute(null);
            base.OnAppearing();
        }
    }
}
