using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace UniversalAdapter.Tests
{
    public class StructTests
    {
        private Mock<IInterfaceHandler> Mock { get; }

        public StructTests()
        {
            Mock = new Mock<IInterfaceHandler>();
        }

        public struct StructWithAMethod
        {
            public void Foo()
            {
            }
        }

        [Fact]
        public void ShouldNotImplementStructs()
        {
            new UniversalAdapterFactory()
                .Invoking(x => x.Create(typeof(StructWithAMethod), Mock.Object))
                .Should().Throw<ArgumentException>();
        }
    }
}
