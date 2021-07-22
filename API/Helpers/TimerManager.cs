using System;
using System.Threading;

namespace API.Helpers
{
    public class TimerManager
    {
        private Timer _timer;
        private AutoResetEvent _autoResetEvent;
        private Action _action;
        public DateTime TimeStarted {get;set;}


        public TimerManager(Action action)
        {
            _timer = new Timer(Execute,_autoResetEvent,1000,2000);
            _autoResetEvent = new AutoResetEvent(false);
            _action = action;
            TimeStarted = DateTime.Now;
        }

       public void Execute(object stateInfo){
            
            _action();
            
            if((DateTime.Now - TimeStarted).Seconds > 60){
                _timer.Dispose();
            }
       }
        
        
    }
}