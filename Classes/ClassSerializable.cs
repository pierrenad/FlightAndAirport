using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvHelper;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Classes
{
    public class ClassSerializable
    {
        private static CsvHelper.Configuration.Configuration ConfigCvs = new CsvHelper.Configuration.Configuration { Delimiter = ";" };

        // Save / Load 
        public static void SaveAsXMLFormat<T>(ObservableCollection<T> tmp, string filename) where T : class
        {
            if (filename == null) return;
            XmlSerializer xmlFormat = new XmlSerializer(typeof(ObservableCollection<T>));
            using (Stream fStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xmlFormat.Serialize(fStream, tmp);
            }
        }
        public static ObservableCollection<T> LoadFromXMLFormat<T>(string filename) where T : class
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(ObservableCollection<T>));
            using (Stream fStream = File.OpenRead(filename))
            {
                try
                {
                    return (ObservableCollection<T>)xmlFormat.Deserialize(fStream);
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Bad file chosen !"); 
                    ObservableCollection<T> tmp = new ObservableCollection<T>();
                    return tmp;
                }
            }
        }

        // Read / Write - csv Format 
        public static bool WriteFile<T>(ObservableCollection<T> tmp, string filename) where T : class
        {
            try
            {
                using (TextWriter tr = new StreamWriter(filename, false, Encoding.GetEncoding(1252)))
                {
                    CsvWriter csv = new CsvWriter(tr, ConfigCvs);
                    csv.WriteRecords(tmp);
                }
                return false;
            }
            catch
            {
                return true;
            }
        }
        public static ObservableCollection<T> ReadFile<T>(string filename) where T : class
        {
            ObservableCollection<T> collection = new ObservableCollection<T>();
            List<T> tmp = new List<T>();
            using (TextReader tr = new StreamReader(filename, Encoding.GetEncoding(1252)))
            {
                CsvReader csv = new CsvReader(tr, ConfigCvs);
                tmp = csv.GetRecords<T>().ToList();
                foreach (var item in tmp) collection.Add(item);
            }
            return collection;
        }
    }
}
