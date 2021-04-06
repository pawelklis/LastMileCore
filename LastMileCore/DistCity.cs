using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    public class DistCity
    {
        public string City { get; set; }
        public List<string> PNAs { get; set; }

        public string CityPL()
        {
            return ZnakiPL(City);
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
