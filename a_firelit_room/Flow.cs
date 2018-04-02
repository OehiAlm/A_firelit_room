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
        EGameEventNames upcomingGameEventName;

        int totalNumberOfEvents = Enum.GetNames(typeof(EGameEventNames)).Length;
        int upcomingEventIndex = 0;

        Stopwatch gameTimeWatch;
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
        
        public void Update()    //wird immer vor!! dem Rendern abgehakt => Vor dem ersten Rendern nichts! an der UI arbeiten. Diese gibts nämlich bisher noch offiziell gar nicht.
        {
            if (totalNumberOfEvents == upcomingEventIndex)
                return;

            upcomingGameEventName = (EGameEventNames)upcomingEventIndex;

            if (gameTimeWatch.ElapsedMilliseconds >= Events.gameEventDictionary[upcomingGameEventName].eventTime)
            {
                Events.gameEventDictionary[upcomingGameEventName].eventAction.Invoke();
                ++upcomingEventIndex;
            }
        }
    }
}
