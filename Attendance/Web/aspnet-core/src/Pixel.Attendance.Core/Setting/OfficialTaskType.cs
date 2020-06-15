using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.Attendance.Setting
{
	[Table("OfficialTaskTypes")]
    public class OfficialTaskType : Entity 
    {

		public virtual string NameAr { get; set; }
		
		public virtual string NameEn { get; set; }
		
		public virtual bool TypeIn { get; set; }
		
		public virtual bool TypeOut { get; set; }
		
		public virtual bool TypeInOut { get; set; }
		

    }
}