namespace BoilerplateFree.Test
{
    using BoilerplateFree;
    using Subpackage;
    using Xunit;

    public class AutoGenerateConstructorTests
    {
        [Fact]
        public void AutoGenerateConstructor_ShouldBeAbleToGenerateConstructorForUnderscoredPrivateFields()
        {
            var class1 = new Class1ToGenerate(firstVariable: 1, secondVariable: 2, thirdVariable: 3);
        }

        [Fact]
        public void
            AutoGenerateConstructor_ShouldBeAbleToGenerateConstructorForUnderscoredPrivateFields_WhenRequestingClassFromDifferentPackage_AndUsingIsInsideNamespace()
        {
            var class1 = new ClassWithUsingsToGenerate(new Subclass());
        }

        [Fact]
        public void
            AutoGenerateConstructor_ShouldBeAbleToGenerateConstructorForUnderscoredPrivateFields_WhenRequestingClassFromDifferentPackage_AndUsingIsOutsideNamespace()
        {
            var class1 = new ClassWithUsingsToGenerateOutsideNamespace(new Subclass());
        }
    }


    [AutoGenerateConstructor]
    public partial class Class1ToGenerate
    {
        private int _firstVariable;
        private int _secondVariable;
        private int _thirdVariable;
    }
}
