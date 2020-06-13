using Abp.Application.Services.Dto;
using GraphQL.Types;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Types
{
    public class UserPagedResultGraphType : ObjectGraphType<PagedResultDto<UserDto>>
    {
        public UserPagedResultGraphType()
        {
            Field(x => x.TotalCount);
            Field(x => x.Items, type: typeof(ListGraphType<UserType>));
        }
    }
}