using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace UniversalAdapter.Tests
{
    public class ClassTests
    {
        private Mock<IInterfaceHandler> Mock { get; }

        public ClassTests()
        {
            Mock = new Mock<IInterfaceHandler>();
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
