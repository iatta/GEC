using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using Pixel.Attendance.Authorization.Users.Importing.Dto;
using Pixel.Attendance.DataExporting.Excel.EpPlus;

namespace Pixel.Attendance.Authorization.Users.Importing
{
    public class UserListExcelDataReader : EpPlusExcelImporterBase<ImportUserDto>, IUserListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public UserListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(AttendanceConsts.LocalizationSourceName);
        }

        public List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportUserDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var user = new ImportUserDto();

            try
            {
                user.UserName = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(user.UserName), exceptionMessage).Replace(" ", "");
                user.Name = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(user.Name), exceptionMessage).Replace(" ", "");
                user.Surname = GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(user.Surname), exceptionMessage);
                //user.EmailAddress = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(user.EmailAddress), exceptionMessage);
                user.EmailAddress = getSaltString();
                user.PhoneNumber = worksheet.Cells[row, 5].Value?.ToString();
                user.Password = GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(user.Password), exceptionMessage);
                user.AssignedRoleNames = GetAssignedRoleNamesFromRow(worksheet, row, 7);
                user.FingerCode = worksheet.Cells[row, 8].Value?.ToString();
                user.Code = worksheet.Cells[row, 9].Value?.ToString();
                user.CivilId = worksheet.Cells[row, 10].Value?.ToString();
                user.OrganizationUnitId =Convert.ToInt32(worksheet.Cells[row, 11].Value?.ToString());
            }
            catch (System.Exception exception)
            {
                user.Exception = exception.Message;
            }

            return user;
        }

        protected String getSaltString()
        {
            Random random = new Random();
            // Generate 10 random email addresses. 
           string address = string.Format("qa{0:0000}@test.com", random.Next(10000));
            return address;

        }

        private string GetRequiredValueFromRowOrNull(ExcelWorksheet worksheet, int row, int column, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                return cellValue.ToString();
            }

            exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
            return null;
        }

        private string[] GetAssignedRoleNamesFromRow(ExcelWorksheet worksheet, int row, int column)
        {
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                return new string[0];
            }

            return cellValue.ToString().Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();
        }

        private string GetLocalizedExceptionMessagePart(string parameter)
        {
            return _localizationSource.GetString("{0}IsInvalid", _localizationSource.GetString(parameter)) + "; ";
        }

        private bool IsRowEmpty(ExcelWorksheet worksheet, int row)
        {
            return worksheet.Cells[row, 1].Value == null || string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Value.ToString());
        }
    }
}
