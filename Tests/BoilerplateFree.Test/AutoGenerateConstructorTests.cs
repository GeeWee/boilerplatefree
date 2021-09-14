namespace BoilerplateFree.Test
{
    using BoilerplateFree;
    using Xunit;

    public class AutoGenerateConstructorTests
    {
        [Fact]
        public void AutogGenerateConstructor_ShouldBeAbleToGenerateConstructorForUnderscoredPrivateFields()
        {
            var class1 = new Class1ToGenerate(firstVariable: 1, secondVariable: 2, thirdVariable: 3);

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
