using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordRecognizerCore
{
    public class RecognitionResulType
    {

        public string Input { get; set; }
        public DictType ResultDict { get; set; }
        public string Method { get; set; }
        public double MatchPercentage { get; set; }


        public class ResulDecisionType
        {
            public string input { get; set; }
            public List<RecognitionResulType> Results { get; set; }
        }

    }



}
