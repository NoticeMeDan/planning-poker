namespace PlanningPoker.App.Views.Components
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using ViewModels;
    using Xamarin.Forms;

    public partial class Items : ListView
    {
        private readonly ItemsViewModel itemsViewModel;

        public Items()
        {
            this.InitializeComponent();
            this.BindingContext = this.itemsViewModel =
                (Application.Current as App)?.Container.GetRequiredService<ItemsViewModel>();
        }
    }
}
