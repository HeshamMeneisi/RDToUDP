using System;
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

        public void SetTime(double newms)
        {
            timer.Stop();
            timer.Dispose();
            timer = new Timer(newms);
            timer.Elapsed += elp;
            timer.AutoReset = true;
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

        internal void SetAction(Action todo)
        {
            this.todo = todo;
        }
    }
}
