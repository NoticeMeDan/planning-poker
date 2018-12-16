using PlanningPoker.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanningPoker.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Summary : ContentPage
    {
        private readonly SummaryViewModel viewmodel;

        public Summary()
        {
            this.InitializeComponent();
            this.BindingContext = this.viewmodel = DependencyService.Resolve<SummaryViewModel>();

        }
        /*
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (this.viewmodel.)
        }*/
    }
}
