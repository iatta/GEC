using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetTimeProfileDetailForEditOutput
    {
		public CreateOrEditTimeProfileDetailDto TimeProfileDetail { get; set; }

		public string TimeProfileDescriptionAr { get; set;}

		public string ShiftNameAr { get; set;}


    }
}