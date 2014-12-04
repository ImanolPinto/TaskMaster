using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Model
{
    public interface IPlaySessionBuilder
    {
        IPlaySessionBuilder WithDate(DateTime date);
        IPlaySessionBuilder WithPlayedTime(TimeSpan time);
        PlaySession Build();
        IPlaySessionBuilder Reset();
    }
}
