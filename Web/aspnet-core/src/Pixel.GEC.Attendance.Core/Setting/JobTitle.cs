using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.GEC.Attendance.Setting
{
	[Table("JobTitles")]
    public class JobTitle : Entity 
    {

		public virtual string NameAr { get; set; }
        public virtual string NameEn { get; set; }


    }
}