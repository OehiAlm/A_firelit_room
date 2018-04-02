using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace a_firelit_room
{
    enum EGameEventNames
    {
        GAMESTART,
        LOOKAROUND,
        FINDMATCHSTICKS,
        LIGHTMATCHSTICK_OPTION,
    }

    struct GameEvent
    {
        internal EGameEventNames eventName { get; private set; }
        internal double eventTime { get; private set; }
        internal Action eventAction { get; private set; }

        internal GameEvent(EGameEventNames event_name, double event_time_in_miliseconds, Action event_action)
        {
            eventName = event_name;
            eventTime = event_time_in_miliseconds;
            eventAction = event_action;
        }
    }

    internal class GameEventsClass
    {
        internal Flow GameFlow;
        internal UIHandler UI;

        static List<GameEvent> GameEventList;
        internal SortedDictionary<EGameEventNames, GameEvent> gameEventDictionary;

        internal GameEventsClass (Flow reference)
        {
            GameFlow = reference;
            UI = GameFlow.UI;

            GameEventList = new List<GameEvent>();
            gameEventDictionary = new SortedDictionary<EGameEventNames, GameEvent>();

            InitializeGameEvents();
        }

        void InitializeGameEvents()
        {
            GameEventList.Add(new GameEvent(EGameEventNames.GAMESTART, 1500, GameStart));
            GameEventList.Add(new GameEvent(EGameEventNames.LOOKAROUND, 1800, LookAround));
            GameEventList.Add(new GameEvent(EGameEventNames.FINDMATCHSTICKS, 2300, FindMatchSticks));
            GameEventList.Add(new GameEvent(EGameEventNames.LIGHTMATCHSTICK_OPTION, 2500, LightmatchstickButtonAppears));

            foreach (GameEvent gameEvent in GameEventList)
            {
                gameEventDictionary.Add(gameEvent.eventName, gameEvent);
            }
        }

        void GameStart ()
        {
            UI.AdjustWindowCanvas(300, 300, Brushes.Black);
            UI.CreateTextOutputElement("TextOutputPanel");
            UI.AddTextToOutputBox("It's dark...", (TextBlock)UI.GetElementFromWindowCanvas("TextOutputPanel"));
        }

        void LookAround()
        {
            UI.AddTextToOutputBox("You reach out with your hands into the darkness", (TextBlock)UI.GetElementFromWindowCanvas("TextOutputPanel"));
        }

        void FindMatchSticks()
        {
            UI.AddTextToOutputBox("You find a box of Matchsticks!", (TextBlock)UI.GetElementFromWindowCanvas("TextOutputPanel"));
        }

        void LightmatchstickButtonAppears ()
        {
            UI.CreateMainButtonPanel("MainButtonPanel");
            UI.AdjustWindowCanvas(450, 500, Brushes.Black);

            UI.AddButton("KindleButton","Light Matchstick", (Panel)UI.GetElementFromWindowCanvas("MainButtonPanel"));
        }
    }
}
