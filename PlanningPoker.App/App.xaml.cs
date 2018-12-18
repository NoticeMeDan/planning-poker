using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using PlanningPoker.App.Models;
using PlanningPoker.App.ViewModels;
using PlanningPoker.App.Views.Session;
using PlanningPoker.App.Views.WelcomeScreen;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PlanningPoker.App
{
    public partial class App : Application
    {
        public static UIParent UiParent { get; set; }

        private readonly Lazy<IServiceProvider> lazyProvider;

        private IPublicClientApplication publicClientApplication;

        public IServiceProvider Container => this.lazyProvider.Value;

        public App()
        {
            this.InitializeComponent();

            var settings = new Settings();

            this.lazyProvider = new Lazy<IServiceProvider>(() => this.ConfigureServices());
            this.publicClientApplication = new PublicClientApplication(settings.ClientId)
            {
                RedirectUri = $"msal{settings.ClientId}://auth",
            };

            DependencyResolver.ResolveUsing(this.Container.GetService);

            // Change Screen for faster development. Standard page is WelcomeScreen()
            this.MainPage = new NavigationPage(new WelcomeScreen());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            var settings = new Settings();
            var publicClientApplication = new PublicClientApplication(settings.ClientId, $"https://login.microsoftonline.com/{settings.TenantId}");
            var handler = new BearerTokenClientHandler(publicClientApplication, settings);
            var httpClient = new HttpClient(handler) { BaseAddress = settings.BackendUrl };

            services.AddSingleton(_ => httpClient);
            services.AddSingleton<ISettings>(settings);
            services.AddSingleton<IPublicClientApplication>(publicClientApplication);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            // Adding the ViewModels
            services.AddScoped<ISessionClient, SessionClient>();
            services.AddScoped<ISummaryClient, SummaryClient>();
            services.AddScoped<WelcomeViewModel>();
            services.AddScoped<SessionCreateViewModel>();
            services.AddScoped<UsersViewModel>();
            services.AddScoped<LobbyViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
