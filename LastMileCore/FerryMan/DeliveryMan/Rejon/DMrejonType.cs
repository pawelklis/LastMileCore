using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    public class DMrejonType
    {
        public string Name { get; set; }
        public string WorkCode { get; set; }
        public string PrzewoznikNazwa { get; set; }
        public string PrzewoznikKOD { get; set; }

        public List<CennikType> Cennik { get; set; }

        public List<StopType> STOPS { get; set; }


        public double SumaStawkaDorecz()
        {

            double ss = 0;

            foreach (var s in STOPS)
            {
                ss += s.SumaStawkaDorecz();
            }

            return ss;
        }


        public void GetCennik()
        {
            Cennik = new List<CennikType>();
            foreach (CennikType c in DicterType.GetFullCennik())
            {
                if (c.Przewoznik_KOD == PrzewoznikKOD)
                {
                    if (c.Rejon == Name)
                    {
                        Cennik.Add(c);
                    }
                }
            }
        }

        public int ParcelCount()
        {
            int sum = 0;
            foreach (var s in STOPS)
            {
                sum += s.ParcelCount();
            }
            return sum;
        }
        public string CreateSTOPS(List<StopFileType> L)
        {
            string returning = "";
            STOPS = new List<StopType>();

            List<string> dictStops = (from o in L
                                      where o.DOR_KOD == WorkCode && o.NR_REJONU == Name
                                      select o.STOP_CODE).Distinct().ToList();

            GetCennik();

            foreach (var s in dictStops)
            {
                StopType o = new StopType();
                o.STOPCode = s;
                o.ParcelList = (from p in L
                                where p.STOP_CODE == o.STOPCode
                                select p).ToList();

                foreach(var p in o.ParcelList)
                {
                    p.SetStawka(Cennik);
                }

                STOPS.Add(o);
            }

            STOPS = STOPS.OrderByDescending(x => x.ParcelList.Count).ToList();


            return returning;
        }

        public List<StopFileType> ParcelList()
        {
            List<StopFileType> L = new List<StopFileType>();

            foreach (var s in STOPS)
            {
                L.AddRange(s.ParcelList.ToArray());
            }

  

            return L;
        }

        public List<PArcelGroupSumType> GetParcelSums()
        {
            List<PArcelGroupSumType> l = new List<PArcelGroupSumType>();



            foreach (var dc in DicterType.ParcelGroups())
            {
                PArcelGroupSumType o = new PArcelGroupSumType();
                o.Name = dc.ParcelGroupName;
                l.Add(o);
            }



            foreach (var od in l)
            {
                foreach (var stop in STOPS)
                {
                    foreach (var parcel in stop.ParcelList)
                    {
                        foreach (var pg in parcel.PARCEL_GROUP)
                        {
                            if (pg.Name == od.Name)
                            {
                                od.IsTrue += pg.IsTrue;
                                od.isTrueDeliveredOnly += pg.isTrueDeliveredOnly;
                            }
                        }
                    }
                }


            }

            return l;
        }

    }
}
