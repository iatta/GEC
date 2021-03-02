using Pixel.Attendance.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Setting.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pixel.Attendance.Authorization.Users.Dto
{
    public class CreateOrUpdateUserInput
    {
       
        [Required]
        public UserEditDto User { get; set; }

        [Required]
        public string[] AssignedRoleNames { get; set; }

        public List<AssignedLocationDto> AssignedLocations { get; set; }

        public bool SendActivationEmail { get; set; }

        public bool SetRandomPassword { get; set; }

        public List<long> OrganizationUnits { get; set; }

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
        public int? ShiftId { get; set; }

        public virtual JobTitleDto JobTitle { get; set; }

        //public string Nationality { get; set; }
        public int? NationalityId { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinDate { get; set; }

        public string CivilId { get; set; }

        public string Device { get; set; }

        public string Address { get; set; }
        public string Address2 { get; set; }
        public int? OrganizationUnitId { get; set; }
        public DateTime? TerminationDate { get; set; }
        public bool Terminated { get; set; }
        public int? TitleId { get; set; }
        public int? ManagerId { get; set; }

        public bool UserLoaded { get; set; }
        public bool IsFixedOverTimeAllowed { get; set; }
        public bool IsNormalOverTimeAllowed { get; set; }
        public int? TaskTypeId { get; set; }

        //public CreateOrEditTimeProfileDto TimeProfile { get; set; }
        [NotMapped]
        public List<GetUserShiftForViewDto> UserShifts { get; set; }
        public List<GetOverrideShiftForViewDto> OverrideShifts { get; set; }

        public int MachineId { get; set; }
        public string UserImage { get; set; }
        public bool UploadUser { get; set; }
        public CreateOrUpdateUserInput()
        {
            OrganizationUnits = new List<long>();
            OverrideShifts = new List<GetOverrideShiftForViewDto>();
        }
    }
}