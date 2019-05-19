using System;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;

namespace Classes
{
    public class ClassRegistry
    {
        public string Nom { get; set; }
        public RegistryKey MyKey { get; set; }
        public string Image { get; set; }
        public string WorkPlace { get; set; }
        public string Ville { get; set; }

        public ClassRegistry()
        {
            // crée ou ouvre les sous dossier 
            MyKey = Registry.CurrentUser.CreateSubKey("Software");
            MyKey = MyKey.CreateSubKey("HEPL");
        }

        public void Connexion(string code, string nom, string mdp) // vérification de connexion login 
        {
            // crée ou ouvre les sous dossier 
            MyKey = Registry.CurrentUser.CreateSubKey("Software");
            MyKey = MyKey.CreateSubKey("HEPL");

            // choix du prochain sous dossier selon le code 
            switch (code.Length)
            {
                case 2: MyKey = MyKey.CreateSubKey("Code_Company"); break;
                case 3: MyKey = MyKey.CreateSubKey("Code_Airport"); break;
            }

            // crée sous dossier du nom du code ou ouvre s'il existe deja 
            MyKey = MyKey.CreateSubKey(code);

            // si utilisateur n'est pas enregistré 
            if (MyKey.GetValue(nom) == null) throw new Exception("Unknown user !"); 

            // si mot de passe ne correspond pas a valeur affectée à l'utilisateur 
            if ((string)MyKey.GetValue(nom) != mdp) throw new Exception("Incorrect password !");

            Image = (string)MyKey.GetValue("Image");
            Ville = (string)MyKey.GetValue("Ville");
            Nom = (string)MyKey.GetValue("Nom");
        }

        public void Init() // on utilise le dossier courant (workplace) pour data 
        {
            // crée ou ouvre les sous dossier 
            MyKey = Registry.CurrentUser.CreateSubKey("Software");
            MyKey = MyKey.CreateSubKey("HEPL");

            // si aucune valeur existe pour Workplace on lui donne une valeur 
            if ((string)MyKey.GetValue("Workplace") == null)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog { SelectedPath = Directory.GetCurrentDirectory() };
                if (fbd.ShowDialog() == DialogResult.OK) MyKey.SetValue("Workplace", fbd.SelectedPath);
            }

            // récupération de la valeur 
            WorkPlace = (string)MyKey.GetValue("Workplace");
        }

        public void Option() // on utilise pour changer le dossier de data 
        {
            // crée ou ouvre les sous dossier 
            MyKey = Registry.CurrentUser.CreateSubKey("Software");
            MyKey = MyKey.CreateSubKey("HEPL");

            FolderBrowserDialog fbd = new FolderBrowserDialog { SelectedPath = Directory.GetCurrentDirectory() };
            if (fbd.ShowDialog() == DialogResult.OK) MyKey.SetValue("Workplace", fbd.SelectedPath);

            // récupération de la valeur 
            WorkPlace = (string)MyKey.GetValue("Workplace");
        }

        public void Nouveau(string code, string password, string password2, string login, string ville, string companyName)
        {
            if (!password.Equals(password2)) throw new Exception("Incorrect password !");

            // crée ou ouvre les sous dossier 
            MyKey = Registry.CurrentUser.CreateSubKey("Software");
            MyKey = MyKey.CreateSubKey("HEPL");

            // choix du prochain sous dossier selon le code 
            switch (code.Length)
            {
                case 2: MyKey = MyKey.CreateSubKey("Code_Company"); break;
                case 3: MyKey = MyKey.CreateSubKey("Code_Airport"); break;
            }

            // crée sous dossier du nom du code ou ouvre s'il existe deja 
            MyKey = MyKey.CreateSubKey(code);

            if ((string)MyKey.GetValue(login) == null) MyKey.SetValue(login, password);
            if ((string)MyKey.GetValue("Ville") == null)
            {
                MyKey.SetValue("Ville", ville);
                Ville = (string)MyKey.GetValue("Ville");
            }
            if ((string)MyKey.GetValue("Nom") == null)
            {
                MyKey.SetValue("Nom", companyName);
                Nom = (string)MyKey.GetValue("Nom");
            }
            if ((string)MyKey.GetValue("Image") == null) // on va chercher image 
            {
                System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog { FileName = Directory.GetCurrentDirectory() }; // aussi dans Win32 
                if (dialog.ShowDialog() == DialogResult.OK) MyKey.SetValue("Image", dialog.FileName);
                Image = dialog.FileName;
            }
        }
    }
}
