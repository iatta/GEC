using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.GEC.Attendance.Operations.Dtos
{
    public class GetTransactionForEditOutput
    {
		public CreateOrEditTransactionDto Transaction { get; set; }
        public string UserName { get; set; }

    }
}