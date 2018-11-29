using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanningPoker.App.Views.WelcomeScreen {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Create : ContentPage
    {
        public Create()
        {
            InitializeComponent();
        }

        private void HandleClicked(object sender, EventArgs e)
        {
            this.DisplayAlert("Login", "Azure Active Directory", "Accept", "Cancel");
        }
    }
}
