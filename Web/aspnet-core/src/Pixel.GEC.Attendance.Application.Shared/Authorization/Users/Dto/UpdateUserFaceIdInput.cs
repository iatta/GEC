using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.GEC.Attendance.Authorization.Users.Dto
{
    public class UpdateUserFaceIdInput
    {
        public long UserId { get; set; }
        public string Image { get; set; }

        public string FaceMap { get; set; }
        public bool IsEnrolled { get; set; }
    }
}
