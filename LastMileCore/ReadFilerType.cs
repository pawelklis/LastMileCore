using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMileCore
{
    public class ReadFilerType
    {

        public static event EventHandler OnNeedNormalizeInformation;

        public static List<StopFileType> ImportSTOPFile(string XLSXFilename, WordRecognizerCore.WordRecognizerType Recognizer,bool NormalizeAdres)
        {
            DataTable dt = ExcelToDataTable(XLSXFilename);
            List<StopFileType> L = CreateList(dt,Recognizer,NormalizeAdres );
            return L;
        }
        private static DataTable  ExcelToDataTable(string XLSXFilename)
        {
            string CSVPAth = XLSXFilename ;
            if(Path.GetExtension(XLSXFilename) == ".xlsx")
            {
                CSVPAth = XLSXtoCSV("^", XLSXFilename);
            }


            DataTable dt = csvToDatatable(CSVPAth, "^");
            try
            {
                System.IO.File.Delete(CSVPAth);
            }
            catch (Exception)
            {

                throw;
            }
            return dt;
        }

        private static List<StopFileType> CreateList(DataTable dt, WordRecognizerCore.WordRecognizerType Recognizer, bool NormalizeAdres)
        {
            StopFileType.OnNeedNormalizeInformation += new EventHandler(OnNeedNormalizeInfo);
            List<StopFileType> L = new List<StopFileType>();
            //Console.Clear();
            for (var i = 0; i <= dt.Rows.Count-1 ; i++)
            {
                StopFileType o = new StopFileType();
                for (var x = 0; x < dt.Columns.Count ; x++)
                {
                    foreach (var prop in o.GetType().GetProperties())
                    {
                        if (prop.Name == dt.Columns[x].ColumnName)
                        {
                            prop.SetValue(o, dt.Rows[i][x].ToString());
                        }
                    }
                }
                o.GenereteSTOPCode();
                o.IsDelivered();
                o.GetParcelGroup();

                if (NormalizeAdres == true)
                {
                    o.NormalizedAdress  = o.NormalizeAdres(Recognizer);
                }
                              
             

                L.Add(o);
            }

            return L;
        }

        static void OnNeedNormalizeInfo(object sender, EventArgs e)
        {
            OnNeedNormalizeInformation(sender, new EventArgs());
        }

        private static string XLSXtoCSV(string separator, string FileName)
        {
            string csvFilename = FileName.Replace("xlsx", "csv");
          
            XLWorkbook workBook = new XLWorkbook(FileName);

            var worksheet = workBook.Worksheets.ToList()[0];
            File.WriteAllLines(csvFilename, worksheet.RowsUsed().Select(row => string.Join(separator, row.Cells(1, row.LastCellUsed(false).Address.ColumnNumber).Select(cell => cell.GetValue<string>()))), System.Text.Encoding.GetEncoding(1250));
            workBook = null/* TODO Change to default(_) if this is not a reference type */;
      
            return csvFilename;
        }

        private static DataTable csvToDatatable(string filename, string separator, int PomijajPierwszeWierszeIlosc = 0)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            bool firstLine = true;
            if (System.IO.File.Exists(filename))
            {
                using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.GetEncoding(1250)))
                {
                    int i = 1;
                    while (!sr.EndOfStream)
                    {
                        if (firstLine)
                        {
                            if (i > PomijajPierwszeWierszeIlosc)
                            {
                                firstLine = false;
                                var cols = sr.ReadLine().Split(char.Parse(separator));
                                foreach (var col in cols)
                                    dt.Columns.Add(new DataColumn(col, typeof(string)));
                            }
                            else
                                sr.ReadLine().Split(char.Parse(separator));
                        }
                        else
                        {
                            string[] data = sr.ReadLine().Split(char.Parse(separator));
                            if (data.Length > dt.Columns.Count)
                                dt.Columns.Add("col_" + dt.Columns.Count);
                            dt.Rows.Add(data.ToArray());
                        }
                        i += 1;
                    }
                }
            }
            return dt;
        }


    }
}
