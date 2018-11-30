using System;
using PlanningPoker.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanningPoker.App.Views.WelcomeScreen {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private readonly JoinViewModel joinViewModel;

        public Login()
        {
            InitializeComponent();
            this.BindingContext = this.joinViewModel = new JoinViewModel();
        }

        private void HandleLoginClicked(object sender, EventArgs e)
        {
            this.joinViewModel.ExecuteLoginCommand();
        }
    }
}
