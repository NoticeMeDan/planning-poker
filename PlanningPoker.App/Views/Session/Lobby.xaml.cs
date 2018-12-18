namespace PlanningPoker.App.Views.Session
{
    using System;
    using System.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;
    using OpenJobScheduler;
    using PlanningPoker.App.ViewModels;
    using Xamarin.Forms;

    public partial class Lobby : ContentPage
    {
        private LobbyViewModel viewModel;
        private JobScheduler jobScheduler;
        
        public Lobby(string sessionKey)
        {
            this.InitializeComponent();

            this.BindingContext = this.viewModel =
               (Application.Current as App)?.Container.GetRequiredService<LobbyViewModel>();
            Debug.Write("SessionKey: " + sessionKey);
            this.viewModel.Key = sessionKey;
            this.viewModel.Title = sessionKey;
            Debug.Write("SessionKey vm: " + this.viewModel.Key);

            this.jobScheduler = new JobScheduler(TimeSpan.FromSeconds(5), new Action(() => { if (this.viewModel.Started) { this.StartSession(); }; }));
            this.jobScheduler.Start();
        }

        private void BeginSessionClicked(object sender, EventArgs e)
        {
            this.StartSession();
        }

        protected override void OnAppearing()
        {
            this.viewModel.GetUsersCommand.Execute(null);
            base.OnAppearing();
        }

        private void StartSession()
        {
            this.jobScheduler.Stop();
            this.viewModel.StopFetchingUsers.Execute(null);
            this.Navigation.PushModalAsync(new Session());
        }
    }
}
