using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using DF_WebModule.Models;

namespace DF_WebModule.Classes
{
    public class clsCarePad
    {
        #region vatiables defined in the class
        private static int? _UserId = null;
        private static string _UserName = "";
        private static string _DocumentAllow = "";
        private static List<MenuModel> _menulis = new List<MenuModel>();

        #endregion
        
        public static class DoctorRecord
        {
            public static string Title;
            public static string Forename;
            public static string Surname;
        }
        public static List<MenuModel> MenuList
        {
            get { return _menulis; }
            set { _menulis = value; }
        }
        public static List<string> Titles()
        {
            List<string> titles = new List<string>();
            titles.Add("Mr");
            titles.Add("Mrs");
            titles.Add("Miss");
            titles.Add("Dr");
            titles.Add("Prof");
            titles.Add("Other");

            return titles;
        }

        public enum EMIS_API_REGIONS
        {
            SYML = 0,
            ENGLAND = 1, //England, Scotland, Isle Of Man
            WALES = 2,
            JERSEY = 3,
            N_IRELAND = 4,
            EGTON = 5
        }

        public enum EMIS_ACCESS_MODE
        {
            LOCAL = 1, PONS = 2, SERVER = 3
        }

        public enum PRODUCT_VERSION
        {
            PUI = 1, PAT = 2, FULL = 3, LOC = 4
        }

        public enum PRODUCT_API
        {
            EMIS = 1, INPS = 2
        }

        #region Properties
        public static PRODUCT_VERSION LICENSED_VERSION { get; set; } = 0;

        private static string _ProgramDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\SymlConnect";

        public static string ProgramDataFolder
        {
            get { return _ProgramDataFolder; }
            set { _ProgramDataFolder = value; }
        }

        private static EMIS_API_REGIONS _Emis_API_Region;

        public static EMIS_API_REGIONS EMIS_API_REGION
        {
            get { return _Emis_API_Region; }
            set { _Emis_API_Region = value; }
        }
        private static int _OrganizationId;
        public static int OrganizationId
        { get { return _OrganizationId; }
            set { _OrganizationId = value; }
        }

        private static String _OrgID;

        public static String OrgID
        {
            get { return _OrgID; }
            set { _OrgID = value; }
        }

        private static EMIS_ACCESS_MODE _EMAccessMode;

        public static EMIS_ACCESS_MODE EMAccessMode
        {
            get { return _EMAccessMode; }
            set { _EMAccessMode = value; }
        }

        private static String _EMISAccess;
        // Local, Pons, or API
        public static String EMISAccess
        {
            get { return _EMISAccess; }
            set { _EMISAccess = value; }
        }
       
        private static String _Emis_IP;

        public static String Emis_IP
        {
            get { return _Emis_IP; }
            set { _Emis_IP = value; }
        }

     
        public static int? UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        public static string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }      

        #endregion

        public static void WriteLog(string record)
        {

            ////string LogFile = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\UserDetails\\log.txt";
            //string LogFile = Path.Combine("C:\\Program Files (x86)\\IIS Express\\Configs\\log.txt");
            ////string LogFile = Path.Combine(clsCarePad.DataPath, "Configs\\" + DateTime.UtcNow.Date +  "log.txt");

            //using (StreamWriter w = File.AppendText(LogFile))
            //{
            //    Log(record, w);
            //}
        }

        public static string GetHostAddress()
        {
            switch (EMIS_API_REGION)
            {
                case EMIS_API_REGIONS.SYML:
                    return "10.207.114.225";
                    //return "10.207.114.225";
                //England, Scotland, Isle Of Man
                case EMIS_API_REGIONS.ENGLAND:
                    return "webinterop.spine.emis.thirdparty.nhs.uk";

                case EMIS_API_REGIONS.WALES:
                    return "webinterop.cymru.nhs.uk";

                case EMIS_API_REGIONS.JERSEY:
                    return "api.jersey.emishosting.com";

                case EMIS_API_REGIONS.N_IRELAND:
                    return "NI_Interop.emishealth.com";

                case EMIS_API_REGIONS.EGTON:
                    return "192.168.174.36";
            }

            return null;
        }

        public static string GetSupplierID()
        {
            switch (EMIS_API_REGION)
            {
                case EMIS_API_REGIONS.SYML:
                    return "449eb24c-cf41-4b55-8745-1a25b3c9ddcb";
                //England, Scotland, Isle Of Man
                case EMIS_API_REGIONS.ENGLAND:
                    return "bff0fe87-1e33-49ab-913a-2675790bba53";

                case EMIS_API_REGIONS.WALES:
                    return "bff0fe87-1e33-49ab-913a-2675790bba53";

                case EMIS_API_REGIONS.JERSEY:
                    return "bff0fe87-1e33-49ab-913a-2675790bba53";

                case EMIS_API_REGIONS.N_IRELAND:
                    return "bff0fe87-1e33-49ab-913a-2675790bba53";

                case EMIS_API_REGIONS.EGTON:
                    //return "449eb24c-cf41-4b55-8745-1a25b3c9ddcb";
                    return "bff0fe87-1e33-49ab-913a-2675790bba53";
            }

            return null;
        }

        public string ActivatePartnerProduct(string SUPPLIERID, string EMISIP, string DBNAME)
        {
            string sRet = "Error: not started";
            try
            {
                string ExeName = "Pons.exe";


                String args = "";
                args += "IPAddress=" + EMISIP;
                args += "|DBName=" + DBNAME;
                args += "|SupplierID=" + SUPPLIERID;

                sRet = ExecExternal(ExeName, "INITIALIZE", args);
            }
            catch (Exception ex)
            {
                sRet = "Error: " + ex.InnerException.ToString();
            }
            return sRet;
        }

        private String ExecExternal(String ExeName, String Command, String Args = "")
        {

            string exefile = @"C:\_Data\Deploy\pONS\" + ExeName;
            exefile = (Path.Combine(@"C:\\ProgramData\\Symlconnect\\External", ExeName));
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = exefile;
            p.StartInfo.Arguments = Command + " " + Args;

            try
            {
                p.Start();
            }
            catch (Exception ex)
            {
                return "Error: exception thrown in ExecExternal: " + ex.Message + "for ExeName:" + exefile + " Command:" + Command + " ARGS: " + Args;
            }
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }

        public static string GetSNOMEDMap(string ReadCode)
        {
            //Term = "";
            string sRet = "";
            //string SQLiteDBFile;
            //String SQLiteConnectionString;
            //String SQLiteQuery;
            //SQLiteConnection SQLiteConn;
            //SQLiteCommand SQLiteComnd;
            //SQLiteDataAdapter SQLiteAdapter;
            //SQLiteCommandBuilder SQLiteBuilder;

            //try
            //{
            //    ReadCode = ReadCode.Replace(".", "");

            //    SQLiteDBFile = @System.Configuration.ConfigurationManager.AppSettings.Get("SQLiteDBFile");
            //    SQLiteQuery = "SELECT SNOMED FROM [CodeMapping] WHERE ReadCode = '" + ReadCode + "'";
            //    String connStr = "Data Source=";
            //    connStr += SQLiteDBFile;
            //    connStr += "; Version=3;";

            //    SQLiteConnectionString = connStr;

            //    SQLiteConn = new SQLiteConnection(SQLiteConnectionString);
            //    SQLiteComnd = new SQLiteCommand(SQLiteQuery, SQLiteConn);
            //    SQLiteAdapter = new SQLiteDataAdapter(SQLiteComnd);
            //    SQLiteBuilder = new SQLiteCommandBuilder(SQLiteAdapter);
            //    DataSet ds = new DataSet("MainDataSet");
            //    DataSet tempDataSet = new DataSet("TempDataSet");

            //    SQLiteAdapter.Fill(ds);

            //    if (ds.Tables.Count > 0)
            //    {
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            sRet = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    sRet = ex.InnerException.ToString();
            //}

            return sRet;
        }

        //public static string GetSNOMEDMap(string ReadCode)
        //{
        //    string sRet = "65568007";
        //    try
        //    {
        //        switch (ReadCode)
        //        {
        //            case "137P.":
        //                sRet = "108938018";
        //                //sRet = "254063019";
        //                break;
        //            case "137S.":
        //                sRet = "649841000006110";
        //                break;
        //            case "137K.":
        //                sRet = "754761000000119";
        //                break;
        //            case "229":
        //                sRet = "253669010";
        //                break;
        //            case "22A..":
        //                sRet = "253677014";
        //                break;
        //            case "22K..":
        //                sRet = "100716012";
        //                break;
        //            case "44P..":
        //                sRet = "150921000006118";
        //                break;
        //            case "46U..":
        //                sRet = "185189016";
        //                break;
        //            case "388f.": //PHQ-9
        //                sRet = "303531000000114";
        //                break; 
        //            case "8H77.": //PhysioTherapy
        //                sRet = "451780012";
        //                break;
        //            case "8B61.": //CPAMS
        //                sRet = "2617874015"; //No Match
        //                break;
        //            case "8H7A.": //mental- health
        //                sRet = "283707011"; //No Match
        //                break;
        //            case "66R1":
        //                sRet = "177771000006116"; //No Match
        //                break;
        //            case "3AD3":
        //                sRet = "2160042019"; //No Match
        //                break;
        //            case "388V":
        //                sRet = "2534185019"; //No Match
        //                break;
        //            case "8H7i.":
        //                sRet = "1489355012";
        //                break;
        //            case "9Ng7.":
        //                sRet = "1175491000000110";
        //                break;
        //            case "242":
        //                sRet = "254020017";
        //                break;
        //            case "246A.":
        //                sRet = "619931000006119";
        //                break;
        //            case "2469.":
        //                sRet = "114311000006111";
        //                break;
        //            case "246":
        //                sRet = "254063019";
        //                break;
        //            default:
        //                break;
        //        }

        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return sRet;
        //}

        private static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");

        }


    }
    class ComboBoxItem
    {
        string displayValue;
        string hiddenValue;
        string hiddenType;

        //Constructor
        public ComboBoxItem(string d, string h, string t)
        {
            displayValue = d;
            hiddenValue = h;
            hiddenType = t;
        }

        //Accessor
        public string HiddenValue
        {
            get
            {
                return hiddenValue;
            }
        }

        //Accessor
        public string HiddenType
        {
            get
            {
                return hiddenType;
            }
        }

        //Override ToString method
        public override string ToString()
        {
            return displayValue;
        }
    }
}
