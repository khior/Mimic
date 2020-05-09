using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;

namespace UniversalAdapter.Tests
{
    public class InterfaceTests
    {
        private Mock<IInterfaceHandler> Mock { get; }

        public InterfaceTests()
        {
            Mock = new Mock<IInterfaceHandler>();
        }

        private T Create<T>() => (T) new UniversalAdapterFactory().Create(typeof(T), Mock.Object);

        public interface IHaveReadOnlyProperty { string Foo { get; } }
        [Fact]
        public void ShouldAdaptReadOnlyPropertiesCorrectly()
        {
            var prop = typeof(IHaveReadOnlyProperty).GetProperty(nameof(IHaveReadOnlyProperty.Foo));

            Mock
                .Setup(m => m.GetProperty(prop))
                .Returns(() => "expected");

            Create<IHaveReadOnlyProperty>().Foo.Should().Be("expected");
        }

        public interface IHaveWriteOnlyProperty { string Foo { set; } }
        [Fact]
        public void ShouldAdaptWriteOnlyPropertiesCorrectly()
        {
            var prop = typeof(IHaveWriteOnlyProperty).GetProperty(nameof(IHaveWriteOnlyProperty.Foo));

            Mock
                .Setup(m => m.SetProperty(prop, "expected"))
                .Verifiable();

            Create<IHaveWriteOnlyProperty>().Foo = "expected";

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

            var adapter = Create<IHaveReadWriteProperty>();

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

            Create<IHaveMethodWithoutReturnTypeOrParameters>().Foo();

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

            Create<IHaveMethodWithoutReturnTypeButWithParameters>().Foo("example", 123);

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

            var adapter = Create<IHaveMethodWithReturnTypeAndParameters>();

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

            var adapter = Create<IHaveMethodWithGenericTypeReturnTypeAndParameters>();

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

            var adapter = Create<IHaveGenericTypeAndMethodWithReturnTypeAndParameters<int>>();

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
                Create<IHaveGenericTypeAndGenericMethodWithReturnTypeAndParameters<
                    ShouldAdaptGenericTypeAndGenericMethodWithReturnTypeAndParametersCorrectlyStruct, string>>();

            adapter.Foo<List<int>>(x).Should().BeEquivalentTo(result);
        }

        public interface IHaveGenerics<in T, out TProp>
        {
            TResult Foo<TResult>(T foo) where TResult : ICollection<int>;
            TProp Bar { get; }
        }
        [Fact]
        public void ShouldCreateDifferentImplementationsForGenericTypes()
        {
            var builder = new UniversalAdapterFactory();

            var adapter1 = builder.Create(typeof(IHaveGenerics<long, string>), Mock.Object);
            var adapter2 = builder.Create(typeof(IHaveGenerics<long, string>), Mock.Object);

            var adapter3 = builder.Create(typeof(IHaveGenerics<string, long>), Mock.Object);
            var adapter4 = builder.Create(typeof(IHaveGenerics<byte[], bool>), Mock.Object);

            adapter1.GetType().Should().Be(adapter2.GetType());

            adapter2.GetType().Should().NotBe(adapter3.GetType());
            adapter3.GetType().Should().NotBe(adapter4.GetType());
        }
    }
}
