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
        private string _mdp;
        #endregion 

        #region PROPRIETES 
        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        public string Mdp
        {
            get { return _mdp; } 
            set { _mdp = value; }
        }
        #endregion 

        public User()
        {
            Nom = "";
            Mdp = ""; 
        }

        public User(string n, string m)
        {
            Nom = n;
            Mdp = m; 
        }
    }
}
