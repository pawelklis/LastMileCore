using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    public class CennikType
    {
        public string Rejon { get; set; }
        public string Przewoznik_KOD { get; set; }
        public string Zadanie { get; set; }
        public string Umowa { get; set; }
        public double StawkaPrzesylkaKurierska { get; set; }
        public double StawkaPrzesylkaPocztowa { get; set; }
        public double Degres { get; set; }
        public bool AneksZPO { get; set; }
        public double Wskaznik { get; set; }

        public string Przewoznik_Nazwa { get; set; }
    }

    public enum StawkaTyp
    {
        BRAK,
        StawkaPrzesylkaKurierska,
        StawkaPrzesylkaPocztowa
    }
}
