using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordRecognizerCore;
using System.Reflection;

namespace LastMileCore
{
    [DebuggerDisplay("{NR_PRZ} {NR_REJONU} {NAZWA_ADRES} {PNA_DORECZ} {MIEJSC_DORECZ} {ULICA_DORECZ} {NR_DOM_DORECZ}")]
    public class StopFileType
    {
        public string PNI_AWIZO { get; set; }
        public string JEDNOSTKA_DOR { get; set; }
        public string NR_REJONU { get; set; }
        public string NR_CHODU { get; set; }
        public string DOR_IMIE { get; set; }
        public string DOR_NAZWISKO { get; set; }
        public string TYP_PRZ { get; set; }
        public string NR_PRZ { get; set; }
        public string DTCZAS_WYDANIA { get; set; }
        public string STATUS_PRZE_W_KO { get; set; }
        public string DATA_STATUSU { get; set; }
        public string KOD_PRZYCZYNY { get; set; }
        public string NAZWA_ADRES { get; set; }
        public string PNA_DORECZ { get; set; }
        public string MIEJSC_DORECZ { get; set; }
        public string ULICA_DORECZ { get; set; }
        public string NR_DOM_DORECZ { get; set; }
        public string OPZ { get; set; }
        public string SERWIS { get; set; }
        public string UP_PNI { get; set; }
        public string W_UP_AWIZACYJNYM { get; set; }
        public string PRZYJ_W_PUNKCIE_EN { get; set; }
        public string PRZEWOZNIK_NAZWA { get; set; }
        public string WAGA { get; set; }
        public string NSTD { get; set; }
        public string NAZWA_JD { get; set; }
        public string DOR_KOD { get; set; }
        public string PRZEWOZNIK_KOD { get; set; }

        public string NormalizeMiejsc { get; set; }
        public string NormalizeUlica { get; set; }
        public string NormalizeNrBud { get; set; }

        public List<PArcelGroupSumType> PARCEL_GROUP { get; set; }
              
        public string STOP_CODE { get; set; }
        public bool Is_Delivered { get; set; }
             
        public bool NormalizedAdress { get; set; }

        public StawkaTyp TypStawki { get; set; }
        public double STAWKA { get; set; }

        public static event EventHandler OnNeedNormalizeInformation;
       
        public bool NormalizeAdres(WordRecognizerType Recognizer)
        {
          
            string city = MIEJSC_DORECZ;
            string street = ULICA_DORECZ;
            string dom = NR_DOM_DORECZ;


            List<RecognitionResulType> result = Recognizer.Match(city, street, PNA_DORECZ);
            if(result.Count > 0)
            {
            var r = result[0];

            NormalizeMiejsc = r.ResultDict.City;
            NormalizeUlica = r.ResultDict.Street;
            NormalizeNrBud = dom;

                return true;
            }
            else
            {
                EventHandler temp = OnNeedNormalizeInformation;
                if (temp != null)
                    temp(this, new EventArgs());
                OnNeedNormalizeInformation(this,  new EventArgs());
                return true;
            }

            return false;
        }

        public string GenereteSTOPCode()
        {
            //nazwa adres  PNA miejsc  ulica nrbud   przewoznikkod dorimie dornazwisko @ nrrejonu ^ data(wydania odate)
            //2 2                2        2   2

            string StopCode = GetNazwaAdesCod() + GetPnaCode() + GetMiejscCode()+GetUlicaCode()+GetNRBUDCode()+GetKurierCode();

            STOP_CODE = StopCode;

            return StopCode;
        }


        public void GetParcelGroup()
        {

            List<ParcelGroupType> l = DicterType.ParcelGroups();
            PARCEL_GROUP = new List<PArcelGroupSumType> ();

            foreach (var od in l)
            {
                PArcelGroupSumType o = new PArcelGroupSumType();
                o.Name = od.ParcelGroupName;

                foreach(var prop in typeof(StopFileType).GetProperties())
                {
                    if (prop.Name == od.Column)
                    {
                        foreach(var pv in od.Parcelcodes())
                        {
                            string val = prop.GetValue(this).ToString();
                            if (pv == val)
                            {
                                o.IsTrue = 1;
                                if (od.TypStawki != StawkaTyp.BRAK)
                                {
                                    TypStawki = od.TypStawki;
                                }


                                if (Is_Delivered == true)
                                {
                                    o.isTrueDeliveredOnly = 1;                                    
                                }
                            }
                        }
                    }
                }

                PARCEL_GROUP.Add(o);
            }        
                        
        }

        public bool IsDelivered()
        {
            bool returning = false;
            List<string> L = DicterType.DeliveredCodes();


            foreach(var code in L)
            {
                if (STATUS_PRZE_W_KO == code)
                {
                    returning = true;
                }
            }

            Is_Delivered = returning;
            return returning;
        }

        public void SetStawka(List<CennikType> L)
        {
            foreach(CennikType c in L)
            {
                if (c.Rejon == NR_REJONU)
                {
                    if (c.Przewoznik_KOD == PRZEWOZNIK_KOD)
                    {
                        if (TypStawki == StawkaTyp.StawkaPrzesylkaKurierska)
                        {
                            STAWKA = c.StawkaPrzesylkaKurierska;
                        }
                        if (TypStawki ==  StawkaTyp.StawkaPrzesylkaPocztowa )
                        {
                            STAWKA = c.StawkaPrzesylkaKurierska;
                        }
                    }
                }
            }
        }
        private string ZnakiPL(string TextZrodlowy)
        {
            string TextDocelowy = TextZrodlowy;       

            TextDocelowy = TextDocelowy.ToLower().Replace("ą", "a");
            TextDocelowy = TextDocelowy.ToLower().Replace("ć", "c");
            TextDocelowy = TextDocelowy.ToLower().Replace("ę", "e");
            TextDocelowy = TextDocelowy.ToLower().Replace("ł", "l");
            TextDocelowy = TextDocelowy.ToLower().Replace("ń", "n");
            TextDocelowy = TextDocelowy.ToLower().Replace("ó", "o");
            TextDocelowy = TextDocelowy.ToLower().Replace("ś", "s");
            TextDocelowy = TextDocelowy.ToLower().Replace("ź", "z");
            TextDocelowy = TextDocelowy.ToLower().Replace("ż", "z");

            return TextDocelowy.ToUpper();
        }

        private string GetKurierCode()
        {
            string KurierCode = "";

            if (string.IsNullOrEmpty(DTCZAS_WYDANIA))
            {
                DTCZAS_WYDANIA = "0001-01-01";
            }

            DateTime dt = DateTime.Parse(DTCZAS_WYDANIA);

            DateTime tt = DateTime.Parse(dt.ToShortDateString());

            string odate = tt.ToOADate().ToString() ;

            KurierCode = PRZEWOZNIK_KOD + DOR_IMIE + DOR_NAZWISKO + "@" + NR_REJONU + "^" + odate;
            KurierCode = KurierCode.Replace(" ", "").ToUpper();

            return KurierCode;
        }

        private string lenghtTotwo(string frase)
        {
            if (string.IsNullOrEmpty(frase))
            {
                return "__";
            }
            if (frase.Length == 1)
            {
                frase = frase + "_";
            }

            return frase;
        }

        private string GetNRBUDCode()
        {
            string NRBUDCode = lenghtTotwo(NR_DOM_DORECZ).Replace(" ", "").Substring(0, 2).ToUpper();

            return NRBUDCode;
        }
        private string GetUlicaCode()
        {
            string UlicaCode = lenghtTotwo(ULICA_DORECZ).Replace(" ", "").Substring(0, 2).ToUpper();

            return UlicaCode;
        }
        private string GetMiejscCode()
        {
            string MiejscCode = lenghtTotwo(MIEJSC_DORECZ).Replace(" ","").Substring(0,2).ToUpper();

            return MiejscCode;
        }

        private string GetPnaCode()
        {
            string Pnacode =PNA_DORECZ.Replace(" ","").Replace("-","");
            return Pnacode;
        }

        private string GetNazwaAdesCod()
        {
            List<string> lNazwaAdres = NAZWA_ADRES.Split(char.Parse(" ")).ToList();
                

            string codNazwaAdres = "";
            if (lNazwaAdres.Count == 1)
            {
                codNazwaAdres = lenghtTotwo(lNazwaAdres[0]).Replace(" ", "").Substring(0, 2);
            }
            if (lNazwaAdres.Count > 1)
            {
                codNazwaAdres = lenghtTotwo(lNazwaAdres[0]).Replace(" ", "").Substring(0, 2) + lenghtTotwo(lNazwaAdres[1]).Replace(" ", "").Substring(0, 2);
            }

            return codNazwaAdres.ToUpper();
        }






    }
}
