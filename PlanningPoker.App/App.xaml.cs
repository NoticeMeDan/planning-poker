using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using PlanningPoker.App.Models;
using PlanningPoker.App.ViewModels;
using PlanningPoker.App.Views.WelcomeScreen;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PlanningPoker.App
{
    public partial class App : Application
    {
        private static PublicClientApplication publicClientApplication = null;

        public static UIParent UiParent { get; set; }

        private readonly Lazy<IServiceProvider> lazyProvider;

        public IServiceProvider Container => this.lazyProvider.Value;

        public App()
        {
            this.InitializeComponent();
            var settings = new Settings();

            this.lazyProvider = new Lazy<IServiceProvider>(() => this.ConfigureServices());
            publicClientApplication = new PublicClientApplication(settings.ClientId)
            {
                RedirectUri = $"msal{settings.ClientId}://auth",
            };

            DependencyResolver.ResolveUsing(this.Container.GetService);

            // Change Screen for faster development. Standard page is WelcomeScreen()
            this.MainPage = new NavigationPage(new WelcomeScreen());
        }

        public static IPublicClientApplication GetPublicClientApplication()
        {
            return publicClientApplication;
        }

        public static void SetPublicClientApplication(PublicClientApplication pca)
        {
            publicClientApplication = pca;
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

            var handler = new BearerTokenHttpClientHandler(publicClientApplication, settings);

            var client = new HttpClient(handler);

            client.BaseAddress = settings.BackendUrl;

            services.AddSingleton<ISettings>(settings);
            services.AddSingleton(_ => client);
            services.AddSingleton<IPublicClientApplication>(publicClientApplication);

            // Adding the ViewModels
            services.AddScoped<LoginViewModel>();
            services.AddScoped<ItemsViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
