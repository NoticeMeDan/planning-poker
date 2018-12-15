using Microsoft.Extensions.DependencyInjection;
using PlanningPoker.App.Models;
using PlanningPoker.App.ViewModels;

namespace PlanningPoker.App.Views.Session
{
    using System;
    using Xamarin.Forms;

    public partial class Session : ContentPage
    {
        private readonly SessionViewModel ViewModel;

        public Session()
        {
            this.InitializeComponent();

            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<SessionViewModel>();
        }

        private void EstimateItem_Clicked(object sender, EventArgs e) {
            throw new NotImplementedException();
        }
    }
}
