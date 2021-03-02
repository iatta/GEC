using System.ComponentModel.DataAnnotations.Schema;

namespace Pixel.Attendance.Authorization.Users.Importing.Dto
{
    public class ImportUserDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }
        
        public string Password { get; set; }
        public string FingerCode { get; set; }
        public string Code { get; set; }
        public string CivilId { get; set; }
        public long? OrganizationUnitId { get; set; }
        [NotMapped]
        public string Department { get; set; }
        [NotMapped]
        public string ShiftName { get; set; }

        /// <summary>
        /// comma separated list
        /// </summary>
        public string[] AssignedRoleNames { get; set; }

        /// <summary>
        /// Can be set when reading data from excel or when importing user
        /// </summary>
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}