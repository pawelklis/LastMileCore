using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    class WordRecognizerType
    {
    public List<DistCity> Cities { get; set; }
    public List<DistStreet> Streets { get; set; }

        public void Load()
        {
            Cities = DicterType.Cities();
            Streets = DicterType.Streets();
        }
        
        







        private List<DistCity> GetCity(string word,bool znakipl)
        {
            List<DistCity> L = new List<DistCity>();
            if (znakipl == false)
            {
                L = (from o in Cities
                    where o.CityPL().ToLower().StartsWith(word.ToLower())
                    select o).ToList();
            }
            if (znakipl == true)
            {
                L = (from o in Cities
                     where o.CityPL().ToLower().StartsWith(word.ToLower())
                     select o).ToList();
            }


            return L;
        }

        private List<DistStreet> GetStreet(string city,string word, bool znakipl)
        {
            List<DistStreet> L = new List<DistStreet>();
            if (znakipl == false)
            {
                L = (from o in Streets
                    where o.Street.ToLower().StartsWith(word.ToLower())
                    select o).ToList();
            }
            if (znakipl == true)
            {
                L = (from o in Streets
                     where o.StreetPL().ToLower().StartsWith(word.ToLower())
                     select o).ToList();
            }


            return L;
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



    }
}
