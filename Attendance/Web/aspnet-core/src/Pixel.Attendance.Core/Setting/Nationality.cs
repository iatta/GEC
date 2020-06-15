using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Pixel.Attendance.Setting
{
	[Table("Nationalities")]
    public class Nationality : Entity 
    {

		public virtual string NameAr { get; set; }
		
		public virtual string NameEn { get; set; }
		

    }
}