namespace PlanningPoker.App.Views.Session
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Session : ContentPage
    {
        private readonly SessionViewModel ViewModel;

        public Session()
        {
            this.ViewModel = new SessionViewModel();
            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<SessionViewModel>();
            this.InitializeComponent();
        }

        private void OnNitpicker_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnReVote_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnNextItem_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnVoteCard_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
