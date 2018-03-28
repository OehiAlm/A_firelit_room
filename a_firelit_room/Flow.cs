using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace a_firelit_room
{
    internal struct TimePassed
    {
        internal int ticks;
        internal int seconds { get; private set; }
        int minutes;

        internal void UpdateTime(int addedTicks)
        {
            ticks += addedTicks;

            if (ticks == 60)
            {
                seconds += 1;
                ticks = 0;
            }

            if (seconds == 60)
            {
                minutes += 1;
                seconds = 0;
            }
        }
    }

    public class Flow
    {
        TimePassed time;

        UIHandler UI;

        Window MainWindow;
        Panel WindowCanvas;

        public Flow()
        {
            MainWindow = Application.Current.MainWindow;
            MainWindow.ResizeMode = ResizeMode.CanMinimize;
            MainWindow.SizeToContent = SizeToContent.WidthAndHeight;

            WindowCanvas = new Canvas();
            MainWindow.Content = WindowCanvas;

            UI = new UIHandler();
        }      
        
        public void Update()    //wird immer vor!! dem Rendern abgehakt => Vor dem ersten Rendern nichts! an der UI arbeiten. Diese gibts nämlich bisher noch offiziell gar nicht.
        {
            switch (time.ticks)
            {

                case (int)EGameEvents.GAMESTART:
                {
                    WindowCanvas.Height = 300;
                    WindowCanvas.Width = 300;
                    WindowCanvas.Background = Brushes.Black;

                    TextBlock TextOutputPanel = new TextBlock()
                    {
                        Name = "TextOutputPanel",
                        Height = WindowCanvas.Height,
                        Width = WindowCanvas.Width,
                        Background = Brushes.Black,
                        Foreground = Brushes.White,
                        Text = UI.OutputString.ToString()
                    };
                    
                    WindowCanvas.Children.Add(TextOutputPanel);
                    UI.AddTextToOutputBox("It's dark...", TextOutputPanel);
                    
                    break;
                }

            case (int)EGameEvents.LOOKAROUND:
                {
                    UI.AddTextToOutputBox("You reach out with your hands into the darkness", (TextBlock)WindowCanvas.Children[(int)EWindowCanvasPanels.TEXTOUTPUTPANEL]); //well, that works... kinda
                    break;
                }

            case (int)EGameEvents.FINDMATCHSTICKS:
                {
                    UI.AddTextToOutputBox("You find a box of Matchsticks!", (TextBlock)WindowCanvas.Children[(int)EWindowCanvasPanels.TEXTOUTPUTPANEL]); //well, that works... kinda
                    break;
                }

            case (int)EGameEvents.LIGHTMATCHSTICK:
                {
                    Panel MainButtonPanel = new StackPanel()
                    {
                        Name = "MainButtonPanel",
                        Height = 500,
                        Width = 150,
                        Background = Brushes.Black
                    };

                    WindowCanvas.Width += MainButtonPanel.Width;
                    MainButtonPanel.Margin = new Thickness(WindowCanvas.Width - MainButtonPanel.Width, 10, 0, 0);
                    WindowCanvas.Children.Add(MainButtonPanel);

                    UIHandler.AddButton("Light Matchstick", (Panel)WindowCanvas.Children[(int)EWindowCanvasPanels.MAINBUTTONPANEL]);
                    break;
                }
            }

            time.ticks++;

        }

    }
}
