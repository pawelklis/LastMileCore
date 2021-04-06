using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WordRecognizerCore;
using LastMileCore;

namespace LMForms
{
    public partial class Form1 : Form
    {
        List<FerryManType> L;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            var  Recognizer = new WordRecognizerType();
            Recognizer.Load("wrdDict.csv", "wrdaliases.csv");

            ReadFilerType.OnNeedNormalizeInformation += OnNeedNormalizeInfo;

            List<StopFileType> l = ReadFilerType.ImportSTOPFile(@"C:\Users\klispawel\Downloads\RD Wrocław2.xlsx", Recognizer, false);
        
            var distnomatch = Recognizer.NoMatched.Distinct().ToList();

            L = FerryManType.CreateStructure(l);


            treeView1.Nodes.Clear();


            foreach (var fman in L)
            {
                TreeNode fN = new TreeNode();
                fN.Text = fman.Name + " Liczba przesyłek:" + fman.ParcelCount();

                foreach (var delman in fman.DeliveryMans)
                {
                    TreeNode dM = new TreeNode();
                    dM.Text = delman.Name + " "+ delman.Surname + " " + delman.WorkCode + " Liczba przesyłek:" + delman.ParcelCount();

                   

                    foreach (var rejon in delman.Rejons )
                    {
                        TreeNode rN = new TreeNode();
                        rN.Text = rejon.Name + " Liczba przesyłek:" + rejon.ParcelCount();

                        List<StopFileType> ls = new List<StopFileType>();

                        foreach (var stop in rejon.STOPS)
                        {
                            TreeNode sN = new TreeNode();
                            sN.Text = stop.STOPCode + " Liczba:" + stop.ParcelList.Count;
                            ls.AddRange(stop.ParcelList.ToArray());
                            foreach (var parc in stop.ParcelList)
                            {
                                TreeNode pN = new TreeNode();
                                pN.Text = parc.NR_PRZ + " " + parc.NAZWA_ADRES + " " + parc.MIEJSC_DORECZ + " " + parc.ULICA_DORECZ + " " + parc.NR_DOM_DORECZ + " " + parc.PNA_DORECZ ;

                                pN.Tag = parc;
                                sN.Nodes.Add(pN);                               
                            }

                            sN.Tag = stop;
                            rN.Nodes.Add(sN);
                        }

                        rN.Tag = rejon;                        
                        dM.Nodes.Add(rN);
                        
                    }

                    dM.Tag = delman;
                 
                    fN.Nodes.Add(dM);
                }

                fN.Tag = fman;
                treeView1.Nodes.Add(fN);
            }
            





        }











        static void OnNeedNormalizeInfo(object sender, EventArgs e)
        {
            //Pytania o uzupełnienie adresu

            //string csb;
            //AliasType a = new AliasType();
            //StopFileType  o = (StopFileType)sender;

            //if (!string.IsNullOrEmpty(o.MIEJSC_DORECZ))
            //{
            //    if (!string.IsNullOrEmpty(o.ULICA_DORECZ))
            //    {



            //        Console.WriteLine(o.MIEJSC_DORECZ);
            //        csb = Console.ReadLine();
            //        a.City = csb;

            //        Console.WriteLine(o.ULICA_DORECZ);
            //        csb = Console.ReadLine();
            //        a.Street = csb;

            //        a.PNA = o.PNA_DORECZ;

            //        a.Alias = o.MIEJSC_DORECZ + o.ULICA_DORECZ + o.PNA_DORECZ;

            //        Recognizer.AddAlias(a, o.MIEJSC_DORECZ, o.ULICA_DORECZ, o.PNA_DORECZ);

            //        o.NormalizeAdres(Recognizer);
            //    }
            //}

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (treeView1.SelectedNode.Level == 0)
            {
                 FerryManType o = (FerryManType)treeView1.SelectedNode.Tag;
                dg1.DataSource = o.ParcelList();
                dg2.DataSource = o.GetParcelSums();

                var c = (from cc in DicterType.GetFullCennik()
                        where cc.Przewoznik_KOD == o.KOD
                        select cc).ToList();

                dg3.DataSource = c;

                label1.Text = o.SumaStawkaDorecz().ToString();
            }

            if (treeView1.SelectedNode.Level == 1)
            {
                DeliveryManType o = (DeliveryManType)treeView1.SelectedNode.Tag;
                dg1.DataSource = o.ParcelList();
                dg2.DataSource = o.GetParcelSums();

                var c = (from cc in DicterType.GetFullCennik()
                         where cc.Przewoznik_KOD == o.PrzewoznikKOD
                         select cc).ToList();

                dg3.DataSource = c;
                label1.Text = o.SumaStawkaDorecz().ToString();
            }

            if (treeView1.SelectedNode.Level == 2)
            {
                DMrejonType o = (DMrejonType)treeView1.SelectedNode.Tag;
                dg1.DataSource = o.ParcelList();
                dg2.DataSource = o.GetParcelSums();
                dg3.DataSource = o.Cennik;

                label1.Text = o.SumaStawkaDorecz().ToString();
            }

            if (treeView1.SelectedNode.Level == 3)
            {
                StopType  o = (StopType)treeView1.SelectedNode.Tag;
                dg1.DataSource = o.ParcelList;
                dg2.DataSource = o.GetParcelSums();

                label1.Text = o.SumaStawkaDorecz().ToString();

            }

            if (treeView1.SelectedNode.Level == 4)
            {
                StopFileType o = (StopFileType)treeView1.SelectedNode.Tag;

                o.GetParcelGroup();

                dg1.DataSource = o;
                dg2.DataSource = o.PARCEL_GROUP;
            }
        }

        private void dg1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex<0)
            {
                return;
            }

            var o = dg1.Rows[e.RowIndex].Cells["NR_PRZ"].Value;


            foreach(var fm in L)
            {
                foreach (var dv in fm.DeliveryMans)
                {
                    foreach (var rm in dv.Rejons)
                    {
                        foreach (var sm in rm.STOPS)
                        {
                            foreach (var pr in sm.ParcelList)
                            {
                                if (pr.NR_PRZ == o.ToString())
                                {
                                    dg2.DataSource = pr.PARCEL_GROUP;
                                    break;
                                }
                            }
                        }
                    }
                }
            }


        }

        private void dg2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dg2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {


            DataGridViewTextBoxColumn cl = new DataGridViewTextBoxColumn();
            cl.HeaderText = "Quantity";
            cl.Name = cl.HeaderText;

            if (dg2.Columns.Contains(cl.HeaderText) == false)
            {
               dg2.Columns.Add(cl);
            }

      

            foreach (DataGridViewRow row in dg2.Rows)
            {

                double per = 0.0;
                double all =double.Parse( row.Cells["IsTrue"].Value.ToString()) ;
                double Real =double.Parse( row.Cells["isTrueDeliveredOnly"].Value.ToString());

                per = (Real / all) * 100;
             

                dg2.Rows[row.Index].Cells["Quantity"].Value=row.Cells["Quantity"].Value = Math.Round(per, 2);

                
            }

        }
    }
}
