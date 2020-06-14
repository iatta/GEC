using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using GEC.Attendance.Setting.Dtos;

namespace GEC.Attendance.Authorization.Users.Dto
{
    public class UserListDto : EntityDto<long>, IPassivable, IHasCreationTime
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public List<UserListRoleDto> Roles { get; set; }

        public bool IsActive { get; set; }
        public int? OrganizationUnitId { get; set; }

        public int? ManagerId { get; set; }
        public DateTime CreationTime { get; set; }

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

        public JobTitleDto JobTitle { get; set; }
        public int TitleId { get; set; }
        public int? NationalityId { get; set; }

        public NationalityDto Nationality { get; set; }
        

        public DateTime DateOfBirth { get; set; }
        public DateTime? TerminationDate { get; set; }
        public DateTime JoinDate { get; set; }
        public bool Terminated { get; set; }

        public string CivilId { get; set; }

        public string Device { get; set; }

        public string Address { get; set; }
        public string Address2 { get; set; }
        public string UnitName { get; set; }

        public string JobTitleName { get; set; }

    }
}