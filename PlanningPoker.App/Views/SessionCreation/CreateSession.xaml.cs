namespace PlanningPoker.App.Views.SessionCreation
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Components;
    using Microsoft.Extensions.DependencyInjection;
    using Session;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSession : ContentPage
    {
        private readonly SessionCreateViewModel viewModel;

        public CreateSession()
        {
            this.BindingContext = this.viewModel =
                (Application.Current as App)?.Container.GetRequiredService<SessionCreateViewModel>();

            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.viewModel.LoadCommand.Execute(null);
        }

        private async Task CreateSessionClicked(object sender, EventArgs e)
        {
            await this.viewModel.CreateSession();
            await this.Navigation.PushModalAsync(new NavigationPage(
                    new Lobby(this.viewModel.Key)));
        }

        private void OnAddItem_Clicked(object sender, EventArgs e)
        {
            this.viewModel.AddItemCommand.Execute(null);
        }
    }
}
