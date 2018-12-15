using PlanningPoker.App.Models;

namespace PlanningPoker.App.Views.Components
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Cards : Grid
    {
        private readonly CardsViewModel ViewModel;

        public Cards()
        {
            this.InitializeComponent();

            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<CardsViewModel>();
        }

        private void OnAddItem_Clicked(object sender, EventArgs e)
        {
        }
    }
}
