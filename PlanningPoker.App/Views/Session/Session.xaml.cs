namespace PlanningPoker.App.Views.Session
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using OpenJobScheduler;
    using PlanningPoker.App.ViewModels;
    using Xamarin.Forms;

    public partial class Session : ContentPage
    {
        private readonly SessionViewModel sessionViewModel;
        private JobScheduler jobScheduler;

        public Session(string sessionKey)
        {
            this.InitializeComponent();
            this.BindingContext = this.sessionViewModel =
              (Application.Current as App)?.Container.GetRequiredService<SessionViewModel>();
            this.sessionViewModel.SessionKey = sessionKey;
            this.sessionViewModel.LoadCommand.Execute(null);
            this.jobScheduler = new JobScheduler(TimeSpan.FromSeconds(2), new Action(() => { this.CheckSession(); }));
        }

        protected override void OnAppearing()
        {
            this.jobScheduler.Start();
            base.OnAppearing();
        }

        private void Btn_NextItem(object sender, EventArgs e)
        {
            this.sessionViewModel.NextItem.Execute(null);
        }

        private void Btn_ReVote(object sender, EventArgs e)
        {
            this.sessionViewModel.NewRound.Execute(null);
        }

        private void CheckSession()
        {
            if (this.sessionViewModel.End == true)
            {
                this.jobScheduler.Stop();
                this.Navigation.PushModalAsync(new NavigationPage(new Summary(12)));
            }
        }
    }
}
