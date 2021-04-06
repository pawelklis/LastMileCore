using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordRecognizerCore
{
    class Program
    {
        static void Main(string[] args)
        {


            WordRecognizerType Recognizer = new WordRecognizerType();
            Recognizer.Load("wrdDict.csv","wrdaliases.csv");

            
            //Recognizer.UseMatchFrasesPNA = false;
            //Recognizer.UseMatchFrases = false;
            //Recognizer.UseContainsFrases = false;

            Console.Clear();
            Recognizer.NoMatched.Clear();
            Recognizer.NeedDecision.Clear();
            int Nomatch = 0;

            List<string> Examples = File.ReadAllLines("example.txt").ToList();

            foreach (var frase in Examples)
            {
                string city = WordRecognizerType.RemoveAllSpecialCharacters( frase.Split(';')[0],true,true,true,false);
                string street = WordRecognizerType.RemoveAllSpecialCharacters(frase.Split(';')[1],true,true,true,false);
                string pna = frase.Split(';')[2];

            

                bool ok = false;
                foreach (var r in Recognizer.Match(city,street,pna))
                {
                    Console.WriteLine(r.Input + "^" + r.Method + "^" + r.ResultDict.City + "^" + r.ResultDict.Street + "^" + r.ResultDict.PNA + "^" + r.MatchPercentage +"%" );
                    ok = true;
                }

                if (ok == false)
                {
                    Console.WriteLine(frase );
                    Nomatch += 1;
                }
            }
            double nom = Nomatch;
            double all = Examples.Count;

            double per = double.Parse("100") - ((nom / all) * 100);

            Console.WriteLine("All:" + Examples.Count + " NoMatch:" + Nomatch + " Percentage:" + per + "%");


            //aliasy
            foreach (var n in Recognizer.NoMatched.Distinct())
            {
                string nn = n.Replace("_", " ");
                string acity;
                string astreet;
                string apna;
                Console.WriteLine("Miejscowosc dla:" + nn);
                acity = Console.ReadLine();
                Console.WriteLine("Ulica dla:" + nn);
                astreet  = Console.ReadLine();
                Console.WriteLine("PNA dla:" + nn);
                apna = Console.ReadLine();

                AliasType o = new AliasType();
                o.Alias = nn;
                o.City = acity;
                o.Street = astreet;
                o.PNA = apna;
                Recognizer.Aliasses.Add(o);

                List<string> nl = new List<string>();
                nl.Add(o.Alias + ";" + o.City + ";" + o.Street + ";" + o.PNA);

                File.AppendAllLines("wrdaliases.csv", nl.ToArray());
            }

        }
    }
}
