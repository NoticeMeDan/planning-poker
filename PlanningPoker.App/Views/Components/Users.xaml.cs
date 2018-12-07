namespace PlanningPoker.App.Views.Components
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Users : ContentPage
    {
        private readonly UsersViewModel ViewModel;

        public Users()
        {
            InitializeComponent();

            this.ViewModel = new UsersViewModel();

            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<UsersViewModel>();
        }

        protected override void OnAppearing()
        {
            this.ViewModel.LoadCommand.Execute(null);
        }

        private void OnStartSession_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
