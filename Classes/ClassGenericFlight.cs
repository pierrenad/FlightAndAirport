using System;
using System.Windows.Forms;
using System.Xml.Serialization; 
using System.ComponentModel; // pour l'event PropertyChanged 
using System.Runtime.CompilerServices; // in NotifyPropertyChanged (CallerMemberName) 

namespace Classes
{
    public class ClassGenericFlight : IComparable<ClassGenericFlight> 
    {
        public static string SCHEDULED = "SCHEDULED (Vol programmé)";
        public static string AIRBORNE = "AIRBORNE (Avion décollé)";
        public static string GATECLOSED = "GATE CLOSED (Embarquement finie)";
        public static string LASTCALL = "LASTCALL (Embarquement presque terminé)";
        public static string BOARDING = "BOARDING (Embarquement en cours)";
        public static string FLYING = "FLYING (En vol)";

        #region VARIABLES 
        private string _code;
        private string _source;
        private string _destination;
        private string _city;
        private string _number; 
        private string _status;
        private TimeSpan _departs;
        private TimeSpan _arrives;
        private TimeSpan _duration;
        #endregion 

        #region PROPRIETES 
        public string Code
        {
            get => _code;
            set
            {
                if (value.Length != 2) MessageBox.Show("Code needs 2 letters"); 
                else if (_code != value)
                {
                    _code = value.ToUpper();
                    NotifyPropertyChanged();
                }
            }
        }
        public string Source
        {
            get => _source; 
            set
            {
                if (value.Length != 3) MessageBox.Show("Source needs 3 letters"); 
                else if (_source != value)
                {
                    _source = value.ToUpper();
                    NotifyPropertyChanged();
                }
            }
        }
        public string Destination
        {
            get => _destination;
            set
            {
                if (value.Length != 3) MessageBox.Show("Destination needs 3 letters"); 
                else if (_destination != value)
                {
                    _destination = value.ToUpper();
                    NotifyPropertyChanged();
                }
            }
        } 
        public string City
        {
            get => _city; 
            set
            {
                if (value.Length == 0) MessageBox.Show("Required field"); 
                else if (_city != value)
                {
                    _city = value.ToLower();
                    NotifyPropertyChanged();
                }
            }
        }
        public string Number
        {
            get => _number;
            set
            {
                if (value.Length == 0) MessageBox.Show("Required field");
                else if (value.Length == 1) MessageBox.Show("Flight number in 5 letters"); 
                else if (value.ToUpper()[0] < 'A' || value.ToUpper()[0] > 'Z') MessageBox.Show("Flight number : starts with 2 letters"); 
                else if (value.ToUpper()[1] < 'A' || value.ToUpper()[2] > 'Z') MessageBox.Show("Flight number : starts with 2 letters"); 
                else if (value.Length != 5) MessageBox.Show("Flight number in 5 letters"); 
                else if (_number != value)
                {
                    _number = value.ToUpper();
                    NotifyPropertyChanged();
                }
            }
        }
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                NotifyPropertyChanged();
            }
        }
        public TimeSpan Departs
        {
            get => _departs;
            set
            {
                if (_departs != value)
                {
                    _departs = value;
                    CalculDuree();
                    NotifyPropertyChanged();
                }
            }
        }
        public TimeSpan Arrives
        {
            get => _arrives;
            set
            {
                if (_arrives != value)
                {
                    _arrives = value;
                    CalculDuree();
                    NotifyPropertyChanged();
                }
            }
        }
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    if (Departs > Arrives) _duration = Arrives + TimeSpan.FromHours(24) - Departs;
                    else _duration = Arrives - Departs;
                    NotifyPropertyChanged();
                }
            }
        }

        [XmlElement("HeureArrivee")] // pour save les heures en .xml 
        public long HeureArriveeTicks
        {
            get { return Arrives.Ticks; }
            set { Arrives = new TimeSpan(value); }
        }
        [XmlElement("HeureDepart")]
        public long HeureDepartTicks
        {
            get { return Departs.Ticks; }
            set { Departs = new TimeSpan(value); }
        }
        #endregion

        #region CONSTRUCTEURS 
        public ClassGenericFlight()
        {

        } 
        public ClassGenericFlight(string src, string dst, string place, string num, TimeSpan dep, TimeSpan arr)
        {
            Source = src;
            Destination = dst;
            City = place;
            Number = num;
            Departs = dep;
            Arrives = arr;
            CalculDuree();
        }
        #endregion

        #region METHODES 
        public void CalculDuree()
        {
            if (_departs > _arrives) _duration = _arrives + TimeSpan.FromHours(24) - _departs;
            else _duration = _arrives - _departs;
            NotifyPropertyChanged();
        }
        public int CompareTo(ClassGenericFlight other) => Departs.CompareTo(other.Departs);
        public override string ToString() => "(Generic flight) Source : " + Source + ", destination : " + Destination + ", city : " + City + ", number : " + Number + ", departs : " + Departs + ", arrives : " + Arrives + ", flight duration : " + Duration;
        public void Affiche() => Console.WriteLine(ToString());
        private void NotifyPropertyChanged([CallerMemberName] string propertyname = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname)); 
        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateFlightStatus(DateTime date)
        {
            TimeSpan tmp = new TimeSpan(date.Hour, date.Minute, date.Second);
            TimeSpan res = Departs - tmp;
            if (res.TotalMinutes <= TimeSpan.FromMinutes(-5).TotalMinutes) Status = ClassGenericFlight.FLYING;
            else if (res.TotalMinutes <= TimeSpan.FromMinutes(0).TotalMinutes) Status = ClassGenericFlight.AIRBORNE;
            else if (res.TotalMinutes <= TimeSpan.FromMinutes(5).TotalMinutes) Status = ClassGenericFlight.GATECLOSED;
            else if (res.TotalMinutes <= TimeSpan.FromMinutes(10).TotalMinutes) Status = ClassGenericFlight.LASTCALL;
            else if (res.TotalMinutes <= TimeSpan.FromMinutes(30).TotalMinutes) Status = ClassGenericFlight.BOARDING;
            else Status = ClassGenericFlight.SCHEDULED;
        }
        #endregion 
    }
}
