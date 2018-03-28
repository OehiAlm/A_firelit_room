using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace a_firelit_room
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Flow Game;
        
        public delegate void GameDelegate(object sender, EventArgs e);  //Delegate, was benötigt wird, um sich auf den Render Thread zu registrieren

        public MainWindow()
        {
            InitializeComponent();

            CompositionTarget.Rendering += Update;    //Registrieren des vorher erstellten Delegates auf den WPF Render Thread
            Game = new Flow();
        }

        void Update(object sender, EventArgs e)
        {
            Game.Update();  //Weitergeben der Kontrolle, über was jeden Tick gemacht werden muss an die eigene Game Flow Klasse
        }
    }
}