using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using Abp.Organizations;
using Abp.Timing;
using Pixel.Attendance.Enums;
using Pixel.Attendance.Operations;
using Pixel.Attendance.Setting;

namespace Pixel.Attendance.Authorization.Users
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User : AbpUser<User>
    {
        public virtual Guid? ProfilePictureId { get; set; }

        public virtual bool ShouldChangePasswordOnNextLogin { get; set; }

        public DateTime? SignInTokenExpireTimeUtc { get; set; }

        public string SignInToken { get; set; }

        public string GoogleAuthenticatorKey { get; set; }

        public int? OrganizationUnitId { get; set; }

        public int? ManagerId { get; set; }

        public List<UserOrganizationUnit> OrganizationUnits { get; set; }

        public string Code { get; set; }
        public string FingerCode { get; set; }
        public string CardNumber { get; set; }

        public string FingerPassword { get; set; }

        public string MobileNumber { get; set; }
        public string FullNameEn { get; set; }

        public Title TitleId { get; set; }
        public string Telephone_1 { get; set; }

        public string Telephone_2 { get; set; }

        public string Extension { get; set; }
        public int? JobTitleId { get; set; }

        [ForeignKey("JobTitleId")]
        public virtual JobTitle JobTitle { get; set; }


        public int? NationalityId { get; set; }

        [ForeignKey("NationalityId")]
        public Nationality Nationality { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime? TerminationDate { get; set; }

        public bool Terminated { get; set; }
        public int? OldId { get; set; }
        public DateTime JoinDate { get; set; }

        public string CivilId { get; set; }

        public string Device { get; set; }

        public string Address { get; set; }
        public string Address2 { get; set; }

       public ICollection<UserLocation> Locations { get; set; }

        public string FaceMap { get; set; }
        public string Image { get; set; }
        public bool IsFaceRegistered { get; set; }
        public string MobilePassword { get; set; }

        public ICollection<ProjectUser> Projects { get; set; }
        
        

        //Can add application specific user properties here

        public User()
        {
            IsLockoutEnabled = true;
            IsTwoFactorEnabled = true;
        }

        /// <summary>
        /// Creates admin <see cref="User"/> for a tenant.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="emailAddress">Email address</param>
        /// <returns>Created <see cref="User"/> object</returns>
        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>(),
                Locations = new List<UserLocation>(),
                OrganizationUnits = new List<UserOrganizationUnit>()
            };

            user.SetNormalizedNames();

            return user;
        }

        public override void SetNewPasswordResetCode()
        {
            /* This reset code is intentionally kept short.
             * It should be short and easy to enter in a mobile application, where user can not click a link.
             */
            PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(10).ToUpperInvariant();
        }

        public void Unlock()
        {
            AccessFailedCount = 0;
            LockoutEndDateUtc = null;
        }

        public void SetSignInToken()
        {
            SignInToken = Guid.NewGuid().ToString();
            SignInTokenExpireTimeUtc = Clock.Now.AddMinutes(1).ToUniversalTime();
        }
    }
}