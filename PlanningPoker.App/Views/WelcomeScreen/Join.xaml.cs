namespace PlanningPoker.App.Views.WelcomeScreen
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Session;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Join : ContentPage
    {
        private readonly JoinViewModel ViewModel;

        public Join()
        {
            this.ViewModel = new JoinViewModel();
            this.BindingContext = this.ViewModel =
                (Application.Current as App)?.Container.GetRequiredService<JoinViewModel>();

            this.InitializeComponent();
        }

        private void HandleClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Lobby());
        }
    }
}
