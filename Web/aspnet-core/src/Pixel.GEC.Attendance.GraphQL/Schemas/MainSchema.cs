using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using Pixel.GEC.Attendance.Queries.Container;

namespace Pixel.GEC.Attendance.Schemas
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