using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordRecognizerCore;

namespace LastMileCore
{
    class Program
    {
        static WordRecognizerType Recognizer;
        static void OnNeedNormalizeInfo(object sender, EventArgs e)
        {                    
            //Pytania o uzupełnienie adresu

            //string csb;
            //AliasType a = new AliasType();
            //StopFileType  o = (StopFileType)sender;

            //if (!string.IsNullOrEmpty(o.MIEJSC_DORECZ))
            //{
            //    if (!string.IsNullOrEmpty(o.ULICA_DORECZ))
            //    {



            //        Console.WriteLine(o.MIEJSC_DORECZ);
            //        csb = Console.ReadLine();
            //        a.City = csb;

            //        Console.WriteLine(o.ULICA_DORECZ);
            //        csb = Console.ReadLine();
            //        a.Street = csb;

            //        a.PNA = o.PNA_DORECZ;

            //        a.Alias = o.MIEJSC_DORECZ + o.ULICA_DORECZ + o.PNA_DORECZ;

            //        Recognizer.AddAlias(a, o.MIEJSC_DORECZ, o.ULICA_DORECZ, o.PNA_DORECZ);

            //        o.NormalizeAdres(Recognizer);
            //    }
            //}
           
        }
        static void Main(string[] args)
        {
            Recognizer = new WordRecognizerType();
            Recognizer.Load("wrdDict.csv", "wrdaliases.csv");

            ReadFilerType.OnNeedNormalizeInformation += OnNeedNormalizeInfo;

            List<StopFileType> l = ReadFilerType.ImportSTOPFile(@"C:\Users\klispawel\Downloads\RD Wrocław2.xlsx", Recognizer ,false);
            string x = "";



            var distnomatch = Recognizer.NoMatched.Distinct().ToList();

            Console.Clear();
            foreach(var o in l)
            {

                string linia = o.MIEJSC_DORECZ + "^" +
                    o.ULICA_DORECZ + "^" +
                    o.NR_DOM_DORECZ + "^" +
                    o.NormalizeMiejsc + "^" +
                    o.NormalizeUlica + "^" +
                    o.NormalizeNrBud + "^";

                if (!string.IsNullOrEmpty(o.MIEJSC_DORECZ ))
                {
                linia = linia.Replace(" ","|");
                Console.WriteLine(linia);
                }
            }



            List<FerryManType> L = FerryManType.CreateStructure(l);
            Console.Clear();
            foreach (var o in l)
            {
                Console.WriteLine(o.MIEJSC_DORECZ.Replace(" ","$") + "_" + o.ULICA_DORECZ.Replace(" ", "$") + "_" + o.NR_DOM_DORECZ.Replace(" ", "$")
                            + "|" + o.NormalizeMiejsc.Replace(" ", "$") + "_" + o.NormalizeUlica.Replace(" ", "$") + "_" + o.NormalizeNrBud.Replace(" ", "$"));
            }


 

        }
    }
}
