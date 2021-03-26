using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    class DicterType
    {


        public static List<ParcelGroupType> ParcelGroups()
        {
            List<ParcelGroupType> Lista = new List<ParcelGroupType>();

            ParcelGroupType o;
         
            o = new ParcelGroupType();
            o.ParcelCode = "B";
            o.ParcelGroupName = "USŁUGA NIEPOWSZECHNA";
            Lista.Add(o);

            o = new ParcelGroupType();
            o.ParcelCode = "EMS";
            o.ParcelGroupName = "USŁUGA NIEPOWSZECHNA";
            Lista.Add(o);

            o = new ParcelGroupType();
            o.ParcelCode = "PPLUS";
            o.ParcelGroupName = "USŁUGA NIEPOWSZECHNA";
            Lista.Add(o);

            o = new ParcelGroupType();
            o.ParcelCode = "UP";
            o.ParcelGroupName = "USŁUGA NIEPOWSZECHNA";
            Lista.Add(o);
         
            o = new ParcelGroupType();
            o.ParcelCode = "BPR";
            o.ParcelGroupName = "USŁUGA KURIERSKA";
            Lista.Add(o);

            o = new ParcelGroupType();
            o.ParcelCode = "PP";
            o.ParcelGroupName = "USŁUGA KURIERSKA";
            Lista.Add(o);

            o = new ParcelGroupType();
            o.ParcelCode = "PP2";
            o.ParcelGroupName = "USŁUGA KURIERSKA";
            Lista.Add(o);

            o = new ParcelGroupType();
            o.ParcelCode = "PXN";
            o.ParcelGroupName = "USŁUGA KURIERSKA";
            Lista.Add(o);

            o = new ParcelGroupType();
            o.ParcelCode = "UK";
            o.ParcelGroupName = "USŁUGA KURIERSKA";
            Lista.Add(o);
         
            o = new ParcelGroupType();
            o.ParcelCode = "P";
            o.ParcelGroupName = "USŁUGA POWSZECHNA";
            Lista.Add(o);

            o = new ParcelGroupType();
            o.ParcelCode = "PPR";
            o.ParcelGroupName = "USŁUGA POWSZECHNA";
            Lista.Add(o);

            o = new ParcelGroupType();
            o.ParcelCode = "PW";
            o.ParcelGroupName = "USŁUGA POWSZECHNA";
            Lista.Add(o);

            o = new ParcelGroupType();
            o.ParcelCode = "PWPR";
            o.ParcelGroupName = "USŁUGA POWSZECHNA";
            Lista.Add(o);
                  



            return Lista;
        }
    
        public static List<DistCity> Cities()
        {
            List<DistCity> L = new List<DistCity>();

            string fc = System.IO.File.ReadAllText("diststreet.csv");
            foreach (var line in fc.Split('\n'))
            {
                DistCity o = new DistCity();
                o.City = line.Split(';')[0];            
                L.Add(o);
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
