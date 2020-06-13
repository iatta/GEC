using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
