// Note that using is outisde the namespace here
using BoilerplateFree.Test.Subpackage;

namespace BoilerplateFree.Test
{
    [AutoGenerateConstructor]
    [AutoGenerateInterface]
    public partial class ClassWithUsingsToGenerateOutsideNamespace : IClassWithUsingsToGenerateOutsideNamespace
    {
        private Subclass subclass;

        public void SetSubClass(Subclass subClass) => this.subclass = subClass;
    }
}
