using System;
using PlanningPoker.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanningPoker.App.Views.WelcomeScreen {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Create : ContentPage
    {
        private readonly CreateViewModel _createViewModel;

        public Create()
        {
            InitializeComponent();
            BindingContext = _createViewModel = new CreateViewModel();
        }

        private void HandleLoginClicked(object sender, EventArgs e)
        {
            _createViewModel.ExecuteLoginCommand();
        }
    }
}
