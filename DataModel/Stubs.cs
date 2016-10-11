using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using W8rtmGrid.Data.Sr26d;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace  W8rtmGrid.Data
{
    public sealed class Sr26Data
    {


        public static string htmlDataItem()
        {

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            //string tabTag = (string)loader.GetString("TableTag");
            string linfFmt = (string)loader.GetString("LineFmt");
            string table = (string)loader.GetString("Table");

            if (Sr26Data.NutDetail.Count <= 0)
                return null;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(table);

            string line = null;
            foreach (var t in Sr26Data.NutDetail)
            {
                line = string.Format(linfFmt, t.Nutr_Val, t.Units, t.Desc);
                sb.AppendLine(line);
            }

            sb.AppendLine("</table>");

            // Share recipe text
            var nutrients = sb.ToString();
            return nutrients;
        }

        /// <summary>
        /// make an HTML Table out of the comma & | separated Food Nutrients data.
        /// </summary>
        /// <param name="text">The Item Content</param>
        /// <returns>The 3 colmn HTML Table</returns>
        public static string Text2Html(string text)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            string linfFmt = (string)loader.GetString("LineFmt");
            string table = (string)loader.GetString("Table");

            string[] lines = text.Split('|');
            if (lines.Length <= 0)
                return null;

            string[] lst = null;


            StringBuilder nl = new StringBuilder();
            string line = null;
            nl.Append(table);


            foreach (string l in lines)
            {
                if (string.IsNullOrWhiteSpace(l) == true)
                    continue;
                lst = l.Split(',');
                if (lst.Length == 2)
                {
                    short nn = short.Parse(lst[0]);
                    NUTR_DEF nd = NUTR_DEFAry.array.First(n => n.Nutr_No == nn);
                    line = string.Format(linfFmt, lst[1], nd.Units, nd.NutrDesc);
                  
                    nl.Append(line);
                }

            }

            nl.Append("</table>");
            return nl.ToString();
        }

        public static ObservableCollection<NutVal> NutDetail
        {
            get { return _nutdet; }
            set { _nutdet = value; }
        }

        private static List<FOOD_DES> _fig;

        public static List<FOOD_DES> FIG
        {
            get { return  _fig; }
            set {  _fig = value; }
        }
        


        public static ObservableCollection<NutVal> _nutdet = new ObservableCollection<NutVal>();

      

        public static int Curr_NDB_No { get; set; }

       
    }
    public class FD_GROUP
    {
        public FD_GROUP() { }
        public System.Int16 FdGrp_CD { get; set; }
        public System.String FdGrp_Desc { get; set; }
        public FD_GROUP(System.Int16 _fdgrp_cd, System.String _fdgrp_desc)
        {
            FdGrp_CD = _fdgrp_cd;
            FdGrp_Desc = _fdgrp_desc;

        }
    }

    public class NUTR_DEF
    {
        public NUTR_DEF() { }
        public System.Int16 Nutr_No { get; set; }
        public System.String Units { get; set; }
        public System.String NutrDesc { get; set; }
        public System.String Num_Dec { get; set; }
        public NUTR_DEF(System.Int16 _nutr_no, System.String _units, System.String _nutrdesc, System.String _num_dec)
        {
            Nutr_No = _nutr_no;
            Units = _units;
            NutrDesc = _nutrdesc;
            Num_Dec = _num_dec;

        }
    }

    public class FOOD_DES
    {
        public FOOD_DES() { }
        public System.Int32 NDB_No { get; set; }
        public System.Int16 FdGrp_Cd { get; set; }
        public System.String Long_Desc { get; set; }
        public FOOD_DES(System.Int32 _ndb_no, System.Int16 _fdgrp_cd, System.String _long_desc)
        {
            NDB_No = _ndb_no;
            FdGrp_Cd = _fdgrp_cd;
            Long_Desc = _long_desc;

        }
    }

    public class NUT_DATA
    {
        public NUT_DATA() { }
        public System.Int32 NDB_No;
        public System.Int16 Nutr_No;
        public System.Single Nutr_Val;
        public NUT_DATA(System.Int32 _ndb_no, System.Int16 _nutr_no, System.Single _nutr_val)
        {
            NDB_No = _ndb_no;
            Nutr_No = _nutr_no;
            Nutr_Val = _nutr_val;

        }
        public NUT_DATA(string[] p)
        {
            int.TryParse(p[0], out NDB_No);
            short.TryParse(p[1], out Nutr_No);
            float.TryParse(p[2], out Nutr_Val);
        }
    }

    public class NutVal
    {
        private float _val;

        private string _units;
        private string _desc;// nutrient 

        public string Desc
        {
            get { return _desc; }
            set { _desc = value; }
        }

        public float Nutr_Val
        {
            get { return _val; }
            set { _val = value; }
        }


        public string Units
        {
            get { return _units; }
            set { _units = value; }
        }
    }

}

