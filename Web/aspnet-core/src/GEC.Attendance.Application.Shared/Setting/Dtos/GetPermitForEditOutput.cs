using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Setting.Dtos
{
    public class GetPermitForEditOutput
    {
		public CreateOrEditPermitDto Permit { get; set; }
        public List<AssignedTypesOfPermitDto> AssignedTypesOfPermit { get; set; }


    }
}