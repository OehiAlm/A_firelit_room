using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace a_firelit_room
{
    enum EWindowCanvasPanels
    {
        TEXTOUTPUTPANEL = 0,
        MAINBUTTONPANEL = 1,

    }

    internal class UIHandler
    {
        internal StringBuilder OutputString { get; private set; }

        internal UIHandler()
        {
            OutputString = new StringBuilder();
        }

        internal static Button AddButton(string button_Content, Panel panel, int width = 120, int height = 35)
        {
            Button button = new Button()
            {
                Content = button_Content,
                Width = width,
                Height = height
            };
            
            panel.Children.Add(button);

            return button;
        }

        internal static bool RemoveButton()
        {
            //TBD
            return true;
        }

        internal string AddTextToOutputBox(string text, TextBlock textOutputElement)
        {
            OutputString.AppendLine(text);
            textOutputElement.Text = OutputString.ToString();
            return textOutputElement.Text;
        }
    }
}
