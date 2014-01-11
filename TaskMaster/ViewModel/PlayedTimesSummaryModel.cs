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

            if (IsInDesignMode)
            {
                _timeListItems = new ObservableCollection<TimeItem>()
                {
                    new TimeItem(new TimeSpan(1, 3, 1), "TimeItem tag 1", "Description bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla"),
                    new TimeItem(new TimeSpan(0, 59, 0), "TimeItem tag 2", "Description bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla Description bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla Description bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                };
                TimeListItems = _timeListItems;
            }
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
            var timeForDay = taskItem.GetPlayedTimeForDay(_timeProvider.Now());
            if (timeForDay < new TimeSpan(0, 1, 0))
                return;

            timeListItems.Add(new TimeItem(timeForDay, taskItem.Tag, taskItem.Description));
        }
    }
}