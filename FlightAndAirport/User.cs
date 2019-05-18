using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightAndAirport
{
    class User
    {
        #region 
        private string _nom;
        private string _localisation;
        #endregion 

        #region PROPRIETES 
        public string Nom { 
            get { return _nom; }
            set { _nom = value; }
        } 

        public string Loca { 
            get { return _localisation; } 
            set { _localisation = value; }
        } 
        #endregion 

        public User()
        {
            Nom = "";
            Loca = ""; 
        }

        public User(string n, string l)
        {
            Nom = n;
            Loca = m; 
        }
    }
}
