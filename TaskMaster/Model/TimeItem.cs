using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public class TimeItem
    {
        private string _description;
        private string _tag;
        private TimeSpan _time;

        public TimeSpan Time
        {
            get
            {
                return _time;
            }
        }

        public string Tag
        {
            get
            {
                return _tag;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public TimeItem(TimeSpan time, string tag, string description)
        {
            _time = time;
            _tag = tag;
            _description = description;
        }
    }
}
