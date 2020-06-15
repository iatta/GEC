using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetPermitForEditOutput
    {
		public CreateOrEditPermitDto Permit { get; set; }
        public List<AssignedTypesOfPermitDto> AssignedTypesOfPermit { get; set; }


    }
}