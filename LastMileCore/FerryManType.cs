using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    class FerryManType
    {
        public string Name { get; set; }
        public List<DeliveryManType> DeliveryMans { get; set; }



        public static List<FerryManType> CreateStructure(List<StopFileType> l)
        {
            var distprzwoznicy = (from o in l
                                  select o.PRZEWOZNIK_NAZWA).Distinct().ToList();


            List<FerryManType> Lista = new List<FerryManType>();

            foreach (var dp in distprzwoznicy)
            {
                FerryManType o = new FerryManType();
                o.Name = dp;
                o.GetDeliveryMans(l);
                Lista.Add(o);
            }


            return Lista;
        }

        private  string GetDeliveryMans(List<StopFileType> L)
        {

            var lo = (from o in L
                      where o.PRZEWOZNIK_NAZWA == this.Name
                      select o.DOR_IMIE + "^" + o.DOR_NAZWISKO + "^" + o.NR_KADROWY ).Distinct().ToList();

            DeliveryMans = new List<DeliveryManType>();


            foreach(var oo in lo)
            {
                List<string> DelData = oo.Split(char.Parse("^")).ToList();
                DeliveryManType o = new DeliveryManType();
                o.Name = DelData[0];
                o.Surname = DelData[1];
                o.WorkCode = DelData[2];
                o.CreateSTOPS(L);
                DeliveryMans.Add(o);
            }



            return "";
        }


    }
}
