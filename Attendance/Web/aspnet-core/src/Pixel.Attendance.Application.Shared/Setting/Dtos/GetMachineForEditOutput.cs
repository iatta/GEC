﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Setting.Dtos
{
    public class GetMachineForEditOutput
    {
		public CreateOrEditMachineDto Machine { get; set; }

		public string OrganizationUnitDisplayName { get; set;}


    }
}