import { PermissionCheckerService } from '@abp/auth/permission-checker.service';
import { AppSessionService } from '@shared/common/session/app-session.service';

import { Injectable } from '@angular/core';
import { AppMenu } from './app-menu';
import { AppMenuItem } from './app-menu-item';

@Injectable()
export class AppNavigationService {

    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService
    ) {

    }

    getMenu(): AppMenu {
        return new AppMenu('MainMenu', 'MainMenu', [

            new AppMenuItem('Dashboard', 'Pages.Administration.Host.Dashboard', 'flaticon-line-graph', '/app/admin/hostDashboard'),
            new AppMenuItem('Dashboard', 'Pages.Tenant.Dashboard', 'flaticon-line-graph', '/app/main/dashboard'),
            new AppMenuItem('Tenants', 'Pages.Tenants', 'flaticon-list-3', '/app/admin/tenants'),
            new AppMenuItem('Editions', 'Pages.Editions', 'flaticon-app', '/app/admin/editions'),

            new AppMenuItem('Shifts', '', 'flaticon-interface-6', '', [
                // new AppMenuItem('ShiftTypes', 'Pages.ShiftTypes', 'flaticon-file-1', '/app/main/setting/shiftTypes'),
                new AppMenuItem('Shifts', 'Pages.Shifts', 'flaticon-calendar-with-a-clock-time-tools', '/app/main/setting/shifts'),
                new AppMenuItem('ManageUserShifts', 'Pages.ManageUserShifts', 'flaticon-calendar-with-a-clock-time-tools', '/app/main/operations/manageUserShifts'),
                new AppMenuItem('OverrideUserShift', 'Pages.ManageUserShifts', 'flaticon-calendar-with-a-clock-time-tools', '/app/main/operations/overrideUserShifts'),

                // new AppMenuItem('UserShifts', 'Pages.UserShifts', 'flaticon-more', '/app/main/operations/userShifts'),
                // new AppMenuItem('TimeProfiles', 'Pages.TimeProfiles', 'flaticon-time-1', '/app/main/operations/mangeTimeProfile'),
                // new AppMenuItem('UploadTimeProfile', 'Pages.UploadTimeProfile', 'flaticon-file-1', '/app/main/operations/uploadTimeProfile')
            ]),


            new AppMenuItem('Holidays', '', 'flaticon-calendar-1', '', [
                new AppMenuItem('Holidays', 'Pages.Holidays', 'flaticon-calendar-1', '/app/main/setting/holidays'),
                new AppMenuItem('LeaveTypes', 'Pages.LeaveTypes', 'flaticon2-contract', '/app/main/setting/leaveTypes'),
                new AppMenuItem('EmployeeVacations', 'Pages.EmployeeVacations', 'flaticon-notes', '/app/main/operations/employeeVacations'),

                new AppMenuItem('EmployeeAbsences', 'Pages.EmployeeAbsences', 'flaticon-close', '/app/main/operation/employeeAbsences'),
                // new AppMenuItem('UploadEmpVacation', 'Pages.UploadEmpVacation', 'flaticon-file-1', '/app/main/operations/uploadEmpVacation'),
            ]),



            new AppMenuItem('Permits', '', 'flaticon-interface-10', '', [
                new AppMenuItem('PermitsTypes', 'Pages.Permits', 'flaticon-interface-10', '/app/main/setting/permits'),
                new AppMenuItem('EmployeePermits', 'Pages.EmployeePermits', 'flaticon-edit-1', '/app/main/operations/employeePermits'),
                new AppMenuItem('EmployeePermitsManage', 'Pages.ApprovePermit', 'flaticon-edit-1', '/app/main/operations/employeePermitsManage'),
            ]),


            new AppMenuItem('EmployeeOfficialTasks', '', 'flaticon-calendar-1', '', [
                new AppMenuItem('OfficialTaskTypes', 'Pages.OfficialTaskTypes', 'flaticon2-layers-2', '/app/main/setting/officialTaskTypes'),
                new AppMenuItem('EmployeeOfficialTasks', 'Pages.EmployeeOfficialTasks', 'flaticon2-pen', '/app/main/operations/employeeOfficialTasks'),
            ]),

            new AppMenuItem('Fingerprints', '', 'flaticon-interface-6', '', [
                new AppMenuItem('FaceId', 'Pages.Administration.Users', 'flaticon-users', '/app/admin/manageFaceId'),
                new AppMenuItem('ManualTransactions', 'Pages.ManualTransactions', 'flaticon2-notepad', '/app/main/operations/manualTransactions'),
                // new AppMenuItem('ProjectManagerTransactions', 'Pages.FingerPrint.ProjectManagerTransactions', 'flaticon-rotate', '/app/main/operations/ProjectManagerTransactions'),
                // new AppMenuItem('UnitManagerTransactions', 'Pages.FingerPrint.UnitManagerTransactions', 'flaticon-rotate', '/app/main/operations/UnitManagerTransactions'),
                // new AppMenuItem('HrTransactions', 'Pages.FingerPrint.HrTransactions', 'flaticon-rotate', '/app/main/operations/HrTransactions'),

            ]),

            new AppMenuItem('Locations', '', 'flaticon2-location', '', [
                new AppMenuItem('Locations', 'Pages.Locations', 'flaticon-map-location', '/app/main/setting/locations'),
            ]),

            // new AppMenuItem('Settings', '', 'flaticon-settings', '', [

            //     // new AppMenuItem('SystemConfigurations', 'Pages.SystemConfigurations', 'flaticon-safe-shield-protection', '/app/main/setting/systemConfigurations'),


            //     new AppMenuItem('MobileWebPages', 'Pages.MobileWebPages', 'pi-mobile', '/app/main/settings/mobileWebPages'),
            // ]),

            // new AppMenuItem('Transactions', 'Pages.Transactions', 'flaticon-more', '/app/main/operations/transactions'),
            new AppMenuItem('Warnings', '', 'flaticon-interface-8', '', [
                new AppMenuItem('WarningTypes', 'Pages.WarningTypes', 'flaticon-alert-1', '/app/main/setting/warningTypes'),
                new AppMenuItem('EmployeeWarnings', 'Pages.EmployeeWarnings', 'flaticon-more', '/app/main/operations/employeeWarnings'),
            ]),








            new AppMenuItem('Beacons', 'Pages.Beacons', 'flaticon-laptop', '/app/main/operations/beacons'),


            // new AppMenuItem('ManualTransactions', 'Pages.ManualTransactions', 'flaticon-more', '/app/main/operations/manualTransactions'),

            // new AppMenuItem('UserTimeSheetApproves', 'Pages.UserTimeSheetApproves', 'flaticon-more', '/app/main/operations/userTimeSheetApproves'),

            new AppMenuItem('UserDelegations', 'Pages.UserDelegations', 'flaticon-rotate', '/app/main/operations/userDelegations'),

            // new AppMenuItem('LocationMachines', 'Pages.LocationMachines', 'flaticon-more', '/app/main/setting/locationMachines'),

            // new AppMenuItem('ProjectLocations', 'Pages.ProjectLocations', 'flaticon-more', '/app/main/operations/projectLocations'),

            // new AppMenuItem('OrganizationLocations', 'Pages.OrganizationLocations', 'flaticon-more', '/app/main/operations/organizationLocations'),

            new AppMenuItem('EmployeeTempTransfers', 'Pages.EmployeeTempTransfers', 'flaticon-more', '/app/main/operations/employeeTempTransfers'),

            // new AppMenuItem('OverrideShifts', 'Pages.OverrideShifts', 'flaticon-more', '/app/main/setting/overrideShifts'),

            // new AppMenuItem('TransactionLogs', 'Pages.TransactionLogs', 'flaticon-more', '/app/main/operations/transactionLogs'),
             new AppMenuItem('Administration', '', 'flaticon-interface-8', '', [
                new AppMenuItem('OrganizationUnits', 'Pages.Administration.OrganizationUnits', 'flaticon-map', '/app/admin/organization-units'),
                new AppMenuItem('JobTitles', 'Pages.JobTitles', 'flaticon2-list-3', '/app/main/setting/jobTitles'),
                new AppMenuItem('Roles', 'Pages.Administration.Roles', 'flaticon-suitcase', '/app/admin/roles'),
                new AppMenuItem('Users', 'Pages.Administration.Users', 'flaticon-users', '/app/admin/users'),
                new AppMenuItem('DelegatedUsers', 'Pages.Administration.DelegatedUsers', 'flaticon-users', '/app/admin/delegatedUsers'),
                new AppMenuItem('Machines', 'Pages.Machines', 'flaticon-delete-2', '/app/main/setting/machines'),
                new AppMenuItem('Projects', 'Pages.Projects', 'flaticon2-map', '/app/main/operations/projects'),
                new AppMenuItem('Languages', 'Pages.Administration.Languages', 'flaticon-tabs', '/app/admin/languages'),
                new AppMenuItem('AuditLogs', 'Pages.Administration.AuditLogs', 'flaticon-folder-1', '/app/admin/auditLogs'),
                // new AppMenuItem('Maintenance', 'Pages.Administration.Host.Maintenance', 'flaticon-lock', '/app/admin/maintenance'),
                // new AppMenuItem('Subscription', 'Pages.Administration.Tenant.SubscriptionManagement', 'flaticon-refresh', '/app/admin/subscription-management'),
                // new AppMenuItem('VisualSettings', 'Pages.Administration.UiCustomization', 'flaticon-medical', '/app/admin/ui-customization'),
                // new AppMenuItem('Settings', 'Pages.Administration.Host.Settings', 'flaticon-settings', '/app/admin/hostSettings'),
                // new AppMenuItem('Settings', 'Pages.Administration.Tenant.Settings', 'flaticon-settings', '/app/admin/tenantSettings'),
                // new AppMenuItem('WebhookSubscriptions', 'Pages.Administration.WebhookSubscription', 'flaticon2-world', '/app/admin/webhook-subscriptions')
            ]),
            // new AppMenuItem('Reports', '', 'flaticon-graph', '', [
            //     new AppMenuItem('GeneralReports', 'Pages.Reports', 'flaticon-diagram', '/app/main/operations/reports'),
            //     new AppMenuItem('EmployeeReport', 'Pages.Reports', 'flaticon-line-graph', '/app/main/operations/employeeReport'),
            //     new AppMenuItem('TimeProfileReport', 'Pages.TimeProfileReport', 'flaticon-line-graph', '/app/main/operations/timeProfileReport'),

            // ]),

            new AppMenuItem('Reports', '', 'flaticon-graph', '', [

                new AppMenuItem('HrReport', 'Pages.HrReport', 'flaticon-graph', '/app/main/operations/hrReportComponent'),
                new AppMenuItem('ManagerReport', 'Pages.ManagerReport', 'flaticon-graph', '/app/main/operations/managerReportComponent'),
                new AppMenuItem('SummerizeReport', 'Pages.SummerizeReport', 'flaticon-graph', '/app/main/operations/summerizeReport'),
                new AppMenuItem('ManagerLevelApprove', 'Pages.ManagerLevelApprove', 'flaticon-profile', '/app/main/operations/managerLevelApprove'),
                new AppMenuItem('NormalOvertime', 'Pages.NormalOvertime', 'flaticon-time', '/app/main/operations/normalOvertime'),
                new AppMenuItem('FixedOvertime', 'Pages.FixedOvertime', 'flaticon-time', '/app/main/operations/fixedOvertime'),
                new AppMenuItem('RegularHours', 'Pages.FixedOvertime', 'flaticon-time', '/app/main/operations/regularHours'),
                new AppMenuItem('FridayOvertime', 'Pages.FixedOvertime', 'flaticon-time', '/app/main/operations/fridayOvertime'),
                new AppMenuItem('ProjectSheet', 'Pages.FixedOvertime', 'flaticon-time', '/app/main/operations/projectSheet')
            ]),
            // new AppMenuItem('DemoUiComponents', 'Pages.DemoUiComponents', 'flaticon-shapes', '/app/admin/demo-ui-components'),

        ]);
    }

    checkChildMenuItemPermission(menuItem): boolean {

        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName === '' || subMenuItem.permissionName === null || subMenuItem.permissionName && this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                return true;
            } else if (subMenuItem.items && subMenuItem.items.length) {
                return this.checkChildMenuItemPermission(subMenuItem);
            }
        }

        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        if (menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' && this._appSessionService.tenant && !this._appSessionService.tenant.edition) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }

    /**
     * Returns all menu items recursively
     */
    getAllMenuItems(): AppMenuItem[] {
        let menu = this.getMenu();
        let allMenuItems: AppMenuItem[] = [];
        menu.items.forEach(menuItem => {
            allMenuItems = allMenuItems.concat(this.getAllMenuItemsRecursive(menuItem));
        });

        return allMenuItems;
    }

    private getAllMenuItemsRecursive(menuItem: AppMenuItem): AppMenuItem[] {
        if (!menuItem.items) {
            return [menuItem];
        }

        let menuItems = [menuItem];
        menuItem.items.forEach(subMenu => {
            menuItems = menuItems.concat(this.getAllMenuItemsRecursive(subMenu));
        });

        return menuItems;
    }
}
