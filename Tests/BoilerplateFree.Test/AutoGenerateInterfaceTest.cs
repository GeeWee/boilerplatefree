// namespace BoilerplateFree.Test
// {
//     using Subpackage;
//     using Xunit;
//
//     public partial class AutoGenerateInterfaceTest
//     {
//         [Fact]
//         public void AutoGenerateInterface_ShouldGenerateInterfaceWithAllPublicMethods()
//         {
//             // Arrange
//             IGenerateAutoInterfaceClass testClass = new GenerateAutoInterfaceClass();
//             // Act
//             testClass.Foo();
//             testClass.Bar(param1: 3);
//         }
//
//         [Fact]
//         public void AutoGenerateInterface_ShouldGenerateInterfaceWithSubclasses_WhenUsingsAreInsideNamespace()
//         {
//             IClassWithUsingsToGenerate testClass = new ClassWithUsingsToGenerate(new Subclass());
//             testClass.SetSubClass(new Subclass());
//         }
//
//         [Fact]
//         public void AutoGenerateInterface_ShouldGenerateInterfaceWithSubclasses_WhenUsingsAreOutsideNamespace()
//         {
//             IClassWithUsingsToGenerateOutsideNamespace testClass = new ClassWithUsingsToGenerateOutsideNamespace(new Subclass());
//             testClass.SetSubClass(new Subclass());
//         }
//     }
// }
