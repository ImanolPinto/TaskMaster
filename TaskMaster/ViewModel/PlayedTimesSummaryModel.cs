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
        List<TaskItem> _taskList;

        private DateTime _targetDate;
        public DateTime TargetDate
        {
            get { return _targetDate; }
            set
            {
                if (_targetDate == value)
                    return;

                _targetDate = value;
                SetTimeListFromTaskList();

                RaisePropertyChanged("TargetDate");
            }
        }

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
                TargetDate = _timeProvider.Now();
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

            _taskList = msg.TaskItems;
            TargetDate = _timeProvider.Now();
        }

        private void SetTimeListFromTaskList()
        {
            if (_taskList == null)
                return;

            _timeListItems = new ObservableCollection<TimeItem>();
            _taskList.ForEach(x => AddTimeListItemFromTask(x, _timeListItems, _targetDate));

            TimeListItems = _timeListItems;
        }

        private void AddTimeListItemFromTask(TaskItem taskItem, ObservableCollection<TimeItem> timeListItems, DateTime targetDate)
        {
            var timeForDay = taskItem.GetPlayedTimeForDay(targetDate);
            if (timeForDay < new TimeSpan(0, 1, 0))
                return;

            timeListItems.Add(new TimeItem(timeForDay, taskItem.Tag, taskItem.Description));
        }

        /// <summary>
        /// Only for testing purposes
        /// </summary>
        /// <param name="taskList"></param>
        public void SetTaskList(List<TaskItem> taskList)
        {
            _taskList = taskList;
        }
    }
}