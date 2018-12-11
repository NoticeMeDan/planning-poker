namespace PlanningPoker.App.Views.SessionCreation
{
    using System;
    using Components;
    using Microsoft.Extensions.DependencyInjection;
    using Session;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSession : ContentPage
    {
        private readonly ItemsViewModel viewModel;

        public CreateSession()
        {
            this.BindingContext = this.viewModel =
                (Application.Current as App)?.Container.GetRequiredService<ItemsViewModel>();

            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.viewModel.LoadCommand.Execute(null);
        }

        private void CreateSessionClicked(object sender, EventArgs e)
        {
            this.viewModel.CreateSessionCommand.Execute(null);
            this.Navigation.PushModalAsync(
                new Lobby());
        }

        private void OnAddItem_Clicked(object sender, EventArgs e)
        {
            this.viewModel.SaveCommand.Execute(null);
        }
    }
}
