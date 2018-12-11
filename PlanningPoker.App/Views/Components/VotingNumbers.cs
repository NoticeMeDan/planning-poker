namespace PlanningPoker.App.Views.Components
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using Xamarin.Forms;

    public partial class VotingNumbers : Grid
    {
        private string estimate;

        public VotingNumbers()
        {
            InitializeComponent();
            this.estimate = "None";
        }

        public string GetEstimate()
        {
            return this.estimate;
        }

        private void OnVote_Clicked(Button sender, EventArgs e)
        {
            this.estimate = sender.Text;
            Debug.WriteLine(estimate);
        }
    }
}
