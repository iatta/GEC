
using System;
using Abp.Application.Services.Dto;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class MachineDto : EntityDto
    {
		public string NameAr { get; set; }

		public string NameEn { get; set; }

		public string IpAddress { get; set; }

		public string SubNet { get; set; }

		public bool Status { get; set; }


		 public long? OrganizationUnitId { get; set; }

		 
    }
}