
using System;
using Abp.Application.Services.Dto;

namespace Pixel.GEC.Attendance.Operations.Dtos
{
    public class TransactionDto : EntityDto
    {
		public int TransType { get; set; }
        public  int Pin { get; set; }
        public int KeyType { get; set; }
        public  DateTime Transaction_Date { get; set; }
        public DateTime CreationTime { get; set; }
        public string UserName { get; set; }
        public string Time { get; set; }
    }
}