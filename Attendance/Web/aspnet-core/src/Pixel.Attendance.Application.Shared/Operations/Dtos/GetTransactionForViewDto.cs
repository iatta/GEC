using Pixel.Attendance.Enums;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetTransactionForViewDto
    {
		public TransactionDto Transaction { get; set; }
        public string UserName { get; set; }
        public string ProjectName { get; set; }
        public string ShiftName { get; set; }
		public DayDto DayOff { get; set; }

		public DayDto DayRest { get; set; }

		public  int TimeIn { get; set; }

		public  int TimeOut { get; set; }

		public  int EarlyIn { get; set; }

		public  int LateIn { get; set; }

		public  int EarlyOut { get; set; }

		public  int LateOut { get; set; }

		public  int TimeInRangeFrom { get; set; }

		public  int TimeInRangeTo { get; set; }

		public  int TimeOutRangeFrom { get; set; }

		public  int TimeOutRangeTo { get; set; }
		public int Overtime { get; set; }
		public int Attendance_LateIn { get; set; }
		public int Attendance_EarlyOut { get; set; }
		public int TotalOverTime { get; set; }
		public int TotalUserCount { get; set; }


	}
}