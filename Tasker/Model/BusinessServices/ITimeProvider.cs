﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public interface ITimeProvider
    {
        DateTime Now();
    }
}
