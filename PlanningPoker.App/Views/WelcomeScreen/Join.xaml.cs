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
        private readonly JoinViewModel joinViewModel;

        public Join()
        {
            this.joinViewModel = new JoinViewModel();
            this.BindingContext = this.joinViewModel =
                (Application.Current as App)?.Container.GetRequiredService<JoinViewModel>();

            this.InitializeComponent();
        }

        private void HandleClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Lobby());
        }
    }
}
