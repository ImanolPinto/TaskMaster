using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace TaskMaster.Model
{
    public class TaskPlayer : ViewModelBase, ITaskPlayer
    {
        ITimeProvider _timeProvider;
        Timer _timer;
        TimeSpan _ellapsedPlayingTime;
        double _interval;

        TaskItem _playingTask;

        public TimeSpan EllapsedPlayingTime
        {
            get
            {
                return _ellapsedPlayingTime;
            }
        }

        public TaskPlayer(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            InstantiateTimerWithInterval(1000);
        }

        public void InstantiateTimerWithInterval(double interval)
        {
            _interval = interval;
            _timer = new Timer();
            _timer.Interval = _interval;
            _timer.Elapsed += PlayerTick;
            _ellapsedPlayingTime = new TimeSpan();
        }

        public void Play(TaskItem task)
        {
            if (!CanPlay(task))
                return;

            _playingTask = task;

            _timer.Start();
            _playingTask.PlayingState = PlayingState.Playing;
            _playingTask.PlayASession(_timeProvider.Now());
        }

        public bool CanPlay(TaskItem _task)
        {
            if (_task == null)
                return false;

            return _task.PlayingState != PlayingState.Playing;
        }

        public void Pause(TaskItem task)
        {
            if (task == null)
                return;

            _timer.Stop();
            task.PlayingState = PlayingState.Paused;
        }

        public bool CanPause(TaskItem _task)
        {
            if (_task == null)
                return false;

            return _task.PlayingState == PlayingState.Playing;
        }

        public void Reset()
        {
            InstantiateTimerWithInterval(_interval);
            RaisePropertyChanged("EllapsedPlayingTime");
        }

        private void PlayerTick(object sender, ElapsedEventArgs e)
        {
            var timeSpan = new TimeSpan(0, 0, 1);
            _ellapsedPlayingTime = _ellapsedPlayingTime.Add(timeSpan);
            _playingTask.AddTimeTick(timeSpan);
            RaisePropertyChanged("EllapsedPlayingTime");
        }
    }
}
