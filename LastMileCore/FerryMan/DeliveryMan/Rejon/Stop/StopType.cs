using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    [DebuggerDisplay("{STOPCode} {ParcelList.Count}")]
    public class StopType
    {

        public string STOPCode { get; set; }
        public List<StopFileType> ParcelList { get; set; }

        public List<PArcelGroupSumType> Sums { get; set; }
 
        public int ParcelCount()
        {
            return ParcelList.Count();
        }


        public double SumaStawkaDorecz()
        {

            double s = (from o in ParcelList
                     where o.Is_Delivered == true
                     select o.STAWKA).Sum();

            return s;
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
                foreach(var parcel in ParcelList)
                {
                    foreach(var pg in parcel.PARCEL_GROUP)
                    {
                        if (pg.Name == od.Name)
                        {
                            od.IsTrue += pg.IsTrue;
                            od.isTrueDeliveredOnly += pg.isTrueDeliveredOnly;
                        }
                    }
                }
            }

            return l;
        }
   

    }
}
