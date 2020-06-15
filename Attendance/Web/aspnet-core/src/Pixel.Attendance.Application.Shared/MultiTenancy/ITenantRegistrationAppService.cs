using System.Threading.Tasks;
using Abp.Application.Services;
using Pixel.Attendance.Editions.Dto;
using Pixel.Attendance.MultiTenancy.Dto;

namespace Pixel.Attendance.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}