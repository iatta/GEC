using Pixel.Attendance.Dto;

namespace Pixel.Attendance.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
