namespace Pixel.Attendance.Operations.Dtos
{
    public class GetManualTransactionForViewDto
    {
		public ManualTransactionDto ManualTransaction { get; set; }

		public string UserName { get; set;}

		public string MachineNameEn { get; set;}
        public string FingerCode { get; set; }


    }
}