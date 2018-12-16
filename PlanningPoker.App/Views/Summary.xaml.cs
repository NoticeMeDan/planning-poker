namespace PlanningPoker.App.Views
{
    using Microsoft.Extensions.DependencyInjection;
    using PlanningPoker.App.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    public partial class Summary : ContentPage
    {
        private readonly SummaryViewModel summaryViewModel;

        public Summary(int sessionId)
        {
            // this.summaryViewModel = new SummaryViewModel();

            this.BindingContext = this.summaryViewModel =
                (Application.Current as App)?.Container.GetRequiredService<SummaryViewModel>();

            this.summaryViewModel.sessionId = sessionId;

            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.summaryViewModel.LoadSummaryCommand.Execute(null);
        }
    }
}
