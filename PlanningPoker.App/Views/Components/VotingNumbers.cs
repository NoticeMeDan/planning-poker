namespace PlanningPoker.App.Views.Components
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using Xamarin.Forms;

    public partial class VotingNumbers : Grid
    {
        public VotingNumbers()
        {
            this.InitializeComponent();
        }

        private void OnVote_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine(e);
            Debug.WriteLine(sender);
        }
    }
}
