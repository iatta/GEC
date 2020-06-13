using System.ComponentModel;
using Abp.AutoMapper;
using Pixel.GEC.Attendance.MultiTenancy.Dto;

namespace Pixel.GEC.Attendance.Models.Tenants
{
    [AutoMapFrom(typeof(TenantEditDto))]
    public class TenantEditModel : TenantEditDto, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}