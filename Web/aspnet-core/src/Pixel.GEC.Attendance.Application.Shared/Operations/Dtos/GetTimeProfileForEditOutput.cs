﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.GEC.Attendance.Operations.Dtos
{
    public class GetTimeProfileForEditOutput
    {
		public CreateOrEditTimeProfileDto TimeProfile { get; set; }

		public string UserName { get; set;}


    }
}