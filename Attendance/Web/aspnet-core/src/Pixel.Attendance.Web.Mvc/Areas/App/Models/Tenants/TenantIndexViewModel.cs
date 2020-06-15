using System.Collections.Generic;
using Pixel.Attendance.Editions.Dto;

namespace Pixel.Attendance.Web.Areas.App.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}