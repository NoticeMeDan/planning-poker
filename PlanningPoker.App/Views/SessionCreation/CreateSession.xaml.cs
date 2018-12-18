namespace PlanningPoker.App.Views.SessionCreation
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Session;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSession : ContentPage
    {
        private readonly SessionCreateViewModel sessionViewModel;

        public CreateSession()
        {
            this.BindingContext = this.sessionViewModel =
                (Application.Current as App)?.Container.GetRequiredService<SessionCreateViewModel>();

            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.sessionViewModel.LoadCommand.Execute(null);
        }

        private async Task CreateSessionClicked(object sender, EventArgs e)
        {
            await this.sessionViewModel.CreateSession();
            await this.Navigation.PushModalAsync(new NavigationPage(
                    new Lobby(this.sessionViewModel.Key)));
        }

        private void OnAddItem_Clicked(object sender, EventArgs e)
        {
            this.sessionViewModel.AddItemCommand.Execute(null);
        }
    }
}
