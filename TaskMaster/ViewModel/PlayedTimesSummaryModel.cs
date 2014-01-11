using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using TaskMaster.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

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
        ITimeProvider _timeProvider;

        private ObservableCollection<TimeItem> _timeListItems;
        public ObservableCollection<TimeItem> TimeListItems
        {
            get { return _timeListItems; }
            set 
            {
                if (value == _timeListItems)
                    return;

                TimeListItems = value;
                RaisePropertyChanged("TimeListItems");
            }
        }

        /// <summary>
        /// Initializes a new instance of the PlayedTimesSummaryModel class.
        /// </summary>
        public PlayedTimesSummaryModel(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public void RegisterForMessagesBeforeView()
        {
            Messenger.Default.Register<OpenPlayedTimesSummaryMsg>(this, x => OpenPlayedTimesSummaryMsgReceived(x));
        }

        private void OpenPlayedTimesSummaryMsgReceived(OpenPlayedTimesSummaryMsg msg)
        {
            if (msg.TaskItems == null)
                return;

            SetTimeListFromTaskList(msg.TaskItems);
        }

        private void SetTimeListFromTaskList(List<TaskItem> taskList)
        {
            if (taskList == null)
                return;

            _timeListItems = new ObservableCollection<TimeItem>();
            taskList.ForEach(x => AddTimeListItemFromTask(x, _timeListItems));

            TimeListItems = _timeListItems;
        }

        private void AddTimeListItemFromTask(TaskItem taskItem, ObservableCollection<TimeItem> timeListItems)
        {
            timeListItems.Add(new TimeItem(taskItem.GetPlayedTimeForDay(_timeProvider.Now()), taskItem.Tag, taskItem.Description));
        }
    }
}