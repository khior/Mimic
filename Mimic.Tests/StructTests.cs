using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Mimic.Tests
{
    public class StructTests
    {
        private Mock<IMimicAdapter> Mock { get; }

        public StructTests()
        {
            Mock = new Mock<IMimicAdapter>();
        }

        public struct StructWithAMethod
        {
            public void Foo()
            {
            }
        }

        [Fact]
        public void ShouldNotMimicStructs()
        {
            new MimicBuilder()
                .Invoking(x => x.Mimic<StructWithAMethod>(Mock.Object))
                .Should().Throw<InvalidOperationException>();
        }
    }
}
