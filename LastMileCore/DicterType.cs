using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    public class DicterType
    {

        public static List<CennikType> GetFullCennik()
        {
            List<CennikType> L = new List<CennikType>();

            List<string> ls = System.IO.File.ReadAllLines("cennik.csv").ToList();

            bool FirstLine = true;

            foreach (var line in ls)
            {
                if (FirstLine == true)
                {
                    FirstLine = false;
                }
                else
                {
                    
                    CennikType o = new CennikType();

                    //Etykiety wierszy	Przewoźnik	Stawka Przesyłka Kurierska	Stawka Przesyłka Pocztowa	Degres 3, 4 lub 11	Aneks ZPO - T lub N	Wskaźnik % 83 85 87 90	Zadanie	nr umowy


                    string srejon = line.Split(';')[0].Replace(",", ".");
                    string sprzewoznikkod = line.Split(';')[1].Replace(",", ".");
                    string sstawkauk = line.Split(';')[2];
                    string sstawkaup = line.Split(';')[3];
                    string sdegres = line.Split(';')[4];
                    string saneks = line.Split(';')[5].Replace(",", ".");
                    string swskaznik = line.Split(';')[6];
                    string szadanie = line.Split(';')[7].Replace(",", ".");
                    string sumowa = line.Split(';')[8].Replace(",", ".");
                    string skod = line.Split(';')[9].Replace(",", ".");

                    o.Rejon = srejon;
                    o.Przewoznik_Nazwa = sprzewoznikkod;
                    o.Zadanie = szadanie ;
                    o.Umowa = sumowa;
                    o.StawkaPrzesylkaKurierska = double.Parse(sstawkauk);
                    o.StawkaPrzesylkaPocztowa = double.Parse(sstawkaup);
                    o.Degres = double.Parse(sdegres );
                    o.AneksZPO = bool.Parse(saneks );
                    o.Wskaznik = double.Parse(swskaznik );
                    o.Przewoznik_KOD = skod;


                    L.Add(o);
                }

            }



            return L;
        }

        public static List<ParcelGroupType> ParcelGroups()
        {
            List<ParcelGroupType> Lista = new List<ParcelGroupType>();

            ParcelGroupType o;
         
            o = new ParcelGroupType();
            o.ParcelCode = "B,EMS,PPLUS,UP";
            o.ParcelGroupName = "USŁUGA NIEPOWSZECHNA";
            o.Column = "TYP_PRZ";
            o.TypStawki = StawkaTyp.StawkaPrzesylkaKurierska;
            Lista.Add(o);

         
            o = new ParcelGroupType();
            o.ParcelCode = "BPR,PP,PP2,PXN,UK";
            o.ParcelGroupName = "USŁUGA KURIERSKA";
            o.Column = "TYP_PRZ";
            o.TypStawki = StawkaTyp.StawkaPrzesylkaKurierska;
            Lista.Add(o);
                      
         
            o = new ParcelGroupType();
            o.ParcelCode = "P,PPR,PW,PWPR";
            o.ParcelGroupName = "USŁUGA POWSZECHNA";
            o.Column = "TYP_PRZ";
            o.TypStawki = StawkaTyp.StawkaPrzesylkaPocztowa;
            Lista.Add(o);
                     

            o = new ParcelGroupType();
            o.ParcelCode = "P_ND_NZPD";
            o.ParcelGroupName = "Niedoręczne wina Kurier";
            o.Column = "KOD_PRZYCZYNY";
            o.TypStawki = StawkaTyp.BRAK;
            Lista.Add(o);


            return Lista;
        }

        public static List<string> DeliveredCodes()
        {
            List<string> L = new List<string>();

            L.Add("D");
            L.Add("DP");
            L.Add("S");

            return L;
        }
    
        public static List<DistCity> Cities()
        {
            List<DistCity> L = new List<DistCity>();

            string fc = System.IO.File.ReadAllText("diststreet.csv");
            foreach (var line in fc.Split('\n'))
            {
                DistCity o = new DistCity();                
                o.City = line.Split(';')[0];
                o.PNAs = new List<string>();
                if ((from v in L where v.City==o.City select v.City).Distinct().ToList().Count == 0) {
                    L.Add(o);
                }                
            }

            foreach (var o in L)
            {
                foreach (var line in fc.Split('\n'))
                {                   
                        if (line.Split(';')[0] == o.City)
                        {
                            o.PNAs.Add(line.Split(';')[2]);
                        }                               
                }
            }

            return L;
        }


        public static List<DistStreet> Streets()
        {
            List<DistStreet> L = new List<DistStreet>();

            string fc = System.IO.File.ReadAllText("diststreet.csv");
            foreach (var line in fc.Split('\n'))
            {
                DistStreet o = new DistStreet();
                o.City = new DistCity();
                o.City.City  = line.Split(';')[0];
                o.Street = line.Split(';')[1];
                L.Add(o);
            }
                      
            return L;
        }
    }
}
