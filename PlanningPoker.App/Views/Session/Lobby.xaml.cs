namespace PlanningPoker.App.Views.Session
{
    using Microsoft.Extensions.DependencyInjection;
    using PlanningPoker.App.ViewModels;
    using System;
    using Xamarin.Forms;

    public partial class Lobby : ContentPage
    {
        private LobbyViewModel _vm;

        public Lobby(string sessionKey)
        {
            this.InitializeComponent();

            this.BindingContext = this._vm =
               (Application.Current as App)?.Container.GetRequiredService<LobbyViewModel>();
        }

        private void BeginSessionClicked(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new Session());
        }
    }
}
