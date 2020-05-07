using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Mimic.Tests
{
    public class ClassTests
    {
        private Mock<IMimicAdapter> Mock { get; }

        public ClassTests()
        {
            Mock = new Mock<IMimicAdapter>();
        }

        public class ClassWithAMethod
        {
            public void Foo()
            {
            }
        }

        [Fact]
        public void ShouldNotMimicClasses()
        {
            new MimicBuilder()
                .Invoking(x => x.Mimic<ClassWithAMethod>(Mock.Object))
                .Should().Throw<InvalidOperationException>();
        }
    }
}
