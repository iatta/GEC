using GEC.Attendance.Dto;

namespace GEC.Attendance.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
