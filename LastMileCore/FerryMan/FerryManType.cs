using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    [DebuggerDisplay("{Name}")]
    public class FerryManType
    {
        public string Name { get; set; }
        public string KOD { get; set; }
        public List<DeliveryManType> DeliveryMans { get; set; }


        public double SumaStawkaDorecz()
        {

            double ss = 0;
            foreach(var dv in DeliveryMans)
            {
            foreach (var r in dv.Rejons)
            {
                foreach (var s in r.STOPS)
                {
                    ss += s.SumaStawkaDorecz();
                }
            }
            }



            return ss;
        }

        public int ParcelCount()
        {
            int sum = 0;
            foreach (var dv in DeliveryMans)
            {
                foreach (var r in dv.Rejons)
                {
                    foreach (var s in r.STOPS)
                    {
                        sum += s.ParcelCount();
                    }
                }
            }


            return sum;
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

            foreach (var dm in DeliveryMans)
            {
                foreach (var rej in dm.Rejons)
                {
                    foreach (var od in l)
                    {
                        foreach (var stop in rej.STOPS)
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
                }
            }



            return l;
        }

        public List<StopFileType> ParcelList()
        {
            List<StopFileType> L = new List<StopFileType>();
            foreach(var dm in DeliveryMans)
            {
                foreach (var rejon in dm.Rejons)
                {
                    foreach (var s in rejon.STOPS)
                    {
                        L.AddRange(s.ParcelList.ToArray());
                    }
                }
            }



            return L;
        }

        public static List<FerryManType> CreateStructure(List<StopFileType> l)
        {
            var distprzwoznicy = (from o in l
                                  select o.PRZEWOZNIK_NAZWA).Distinct().ToList();

   

            List<FerryManType> Lista = new List<FerryManType>();

            foreach (var dp in distprzwoznicy)
            {
                string kod = (from oo in l
                              where oo.PRZEWOZNIK_NAZWA == dp
                              select oo.PRZEWOZNIK_KOD).Distinct().ToList()[0];


                FerryManType o = new FerryManType();
                o.Name = dp;
                o.KOD = kod;
                o.GetDeliveryMans(l);
                Lista.Add(o);
            }


            return Lista;
        }

        private  string GetDeliveryMans(List<StopFileType> L)
        {

            var lo = (from o in L
                      where o.PRZEWOZNIK_NAZWA == this.Name
                      select o.DOR_IMIE + "^" + o.DOR_NAZWISKO + "^" + o.DOR_KOD ).Distinct().ToList();

            DeliveryMans = new List<DeliveryManType>();


            foreach(var oo in lo)
            {
                List<string> DelData = oo.Split(char.Parse("^")).ToList();
                DeliveryManType o = new DeliveryManType();
                o.Name = DelData[0];
                o.Surname = DelData[1];
                o.WorkCode = DelData[2];
                o.PrzewoznikNazwa = Name;
                o.PrzewoznikKOD = KOD;
                o.CreateRejons(L);
                DeliveryMans.Add(o);
            }



            return "";
        }


    }
}
