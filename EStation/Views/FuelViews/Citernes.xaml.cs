﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EStation.Views.Fuel
{
    
    internal partial class Citernes 
    {

        public event EventHandler CiterneSelectionChanged;

        public Citernes()
        {
            InitializeComponent();

            Refresh();
        }


        internal void Refresh()
            => new Task(() => Dispatcher.BeginInvoke(new Action(()
                => _CITERNES.ItemsSource = App.Store.Citernes.GetCiternesCards()))).Start();


        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wind = new AddCiterne(Guid.Empty) { Owner = Window.GetWindow(this) };
            wind.ShowDialog();
            Refresh();
        }


        protected virtual void OnCiterneSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs) 
            => CiterneSelectionChanged?.Invoke(_CITERNES.SelectedValue, EventArgs.Empty);


    }
}
