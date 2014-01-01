using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public class PlayedTask : ViewModelBase
    {
        private Guid _taskId;
        private string _taskTag;
        private string _taskDescription;
        private DateTime _playedDate;
        private TimeSpan _playedTime;

        public Guid TaskId
        {
            get
            {
                return _taskId;
            }
        }

        public DateTime PlayedDate
        {
            get
            {
                return _playedDate;
            }
        }

        public TimeSpan PlayedTime
        {
            get
            {
                return _playedTime;
            }
            set
            {
                if (value != _playedTime)
                {
                    _playedTime = value;
                    RaisePropertyChanged("PlayedTime");
                }
            }
        }

        public string TaskTag
        {
            get
            {
                return _taskTag;
            }
        }

        public string TaskDescription
        {
            get
            {
                return _taskDescription;
            }
        }

        public PlayedTask(Guid taskId, string taskTag, string taskDescription, DateTime playedDate)
        {
            _taskId = taskId;
            _taskTag = taskTag;
            _taskDescription = taskDescription;
            _playedDate = playedDate;
        }
    }
}
