namespace PlanningPoker.App.Views.Session
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lobby : TabbedPage
    {
        private readonly LobbyViewModel lobbyViewModel;

        public Lobby()
        {
            this.lobbyViewModel = new LobbyViewModel();
            this.BindingContext = this.lobbyViewModel =
                (Application.Current as App)?.Container.GetRequiredService<LobbyViewModel>();

            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.lobbyViewModel.LoadCommand.Execute(null);
        }
    }
}
