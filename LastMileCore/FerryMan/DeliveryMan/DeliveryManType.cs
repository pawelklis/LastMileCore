using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    [DebuggerDisplay("{Name} {Surname} {WorkCode}")]
    public class DeliveryManType
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string WorkCode { get; set; }

        public string PrzewoznikNazwa { get; set; }
        public string PrzewoznikKOD { get; set; }

        public List<DMrejonType> Rejons { get; set; }

        public double SumaStawkaDorecz()
        {

            double ss = 0;
            foreach(var r in Rejons)
            {
            foreach (var s in r.STOPS)
            {
                ss += s.SumaStawkaDorecz();
            }
            }


            return ss;
        }

        public int ParcelCount()
        {
            int sum = 0;
            foreach (var r in Rejons)
            {
                foreach (var s in r.STOPS)
                {
                    sum += s.ParcelCount();
                }
            }

            return sum;
        }

        public string CreateRejons(List<StopFileType> L)
        {
            string returning = "";
            Rejons = new List<DMrejonType>();

            List<string> dictRejons = (from o in L
                                      where o.DOR_KOD == WorkCode
                                      select o.NR_REJONU).Distinct().ToList();

           foreach (var s in dictRejons)
            {
                DMrejonType  o = new DMrejonType();
                o.Name = s;
                o.WorkCode = WorkCode;
                o.PrzewoznikNazwa  = PrzewoznikNazwa ;
                o.PrzewoznikKOD = PrzewoznikKOD ;
                o.CreateSTOPS(L);

                o.GetCennik();

                Rejons.Add(o);
            }

            Rejons = Rejons.OrderByDescending(x => x.Name).ToList();


            return returning;
        }

        public List<StopFileType> ParcelList()
        {
            List<StopFileType> L = new List<StopFileType>();
            foreach (var rejon in Rejons)
            {
                foreach (var s in rejon.STOPS)
                {
                    L.AddRange(s.ParcelList.ToArray());
                }
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


            foreach(var rej in Rejons)
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
    

            return l;
        }

    }
}
