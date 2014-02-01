using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using TaskMaster.Design;
using TaskMaster.Model;

namespace TaskMaster.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ArchiveViewModel : ViewModelBase
    {
        ObservableCollection<TaskItem> _archiveTaskList;
        public ObservableCollection<TaskItem> ArchiveTaskList
        {
            get
            {
                return _archiveTaskList;
            }

            set
            {
                if (_archiveTaskList == value)
                {
                    return;
                }

                _archiveTaskList = value;
                RaisePropertyChanged("ArchiveTaskList");
            }
        }

        /// <summary>
        /// Initializes a new instance of the ArchiveViewModel class.
        /// </summary>
        public ArchiveViewModel()
        {
            if (this.IsInDesignMode)
            {
                var designDataService = new DesignTaskListDataService();
                ArchiveTaskList = new ObservableCollection<TaskItem>(designDataService.GetActiveTasks());
            }
        }

        public void RegisterForMessagesBeforeView()
        {
            Messenger.Default.Register<OpenArchivedTasksViewMsg>(this, x => OpenArchivedTasksViewMsgReceived(x));
        }

        private void OpenArchivedTasksViewMsgReceived(OpenArchivedTasksViewMsg msg)
        {
            if (msg == null)
                return;

            ArchiveTaskList = new ObservableCollection<TaskItem>(msg.TaskItems);
        }
    }
}