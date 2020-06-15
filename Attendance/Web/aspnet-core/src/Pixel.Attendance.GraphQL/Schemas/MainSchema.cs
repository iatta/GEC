using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using Pixel.Attendance.Queries.Container;

namespace Pixel.Attendance.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}