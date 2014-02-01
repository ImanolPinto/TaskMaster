/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:TaskMaster.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using TaskMaster.Model;

namespace TaskMaster.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<ITaskPlayer, TaskPlayer>();
            SimpleIoc.Default.Register<ITimeProvider, TimeProvider>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<ITaskListDataService, Design.DesignTaskListDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<ITaskListDataService, TaskListDataService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<PlayedTimesSummaryModel>();
            SimpleIoc.Default.Register<ArchiveViewModel>();

            // Register for messages sent before the view binds the model
            var playedTimesSummaryModel = ServiceLocator.Current.GetInstance<PlayedTimesSummaryModel>();
            playedTimesSummaryModel.RegisterForMessagesBeforeView();

            var archiveViewModel = ServiceLocator.Current.GetInstance<ArchiveViewModel>();
            archiveViewModel.RegisterForMessagesBeforeView();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PlayedTimesSummaryModel PlayedTimesSummaryModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlayedTimesSummaryModel>();
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ArchiveViewModel ArchiveViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ArchiveViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}