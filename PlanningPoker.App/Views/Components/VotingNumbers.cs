using Microsoft.Extensions.DependencyInjection;
using PlanningPoker.App.ViewModels;

namespace PlanningPoker.App.Views.Components {
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using Xamarin.Forms;

    public partial class VotingNumbers : Grid {
        private string estimate;
        private readonly VotingNumbersViewModel ViewModel;

        public VotingNumbers() {

            InitializeComponent();
            this.estimate = "None";
            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<VotingNumbersViewModel>();
        }

        public string GetEstimate() {
            return this.estimate;
        }

        private void OnVote_Clicked(object o, EventArgs e) {
            // this.estimate = sender.Text;
            Debug.WriteLine(estimate);
        }
    }
}
