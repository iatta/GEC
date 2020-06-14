using System.Threading.Tasks;
using Abp.Dependency;

namespace GEC.Attendance.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}