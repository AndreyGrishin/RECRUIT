using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using RecruIT.View;

namespace RecruIT.ViewModel
{
    public class ViewModelLocator
    {/// <summary>
     /// Initializes a new instance of the ViewModelLocator class.
     /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            var nav = new NavigationService();
            nav.Configure("LoginPage", typeof(LoginPage));
            nav.Configure("MainPage", typeof(MainPage));
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
            }
            else
            {
                // Create run time view services and models
            }
            
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<INavigationService>(() => nav);
        }

        public LoginViewModel Login => ServiceLocator.Current.GetInstance<LoginViewModel>();

        public static MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();


        public static void Cleanup()
        {
            
            // TODO Clear the ViewModels
        }
    }
}
