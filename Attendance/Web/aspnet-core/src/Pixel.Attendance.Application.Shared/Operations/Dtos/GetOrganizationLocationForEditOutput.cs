using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetOrganizationLocationForEditOutput
    {
		public CreateOrEditOrganizationLocationDto OrganizationLocation { get; set; }

		public string OrganizationUnitDisplayName { get; set;}

		public string LocationTitleEn { get; set;}


    }
}