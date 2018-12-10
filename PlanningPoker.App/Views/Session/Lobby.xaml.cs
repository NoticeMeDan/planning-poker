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
        private readonly LobbyViewModel ViewModel;

        public Lobby()
        {
            this.ViewModel = new LobbyViewModel();
            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<LobbyViewModel>();

            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.ViewModel.LoadCommand.Execute(null);
        }
    }
}
