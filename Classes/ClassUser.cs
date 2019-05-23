using System;

namespace Classes
{
    public class ClassUser
    {
        public string Nom { get; set; }
        public string Code { get; set; }
        public string Image { get; set; } 
        public string Localisation { get; set; } 

        public ClassUser() { }

        public ClassUser(string nom, string code, string image, string  loca)
        {
            Nom = nom;
            Code = code;
            Image = image; 
            Localisation = loca; 
        }

        public override string ToString() => "Name : " + Nom + ", Code : " + Code + ", Localisation : " + Localisation; 
    }
}