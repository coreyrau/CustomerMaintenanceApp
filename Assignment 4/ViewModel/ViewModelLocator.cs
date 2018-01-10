/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Assignment_4"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Messaging;
using Assignment_4.ViewModel;


namespace Assignment_4
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddViewModel>();
            SimpleIoc.Default.Register<ModifyViewModel>();
            Messenger.Default.Register <NotificationMessage>(this, NotifyUserMethod);
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public AddViewModel Add
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddViewModel>();
            }
        }

        public ModifyViewModel Modify
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ModifyViewModel>();
            }
        }

        private void NotifyUserMethod(NotificationMessage message)
        {
            MessageBox.Show(message.Notification);
        }
    }
}