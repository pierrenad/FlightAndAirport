using System;

namespace Classes
{
    public class ClassUser
    {
        public string Nom { get; set; }
        public string Mdp { get; set; }
        public string Code { get; set; }

        public ClassUser() { }

        public ClassUser(string nom, string mdp, string code)
        {
            Nom = nom;
            Mdp = mdp;
            Code = code;
        }

        public override string ToString() => "Nom : " + Nom + ", Mot de passe : " + Mdp + ", Code : " + Code;
        public void Affiche() => Console.WriteLine(ToString());
    }
}