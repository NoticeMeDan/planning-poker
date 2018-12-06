namespace PlanningPoker.App.Views.SessionCreation
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using PlanningPoker.App.ViewModels;
    using PlanningPoker.App.Views.CreateSession;
    using PlanningPoker.App.Views.Session;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSession : ContentPage
    {
        private readonly ItemsViewModel viewModel;

        public CreateSession()
        {
            this.InitializeComponent();

            this.BindingContext = this.viewModel =
                (Application.Current as App)?.Container.GetRequiredService<ItemsViewModel>();
        }

        protected override void OnAppearing()
        {
            this.viewModel.LoadCommand.Execute(null);
        }

        private void ToolbarItem_OnActivated(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new NewItem());
        }

        private void CreateSessionClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Lobby());
        }
    }
}
