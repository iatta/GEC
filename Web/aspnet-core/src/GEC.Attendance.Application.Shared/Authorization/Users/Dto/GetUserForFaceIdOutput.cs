using System;
using System.Collections.Generic;
using System.Text;

namespace GEC.Attendance.Authorization.Users.Dto
{
    public class GetUserForFaceIdOutput
    {
        public long? Id { get; set; }

       
        public string Name { get; set; }


        public string Surname { get; set; }

        public string Image { get; set; }
        

        public string UserName { get; set; }

       
        public string EmailAddress { get; set; }

        
        public string PhoneNumber { get; set; }


        public bool IsActive { get; set; }
        public int? OrganizationUnitId { get; set; }

        public int? ManagerId { get; set; }

        public string Code { get; set; }
        public string FingerCode { get; set; }
        public string CardNumber { get; set; }

        public string FingerPassword { get; set; }

        public string MobileNumber { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameAr { get; set; }

        public string Title { get; set; }
        public string Telephone_1 { get; set; }

        public string Telephone_2 { get; set; }

        public string Extension { get; set; }
        public int? JobTitleId { get; set; }

        public string Nationality { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinDate { get; set; }

        public string CivilId { get; set; }

        public string Device { get; set; }

        public string Address { get; set; }
        public string Address2 { get; set; }

        public string UnitDisplayName { get; set; }
        public string ManagerDisplayName { get; set; }

    }
}
