using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TaskMaster.Model
{
    public class BgWorkerBuilder
    {
        DoWorkEventHandler _doWork;
        RunWorkerCompletedEventHandler _runWorkerCompleted;

        public BgWorkerBuilder(DoWorkEventHandler doWorkEventHandler)
        {
            _doWork = doWorkEventHandler;
        }

        public BgWorkerBuilder WithRunWorkerCompletedEventHandler(RunWorkerCompletedEventHandler runWorkerCompletedEventHandler)
        {
            _runWorkerCompleted = runWorkerCompletedEventHandler;
            return this;
        }

        public BackgroundWorker Build()
        {
            var bgWorker = new BackgroundWorker();
            bgWorker.DoWork += _doWork;
            bgWorker.RunWorkerCompleted += _runWorkerCompleted;

            return bgWorker;
        }
    }
}
