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

            this.LoadSummaryCommand = new RelayCommand(_ => this.ExecuteLoadSummaryCommand());
        }


        //public ObservableCollection<SummaryDTO> Summary { get; set; }

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

        /*

        private static ObservableCollection<ItemEstimateDTO> MockData()
        {
            var data = new ObservableCollection<ItemEstimateDTO>();

            var item1 = new ItemEstimateDTO { Id = 1, Estimate = 6, ItemTitle = "First item" };
            var item2 = new ItemEstimateDTO { Id = 2, Estimate = 42, ItemTitle = "Second item" };
            var item3 = new ItemEstimateDTO { Id = 3, Estimate = 7, ItemTitle = "Third item" };

            data.Add(item1);
            data.Add(item2);
            data.Add(item3);

            return data;
        } */

        private async Task ExecuteLoadSummaryCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            this.Items.Clear();
            //give input this.sessionId
            var summary = await this.summaryClient.FindBySessionIdAsync(12);

            //var items = this.Items;

            foreach (var s in summary.ItemEstimates)
            {
                this.Items.Add(s);
            }

            this.IsBusy = false;
        }
    }
}
