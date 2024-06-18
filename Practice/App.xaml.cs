using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Practice.Extensions;

namespace Practice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App current = (App)Application.Current;

        public IServiceProvider services { get; }

        public App()
        {
            services = ConfigureServices();
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.CollectViewModels();
            services.CollectViews();

            return services.BuildServiceProvider();

        }
    }

}
