namespace BoilerplateFree.Test
{
    // Note that the using is inside the namespace here.
    using Subpackage;

    [AutoGenerateConstructor]
    [AutoGenerateInterface]
    public partial class ClassWithUsingsToGenerate : IClassWithUsingsToGenerate
    {
        private Subclass subclass;

        public void SetSubClass(Subclass subClass) => this.subclass = subClass;
    }
}
