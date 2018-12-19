using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using OpenJobScheduler;

namespace PlanningPoker.App.Views.Session
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Session : ContentPage
    {
        private readonly SessionViewModel viewModel;
        private JobScheduler jobScheduler;

        public Session(string sessionkey)
        {
            this.BindingContext = this.viewModel =
                (Application.Current as App)?.Container.GetRequiredService<SessionViewModel>();
            this.viewModel.sessionKey = sessionkey;
            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {    
            //if I am host, do this
            this.viewModel.NextItemCommand.Execute(null);
            //else do this
            //this.viewModel.LoadSessionCommand.Execute(null);
            this.StartCheckSessionThread();
        }

        private void StartCheckSessionThread()
        {
            this.jobScheduler = new JobScheduler(TimeSpan.FromSeconds(5), new Action(async () => { await this.CheckSessionStatus(); }));
            this.jobScheduler.Start();
        }

        private async Task CheckSessionStatus()
        {
            try
            {
                if (this.viewModel.CheckSessionStatus())
                {
                    var id = await this.viewModel.GetId();
                    Device.BeginInvokeOnMainThread(() => {
                        this.Navigation.PushModalAsync(new NavigationPage(new Summary(id)));
                    });
                }
            }
            catch (Exception e) {
                Debug.WriteLine("No fucking way man");
                this.jobScheduler.Stop();
                this.jobScheduler = null;
                this.StartCheckSessionThread();
            }
        }
    }
}
