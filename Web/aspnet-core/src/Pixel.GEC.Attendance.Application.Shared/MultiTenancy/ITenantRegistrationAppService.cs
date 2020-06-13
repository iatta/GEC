using System.Threading.Tasks;
using Abp.Application.Services;
using Pixel.GEC.Attendance.Editions.Dto;
using Pixel.GEC.Attendance.MultiTenancy.Dto;

namespace Pixel.GEC.Attendance.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}