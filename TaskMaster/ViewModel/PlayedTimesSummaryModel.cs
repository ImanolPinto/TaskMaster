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
                SetTargetDate(value);
            }
        }

        private void SetTargetDate(DateTime value)
        {
            if (_targetDate == value)
                return;

            _targetDate = value;
            SetTimeListFromTaskList();
            RaisePropertyChanged("TargetDate");
        }

        private ObservableCollection<TimeItem> _timeListItems;
        public ObservableCollection<TimeItem> TimeListItems
        {
            get { return _timeListItems; }
            set 
            {
                SetTimeListItems(value);
            }
        }

        private void SetTimeListItems(ObservableCollection<TimeItem> value)
        {
            if (value == _timeListItems)
                return;

            _timeListItems = value;
            RaisePropertyChanged("TimeListItems");
        }

        /// <summary>
        /// Initializes a new instance of the PlayedTimesSummaryModel class.
        /// </summary>
        public PlayedTimesSummaryModel(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;

            if (IsInDesignMode)
            {
                var timeListItems = new ObservableCollection<TimeItem>()
                {
                    new TimeItem(new TimeSpan(1, 3, 1), "TimeItem tag 1", "Description bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla"),
                    new TimeItem(new TimeSpan(0, 59, 0), "TimeItem tag 2", "Description bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla Description bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla Description bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                };
                SetTimeListItems(timeListItems);
                SetTargetDate(_timeProvider.Now());
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
            SetTargetDate(_timeProvider.Now());
        }

        private void SetTimeListFromTaskList()
        {
            if (_taskList == null)
                return;

            _taskList.ForEach(x => AddTimeListItemFromTask(x, _targetDate));
        }

        private void AddTimeListItemFromTask(TaskItem taskItem, DateTime targetDate)
        {
            var timeForDay = taskItem.GetPlayedTimeForDay(targetDate);
            if (timeForDay < new TimeSpan(0, 1, 0))
                return;

            if (_timeListItems == null)
                SetTimeListItems(new ObservableCollection<TimeItem>());

            TimeListItems.Add(new TimeItem(timeForDay, taskItem.Tag, taskItem.Description));
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