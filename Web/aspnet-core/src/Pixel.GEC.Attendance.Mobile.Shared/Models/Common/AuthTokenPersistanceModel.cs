using System;
using Abp.AutoMapper;
using Pixel.GEC.Attendance.Sessions.Dto;

namespace Pixel.GEC.Attendance.Models.Common
{
    [AutoMapFrom(typeof(ApplicationInfoDto)),
     AutoMapTo(typeof(ApplicationInfoDto))]
    public class ApplicationInfoPersistanceModel
    {
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}