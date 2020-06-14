using System.Threading.Tasks;
using Abp.Application.Services;
using GEC.Attendance.Editions.Dto;
using GEC.Attendance.MultiTenancy.Dto;

namespace GEC.Attendance.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}