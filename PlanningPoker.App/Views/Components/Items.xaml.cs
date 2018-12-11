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
        private readonly ItemListViewModel itemListViewModel;

        public Items()
        {
            this.InitializeComponent();
            this.BindingContext = this.itemsViewModel =
                (Application.Current as App)?.Container.GetRequiredService<ItemsViewModel>();
            this.BindingContext = this.itemListViewModel =
                (Application.Current as App)?.Container.GetRequiredService<ItemListViewModel>();
        }
    }
}
