using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace a_firelit_room
{
    partial class GameEventsClass
    {
        byte eventPhase;
        byte numberOfTimedEventsInCurrentPhase;
        byte upcomingEventIndex;
        
        internal Flow GameFlow;
        internal UIHandler UI;
      
        internal GameEventsClass (Flow reference)
        {
            GameFlow = reference;
            UI = GameFlow.UI;

            UIGameEventList = new List<UIGameEvent>();
            UIEventDictionary = new Dictionary<EUIEventNames,UIGameEvent>();
            
            CreateGameEvents();

            eventPhase = 0;
            numberOfTimedEventsInCurrentPhase = (byte)TimedGameEventsArray[eventPhase].Length;
            upcomingEventIndex = 0;

            eventTimeWatch = new System.Diagnostics.Stopwatch();
            eventTimeWatch.Start();
        }

        internal void CheckForTimedEvents()
        {
            if (numberOfTimedEventsInCurrentPhase == upcomingEventIndex)
                return;
            
            if (eventTimeWatch.ElapsedMilliseconds >= TimedGameEventsArray[eventPhase][upcomingEventIndex].eventTime)
            {
                TimedGameEventsArray[eventPhase][upcomingEventIndex].eventAction.Invoke();
                ++upcomingEventIndex;
            }
        }

        internal void AdvanceToNextEventPhase ()
        {
            upcomingEventIndex = 0;
            eventPhase++;
            numberOfTimedEventsInCurrentPhase = (byte)TimedGameEventsArray[eventPhase].Length;
            eventTimeWatch.Restart();
        }

        /*/////////////////////////////////////////////
                    Game Event methods
        ////////////////////////////////////////////*/

        void GameStart ()
        {
            UI.AdjustWindowCanvasSize(500, 250);
            UI.SetWindowCanvasColor(Brushes.Black);
            UI.CreateTextOutputElement("TextOutputPanel");
            UI.AddTextToOutputBox(TextManager.GetTexts(ETimedGameEventNames.GAMESTART));
        }

        void LookAround ()
        {
            UI.AddTextToOutputBox(TextManager.GetTexts(ETimedGameEventNames.LOOKAROUND));
        }

        void FindMatchSticks ()
        {
            UI.AddTextToOutputBox(TextManager.GetTexts(ETimedGameEventNames.FINDMATCHSTICKS));
        }

        void LightmatchstickButtonAppears ()
        {
            Panel tempWindowCanvas = UI.AdjustWindowCanvasSize(500, 500);
            Panel tempMainButtonPanel = UI.CreateMainButtonPanel("MainButtonPanel");
            UI.AdjustTextOutputElementSize(tempWindowCanvas.Width - tempMainButtonPanel.Width, tempWindowCanvas.Height);
            UI.AddButton("KindleButton","Streichholz anstecken", (Panel)UI.GetElementFromWindowCanvas("MainButtonPanel"), UI.KindleButtonClick);
        }

        void InspectSurroundingArea()
        {
            UI.AddTextToOutputBox(TextManager.GetTexts(ETimedGameEventNames.INSPECTSURROUNDINGAREA));
        }

        void SearchSurroundingArea()
        {
            UI.AddTextToOutputBox(TextManager.GetTexts(ETimedGameEventNames.SEARCHSURROUNDINGAREA));
        }
    }
}
