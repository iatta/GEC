using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Pixel.Attendance.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var userDelegations = pages.CreateChildPermission(AppPermissions.Pages_UserDelegations, L("UserDelegations"));
            userDelegations.CreateChildPermission(AppPermissions.Pages_UserDelegations_Create, L("CreateNewUserDelegation"));
            userDelegations.CreateChildPermission(AppPermissions.Pages_UserDelegations_Edit, L("EditUserDelegation"));
            userDelegations.CreateChildPermission(AppPermissions.Pages_UserDelegations_Delete, L("DeleteUserDelegation"));



            var userTimeSheetApproves = pages.CreateChildPermission(AppPermissions.Pages_UserTimeSheetApproves, L("UserTimeSheetApproves"));
            userTimeSheetApproves.CreateChildPermission(AppPermissions.Pages_UserTimeSheetApproves_Create, L("CreateNewUserTimeSheetApprove"));
            userTimeSheetApproves.CreateChildPermission(AppPermissions.Pages_UserTimeSheetApproves_Edit, L("EditUserTimeSheetApprove"));
            userTimeSheetApproves.CreateChildPermission(AppPermissions.Pages_UserTimeSheetApproves_Delete, L("DeleteUserTimeSheetApprove"));



            var beacons = pages.CreateChildPermission(AppPermissions.Pages_Beacons, L("Beacons"));
            beacons.CreateChildPermission(AppPermissions.Pages_Beacons_Create, L("CreateNewBeacon"));
            beacons.CreateChildPermission(AppPermissions.Pages_Beacons_Edit, L("EditBeacon"));
            beacons.CreateChildPermission(AppPermissions.Pages_Beacons_Delete, L("DeleteBeacon"));



            
            var ManageUserShifts = pages.CreateChildPermission(AppPermissions.Pages_ManageUserShifts, L("ManageUserShiftsPermission"));
            var FingerPrints = pages.CreateChildPermission(AppPermissions.Pages_FingerPrint, L("FingerPrint"));
            FingerPrints.CreateChildPermission(AppPermissions.Pages_ProjectManagerTransactions, L("ProjectManagerTransactionsPermission"));
            FingerPrints.CreateChildPermission(AppPermissions.Pages_UnitManagerTransactions, L("UnitManagerTransactionsPermission"));
            FingerPrints.CreateChildPermission(AppPermissions.Pages_HrTransactions, L("HrTransactionsPermission"));
            

            var userShifts = pages.CreateChildPermission(AppPermissions.Pages_UserShifts, L("UserShifts"));
            userShifts.CreateChildPermission(AppPermissions.Pages_UserShifts_Create, L("CreateNewUserShift"));
            userShifts.CreateChildPermission(AppPermissions.Pages_UserShifts_Edit, L("EditUserShift"));
            userShifts.CreateChildPermission(AppPermissions.Pages_UserShifts_Delete, L("DeleteUserShift"));



            var projects = pages.CreateChildPermission(AppPermissions.Pages_Projects, L("Projects"));
            projects.CreateChildPermission(AppPermissions.Pages_Projects_Create, L("CreateNewProject"));
            projects.CreateChildPermission(AppPermissions.Pages_Projects_Edit, L("EditProject"));
            projects.CreateChildPermission(AppPermissions.Pages_Projects_Delete, L("DeleteProject"));



            var mobileWebPages = pages.CreateChildPermission(AppPermissions.Pages_MobileWebPages, L("MobileWebPages"));
            mobileWebPages.CreateChildPermission(AppPermissions.Pages_MobileWebPages_Create, L("CreateNewMobileWebPage"));
            mobileWebPages.CreateChildPermission(AppPermissions.Pages_MobileWebPages_Edit, L("EditMobileWebPage"));
            mobileWebPages.CreateChildPermission(AppPermissions.Pages_MobileWebPages_Delete, L("DeleteMobileWebPage"));



            var nationalities = pages.CreateChildPermission(AppPermissions.Pages_Nationalities, L("Nationalities"));
            nationalities.CreateChildPermission(AppPermissions.Pages_Nationalities_Create, L("CreateNewNationality"));
            nationalities.CreateChildPermission(AppPermissions.Pages_Nationalities_Edit, L("EditNationality"));
            nationalities.CreateChildPermission(AppPermissions.Pages_Nationalities_Delete, L("DeleteNationality"));



            var employeeWarnings = pages.CreateChildPermission(AppPermissions.Pages_EmployeeWarnings, L("EmployeeWarnings"));
            employeeWarnings.CreateChildPermission(AppPermissions.Pages_EmployeeWarnings_Create, L("CreateNewEmployeeWarning"));
            employeeWarnings.CreateChildPermission(AppPermissions.Pages_EmployeeWarnings_Edit, L("EditEmployeeWarning"));
            employeeWarnings.CreateChildPermission(AppPermissions.Pages_EmployeeWarnings_Delete, L("DeleteEmployeeWarning"));



            //var tempTransactions = pages.CreateChildPermission(AppPermissions.Pages_TempTransactions, L("TempTransactions"));
            //tempTransactions.CreateChildPermission(AppPermissions.Pages_TempTransactions_Create, L("CreateNewTempTransaction"));
            //tempTransactions.CreateChildPermission(AppPermissions.Pages_TempTransactions_Edit, L("EditTempTransaction"));
            //tempTransactions.CreateChildPermission(AppPermissions.Pages_TempTransactions_Delete, L("DeleteTempTransaction"));



            //var userDevices = pages.CreateChildPermission(AppPermissions.Pages_UserDevices, L("UserDevices"));
            //userDevices.CreateChildPermission(AppPermissions.Pages_UserDevices_Create, L("CreateNewUserDevice"));
            //userDevices.CreateChildPermission(AppPermissions.Pages_UserDevices_Edit, L("EditUserDevice"));
            //userDevices.CreateChildPermission(AppPermissions.Pages_UserDevices_Delete, L("DeleteUserDevice"));



            //var mobileTransactions = pages.CreateChildPermission(AppPermissions.Pages_MobileTransactions, L("MobileTransactions"));
            //mobileTransactions.CreateChildPermission(AppPermissions.Pages_MobileTransactions_Create, L("CreateNewMobileTransaction"));
            //mobileTransactions.CreateChildPermission(AppPermissions.Pages_MobileTransactions_Edit, L("EditMobileTransaction"));
            //mobileTransactions.CreateChildPermission(AppPermissions.Pages_MobileTransactions_Delete, L("DeleteMobileTransaction"));



            var manualTransactions = pages.CreateChildPermission(AppPermissions.Pages_ManualTransactions, L("ManualTransactions"));
            manualTransactions.CreateChildPermission(AppPermissions.Pages_ManualTransactions_Create, L("CreateNewManualTransaction"));
            manualTransactions.CreateChildPermission(AppPermissions.Pages_ManualTransactions_Edit, L("EditManualTransaction"));
            manualTransactions.CreateChildPermission(AppPermissions.Pages_ManualTransactions_Delete, L("DeleteManualTransaction"));



            //var trans = pages.CreateChildPermission(AppPermissions.Pages_Trans, L("Trans"));
            //trans.CreateChildPermission(AppPermissions.Pages_Trans_Create, L("CreateNewTran"));
            //trans.CreateChildPermission(AppPermissions.Pages_Trans_Edit, L("EditTran"));
            //trans.CreateChildPermission(AppPermissions.Pages_Trans_Delete, L("DeleteTran"));



            var employeeAbsences = pages.CreateChildPermission(AppPermissions.Pages_EmployeeAbsences, L("EmployeeAbsences"));
            employeeAbsences.CreateChildPermission(AppPermissions.Pages_EmployeeAbsences_Create, L("CreateNewEmployeeAbsence"));
            employeeAbsences.CreateChildPermission(AppPermissions.Pages_EmployeeAbsences_Edit, L("EditEmployeeAbsence"));
            employeeAbsences.CreateChildPermission(AppPermissions.Pages_EmployeeAbsences_Delete, L("DeleteEmployeeAbsence"));



            var locationCredentials = pages.CreateChildPermission(AppPermissions.Pages_LocationCredentials, L("LocationCredentials"));
            locationCredentials.CreateChildPermission(AppPermissions.Pages_LocationCredentials_Create, L("CreateNewLocationCredential"));
            locationCredentials.CreateChildPermission(AppPermissions.Pages_LocationCredentials_Edit, L("EditLocationCredential"));
            locationCredentials.CreateChildPermission(AppPermissions.Pages_LocationCredentials_Delete, L("DeleteLocationCredential"));



            var locations = pages.CreateChildPermission(AppPermissions.Pages_Locations, L("Locations"));
            locations.CreateChildPermission(AppPermissions.Pages_Locations_Create, L("CreateNewLocation"));
            locations.CreateChildPermission(AppPermissions.Pages_Locations_Edit, L("EditLocation"));
            locations.CreateChildPermission(AppPermissions.Pages_Locations_Delete, L("DeleteLocation"));



            //var employeeOfficialTaskDetails = pages.CreateChildPermission(AppPermissions.Pages_EmployeeOfficialTaskDetails, L("EmployeeOfficialTaskDetails"));
            //employeeOfficialTaskDetails.CreateChildPermission(AppPermissions.Pages_EmployeeOfficialTaskDetails_Create, L("CreateNewEmployeeOfficialTaskDetail"));
            //employeeOfficialTaskDetails.CreateChildPermission(AppPermissions.Pages_EmployeeOfficialTaskDetails_Edit, L("EditEmployeeOfficialTaskDetail"));
            //employeeOfficialTaskDetails.CreateChildPermission(AppPermissions.Pages_EmployeeOfficialTaskDetails_Delete, L("DeleteEmployeeOfficialTaskDetail"));



            //var transactions = pages.CreateChildPermission(AppPermissions.Pages_Transactions, L("Transactions"));
            //transactions.CreateChildPermission(AppPermissions.Pages_Transactions_Create, L("CreateNewTransaction"));
            //transactions.CreateChildPermission(AppPermissions.Pages_Transactions_Edit, L("EditTransaction"));
            //transactions.CreateChildPermission(AppPermissions.Pages_Transactions_Delete, L("DeleteTransaction"));



            var machines = pages.CreateChildPermission(AppPermissions.Pages_Machines, L("Machines"));
            machines.CreateChildPermission(AppPermissions.Pages_Machines_Create, L("CreateNewMachine"));
            machines.CreateChildPermission(AppPermissions.Pages_Machines_Edit, L("EditMachine"));
            machines.CreateChildPermission(AppPermissions.Pages_Machines_Delete, L("DeleteMachine"));



            var employeeOfficialTasks = pages.CreateChildPermission(AppPermissions.Pages_EmployeeOfficialTasks, L("EmployeeOfficialTasks"));
            employeeOfficialTasks.CreateChildPermission(AppPermissions.Pages_EmployeeOfficialTasks_Create, L("CreateNewEmployeeOfficialTask"));
            employeeOfficialTasks.CreateChildPermission(AppPermissions.Pages_EmployeeOfficialTasks_Edit, L("EditEmployeeOfficialTask"));
            employeeOfficialTasks.CreateChildPermission(AppPermissions.Pages_EmployeeOfficialTasks_Delete, L("DeleteEmployeeOfficialTask"));



            var officialTaskTypes = pages.CreateChildPermission(AppPermissions.Pages_OfficialTaskTypes, L("OfficialTaskTypes"));
            officialTaskTypes.CreateChildPermission(AppPermissions.Pages_OfficialTaskTypes_Create, L("CreateNewOfficialTaskType"));
            officialTaskTypes.CreateChildPermission(AppPermissions.Pages_OfficialTaskTypes_Edit, L("EditOfficialTaskType"));
            officialTaskTypes.CreateChildPermission(AppPermissions.Pages_OfficialTaskTypes_Delete, L("DeleteOfficialTaskType"));



            //var timeProfileDetails = pages.CreateChildPermission(AppPermissions.Pages_TimeProfileDetails, L("TimeProfileDetails"));
            //timeProfileDetails.CreateChildPermission(AppPermissions.Pages_TimeProfileDetails_Create, L("CreateNewTimeProfileDetail"));
            //timeProfileDetails.CreateChildPermission(AppPermissions.Pages_TimeProfileDetails_Edit, L("EditTimeProfileDetail"));
            //timeProfileDetails.CreateChildPermission(AppPermissions.Pages_TimeProfileDetails_Delete, L("DeleteTimeProfileDetail"));



            var timeProfiles = pages.CreateChildPermission(AppPermissions.Pages_TimeProfiles, L("TimeProfiles"));
            timeProfiles.CreateChildPermission(AppPermissions.Pages_TimeProfiles_Create, L("CreateNewTimeProfile"));
            timeProfiles.CreateChildPermission(AppPermissions.Pages_TimeProfiles_Edit, L("EditTimeProfile"));
            timeProfiles.CreateChildPermission(AppPermissions.Pages_TimeProfiles_Delete, L("DeleteTimeProfile"));
            timeProfiles.CreateChildPermission(AppPermissions.Pages_UploadTimeProfile, L("UploadTimeProfile"));
            


            //var shiftTypeDetails = pages.CreateChildPermission(AppPermissions.Pages_ShiftTypeDetails, L("ShiftTypeDetails"));
            //shiftTypeDetails.CreateChildPermission(AppPermissions.Pages_ShiftTypeDetails_Create, L("CreateNewShiftTypeDetail"));
            //shiftTypeDetails.CreateChildPermission(AppPermissions.Pages_ShiftTypeDetails_Edit, L("EditShiftTypeDetail"));
            //shiftTypeDetails.CreateChildPermission(AppPermissions.Pages_ShiftTypeDetails_Delete, L("DeleteShiftTypeDetail"));



            var shiftTypes = pages.CreateChildPermission(AppPermissions.Pages_ShiftTypes, L("ShiftTypes"));
            shiftTypes.CreateChildPermission(AppPermissions.Pages_ShiftTypes_Create, L("CreateNewShiftType"));
            shiftTypes.CreateChildPermission(AppPermissions.Pages_ShiftTypes_Edit, L("EditShiftType"));
            shiftTypes.CreateChildPermission(AppPermissions.Pages_ShiftTypes_Delete, L("DeleteShiftType"));



            var shifts = pages.CreateChildPermission(AppPermissions.Pages_Shifts, L("Shifts"));
            shifts.CreateChildPermission(AppPermissions.Pages_Shifts_Create, L("CreateNewShift"));
            shifts.CreateChildPermission(AppPermissions.Pages_Shifts_Edit, L("EditShift"));
            shifts.CreateChildPermission(AppPermissions.Pages_Shifts_Delete, L("DeleteShift"));



            var warningTypes = pages.CreateChildPermission(AppPermissions.Pages_WarningTypes, L("WarningTypes"));
            warningTypes.CreateChildPermission(AppPermissions.Pages_WarningTypes_Create, L("CreateNewWarningType"));
            warningTypes.CreateChildPermission(AppPermissions.Pages_WarningTypes_Edit, L("EditWarningType"));
            warningTypes.CreateChildPermission(AppPermissions.Pages_WarningTypes_Delete, L("DeleteWarningType"));



            var employeeVacations = pages.CreateChildPermission(AppPermissions.Pages_EmployeeVacations, L("EmployeeVacations"));
            employeeVacations.CreateChildPermission(AppPermissions.Pages_EmployeeVacations_Create, L("CreateNewEmployeeVacation"));
            employeeVacations.CreateChildPermission(AppPermissions.Pages_EmployeeVacations_Edit, L("EditEmployeeVacation"));
            employeeVacations.CreateChildPermission(AppPermissions.Pages_EmployeeVacations_Delete, L("DeleteEmployeeVacation"));
            employeeVacations.CreateChildPermission(AppPermissions.Pages_UploadEmpVacation, L("UploadEmpVacation"));



            var employeePermits = pages.CreateChildPermission(AppPermissions.Pages_EmployeePermits, L("EmployeePermits"));
            employeePermits.CreateChildPermission(AppPermissions.Pages_EmployeePermits_Create, L("CreateNewEmployeePermit"));
            employeePermits.CreateChildPermission(AppPermissions.Pages_EmployeePermits_Edit, L("EditEmployeePermit"));
            employeePermits.CreateChildPermission(AppPermissions.Pages_EmployeePermits_Delete, L("DeleteEmployeePermit"));
            employeePermits.CreateChildPermission(AppPermissions.Pages_Approve_Permit, L("ApprovePermit"));

            //new permissions 

            var reports = pages.CreateChildPermission(AppPermissions.Pages_Reports, L("Reports"));
            reports.CreateChildPermission(AppPermissions.Pages_GeneralReports, L("GeneralReport"));
            reports.CreateChildPermission(AppPermissions.Pages_TimeProfileReport, L("TimeProfileReport"));

            var systemConfigurations = pages.CreateChildPermission(AppPermissions.Pages_SystemConfigurations, L("SystemConfigurations"));
            systemConfigurations.CreateChildPermission(AppPermissions.Pages_SystemConfigurations_Create, L("CreateNewSystemConfiguration"));
            systemConfigurations.CreateChildPermission(AppPermissions.Pages_SystemConfigurations_Edit, L("EditSystemConfiguration"));
            //systemConfigurations.CreateChildPermission(AppPermissions.Pages_SystemConfigurations_Delete, L("DeleteSystemConfiguration"));



            var permits = pages.CreateChildPermission(AppPermissions.Pages_Permits, L("Permits"));
            permits.CreateChildPermission(AppPermissions.Pages_Permits_Create, L("CreateNewPermit"));
            permits.CreateChildPermission(AppPermissions.Pages_Permits_Edit, L("EditPermit"));
            permits.CreateChildPermission(AppPermissions.Pages_Permits_Delete, L("DeletePermit"));



            var typesOfPermits = pages.CreateChildPermission(AppPermissions.Pages_TypesOfPermits, L("TypesOfPermits"));
            typesOfPermits.CreateChildPermission(AppPermissions.Pages_TypesOfPermits_Create, L("CreateNewTypesOfPermit"));
            typesOfPermits.CreateChildPermission(AppPermissions.Pages_TypesOfPermits_Edit, L("EditTypesOfPermit"));
            typesOfPermits.CreateChildPermission(AppPermissions.Pages_TypesOfPermits_Delete, L("DeleteTypesOfPermit"));



            var holidays = pages.CreateChildPermission(AppPermissions.Pages_Holidays, L("Holidays"));
            holidays.CreateChildPermission(AppPermissions.Pages_Holidays_Create, L("CreateNewHoliday"));
            holidays.CreateChildPermission(AppPermissions.Pages_Holidays_Edit, L("EditHoliday"));
            holidays.CreateChildPermission(AppPermissions.Pages_Holidays_Delete, L("DeleteHoliday"));



            var leaveTypes = pages.CreateChildPermission(AppPermissions.Pages_LeaveTypes, L("LeaveTypes"));
            leaveTypes.CreateChildPermission(AppPermissions.Pages_LeaveTypes_Create, L("CreateNewLeaveType"));
            leaveTypes.CreateChildPermission(AppPermissions.Pages_LeaveTypes_Edit, L("EditLeaveType"));
            leaveTypes.CreateChildPermission(AppPermissions.Pages_LeaveTypes_Delete, L("DeleteLeaveType"));



            var jobTitles = pages.CreateChildPermission(AppPermissions.Pages_JobTitles, L("JobTitles"));
            jobTitles.CreateChildPermission(AppPermissions.Pages_JobTitles_Create, L("CreateNewJobTitle"));
            jobTitles.CreateChildPermission(AppPermissions.Pages_JobTitles_Edit, L("EditJobTitle"));
            jobTitles.CreateChildPermission(AppPermissions.Pages_JobTitles_Delete, L("DeleteJobTitle"));


            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));
            

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_DelegatedUsers, L("DelegatedUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_SendNotification, L("SendNotification"));
            

            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AttendanceConsts.LocalizationSourceName);
        }
    }
}
