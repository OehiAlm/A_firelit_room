using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;

namespace a_firelit_room
{
    internal delegate void OutputStringChanges(Run text);
    internal delegate void TextOutputBoxChanges(double lifetime, Run item);

    class LifeTimeObject<T>
    {
        internal T item;
        internal double lifetime;
        
        internal LifeTimeObject(T _item, double life_time)
        {
            item = _item;
            lifetime = life_time;
        }
    }
          
    internal static class TextManager
    {
        const int defaultTextLifetimeInMiliseconds = 3000;

        static internal event OutputStringChanges OnRemoveString;
        static internal event TextOutputBoxChanges OnTextBoxUpdate;

        static List<LifeTimeObject<Run>> StringLifeTimeTracker = new List<LifeTimeObject<Run>>();
        
        private static readonly Dictionary<string, string[]> AllTexts = new Dictionary<string, string[]>()
        {
            {"GAMESTART",               new string[] {"Du schlägst die Augen auf" } },
            {"LOOKAROUND",              new string[] {"Es ist still","Der Boden ist kalt","Du liegst in absoluter Schwärze","Du setzt dich auf" } },
            {"FINDMATCHSTICKS",         new string[] {"Dein Bein berührt etwas","Es ist eine kleine Schachtel","Ein Rascheln verrät dir, dass du Streichhölzer gefunden hast" } },
          //{"LIGHTMATCHSTICK_OPTION",  new string[] {"Braucht kein Text?" } },
            {"LIGHTMATCHSTICK_ACTION",  new string[] {"Du nimmst ein Streichholz aus der Schachtel und schleifst es mit einer Bewegung an der Außenseite der Box","Das Streichholz ist abgebrochen...","Es sind noch 2 Streichhölzer übrig","Du versuchst es erneut","...","Das Streichholz brennt!", "Das letzte Streichholz in der Schachtel wurde anscheinend schon einmal benutzt","Es lässt sich nicht anzünden", "Die Schachtel ist jetzt leer"} },
            {"INSPECTSURROUNDINGAREA",  new string[] {"Der Schein des Streichholzes erleuchtet die unmittelbare Umgebung","Du siehst einen kleinen Haufen Hölzer","..." } },
            {"SEARCHSURROUNDINGAREA",   new string[] {"...", "Das Streichholz ist ausgegangen", "...", Environment.NewLine,"Du suchst mit den Händen nach dem Holz auf dem Boden", "Du ertastest einen kleinen Spalt", "...Er ist warm" } },

        };

        /*/////////////////////////////////////////////
                        GetText methods
        ////////////////////////////////////////////*/

        internal static string[] GetTexts(EUIEventNames identifier, byte lines_of_text = 0, byte starting_from_index = 0)
        {
            if (lines_of_text == 0)
            {
                AllTexts.TryGetValue(identifier.ToString(), out string[] temp_value);
                string[] value = new string[temp_value.Length-starting_from_index];
                
                for (int i = starting_from_index; i < value.Length + starting_from_index; ++i)
                {
                    value[i - starting_from_index] = temp_value[i];
                }

                return value;
            }
            else
            {
                AllTexts.TryGetValue(identifier.ToString(),out string[] temp_value);
                string[] value = new string[lines_of_text];

                for (int i = starting_from_index; i < starting_from_index + lines_of_text; ++i)
                {
                    value[i-starting_from_index] = temp_value[i];
                }

                return value;
            }
        }

        internal static string[] GetTexts(ETimedGameEventNames identifier)
        {
            AllTexts.TryGetValue(identifier.ToString(), out string[] value);
            return value;
        }

        /*/////////////////////////////////////////////
                    LifeTimeTracker methods
        ////////////////////////////////////////////*/

        internal static void TrackLifeTimeObject(Run run)
        {
            StringLifeTimeTracker.Add(new LifeTimeObject<Run>(run, defaultTextLifetimeInMiliseconds));
        }

        static void RemoveStringFromLifeTimeTracker(byte index)
        {
            StringLifeTimeTracker.RemoveAt(index);
        }

        internal static void UpdateStringLifeTimes()
        {
            byte index = 0;

            while (index < StringLifeTimeTracker.Count)
            {
                if (StringLifeTimeTracker[index].lifetime > 0)
                {
                    StringLifeTimeTracker[index].lifetime -= 1;
                    OnTextBoxUpdate.Invoke(StringLifeTimeTracker[index].lifetime,StringLifeTimeTracker[index].item);
                    ++index;
                }
                else
                {
                    OnRemoveString.Invoke(StringLifeTimeTracker[index].item);
                    RemoveStringFromLifeTimeTracker(index);
                }
            }
        }
    }
}
