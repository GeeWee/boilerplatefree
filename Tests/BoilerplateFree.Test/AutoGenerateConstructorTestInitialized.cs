using Xunit;

namespace BoilerplateFree.Test
{
    public class AutoGenerateConstructorTestConsts
    {
        [Fact]
        public void AutoGenerateConstructor_ShouldOnlyGenerateConstructorFields_ForFieldsWithoutInitializers()
        {
            var myClass = new ClassWithInitializedFields("myStringIWanted");
            Assert.Equal(123, myClass.myInt);
            Assert.Equal("MyConst", ClassWithInitializedFields.MyConst);
            Assert.Equal("myStringIWanted", myClass.myStringIWantFromConstructor);
        }
    }


    
    [AutoGenerateConstructor]
    public partial class ClassWithInitializedFields
    {
        public const string MyConst = "MyConst";
        public int myInt = 123;

        public readonly string myStringIWantFromConstructor;

    }
}