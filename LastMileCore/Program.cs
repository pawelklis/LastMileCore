using NHunspell;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    class Program
    {
        static void Main(string[] args)
        {

            NHunspell.Hunspell nh = new NHunspell.Hunspell();
            nh.Load("index.aff", "index.dic");
            var ox=nh.Generate("Kąty wrocławskie", "katy wroclawskie");
            nh.AddWithAffix("Kąty wrocławskie", "katy wroclawskie");

            var xx=nh.Spell(" katy wroclawskie");
            var sx = nh.Suggest("dlugoleka");
            List<string> lh = nh.Analyze("wroclaw");
            var aasx=nh.Stem("wroclaw");

        



            using (Hyphen hyphen = new Hyphen("index.dic"))
            {
                Console.WriteLine("Get the hyphenation of the word 'Recommendation'");
                HyphenResult hyphenated = hyphen.Hyphenate("GAŁ=CZYNSKIEGOP");
                Console.WriteLine("'Recommendation' is hyphenated as: " +
                                  hyphenated.HyphenatedWord);
            }

       

       


            List<StopFileType> l = ReadFilerType.ImportSTOPFile(@"C:\Users\klispawel\Downloads\RD Wrocław2.xlsx");
            string x = "";

            Console.Clear();
            foreach(var o in l)
            {

                string linia = o.MIEJSC_DORECZ + "^" +
                    o.ULICA_DORECZ + "^" +
                    o.NR_DOM_DORECZ + "^" +
                    o.NormalizeMiejsc + "^" +
                    o.NormalizeUlica + "^" +
                    o.NormalizeNrBud + "^" +
                    o.PercentegCity + "^" +
                    o.PercentegeStreet;
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


            Console.Clear();
            foreach (var f in L)
            {
                foreach (var d in f.DeliveryMans)
                {
                    foreach (var s in d.STOPS)
                    {
                        if (s.ParcelList.Count > 1)
                        {
                            Console.WriteLine(s.STOPCode + "^" + s.ParcelList.Count + "^" + s.ParcelList[0].MIEJSC_DORECZ + "^" + s.ParcelList[0].ULICA_DORECZ + "^" + s.ParcelList[0].NR_DOM_DORECZ + "^" + s.ParcelList[0].PNA_DORECZ );
                        }
                    }
                }
            }

        }
    }
}
