namespace BoilerplateFree.Test
{
    using BoilerplateFree;
    using Subpackage;
    using Xunit;

    public class AutoGenerateConstructorTestsWithEmpty
    {
        [Fact]
        public void AutoGenerateConstructor_ShouldBeAbleToGenerateConstructorForUnderscoredPrivateFields()
        {
            // We had an issue where accidentally adding [AutoGenerateConstructor] to a class without fields meant that it did not build properly
            // because it tried to generate a constructor for an empty field-set.
            var class1 = new ClassWithoutFields();
            
            Assert.Equal("3", class1.MyMethod());
        }
    }


    [AutoGenerateConstructor]
    public partial class ClassWithoutFields
    {
        public string MyMethod() => "3";
    }
}
