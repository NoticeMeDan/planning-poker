using System.Diagnostics;

namespace PlanningPoker.App.Views.Session
{
    using System;
    using System.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Lobby : ContentPage
    {
        private readonly LobbyViewModel ViewModel;

        public Lobby()
        {
            this.ViewModel = new LobbyViewModel();

            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<LobbyViewModel>();

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.ViewModel.LoadCommand.Execute(null);
        }

        private void OnStartSession_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine(this.ViewModel.LoadCommand);
            Debug.WriteLine(this.ViewModel.Users);
            this.Navigation.PushModalAsync(new Session());
        }
    }
}
