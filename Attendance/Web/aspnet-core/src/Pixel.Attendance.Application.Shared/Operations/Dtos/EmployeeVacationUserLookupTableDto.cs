using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Operations.Dtos
{
    public class EmployeeVacationUserLookupTableDto
    {
		public long Id { get; set; }

		public string DisplayName { get; set; }
        public string FingerCode { get; set; }
    }
}