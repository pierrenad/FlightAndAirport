using System;
using System.Collections.Generic;
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

namespace FlightAndAirport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region BOUTONS
        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            Login.Text = "";
            Password.Text = "";
            Code.Text = ""; 
        }

        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            // pour le moment 
            if(Password.Text.Equals("admin") && Login.Text.Equals("Admin")) 
            {
                if() 
            }
            else
            {
                MessageBox.Show("Login incorrect !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        #endregion 
    }
}
