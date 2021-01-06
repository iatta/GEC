using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
   public class UserFlatDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? OrganizationUnitId { get; set; }

    }
}
