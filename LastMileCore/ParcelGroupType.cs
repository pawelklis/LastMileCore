using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    public class ParcelGroupType
    {

        public string ParcelCode { get; set; }
        public string ParcelGroupName { get; set; }
        public string Column { get; set; }
        public StawkaTyp TypStawki { get; set; }



       public List<string> Parcelcodes()
        {
            return ParcelCode.Split(',').ToList();
        }

    }
}
