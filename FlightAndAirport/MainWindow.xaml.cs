using System;
using System.Windows;
using Classes; 

namespace FlightAndAirport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ClassUser Uti { get; set; } 
        public ClassRegistry Manager { get; set; } 

        public MainWindow()
        {
            InitializeComponent();
            Uti = new ClassUser();
            Manager = new ClassRegistry();
            Affichage("Connexion");
            // plus rapide pour les tests 
            Login.Text = "Admin";
            Password.Password = "admin"; 
        }

        #region BOUTONS_CONNEXION 
        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Code.Text.Length != 2 && Code.Text.Length != 3) throw new Exception("Bad code ! (2 or 3 letters)");
                Manager.Init(); 
                Manager.Connexion(Code.Text, Login.Text, Password.Password);
                // si on passe ligne au dessus c'est que ça c'est bien passé 
                Uti = new ClassUser(Manager.Nom, Code.Text, Manager.Image, Manager.Ville); // on va l'utiliser dans company donc faut tous les elements qu'on a besoin, dans la classe 

                if(Code.Text.Length == 2)
                {
                    Company comp = new Company(this);
                    comp.Show(); 
                }
                else if(Code.Text.Length == 3)
                {
                    Airport aer = new Airport(this);
                    aer.Show(); 
                }
                this.Close(); 
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Password.Password = "";
                Code.Text = ""; 
            }
        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            Login.Text = "";
            Password.Password = "";
            Code.Text = "";
            this.Close(); 
        }

        private void Enregistrer_Click(object sender, RoutedEventArgs e)
        {
            Affichage("Nouveau"); 
        }
        #endregion

        #region BOUTONS_NOUVEL_UTI
        private void ButtonCreer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (New_Code.Text.Length != 2 && New_Code.Text.Length != 3) throw new Exception("Mauvais code ! (2 ou 3 lettres)");
                // creation de l'utilisateur 
                Manager.Nouveau(New_Code.Text, New_Password.Password, New_PasswordConfirmation.Password, New_Login.Text, New_Ville.Text, New_Company.Text);
                // apres ça on affiche la connexion 
                Affichage("Connexion");
                New_Code.Text = "";
                New_Company.Text = "";
                New_Login.Text = "";
                New_Password.Password = "";
                New_PasswordConfirmation.Password = "";
                New_Ville.Text = ""; 
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                New_Code.Text = "";
                New_Password.Password = "";
                New_PasswordConfirmation.Password = ""; 
            }
        }

        private void ButtonAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Affichage("Connexion"); 
        }
        #endregion 

        public void Affichage(string aff)
        {
            switch(aff)
            {
                case "Connexion":
                    StackNouveau.Visibility = Visibility.Collapsed; 
                    StackConnexion.Visibility = Visibility.Visible; 
                    break;
                case "Nouveau":
                    StackConnexion.Visibility = Visibility.Collapsed; 
                    StackNouveau.Visibility = Visibility.Visible; 
                    break;
            }
        }

    }
}
