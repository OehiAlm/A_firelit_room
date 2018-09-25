using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace a_firelit_room
{
    internal class UIHandler
    {
        Flow GameFlow;
        Dictionary<Button, SByte> ButtonClicks;

        internal Window MainWindow { get; private set; }
        internal Panel WindowCanvas { get; private set; }

        /////////     Constructor     /////////

        internal UIHandler(Flow reference)
        {
            GameFlow = reference;
            ButtonClicks = new Dictionary<Button, sbyte>();

            MainWindow = Application.Current.MainWindow;
            MainWindow.ResizeMode = ResizeMode.CanMinimize;
            MainWindow.SizeToContent = SizeToContent.WidthAndHeight;

            WindowCanvas = new Canvas();
            MainWindow.Content = WindowCanvas;

            TextManager.OnRemoveString += RemoveTextFromOutputBox;
            TextManager.OnTextBoxUpdate += UpdateRunOpacity;
        }

        /*/////////////////////////////////////////////
                        Get methods
        ////////////////////////////////////////////*/

        //use for storing a reference in code someplace, to later use for browsing through the Children Elements faster
        internal byte GetIndexFromElementInWindowCanvas (string Name)
        {
            foreach (FrameworkElement element in WindowCanvas.Children)     
            {
                if (element.Name == Name)
                    return (byte)WindowCanvas.Children.IndexOf(element);
            }

            throw new KeyNotFoundException("Framework Element Name: '" + Name + "' not found in WindowCanvas Children Collection! Are you sure its in there?");
        }
            
        internal UIElement GetElementFromWindowCanvas(byte index)
        {
            return WindowCanvas.Children[index];    //fast, but only usable, if you know the index (and it never changes!!)
        }

        internal FrameworkElement GetElementFromWindowCanvas(string Name)
        {
            foreach (FrameworkElement element in WindowCanvas.Children)     //only good, if the number of Children stays small (which it should - 4.4.18)
            {
                if (element.Name == Name)
                    return element;
            }

            throw new KeyNotFoundException("Framework Element Name: '" + Name + "' not found in WindowCanvas Children Collection! Are you sure its in there?");
        }

        /*/////////////////////////////////////////////
                    Create & Add UI Elements
        ////////////////////////////////////////////*/

        SolidColorBrush CreateSolidColorBrush(byte opacity = 255, byte red = 255, byte green = 255, byte blue = 255)
        {
            return new SolidColorBrush(Color.FromArgb(opacity,red,green,blue));
        }

        internal Panel CreateMainButtonPanel ()
        {
            Panel MainButtonPanel = new StackPanel()
            {
                Name            = "MainButtonPanel",
                Height          = 500,
                Width           = 150,
                Background      = Brushes.Black,
            };

            MainButtonPanel.Margin = new Thickness(WindowCanvas.Width - MainButtonPanel.Width, 10, 0, 0);
            WindowCanvas.Children.Add(MainButtonPanel);
            return MainButtonPanel;
        }

        internal TextBlock CreateTextOutputPanel ()
        {
            TextBlock TextOutputPanel= new TextBlock()
            {
                Name            = "TextOutputPanel",
                Height          = WindowCanvas.Height,
                Width           = WindowCanvas.Width,
                Background      = Brushes.Black,
                Foreground      = Brushes.White,
                TextTrimming    = TextTrimming.WordEllipsis,
                TextWrapping    = TextWrapping.Wrap,
            };

            WindowCanvas.Children.Add(TextOutputPanel);
            return TextOutputPanel;
        }

        internal Button AddButton(string button_Name, string button_Content, Panel panel, Action<object, EventArgs> click_event, int width = 120, int height = 35)
        {
            Button button = new Button()
            {
                Name    = button_Name,
                Content = button_Content,
                Width   = width,
                Height  = height,
            };

            button.Click += new RoutedEventHandler(click_event);
            ButtonClicks.Add(button, 0);
            panel.Children.Add(button);
            
            return button;
        }

        /*/////////////////////////////////////////////
                        Add Text methods
        ////////////////////////////////////////////*/

        internal string AddTextToOutputBox(params string[] texts)
        {
            TextBlock TextOutputElement = (TextBlock)GetElementFromWindowCanvas("TextOutputPanel");

            for (int i = texts.Length - 1; i >= 0; --i)
            {
                Run run = new Run()
                {
                    Foreground = CreateSolidColorBrush(),
                    Text = texts[i] + Environment.NewLine,
                };

                if (i == 0)
                    run.Text = run.Text.Insert(0,Environment.NewLine);

                if (TextOutputElement.Inlines.Count != 0)
                    TextOutputElement.Inlines.InsertBefore(TextOutputElement.Inlines.FirstInline, run);
                else
                    TextOutputElement.Inlines.Add(run);

                TextManager.TrackLifeTimeObject(run);
            }
            
            return TextOutputElement.Text;
        }

        /*/////////////////////////////////////////////
                    Update Text methods
        ////////////////////////////////////////////*/

        void UpdateRunOpacity(double lifetime, Run item)
        {
            if (lifetime < 500)
                item.Foreground.Opacity = lifetime / 250;
        }

        /*/////////////////////////////////////////////
                     Remove Text methods
        ////////////////////////////////////////////*/

        internal void RemoveTextFromOutputBox(Run run_to_be_removed)
        {
            TextBlock TextOutputElement = (TextBlock)GetElementFromWindowCanvas("TextOutputPanel");
            TextOutputElement.Inlines.Remove(run_to_be_removed);
        }

        /*/////////////////////////////////////////////
                        Adjust UI Elements
        ////////////////////////////////////////////*/

        internal Button GetButtonFrom(string button_name, Panel panel)
        {
            foreach (FrameworkElement element in panel.Children)
            {
                if (button_name == element.Name)
                    return (Button)element;
            }

            throw new KeyNotFoundException("Button called: '" + button_name + "' not found in '" + panel.Name + "' Children!");
        }

        internal Panel AdjustWindowCanvasSize (double width = -1, double height = -1)
        {
            if (width >= 0)
                WindowCanvas.Width = width;

            if (height >= 0)
                WindowCanvas.Height = height;

            return WindowCanvas;
        }

        internal Panel SetWindowCanvasColor(SolidColorBrush brush)
        {
            WindowCanvas.Background = brush;
            return WindowCanvas;
        }

        internal TextBlock AdjustTextOutputElementSize (double width = Double.NaN, double height = Double.NaN)
        {
            TextBlock TextOutputElement = (TextBlock)GetElementFromWindowCanvas("TextOutputPanel");

            if(width != Double.NaN)
                TextOutputElement.Width = width;

            if (height != Double.NaN)
                TextOutputElement.Height = height;

            return TextOutputElement;
        }

        internal TextBlock SetTextOutputBackgroundColor(SolidColorBrush brush)
        {
            TextBlock TextOutputElement = (TextBlock)GetElementFromWindowCanvas("TextOutputPanel");
            TextOutputElement.Background = brush;
            return TextOutputElement;
        }

        /*/////////////////////////////////////////////
                        Event Actions
        ////////////////////////////////////////////*/

        internal void LookAroundButtonClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            ButtonClicks[button]++;

            switch (ButtonClicks[button])
            {
            case 1:
                {
                    AddTextToOutputBox("Du siehst dich um");
                    GameFlow.Events.AdvanceToNextEventPhase();
                    break;
                }

            case 2:
                {
                    AddTextToOutputBox("Du siehst dich um");
                    Button Kindlebutton = GetButtonFrom("KindleButton", (Panel)GetElementFromWindowCanvas("MainButtonPanel"));
                    Kindlebutton.Content = "Hölzer anzünden";
                    Kindlebutton.IsEnabled = true;

                    GameFlow.Events.AdvanceToNextEventPhase();
                    break;
                }
            }
            button.IsEnabled = false;
        }


        internal void KindleButtonClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            ButtonClicks[button]++;

            switch (ButtonClicks[button])
            {
            case 1:
                {
                    AddTextToOutputBox(TextManager.GetTexts(EUIEventNames.LIGHTMATCHSTICK_ACTION, lines_of_text: 3));
                    break;
                }

            case 2:
                {

                    byte MainButtonPanelIndex = GetIndexFromElementInWindowCanvas("MainButtonPanel");

                    AddTextToOutputBox(TextManager.GetTexts(EUIEventNames.LIGHTMATCHSTICK_ACTION, 3, 3));
                    GetButtonFrom("LookAroundButton", (Panel)GetElementFromWindowCanvas(MainButtonPanelIndex)).IsEnabled = true;
                    GetButtonFrom("KindleButton", (Panel)GetElementFromWindowCanvas(MainButtonPanelIndex)).IsEnabled = false;
                    break;
                }

            case 3:
                {
                    AddTextToOutputBox(TextManager.GetTexts(EUIEventNames.LIGHTMATCHSTICK_ACTION, starting_from_index: 6));
                    button.IsEnabled = false;
                    break;
                }
            }
        }
    }
}
