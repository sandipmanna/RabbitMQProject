using Model;
using MQPublisher.Services;
using SenderApplication.BusinessLogic;
using SenderApplication.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Util;

namespace SenderApplication
{
    public partial class Home : System.Web.UI.Page
    {
        //public IEventPublisher eventPublisher { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlTable.DataSourceID = "xmlTableList";
                ddlTable.DataTextField = "Text";
                ddlTable.DataValueField = "value";
                ddlTable.DataBind();
                ddlTable.Items.Insert(0, new ListItem("Select", "Select"));
            }
        }

        protected void BtnAudit_Click(object sender, EventArgs e)
        {
            try
            {
                string TableName = ddlTable.SelectedValue;
                string DBServerName = Utility.GetConfigParam<string>("DBServerName");
                string DBName = Utility.GetConfigParam<string>("DBName");
                List<UserEventDTO> auditRecordList = new List<UserEventDTO>();
                //AccessLayer.FetchDataFromSQL(DBServerName, DBName, TableName, out auditRecordList);
                //auditRecordList.Take(2);
                //string out_Query = "{'AuditObject':[{'Org_id':1,'userid':1,'AuditTime':'2019 - 11 - 15T18: 33:23.403','branchID':null,'EMRActiontype':'Added Document'," +
                //    "'EMRActionid':25,'severity':1,'restype':'Payments','resids':'PaymentMasterID = 105722 / PaymentNo = / UserType = / ClaimID = 0/ ClaimNo = / IsAuditNotShow = 0'," +
                //    "'resdata_json':{'data':'{\"Audittext\":\"Document test.txt Added. \", \"Page\":\"\", \"SQLAuditID\":\"0\", \"PayerType\":\"\", \"payerID\":\"0\", \"EpisodeID\":\"0\", " +
                //    "\"IsSingleActivity\":\"1\", \"IsGroupActivity\":\"1\", \"OldBalance\":\"\", \"LegacyAccountAmount\":\"\", \"NewBalance\":\"0.00\", " +
                //    "\"PaymentAmount\":\"905.00\", \"UnappliedCreditBalance\":\"\", \"ProcedureName\":\"_CL_AddNewDocument\", \"ParentActivityName\":\"AddDocument\"}'}," +
                //    "'restaction':1}]}";

                string out_Query = @"{""AuditObject"":[{""Org_id"":1,""userid"":1,""AuditTime"":""2019 - 11 - 15T18: 33:23.403"",""branchID"":null, ""EMRActiontype"":""Added Document"", ""EMRActionid"":25,""severity"":1,""restype"":""Payments"",""resids"":""PaymentMasterID = 105722 / PaymentNo = / UserType = / ClaimID = 0/ ClaimNo = / IsAuditNotShow = 0"",""resdata_json"":{""Audittext"":""Document test.txt Added. "", ""Page"":"""", ""SQLAuditID"":""0"", ""PayerType"":"""", ""payerID"":""0"", ""EpisodeID"":""0"", ""IsSingleActivity"":""1"", ""IsGroupActivity"":""1"", ""OldBalance"":"""", ""LegacyAccountAmount"":"""", ""NewBalance"":""0.00"", ""PaymentAmount"":""905.00"", ""UnappliedCreditBalance"":"""", ""ProcedureName"":""_CL_AddNewDocument"", ""ParentActivityName"":""AddDocument""},""restaction"":1}]}";



                //string out_Query = @"{""AuditObject"": {""FName"":""Sandip"", ""OrgID"":1}}";

                //if (auditRecordList.Count > 0)
                //    InsertQueryBuilder(auditRecordList, out out_Query);
                //var HAA = auditRecordList.Select(x => new { orgid = x.OrgId }).FirstOrDefault();
                if (out_Query != "")
                {
                    IDataChangesPublisher dataChangesPublisher = new DataChangesPublisher();
                    dataChangesPublisher.PublishAuditLogData(1, 12, 12, out_Query);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
                Utility.WriteExceptionLog(ex);
            }
        }

        public static void InsertQueryBuilder(List<UserEventDTO> RecordList, out string Query)
        {
            Query = string.Empty;
            try
            {
                int TotalRecord = 0;
                TotalRecord = RecordList.Count;
                Console.WriteLine("Count: {0}", TotalRecord);
                var orgid = RecordList.Select(x => new { org_id = x.OrgId }).FirstOrDefault();

                Query = "INSERT INTO logs.user_events Partition(org_id =" + orgid + ") VALUES ";
                StringBuilder finalstr;
                finalstr = new StringBuilder(Query);
                foreach (UserEventDTO item in RecordList)
                {
                    finalstr.Append("('" + item.IpAddr + "', " + item.UserId + ", '" + item.Url + "', '" + item.UserAgent + "', " +
                        "'" + item.AuditTime + "', '" + item.EMRActionType + "', " + item.EMRActionid + "," + item.Severity + ", '" +
                        item.ResType + "', '" + item.ResDataJSON + "', str_to_map('" + item.ResIds + "','/','=')), ");
                }
                if (finalstr.Length != 0)
                {
                    Query = finalstr.ToString().Trim().TrimEnd(',');
                    //Utility.WriteQuery(insertQuery, i, org.org_id, listBasedOnOrgid.Count());
                }
                //type.InsertData(insertQuery, out insertedRow);
                //new Publish().PublishData(insertQuery, AuditIDs.ToString(), DBServerName, DBName, tablename, identitycolumn);
                //Utility.WritemessageToFile(string.Format("Total time taken to insert {0} records is {1}", insertedList.Count(), sw.ElapsedMilliseconds));
            }
            catch (Exception ex)
            {
                Utility.WriteExceptionLog(ex);
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            try
            {
                string users = "1";
                SearchParameters searchParameters = new SearchParameters();
                searchParameters.Org_id = 5;
                searchParameters.App_id = new List<int>() { 1, 2 };
                foreach (string item in users.ToString().Split(','))
                {
                    searchParameters.User_id.Add(int.Parse(item));
                }
                searchParameters.StartDate = "1/1/2019";
                searchParameters.EndDate = "1/31/2019";
                searchParameters.Branch_ids = new List<int>() { 5, 6, 10 };
                searchParameters.EmrActionType = new List<string>() { "Added", "Modified", "Deleted" };
                searchParameters.Sortby = new List<string>() {"ClaimID","AuditDate" };
                searchParameters.ResTypesList = new List<ResTypes>() {
                    new ResTypes(){
                        ResType = "Claim",
                        ResIds = new Dictionary<string, List<string>>
                        {
                            { "ClaimID",new List<string>(){ "123", "423" } },
                            { "ClaimAdjustmentID",new List<string>(){ "5346", "9586" } },
                            { "LineItemID",new List<string>(){ "87398", "793875" } }
                        }
                    },

                    new ResTypes(){
                        ResType = "Adjustment",
                        ResIds = new Dictionary<string, List<string>>
                        {

                            { "ClaimID",new List<string>(){ "435", "978" } },
                            { "ClaimAdjustmentID",new List<string>(){ "1231", "4646" } },
                            { "LineItemID",new List<string>(){ "078986", "121232" } }
                        }
                    }
                };
                searchParameters.IsAuditShow = 1;
                searchParameters.IsSortOrderAsc = 1;
                searchParameters.PageID = 09;

                string parameter = Newtonsoft.Json.JsonConvert.SerializeObject(searchParameters);

                string exceptionMessage = string.Empty;
                dynamic actualOutputText = null;
                string urlWebAPI = "http://192.168.1.228:8080/", methodSignature = "audits";
                ICSharpPythonRESTfulAPI csharpPythonRESTfulAPI = new CSharpPythonRestfulAPI(urlWebAPI, methodSignature);
                bool retVal = csharpPythonRESTfulAPI.CSharpPythonRestfulApiSimpleTest(parameter, out actualOutputText, out exceptionMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}