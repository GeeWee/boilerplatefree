using System;

namespace BoilerplateFree.Test
{
    [AutoGenerateInterface]
        public class GenerateAutoInterfaceClass : IGenerateAutoInterfaceClass
        {
            public void Foo()
            {
                Console.WriteLine("Foo");
            }

            public int Bar(int param1) => 1 + param1;
            
            private int SuperSecretMethod(int param1) => 1 + param1;
            private int SuperSecretProperty { get; set; }
            
            public int MyPublicIntWithDefaultImplementation => 3;
            public int MyNextPublicInt { get; set; }
        }
}