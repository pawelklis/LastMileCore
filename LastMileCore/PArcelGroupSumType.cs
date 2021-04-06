using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    public class PArcelGroupSumType
    {
        
        public string Name { get; set; }
        public int IsTrue { get; set; }
        public int isTrueDeliveredOnly { get; set; }     
        public double Quantity()
        {
            double per = 0.0;
            double all = IsTrue;
            double Real = isTrueDeliveredOnly;

            per = (Real / all) * 100;
            return Math.Round(per, 2);
        }




    }
}
