using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace UniversalAdapter.Tests
{
    public class ClassTests
    {
        private Mock<IInterfaceAdapter> Mock { get; }

        public ClassTests()
        {
            Mock = new Mock<IInterfaceAdapter>();
        }

        public class ClassWithAMethod
        {
            public void Foo()
            {
            }
        }

        [Fact]
        public void ShouldNotImplementClasses()
        {
            new UniversalAdapterFactory()
                .Invoking(x => x.Create(typeof(ClassWithAMethod), Mock.Object))
                .Should().Throw<ArgumentException>();
        }
    }
}
