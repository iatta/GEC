using GEC.Attendance.Setting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace GEC.Attendance.Operations
{
	[Table("EmployeeOfficialTasks")]
    public class EmployeeOfficialTask : FullAuditedEntity 
    {
		public EmployeeOfficialTask()
		{
			OfficialTaskDetails = new HashSet<EmployeeOfficialTaskDetail>();
		}

		public virtual DateTime FromDate { get; set; }
		
		public virtual DateTime ToDate { get; set; }
		
		public virtual string Remarks { get; set; }
		
		public virtual string DescriptionAr { get; set; }
		
		public virtual string DescriptionEn { get; set; }
		

		public virtual int? OfficialTaskTypeId { get; set; }
		
        [ForeignKey("OfficialTaskTypeId")]
		public OfficialTaskType OfficialTaskTypeFk { get; set; }

		public virtual ICollection<EmployeeOfficialTaskDetail> OfficialTaskDetails { get; set; }

	}
}