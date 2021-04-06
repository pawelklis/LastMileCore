using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordRecognizerCore
{
    public class WordRecognizerType
    {

        private List<DictType> Dict { get; set; }
        public List<AliasType> Aliasses { get; set; }

        public List<String> NoMatched { get; set; }
        public List<RecognitionResulType.ResulDecisionType> NeedDecision {get;set;}

        public double PercentageMatcherPrecision = 79.0;


        //matchAlias matchequals matchfrases ContainsFrases MatchFrasesPNA

        public bool UseMatchAlias = true;
        public bool UseMatchEquals =true;
        public bool UseMatchFrases = true;
        public bool UseMatchFrasesPNA = true;
        public bool UseContainsFrases = true;


        private List<string> DictCities()
        {
            return (from o in Dict
                    select o.City).Distinct().ToList();
        }

        private List<string> DictStreets()
        {
            return (from o in Dict
                    select o.Street).Distinct().ToList();
        }
        private List<string> DictPNA()
        {
            return (from o in Dict
                    select o.PNA).Distinct().ToList();
        }


        public void AddAlias(AliasType a,string city, string street, string pna)
        {
            if (!Aliasses.Contains(a))
            {
                this.Aliasses.Add(a);
            }

         

            List<string> nl = new List<string>();
            nl.Add(a.Alias + ";" +city + ";" + street + ";" + pna);

            File.AppendAllLines("wrdaliases.csv", nl.ToArray());

        }

        /// <summary>
        /// plik csv o kolumnach : Miejscowosc,Ulica,PNA, -rozdzielany ;
        /// </summary>
        /// <param name="dictPath"></param>
        public void Load(string dictPath,string AliastPath)
        {
            List<string> dictLines = File.ReadAllLines(dictPath).ToList();
            Dict = new List<DictType>();
            foreach (var line in dictLines)
            {
                NeedDecision = new List<RecognitionResulType.ResulDecisionType>();
                NoMatched = new List<string>();

                List<string> Words = line.Split(';').ToList();

                DictType o = new DictType();
                o.City = Words[0];
                o.Street = Words[1];
                o.PNA = RemoveAllSpecialCharacters( Words[2],true,true,true,true);


                Dict.Add(o);
            }

            List<string> aliasLineas = File.ReadAllLines(AliastPath).ToList();
            Aliasses  = new List<AliasType>();
            foreach (var line in aliasLineas)
            {

                List<string> Words = line.Split(';').ToList();

                AliasType  o = new AliasType();
                o.Alias = Words[0];
                o.City = Words[1];
                o.Street = Words[2];
                o.PNA = RemoveAllSpecialCharacters(Words[3], true, true, true, true);


                Aliasses.Add(o);
            }

        }

        public List<RecognitionResulType> Match(string city, string street, string pna)
        {

             city = WordRecognizerType.RemoveAllSpecialCharacters(city, true, true, true, false);
             street = WordRecognizerType.RemoveAllSpecialCharacters(street, true, true, true, false);
            pna = pna.Replace("-", "").Replace(" ", "");

            city = city.ToLower();
            street = street.ToLower();
            pna = pna.ToLower();

            if (string.IsNullOrEmpty(city))
            {
                NoMatched.Add(city + "_" + street + "_" + pna);
                return new List<RecognitionResulType>();
            }

            if (string.IsNullOrEmpty(pna))
            {
                new List<RecognitionResulType>();
            }

            street = street.Replace("ul.", "");
            street = street.ToLower().Replace("ul.", " ").Replace("Ul.", "");
            if (street.ToLower().StartsWith("ul "))
            {
                street = street.Substring(3, street.Length - 3);
            }


            street = street.Replace("pl.", "");
            street = street.ToLower().Replace("pl.", " ").Replace("Pl.", "");
            if (street.ToLower().StartsWith("pl "))
            {
                street = street.Substring(3, street.Length - 3);
            }

            List<RecognitionResulType> Results = new List<RecognitionResulType>();


            if (UseMatchAlias == true)
            {
                Results = MatchAlias(city + street);
            }


            if (Results.Count == 0)
            {
                if (UseMatchEquals == true)
                {
                    Results = MatchEquals(city, street, pna).ToList();
                }
            }

            if (Results.Count == 0)
            {
                if (UseMatchFrases == true)
                {
                    Results = MatchFrases(city, street, pna).ToList();
                }
            }

            if (Results.Count == 0)
            {
                if (UseContainsFrases == true)
                {
                    Results = ContainsFrases(city, street, pna).ToList();
                }
            }

            if (Results.Count == 0)
            {
                if (UseMatchFrasesPNA == true)
                {
                    Results = MatchFrasesPNA(city, street, pna).ToList();
                }
            }



            if (Results.Count > 1)
            {

                List<string> DistResults = new List<string>();

                var dist = (from o in Results
                           select o.ResultDict.City+o.ResultDict.Street).Distinct().ToList();

                var ndResults = Results;

                if (UseMatchFrasesPNA == true)
                {
                    Results = MatchFrasesPNA(city, street, pna).ToList();
                }

                if (Results.Count == 0)
                {

                    if (dist.Count > 1)
                    {
                        RecognitionResulType.ResulDecisionType nd = new RecognitionResulType.ResulDecisionType();
                        nd.input = city + "_" + street + "_" + pna;
                        nd.Results = ndResults;
                        NeedDecision.Add(nd);
                    }


                    NoMatched.Add(city + "_" + street + "_" + pna);
                    return new List<RecognitionResulType>();
                }



            }



            if (Results.Count == 0)
            {
                NoMatched.Add(city + "_" + street + "_" + pna);
            }

            return DistResults(Results);
        }

        private List<RecognitionResulType> MatchAlias(string citystreet)
        {
            List<RecognitionResulType> Results = new List<RecognitionResulType>();

            citystreet = citystreet.Replace(" ", "");

            foreach(var alias in Aliasses)
            {
                if (RemoveAllSpecialCharacters( alias.Alias.ToLower().Replace(" ", ""),true,true,true,true).Contains(citystreet.ToLower()))
                {

                    RecognitionResulType R = new RecognitionResulType();
                    R.Input = citystreet;
                    R.Method = "MatchAlias";
                    R.ResultDict = alias.ToDict();
                    R.MatchPercentage = 100.00;

                    Results.Add(R);
                }
            }

            return DistResults(Results);
        }

        private List<RecognitionResulType> MatchEquals(string city,string street,string pna)
        {
            List<RecognitionResulType> Results = new List<RecognitionResulType>();



            foreach (var dd in Dict)
            {
                if (dd.CITY_LOWER() == city.ToLower())
                {
                    if (dd.STREET_LOWER() == street.ToLower())
                    {
                        if (dd.PNA  == pna)
                        {
                            RecognitionResulType R = new RecognitionResulType();
                            R.Input = city + " " + street + " " + pna;
                            R.Method = "MatchEquals";
                            R.ResultDict = dd;
                            R.MatchPercentage = 100.00;

                            if (!Results.Contains(R))
                            {
                                Results.Add(R);
                            }
                        }
                    }
                }
            }

 
                        
            if (Results.Count == 0)
            {
                foreach (var dd in Dict)
                {
                    if (dd.CITY_LOWER() == city.ToLower())
                    {
                        if (dd.STREET_LOWER() == street.ToLower())
                        {
                                RecognitionResulType R = new RecognitionResulType();
                                R.Input = city + " " + street + " " + pna;
                                R.Method = "MatchEquals";
                                R.ResultDict = dd;
                                R.MatchPercentage = 100.00;

                                if (!Results.Contains(R))
                                {
                                    Results.Add(R);
                                }                            
                        }
                    }
                }
            }
                   


            return DistResults(Results); 
        }

        private List<RecognitionResulType> MatchFrases(string city, string street, string pna)
        {
            List<RecognitionResulType> Results = new List<RecognitionResulType>();

            string input = city + street;
            input = ZnakiPL(RemoveAllSpecialCharacters(input, true, true, true, true)).ToLower();

            foreach (var o in Dict)
            {
                string dictfraase = ZnakiPL(RemoveAllSpecialCharacters(o.CITY_STREET_LOWER(), true, true, true, true)).ToLower();
                if (dictfraase  == input)
                {
                    RecognitionResulType R = new RecognitionResulType();
                    R.Input = city + " " + street + " " + pna;
                    R.Method = "MatchFrases";
                    R.ResultDict = o;
                    R.MatchPercentage = 100.00;

                    if (!Results.Contains(R))
                    {
                        Results.Add(R);
                    }
                }
            }


            return DistResults(Results);
        }

        private List<RecognitionResulType> ContainsFrases(string city, string street, string pna)
        {
            List<RecognitionResulType> Results = new List<RecognitionResulType>();


            string input = city + street;
            input = ZnakiPL(RemoveAllSpecialCharacters(input, true, true, true, true)).ToLower();

            foreach (var o in Dict)
            {
                string dictfraase = ZnakiPL(RemoveAllSpecialCharacters(o.CITY_STREET_LOWER(), true, true, true, true)).ToLower();
                if (dictfraase.Contains( input))
                {
                    RecognitionResulType R = new RecognitionResulType();
                    R.Input = city + " " + street + " " + pna;
                    R.Method = "ContainsFrases";
                    R.ResultDict = o;
                    R.MatchPercentage = 100.00;

                    if (!Results.Contains(R))
                    {
                        Results.Add(R);
                    }
                }

                if (input.Contains(dictfraase))
                {
                    RecognitionResulType R = new RecognitionResulType();
                    R.Input = city + " " + street + " " + pna;
                    R.Method = "ContainsFrases";
                    R.ResultDict = o;
                    R.MatchPercentage = 100.00;

                    if (!Results.Contains(R))
                    {
                        Results.Add(R);
                    }
                }



            }

            if (Results.Count == 0)
            {
                input =  street;
                input = ZnakiPL(RemoveAllSpecialCharacters(input, true, true, true, false)).ToLower();

                foreach (var o in Dict)
                {

                    string dictfraase = ZnakiPL(RemoveAllSpecialCharacters(o.CITY_STREET_LOWER(), true, true, true, true)).ToLower();


                    if (ZnakiPL(RemoveAllSpecialCharacters(o.City, true, true, true, true)).ToLower() .Contains( ZnakiPL(RemoveAllSpecialCharacters(city, true, true, true, true)).ToLower()))
                    {
                        foreach (var word in input.Split(' '))
                        {
                            if (dictfraase.Contains(word))
                            {
                                RecognitionResulType R = new RecognitionResulType();
                                R.Input = city + " " + street + " " + pna;
                                R.Method = "ContainsFrasesByPArtWord";
                                R.ResultDict = o;
                                R.MatchPercentage = 100.00;

                                if (!Results.Contains(R))
                                {
                                    Results.Add(R);
                                }
                            }

                            if (word.Contains(dictfraase))
                            {
                                RecognitionResulType R = new RecognitionResulType();
                                R.Input = city + " " + street + " " + pna;
                                R.Method = "ContainsFrasesByPArtWordDict";
                                R.ResultDict = o;
                                R.MatchPercentage = 100.00;

                                if (!Results.Contains(R))
                                {
                                    Results.Add(R);
                                }
                            }
                        }

                    }
                }
            }


            return DistResults(Results);
        }

        private List<RecognitionResulType> MatchFrasesPNA(string city, string street, string pna)
        {
            List<RecognitionResulType> Results = new List<RecognitionResulType>();

            string input = city + street;
            input = ZnakiPL(RemoveAllSpecialCharacters(input, true, true, true, true)).ToLower();

         

            List<DictType> DictByPNA = new List<DictType>();
            DictByPNA = (from o in Dict
                         where ZnakiPL(RemoveAllSpecialCharacters(o.CITY_STREET_LOWER(), true, true, true, true)) == ZnakiPL(RemoveAllSpecialCharacters(city.ToLower(), true, true, true, true))
                         select o).ToList();

            if (DictByPNA.Count == 1)
            {
                if (ZnakiPL(RemoveAllSpecialCharacters(DictByPNA[0].CITY_STREET_LOWER(), true, true, true, true)).Contains(ZnakiPL(RemoveAllSpecialCharacters(street.ToLower(), true, true, true, true))))
                {
                    RecognitionResulType R = new RecognitionResulType();
                    R.Input = city + " " + street + " " + pna;
                    R.Method = "MatchFrases_By_PNA";
                    R.ResultDict = DictByPNA[0];
                    R.MatchPercentage = 100.00;

                    if (!Results.Contains(R))
                    {
                        Results.Add(R);
                    }
                }

            }


            foreach (var o in DictByPNA)
            {
                string dictfraase = ZnakiPL(RemoveAllSpecialCharacters(o.CITY_STREET_LOWER(), true, true, true, true)).ToLower();
                if (dictfraase .Contains( ZnakiPL(RemoveAllSpecialCharacters(city, true, true, true, true)).ToLower()))
                {
                    if (dictfraase.Contains(ZnakiPL(RemoveAllSpecialCharacters(street, true, true, true, true)).ToLower()))
                    {
                        RecognitionResulType R = new RecognitionResulType();
                        R.Input = city + " " + street + " " + pna;
                        R.Method = "MatchFrasesPNA_ByCityStreet";
                        R.ResultDict = o;
                        R.MatchPercentage = 100.00;

                        if (!Results.Contains(R))
                        {
                            Results.Add(R);
                        }
                    }
                }
            }


            foreach (var o in DictByPNA)
            {
                string dictfraase = ZnakiPL(RemoveAllSpecialCharacters(o.CITY_STREET_LOWER(), true, true, true, true)).ToLower();

                foreach(var cityword in city.Split(' '))
                {

                    if (dictfraase.Contains(ZnakiPL(cityword).ToLower()))
                    {
                        foreach (var streetword in street.Split(' '))
                        {
                            if (dictfraase.Contains(ZnakiPL(streetword).ToLower()))
                            {
                                RecognitionResulType R = new RecognitionResulType();
                                R.Input = city + " " + street + " " + pna;
                                R.Method = "MatchFrasesPNA_ByPartOf_CityStreet";
                                R.ResultDict = o;
                                R.MatchPercentage = 100.00;

                                if (!Results.Contains(R))
                                {
                                    Results.Add(R);
                                }
                            }
                        }
                    }
                }

 
            }


            if (Results.Count == 0)
            {
                DictByPNA = (from o in Dict
                             where o.PNA == pna
                             select o).ToList();

                if (DictByPNA.Count == 1)
                {
                    double perm = PercentageMatcher(DictByPNA[0].Street, street);
                    if ( perm > PercentageMatcherPrecision)
                    {
                        RecognitionResulType R = new RecognitionResulType();
                        R.Input = city + " " + street + " " + pna;
                        R.Method = "MatchFrases_By_PNA";
                        R.ResultDict = DictByPNA[0];
                        R.MatchPercentage = 100.00;

                        if (!Results.Contains(R))
                        {
                            Results.Add(R);
                        }
                    }
                    else
                    {
                        NoMatched.Add(city + "_" + street + "_" + pna + "_" + perm);
                    }
                }
            }


            return DistResults(Results);
        }

        private double PercentageMatcher(string WordA,string WordB)
        {

            WordA = ZnakiPL(RemoveAllSpecialCharacters(WordA, true, true, true, true)).ToLower();
            WordB = ZnakiPL(RemoveAllSpecialCharacters(WordB, true, true, true, true)).ToLower();

            double percent = 0.0;
            double matched = 0.0;
            double all = 0.0;

            all = WordB.Length;

            foreach (var leter in WordA)
            {
                if (WordB.Contains(leter))
                {
                    matched += 1;
                }
            }


            percent = (matched / all) * 100;

            return percent;
        }

        private List<RecognitionResulType> DistResults(List<RecognitionResulType> L)
        {
            List<RecognitionResulType> results = new List<RecognitionResulType>();


            foreach(var o in L)
            {
                bool ok = true;

                foreach (var r in results)
                {
                    if( o.ResultDict.City==r.ResultDict.City)
                    {
                        if (o.ResultDict.Street==r.ResultDict.Street)
                        {
                            ok = false;
                        }
                    }
                }

                if (ok == true)
                {
                    results.Add(o);
                }

            }




            return results;
        }
    

        public static string RemoveAllSpecialCharacters(string text,bool RemoveStartSpaces,bool RemoveEndSpaces,bool RemoveDoubleSpaces, bool RemoveAllSpaces)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            string result = Regex.Replace(text, "[:!@#$%^&*()}{|\":?><\\[\\]\\;'/.,~:>?<>{}|~`!@#$%^&*()_+]", "");
            result = result.Replace("-", "");


            if (RemoveStartSpaces == true)
            {
                do
                {
                    if (result.StartsWith(" "))
                    {
                        result = result.Substring(1, result.Length - 1);
                    }
                } while (result.StartsWith(" ") == true);
            }

            if (RemoveEndSpaces == true)
            {
                do
                {
                    if (result.EndsWith(" "))
                    {
                        result = result.Substring(0, result.Length - 1);
                    }
                } while (result.EndsWith(" ") == true);
            }

            if (RemoveDoubleSpaces == true)
            {
                result = result.Replace("  ", " ");
            }

            if (RemoveAllSpaces == true)
            {
                result = result.Replace(" ", "");
            }


            return result;
        }
        public static string RemoveNumerics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            string result = Regex.Replace(text, "[0,1,2,3,4,5,6,7,8,9]", "");
            return result;
        }
        public  static string ZnakiPL(string TextZrodlowy)
        {
            string TextDocelowy = TextZrodlowy;


            TextDocelowy = TextDocelowy.ToLower().Replace("ą", "a");
            TextDocelowy = TextDocelowy.ToLower().Replace("ć", "c");
            TextDocelowy = TextDocelowy.ToLower().Replace("ę", "e");
            TextDocelowy = TextDocelowy.ToLower().Replace("ł", "l");
            TextDocelowy = TextDocelowy.ToLower().Replace("ń", "n");
            TextDocelowy = TextDocelowy.ToLower().Replace("ó", "o");
            TextDocelowy = TextDocelowy.ToLower().Replace("ś", "s");
            TextDocelowy = TextDocelowy.ToLower().Replace("ź", "z");
            TextDocelowy = TextDocelowy.ToLower().Replace("ż", "z");

            return TextDocelowy.ToUpper();
        }
    }
}
