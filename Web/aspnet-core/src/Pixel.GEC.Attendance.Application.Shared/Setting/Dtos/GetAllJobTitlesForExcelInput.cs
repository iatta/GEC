﻿using Abp.Application.Services.Dto;
using System;

namespace Pixel.GEC.Attendance.Setting.Dtos
{
    public class GetAllJobTitlesForExcelInput
    {
		public string Filter { get; set; }

        public string NameArFilter { get; set; }
        public string NameEnFilter { get; set; }



    }
}