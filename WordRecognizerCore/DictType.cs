using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordRecognizerCore
{
    public class DictType
    {

        public string PNA { get; set; }
        public string City { get; set; }
        public string Street { get; set; }


        public string PNA_CITY_STREET_LOWER()
        {
            string Result = PNA + " " + City + " " + Street;
            return Result.ToLower();
        }

        public string CITY_STREET_LOWER()
        {
            string Result = City + " " + Street;
            return Result.ToLower();
        }

        public string CITY_LOWER()
        {
            string Result = City ;
            return Result.ToLower();
        }

        public string STREET_LOWER()
        {
            string Result = Street;
            return Result.ToLower();
        }
    }

    public class AliasType
    {
        public string Alias { get; set; }
        public string PNA { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public DictType ToDict()
        {
            DictType o = new DictType();
            o.City = this.City ;
            o.Street = this.Street ;
            o.PNA = this.PNA;
            return o;
        }
    
    }
}
