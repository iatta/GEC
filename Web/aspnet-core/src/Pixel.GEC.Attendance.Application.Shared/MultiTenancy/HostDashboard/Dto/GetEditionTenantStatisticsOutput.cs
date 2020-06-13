using System.Collections.Generic;

namespace Pixel.GEC.Attendance.MultiTenancy.HostDashboard.Dto
{
    public class GetEditionTenantStatisticsOutput
    {
        public GetEditionTenantStatisticsOutput(List<TenantEdition> editionStatistics)
        {
            EditionStatistics = editionStatistics;
        }

        public List<TenantEdition> EditionStatistics { get; set; }
    }
}