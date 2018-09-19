using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace a_firelit_room
{
    enum ETimedGameEventNames
    {
        GAMESTART,
        LOOKAROUND,
        LOOKAROUND_OPTION,
        FINDMATCHSTICKS,
        LIGHTMATCHSTICK_OPTION,
        INSPECTSURROUNDINGAREA,
        SEARCHSURROUNDINGAREA,

    }

    enum EUIEventNames
    {
        LIGHTMATCHSTICK_ACTION,
        PICKUPSTICK,
    }

    struct UIGameEvent
    {
        internal EUIEventNames UIEventName { get; private set; }
        internal Action<object,EventArgs> UIEventAction { get; private set; }

        internal UIGameEvent(EUIEventNames UI_event_name,Action<object,EventArgs> event_action)
        {
            UIEventName = UI_event_name;
            UIEventAction = event_action;
        }
    }

    struct TimedGameEvent
    {
        internal ETimedGameEventNames timedEventName { get; private set; }
        internal Action eventAction { get; private set; }
        internal long eventTime { get; private set; }

        internal TimedGameEvent(ETimedGameEventNames timed_event_name,Action event_action,long event_time_in_miliseconds)
        {
            timedEventName = timed_event_name;
            eventAction = event_action;
            eventTime = event_time_in_miliseconds;
        }
    }

    partial class GameEventsClass
    {
        internal Stopwatch eventTimeWatch;

        static TimedGameEvent[][] TimedGameEventsArray;      
        static List<UIGameEvent> UIGameEventList;

        internal Dictionary<EUIEventNames,UIGameEvent> UIEventDictionary;
        
        void CreateGameEvents()
        {
            /*UIGameEventList.Add(new UIGameEvent(EUIEventNames.LIGHTMATCHSTICK_ACTION,       UI.KindleButtonClick));
            UIGameEventList.Add(new UIGameEvent(EUIEventNames.PICKUPSTICK,                  UI.PickupStick));
            */
            foreach (UIGameEvent gameEvent in UIGameEventList)
            {
                UIEventDictionary.Add(gameEvent.UIEventName,gameEvent);
            }

            TimedGameEvent[][] timedGameEventsArray =           // All values in miliseconds -> need to be divided by 1000 to get to "seconds"
            {
                new [] 
                {
                    new TimedGameEvent(ETimedGameEventNames.GAMESTART,              GameStart,                      0000),
                    new TimedGameEvent(ETimedGameEventNames.LOOKAROUND,             LookAround,                     3500),
                    new TimedGameEvent(ETimedGameEventNames.LOOKAROUND_OPTION, LookAroundButtonAppears, 40000),
                    new TimedGameEvent(ETimedGameEventNames.FINDMATCHSTICKS,        FindMatchSticks,                9500),
                    new TimedGameEvent(ETimedGameEventNames.LIGHTMATCHSTICK_OPTION, LightmatchstickButtonAppears,   9500),
                },

                new []
                {
                    new TimedGameEvent(ETimedGameEventNames.INSPECTSURROUNDINGAREA, InspectSurroundingArea,         2000),
                    new TimedGameEvent(ETimedGameEventNames.SEARCHSURROUNDINGAREA,  SearchSurroundingArea,          6000),

                },
            };
            TimedGameEventsArray = timedGameEventsArray;
        }


    }
}
