namespace PlanningPoker.App.ViewModels
{
    using PlanningPoker.App.Models;
    using PlanningPoker.Shared;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class SummaryViewModel : BaseViewModel
    {
        private readonly ISummaryRepository repository;

        public ObservableCollection<SummaryDTO> Summary { get; }

        public ICommand LoadSummaryCommand { get; set; }
        /*
        public SummaryViewModel(ISummaryRepository repo)
        {
            this.repository = repo;
            this.Title = "Summary Overview";
            this.Summary = new ObservableCollection<SummaryDTO>();

            this.LoadSummaryCommand = new RelayCommand(_ => this.ExecuteLoadSummaryCommand());
        }
        /*
        private void ExecuteLoadSummaryCommand()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;

            try
            {
                this.Summary.Clear();

                var Summary = this.repository.BuildSummary(session);

                foreach (var car in cars)
                {
                    Cars.Add(car);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }*/
    }
}
