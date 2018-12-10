using PlanningPoker.App.Models;

namespace PlanningPoker.App.Views.Components
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Items : ListView
    {
        private readonly ItemsViewModel viewModel;

        public Items()
        {
            this.BindingContext = this.viewModel =
                (Application.Current as App)?.Container.GetRequiredService<ItemsViewModel>();

            InitializeComponent();
        }
    }
}
