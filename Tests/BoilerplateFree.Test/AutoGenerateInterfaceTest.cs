namespace BoilerplateFree.Test
{
    using System;
    using Subpackage;
    using Xunit;

    public class AutoGenerateInterfaceTest
    {
        [Fact]
        public void AutoGenerateInterface_ShouldGenerateInterfaceWithAllPublicMethods()
        {
            // Arrange
            IGenerateAutoInterfaceClass testClass = new GenerateAutoInterfaceClass();
            // Act
            testClass.Foo();
            testClass.Bar(param1: 3);
        }

        [Fact]
        public void AutoGenerateInterface_ShouldGenerateInterfaceWithSubclasses_WhenUsingsAreInsideNamespace()
        {
            IClassWithUsingsToGenerate testClass = new ClassWithUsingsToGenerate(new Subclass());
            testClass.SetSubClass(new Subclass());
        }

        [Fact]
        public void AutoGenerateInterface_ShouldGenerateInterfaceWithSubclasses_WhenUsingsAreOutsideNamespace()
        {
            IClassWithUsingsToGenerateOutsideNamespace testClass = new ClassWithUsingsToGenerateOutsideNamespace(new Subclass());
            testClass.SetSubClass(new Subclass());
        }

        [AutoGenerateInterface]
        public class GenerateAutoInterfaceClass : IGenerateAutoInterfaceClass
        {
            public void Foo()
            {
                Console.WriteLine("Foo");
            }

            public int Bar(int param1) => 1 + param1;
        }
    }
}
