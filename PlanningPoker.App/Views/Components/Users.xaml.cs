namespace PlanningPoker.App.Views.Components
{
    using System;
    using System.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Users : ContentPage
    {
        private readonly UsersViewModel usersViewModel;

        public Users()
        {
            this.usersViewModel = new UsersViewModel();

            this.BindingContext = this.usersViewModel =
                (Application.Current as App)?.Container.GetRequiredService<UsersViewModel>();

            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.usersViewModel.LoadCommand.Execute(null);
        }

        private void OnStartSession_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine(this.usersViewModel.LoadCommand);
            Debug.WriteLine(this.usersViewModel.Users);
        }
    }
}
