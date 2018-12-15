namespace PlanningPoker.App.Views.WelcomeScreen
{
    using System;
    using System.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;
    using PlanningPoker.App.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Join : ContentPage
    {
        private JoinViewModel _vm;

        public string Nickname { get; set; }

        public string Key { get; set; }

        public Join()
        {
            this.InitializeComponent();
        }

        private void HandleClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("try to connection?");
            this._vm.user.Nickname = this.Nickname;
            this._vm.JoinLobby(this.Key);
        }
    }
}
