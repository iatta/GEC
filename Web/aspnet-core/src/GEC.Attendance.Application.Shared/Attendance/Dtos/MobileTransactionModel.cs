using System;
using System.Collections.Generic;
using System.Text;

namespace GEC.Attendance.Attendance.Dtos
{
    public class MobileTransactionInputModel
    {
        public int LanguaugeId { get; set; }
    }

    public class MobileTransactionOutputModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }

    public class CheckEmpLocationInputModel : MobileTransactionInputModel
    {
        public string CivilID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class CheckEmpLocationOutputModel : MobileTransactionOutputModel
    {
        public string SiteName { get; set; }
        public int SiteID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

    }

    public class InsertTransactionInputModel : MobileTransactionInputModel
    {
        public int UserId { get; set; }
        public string CivilId { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string ZoomAuditImage { get; set; }
        public string Type { get; set; }
        public bool TransStatus { get; set; }
        public string EmpCode { get; set; }
    }

    public class InsertTransactionOutputModel : MobileTransactionOutputModel
    {
        public string date { get; set; }
        public string Time { get; set; }
    }

    public class ReportInputModel : MobileTransactionInputModel
    {
        public string CivilId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
    public class LastTransactionInputModel : MobileTransactionInputModel
    {
        public string CivilId { get; set; }

    }
    public class TransEntityModel
    {
        public DateTime TransDate { get; set; }
        public List<ReportEntityModel> Transactions { get; set; }
    }
    public class ReportOutputModel : MobileTransactionOutputModel
    {
        public List<TransEntityModel> Transactions { get; set; }
    }
    public class LastTransOutputModel : MobileTransactionOutputModel
    {
        public List<ReportEntityModel> Transactions { get; set; }
    }
    public class ReportEntityModel
    {
        public string EmpCode { get; set; }

        public string MachineID { get; set; }

        public bool TransStatus { get; set; }

        public DateTime TransDate { get; set; }

        public string TransType { get; set; }

        public string CivilId { get; set; }

        //public string Image { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int SiteId { get; set; }

        public string SiteName { get; set; }
        public string SiteAdress { get; set; }

    }
    public class GetEmpLocationsOutputModel : MobileTransactionOutputModel
    {
        public List<LocationModel> locations { get; set; }
    }
    public class LocationModel
    {
        public int Siteid { get; set; }
        public string SiteName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<LocationCerdentialModel> cerdentials { get; set; }
    }
    public class LocationCerdentialModel
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }
}
