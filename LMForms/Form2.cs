using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMForms
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            MainMap.MapProvider = GMapProviders.OpenStreetMap;
            MainMap.Position = new PointLatLng(51.135468, 17.025839); //	

            MainMap.MinZoom = 0;
            MainMap.MaxZoom = 24;
            MainMap.Zoom = 9;
        }


        List<RoutePos> L;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            if(opf.ShowDialog() == DialogResult.OK )
            {
                L = new List<RoutePos>();


                MainMap.MapProvider = GMapProviders.OpenStreet4UMap;

                List<string> plik = System.IO.File.ReadAllLines(opf.FileName).ToList();


                foreach(string line in plik)
                {

                    List<string> lp = line.Split(';').ToList();

                    RoutePos o = new RoutePos();
                    o.Trasa = lp[0];
                    o.Nrpunktu  = lp[1];
                    o.Dataiczas  = lp[2];
                    o.Czasodostatniejoperacjipocztowej  = lp[3];
                    o.Operacjapocztowa = lp[4];
                    o.Informacjadodatkowa  = lp[5];
                    o.Długośćgeogr = lp[6];
                    o.Szerokośćgeogr  = lp[7];
                    o.Operacja  = lp[8];
                    L.Add(o);
                }



                var dist = (from o in L
                            select o.Trasa).Distinct().ToList();

                listBox1.Items.Clear();

                foreach (var s in dist)
                {
                    listBox1.Items.Add(s);
                }



            }
        }



        class RoutePos
        {
            public string Trasa { get; set; }
            public string Nrpunktu { get; set; }
            public string Dataiczas { get; set; }
            public string Czasodostatniejoperacjipocztowej { get; set; }
            public string Operacjapocztowa { get; set; }
            public string Informacjadodatkowa { get; set; }
            public string Długośćgeogr { get; set; }
            public string Szerokośćgeogr { get; set; }
            public string Operacja { get; set; }


            public PointLatLng Point()
            {
                double szer = double.Parse(Szerokośćgeogr.Replace(".", ","));
                double dlu = double.Parse(Długośćgeogr.Replace(".", ","));

                PointLatLng p = new PointLatLng(szer,dlu);
                return p;
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

          //

            string trasaid = listBox1.SelectedItem.ToString();


            List<RoutePos> ls = (from o in L
                                 where o.Trasa == trasaid && string.IsNullOrEmpty(o.Szerokośćgeogr )==false 
                                 select o).ToList();

            MainMap.Overlays.Clear();
            int i = 0;
            int x = 0;
            GMapOverlay markersOverlay = new GMapOverlay("markers");

            //GMapOverlay overlayRoute = new GMapOverlay("overlayRoute");
            //foreach (RoutePos pos in ls)
            //{
            //    if (x + 1 < ls.Count - 1)
            //    {
            //       var path = GMap.NET.MapProviders.OpenStreetMapProvider.Instance.GetRoute(ls[x].Point(), ls[x+1].Point(), false, false, 15);
            //        if(path != null)
            //        {
            //              GMapRoute route = new GMapRoute(path.Points, "My route");

                   
            //                        overlayRoute.Routes.Add(route);
            //        }
          
            //        x += 1;
            //    }
  
            //}
            //MainMap.Overlays.Add(overlayRoute);


            foreach (RoutePos r in ls)
            {

           

                if (!string.IsNullOrEmpty(r.Długośćgeogr))
                {
                double szer = double.Parse(r.Szerokośćgeogr.Replace(".",",") );
                double dlu = double.Parse(r.Długośćgeogr.Replace(".", ","));


                GMarkerGoogle marker1 = new GMarkerGoogle(new PointLatLng(szer, dlu), GMarkerGoogleType.green);
                    marker1.ToolTipText = r.Operacjapocztowa + " " + r.Informacjadodatkowa;
                    
                    bool ok = true;
                    foreach (var marker in markersOverlay.Markers)
                    {
                        if(marker.Position.Lat==marker1.Position.Lat)
                        {
                            if(marker.Position.Lng  == marker1.Position.Lng)
                            {
                                
                                marker.ToolTipText += "\n" + marker1.ToolTipText;
                                ok = false;
                            }
                        }
                    }

                    if (ok == true)
                    {
         markersOverlay.Markers.Add(marker1);
                    }
       
                



                    i += 1;
                    this.Text = i.ToString();
                }




            }

                MainMap.Overlays.Add(markersOverlay);

        }
    }
}
