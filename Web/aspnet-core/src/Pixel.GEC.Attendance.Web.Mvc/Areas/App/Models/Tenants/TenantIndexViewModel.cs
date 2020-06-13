using System.Collections.Generic;
using Pixel.GEC.Attendance.Editions.Dto;

namespace Pixel.GEC.Attendance.Web.Areas.App.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}