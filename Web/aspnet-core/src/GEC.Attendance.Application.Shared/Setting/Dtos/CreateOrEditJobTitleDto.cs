
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Setting.Dtos
{
    public class CreateOrEditJobTitleDto : EntityDto<int?>
    {

		public string NameAr { get; set; }
        public string NameEn { get; set; }



    }
}