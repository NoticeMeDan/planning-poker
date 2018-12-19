namespace PlanningPoker.App.Views.Session
{
    using Microsoft.Extensions.DependencyInjection;
    using PlanningPoker.App.ViewModels;
    using Xamarin.Forms;

    public partial class Summary : ContentPage
    {
        private readonly SummaryViewModel summaryViewModel;

        public Summary(int sessionId)
        {
            this.BindingContext = this.summaryViewModel =
                (Application.Current as App)?.Container.GetRequiredService<SummaryViewModel>();

            this.summaryViewModel.SessionId = sessionId;

            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.summaryViewModel.LoadSummaryCommand.Execute(null);
        }
    }
}
