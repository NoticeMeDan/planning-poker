namespace PlanningPoker.App.Views.WelcomeScreen
{
    using System;
    using System.Diagnostics;
    using PlanningPoker.App.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Join : ContentPage
    {
        private JoinViewModel joinViewModel;

        public Join()
        {
            this.joinViewModel = new JoinViewModel();
            this.InitializeComponent();
        }

        private void HandleClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Do we have a connection?");
            this.joinViewModel.JoinLobby("1234567");
        }
    }
}
