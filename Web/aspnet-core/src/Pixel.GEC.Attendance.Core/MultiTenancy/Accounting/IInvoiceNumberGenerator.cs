using System.Threading.Tasks;
using Abp.Dependency;

namespace Pixel.GEC.Attendance.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}