using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using GEC.Attendance.Queries.Container;

namespace GEC.Attendance.Schemas
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