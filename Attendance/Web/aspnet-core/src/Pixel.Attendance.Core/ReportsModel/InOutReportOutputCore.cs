using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pixel.Attendance.ReportsModel
{

    public class InOutReportOutputCore
    {
        [Key]
        public int Id { get; set; }

        public DateTime? AttendanceDate { get; set; }

        public DateTime? AttendanceIn { get; set; }

        public string AttendanceInStr { get; set; }

        public long? AttendanceLateIn { get; set; }

        public DateTime? AttendanceOut { get; set; }

        public string AttendanceOutStr { get; set; }
        public long? AttendanceEarlyOut { get; set; }

        public bool? IsOff { get; set; }

        public bool? IsHoliday { get; set; }

        public bool? IsVacation { get; set; }

        public string VacationCode { get; set; }
        public int? IsAbsent { get; set; }

        public bool? IsPermissionOff { get; set; }

        public bool? IsShiftOff { get; set; }

        public long? TimeProfileId { get; set; }
        public long? EmpId { get; set; }

        public int? TotalHours { get; set; }

        public bool? MissedScan { get; set; }

        public string TotalHoursStr { get; set; }

        public long? ShiftNo { get; set; }

        public string UnitName { get; set; }


        public string UserName { get; set; }

        public string UserFullName { get; set; }

        public string EmpName { get; set; }

        public long? ShiftId { get; set; }

        public long? UnitId { get; set; }

        public int? TotalMin { get; set; }


        public long? TotalPerMin { get; set; }

        public int? ForgetOut { get; set; }

        public int? ForgetIn { get; set; }

        public long? OverTime { get; set; }
        public string ShiftCode { get; set; }
        public string CivilId { get; set; }

        public string JobTitleName { get; set; }

        public string MainDisplayName { get; set; }

        public long? MainUnitId { get; set; }

        public string LeaveTypeNameE { get; set; }
        public string LeaveTypeNameA { get; set; }

        public string PermitCode { get; set; }

        public string PermitCodeEn { get; set; }

        public string PermitIsDeducted { get; set; }

        public string OfficialTaskTypeNameAr { get; set; }

        public string Notes { get; set; }
        public string AttendanceOfficialTaskText { get; set; }

        public int? AbsentsCount { get; set; }

        public int? VacationsCount { get; set; }

        public int? NoInCount { get; set; }

        public int? NoOutCount { get; set; }

        public int? OffDaysCount { get; set; }

        public int? PermitCount { get; set; }

        public int? PermitTotal { get; set; }

        public string ShiftNameAr { get; set; }

        public string ShiftNameEn { get; set; }

        public string HolidayName { get; set; }
        public string FingerCode { get; set; }
        public string  Code { get; set; }
    }
}
