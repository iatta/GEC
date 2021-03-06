﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetProjectForEditOutput
    {
		public CreateOrEditProjectDto Project { get; set; }

		public string UserName { get; set;}

        public string AssistantUserName { get; set; }
        public string LocationTitleEn { get; set;}

		public string OrganizationUnitDisplayName { get; set;}


    }
}