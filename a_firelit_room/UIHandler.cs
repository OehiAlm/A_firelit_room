using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace a_firelit_room
{
    internal class UIHandler
    {
        Flow GameFlow;

        internal Window MainWindow { get; private set; }
        internal Panel WindowCanvas { get; private set; }

        internal StringBuilder OutputString { get; private set; }

        internal UIHandler(Flow reference)
        {
            GameFlow = reference;

            MainWindow = Application.Current.MainWindow;
            MainWindow.ResizeMode = ResizeMode.CanMinimize;
            MainWindow.SizeToContent = SizeToContent.WidthAndHeight;

            WindowCanvas = new Canvas();
            MainWindow.Content = WindowCanvas;

            OutputString = new StringBuilder();
        }

        internal Button AddButton(string button_Name, string button_Content, Panel panel, int width = 120, int height = 35)
        {
            Button button = new Button()
            {
                Name = button_Name,
                Content = button_Content,
                Width = width,
                Height = height
            };
            
            panel.Children.Add(button);

            return button;
        }

        internal Panel CreateMainButtonPanel (string Name)
        {
            Panel MainButtonPanel = new StackPanel()
            {
                Name = "MainButtonPanel",
                Height = 500,
                Width = 150,
                Background = Brushes.Black
            };

            MainButtonPanel.Margin = new Thickness(WindowCanvas.Width - MainButtonPanel.Width, 10, 0, 0);
            WindowCanvas.Children.Add(MainButtonPanel);
            return MainButtonPanel;
        }

        internal TextBlock CreateTextOutputElement (string Name)
        {
            TextBlock TextOutputElement = new TextBlock()
            {
                Name = "TextOutputPanel",
                Height = WindowCanvas.Height,
                Width = WindowCanvas.Width,
                Background = Brushes.Black,
                Foreground = Brushes.White,
                Text = OutputString.ToString()
            };

            WindowCanvas.Children.Add(TextOutputElement);
            return TextOutputElement;
        }
        
        internal FrameworkElement GetElementFromWindowCanvas (string Name)
        {
            foreach (FrameworkElement element in WindowCanvas.Children)
            {
                if (element.Name == Name)
                    return element;
            }

            KeyNotFoundException exception = new KeyNotFoundException(Name + " not found in Collection. New Element returned");
            return new FrameworkElement();
        }

        internal Panel AdjustWindowCanvas (int width, int height, SolidColorBrush brush)
        {
            WindowCanvas.Height = width;
            WindowCanvas.Width = height;
            WindowCanvas.Background = brush;
            return WindowCanvas;
        }

        internal string AddTextToOutputBox(string text, TextBlock textOutputElement)
        {
            OutputString.AppendLine(text);
            textOutputElement.Text = OutputString.ToString();
            return textOutputElement.Text;
        }
    }
}
