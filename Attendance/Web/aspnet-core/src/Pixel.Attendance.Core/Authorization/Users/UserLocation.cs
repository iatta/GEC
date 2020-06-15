using Pixel.Attendance.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Authorization.Users
{
    public class UserLocation
    {
        public UserLocation(long userId , int locationId,DateTime fromDate, DateTime toDate)
        {
            this.UserId = userId;
            this.LocationId = locationId;
            this.FromDate = fromDate;
            this.ToDate = toDate;
        }
        public long UserId { get; set; }
        public int LocationId { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Location Location { get; set; }

        public User User { get; set; }
    }
}
