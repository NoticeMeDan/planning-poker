namespace PlanningPoker.App.Views.Session
{
    using System;
    using Xamarin.Forms;

    public partial class Lobby : ContentPage
    {
        public Lobby()
        {
            InitializeComponent();
        }

        private void BeginSessionClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Session());
        }
    }
}
