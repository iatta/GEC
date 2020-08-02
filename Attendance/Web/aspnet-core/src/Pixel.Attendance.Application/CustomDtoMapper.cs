using Pixel.Attendance.Settings.Dtos;
using Pixel.Attendance.Settings;
using Pixel.Attendance.Attendance.Dtos;
using Pixel.Attendance.Attendance;
using Pixel.Attendance.Operation.Dtos;
using Pixel.Attendance.Operation;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Operations;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Setting;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using Pixel.Attendance.Auditing.Dto;
using Pixel.Attendance.Authorization.Accounts.Dto;
using Pixel.Attendance.Authorization.Permissions.Dto;
using Pixel.Attendance.Authorization.Roles;
using Pixel.Attendance.Authorization.Roles.Dto;
using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Authorization.Users.Dto;
using Pixel.Attendance.Authorization.Users.Importing.Dto;
using Pixel.Attendance.Authorization.Users.Profile.Dto;
using Pixel.Attendance.Chat;
using Pixel.Attendance.Chat.Dto;
using Pixel.Attendance.Editions;
using Pixel.Attendance.Editions.Dto;
using Pixel.Attendance.Friendships;
using Pixel.Attendance.Friendships.Cache;
using Pixel.Attendance.Friendships.Dto;
using Pixel.Attendance.Localization.Dto;
using Pixel.Attendance.MultiTenancy;
using Pixel.Attendance.MultiTenancy.Dto;
using Pixel.Attendance.MultiTenancy.HostDashboard.Dto;
using Pixel.Attendance.MultiTenancy.Payments;
using Pixel.Attendance.MultiTenancy.Payments.Dto;
using Pixel.Attendance.Notifications.Dto;
using Pixel.Attendance.Organizations.Dto;
using Pixel.Attendance.Sessions.Dto;
using Pixel.Attendance.WebHooks.Dto;
using Pixel.Attendance.Dto;
using Pixel.Attendance.ReportsModel;
using Pixel.Attendance.Extended;
using Pixel.Attendance.Enums;

namespace Pixel.Attendance
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditUserDelegationDto, UserDelegation>().ReverseMap();
            configuration.CreateMap<UserDelegationDto, UserDelegation>().ReverseMap();
            configuration.CreateMap<CreateOrEditUserTimeSheetApproveDto, UserTimeSheetApprove>().ReverseMap();
            configuration.CreateMap<UserTimeSheetApproveDto, UserTimeSheetApprove>().ReverseMap();
            configuration.CreateMap<CreateOrEditBeaconDto, Beacon>().ReverseMap();
            configuration.CreateMap<BeaconDto, Beacon>().ReverseMap();
            configuration.CreateMap<CreateOrEditUserShiftDto, UserShift>().ReverseMap();
            configuration.CreateMap<UserShiftDto, UserShift>().ReverseMap();
            
             configuration.CreateMap<ShiftTypeEnumDto, ShiftTypeEnum>().ReverseMap();
            configuration.CreateMap<CreateOrEditProjectDto, Project>().ReverseMap();
            configuration.CreateMap<ProjectDto, Project>().ReverseMap();
            configuration.CreateMap<CreateOrEditMobileWebPageDto, MobileWebPage>().ReverseMap();
            configuration.CreateMap<MobileWebPageDto, MobileWebPage>().ReverseMap();
            configuration.CreateMap<CreateOrEditNationalityDto, Nationality>().ReverseMap();
            configuration.CreateMap<NationalityDto, Nationality>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeeWarningDto, EmployeeWarning>().ReverseMap();
            configuration.CreateMap<EmployeeWarningDto, EmployeeWarning>().ReverseMap();
            
            configuration.CreateMap<EmployeeReportOutput, EmployeeReportCore>().ReverseMap();
            configuration.CreateMap<PermitReportOutput, PermitReportCore>().ReverseMap();
            configuration.CreateMap<InOutReportOutput, ForgetInOutCore>().ReverseMap();
            configuration.CreateMap<FingerReportOutput, FingerReportCore>().ReverseMap();
            configuration.CreateMap<InOutReportOutput, InOutReportOutputCore>().ReverseMap();
            configuration.CreateMap<CreateOrEditTempTransactionDto, TempTransaction>().ReverseMap();
            configuration.CreateMap<TempTransactionDto, TempTransaction>().ReverseMap();
            configuration.CreateMap<CreateOrEditManualTransactionDto, ManualTransaction>().ReverseMap();
            configuration.CreateMap<ManualTransactionDto, ManualTransaction>().ReverseMap();
            configuration.CreateMap<CreateOrEditTranDto, Tran>().ReverseMap();
            configuration.CreateMap<TranDto, Tran>().ReverseMap();
            configuration.CreateMap<CreateOrEditUserDeviceDto, UserDevice>().ReverseMap();
            configuration.CreateMap<UserDeviceDto, UserDevice>().ReverseMap();
            configuration.CreateMap<CreateOrEditMobileTransactionDto, MobileTransaction>().ReverseMap();
            configuration.CreateMap<MobileTransactionDto, MobileTransaction>().ReverseMap();
            configuration.CreateMap<GetUserForFaceIdOutput, User>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeeAbsenceDto, EmployeeAbsence>().ReverseMap();
            configuration.CreateMap<EmployeeAbsenceDto, EmployeeAbsence>().ReverseMap();
            configuration.CreateMap<CreateOrEditLocationCredentialDto, LocationCredential>().ReverseMap();
            configuration.CreateMap<LocationCredentialDto, LocationCredential>().ReverseMap();
            configuration.CreateMap<CreateOrEditLocationDto, Location>().ReverseMap();
            configuration.CreateMap<LocationDto, Location>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeeOfficialTaskDetailDto, EmployeeOfficialTaskDetail>().ReverseMap();
            configuration.CreateMap<EmployeeOfficialTaskDetailDto, EmployeeOfficialTaskDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditTransactionDto, Transaction>().ReverseMap();
            configuration.CreateMap<TransactionDto, Transaction>().ReverseMap();
            configuration.CreateMap<CreateOrEditMachineDto, Machine>().ReverseMap();
            configuration.CreateMap<MachineDto, Machine>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeeOfficialTaskDto, EmployeeOfficialTask>().ReverseMap();
            configuration.CreateMap<EmployeeOfficialTaskDto, EmployeeOfficialTask>().ReverseMap();
            configuration.CreateMap<CreateOrEditOfficialTaskTypeDto, OfficialTaskType>().ReverseMap();
            configuration.CreateMap<OfficialTaskTypeDto, OfficialTaskType>().ReverseMap();
            configuration.CreateMap<CreateOrEditTimeProfileDetailDto, TimeProfileDetail>().ReverseMap();
            configuration.CreateMap<TimeProfileDetailDto, TimeProfileDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditTimeProfileDto, TimeProfile>().ReverseMap();
            configuration.CreateMap<TimeProfileDto, TimeProfile>().ReverseMap();
            configuration.CreateMap<CreateOrEditShiftTypeDetailDto, ShiftTypeDetail>().ReverseMap();
            configuration.CreateMap<ShiftTypeDetailDto, ShiftTypeDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditShiftTypeDto, ShiftType>().ReverseMap();
            configuration.CreateMap<ShiftTypeDto, ShiftType>().ReverseMap();
            configuration.CreateMap<CreateOrEditShiftDto, Shift>().ReverseMap();
            configuration.CreateMap<ShiftDto, Shift>().ReverseMap();
            configuration.CreateMap<CreateOrEditWarningTypeDto, WarningType>().ReverseMap();
            configuration.CreateMap<WarningTypeDto, WarningType>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeeVacationDto, EmployeeVacation>().ReverseMap();
            configuration.CreateMap<EmployeeVacationDto, EmployeeVacation>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeePermitDto, EmployeePermit>().ReverseMap();
            configuration.CreateMap<EmployeePermitDto, EmployeePermit>().ReverseMap();
            configuration.CreateMap<CreateOrEditSystemConfigurationDto, SystemConfiguration>().ReverseMap();
            configuration.CreateMap<SystemConfigurationDto, SystemConfiguration>().ReverseMap();
            
            configuration.CreateMap<PermitTypeDto, PermitType>().ReverseMap();
            configuration.CreateMap<CreateOrEditPermitDto, Permit>().ReverseMap();
            configuration.CreateMap<PermitDto, Permit>().ReverseMap();
            configuration.CreateMap<CreateOrEditTypesOfPermitDto, TypesOfPermit>().ReverseMap();
            configuration.CreateMap<TypesOfPermitDto, TypesOfPermit>().ReverseMap();
            configuration.CreateMap<CreateOrEditHolidayDto, Holiday>().ReverseMap();
            configuration.CreateMap<HolidayDto, Holiday>().ReverseMap();
            configuration.CreateMap<CreateOrEditLeaveTypeDto, LeaveType>().ReverseMap();
            configuration.CreateMap<LeaveTypeDto, LeaveType>().ReverseMap();
            configuration.CreateMap<CreateOrEditJobTitleDto, JobTitle>().ReverseMap();
            configuration.CreateMap<JobTitleDto, JobTitle>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnitExtended, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();


            configuration.CreateMap<MobileTransaction, ReportEntityModel>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}
