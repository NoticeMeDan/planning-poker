namespace PlanningPoker.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using PlanningPoker.App.Models;
    using PlanningPoker.App.Views;
    using PlanningPoker.Shared;
    using Xamarin.Forms;

    public class SummaryViewModel : BaseViewModel
    {

        private readonly ISummaryClient summaryClient;

        private string title;
        private int estimate;

        public int sessionId { get; set; }

        public SummaryViewModel(ISummaryClient summaryClient)
        {
            this.summaryClient = summaryClient;

            this.BaseTitle = "Summary Overview";

            this.Items = new ObservableCollection<ItemEstimateDTO>();

            this.LoadSummaryCommand = new RelayCommand(async _ => await this.ExecuteLoadSummaryCommand());
        }

        public ObservableCollection<ItemEstimateDTO> Items { get; set; }

        public ICommand LoadSummaryCommand { get; set; }

        public string Title
        {
            get => this.title;
            set => this.SetProperty(ref this.title, value);
        }

        public int Estimate
        {
            get => this.estimate;
            set => this.SetProperty(ref this.estimate, value);
        }

        private async Task ExecuteLoadSummaryCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            this.Items.Clear();

            var summary = await this.summaryClient.FindBySessionIdAsync(this.sessionId);

            foreach (var s in summary.ItemEstimates)
            {
                this.Items.Add(s);
            }

            this.IsBusy = false;
        }
    }
}
