﻿using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public class PlaySession : ViewModelBase, IEqualityComparer
    {
        private DateTime _date;
        private TimeSpan _playedTime;
        
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (value == _date)
                    return;

                SetAndRiseDate(value);
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
                if (value == _playedTime)
                    return;
                
                SetAndRisePlayedTime(value);
            }
        }

        public PlaySession(DateTime playedDate)
        {
            SetAndRiseDate(playedDate);
        }

        public PlaySession(DateTime playedDate, TimeSpan playedTime)
        {
            SetAndRiseDate(playedDate);
            SetAndRisePlayedTime(playedTime);
        }

        private void SetAndRiseDate(DateTime date)
        {
            _date = date;
            RaisePropertyChanged("Date");
        }

        private void SetAndRisePlayedTime(TimeSpan playedTime)
        {
            _playedTime = playedTime;
            RaisePropertyChanged("PlayedTime");
        }

        #region implement IEqualityComparer


        public bool Equals(object x, object y)
        {
            PlaySession x_ps = x as PlaySession;
            PlaySession y_ps = y as PlaySession;

            if (x_ps == null && y_ps == null)
                return true;

            if (x_ps == null && y_ps != null)
                return false;

            if (y_ps == null && x_ps != null)
                return false;

            return x_ps.Date == y_ps.Date;
        }

        public int GetHashCode(object obj)
        {
            PlaySession obj_ps = obj as PlaySession;

            if (obj_ps == null)
                return -1;

            return obj_ps.Date.GetHashCode();
        }

        #endregion
    }
}
