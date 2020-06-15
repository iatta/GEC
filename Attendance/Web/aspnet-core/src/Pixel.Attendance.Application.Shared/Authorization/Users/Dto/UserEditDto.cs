using System;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Setting.Dtos;

namespace Pixel.Attendance.Authorization.Users.Dto
{
    //Mapped to/from User in CustomDtoMapper
    public class UserEditDto : IPassivable
    {
        /// <summary>
        /// Set null to create a new user. Set user's Id to update a user
        /// </summary>
        public long? Id { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [StringLength(UserConsts.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        // Not used "Required" attribute since empty value is used to 'not change password'
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool IsActive { get; set; }
        public int? OrganizationUnitId { get; set; }

        public int? ManagerId { get; set; }

        public bool ShouldChangePasswordOnNextLogin { get; set; }

        public virtual bool IsTwoFactorEnabled { get; set; }

        public virtual bool IsLockoutEnabled { get; set; }

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

        //public string Nationality { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime JoinDate { get; set; }
        public bool Terminated { get; set; }

        public DateTime? TerminationDate { get; set; }
        public int? NationalityId { get; set; }
        public int? TitleId { get; set; }

        public string CivilId { get; set; }

        public string Device { get; set; }

        public string Address { get; set; }
        public string Address2 { get; set; }
        public string MobilePasword { get; set; }
        public CreateOrEditTimeProfileDto TimeProfile { get; set; }
    }
}