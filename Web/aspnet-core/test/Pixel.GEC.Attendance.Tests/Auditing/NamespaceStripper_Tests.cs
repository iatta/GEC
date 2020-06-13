﻿using Pixel.GEC.Attendance.Auditing;
using Pixel.GEC.Attendance.Test.Base;
using Shouldly;
using Xunit;

namespace Pixel.GEC.Attendance.Tests.Auditing
{
    // ReSharper disable once InconsistentNaming
    public class NamespaceStripper_Tests: AppTestBase
    {
        private readonly INamespaceStripper _namespaceStripper;

        public NamespaceStripper_Tests()
        {
            _namespaceStripper = Resolve<INamespaceStripper>();
        }

        [Fact]
        public void Should_Stripe_Namespace()
        {
            var controllerName = _namespaceStripper.StripNameSpace("Pixel.GEC.Attendance.Web.Controllers.HomeController");
            controllerName.ShouldBe("HomeController");
        }

        [Theory]
        [InlineData("Pixel.GEC.Attendance.Auditing.GenericEntityService`1[[Pixel.GEC.Attendance.Storage.BinaryObject, Pixel.GEC.Attendance.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null]]", "GenericEntityService<BinaryObject>")]
        [InlineData("CompanyName.ProductName.Services.Base.EntityService`6[[CompanyName.ProductName.Entity.Book, CompanyName.ProductName.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[CompanyName.ProductName.Services.Dto.Book.CreateInput, N...", "EntityService<Book, CreateInput>")]
        [InlineData("Pixel.GEC.Attendance.Auditing.XEntityService`1[Pixel.GEC.Attendance.Auditing.AService`5[[Pixel.GEC.Attendance.Storage.BinaryObject, Pixel.GEC.Attendance.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[Pixel.GEC.Attendance.Storage.TestObject, Pixel.GEC.Attendance.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],]]", "XEntityService<AService<BinaryObject, TestObject>>")]
        public void Should_Stripe_Generic_Namespace(string serviceName, string result)
        {
            var genericServiceName = _namespaceStripper.StripNameSpace(serviceName);
            genericServiceName.ShouldBe(result);
        }
    }
}
