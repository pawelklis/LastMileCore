using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    public class DistStreet
    {
        public DistCity  City { get; set; }
        public string Street { get; set; }

        public string StreetPL()
        {
            return ZnakiPL(Street);
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
