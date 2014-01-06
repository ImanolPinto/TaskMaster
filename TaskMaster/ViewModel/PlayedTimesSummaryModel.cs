using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using TaskMaster.Model;
using System.Collections.Generic;

namespace TaskMaster.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PlayedTimesSummaryModel : ViewModelBase
    {
        private List<TaskItem> _taskList;
        public List<TaskItem> TaskList
        {
            get { return _taskList; }
        }



        /// <summary>
        /// Initializes a new instance of the PlayedTimesSummaryModel class.
        /// </summary>
        public PlayedTimesSummaryModel()
        {
            
        }

        public void RegisterForMessagesBeforeView()
        {
            Messenger.Default.Register<OpenPlayedTimesSummaryMsg>(this, x => SetTaskList(x.TaskItems));
        }

        public void SetTaskList(List<TaskItem> taskList)
        {
            _taskList = taskList;
        }
    }
}