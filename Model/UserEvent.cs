using System;
using System.Collections.Generic;

namespace Model
{
    public class UserEventDTO
    {
        public int OrgId { get; set; }
        public string IpAddr { get; set; }
        public int UserId { get; set; }
        public string Url { get; set; }
        public string UserAgent { get; set; }
        public DateTime? AuditTime { get; set; }
        public int BranchId { get; set; }
        public int EMRActionid { get; set; }
        public string EMRActionType { get; set; }
        public int Severity { get; set; }
        public string ResType { get; set; }
        public string ResDataJSON { get; set; }
        public string ResIds { get; set; }
        public int Sqlauditid { get; set; }

        //2nd option to compare complex type (override Equals and GetHashCode method in the class itself)
        public override bool Equals(object obj)
        {
            return OrgId == ((UserEventDTO)obj).OrgId;
        }

        public override int GetHashCode()
        {
            return OrgId.GetHashCode();
        }
    }

    /*1st way to compare complex type (create a new class which is inherited fron IEqualityComparer interface 
     * and override the Equals and GetHashCode method)
     */
    public class EventComparer : IEqualityComparer<UserEventDTO>
    {
        public bool Equals(UserEventDTO x, UserEventDTO y)
        {
            return x.OrgId == y.OrgId;
        }

        public int GetHashCode(UserEventDTO obj)
        {
            return obj.OrgId.GetHashCode();
        }
    }

    public class DataBaseDTO
    {
        public string DBServerName { get; set; }
        public string DBName { get; set; }
    }

    public class AuditTableDTO
    {
        public string TableName { get; set; }
        public string IdentityColumn { get; set; }
    }


    public class SearchParameters
    {
        public int Org_id { get; set; }
        public List<int> App_id { get; set; }
        public List<int> User_id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<int> Branch_ids { get; set; }
        public List<string> EmrActionType { get; set; }
        public List<ResTypes> ResTypesList { get; set; }
        public int IsSortOrderAsc { get; set; }
        public List<string> Sortby { get; set; }
        public int PageID { get; set; }
        public int IsAuditShow { get; set; }
        public SearchParameters()
        {
            App_id = new List<int>();
            User_id = new List<int>();
            Branch_ids = new List<int>();
            EmrActionType = new List<string>();
            ResTypesList = new List<ResTypes>();
            Sortby = new List<string>();
        }
    }

    public class ResTypes
    {
        public string ResType { get; set; }
        public Dictionary<string, List<string>> ResIds { get; set; }
        public ResTypes()
        {
            ResIds = new Dictionary<string, List<string>>();
        }
    }
}
