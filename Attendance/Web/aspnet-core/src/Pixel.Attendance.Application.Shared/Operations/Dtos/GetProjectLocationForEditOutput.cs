using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetProjectLocationForEditOutput
    {
		public CreateOrEditProjectLocationDto ProjectLocation { get; set; }

		public string ProjectNameEn { get; set;}

		public string LocationTitleEn { get; set;}


    }
}