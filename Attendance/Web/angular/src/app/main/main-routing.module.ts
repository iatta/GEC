import { ManageMachineUsersComponent } from './setting/machines/manage-machine-users.component';
import { OverrideUserShiftComponent } from './operations/userShifts/override-user-shift.component';
import { ManagerReportComponent } from './operations/reports/manager-report.component';
import { RamadanDatesComponent } from './setting/ramadanDates/ramadanDates.component';
import { FixedOvertimeComponent } from './operations/reports/fixed-overtime.component';
import { TransactionLogsComponent } from './operations/transactionLogs/transactionLogs.component';
import { OverrideShiftsComponent } from './setting/overrideShifts/overrideShifts.component';
import { EmployeeTempTransfersComponent } from './operations/employeeTempTransfers/employeeTempTransfers.component';
import { OrganizationLocationsComponent } from './operations/organizationLocations/organizationLocations.component';
import { ProjectLocationsComponent } from './operations/projectLocations/projectLocations.component';
import { LocationMachinesComponent } from './setting/locationMachines/locationMachines.component';
import { NormalOvertimeComponent } from './operations/reports/normal-overtime.component';
import { ManagerLevelApproveComponent } from './operations/reports/maneger-level-approve';
import { SummerizeReportComponent } from './operations/reports/summerize.component';
import { UserDelegationsComponent } from './operations/userDelegations/userDelegations.component';
import { ReportComponent } from './operations/reports/report.component';
import { UserTimeSheetApprovesComponent } from './operations/userTimeSheetApproves/userTimeSheetApproves.component';
import { EmployeeReportComponent } from './operations/reports/employee-report.component';
import { BeaconsComponent } from './operations/beacons/beacons.component';
import { UserShiftsComponent } from './operations/userShifts/userShifts.component';
import { ProjectsComponent } from './operations/projects/projects.component';
import { MobileWebPagesComponent } from './settings/mobileWebPages/mobileWebPages.component';
import { EmployeeWarningsComponent } from './operations/employeeWarnings/employeeWarnings.component';
import { TimeProfileReportComponent } from './operations/timeProfiles/timeProfile-report.component';
import { UploadEmpVacationComponent } from './operations/employeeVacations/upload-emp-vacation.component';
import { UploadTimeProfileComponent } from './operations/timeProfiles/upload-timeProfile.component';
import { ManualTransactionsComponent } from './operations/manualTransactions/manualTransactions.component';
import { TransComponent } from './operations/trans/trans.component';
import { ManageLocationComponent } from './setting/locations/manage-location.component';
import { ManageTimeProfileComponent } from './operations/timeProfiles/manage-timeProfile.component';
import { EmployeeAbsencesComponent } from './operation/employeeAbsences/employeeAbsences.component';
import { ManageShiftComponent } from './setting/shifts/manage-shift.component';
import { LocationCredentialsComponent } from './setting/locationCredentials/locationCredentials.component';
import { LocationsComponent } from './setting/locations/locations.component';
import { EmployeeOfficialTaskDetailsComponent } from './operations/employeeOfficialTaskDetails/employeeOfficialTaskDetails.component';
import { TransactionsComponent } from './operations/transactions/transactions.component';
import { MachinesComponent } from './setting/machines/machines.component';
import { EmployeeOfficialTasksComponent } from './operations/employeeOfficialTasks/employeeOfficialTasks.component';
import { OfficialTaskTypesComponent } from './setting/officialTaskTypes/officialTaskTypes.component';
import { CreateOrEditSystemConfigurationModalComponent } from './setting/systemConfigurations/create-or-edit-systemConfiguration-modal.component';
import { NgModule } from '@angular/core';
import { TimeProfileDetailsComponent } from './operations/timeProfileDetails/timeProfileDetails.component';
import { TimeProfilesComponent } from './operations/timeProfiles/timeProfiles.component';
import { ShiftTypeDetailsComponent } from './setting/shiftTypeDetails/shiftTypeDetails.component';
import { ShiftTypesComponent } from './setting/shiftTypes/shiftTypes.component';
import { ShiftsComponent } from './setting/shifts/shifts.component';
import { WarningTypesComponent } from './setting/warningTypes/warningTypes.component';
import { EmployeeVacationsComponent } from './operations/employeeVacations/employeeVacations.component';
import { EmployeePermitsComponent } from './operations/employeePermits/employeePermits.component';
import { RouterModule } from '@angular/router';
import { SystemConfigurationsComponent } from './setting/systemConfigurations/systemConfigurations.component';
import { PermitsComponent } from './setting/permits/permits.component';
import { TypesOfPermitsComponent } from './setting/typesOfPermits/typesOfPermits.component';
import { HolidaysComponent } from './setting/holidays/holidays.component';
import { LeaveTypesComponent } from './setting/leaveTypes/leaveTypes.component';
import { JobTitlesComponent } from './setting/jobTitles/jobTitles.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { EmployeePermitsManagerComponent } from './operations/employeePermits/employeePermits-manager.component';
import { ManageOfficialTaskComponent } from './operations/employeeOfficialTasks/manage-official-task.component';
import { ManageUserShiftComponent } from './operations/userShifts/manage-user-shift.component';
import { ProjectManagerTransactionsComponent } from './operations/manualTransactions/transaction-project-manager.component.';
import { UnitManagerTransactionsComponent } from './operations/manualTransactions/transaction-unit-manager.component';
import { HrTransactionsComponent } from './operations/manualTransactions/transaction-hr.component';
import { HrReportComponent } from './operations/reports/hr-report.component';
import { RegularHoursComponent } from './operations/reports/regular-hours.component';
import { FridayOvertimeComponent } from './operations/reports/friday-overtime.component';
import { ProjectSheetComponent } from './operations/reports/project-sheet.component';
import { HrLevelApproveComponent } from './operations/reports/hr-level-approve.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: 'setting/ramadanDates', component: RamadanDatesComponent, data: { permission: 'Pages.RamadanDates' }  },
                    { path: 'operations/transactionLogs', component: TransactionLogsComponent, data: { permission: 'Pages.TransactionLogs' }  },
                    { path: 'setting/overrideShifts', component: OverrideShiftsComponent, data: { permission: 'Pages.OverrideShifts' }  },
                    { path: 'operations/employeeTempTransfers', component: EmployeeTempTransfersComponent, data: { permission: 'Pages.EmployeeTempTransfers' }  },
                    { path: 'operations/organizationLocations', component: OrganizationLocationsComponent, data: { permission: 'Pages.OrganizationLocations' }  },
                    { path: 'operations/projectLocations', component: ProjectLocationsComponent, data: { permission: 'Pages.ProjectLocations' }  },
                    { path: 'setting/locationMachines', component: LocationMachinesComponent, data: { permission: 'Pages.LocationMachines' }  },
                    { path: 'operations/userDelegations', component: UserDelegationsComponent, data: { permission: 'Pages.UserDelegations' }  },
                    { path: 'operations/userTimeSheetApproves', component: UserTimeSheetApprovesComponent, data: { permission: 'Pages.UserTimeSheetApproves' }  },
                    { path: 'operations/managerLevelApprove', component: ManagerLevelApproveComponent, data: { permission: 'Pages.ManagerLevelApprove' }  },
                    { path: 'operations/normalOvertime', component: NormalOvertimeComponent, data: { permission: 'Pages.NormalOvertime' }  },
                    { path: 'operations/regularHours', component: RegularHoursComponent, data: { permission: 'Pages.NormalOvertime' }  },
                    { path: 'operations/fridayOvertime', component: FridayOvertimeComponent, data: { permission: 'Pages.NormalOvertime' }  },
                    { path: 'operations/projectSheet', component: ProjectSheetComponent, data: { permission: 'Pages.NormalOvertime' }  },



                    { path: 'operations/fixedOvertime', component: FixedOvertimeComponent, data: { permission: 'Pages.FixedOvertime' }  },
                    { path: 'operations/managerReportComponent', component: ManagerReportComponent, data: { permission: 'Pages.ManagerReport' }  },
                    { path: 'operations/hrReportComponent', component: HrReportComponent, data: { permission: 'Pages.HrReport' }  },


                    { path: 'operations/hrLevelApprove', component: HrLevelApproveComponent, data: { permission: 'Pages.HrLevelApprove' }  },
                    { path: 'operations/summerizeReport', component: SummerizeReportComponent, data: { permission: 'Pages.SummerizeReport' }  },
                    { path: 'operations/beacons', component: BeaconsComponent, data: { permission: 'Pages.Beacons' }  },
                    { path: 'operations/manageUserShifts', component: ManageUserShiftComponent, data: { permission: 'Pages.ManageUserShifts' }  },
                    { path: 'operations/overrideUserShifts', component: OverrideUserShiftComponent, data: { permission: 'Pages.ManageUserShifts' }  },

                    // { path: 'operations/userShifts', component: UserShiftsComponent, data: { permission: 'Pages.UserShifts' }  },
                    { path: 'operations/projects', component: ProjectsComponent, data: { permission: 'Pages.Projects' }  },
                    // { path: 'settings/mobileWebPages', component: MobileWebPagesComponent, data: { permission: 'Pages.MobileWebPages' }  },
                    { path: 'operations/employeeWarnings', component: EmployeeWarningsComponent, data: { permission: 'Pages.EmployeeWarnings' }  },
                    { path: 'operations/employeeReport', component: EmployeeReportComponent, data: { permission: 'Pages.Reports' }  },
                    { path: 'operations/reports', component: ReportComponent, data: { permission: 'Pages.Reports' }  },
                    { path: 'operations/timeProfileReport', component: TimeProfileReportComponent, data: { permission: 'Pages.TimeProfileReport' }  },
                    { path: 'operations/manualTransactions', component: ManualTransactionsComponent, data: { permission: 'Pages.ManualTransactions' }  },
                    { path: 'operations/ProjectManagerTransactions', component: ProjectManagerTransactionsComponent, data: { permission: 'Pages.FingerPrint.ProjectManagerTransactions' }  },
                    { path: 'operations/UnitManagerTransactions', component: UnitManagerTransactionsComponent, data: { permission: 'Pages.FingerPrint.UnitManagerTransactions' }  },
                    { path: 'operations/HrTransactions', component: HrTransactionsComponent, data: { permission: 'Pages.FingerPrint.HrTransactions' }  },



                    { path: 'operations/trans', component: TransComponent, data: { permission: 'Pages.Trans' }  },
                    { path: 'operations/uploadEmpVacation', component: UploadEmpVacationComponent, data: { permission: 'Pages.UploadEmpVacation' }  },
                    { path: 'operations/uploadTimeProfile', component: UploadTimeProfileComponent, data: { permission: 'Pages.UploadTimeProfile' }  },
                    { path: 'operation/employeeAbsences', component: EmployeeAbsencesComponent, data: { permission: 'Pages.EmployeeAbsences' }  },
                    { path: 'setting/manageLocation', component: ManageLocationComponent, data: { permission: 'Pages.EmployeeOfficialTasks' }  },
                    { path: 'setting/manageLocation/:id', component: ManageLocationComponent, data: { permission: 'Pages.EmployeeOfficialTasks' }  },
                    { path: 'setting/locationCredentials', component: LocationCredentialsComponent, data: { permission: 'Pages.LocationCredentials' }  },
                    { path: 'setting/locations', component: LocationsComponent, data: { permission: 'Pages.Locations' }  },
                    { path: 'operations/employeeOfficialTaskDetails', component: EmployeeOfficialTaskDetailsComponent, data: { permission: 'Pages.EmployeeOfficialTaskDetails' }  },
                    { path: 'operations/transactions', component: TransactionsComponent, data: { permission: 'Pages.Transactions' }  },
                    { path: 'setting/machines', component: MachinesComponent, data: { permission: 'Pages.Machines' }  },
                    { path: 'setting/manage-machines', component: ManageMachineUsersComponent, data: { permission: 'Pages.Machines' }  },
                    { path: 'operations/manageOfficialTask', component: ManageOfficialTaskComponent, data: { permission: 'Pages.EmployeeOfficialTasks' }  },
                    { path: 'operations/manageOfficialTask/:id', component: ManageOfficialTaskComponent, data: { permission: 'Pages.EmployeeOfficialTasks' }  },
                    { path: 'operations/employeeOfficialTasks', component: EmployeeOfficialTasksComponent, data: { permission: 'Pages.EmployeeOfficialTasks' }  },
                    { path: 'setting/officialTaskTypes', component: OfficialTaskTypesComponent, data: { permission: 'Pages.OfficialTaskTypes' }  },
                    // { path: 'operations/timeProfileDetails', component: TimeProfileDetailsComponent, data: { permission: 'Pages.TimeProfileDetails' }  },
                    { path: 'operations/mangeTimeProfile', component: ManageTimeProfileComponent, data: { permission: 'Pages.TimeProfiles' }  },
                    { path: 'operations/timeProfiles', component: TimeProfilesComponent, data: { permission: 'Pages.TimeProfiles' }  },
                    // { path: 'setting/shiftTypeDetails', component: ShiftTypeDetailsComponent, data: { permission: 'Pages.ShiftTypeDetails' }  },
                    { path: 'setting/shiftTypes', component: ShiftTypesComponent, data: { permission: 'Pages.ShiftTypes' }  },
                    { path: 'setting/shifts/manage/:id', component: ManageShiftComponent, data: { permission: 'Pages.ShiftTypes' }  },
                    { path: 'setting/shifts/manage', component: ManageShiftComponent, data: { permission: 'Pages.ShiftTypes' }  },
                    { path: 'setting/shifts', component: ShiftsComponent, data: { permission: 'Pages.Shifts' }  },
                    { path: 'setting/warningTypes', component: WarningTypesComponent, data: { permission: 'Pages.WarningTypes' }  },
                    { path: 'operations/employeeVacations', component: EmployeeVacationsComponent, data: { permission: 'Pages.EmployeeVacations' }  },
                    { path: 'operations/employeePermits', component: EmployeePermitsComponent, data: { permission: 'Pages.EmployeePermits' }  },
                    { path: 'operations/employeePermitsManage', component: EmployeePermitsManagerComponent, data: { permission: 'Pages.ApprovePermit' }  },
                    { path: 'setting/systemConfigurations', component: CreateOrEditSystemConfigurationModalComponent, data: { permission: 'Pages.SystemConfigurations' }  },
                    { path: 'setting/permits', component: PermitsComponent, data: { permission: 'Pages.Permits' }  },
                    { path: 'setting/typesOfPermits', component: TypesOfPermitsComponent, data: { permission: 'Pages.TypesOfPermits' }  },
                    { path: 'setting/holidays', component: HolidaysComponent, data: { permission: 'Pages.Holidays' }  },
                    { path: 'setting/leaveTypes', component: LeaveTypesComponent, data: { permission: 'Pages.LeaveTypes' }  },
                    { path: 'setting/jobTitles', component: JobTitlesComponent, data: { permission: 'Pages.JobTitles' }  },
                    { path: 'dashboard', component: DashboardComponent, data: { permission: 'Pages.Tenant.Dashboard' } },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: '**', redirectTo: 'dashboard' }
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class MainRoutingModule { }
