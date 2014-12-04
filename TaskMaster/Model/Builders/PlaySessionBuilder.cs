using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Model
{
    public class PlaySessionBuilder : IPlaySessionBuilder
    {
        DateTime _date;
        TimeSpan _playedTime;

        public IPlaySessionBuilder WithDate(DateTime date)
        {
            _date = date;
            return this;
        }

        public IPlaySessionBuilder WithPlayedTime(TimeSpan time)
        {
            _playedTime = time;
            return this;
        }

        public PlaySession Build()
        {
            return new PlaySession(_date, _playedTime);
        }

        public IPlaySessionBuilder Reset()
        {
            return new PlaySessionBuilder();
        }
    }
}
