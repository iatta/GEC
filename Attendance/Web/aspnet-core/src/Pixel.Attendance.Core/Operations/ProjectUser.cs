using Pixel.Attendance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pixel.Attendance.Operations
{
    
    [Table("ProjectUsers")]
    public class ProjectUser
    {
        public long UserId { get; set; }
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
