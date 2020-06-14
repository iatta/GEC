using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using GEC.Attendance.MultiTenancy.Accounting.Dto;

namespace GEC.Attendance.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
