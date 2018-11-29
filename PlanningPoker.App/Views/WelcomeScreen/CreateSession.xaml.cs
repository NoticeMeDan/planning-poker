using System;
using PlanningPoker.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanningPoker.App.Views.WelcomeScreen {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSession : ContentPage
    {
        private readonly CreateSessionViewModel _createSessionViewModel;

        public CreateSession()
        {
            InitializeComponent();
            BindingContext = _createSessionViewModel = new CreateSessionViewModel();
        }

        private void HandleLoginClicked(object sender, EventArgs e)
        {
            _createSessionViewModel.ExecuteLoginCommand();
        }
    }
}
