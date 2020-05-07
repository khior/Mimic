using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Mimic.Tests
{
    public class InterfaceTests
    {
        private Mock<IMimicAdapter> Mock { get; }

        public InterfaceTests()
        {
            Mock = new Mock<IMimicAdapter>();
        }

        private T Mimic<T>() => new MimicBuilder().Mimic<T>(Mock.Object);

        public interface IHaveReadOnlyProperty { string Foo { get; } }
        [Fact]
        public void ShouldAdaptReadOnlyPropertiesCorrectly()
        {
            var prop = typeof(IHaveReadOnlyProperty).GetProperty(nameof(IHaveReadOnlyProperty.Foo));

            Mock
                .Setup(m => m.GetProperty(prop))
                .Returns(() => "expected");

            Mimic<IHaveReadOnlyProperty>().Foo.Should().Be("expected");
        }

        public interface IHaveWriteOnlyProperty { string Foo { set; } }
        [Fact]
        public void ShouldAdaptWriteOnlyPropertiesCorrectly()
        {
            var prop = typeof(IHaveWriteOnlyProperty).GetProperty(nameof(IHaveWriteOnlyProperty.Foo));

            Mock
                .Setup(m => m.SetProperty(prop, "expected"))
                .Verifiable();

            Mimic<IHaveWriteOnlyProperty>().Foo = "expected";

            Mock.Verify();
        }

        public interface IHaveReadWriteProperty { string Foo { get; set; } }
        [Fact]
        public void ShouldAdaptReadWritePropertiesCorrectly()
        {
            var prop = typeof(IHaveReadWriteProperty).GetProperty(nameof(IHaveReadWriteProperty.Foo));

            Mock
                .Setup(m => m.GetProperty(prop))
                .Returns(() => "expected 1");

            Mock
                .Setup(m => m.SetProperty(prop, "expected 2"))
                .Verifiable();

            var adapter = Mimic<IHaveReadWriteProperty>();

            adapter.Foo.Should().Be("expected 1");

            adapter.Foo = "expected 2";
            Mock.Verify();
        }

        public interface IHaveMethodWithoutReturnTypeOrParameters { void Foo(); }
        [Fact]
        public void ShouldAdaptMethodWithoutReturnTypeOrParametersCorrectly()
        {
            var method = typeof(IHaveMethodWithoutReturnTypeOrParameters)
                .GetMethod(nameof(IHaveMethodWithoutReturnTypeOrParameters.Foo));

            Mock
                .Setup(m => m.Method(method, new object[] { }))
                .Verifiable();

            Mimic<IHaveMethodWithoutReturnTypeOrParameters>().Foo();

            Mock.Verify();
        }

        public interface IHaveMethodWithoutReturnTypeButWithParameters { void Foo(string foo, int bar); }
        [Fact]
        public void ShouldAdaptMethodWithoutReturnTypeButWithParametersCorrectly()
        {
            var method = typeof(IHaveMethodWithoutReturnTypeButWithParameters)
                .GetMethod(nameof(IHaveMethodWithoutReturnTypeButWithParameters.Foo));

            Mock
                .Setup(m => m.Method(method, new object[] { "example", 123 }))
                .Verifiable();

            Mimic<IHaveMethodWithoutReturnTypeButWithParameters>().Foo("example", 123);

            Mock.Verify();
        }

        public interface IHaveMethodWithReturnTypeAndParameters { string Foo(string foo, int bar); }
        [Fact]
        public void ShouldAdaptMethodWithReturnTypeAndParametersCorrectly()
        {
            var method = typeof(IHaveMethodWithReturnTypeAndParameters)
                .GetMethod(nameof(IHaveMethodWithReturnTypeAndParameters.Foo));

            Mock
                .Setup(m => m.Method(method, new object[] { "example", 123 }))
                .Returns("expected");

            var adapter = Mimic<IHaveMethodWithReturnTypeAndParameters>();

            adapter.Foo("example", 123).Should().Be("expected");
        }

        public interface IHaveMethodWithGenericTypeReturnTypeAndParameters { T Foo<T>(T foo); }
        [Fact]
        public void ShouldAdaptMethodWithGenericTypeReturnTypeAndParametersCorrectly()
        {
            var method = typeof(IHaveMethodWithGenericTypeReturnTypeAndParameters)
                .GetMethod(nameof(IHaveMethodWithGenericTypeReturnTypeAndParameters.Foo));

            Mock
                .Setup(m => m.Method(method, new object[] { "example" }))
                .Returns("expected");

            var adapter = Mimic<IHaveMethodWithGenericTypeReturnTypeAndParameters>();

            adapter.Foo("example").Should().Be("expected");
        }

        public interface IHaveGenericTypeAndMethodWithReturnTypeAndParameters<T>
        {
            T Foo(T foo, string bar);
            string Bar { get; set; }
        }
        [Fact]
        public void ShouldAdaptGenericTypeAndMethodWithReturnTypeAndParametersCorrectly()
        {
            var method = typeof(IHaveGenericTypeAndMethodWithReturnTypeAndParameters<int>)
                .GetMethod(nameof(IHaveGenericTypeAndMethodWithReturnTypeAndParameters<int>.Foo));

            Mock
                .Setup(m => m.Method(method, new object[] { 123, "example" }))
                .Returns(456);

            var adapter = Mimic<IHaveGenericTypeAndMethodWithReturnTypeAndParameters<int>>();

            adapter.Foo(123, "example").Should().Be(456);
        }

        public interface IHaveGenericTypeAndGenericMethodWithReturnTypeAndParameters<in T, TProp> where T : struct
        {
            TProp Bar { get; }
            TResult Foo<TResult>(T foo) where TResult : ICollection<int>;
        }
        public struct ShouldAdaptGenericTypeAndGenericMethodWithReturnTypeAndParametersCorrectlyStruct
        {
            public string Foo;
        }
        [Fact]
        public void ShouldAdaptGenericTypeAndGenericMethodWithReturnTypeAndParametersCorrectly()
        {
            var method =
                typeof(IHaveGenericTypeAndGenericMethodWithReturnTypeAndParameters<
                        ShouldAdaptGenericTypeAndGenericMethodWithReturnTypeAndParametersCorrectlyStruct,string>)
                    .GetMethod(
                        nameof(IHaveGenericTypeAndGenericMethodWithReturnTypeAndParameters<
                            ShouldAdaptGenericTypeAndGenericMethodWithReturnTypeAndParametersCorrectlyStruct, string>.Foo));

            var x = new ShouldAdaptGenericTypeAndGenericMethodWithReturnTypeAndParametersCorrectlyStruct
            { Foo = "example;" };
            var result = new List<int> { 1, 2, 3 };
            Mock
                .Setup(m => m.Method(method, new object[] { x }))
                .Returns(result);

            var adapter =
                Mimic<IHaveGenericTypeAndGenericMethodWithReturnTypeAndParameters<
                    ShouldAdaptGenericTypeAndGenericMethodWithReturnTypeAndParametersCorrectlyStruct, string>>();

            adapter.Foo<List<int>>(x).Should().BeEquivalentTo(result);
        }
    }
}
