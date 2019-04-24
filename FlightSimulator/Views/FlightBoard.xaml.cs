﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlightSimulator.Model;
using FlightSimulator.ViewModels;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace FlightSimulator.Views {
    /// <summary>
    /// Interaction logic for MazeBoard.xaml
    /// </summary>
    public partial class FlightBoard : UserControl {

        ObservableDataSource<Point> planeLocations = null;
        FlightBoardViewModel flightBoardViewModel;

        public FlightBoard() {
            InitializeComponent();
            flightBoardViewModel = new FlightBoardViewModel();
            DataContext = flightBoardViewModel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            planeLocations = new ObservableDataSource<Point>();
            // Set identity mapping of point in collection to point on plot
            planeLocations.SetXYMapping(p => p);
            plotter.AddLineGraph(planeLocations, 2, "Route");
            flightBoardViewModel.PropertyChanged += Vm_PropertyChanged;
        }

        private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName.Equals("Lat") || e.PropertyName.Equals("Lon")) {
                Point p1 = new Point(flightBoardViewModel.Lon, flightBoardViewModel.Lat);            // Fill here!
                /*FlightBoardViewModel x = (FlightBoardViewModel)sender;
                Console.WriteLine("sender Lat:" + x.Lat+ " sender Lon:" + x.Lon);
                Console.WriteLine("Lat:" + flightBoardViewModel.Lat + " Lon:" + flightBoardViewModel.Lon);*/
                planeLocations.AppendAsync(Dispatcher, p1);
            }
        }

    }

}

