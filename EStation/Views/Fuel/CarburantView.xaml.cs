﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace EStation.Views.Fuel
{
    internal partial class CarburantView 
    {
        public CarburantView()
        {
            InitializeComponent();

            Refresh();
        }


       
        internal void Refresh()
        {
           
            new Task(() => Dispatcher.BeginInvoke(new Action(() =>
            {
                _CARBURANTS.ItemsSource = App.EStation.Fuels.GetFuelCards();                
            }))).Start();
        }


        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddFuel(Guid.Empty){ Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh();
        }


        private void CARBURANTS_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void CARBURANTS_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var wind = new AddFuel((Guid) _CARBURANTS.SelectedValue) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh();
        }


    }
}