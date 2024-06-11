using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using WebAppForm.BusinessEntity;
using WebAppForm.ExtensionEntity;

namespace WebAppForm.DAL
{
    public class ServicesLogs
    {
        ILog log = log4net.LogManager.GetLogger(typeof(ServicesLogs));
        private SqlConnection con = new SqlConnection();
        private SqlCommand cmd = new SqlCommand();
        internal Tuple<bool, int> InsertServiceRequestAudit(AuditServiceRequest auditSR, int UpdatedId = 0)
        {
            bool qresult = false;
            int ID = 0;

            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["FuseConnectionString"].ToString();

                con.ConnectionString = conString;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "USP_UPDATE_INSERT_SERVICE_REQUEST_AUDIT";

                if (UpdatedId == 0)
                {
                    cmd.Parameters.AddWithValue("@requestPayload", auditSR.RequestPayLoad);
                    cmd.Parameters.AddWithValue("@XmlRequest", auditSR.XmlRequest);
                    cmd.Parameters.AddWithValue("@ExternalCRMsrId", auditSR.ExternalCRMsrId);
                    cmd.Parameters.AddWithValue("@ServiceOperation", auditSR.ServiceOperation);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@responsePayload", auditSR.ResponsePayload);
                    cmd.Parameters.AddWithValue("@IncidentUID", auditSR.IncidentUID);
                    cmd.Parameters.AddWithValue("@XmlResponse", auditSR.XmlResponse);
                    cmd.Parameters.AddWithValue("@ExternalCRMsrId", auditSR.ExternalCRMsrId);
                    cmd.Parameters.AddWithValue("@UpdateId", UpdatedId);
                    cmd.Parameters.AddWithValue("@ServiceOperation", auditSR.ServiceOperation);

                }
                DataTable dt = cmd.ExecuteDataTable();
                foreach (DataRow dr in dt.Rows)
                {
                    qresult = dr["Result"] == DBNull.Value ? qresult : Convert.ToBoolean(dr["Result"]);
                    ID = dr["ID"] == DBNull.Value ? ID : Convert.ToInt32(dr["ID"]);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                Dispose();
            }
            var result = new Tuple<bool, int>(qresult, ID);
            return result;
        }

        internal Tuple<bool, int> InsertWORequestAudit(AuditWORequest auditWO, int UpdatedId = 0)
        {
            bool qresult = false;
            int ID = 0;

            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["FuseConnectionString"].ToString();

                con.ConnectionString = conString;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "USP_UPDATE_INSERT_WORK_ORDER_REQUEST_AUDIT";

                if (UpdatedId == 0)
                {
                    cmd.Parameters.AddWithValue("@requestPayload", auditWO.RequestPayLoad);
                    cmd.Parameters.AddWithValue("@XmlRequest", auditWO.XmlRequest);
                    cmd.Parameters.AddWithValue("@ExternalRefID", auditWO.ExternalRefID);
                    cmd.Parameters.AddWithValue("@ServiceOperation", auditWO.ServiceOperation);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@responsePayload", auditWO.ResponsePayload);
                    cmd.Parameters.AddWithValue("@uniqueId_WONumber", auditWO.UniqueId_WONumber);
                    cmd.Parameters.AddWithValue("@XmlResponse", auditWO.XmlResponse);
                    cmd.Parameters.AddWithValue("@ExternalRefID", auditWO.ExternalRefID);
                    cmd.Parameters.AddWithValue("@ServiceOperation", auditWO.ServiceOperation);
                    cmd.Parameters.AddWithValue("@UpdateId", UpdatedId);

                }
                DataTable dt = cmd.ExecuteDataTable();
                foreach (DataRow dr in dt.Rows)
                {
                    qresult = dr["Result"] == DBNull.Value ? qresult : Convert.ToBoolean(dr["Result"]);
                    ID = dr["ID"] == DBNull.Value ? ID : Convert.ToInt32(dr["ID"]);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                Dispose();
            }
            var result = new Tuple<bool, int>(qresult, ID);
            return result;
        }

        internal string GetUniqueExternalRefID(string request)
        {
            string result = "";

            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["FuseConnectionString"].ToString();

                con.ConnectionString = conString;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "USP_GET_RANDOM_VALUES";
                cmd.Parameters.AddWithValue("@Request", request);
                DataTable dt = cmd.ExecuteDataTable();
                foreach (DataRow dr in dt.Rows)
                {
                    result = dr["ExternalID"] == DBNull.Value ? result : Convert.ToString(dr["ExternalID"]);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                Dispose();
            }
            return result;
        }

        internal List<ClassSpecification> GetClassSpecification(string ASSETATTRID = null)
        {
            List<ClassSpecification> classSpecificationsLst = new List<ClassSpecification>();

            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["ODWConnectionString"].ToString();

                con.ConnectionString = conString;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "EAMS.USP_GET_CLASS_SPECIFICATION";
                cmd.Parameters.AddWithValue("@ASSETATTRID", ASSETATTRID);

                DataTable dt = cmd.ExecuteDataTable();
                foreach (DataRow dr in dt.Rows)
                {
                    ClassSpecification classSpecification = new ClassSpecification();

                    classSpecification.ALNValue = dr["ALNValue"] == DBNull.Value ? classSpecification.ALNValue : Convert.ToString(dr["ALNValue"]);
                    classSpecification.CLASSSPECID = dr["CLASSSPECID"] == DBNull.Value ? classSpecification.CLASSSPECID : Convert.ToString(dr["CLASSSPECID"]);
                    classSpecification.ClassStructureId = dr["ClassStructureId"] == DBNull.Value ? classSpecification.ClassStructureId : Convert.ToString(dr["ClassStructureId"]);
                    classSpecification.SEQ = dr["SEQ"] == DBNull.Value ? classSpecification.SEQ : Convert.ToString(dr["SEQ"]);
                    classSpecification.ASSETATTRID = dr["ASSETATTRID"] == DBNull.Value ? classSpecification.ASSETATTRID : Convert.ToString(dr["ASSETATTRID"]);
                    classSpecification.ASSETATTRIDODW = dr["ASSETATTRIDODW"] == DBNull.Value ? classSpecification.ASSETATTRIDODW : Convert.ToString(dr["ASSETATTRIDODW"]);

                    classSpecificationsLst.Add(classSpecification);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                Dispose();
            }
            return classSpecificationsLst;
        }

        internal Tuple<bool, string, string> AssignVehicle(string WONUM, string VEHNUM, string USER,string SPName)
        {
            string result = string.Empty;
            string message = string.Empty;
            bool success = false;

            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["ODW_STG_ConnectionString"].ToString();

                con.ConnectionString = conString;

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SPName;

                cmd.Parameters.AddWithValue("@WONUM", WONUM);
                cmd.Parameters.AddWithValue("@PlateNumber", VEHNUM);
                cmd.Parameters.AddWithValue("@USER", USER);
                cmd.Parameters.Add("@RESULT", SqlDbType.VarChar, 500);
                cmd.Parameters["@RESULT"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar, 500);
                cmd.Parameters["@Status"].Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                success = true;

                result = Convert.ToString(cmd.Parameters["@RESULT"].Value);
                message = Convert.ToString(cmd.Parameters["@Status"].Value);
                
            }
            catch (Exception ex)
            {
                log.Error("Assign Vehicle Error: " + ex.Message);
                result = ex.Message;
            }
            finally
            {
                Dispose();
            }
            var assignResult = new Tuple<bool, string, string>(success,result, message);
            return assignResult;
        }
            private void Dispose()
        {
            cmd.Dispose();
            con.Dispose();
            con.Close();
        }

    }
}