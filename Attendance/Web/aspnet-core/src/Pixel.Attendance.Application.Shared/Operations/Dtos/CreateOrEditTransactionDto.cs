
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateOrEditTransactionDto : EntityDto<int?>
    {

		public int SerialNo { get; set; }
		
		
		public int TransType { get; set; }
		
		
		public int Pin { get; set; }
		
		
		public int Finger { get; set; }
		
		
		public string Address { get; set; }
		
		
		public int Manual { get; set; }
		
		
		public int ScanType { get; set; }
		
		
		public bool IsProcessed { get; set; }
		
		
		public bool IsException { get; set; }
		
		
		public string Reason { get; set; }
		
		
		public int TimeSheetID { get; set; }
		
		
		public int ProcessInOut { get; set; }
		
		
		public string Remark { get; set; }
		
		
		public int KeyType { get; set; }
		
		
		public DateTime ImportDate { get; set; }
		public DateTime Transaction_Date { get; set; }
		public string UserName { get; set; }
		public string Time { get; set; }

		public bool ProjectManagerApprove { get; set; }
		public bool UnitManagerApprove { get; set; }
		public int MachineId { get; set; }
		public string EditNote { get; set; }

	}
}