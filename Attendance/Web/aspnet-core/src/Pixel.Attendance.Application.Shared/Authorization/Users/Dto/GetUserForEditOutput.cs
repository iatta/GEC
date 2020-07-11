using System;
using System.Collections.Generic;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Organizations.Dto;
using Pixel.Attendance.Setting.Dtos;

namespace Pixel.Attendance.Authorization.Users.Dto
{
    public class GetUserForEditOutput
    {
        public Guid? ProfilePictureId { get; set; }

        public UserEditDto User { get; set; }

        public UserRoleDto[] Roles { get; set; }

        public List<NationalityDto> Nationalities { get; set; }
        public UserLocationDto[] Locations { get; set; }

        public List<OrganizationUnitDto> AllOrganizationUnits { get; set; }
        public List<BeaconDto> AllBeacons { get; set; }

        public List<string> MemberedOrganizationUnits { get; set; }

        public string MemberOrganizationUnit { get; set; }

        public string Code { get; set; }
        public string FingerCode { get; set; }
        public string CardNumber { get; set; }

        public string FingerPassword { get; set; }

        public string MobileNumber { get; set; }
        public string FullNameEn { get; set; }

        public string Title { get; set; }
        public string Telephone_1 { get; set; }

        public string Telephone_2 { get; set; }

        public string Extension { get; set; }
        public int? JobTitleId { get; set; }

        
        public virtual JobTitleDto JobTitle { get; set; }

        public string Nationality { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinDate { get; set; }
        public DateTime? TerminationDate { get; set; }

        public bool Terminated { get; set; }
        public int TitleId { get; set; }

        public int? NationalityId { get; set; }

        

        public string CivilId { get; set; }

        public string Device { get; set; }

        public string Address { get; set; }
        public string Address2 { get; set; }
        


        //public CreateOrEditTimeProfileDto TimeProfile { get; set; }

        //public List<GetUserShiftForViewDto> UserShifts { get; set; }



    }
}