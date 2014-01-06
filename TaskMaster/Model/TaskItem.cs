using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace TaskMaster.Model
{
    public class TaskItem : ViewModelBase
    {
        private Guid _id;
        private string _description;
        private string _tag;
        private PlayingState? _playingState;
        private TimeSpan _totalPlayedTime;

        private ObservableCollection<PlaySession> _playSessions;
        private PlaySession _currentPlaySession;

        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }

        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                if (value != _tag)
                {
                    _tag = value;
                    RaisePropertyChanged("Tag");
                }
            }
        }

        public ObservableCollection<PlaySession> PlaySessions
        {
            get
            {
                return _playSessions;
            }
        }

        public PlayingState? PlayingState
        {
            get
            {
                return _playingState;
            }
            set
            {
                if (value != _playingState)
                {
                    _playingState = value;
                    RaisePropertyChanged("PlayingState");
                }
            }
        }

        public TimeSpan TotalPlayedTime
        {
            get
            {
                RefreshTotalPlayedTime();
                return _totalPlayedTime;
            }
        }

        public TaskItem(Guid id, string description)
        {
            _id = id;
            _description = description;
        }

        public void PlayASession(DateTime date)
        {
            if (_playSessions == null)
                _playSessions = new ObservableCollection<PlaySession>();

            _currentPlaySession = GetPlaySessionForDay(date);

            if (_currentPlaySession == null)
            {
                _currentPlaySession = new PlaySession(date);
                _playSessions.Add(_currentPlaySession);
            }
        }

        public void AddTimeTick(TimeSpan time)
        {
            _currentPlaySession.PlayedTime += time;
        }

        public bool ThereIsAPlayedSessionThatDay(DateTime date)
        {
            return GetPlaySessionForDay(date) != null;
        }

        public PlaySession GetPlaySessionForDay(DateTime date)
        {
            if (_playSessions == null || date == null)
                return null;

            return _playSessions.Where(x => DateTimesInSameDay(x.Date, date)).FirstOrDefault();
        }

        public bool DateTimesInSameDay(DateTime date1, DateTime date2)
        {
            return
                date1.Year == date2.Year &&
                date1.Month == date2.Month &&
                date1.DayOfYear == date2.DayOfYear;
        }

        public void LoadPlaySessions(List<PlaySession> playSessions)
        {
            if (playSessions == null)
            {
                _playSessions = null;
                return;
            }

            if (!AllPlaySessionsHaveDifferentDates(playSessions))
                throw new ArgumentException(string.Format("Duplicate dates in play sessions for the task {0} {1}", this.Id, this.Tag));

            _playSessions = new ObservableCollection<PlaySession>(playSessions);
        }

        public TimeSpan GetPlayedTimeForDay(DateTime date)
        {
            var playedSession = GetPlaySessionForDay(date);
            if (playedSession == null)
                return new TimeSpan(0);

            return playedSession.PlayedTime;
        }

        private void RefreshTotalPlayedTime()
        {
            _totalPlayedTime = new TimeSpan(0);
            if (_playSessions == null)
                return;

            _playSessions.ToList().ForEach(x => _totalPlayedTime += x.PlayedTime);
        }

        private bool AllPlaySessionsHaveDifferentDates(List<PlaySession> playSessions)
        {
            try
            {
                Dictionary<DateTime, PlaySession> dict = playSessions.ToDictionary<PlaySession, DateTime>(x => x.Date);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        #region Override

        public override bool Equals(object obj)
        {
            var task2 = obj as TaskItem;
            if (task2 != null)
                return this.Id.Equals(task2.Id);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion
    }
}
