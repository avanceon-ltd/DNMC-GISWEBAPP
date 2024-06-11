using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppForm
{
    public partial class ECCCrew : System.Web.UI.Page
    {

        public static List<FMPMaster> _FM = new List<FMPMaster>();
        public static List<FMPCord> _FMPCord = new List<FMPCord>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _FM.Clear();
                _FMPCord.Clear();


                _FM.Add(new FMPMaster { FID = 1, Name = "ECC_FMP_N_Rep", Desc = "ECC_FMP_North_Rep", OwnerType = "FMP" });
                _FM.Add(new FMPMaster { FID = 2, Name = "ECC_FMP_S_Rep", Desc = "ECC_FMP_South_Rep", OwnerType = "FMP" });
                _FM.Add(new FMPMaster { FID = 3, Name = "ECC_FMP_W_Rep", Desc = "ECC_FMP_West_Rep", OwnerType = "FMP" });
                _FM.Add(new FMPMaster { FID = 4, Name = "CSE_SCH_ECC", Desc = "ECC_CustomerService_Rep", OwnerType = "CS" });
                _FM.Add(new FMPMaster { FID = 5, Name = "ECC_WS_Rep", Desc = "ECC_WorkShops_Rep", OwnerType = "Workshop" });
                _FM.Add(new FMPMaster { FID = 6, Name = "ECC_BLD_Rep", Desc = "ECC_Baladiya_Rep", OwnerType = "Baladiya" });
                _FM.Add(new FMPMaster { FID = 7, Name = "CSE_SCH_ECC", Desc = "ECC_Roads_Rep", OwnerType = "Roads" });
                _FM.Add(new FMPMaster { FID = 8, Name = "ECC_PA_Rep", Desc = "ECC_IAProject_Rep", OwnerType = "IAProject" });


                _FMPCord.Add(new FMPCord { CID = 1, Name = "FMP_ECC_ncord_1", Desc = "FMP North Zone Co-ordinator 1", FID_FK = "1" });
                _FMPCord.Add(new FMPCord { CID = 2, Name = "FMP_ECC_ncord_2", Desc = "FMP North Zone Co-ordinator 2", FID_FK = "1" });
                _FMPCord.Add(new FMPCord { CID = 3, Name = "FMP_ECC_ncord_3", Desc = "FMP North Zone Co-ordinator 3", FID_FK = "1" });
                _FMPCord.Add(new FMPCord { CID = 4, Name = "FMP_ECC_ncord_4", Desc = "FMP North Zone Co-ordinator 4", FID_FK = "1" });
                _FMPCord.Add(new FMPCord { CID = 5, Name = "FMP_ECC_ncord_5", Desc = "FMP North Zone Co-ordinator 5", FID_FK = "1" });
                _FMPCord.Add(new FMPCord { CID = 6, Name = "FMP_ECC_ncord_6", Desc = "FMP North Zone Co-ordinator 6", FID_FK = "1" });
                _FMPCord.Add(new FMPCord { CID = 7, Name = "FMP_ECC_ncord_7", Desc = "FMP North Zone Co-ordinator 7", FID_FK = "1" });
                _FMPCord.Add(new FMPCord { CID = 8, Name = "FMP_ECC_ncord_8", Desc = "FMP North Zone Co-ordinator 8", FID_FK = "1" });
                _FMPCord.Add(new FMPCord { CID = 9, Name = "FMP_ECC_ncord_9", Desc = "FMP North Zone Co-ordinator 9", FID_FK = "1" });
                _FMPCord.Add(new FMPCord { CID = 10, Name = "FMP_ECC_ncord_10", Desc = "FMP North Zone Co-ordinator 10", FID_FK = "1" });

                _FMPCord.Add(new FMPCord { CID = 11, Name = "FMP_ECC_sord_1", Desc = "FMP South Zone Co-ordinator 1", FID_FK = "2" });
                _FMPCord.Add(new FMPCord { CID = 12, Name = "FMP_ECC_scord_2", Desc = "FMP South Zone Co-ordinator 2", FID_FK = "2" });
                _FMPCord.Add(new FMPCord { CID = 13, Name = "FMP_ECC_scord_3", Desc = "FMP South Zone Co-ordinator 3", FID_FK = "2" });
                _FMPCord.Add(new FMPCord { CID = 14, Name = "FMP_ECC_scord_4", Desc = "FMP South Zone Co-ordinator 4", FID_FK = "2" });
                _FMPCord.Add(new FMPCord { CID = 15, Name = "FMP_ECC_scord_5", Desc = "FMP South Zone Co-ordinator 5", FID_FK = "2" });
                _FMPCord.Add(new FMPCord { CID = 16, Name = "FMP_ECC_scord_6", Desc = "FMP South Zone Co-ordinator 6", FID_FK = "2" });
                _FMPCord.Add(new FMPCord { CID = 17, Name = "FMP_ECC_scord_7", Desc = "FMP South Zone Co-ordinator 7", FID_FK = "2" });
                _FMPCord.Add(new FMPCord { CID = 18, Name = "FMP_ECC_scord_8", Desc = "FMP South Zone Co-ordinator 8", FID_FK = "2" });
                _FMPCord.Add(new FMPCord { CID = 19, Name = "FMP_ECC_scord_9", Desc = "FMP South Zone Co-ordinator 9", FID_FK = "2" });
                _FMPCord.Add(new FMPCord { CID = 20, Name = "FMP_ECC_scord_10", Desc = "FMP South Zone Co-ordinator 10", FID_FK = "2" });

                _FMPCord.Add(new FMPCord { CID = 21, Name = "FMP_ECC_wcord_1", Desc = "FMP West Zone Co-ordinator 1", FID_FK = "3" });
                _FMPCord.Add(new FMPCord { CID = 22, Name = "FMP_ECC_wcord_2", Desc = "FMP West Zone Co-ordinator 2", FID_FK = "3" });
                _FMPCord.Add(new FMPCord { CID = 23, Name = "FMP_ECC_wcord_3", Desc = "FMP West Zone Co-ordinator 3", FID_FK = "3" });
                _FMPCord.Add(new FMPCord { CID = 24, Name = "FMP_ECC_wcord_4", Desc = "FMP West Zone Co-ordinator 4", FID_FK = "3" });
                _FMPCord.Add(new FMPCord { CID = 25, Name = "FMP_ECC_wcord_5", Desc = "FMP West Zone Co-ordinator 5", FID_FK = "3" });
                _FMPCord.Add(new FMPCord { CID = 26, Name = "FMP_ECC_wcord_6", Desc = "FMP West Zone Co-ordinator 6", FID_FK = "3" });
                _FMPCord.Add(new FMPCord { CID = 27, Name = "FMP_ECC_wcord_7", Desc = "FMP West Zone Co-ordinator 7", FID_FK = "3" });
                _FMPCord.Add(new FMPCord { CID = 28, Name = "FMP_ECC_wcord_8", Desc = "FMP West Zone Co-ordinator 8", FID_FK = "3" });
                _FMPCord.Add(new FMPCord { CID = 29, Name = "FMP_ECC_wcord_9", Desc = "FMP South Zone Co-ordinator 9", FID_FK = "3" });
                _FMPCord.Add(new FMPCord { CID = 30, Name = "FMP_ECC_wcord_10", Desc = "FMP West Zone Co-ordinator 10", FID_FK = "3" });

                _FMPCord.Add(new FMPCord { CID = 31, Name = "WS_ECC_CORD_1", Desc = "WS Zone co-ordinator 1", FID_FK = "5" });
                _FMPCord.Add(new FMPCord { CID = 32, Name = "WS_ECC_CORD_2", Desc = "WS Zone co-ordinator 2", FID_FK = "5" });
                _FMPCord.Add(new FMPCord { CID = 33, Name = "WS_ECC_CORD_3", Desc = "WS Zone co-ordinator 3", FID_FK = "5" });
                _FMPCord.Add(new FMPCord { CID = 34, Name = "WS_ECC_CORD_4", Desc = "WS Zone co-ordinator 4", FID_FK = "5" });
                _FMPCord.Add(new FMPCord { CID = 35, Name = "WS_ECC_CORD_5", Desc = "WS Zone co-ordinator 5", FID_FK = "5" });
                _FMPCord.Add(new FMPCord { CID = 36, Name = "WS_ECC_CORD_6", Desc = "WS Zone co-ordinator 6", FID_FK = "5" });
                _FMPCord.Add(new FMPCord { CID = 37, Name = "WS_ECC_CORD_7", Desc = "WS Zone co-ordinator 7", FID_FK = "5" });
                _FMPCord.Add(new FMPCord { CID = 38, Name = "WS_ECC_CORD_8", Desc = "WS Zone co-ordinator 8", FID_FK = "5" });
                _FMPCord.Add(new FMPCord { CID = 39, Name = "WS_ECC_CORD_9", Desc = "WS Zone co-ordinator 9", FID_FK = "5" });
                _FMPCord.Add(new FMPCord { CID = 40, Name = "WS_ECC_CORD_10", Desc = "WS Zone co-ordinator 10", FID_FK = "5" });

                String OwnerType = "CS";

                if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    OwnerType = Request.QueryString["ID"];
                }

                List<FMPMaster> mylist = GetYourList(OwnerType, _FM);

                gvCustomers.DataSource = mylist;
                gvCustomers.DataBind();
            }
        }

        private List<FMPMaster> GetYourList(string v, List<FMPMaster> fM)
        {
            IEnumerable<FMPMaster> results = (IEnumerable<FMPMaster>)fM.FindAll(
                         delegate (FMPMaster p)
                         {
                             return p.OwnerType.StartsWith(v);
                         }
                      );
            return (List<FMPMaster>)results;
        }


        public class FMPMaster
        {
            public int FID { get; set; }
            public string Name { get; set; }
            public string Desc { get; set; }

            public string OwnerType { get; set; }

        }

        public class FMPCord
        {
            public int CID { get; set; }
            public string Name { get; set; }
            public string Desc { get; set; }

            public String FID_FK { get; set; }
        }



        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string customerId = gvCustomers.DataKeys[e.Row.RowIndex].Value.ToString();
                GridView gvOrders = e.Row.FindControl("gvOrders") as GridView;
                gvOrders.DataSource = GetDataCORD(customerId);
                gvOrders.DataBind();
            }
        }

        private object GetDataCORD(string customerId)
        {
            IEnumerable<FMPCord> results = (IEnumerable<FMPCord>)_FMPCord.FindAll(
                         delegate (FMPCord p)
                         {
                             return p.FID_FK.Contains(customerId);
                         }
                      );
            return (List<FMPCord>)results;
        }


    }
}