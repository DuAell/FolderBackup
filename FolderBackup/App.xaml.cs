using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using FolderBackup.Service;
using FolderBackup.Service.Implementation;
using FolderBackup.ViewModel;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace FolderBackup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static ServiceProvider ServiceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<IFileSystemService, FileSystemService>();
            services.AddSingleton<ITimeProvider, TimeProvider>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Internationalization fix
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            DispatcherHelper.Initialize();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ServiceProvider.GetService<MainViewModel>().Save();

            base.OnExit(e);
        }
    }
}
