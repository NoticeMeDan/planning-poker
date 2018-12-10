namespace PlanningPoker.App.Views.Components
{
    using System;
    using System.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Users : ContentPage
    {
        private readonly UsersViewModel ViewModel;

        public Users()
        {
            this.ViewModel = new UsersViewModel();

            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<UsersViewModel>();

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
            this.Navigation.PushAsync(new Session.Session());
        }
    }
}
