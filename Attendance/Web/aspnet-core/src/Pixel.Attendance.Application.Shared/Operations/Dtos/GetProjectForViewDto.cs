using System.Collections.Generic;

namespace Pixel.Attendance.Operations.Dtos
{
    public class GetProjectForViewDto
    {
        public GetProjectForViewDto()
        {
            MachinesNames = new List<string>();
        }
		public ProjectDto Project { get; set; }

		public string UserName { get; set;}
        public string AssistantUserName { get; set; }

        public string LocationTitleEn { get; set;}

        public IEnumerable<string> LocationsTitles { get; set; }
        public IEnumerable<string> MachinesNames { get; set; }

        public string OrganizationUnitDisplayName { get; set;}


    }
}