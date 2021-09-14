namespace BoilerplateFree.Test
{
    using System;
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
