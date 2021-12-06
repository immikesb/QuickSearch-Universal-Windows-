using QuickSearch.ViewModels;
using Unity;

namespace QuickSearch.Helpers
{
    public sealed class ViewModelLocator
    {
        public ViewModelLocator()
        {
            container = new();

            container.RegisterType<MainViewModel>();
        }

        #region Properties

        public MainViewModel MainViewModel
        {
            get => container.Resolve<MainViewModel>();
        }

        #endregion

        private readonly UnityContainer container;
    }
}
