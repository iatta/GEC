using System.Collections.Generic;
using GEC.Attendance.Editions.Dto;

namespace GEC.Attendance.Web.Areas.App.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}