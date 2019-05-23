using System;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Classes;
using MessageBox = System.Windows.MessageBox; // car sinon prend celui de Forms 
// pour nouvelle classe permettant de changer couleur 
//using System.Globalization;
//using System.Windows.Data;
//using System.Windows.Media; 

namespace FlightAndAirport
{
    /// <summary>
    /// Logique d'interaction pour Airport.xaml
    /// </summary>
    public partial class Airport : Window
    {
        private ClassUser MyUtil { get; set; } 
        private ClassRegistry MyManager { get; set; } 
        private MainWindow MyMain { get; set; } 
        private ObservableCollection<ClassScheduledFlight> MyScheduledFlights { get; set; } 
        private ObservableCollection<ClassScheduledFlight> MySimFlights { get; set; } 
        public int Vitesse { get; set; }
        public bool Pause { get; set; } 
        public Timer MyTimer { get; set; } 

        public Airport(MainWindow main)
        {
            MyScheduledFlights = new ObservableCollection<ClassScheduledFlight>();
            MySimFlights = new ObservableCollection<ClassScheduledFlight>();
            MyMain = main;
            MyManager = main.Manager;
            MyUtil = new ClassUser();
            MyUtil = main.Uti;

            InitializeComponent();
            Stack_ScheduledFlights.Visibility = Visibility.Visible; // on s'assure d'etre sur le bon affichage
            Stack_sim.Visibility = Visibility.Collapsed; 
            DataContext = MyUtil;
            Datagrid_Flights.DataContext = MyScheduledFlights;

            ObservableCollection<ClassScheduledFlight> flight = new ObservableCollection<ClassScheduledFlight>(); 
            try
            {
                flight = ClassSerializable.LoadFromXMLFormat<ClassScheduledFlight>(MyManager.WorkPlace + "/" + MyUtil.Code + "_FlightS.xml");
                foreach (ClassScheduledFlight item in flight) MyScheduledFlights.Add(item);
                Sort();

                List<ClassScheduledFlight> temp = new List<ClassScheduledFlight>(); 
                foreach(ClassScheduledFlight item in MyScheduledFlights)
                {
                    if (item.Flight.Source == null) item.Flight.Source = MyUtil.Code;
                    else if (!item.Flight.Source.Equals(MyUtil.Code)) temp.Add(item); 
                }

                foreach (ClassScheduledFlight item in temp) MyScheduledFlights.Remove(item);
                if (MyScheduledFlights.Count == 0) MessageBox.Show("No scheduled flight found for this company !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Datagrid_Flights.DataContext = MyScheduledFlights; 
            }
            catch (Exception)
            {
                MessageBox.Show("File doesn't exists - File creation !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ClassSerializable.SaveAsXMLFormat(MyScheduledFlights, MyManager.WorkPlace + "/" + MyUtil.Code + "_FlightS.xml");
            }
            
            Closing += Closing_Airport;
            Pause = false;
            ButtonStart.IsEnabled = true;
            ButtonStop.IsEnabled = false;
            ButtonPause.IsEnabled = false;
        }

        #region BUTTONS_METHODES
        #region FILE_MENU 
        private void Save_Flight_Click(object sender, RoutedEventArgs e)
        {
            if (MyScheduledFlights.Count == 0) MessageBox.Show("No generic flight to save !", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            else
            {
                #region Application.Current.Shutdown();
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = MyUtil.Code + "_FlightS"; // Default file name
                dlg.DefaultExt = ".xml"; // Default file extension
                dlg.Filter = "Text documents (.xml)|*.xml"; // Filter files by extension
                dlg.InitialDirectory = MyManager.WorkPlace;
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true) ClassSerializable.SaveAsXMLFormat(MyScheduledFlights, dlg.FileName);
                #endregion
            }
        }

        private void Load_Flight_Click(object sender, RoutedEventArgs e)
        {
            #region OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = MyUtil.Code + "_FlightS"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "Text documents (.xml)|*.xml"; // Filter files by extension
            dlg.InitialDirectory = MyManager.WorkPlace;
            Nullable<bool> result = dlg.ShowDialog();
            ObservableCollection<ClassScheduledFlight> vol = new ObservableCollection<ClassScheduledFlight>();
            if (result == true) vol = ClassSerializable.LoadFromXMLFormat<ClassScheduledFlight>(dlg.FileName);
            foreach (ClassScheduledFlight t in vol) MyScheduledFlights.Add(t);
            Sort();
            List<ClassScheduledFlight> tmp = new List<ClassScheduledFlight>();
            foreach (ClassScheduledFlight item in MyScheduledFlights)
            {
                if (item.Flight.Source == null) item.Flight.Source = MyUtil.Code;
                else if (!item.Flight.Source.Equals(MyUtil.Code)) tmp.Add(item);
            }
            foreach (ClassScheduledFlight item in tmp) MyScheduledFlights.Remove(item);
            if (MyScheduledFlights.Count == 0) MessageBox.Show("No scheduled flight found for this company !", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            Datagrid_Flights.DataContext = MyScheduledFlights;
            #endregion 
        }

        private void Disconnection_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
        #endregion

        #region OPTION_MENU 
        private void Option_Click(object sender, RoutedEventArgs e)
        {
            MyManager.Option(); 
        }
        #endregion

        #region BUTTONS 
        private void ButtonFaster_Click(object sender, RoutedEventArgs e)
        {
            if (Vitesse == 100) MessageBox.Show("Speed too high !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); // plus vitesse est basse plus ca va vite 
            else Vitesse -= 100;
        }

        private void ButtonSlower_Click(object sender, RoutedEventArgs e)
        {
            if (Vitesse > 1000) MessageBox.Show("Speed too low !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); // plus vitesse est haute plus ca va lentement 
            else Vitesse += 100;
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (MyScheduledFlights.Count == 0) MessageBox.Show("No flight !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (Datepicker_flight.Text.Length == 0) MessageBox.Show("No date !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                //Je remet tout a SCHEDULED
                foreach (ClassScheduledFlight item in MyScheduledFlights) item.Flight.Status = ClassGenericFlight.SCHEDULED;
                //J'ajoute Les vols programmé
                foreach (ClassScheduledFlight item in MyScheduledFlights) if (item.Date.Equals(Convert.ToDateTime(Datepicker_flight.Text))) MySimFlights.Add(item);

                if (MySimFlights.Count == 0) MessageBox.Show("No flight for this date !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    ButtonStart.IsEnabled = false;
                    ButtonPause.IsEnabled = true;
                    ButtonStop.IsEnabled = true;
                    ButtonFaster.IsEnabled = true;
                    ButtonSlower.IsEnabled = true;
                    Vitesse = 700;
                    Stack_ScheduledFlights.Visibility = Visibility.Collapsed;
                    Stack_sim.Visibility = Visibility.Visible;
                    var t = new ObservableCollection<ClassScheduledFlight>();

                    SortSim();
                    DateTime tmp = Convert.ToDateTime(Datepicker_flight.Text);
                    DateTime Aujourdhui = new DateTime(tmp.Year, tmp.Month, tmp.Day, 0, 0, 0, DateTimeKind.Local);
                    MyTimer = new System.Windows.Forms.Timer();
                    MyTimer.Stop();
                    MyTimer.Tick += (Sender, eventArgs) =>
                    {
                        if (!Pause)
                        {
                            //Définir le status
                            foreach (ClassScheduledFlight item in MySimFlights) item.Flight.UpdateFlightStatus(Aujourdhui);
                            t.Clear();
                            foreach (ClassScheduledFlight item in MySimFlights) if (item.Flight.Status.Equals(ClassGenericFlight.FLYING)) t.Add(item);
                            foreach (ClassScheduledFlight item in t) MySimFlights.Remove(item);
                            Datagrid_sim.DataContext = MySimFlights;
                            Datagrid_sim.Items.Refresh();
                            Label_time.Content = String.Format("{0:d/MM/yyyy  HH:mm:ss}", Aujourdhui);
                            Aujourdhui = Aujourdhui.AddMinutes(5);
                            MyTimer.Interval = Vitesse;
                            if (MySimFlights.Count == 0)
                            {
                                ButtonFaster.IsEnabled = false;
                                ButtonSlower.IsEnabled = false;
                                ButtonStop.IsEnabled = true;
                                Label_time.Content = "Simulation ended";
                                t.Clear();
                                MySimFlights.Clear();
                                ButtonStart.IsEnabled = true;
                                ButtonPause.IsEnabled = false;
                                MyTimer.Stop();
                            }
                        }
                    };
                    MyTimer.Interval = Vitesse;
                    MyTimer.Start();
                }
            }
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            if (Pause)
            {
                Pause = false;
                ButtonPause.Content = "Pause";
            }
            else if (!Pause)
            {
                Pause = true;
                ButtonPause.Content = "Continue";
            }
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            if (MyTimer != null) MyTimer.Stop();
            Label_time.Content = "";
            ButtonStart.IsEnabled = true;
            ButtonStop.IsEnabled = false;
            ButtonPause.IsEnabled = false;
            MySimFlights.Clear();
            Stack_sim.Visibility = Visibility.Collapsed;
            Stack_ScheduledFlights.Visibility = Visibility.Visible;
        }
        #endregion 

        #endregion

        #region METHODES 
        private void Closing_Airport(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult i = MessageBox.Show(this, "Do you want to save ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning); 
            if (MessageBoxResult.Yes == i)
            {
                ClassSerializable.SaveAsXMLFormat(MyScheduledFlights, MyManager.WorkPlace + "/" + MyUtil.Code + "_FlightS.xml");
            }
            MainWindow tmp = new MainWindow();
            tmp.Show();
        }
        public void Sort()
        {
            List<ClassScheduledFlight> sortableList = new List<ClassScheduledFlight>(MyScheduledFlights);
            sortableList.Sort();

            for (int i = 0; i < sortableList.Count; i++)
                MyScheduledFlights.Move(MyScheduledFlights.IndexOf(sortableList[i]), i);
        }
        public void SortSim()
        {
            List<ClassScheduledFlight> sortableList = new List<ClassScheduledFlight>(MySimFlights);
            sortableList.Sort();

            for (int i = 0; i < sortableList.Count; i++)
            {
                MySimFlights.Move(MySimFlights.IndexOf(sortableList[i]), i);
            }
        }
        #endregion
    }
}
