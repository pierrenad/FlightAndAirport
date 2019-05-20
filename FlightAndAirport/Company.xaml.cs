using System;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using Classes;

namespace FlightAndAirport
{
    /// <summary>
    /// Logique d'interaction pour Company.xaml
    /// </summary>
    public partial class Company : Window
    {
        public ClassUser MyUtil { get; set; } 
        public MainWindow MyMain { get; set; } 
        public ClassRegistry MyManager { get; set; } 
        public ObservableCollection<ClassGenericFlight> MyGenericFlights { get; set; } 
        public ObservableCollection<ClassScheduledFlight> MyScheduledFlights { get; set; } 

        public Company(MainWindow Main)
        {
            MyGenericFlights = new ObservableCollection<ClassGenericFlight>();
            MyScheduledFlights = new ObservableCollection<ClassScheduledFlight>();
            MyMain = Main;
            MyManager = Main.Manager;
            MyUtil = new ClassUser();
            MyUtil = Main.Uti; 
            InitializeComponent();

            DataContext = MyUtil; 

            try // on va récupérer le fichier enregistré si y en a un, sinon on le crée (que vols génériques) 
            {
                MyGenericFlights = ClassSerializable.LoadFromXMLFormat<ClassGenericFlight>(MyManager.WorkPlace + "/" + MyUtil.Code + "_Flight.xml");
                Sort();

                List<ClassGenericFlight> temp = new List<ClassGenericFlight>(); 
                foreach(ClassGenericFlight _item in MyGenericFlights) // ajout dans la liste  
                {
                    if (_item.Code == null) _item.Code = MyUtil.Code;
                    else if (!_item.Code.Equals(MyUtil.Code)) temp.Add(_item); 
                }
                foreach (ClassGenericFlight item in temp) MyGenericFlights.Remove(item);
                if (MyGenericFlights.Count == 0) MessageBox.Show("No generic flight found for this company !", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
            catch(Exception)
            {
                MessageBox.Show("File doesn't exists - File creation !", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
                ClassSerializable.SaveAsXMLFormat(MyGenericFlights, MyManager.WorkPlace + "/" + MyUtil.Code + "_Flight.xml"); 
            }
            Datagrid_Generic.DataContext = MyGenericFlights;
            Datagrid_Scheduled.DataContext = MyScheduledFlights;
            Closing += Closing_Company; 
        }

        #region BUTTONS_METHODES 
        #region FILE_MENU 
        private void Save_Gen_Click(object sender, RoutedEventArgs e)
        {
            if (MyGenericFlights.Count == 0) MessageBox.Show("No generic flight to save !", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            else
            {
                Sort();
                foreach (ClassGenericFlight tmp in MyGenericFlights) tmp.Code = MyUtil.Code; 

                #region Application.Current.Shutdown();
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = MyUtil.Code + "_Flight"; // Default file name
                dlg.DefaultExt = ".xml"; // Default file extension
                dlg.Filter = "Text documents (.xml)|*.xml"; // Filter files by extension
                dlg.InitialDirectory = MyManager.WorkPlace;
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true) ClassSerializable.SaveAsXMLFormat(MyGenericFlights, dlg.FileName);
                #endregion
            }
        }

        private void Load_Gen_Click(object sender, RoutedEventArgs e)
        {
            #region OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = MyUtil.Code + "_Flight"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "Text documents (.xml)|*.xml"; // Filter files by extension
            dlg.InitialDirectory = MyManager.WorkPlace;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true) MyGenericFlights = ClassSerializable.LoadFromXMLFormat<ClassGenericFlight>(dlg.FileName);
            Sort();
            List<ClassGenericFlight> tmp = new List<ClassGenericFlight>();
            foreach (ClassGenericFlight item in MyGenericFlights)
            {
                if (item.Code == null) item.Code = MyUtil.Code;
                else if (!item.Code.Equals(MyUtil.Code)) tmp.Add(item);
            }
            foreach (ClassGenericFlight item in tmp) MyGenericFlights.Remove(item);
            if (MyGenericFlights.Count == 0) MessageBox.Show("No generic flight found for this company !", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            Datagrid_Generic.DataContext = MyGenericFlights;
            #endregion 
        }

        private void Export_Gen_Click(object sender, RoutedEventArgs e)
        {
            if (MyGenericFlights.Count == 0) MessageBox.Show("No generic flight to save !", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            else
            {
                Sort();
                foreach (ClassGenericFlight tmp in MyGenericFlights) tmp.Code = MyUtil.Code;
                #region Application.Current.Shutdown();
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = MyUtil.Code + "_Flight"; // Default file name
                dlg.DefaultExt = ".txt"; // Default file extension
                dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
                dlg.InitialDirectory = MyManager.WorkPlace;
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true) ClassSerializable.WriteFile<ClassGenericFlight>(MyGenericFlights, dlg.FileName);
                #endregion
            }
        }

        private void Import_Gen_Click(object sender, RoutedEventArgs e)
        {
            #region OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = MyUtil.Code + "_Flight"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            dlg.InitialDirectory = MyManager.WorkPlace;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true) MyGenericFlights = ClassSerializable.ReadFile<ClassGenericFlight>(dlg.FileName);
            Sort();
            List<ClassGenericFlight> tmp = new List<ClassGenericFlight>();
            foreach (ClassGenericFlight item in MyGenericFlights)
            {
                if (item.Code == null) item.Code = MyUtil.Code;
                else if (!item.Code.Equals(MyUtil.Code)) tmp.Add(item);
            }
            foreach (ClassGenericFlight item in tmp) MyGenericFlights.Remove(item);
            if (MyGenericFlights.Count == 0) MessageBox.Show("No generic flight found for this company !", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            Datagrid_Generic.DataContext = MyGenericFlights;
            #endregion
        }

        private void Save_Sched_Click(object sender, RoutedEventArgs e)
        {
            if (MyScheduledFlights.Count == 0) MessageBox.Show("No scheduled flight to save !", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
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

        private void Load_Sched_Click(object sender, RoutedEventArgs e)
        {
            #region OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = MyUtil.Code + "_FlightS"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "Text documents (.xml)|*.xml"; // Filter files by extension
            dlg.InitialDirectory = MyManager.WorkPlace;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true) MyScheduledFlights = ClassSerializable.LoadFromXMLFormat<ClassScheduledFlight>(dlg.FileName);
            Sort();
            List<ClassScheduledFlight> tmp = new List<ClassScheduledFlight>();
            foreach (ClassScheduledFlight item in MyScheduledFlights)
            {
                if (item.Flight.Code == null) item.Flight.Code = MyUtil.Code;
                else if (!item.Flight.Code.Equals(MyUtil.Code)) tmp.Add(item);
            }
            foreach (ClassScheduledFlight item in tmp) MyScheduledFlights.Remove(item);
            if (MyScheduledFlights.Count == 0) MessageBox.Show("No scheduled flight found for this company !", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            Datagrid_Scheduled.DataContext = MyScheduledFlights; 
            #endregion 
        }

        private void Deconnexion_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
        #endregion

        #region OPTION_MENU 
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Nadin Pierre \nGroupe 2222 \nCopyright HEPL", "About", MessageBoxButton.OK, MessageBoxImage.Information); 
        }

        private void Option_Click(object sender, RoutedEventArgs e)
        {
            MyManager.Option(); 
        }
        #endregion

        private void Button_PlanningFlight_Click(object sender, RoutedEventArgs e)
        {
            if (Datepicker_Scheduledflight.Text.Length == 0) MessageBox.Show("Select a date before !", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            else if (Datagrid_Generic.SelectedIndex == -1) MessageBox.Show("Select at least a generic flight !", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            else
            {
                try
                {
                    foreach (ClassGenericFlight item in Datagrid_Generic.SelectedItems)
                    {
                        ClassScheduledFlight flight = new ClassScheduledFlight(Convert.ToDateTime(Datepicker_Scheduledflight.Text), item);
                        MyScheduledFlights.Add(flight);
                    }
                    Datagrid_Scheduled.DataContext = MyScheduledFlights;
                }
                catch (Exception) { }
            }
        }

        private void Delete_GenericFlight_Click(object sender, RoutedEventArgs e)
        {
            if (Datagrid_Generic.SelectedIndex == -1) MessageBox.Show("Select at least a generic flight !", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            else
            {
                try
                {
                    List<ClassGenericFlight> tmp = new List<ClassGenericFlight>();
                    foreach (ClassGenericFlight item in Datagrid_Generic.SelectedItems) tmp.Add(item);
                    foreach (ClassGenericFlight item in tmp) MyGenericFlights.Remove(item);
                    Datagrid_Generic.DataContext = MyGenericFlights;
                }
                catch (Exception) { }
            }
        }

        private void Delete_ScheduledFlight_Click(object sender, RoutedEventArgs e)
        {
            if (Datagrid_Scheduled.SelectedIndex == -1) MessageBox.Show("Select at least a scheduled flight !", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            else
            {
                try
                {
                    List<ClassScheduledFlight> tmp = new List<ClassScheduledFlight>();
                    foreach (ClassScheduledFlight item in Datagrid_Scheduled.SelectedItems) tmp.Add(item);
                    foreach (ClassScheduledFlight item in tmp) MyScheduledFlights.Remove(item);
                    Datagrid_Scheduled.DataContext = MyScheduledFlights;
                }
                catch (Exception) { }
            }
        }
        #endregion

        #region METHODES 
        private void Closing_Company(object sender, System.ComponentModel.CancelEventArgs e) // ouvre fenetre pour demander si on sauvegarde ou non 
        {
            MessageBoxResult i = MessageBox.Show(this, "Do you want to save ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (MessageBoxResult.Yes == i) 
            {
                ClassSerializable.SaveAsXMLFormat(MyGenericFlights, MyManager.WorkPlace + "/" + MyUtil.Code + "_Flight.xml"); 
                ClassSerializable.SaveAsXMLFormat(MyScheduledFlights, MyManager.WorkPlace + "/" + MyUtil.Code + "_FlightS.xml"); 
            }
            MainWindow tmp = new MainWindow();
            tmp.Show();
        }
        public void Sort() 
        {
            List<ClassGenericFlight> sortableList = new List<ClassGenericFlight>(MyGenericFlights);
            sortableList.Sort(); 

            for (int i = 0; i < sortableList.Count; i++) 
            {
                MyGenericFlights.Move(MyGenericFlights.IndexOf(sortableList[i]), i);
            }
        }
        #endregion
    }
}
