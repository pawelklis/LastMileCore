using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    class DeliveryManType
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string WorkCode { get; set; }

        public List<StopType > STOPS { get; set; }




        public string CreateSTOPS(List<StopFileType> L)
        {
            string returning = "";
            STOPS = new List<StopType>();

            List<string> dictStops = (from o in L
                                      where o.NR_KADROWY == WorkCode
                                      select o.STOP_CODE).Distinct().ToList();

           foreach (var s in dictStops)
            {
                StopType o = new StopType();
                o.STOPCode = s;
                o.ParcelList = (from p in L
                                where p.STOP_CODE == o.STOPCode
                                select p).ToList();

                STOPS.Add(o);
            }




            return returning;
        }

    }
}
