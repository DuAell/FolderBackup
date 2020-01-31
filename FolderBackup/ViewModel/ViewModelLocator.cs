using Microsoft.Extensions.DependencyInjection;

namespace FolderBackup.ViewModel
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => App.ServiceProvider.GetService<MainViewModel>();
    }
}
