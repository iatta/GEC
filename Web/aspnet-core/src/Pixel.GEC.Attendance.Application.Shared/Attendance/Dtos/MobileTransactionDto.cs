﻿
using System;
using Abp.Application.Services.Dto;

namespace Pixel.GEC.Attendance.Attendance.Dtos
{
    public class MobileTransactionDto : EntityDto
    {
		public int SiteId { get; set; }

		public string SiteName { get; set; }



    }
}