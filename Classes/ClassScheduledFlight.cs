using System;
using System.Windows.Forms;
using System.ComponentModel; // pour l'event PropertyChanged 
using System.Runtime.CompilerServices; // in NotifyPropertyChanged (CallerMemberName) 

namespace Classes
{
    public class ClassScheduledFlight : IComparable<ClassScheduledFlight> 
    {
        private int _passengers; 
        private DateTime _date;
        public ClassGenericFlight Flight { get; set; } 
        
        #region PROPRIETES 
        public int Passengers 
        {
            get => _passengers;
            set
            {
                if (value < 0) MessageBox.Show("Pas de passagers négatifs");
                else if (value > 1000) MessageBox.Show("Abuse pas...");
                else _passengers = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region CONSTRUCTEURS 
        public ClassScheduledFlight()
        {

        }
        public ClassScheduledFlight(DateTime date, ClassGenericFlight flight) 
        {
            Date = new DateTime(date.Year, date.Month, date.Day);
            Flight = flight; 
        }
        #endregion

        #region METHODES 
        public int CompareTo(ClassScheduledFlight other) => Flight.Departs.CompareTo(other.Flight.Departs); 
        private void NotifyPropertyChanged([CallerMemberName] string propertyname = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion 
    }
}
