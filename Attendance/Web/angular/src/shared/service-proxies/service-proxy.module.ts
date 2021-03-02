﻿import { AbpHttpInterceptor, RefreshTokenService } from '@abp/abpHttpInterceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import * as ApiServiceProxies from './service-proxies';
import { ZeroRefreshTokenService } from '@account/auth/zero-refresh-token.service';

@NgModule({
    providers: [
        ApiServiceProxies.TaskTypesServiceProxy,        
        ApiServiceProxies.RamadanDatesServiceProxy,
        ApiServiceProxies.TransactionLogsServiceProxy,
        ApiServiceProxies.OverrideShiftsServiceProxy,
        ApiServiceProxies.EmployeeTempTransfersServiceProxy,
        ApiServiceProxies.OrganizationLocationsServiceProxy,
        ApiServiceProxies.ProjectLocationsServiceProxy,
        ApiServiceProxies.LocationMachinesServiceProxy,
        ApiServiceProxies.UserDelegationsServiceProxy,
        ApiServiceProxies.UserTimeSheetApprovesServiceProxy,
        ApiServiceProxies.BeaconsServiceProxy,
        ApiServiceProxies.UserShiftsServiceProxy,
        ApiServiceProxies.ProjectsServiceProxy,
        ApiServiceProxies.MobileWebPagesServiceProxy,
        ApiServiceProxies.EmployeeWarningsServiceProxy,
        ApiServiceProxies.ManualTransactionsServiceProxy,
        ApiServiceProxies.TransServiceProxy,
        ApiServiceProxies.EmployeeAbsencesServiceProxy,
        ApiServiceProxies.LocationCredentialsServiceProxy,
        ApiServiceProxies.LocationsServiceProxy,
        ApiServiceProxies.TransactionsServiceProxy,
        ApiServiceProxies.MachinesServiceProxy,
        ApiServiceProxies.EmployeeOfficialTasksServiceProxy,
        ApiServiceProxies.OfficialTaskTypesServiceProxy,
        ApiServiceProxies.TimeProfileDetailsServiceProxy,
        ApiServiceProxies.TimeProfilesServiceProxy,
        ApiServiceProxies.ShiftTypeDetailsServiceProxy,
        ApiServiceProxies.ShiftTypesServiceProxy,
        ApiServiceProxies.ShiftsServiceProxy,
        ApiServiceProxies.WarningTypesServiceProxy,
        ApiServiceProxies.EmployeeVacationsServiceProxy,
        ApiServiceProxies.EmployeePermitsServiceProxy,
        ApiServiceProxies.SystemConfigurationsServiceProxy,
        ApiServiceProxies.PermitsServiceProxy,
        ApiServiceProxies.TypesOfPermitsServiceProxy,
        ApiServiceProxies.HolidaysServiceProxy,
        ApiServiceProxies.LeaveTypesServiceProxy,
        ApiServiceProxies.JobTitleServiceProxy,
        ApiServiceProxies.AuditLogServiceProxy,
        ApiServiceProxies.CachingServiceProxy,
        ApiServiceProxies.ChatServiceProxy,
        ApiServiceProxies.CommonLookupServiceProxy,
        ApiServiceProxies.EditionServiceProxy,
        ApiServiceProxies.FriendshipServiceProxy,
        ApiServiceProxies.HostSettingsServiceProxy,
        ApiServiceProxies.InstallServiceProxy,
        ApiServiceProxies.LanguageServiceProxy,
        ApiServiceProxies.NotificationServiceProxy,
        ApiServiceProxies.OrganizationUnitServiceProxy,
        ApiServiceProxies.PermissionServiceProxy,
        ApiServiceProxies.ProfileServiceProxy,
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.TenantServiceProxy,
        ApiServiceProxies.TenantDashboardServiceProxy,
        ApiServiceProxies.TenantSettingsServiceProxy,
        ApiServiceProxies.TimingServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.UserLinkServiceProxy,
        ApiServiceProxies.UserLoginServiceProxy,
        ApiServiceProxies.WebLogServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.TenantRegistrationServiceProxy,
        ApiServiceProxies.HostDashboardServiceProxy,
        ApiServiceProxies.PaymentServiceProxy,
        ApiServiceProxies.DemoUiComponentsServiceProxy,
        ApiServiceProxies.InvoiceServiceProxy,
        ApiServiceProxies.SubscriptionServiceProxy,
        ApiServiceProxies.InstallServiceProxy,
        ApiServiceProxies.UiCustomizationSettingsServiceProxy,
        ApiServiceProxies.PayPalPaymentServiceProxy,
        ApiServiceProxies.StripePaymentServiceProxy,
        ApiServiceProxies.DashboardCustomizationServiceProxy,
        ApiServiceProxies.WebhookEventServiceProxy,
        ApiServiceProxies.WebhookSubscriptionServiceProxy,
        ApiServiceProxies.WebhookSendAttemptServiceProxy,
        ApiServiceProxies.MachineUsersServiceProxy,
        { provide: RefreshTokenService, useClass: ZeroRefreshTokenService },
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }
    ]
})
export class ServiceProxyModule { }
