
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;

namespace Pixel.Attendance.Setting.Dtos
{
    public class OverrideShiftDto : EntityDto
    {
        public DateTime Day { get; set; }


        public long? UserId { get; set; }

        public int? ShiftId { get; set; }

        public ShiftDto Shift { get; set; }
        [NotMapped]
        public bool IsNew { get; set; }
        public bool IsModified { get; set; }
        public bool IsDeleted { get; set; }
    }
}