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
            this.viewModel.NextItemCommand.Execute(null);
        }

        private async Task Next_Clicked()
        {
            var x = await this.viewModel.CheckSessionStatus();

            if (x == null)
            {
                var id = await this.viewModel.GetId();
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.Navigation.PushModalAsync(new NavigationPage(new Summary(id)));
                });
            }
            else
            {
                this.viewModel.NextItemCommand.Execute(null);
            }
        }
    }
}
