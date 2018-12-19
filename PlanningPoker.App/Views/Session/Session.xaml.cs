namespace PlanningPoker.App.Views.Session
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
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
            this.jobScheduler = new JobScheduler(TimeSpan.FromSeconds(3), new Action(async () => {await this.CheckSession(); }));
            this.jobScheduler.Start();
            Debug.WriteLine("start success");
        }

        private void Btn_NextItem(object sender, EventArgs e)
        {
            this.sessionViewModel.NextItem.Execute(null);
        }

        private void Btn_ReVote(object sender, EventArgs e)
        {
            this.sessionViewModel.NewRound.Execute(null);
        }

        private async Task CheckSession()
        {
                await this.sessionViewModel.FetchVotes();
                if (this.sessionViewModel.End == true)
                {
                    this.jobScheduler.Stop();
                    await this.Navigation.PushModalAsync(new NavigationPage(new Summary(12)));
                }
        }
    }
}
