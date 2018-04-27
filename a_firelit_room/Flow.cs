using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace a_firelit_room
{
    public class Flow
    {
        internal Stopwatch gameTimeWatch;
      //TimeSpan gameTime; --> needed for a deltaTime at some point?

        internal GameEventsClass Events { get; private set; }
        internal UIHandler UI { get; private set; }

        public Flow()
        {
            UI = new UIHandler(this);
            Events = new GameEventsClass(this);

            //gameTime = new TimeSpan();  --> only needed if a deltaTime is needed at some point
            gameTimeWatch = new Stopwatch();
            gameTimeWatch.Start();
        }      
        
        public void Update()    //wird vor dem Rendern der UI (WPF-intern) abgehakt
        {
            Events.CheckForTimedEvents();
            TextManager.UpdateStringLifeTimes();
        }
    }
}
