using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Common
{
    public class TimedTask
    {
        Action todo;
        Timer timer;

        public bool Running { get; private set; }

        public TimedTask(double ms, Action todo)
        {
            timer = new Timer(ms);
            timer.Elapsed += elp;
            timer.AutoReset = true;
            this.todo = todo;
        }

        private void elp(object sender, ElapsedEventArgs e)
        {
            if (Running)
                todo();
        }

        public void Start()
        {
            Running = true;
            timer.Start();
        }

        public void Stop()
        {
            Running = false;
            timer.Stop();
        }

        internal void Set(Action todo)
        {
            this.todo = todo;
        }
    }
}
